using System.Security.Claims;
using System.Threading.Tasks;
using BackendServiceStarter.Models.Requests.Auth;
using BackendServiceStarter.Services.Auth;
using BackendServiceStarter.Services.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BackendServiceStarter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> Auth([FromBody] AuthRequest request)
        {
            ClaimsIdentity identity;
            
            try
            {
                identity = await _authService.GetIdentity(request.Email, request.Password);
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (UserHashVerifyException)
            {
                return Unauthorized();
            }

            return new
            {
                Token = _authService.GenerateToken(identity)
            };
        }
    }
}