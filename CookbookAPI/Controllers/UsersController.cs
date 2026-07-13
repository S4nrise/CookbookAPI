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
        public ActionResult<JwtTokenVm> Registration([FromBody] CreateUserDto createUserDto)
        {
            var token= authService.SignUp(createUserDto);
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("/login")]
        public ActionResult<JwtTokenVm> Login(LoginUserDto loginUserDto)
        {
            var result = authService.LogIn(loginUserDto);
            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost("/logout/")]
        public IActionResult Logout([FromBody] int userId)
        {
            var result = authService.LogOut(userId);
            if(!result) return NotFound();

            return Ok(result);
        }
    }
}
