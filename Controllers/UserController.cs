using System.Collections.Generic;
using System.Threading.Tasks;
using BackendServiceStarter.Models;
using BackendServiceStarter.Models.Requests.User;
using BackendServiceStarter.Services.Models;
using BackendServiceStarter.Services.Models.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendServiceStarter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator")]
        public Task<List<User>> Index()
        {
            return _userRepository.Find();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<User>> Show(int id)
        {
            var user = await _userRepository.FindByPk(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Create([FromBody] CreateUserRequest request)
        {
            if (request.IsPasswordNotValid())
            {
                return BadRequest();
            }

            var isNotAdministrator = !HttpContext.User.Identity.IsAuthenticated ||
                                     !HttpContext.User.IsInRole("Administrator");

            if (isNotAdministrator && request.IsRoleNotDefault())
            {
                return Forbid();
            }

            var user = new User
            {
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            };

            await _userRepository.Create(user);

            return await _userRepository.FindByEmail(user.Email);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<User>> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _userRepository.FindByPk(id);

            if (user == null)
            {
                return NotFound();
            }

            var isNotCurrentUser = HttpContext.User.Identity.Name != user.Id.ToString();
            var isNotAdministrator = !HttpContext.User.IsInRole("Administrator");

            if (isNotCurrentUser && isNotAdministrator)
            {
                return Forbid();
            }

            if (request.IsEmailExist())
            {
                user.Email = request.Email;
            }

            if (request.IsPasswordExist())
            {
                if (request.IsPasswordNotValid())
                {
                    return BadRequest();
                }

                user.Password = request.Password;
            }

            if (request.Role.HasValue)
            {
                if (isNotAdministrator)
                {
                    return Forbid();
                }

                user.Role = request.Role.Value;
            }

            await _userRepository.Update(user);

            return user;
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Destroy(int id)
        {
            var isNotCurrentUser = HttpContext.User.Identity.Name != id.ToString();
            var isNotAdministrator = !HttpContext.User.IsInRole("Administrator");

            if (isNotCurrentUser && isNotAdministrator)
            {
                return Forbid();
            }

            try
            {
                await _userRepository.DeleteByPk(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}