using System.ComponentModel.DataAnnotations;

namespace Cutline.Api.Entities;

public class User
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    [MaxLength(64)]
    public required string Name { get; set; }

    [MaxLength(128)]
    public required string Email { get; set; }
}
