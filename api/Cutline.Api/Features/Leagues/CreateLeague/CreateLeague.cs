using Cutline.Api.Database;
using Cutline.Api.Entities;

namespace Cutline.Api.Features.Leagues;

public static class CreateLeague
{
    public static async Task<IResult> Handle(AppDbContext dbContext, CreateLeagueRequest request)
    {
        var league = new League { Name = request.Name };
        dbContext.League.Add(league);
        await dbContext.SaveChangesAsync();

        var response = new CreateLeagueResponse(league.Id, league.Name);
        return Results.Created($"/api/leagues/{league.Id}", response);
    }

    public sealed record CreateLeagueRequest(string Name);

    public sealed record CreateLeagueResponse(Guid Id, string Name);
}
