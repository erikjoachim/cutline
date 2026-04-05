namespace Cutline.Api.Features.Leagues;

public static class LeaguesEndpoints
{
    public static void MapLeaguesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/leagues", GetLeagues.Handle)
            .WithName("GetLeagues")
            .WithSummary("Gets leagues")
            .WithTags("Leagues")
            .Produces<GetLeagues.GetLeaguesResponse>();

        app.MapPost("/api/leagues", CreateLeague.Handle)
            .WithName("CreateLeague")
            .WithSummary("Creates a league")
            .WithTags("Leagues")
            .Produces<CreateLeague.CreateLeagueResponse>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
