using System.ComponentModel.DataAnnotations;

namespace Cutline.Api.Entities;

public class Player
{
    public int Id { get; set; }

    public int ExternalSystemId { get; set; }

    [MaxLength(64)] public required string FirstName { get; set; }

    [MaxLength(64)] public required string LastName { get; set; }

    [MaxLength(128)] public required string FullName { get; set; }

    public int CurrentWorldRank { get; set; }
    public int PreviousWorldRank { get; set; }
    public int WorldRankingYear { get; set; }
    public int WorldRankingWeek { get; set; }
}