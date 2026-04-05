using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateUserRequest request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        await _service.CreateUser(request, email);

        return Ok();
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Analyst,Viewer")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.GetUsers();

        return Ok(users);
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(UpdateUserRequest request)
    {
        var email = User.FindFirst(ClaimTypes.Email)?.Value;

        await _service.UpdateUser(request, email);

        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _service.DeactivateUser(id);

        return Ok();
    }
}