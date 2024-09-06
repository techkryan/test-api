namespace MusicCatalog.Api.Entities;

public class UserRecordEntity
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public required int AlbumId { get; set; }

    public AlbumEntity Album { get; set; } = null!;

    public byte? Rating { get; set; }
}
