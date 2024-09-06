namespace MusicCatalog.Api.Entities;

public class BandEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Country { get; set; }

    public int? FormedIn { get; set; }
}
