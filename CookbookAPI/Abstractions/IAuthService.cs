using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IAuthService
    {
        LogInResponse SignUp(CreateUserDto createUserDtodto);
        LogInResponse? LogIn(LoginUserDto loginUserDto);
        bool LogOut(int userId);
        bool VerifyToken(int userId, string token);
        LogInResponse? Refresh(string refreshToken);
        void Revoke(string refreshToken);
    }
}
