namespace API.Features.Users.Requests;

/// <summary>
/// Request parameters for getting a paginated list of users.
/// </summary>
/// <param name="MinAge">The minimum age of users to include.</param>
/// <param name="MaxAge">The maximum age of users to include.</param>
/// <param name="OrderBy">How to order the results (e.g. by LastActive).</param>
/// <param name="RelationFilter">Filter by relation (e.g. Friends, Pending).</param>
/// <param name="PageNumber">The page number to retrieve.</param>
/// <param name="ItemsPerPage">The number of items per page.</param>
public record GetUsersRequest(int MinAge = SystemPolicy.UsersMinimumAge,
    int? MaxAge = null,
    OrderByOptions OrderBy = OrderByOptions.LastActive,
    RelationFilter RelationFilter = RelationFilter.All,
    int PageNumber = 1,
    int ItemsPerPage = 10);

