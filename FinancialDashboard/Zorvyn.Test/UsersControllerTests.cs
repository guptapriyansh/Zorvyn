using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class UsersControllerTests
{
    private UsersController GetController(IUserService service)
    {
        var controller = new UsersController(service);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Email, "admin@zorvyn.in"),
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUserList()
    {
        var mockService = new Mock<IUserService>();

        mockService.Setup(x => x.GetUsers())
            .ReturnsAsync(new List<User>());

        var controller = GetController(mockService.Object);

        var result = await controller.GetUsers();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Create_ShouldAllowAdminToCreateUser()
    {
        var mockService = new Mock<IUserService>();

        var controller = GetController(mockService.Object);

        var request = new CreateUserRequest
        {
            Email = "test@zorvyn.in",
            Password = "Password123",
            Role = "Viewer"
        };

        var result = await controller.Create(request);

        result.Should().BeOfType<OkResult>();
    }
}