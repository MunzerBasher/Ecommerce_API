using EcommerceDataLayer.Entities.Users;

namespace Api.Auth
{
    public interface IJwtToken
    {
        public string GenerateToken(UserIdentity user, IEnumerable<string> roles, IEnumerable<string> permissions);

        string? ValidateToken(string token);


    }
}
