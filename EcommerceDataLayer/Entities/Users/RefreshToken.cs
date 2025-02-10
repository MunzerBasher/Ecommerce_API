using Microsoft.EntityFrameworkCore;

namespace EcommerceDataLayer.Entities.Users
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime? ReVokedOn { get; set; }
        public bool IsExpires => ExpiresOn <= DateTime.Now;
        public bool IsActive => ReVokedOn is null && !IsExpires;

    }
}
