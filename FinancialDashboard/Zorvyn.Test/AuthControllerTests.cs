using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_ShouldReturnToken_WhenCredentialsValid()
    {
        var mockService = new Mock<IAuthService>();
        var mockLogger = new Mock<ILogger<AuthController>>();

        var request = new LoginRequest
        {
            Email = "admin@zorvyn.in",
            Password = "Admin123Hash"
        };

        mockService.Setup(x => x.LoginAsync(It.IsAny<LoginRequest>()))
            .ReturnsAsync(new LoginResponse
            {
                Email = request.Email,
                Token = "test-jwt"
            });

        var controller = new AuthController(mockService.Object, mockLogger.Object);

        var result = await controller.Login(request);

        result.Should().BeOfType<OkObjectResult>();
    }
}