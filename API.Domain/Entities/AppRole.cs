using Microsoft.AspNetCore.Identity;

namespace API.Domain.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
