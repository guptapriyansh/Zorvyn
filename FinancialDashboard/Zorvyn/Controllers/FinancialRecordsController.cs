using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[Route("api/[controller]/[action]")]
[EnableRateLimiting("ApiLimiter")]
[Authorize]
public class FinancialRecordsController : ControllerBase
{
    private readonly IFinancialRecordService _service;
    public FinancialRecordsController(IFinancialRecordService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateFinancialRecordRequest request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        await _service.CreateRecord(request, email);
        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Analyst")]
    public async Task<IActionResult> Get(DateTime? startDate, DateTime? endDate, string category, string type)
    {
        var data = await _service.GetRecords(startDate, endDate, category, type);
        return Ok(data);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UpdateFinancialRecordRequest request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        await _service.UpdateRecord(request, email);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteRecord(id);
        return Ok();
    }
}