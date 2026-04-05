using Cutline.Api.Database;
using Microsoft.EntityFrameworkCore;

namespace Cutline.Api.Features.Players;

public static class GetPlayers
{
    public const int DefaultTake = 50;
    public const int MaxTake = 200;

    public static async Task<IResult> Handle(
        AppDbContext dbContext,
        PlayerSortOrder sortBy = PlayerSortOrder.WorldRanking,
        int take = DefaultTake
    )
    {
        var effectiveTake = Math.Clamp(take, 1, MaxTake);

        var query = dbContext.Player.AsQueryable();

        query = sortBy switch
        {
            PlayerSortOrder.WorldRanking => query.OrderBy(p => p.CurrentWorldRank),
            PlayerSortOrder.FullName => query.OrderBy(p => p.FullName),
            _ => query.OrderBy(p => p.CurrentWorldRank),
        };

        var players = await query
            .Take(effectiveTake)
            .Select(p => new GetPlayersDto(
                p.ExternalSystemId,
                p.FullName,
                p.CurrentWorldRank,
                PlayerImageUrlHelper(p.ExternalSystemId)
            ))
            .ToListAsync();

        var response = new GetPlayersResponse(players);
        return Results.Ok(response);
    }

    public sealed record GetPlayersDto(
        int Id,
        string FullName,
        int WorldRanking,
        string ProfileImageUrl
    );

    public sealed record GetPlayersResponse(IReadOnlyList<GetPlayersDto> Players);

    private static string PlayerImageUrlHelper(int playerId)
    {
        return $"https://pga-tour-res.cloudinary.com/image/upload/c_fill,g_face:center,q_auto,f_auto,dpr_2.0,h_220,w_200,d_stub:default_avatar_light.webp/headshots_{playerId}";
    }
}

public enum PlayerSortOrder
{
    WorldRanking,
    FullName,
}
