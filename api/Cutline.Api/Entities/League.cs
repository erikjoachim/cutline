using System.ComponentModel.DataAnnotations;

namespace Cutline.Api.Entities;

public class League
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    [MaxLength(64)]
    public required string Name { get; set; }

    public ICollection<Team> Teams { get; set; } = new List<Team>();
}
