namespace Test.Api.Dtos;

public record class CreateAlbumDto(
    string Band,
    string Name,
    string Genre,
    DateOnly ReleaseDate
);
