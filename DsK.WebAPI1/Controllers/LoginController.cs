using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DsK.WebAPI1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var token = await GenerateAuthenticationToken();
            return Ok(token);
        }

        private async Task<string> GenerateAuthenticationToken()
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThisIsTheKeyThisIsTheKeyThisIsTheKeyThisIsTheKey"));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            
            var userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.Email, "email@domain.com"));
            userClaims.Add(new Claim("UserId", "ThisIsAUserID"));
            userClaims.Add(new Claim("UserName", "ThisIsAUserName"));
            userClaims.Add(new Claim(ClaimTypes.Role, "ThisIsARole"));
            

            var newJwtToken = new JwtSecurityToken(
                    issuer: "DsKIssuer",
                    audience: "DsKAudience",
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: credentials,
                    claims: userClaims
            );

            string token = new JwtSecurityTokenHandler().WriteToken(newJwtToken);
            return token;
        }
    }
}
