using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace VanadoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // 1. Citanje Basic Auth headera
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized("Nedostaje Authorization header");

            var authHeader = Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Unauthorized("Authorization mora biti Basic");

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            string decodedCredentials;

            try
            {
                decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            }
            catch
            {
                return Unauthorized("Neispravan Base64 format");
            }

            var parts = decodedCredentials.Split(':');
            if (parts.Length != 2)
                return Unauthorized("Neispravan format korisničkog imena i lozinke");

            var username = parts[0];
            var password = parts[1];

            // 2. Provjera hardkodiranog korisnika - samo za testiranje
            if (username != "admin" || password != "admin")
                return Unauthorized("Pogrešno korisničko ime ili lozinka");

            // 3. Generiraj JWT token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}