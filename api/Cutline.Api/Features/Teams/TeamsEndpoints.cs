namespace Cutline.Api.Features.Teams;

public static class TeamsEndpoints
{
    public static void MapTeamsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/teams", GetTeams.Handle)
            .WithName("GetTeams")
            .WithSummary("Gets all teams")
            .WithTags("Teams")
            .Produces<GetTeams.GetTeamsResponse>();
    }
}
