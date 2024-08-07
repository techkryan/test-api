namespace Test.Api.Dtos;

public record class CreateAlbumDto(
    // string Band,
    int BandId,
    string Name,
    // string Genre,
    int GenreId,
    DateOnly ReleaseDate
);
