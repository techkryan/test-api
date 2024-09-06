namespace MusicCatalog.Api.Dtos;

public record class CreateAlbumDto(
    // string Band,
    string Name,
    int BandId,
    // string Genre,
    int GenreId,
    DateOnly ReleaseDate
);
