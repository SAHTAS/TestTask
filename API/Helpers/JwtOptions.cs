using Microsoft.IdentityModel.Tokens;

namespace API.Helpers
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}