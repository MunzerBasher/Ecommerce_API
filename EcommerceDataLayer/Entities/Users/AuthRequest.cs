namespace EcommerceDataLayer.Entities.Users
{
    public class AuthenticationRequest
    {
        public required string role { get; set; }

        public required string UserName { get; set; }
    }
}