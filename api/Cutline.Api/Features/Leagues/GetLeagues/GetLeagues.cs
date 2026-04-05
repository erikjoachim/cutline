using Cutline.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Cutline.Api.Features.Leagues;

public static class GetLeagues
{
    public static async Task<IResult> Handle(AppDbContext dbContext)
    {
        var leagues = await dbContext
            .League.Include(l => l.Teams)
            .Select(l => new GetLeaguesDto(
                l.Id,
                l.Name,
                l.Teams.Select(t => new TeamDto(t.Id, t.Name)).ToList()
            ))
            .ToListAsync();

        var response = new GetLeaguesResponse(leagues);
        return Results.Ok(response);
    }

    public sealed record GetLeaguesDto(Guid Id, string Name, IReadOnlyList<TeamDto> Teams);

    public sealed record TeamDto(Guid Id, string Name);

    public sealed record GetLeaguesResponse(IReadOnlyList<GetLeaguesDto> Leagues);
}
