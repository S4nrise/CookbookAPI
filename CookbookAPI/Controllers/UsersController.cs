using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController(IAuthService authService, IValidator<SignUpDto> signUpValidator) : BaseController
    {
        [AllowAnonymous]
        [HttpPost("/registration")]
        public ActionResult<LogInResponse> Registration([FromBody] SignUpDto signUpUserDto)
        {
            var validationResult = signUpValidator.Validate(signUpUserDto);
            if (validationResult.IsValid)
            {
                var token = authService.SignUp(signUpUserDto);

                return Ok(token);
            }
            return BadRequest();
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
