using Domain.Common.Constants;
using Domain.Enums;
using Shared.Pagination;

namespace Application.Features.Users;

public record UserParams(int MinAge = SystemPolicy.UsersMinimumAge,
    int? MaxAge = null,
    OrderByOptions OrderBy = OrderByOptions.LastActive, 
    RelationFilter RelationFilter = RelationFilter.All) : PaginationParams;