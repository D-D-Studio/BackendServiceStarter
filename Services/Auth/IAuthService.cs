using System.Security.Claims;
using System.Threading.Tasks;

namespace BackendServiceStarter.Services.Auth
{
    public interface IAuthService
    {
        public string GenerateToken(ClaimsIdentity identity);
        
        public Task<ClaimsIdentity> GetIdentity(string email, string password);
    }
}