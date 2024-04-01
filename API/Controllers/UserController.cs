using Application.Contracts;
using Application.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser user;
        public UserController(IUser user)
        {
            this.user = user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> LogInUser(LoginDTO loginDTO)
        {
           //Test middleware
           // if (loginDTO is not null) throw new UnauthorizedAccessException("Invalid Token");

            var result = await user.LoginUserAsync(loginDTO);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResponse>> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            var result = await user.RegisterUserAsync(registerUserDTO);
            return Ok(result);
        }

        [HttpPost("changeRole")]
        [Authorize]
        public async Task<ActionResult<ChangeUserRoleResponse>> ChangeUserRole(ChangeRoleDTO changeRoleDTO)
        {
            var result = await user.ChangeUserRole(changeRoleDTO);
            return Ok(result);
        }
    }
}
