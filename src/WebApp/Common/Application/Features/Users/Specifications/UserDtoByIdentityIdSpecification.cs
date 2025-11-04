using Application.Common.Mappings;
using Domain.Entities;
using Shared.Specification;

namespace Application.Features.Users.Specifications;

public class UserDtoByIdentityIdSpecification : BaseSpecification<ApplicationUser, UserDTO>
{
    public UserDtoByIdentityIdSpecification(string id)
    {
        Criteria = u => u.IdentityId == id;
        Selector = u => UserMappings.ToDto(u);
    }
}
