using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.Models;
using MusicCatalog.Api.Dtos;

namespace MusicCatalog.Api.Repository;

public interface IAlbumRepository
{
    Task<List<AlbumSummaryDto>> GetAllAsync(Query query);

    Task<AlbumDetailsDto?> GetByIdAsync(int id);

    Task<AlbumDetailsDto> CreateAsync(CreateAlbumDto albumDto);

    Task<int?> UpdateAsync(int id, UpdateAlbumDto updatedAlbum);

    Task DeleteAsync(int id);
}
