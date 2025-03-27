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
            if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
            {
                var expiry = DateTime.UtcNow.AddMinutes(10);
                return Ok(new
                {
                    access_token = Authenticator.CreateToken(credential.ClientId, expiry,Configuration.GetValue<string>("SecretKey")),
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
    }
}
