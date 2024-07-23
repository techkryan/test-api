namespace Test.Api.Dtos;

public record class UpdateAlbumDto(
    string Band,
    string Name,
    string Genre,
    DateOnly ReleaseDate
);
