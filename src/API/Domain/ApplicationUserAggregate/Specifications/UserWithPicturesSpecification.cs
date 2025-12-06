namespace Domain.ApplicationUserAggregate.Specifications;

public class UserWithPicturesSpecification : Specification<ApplicationUser>
{
    public UserWithPicturesSpecification(int userId)
    {
        Filter = new UserByIdFilter(userId);
        AddInclude(u => u.Pictures);
    }
}
