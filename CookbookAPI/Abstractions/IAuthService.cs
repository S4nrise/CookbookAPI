using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IAuthService
    {
        Task<LogInResponse> SignUpAsync(SignUpDto createUserDtodto, CancellationToken cancellationToken);
        Task<LogInResponse?> LogInAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken);
        Task<bool> LogOutAsync(int userId, CancellationToken cancellationToken);
        Task<bool> VerifyTokenAsync(int userId, string token, CancellationToken cancellationToken);
        Task<LogInResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
        Task RevokeAsync(string refreshToken, CancellationToken cancellationToken);
    }
}
