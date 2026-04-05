using System.ComponentModel.DataAnnotations;

namespace Cutline.Api.Entities;

public class Team
{
    public Guid Id { get; init; } = Guid.CreateVersion7();

    [MaxLength(64)]
    public required string Name { get; set; }

    public Guid? OwnerId { get; set; }
    public User? Owner { get; set; }

    public Guid? LeagueId { get; set; }
    public League? League { get; set; }

    public ICollection<Player> Players { get; set; } = new List<Player>();
}
