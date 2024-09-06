using MusicCatalog.Api.Dtos;
using MusicCatalog.Api.Entities;

namespace MusicCatalog.Api.Mapping;

public static class AlbumMapping
{
    public static AlbumEntity ToEntity(this CreateAlbumDto album)
    {
        return new AlbumEntity()
        {
            Name = album.Name,
            BandId = album.BandId,
            GenreId = album.GenreId,
            ReleaseDate = album.ReleaseDate
        };
    }

    public static AlbumSummaryDto ToAlbumSummaryDto(this AlbumEntity album)
    {
        return new(
            album.Id,
            album.Name,
            album.Band.Name,
            album.Genre!.Name,
            album.ReleaseDate
        );
    }


    public static AlbumDetailsDto ToAlbumDetailsDto(this AlbumEntity album)
    {
        return new(
            album.Id,
            album.Name,
            album.BandId,
            album.GenreId,
            album.ReleaseDate
        );
    }

    public static AlbumEntity ToEntity(this UpdateAlbumDto album, int id)
    {
        return new AlbumEntity()
        {
            Id = id,
            Name = album.Name,
            BandId = album.BandId,
            GenreId = album.GenreId,
            ReleaseDate = album.ReleaseDate
        };
    }
}


