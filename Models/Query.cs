namespace MusicCatalog.Api.Models;

public class Query
{
    public string? Text { get; set; }

    public string? SortBy { get; set; }

    public bool IsDescending { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 1;
}
