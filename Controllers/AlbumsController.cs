using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using MusicCatalog.Api.Dtos;
using MusicCatalog.Api.Models;
using MusicCatalog.Api.Repository;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlbumsController : ControllerBase
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumsController(IAlbumRepository albumRepository)
    {
        _albumRepository = albumRepository;
    }
    
    /// <summary>
    /// Returns all albums.
    /// </summary>
    /// <returns>All albums</returns>

    [HttpGet]
    public async Task<ActionResult<List<AlbumSummaryDto>>> Get([FromQuery] Query query)
    {
        return await _albumRepository.GetAllAsync(query);
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
        var album = await _albumRepository.GetByIdAsync(id);

        return album is null
            ? NotFound()
            : album;
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
        var album = await _albumRepository.CreateAsync(newAlbum);

        if (album is not null)
        {
            return Conflict(new {
                    message = $"An album with the name '{album.Name}' already exists for this band.",
                    content = album});
        }

        return CreatedAtAction(nameof(GetById), new { id = album!.Id },  album);
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
        var result = await _albumRepository.UpdateAsync(id, updatedAlbum);

        if (result is null)
        {
            return NotFound();
        }

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
        await _albumRepository.DeleteAsync(id);

        return NoContent();
    }
}
