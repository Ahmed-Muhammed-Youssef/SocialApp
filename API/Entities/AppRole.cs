using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace API.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
    }
}
