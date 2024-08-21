using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Test.Api.Data;
using Test.Api.Mapping;
using Test.Api.Dtos;
using Test.Api.Entities;

namespace Test.Api.Controllers;

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
    public async Task<ActionResult<List<AlbumSummaryDto>>> GetAll()
    {
        var albums = await _dbContext.Albums
            .Include(album => album.Genre)
            .Include(album => album.Band)
            .Select(album => album.ToAlbumSummaryDto())
            .AsNoTracking()
            .ToListAsync();

        return albums;
    }

    /// <summary>
    /// Returns a specific album.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A specific album</returns>
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AlbumDetailsDto>> Create(CreateAlbumDto newAlbum)
    {
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<NoContentResult>> Update(int id, UpdateAlbumDto updatedAlbum)
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
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<NoContentResult>> Delete(int id)
    {
        await _dbContext.Albums
                       .Where(album => album.Id == id)
                       .ExecuteDeleteAsync();

        return NoContent();
    }
}
