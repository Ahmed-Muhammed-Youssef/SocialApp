namespace Application.Features.Users.Specifications;

public class UserDtoByIdentitySpecification : Specification<ApplicationUser, UserDTO>
{
    public UserDtoByIdentitySpecification(string identity) : base(UserMappings.ToDtoExpression)
    {
        Filter = new UserByIdentityFilter(identity);
    }
}
