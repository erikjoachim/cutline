using Cutline.Api.Database;
using Cutline.Api.Integrations.GolfApi;
using Microsoft.EntityFrameworkCore;
using TickerQ.Utilities.Base;

namespace Cutline.Api.Jobs;

public class FetchPlayersJob
{
    private readonly AppDbContext _dbContext;
    private readonly GolfApiClient _golfApiClient;

    private const string DEFAULT_YEAR = "2026";

    public FetchPlayersJob(AppDbContext dbContext, GolfApiClient golfApiClient)
    {
        _dbContext = dbContext;
        _golfApiClient = golfApiClient;
    }

    [TickerFunction(nameof(FetchPlayersJob))]
    public async Task ExecuteAsync(
        TickerFunctionContext context,
        CancellationToken cancellationToken = default
    )
    {
        var worldRankings = await _golfApiClient.GetWorldRankingsAsync(
            DEFAULT_YEAR,
            cancellationToken
        );

        if (worldRankings?.Rankings == null || worldRankings.Rankings.Count == 0)
            return;

        var existingPlayers = await _dbContext.Player.ToDictionaryAsync(
            p => p.ExternalSystemId,
            cancellationToken
        );

        var currentYear = int.TryParse(DEFAULT_YEAR, out var y) ? y : DateTime.UtcNow.Year;
        var weekNum = worldRankings.WeekNum;

        foreach (var ranking in worldRankings.Rankings)
        {
            if (!int.TryParse(ranking.PlayerId, out var playerId))
                continue;

            if (existingPlayers.TryGetValue(playerId, out var existingPlayer))
            {
                existingPlayer.FirstName = ranking.FirstName ?? existingPlayer.FirstName;
                existingPlayer.LastName = ranking.LastName ?? existingPlayer.LastName;
                existingPlayer.FullName = $"{ranking.FirstName} {ranking.LastName}".Trim();
                existingPlayer.CurrentWorldRank = ranking.Rank;
                existingPlayer.PreviousWorldRank = ranking.PreviousRank;
                existingPlayer.WorldRankingYear = currentYear;
                existingPlayer.WorldRankingWeek = weekNum;
            }
            else
            {
                var newPlayer = new Player
                {
                    ExternalSystemId = playerId,
                    FirstName = ranking.FirstName ?? string.Empty,
                    LastName = ranking.LastName ?? string.Empty,
                    FullName = $"{ranking.FirstName} {ranking.LastName}".Trim(),
                    CurrentWorldRank = ranking.Rank,
                    PreviousWorldRank = ranking.PreviousRank,
                    WorldRankingYear = currentYear,
                    WorldRankingWeek = weekNum,
                };
                _dbContext.Player.Add(newPlayer);
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
