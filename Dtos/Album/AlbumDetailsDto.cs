namespace MusicCatalog.Api.Dtos;

public record class AlbumDetailsDto(
    int Id,
    string Name,
    int BandId,
    int? GenreId,
    DateOnly? ReleaseDate
);
