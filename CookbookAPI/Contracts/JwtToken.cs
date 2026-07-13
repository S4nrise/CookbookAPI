namespace CookbookAPI.Contracts
{
    public record JwtTokenVm(int UserId, string Token, DateTime ExpiresAt);
}
