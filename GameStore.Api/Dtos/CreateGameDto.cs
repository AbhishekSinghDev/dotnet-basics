using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record class CreateGameDto(
    [Required][StringLength(40)] string Name,
    [Required][StringLength(20)] string Genre,
    [Required][Range(0, 1000)] decimal Price,
    DateOnly ReleaseDate
);
