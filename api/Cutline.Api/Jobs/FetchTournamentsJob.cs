using Cutline.Api.Database;
using Cutline.Api.Entities;
using Cutline.Api.Integrations.GolfApi;
using Microsoft.EntityFrameworkCore;
using TickerQ.Utilities.Base;

namespace Cutline.Api.Jobs;

public class FetchTournamentsJob
{
    private readonly AppDbContext _dbContext;
    private readonly GolfApiClient _golfApiClient;

    private const string DEFAULT_YEAR = "2026";
    private const string TOURNAMENT_ORG_ID = "1"; // PGA Tour

    public FetchTournamentsJob(AppDbContext dbContext, GolfApiClient golfApiClient)
    {
        _dbContext = dbContext;
        _golfApiClient = golfApiClient;
    }

    [TickerFunction(nameof(FetchTournamentsJob))]
    public async Task ExecuteAsync(
        TickerFunctionContext ticker,
        CancellationToken cancellationToken = default
    )
    {
        var schedule = await _golfApiClient.GetScheduleAsync(
            DEFAULT_YEAR,
            TOURNAMENT_ORG_ID,
            cancellationToken
        );

        if (schedule?.Schedule == null || schedule.Schedule.Count == 0)
            return;

        var existingIds = await _dbContext
            .Tournament.Select(t => t.ExternalTournamentId)
            .ToListAsync(cancellationToken);

        var tournamentsToInsert = schedule
            .Schedule.Where(s => s.TournId != null && !existingIds.Contains(s.TournId))
            .Select(s => new Tournament
            {
                ExternalTournamentId = s.TournId!,
                Name = s.Name ?? string.Empty,
                StartDate = ParseUtcDateTime(s.Date?.Start),
                EndDate = ParseUtcDateTime(s.Date?.End),
                WeekNumber = int.TryParse(s.Date?.WeekNumber, out var week) ? week : 0,
                IsMajor = false,
            })
            .ToList();

        if (tournamentsToInsert.Count > 0)
        {
            _dbContext.Tournament.AddRange(tournamentsToInsert);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private static DateTime ParseUtcDateTime(string? utcString)
    {
        if (string.IsNullOrEmpty(utcString))
            return DateTime.MinValue;

        return DateTime.TryParse(utcString, out var date)
            ? DateTime.SpecifyKind(date, DateTimeKind.Utc)
            : DateTime.MinValue;
    }
}
