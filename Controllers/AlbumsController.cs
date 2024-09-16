using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using MusicCatalog.Api.Data;
using MusicCatalog.Api.Mapping;
using MusicCatalog.Api.Dtos;
using MusicCatalog.Api.Entities;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlbumsController : ControllerBase
{
    private readonly CatalogDbContext _dbContext;

    public AlbumsController(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    /// <summary>
    /// Returns all albums.
    /// </summary>
    /// <returns>All albums</returns>

    [HttpGet]
    public async Task<ActionResult<List<AlbumSummaryDto>>> Get([FromQuery] string? query)
    {
        return await _dbContext.Albums
            .Where(album => query == null || EF.Functions.TrigramsAreWordSimilar(query, album.Band.Name + album.Name))
            .Include(album => album.Band)
            .Include(album => album.Genre)
            .Select(album => album.ToAlbumSummaryDto())
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Returns a specific album.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A specific album</returns>
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<AlbumDetailsDto>> GetById(int id)
    {
        AlbumEntity? album = await _dbContext.Albums.FindAsync(id);

        return album is null
            ? NotFound()
            : album.ToAlbumDetailsDto();
    }

    /// <summary>
    /// Creates a provided album.
    /// </summary>
    /// <param name="newAlbum"></param>
    /// <returns>A newly created album</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /albums
    ///     {
    ///         "name": "Music Album",
    ///         "bandId": 1,
    ///         "genreId": 1,
    ///         "releaseDate": "2000-12-31"
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">The item is null</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AlbumDetailsDto>> Create(CreateAlbumDto newAlbum)
    {
        var oldAlbum = _dbContext.Albums
            .Where(album => album.BandId == newAlbum.BandId)
            .Include(album => album.Genre)
            .Include(album => album.Band)
            .SingleOrDefault(album => album.Name == newAlbum.Name);
        // _dbContext.Albums.Any(album => album.Name == newAlbum.Name)
        if (oldAlbum is not null)
        {
            return Conflict(new {
                    message = $"An album with the name '{oldAlbum.Name}' already exists.",
                    content = oldAlbum.ToAlbumSummaryDto()});
        }

        AlbumEntity album = newAlbum.ToEntity();

        _dbContext.Albums.Add(album);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = album.Id }, album.ToAlbumDetailsDto());
    }

    /// <summary>
    /// Updates a specific album.
    /// </summary>
    /// <param name="id">ID of the album to update</param>
    /// <param name="updatedAlbum"></param>
    /// <response code="204">The item updated successfully</response>
    /// <response code="404">The item not found</response>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, UpdateAlbumDto updatedAlbum)
    {
        var existingAlbum = await _dbContext.Albums.FindAsync(id);

        if (existingAlbum is null)
        {
            return NotFound();
        }

        _dbContext.Entry(existingAlbum)
                 .CurrentValues
                 .SetValues(updatedAlbum.ToEntity(id));

        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Deletes a specific album.
    /// </summary>
    /// <param name="id">ID of the album to delete</param>
    /// <response code="204">The item deleted successfully</response>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Delete(int id)
    {
        await _dbContext.Albums
                       .Where(album => album.Id == id)
                       .ExecuteDeleteAsync();

        return NoContent();
    }
}
