namespace Test.Api.Dtos;

public record class AlbumSummaryDto(
    int Id,
    string Name,
    string Band,
    string Genre,
    DateOnly ReleaseDate
);
