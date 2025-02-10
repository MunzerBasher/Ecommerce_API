using Microsoft.AspNetCore.Identity;

namespace EcommerceDataLayer.Entities.Users
{
    public class UserIdentity : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public bool  IsDesable { get; set; } = false;

        public List<RefreshToken> RefreshTokens { get; set; } = [];

    }
}
