using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using MusicCatalog.Api.Models;
using MusicCatalog.Api.Data;

namespace MusicCatalog.Api.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<ActionResult> Register([FromBody] Register model)
    {
        var user = new ApplicationUser {UserName = model.Username};
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok(new {message = "User registered successfully"});
        }

        return BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromBody] Login model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                    SecurityAlgorithms.HmacSha256   
                )
            );
            
            return Ok(new {Token = new JwtSecurityTokenHandler().WriteToken(token)});
        }
        return Unauthorized();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AssignRole([FromBody] UserRole model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user == null)
        {
            return BadRequest("User not found");
        }

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        if (result.Succeeded)
        {
            return Ok(new {message = "Role assigned successfully"});
        }

        return BadRequest(result.Errors);
    }
}                            
