using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tables.Services;

namespace Tables.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDTO userLogin)
        {

            var user = _userService.Authenticate(userLogin);

            if (user != null)
            {
                var token = _userService.GenerateToken(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }
    }
}