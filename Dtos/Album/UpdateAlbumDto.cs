using System.ComponentModel.DataAnnotations;
using MusicCatalog.Api.Utilities;

namespace MusicCatalog.Api.Dtos;

public record class UpdateAlbumDto(
    [Required]
    [MaxLength(128)]
    string Name,

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "IDs bigger than {1} are only allowed.")]
    int BandId,

    [Range(1, int.MaxValue, ErrorMessage = "IDs bigger than {1} are only allowed.")]
    int GenreId,

    [YearRange(1912, ErrorMessage = "An album must have the release date in the year range between {1} and {2}.")]
    DateOnly ReleaseDate
);
