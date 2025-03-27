using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIDemo.Authority;

namespace WebAPIDemo.Controllers
{
    [ApiController]
    public class AuthorityController : ControllerBase
    {
        public AuthorityController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] AppCredential credential)
        {
            if (AppRepository.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiry = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = CreateToken(credential.ClientId, expiry),
                    expiresAt = expiry
                });
            }
            else
            {
                ModelState.AddModelError("Unauthorized", "You are not authorised to access the site.");
                var problemDetails = new ValidationProblemDetails(ModelState)
                {
                    Status = StatusCodes.Status401Unauthorized,
                };
                return new UnauthorizedObjectResult(problemDetails);
            }
                
        }

        private string CreateToken(string clientId,DateTime expiry)
        {
            var app = AppRepository.GetApplication(clientId);
            var claims = new List<Claim> 
            {
                new Claim("AppName",app.ApplicationName??string.Empty),
                new Claim("Read",(app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write",(app?.Scopes??string.Empty).Contains("write")?"true":"false"),
            };
            var secretKey = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("Secretkey"));
            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                    claims: claims,
                    expires: expiry,
                    notBefore: DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
