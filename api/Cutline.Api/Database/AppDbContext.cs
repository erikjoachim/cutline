using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Cutline.Api.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Player> Player { get; set; }
    public DbSet<Tournament> Tournament { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>().HasIndex(p => p.ExternalSystemId).IsUnique();

        modelBuilder.Entity<Tournament>().HasIndex(t => t.ExternalTournamentId).IsUnique();
    }
}

public class Player
{
    public int Id { get; set; }

    public int ExternalSystemId { get; set; }

    [MaxLength(64)]
    public required string FirstName { get; set; }

    [MaxLength(64)]
    public required string LastName { get; set; }

    [MaxLength(128)]
    public required string FullName { get; set; }

    public int CurrentWorldRank { get; set; }
    public int PreviousWorldRank { get; set; }
    public int WorldRankingYear { get; set; }
    public int WorldRankingWeek { get; set; }
}

public class Tournament
{
    public int Id { get; set; }

    [MaxLength(32)]
    public required string ExternalTournamentId { get; set; }

    [MaxLength(128)]
    public required string Name { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int WeekNumber { get; set; }
    public bool IsMajor { get; set; }
}
