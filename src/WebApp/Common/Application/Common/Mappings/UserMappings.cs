namespace Application.Common.Mappings;

public static class UserMappings
{
    public static UserDTO ToDto(ApplicationUser user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Gender = user.Gender,
        Age = user.DateOfBirth.CalculateAge(),
        Created = user.Created,
        LastActive = user.LastActive,
        Bio = user.Bio ?? string.Empty,
        RelationStatus = RelationStatus.None
    };

    public static Expression<Func<ApplicationUser, UserDTO>> ToDtoExpression => user => new UserDTO()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Gender = user.Gender,
        Age = DateTime.UtcNow.Year - user.DateOfBirth.Year
              - (DateTime.UtcNow < user.DateOfBirth.AddYears(DateTime.UtcNow.Year - user.DateOfBirth.Year) ? 1 : 0),
        Created = user.Created,
        LastActive = user.LastActive,
        Bio = user.Bio ?? string.Empty,
        RelationStatus = RelationStatus.None
    };
}
