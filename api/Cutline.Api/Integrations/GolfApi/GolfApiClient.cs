using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cutline.Api.Integrations.GolfApi;

public class GolfApiClient
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private const string WORLD_RANKINGS_STAT_ID = "186";

    public GolfApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WorldRankingsResponse?> GetWorldRankingsAsync(
        string year,
        CancellationToken cancellationToken = default
    )
    {
        return await _httpClient.GetFromJsonAsync<WorldRankingsResponse>(
            $"stats?year={year}&statId={WORLD_RANKINGS_STAT_ID}",
            _jsonOptions,
            cancellationToken
        );
    }

    public async Task<ScheduleResponse?> GetScheduleAsync(
        string year,
        string orgId,
        CancellationToken cancellationToken = default
    )
    {
        return await _httpClient.GetFromJsonAsync<ScheduleResponse>(
            $"schedule?year={year}&orgId={orgId}",
            _jsonOptions,
            cancellationToken
        );
    }
}

public class WorldRankingsResponse
{
    [JsonPropertyName("year")]
    public string? Year { get; set; }

    [JsonPropertyName("statId")]
    public string? StatId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("weekNum")]
    public int WeekNum { get; set; }

    [JsonPropertyName("rankings")]
    public List<WorldRanking>? Rankings { get; set; }
}

public class WorldRanking
{
    [JsonPropertyName("playerId")]
    public string? PlayerId { get; set; }

    [JsonPropertyName("firstName")]
    public string? FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public string? LastName { get; set; }

    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("previousRank")]
    public int PreviousRank { get; set; }

    [JsonPropertyName("events")]
    public int Events { get; set; }

    [JsonPropertyName("totalPoints")]
    public double TotalPoints { get; set; }

    [JsonPropertyName("avgPoints")]
    public double AvgPoints { get; set; }

    [JsonPropertyName("pointsLost")]
    public double PointsLost { get; set; }

    [JsonPropertyName("pointsGained")]
    public double PointsGained { get; set; }
}

public class ScheduleResponse
{
    [JsonPropertyName("year")]
    public string? Year { get; set; }

    [JsonPropertyName("schedule")]
    public List<ScheduleItem>? Schedule { get; set; }
}

public class ScheduleItem
{
    [JsonPropertyName("tournId")]
    public string? TournId { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("date")]
    public ScheduleDate? Date { get; set; }

    [JsonPropertyName("format")]
    public string? Format { get; set; }

    [JsonPropertyName("purse")]
    public long Purse { get; set; }

    [JsonPropertyName("fedexCupPoints")]
    public int FedexCupPoints { get; set; }
}

public class ScheduleDate
{
    [JsonPropertyName("weekNumber")]
    public string? WeekNumber { get; set; }

    [JsonPropertyName("start")]
    public string? Start { get; set; }

    [JsonPropertyName("end")]
    public string? End { get; set; }
}
