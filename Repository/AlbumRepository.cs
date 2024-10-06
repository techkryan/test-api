using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MusicCatalog.Api.Models;
using MusicCatalog.Api.Data;
using MusicCatalog.Api.Dtos;
using MusicCatalog.Api.Mapping;
using MusicCatalog.Api.Utilities;

namespace MusicCatalog.Api.Repository;

public class AlbumRepository : IAlbumRepository
{
    private readonly CatalogDbContext _dbContext;

    public AlbumRepository(CatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<AlbumSummaryDto>> GetAllAsync([FromQuery] Query query)
    {
        var albums = _dbContext.Albums
            .Include(album => album.Band)
            .Include(album => album.Genre)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Text))
        {
            albums = albums.Where(
                album => EF.Functions.TrigramsAreWordSimilar(query.Text, album.Name));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
            {
                albums = albums.OrderByDirected(album => album.Name, query.IsDescending);
            }

            if (query.SortBy.Equals("Band", StringComparison.OrdinalIgnoreCase))
            {
                albums = albums.OrderByDirected(album => album.Band.Name, query.IsDescending);
            }
        }

        int skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await albums
            .Skip(skipNumber)
            .Take(query.PageSize)
            .Select(album => album.ToAlbumSummaryDto())
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<AlbumDetailsDto?> GetByIdAsync(int id)
    {
        var album = await _dbContext.Albums.FindAsync(id);

        return album?.ToAlbumDetailsDto();
    }

    public async Task<AlbumDetailsDto> CreateAsync(CreateAlbumDto albumDto)
    {
        // _dbContext.Albums.Any(album => album.Name == newAlbum.Name)
        var oldAlbum = _dbContext.Albums
            .Where(album => album.BandId == albumDto.BandId)
            .Include(album => album.Genre)
            .Include(album => album.Band)
            .SingleOrDefault(album => album.Name == albumDto.Name);

        if (oldAlbum is not null)
        {
            return oldAlbum.ToAlbumDetailsDto();
        }

        var album = albumDto.ToEntity();

        await _dbContext.Albums.AddAsync(album);
        await _dbContext.SaveChangesAsync();

        return album.ToAlbumDetailsDto();
    }

    public async Task DeleteAsync(int id)
    {
        await _dbContext.Albums
                       .Where(album => album.Id == id)
                       .ExecuteDeleteAsync();
    }


    public async Task<int?> UpdateAsync(int id, UpdateAlbumDto updatedAlbum)
    {
        var oldAlbum = await _dbContext.Albums.FindAsync(id);

        if (oldAlbum is null)
        {
            return null;
        }
        
        _dbContext.Entry(oldAlbum)?
                         .CurrentValues
                         .SetValues(updatedAlbum.ToEntity(id));

        return await _dbContext.SaveChangesAsync();
        
    }
}
