namespace Test.Api.Entities;

public class AlbumEntity
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required int BandId { get; set; }

    public BandEntity Band { get; set; } = null!;

    public int? GenreId { get; set; }

    public GenreEntity? Genre { get; set; }

    public DateOnly? ReleaseDate { get; set; }
}
