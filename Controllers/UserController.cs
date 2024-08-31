using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Test.Api.Data;
using Test.Api.Mapping;
using Test.Api.Dtos;
using Test.Api.Entities;

namespace Test.Api.Controllers;

[Authorize(Roles = "User")]
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(CatalogDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [Route("~/api/[controller]")]
    [HttpGet]
    public ActionResult Get()
    {
        return Ok("You have accessed the User controller.");
    }

    [HttpGet]
    public async Task<ActionResult<List<UserRecordDto>>> ListAlbums()
    {
        var records = await _dbContext.UserRecords
            .Include(r => r.Album)
                .ThenInclude(a => a.Band)
            .Include(r => r.Album)
                .ThenInclude(a => a.Genre)
            .Select(r => new UserRecordDto(
                r.Album.ToAlbumSummaryDto(),
                r.Rating
            ))
            .AsNoTracking()
            .ToListAsync();

        return records;
    }

    [HttpPost]
    public async Task<ActionResult> AddAlbum(CreateUserRecordDto payload)
    {
        var userId =  User.FindFirstValue(ClaimTypes.NameIdentifier);

        var record = new UserRecordEntity()
        {
            UserId = userId!,
            AlbumId = payload.AlbumId,
            Rating = payload.Rating
        };

        _dbContext.UserRecords.Add(record);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult<NoContentResult>> RemoveAlbum(int id)
    {
        await _dbContext.UserRecords
                        .Where(r => r.Album.Id == id)
                        .ExecuteDeleteAsync();

        return NoContent();
    }


}
