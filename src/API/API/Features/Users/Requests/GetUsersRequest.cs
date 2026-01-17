namespace API.Features.Users.Requests;

public record GetUsersRequest(int MinAge = SystemPolicy.UsersMinimumAge,
    int? MaxAge = null,
    OrderByOptions OrderBy = OrderByOptions.LastActive,
    RelationFilter RelationFilter = RelationFilter.All,
    int PageNumber = 1,
    int ItemsPerPage = 10);

