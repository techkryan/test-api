namespace MusicCatalog.Api.Dtos;

public record class UserRecordDto(
    AlbumSummaryDto Album,
    byte? Rating
);
