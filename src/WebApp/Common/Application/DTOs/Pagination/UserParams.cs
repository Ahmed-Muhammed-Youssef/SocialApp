using Domain.Constants;
using Domain.Enums;
using Shared.Pagination;

namespace Application.DTOs.Pagination;

public record UserParams : PaginationParams
{
    private int minAge = SystemPolicy.UsersMinimumAge;
    private int? maxAge = null;
    public OrderByOptions OrderBy { get; init; } = OrderByOptions.LastActive;
    public int MinAge
    {
        get
        {
            return minAge;
        }
        init
        {
            if (value >= SystemPolicy.UsersMinimumAge)
            {
                minAge = value;
            }
        }
    }
    public int? MaxAge
    {
        get
        {
            return maxAge;
        }
        init
        {
            if (value >= minAge)
            {
                maxAge = value;
            }
        }
    }
}
