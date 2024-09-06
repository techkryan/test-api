namespace MusicCatalog.Api.Dtos;

public record class CreateUserRecordDto(
    int AlbumId,
    byte? Rating
);
