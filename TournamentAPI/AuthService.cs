using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TournamentAPI
{
    public class AuthService
    {
        public string GenerateToken(User user)
        {
            var key = Encoding.UTF8.GetBytes("MojSuperTajnyKluczDoJWT1234567890123456!");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: "TournamentAPI",
                audience: "TournamentAPI",
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}