using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using MusicCatalog.Api.Models;
using MusicCatalog.Api.Services;

namespace MusicCatalog.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult> Register([FromBody] Register model)
    {
        var result = await _authService.Register(model);

        if (result.Succeeded)
        {
            return Ok(new {message = "User registered successfully"});
        }

        return BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromBody] Login model)
    {
        var token = await _authService.Login(model);

        if (token != null)
        {
            return Ok(new {Token = token});
        }

        return Unauthorized();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AssignRole([FromBody] UserRole model)
    {
        var result = await _authService.AssignRole(model);

        if (result == null)
        {
            return BadRequest("User not found");
        }

        if (result.Succeeded)
        {
            return Ok(new {message = "Role assigned successfully"});
        }

        return BadRequest(result.Errors);
    }
}                            
