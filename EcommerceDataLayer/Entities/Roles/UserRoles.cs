using Microsoft.AspNetCore.Identity;

namespace EcommerceDataLayer.Entities.Roles
{
    public class UserRoles : IdentityRole
    {
        public bool IsDefault { get; set; }
        public bool IsDeleted { get; set; }
    }
}
