using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class DashboardControllerTests
{
    private DashboardController GetController(IDashboardService service)
    {
        var controller = new DashboardController(service);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Role, "Analyst")
        }, "mock"));

        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        return controller;
    }

    [Fact]
    public async Task GetDashboard_ShouldReturnSummary()
    {
        var mockService = new Mock<IDashboardService>();

        mockService.Setup(x => x.GetDashboardSummary())
            .ReturnsAsync(new DashboardSummaryResponse
            {
                TotalIncome = 5000,
                TotalExpense = 2000
            });

        var controller = GetController(mockService.Object);

        var result = await controller.GetDashboard();

        result.Should().BeOfType<OkObjectResult>();
    }
}