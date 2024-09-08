using MusicCatalog.Api.Models;
using MusicCatalog.Api.Data;
using Microsoft.AspNetCore.Identity;

namespace MusicCatalog.Api.Services;

public interface IAuthService
{
    Task<string> Login(Login user);
    Task<string> GenerateToken(ApplicationUser user);
    Task<IdentityResult> Register(Register user);
    Task<IdentityResult> AssignRole(UserRole model);
}
