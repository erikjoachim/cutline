namespace Cutline.Api.Domain.Entities;

public class Item : BaseEntity
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
