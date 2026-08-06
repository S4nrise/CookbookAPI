using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IJwtTokenGenerator
    {
        public JwtToken GenerateJwtToken(User user);
        public RefreshToken GetRefreshToken(int userId);
    }
}
