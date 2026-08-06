using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IAuthService authService) : BaseController
    {
        [AllowAnonymous]
        [HttpPost("/registration")]
        public ActionResult<LogInResponse> Registration([FromBody] CreateUserDto createUserDto)
        {
            var token = authService.SignUp(createUserDto);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public ActionResult<LogInResponse> Login(LoginUserDto loginUserDto)
        {
            var result = authService.LogIn(loginUserDto);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("/logout")]
        public IActionResult Logout([FromBody] int userId)
        {
            var result = authService.LogOut(userId);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpPost("/refresh")]
        public ActionResult<LogInResponse> Refresh([FromBody] string refreshToken)
        {
            var result = authService.Refresh(refreshToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("/revoke")]
        public ActionResult Revoke([FromBody] string refreshToken)
        {
            authService.Revoke(refreshToken);

            return NoContent();
        }
    }
}
