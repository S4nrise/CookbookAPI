using CookbookAPI.Contracts;

namespace CookbookAPI.Models
{
    public partial class JwtToken
    {
        public JwtTokenVm ToJwtTokenVm()
        {
            return new JwtTokenVm(UserId, Token, ExpiresAt);
        }
    }
}
