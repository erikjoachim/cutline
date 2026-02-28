using Cutline.Api.Entities;
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