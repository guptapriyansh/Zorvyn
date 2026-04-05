using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Analyst,Viewer")]
[EnableRateLimiting("ApiLimiter")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;

    public DashboardController(IDashboardService service)
    {
        _service = service;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetDashboard()
    {
        var data = await _service.GetDashboardSummary();

        return Ok(data);
    }
}