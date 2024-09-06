using Microsoft.AspNetCore.Identity;

namespace MusicCatalog.Api.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? MiddleName { get; set; }
}                              

