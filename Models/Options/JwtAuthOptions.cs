using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BackendServiceStarter.Models.Options
{
    public class JwtAuthOptions
    {
        public string Issuer { get; set; }
        
        public string Audience { get; set; }
        
        public string Key { get; set; }
        
        public uint Lifetime { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        }
    }
}