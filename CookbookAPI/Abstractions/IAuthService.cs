using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IAuthService
    {
        JwtTokenVm SignUp(CreateUserDto createUserDtodto);
        JwtTokenVm? LogIn(LoginUserDto loginUserDto);
        bool LogOut(int userId);
        bool VerifyToken(int userId, string token);
    }
}
