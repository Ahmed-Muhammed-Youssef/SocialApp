namespace Application.Features.Users;

public enum RelationFilter
{
    All = 0,
    OnlyFriends = 1,
    OnlyFriendRequested = 2,
    ExcludeFriendsAndFriendRequested = 3
}
