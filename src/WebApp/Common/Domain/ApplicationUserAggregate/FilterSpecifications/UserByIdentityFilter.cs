namespace Domain.ApplicationUserAggregate.FilterSpecifications
{
    public class UserByIdentityFilter(string identity) : BaseFilterSpecification<ApplicationUser>(u => u.IdentityId == identity)
    {
    }
}
