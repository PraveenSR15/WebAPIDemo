using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPIDemo.Authority
{
    public class Authenticator
    {
        public static bool Authenticate(string clientId, string secret)
        {
            var app = AppRepository.GetApplication(clientId);
            if (app == null)
                return false;
            return (app.ClientId == clientId && app.Secret == secret);
        }

        public static string CreateToken(string clientId, DateTime expiry, string strSecretKey)
        {
            var app = AppRepository.GetApplication(clientId);
            var claims = new List<Claim>
            {
                new Claim("AppName",app.ApplicationName??string.Empty),
                new Claim("Read",(app?.Scopes??string.Empty).Contains("read")?"true":"false"),
                new Claim("Write",(app?.Scopes??string.Empty).Contains("write")?"true":"false"),
            };
            var secretKey = Encoding.ASCII.GetBytes(strSecretKey);
            var jwt = new JwtSecurityToken(
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature),
                    claims: claims,
                    expires: expiry,
                    notBefore: DateTime.UtcNow);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        internal static bool VerifyToken(StringValues token, string strSecretKey)
        {
            if(string.IsNullOrWhiteSpace(token)) return false;

            SecurityToken securityToken;
            try
            {
                var secretKey = Encoding.ASCII.GetBytes(strSecretKey);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out securityToken);
            }
            catch(SecurityTokenException e)
            {
                return false;
            }
            catch(Exception e)
            {
                throw;
            }

            return securityToken != null;
        }
    }
}
