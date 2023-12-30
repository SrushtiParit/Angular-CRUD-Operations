using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Project1API.Controllers
{
    public class TokenController : ApiController
    {
        private const string SecretKey = "yourSecretKey1234567890123456"; // Replace with your actual secret key

        [HttpGet]
        public IHttpActionResult VerifyToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = "yourIssuer", // Replace with your actual issuer
                    ValidateAudience = true,
                    ValidAudience = "yourAudience", // Replace with your actual audience
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Set zero if you don't want to tolerate any clock skew
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // If the token is valid, you can access claims from the principal
                var username = principal.FindFirst(ClaimTypes.Name)?.Value;

                // Perform additional verification or logic as needed

                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest($"Token verification failed: {ex.Message}");
            }
        }
    }
}
