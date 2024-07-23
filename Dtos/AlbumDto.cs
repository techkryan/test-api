namespace Test.Api.Dtos;

public record class AlbumDto(
    int Id,
    string Band,
    string Name,
    string Genre,
    DateOnly ReleaseDate
);
