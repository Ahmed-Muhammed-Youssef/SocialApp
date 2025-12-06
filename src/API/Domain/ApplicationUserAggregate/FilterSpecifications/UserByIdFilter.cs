namespace Domain.ApplicationUserAggregate.FilterSpecifications;

public class UserByIdFilter(int userId) : BaseFilterSpecification<ApplicationUser>(u => u.Id == userId)
{
}
