using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

public class FinancialRecordsControllerTests
{
    private FinancialRecordsController GetController(IFinancialRecordService service)
    {
        var controller = new FinancialRecordsController(service);

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
    public async Task Create_ShouldReturnOk_WhenAdminCreatesRecord()
    {
        var mockService = new Mock<IFinancialRecordService>();

        var controller = GetController(mockService.Object);

        var request = new CreateFinancialRecordRequest
        {
            Amount = 200,
            Category = "Food",
            Type = "Expense",
            RecordDate = DateTime.UtcNow
        };

        var result = await controller.Create(request);

        result.Should().BeOfType<OkResult>();
    }

    [Fact]
    public async Task Get_ShouldReturnRecords()
    {
        var mockService = new Mock<IFinancialRecordService>();

        mockService.Setup(x => x.GetRecords(null, null, null, null))
            .ReturnsAsync(new List<FinancialRecord>());

        var controller = GetController(mockService.Object);

        var result = await controller.Get(null, null, null, null);

        result.Should().BeOfType<OkObjectResult>();
    }
}