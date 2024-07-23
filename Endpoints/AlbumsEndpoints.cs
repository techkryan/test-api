using Test.Api.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Test.Api.Endpoints;

public static class AlbumsEndpoints
{
    // const string GetAlbumEndpointName = "GetAlbum";

    private static readonly List<AlbumDto> albums = [
        new(
            1,
            "Bethlehem",
            "Dictius Te Necare",
            "Black Metal",
            new DateOnly(1996, 5, 14)),
        new(
            2,
            "Pink Floyd",
            "The Dark Side of the Moon",
            "Progressive Rock",
            new DateOnly(1972, 3, 1)),
        new(
            3,
            "Joy Division",
            "Unknown Pleasures",
            "Post-Punk",
            new DateOnly(1979, 6, 15))
    ];

    public static RouteGroupBuilder MapAlbumsEndpoinsts(this WebApplication app)
    {
        var group = app.MapGroup("albums");

        group.MapGet("/", () => albums);

        group.MapGet("/{id}", Results<Ok<AlbumDto>, NotFound> (int id) => 
        {
            AlbumDto? album = albums.Find(album => album.Id == id);

            return album is null
                ? TypedResults.NotFound()
                : TypedResults.Ok(album);
        });
           // .WithName(GetAlbumEndpointName);

        group.MapPost("/", (CreateAlbumDto newAlbum) =>
        {
            AlbumDto album = new(
                albums.Count + 1,
                newAlbum.Band,
                newAlbum.Name,
                newAlbum.Genre,
                newAlbum.ReleaseDate);

            albums.Add(album);

            return TypedResults.Created("/albums/{id}", album);
        });

        group.MapPut("/{id}", Results<NoContent, NotFound> (int id, UpdateAlbumDto updatedAlbum) => 
        {
            var index = albums.FindIndex(album => album.Id == id);

            if (index == -1)
            {
                return TypedResults.NotFound();
            }

            albums[index] = new AlbumDto(
                id,
                updatedAlbum.Band,
                updatedAlbum.Name,
                updatedAlbum.Genre,
                updatedAlbum.ReleaseDate
            );

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", (int id) =>
        {
            albums.RemoveAll(album => album.Id == id);

            return TypedResults.NoContent();
        });

        return group;
    }
}
