namespace Cutline.Api.Features.Players;

public static class PlayersEndpoints
{
    public static void MapPlayersEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/players", GetPlayers.Handle)
            .WithName("GetPlayers")
            .WithSummary("Gets players")
            .WithTags("Players")
            .Produces<GetPlayers.GetPlayersResponse>();
    }
}
