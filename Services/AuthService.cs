using MusicCatalog.Api.Models;
using Microsoft.AspNetCore.Identity;
using MusicCatalog.Api.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MusicCatalog.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> Register(Register model)
    {
        var user = new ApplicationUser {UserName = model.Username, Email = model.Email};
        var result = await _userManager.CreateAsync(user, model.Password);

        return result;
    }

    public async Task<string> GenerateToken(ApplicationUser user)
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

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
        
    public async Task<string> Login(Login model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return await GenerateToken(user); 
        }
        return null!;
    }

    public async Task<IdentityResult> AssignRole(UserRole model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user != null)
        {
            return  await _userManager.AddToRoleAsync(user, model.Role);
        }
            return null!;
    }
}
