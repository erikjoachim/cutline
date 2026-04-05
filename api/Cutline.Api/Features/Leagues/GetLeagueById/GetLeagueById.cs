using Cutline.Api.Database;
using Cutline.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cutline.Api.Features.Leagues;

public static class GetLeagueById
{
    public static async Task<IResult> Handle(AppDbContext dbContext, Guid id)
    {
        var league = await dbContext
            .League.Include(l => l.Teams)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league is null)
        {
            return Results.NotFound();
        }

        var response = new GetLeagueByIdResponse(
            league.Id,
            league.Name,
            league.Teams.Select(t => new TeamDto(t.Id, t.Name)).ToList()
        );
        return Results.Ok(response);
    }

    public sealed record GetLeagueByIdResponse(Guid Id, string Name, IReadOnlyList<TeamDto> Teams);

    public sealed record TeamDto(Guid Id, string Name);
}
