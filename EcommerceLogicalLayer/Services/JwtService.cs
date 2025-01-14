using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EcommerceLogicalLayer.Services
{
    public class CreateJwtToken
    {
        private readonly Jwt _jwt;

        public CreateJwtToken(Jwt jwt1)
        {
            _jwt = jwt1;
        }

        public string CreateJwToken(AuthenticationRequest authenticationRequest)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwt.Issure,
                Audience = _jwt.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                    SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new (ClaimTypes.NameIdentifier, authenticationRequest.UserName),
                    new (ClaimTypes.Email, "a@b.com"),
                    new (ClaimTypes.Role,authenticationRequest.role),
                })

            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(securityToken);
        }


    }


}