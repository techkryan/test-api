namespace Test.Api.Dtos;

public record class UpdateAlbumDto(
    string Name,
    int BandId,
    int GenreId,
    DateOnly ReleaseDate
);
