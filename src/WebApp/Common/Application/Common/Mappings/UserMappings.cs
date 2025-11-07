namespace Application.Common.Mappings;

public static class UserMappings
{
    public static UserDTO ToDto(ApplicationUser user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePictureUrl = user.ProfilePictureUrl,
        Sex = user.Sex,
        Age = user.DateOfBirth.CalculateAge(),
        Created = user.Created,
        LastActive = user.LastActive,
        Bio = user.Bio ?? string.Empty,
        RelationStatus = RelationStatus.None,
        Pictures = user.Pictures.Select(PictureMappings.ToDto)
    };
}
