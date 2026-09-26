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
        public async Task<ActionResult<LogInResponse>> RegistrationAsync([FromBody] SignUpDto signUpUserDto, CancellationToken cancellationToken)
        {
            var validationResult = signUpValidator.Validate(signUpUserDto);
            if (validationResult.IsValid)
            {
                var token = await authService.SignUpAsync(signUpUserDto, cancellationToken);

                return Ok(token);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public async Task<ActionResult<LogInResponse>> LoginAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            var result = await authService.LogInAsync(loginUserDto, cancellationToken);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("/logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] int userId, CancellationToken cancellationToken)
        {
            var result = await authService.LogOutAsync(userId, cancellationToken);
            if (!result) return NotFound();

            return Ok(result);
        }

        [HttpPost("/refresh")]
        public async Task<ActionResult<LogInResponse>> RefreshAsync([FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            var result = await authService.RefreshAsync(refreshToken, cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("/revoke")]
        public async Task<ActionResult> RevokeAsync([FromBody] string refreshToken, CancellationToken cancellationToken)
        {
            await authService.RevokeAsync(refreshToken, cancellationToken);

            return NoContent();
        }
    }
}
