using Test.Api.Data;
using Test.Api.Dtos;
using Test.Api.Entities;
using Test.Api.Mapping;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Test.Api.Endpoints;

public static class AlbumsEndpoints
{
    // const string GetAlbumEndpointName = "GetAlbum";

    // private static readonly List<AlbumSummaryDto> albums = [
    //     new(
    //         1,
    //         "Bethlehem",
    //         "Dictius Te Necare",
    //         "Black Metal",
    //         new DateOnly(1996, 5, 14)),
    //     new(
    //         2,
    //         "Pink Floyd",
    //         "The Dark Side of the Moon",
    //         "Progressive Rock",
    //         new DateOnly(1972, 3, 1)),
    //     new(
    //         3,
    //         "Joy Division",
    //         "Unknown Pleasures",
    //         "Post-Punk",
    //         new DateOnly(1979, 6, 15))
    // ];

    public static RouteGroupBuilder MapAlbumsEndpoinsts(this WebApplication app)
    {
        var group = app.MapGroup("albums");

        group.MapGet("/", async (CatalogDbContext dbContext) => 
            await dbContext.Albums
                     .Include(album => album.Genre)
                     .Select(album => album.ToAlbumSummaryDto())
                     .AsNoTracking()
                     .ToListAsync());

        group.MapGet("/{id}", async Task<Results<Ok<AlbumDetailsDto>, NotFound>> (int id, CatalogDbContext dbContext) => 
        {
            AlbumEntity? album = await dbContext.Albums.FindAsync(id);

            return album is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(album.ToAlbumDetailsDto());
        });
           // .WithName(GetAlbumEndpointName);

        group.MapPost("/", async (CreateAlbumDto newAlbum, CatalogDbContext dbContext) =>
        {
            AlbumEntity album = newAlbum.ToEntity();

            dbContext.Albums.Add(album);
            await dbContext.SaveChangesAsync();

            return TypedResults.Created("/albums/{id}", album.ToAlbumDetailsDto());
        });

        group.MapPut("/{id}", async Task<Results<NoContent, NotFound>> (int id, UpdateAlbumDto updatedAlbum, CatalogDbContext dbContext) => 
        {
            var existingAlbum = await dbContext.Albums.FindAsync(id);

            if (existingAlbum is null)
            {
                return TypedResults.NotFound();
            }

            dbContext.Entry(existingAlbum)
                     .CurrentValues
                     .SetValues(updatedAlbum.ToEntity(id));

            await dbContext.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async (int id, CatalogDbContext dbContext) =>
        {
            await dbContext.Albums
                           .Where(album => album.Id == id)
                           .ExecuteDeleteAsync();

            return TypedResults.NoContent();
        });

        return group;
    }
}
