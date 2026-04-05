using Cutline.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Cutline.Api.Features.Teams;

public static class GetTeams
{
    public static async Task<IResult> Handle(AppDbContext dbContext)
    {
        var teams = await dbContext
            .Team.Include(t => t.Players)
            .Include(t => t.Owner)
            .Select(team => new GetTeamsDto(
                team.Id.ToString(),
                team.Name,
                team.Players.Select(p => new PlayerDto(p.Id.ToString(), p.FullName)).ToList(),
                new OwnerDto(team.Owner!.Id.ToString(), team.Owner.Name)
            ))
            .ToListAsync();

        if (teams.Count == 0)
            return Results.NoContent();

        var response = new GetTeamsResponse(teams);
        return Results.Ok(response);
    }

    public sealed record GetTeamsDto(
        string Id,
        string Name,
        ICollection<PlayerDto> Players,
        OwnerDto Owner
    );

    public sealed record PlayerDto(string Id, string Name);

    public sealed record OwnerDto(string Id, string Name);

    public sealed record GetTeamsResponse(IReadOnlyList<GetTeamsDto> Teams);
}
