namespace Application.Features.Users.Specifications;

public class UserByIdentitySpecification : Specification<ApplicationUser>
{
    public UserByIdentitySpecification(string identity)
    {
        Filter = new UserByIdentityFilter(identity);
    }
}
