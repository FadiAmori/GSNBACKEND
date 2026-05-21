using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BudgetBackend.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(int id, string email, string userType, string? name = null);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            var jwtSettings = configuration.GetSection("JwtSettings");
            _secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long-for-hs256";
            _issuer = jwtSettings["Issuer"] ?? "BudgetBackend";
            _audience = jwtSettings["Audience"] ?? "BudgetApp";
        }

        public string GenerateToken(int id, string email, string userType, string? name = null)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim("userType", userType),
            };

            if (!string.IsNullOrWhiteSpace(name))
                claims.Add(new Claim(ClaimTypes.Name, name));

            if (userType == "Admin")
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            else if (userType == "Societe")
                claims.Add(new Claim(ClaimTypes.Role, "Societe"));

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
