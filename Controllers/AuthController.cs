using System.Security.Claims;
using System.Threading.Tasks;
using BackendServiceStarter.Models.Requests.Auth;
using BackendServiceStarter.Services.Auth;
using BackendServiceStarter.Services.Auth.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BackendServiceStarter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<ActionResult<object>> Auth([FromBody] AuthRequest request)
        {
            ClaimsIdentity identity;
            
            _logger.LogInformation($"User authentication: {request.Email}");
            
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