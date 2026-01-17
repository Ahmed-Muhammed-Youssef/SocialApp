namespace Application.Features.Users;

public record UserParams : PaginationParams
{
    public int MinAge { get; init; } = SystemPolicy.UsersMinimumAge;
    public int? MaxAge { get; init; } = null;
    public OrderByOptions OrderBy { get; init; } = OrderByOptions.LastActive;
    public RelationFilter RelationFilter { get; init; } = RelationFilter.All;
}
