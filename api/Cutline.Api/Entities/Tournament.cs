using System.ComponentModel.DataAnnotations;

namespace Cutline.Api.Entities;

public class Tournament
{
    public int Id { get; set; }

    [MaxLength(32)] public required string ExternalTournamentId { get; set; }

    [MaxLength(128)] public required string Name { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int WeekNumber { get; set; }
    public bool IsMajor { get; set; }
}