using System.ComponentModel.DataAnnotations;

namespace MusicCatalog.Api.Dtos;

public record class CreateUserRecordDto(
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "IDs bigger than {1} are only allowed.")]
    int AlbumId,

    [Range(0, 5)]
    byte? Rating
);
