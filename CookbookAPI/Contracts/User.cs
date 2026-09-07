namespace CookbookAPI.Contracts
{
    public record SignUpDto(string Name, string Password);
    public record LoginUserDto(string Name, string Password);
    public record DeleteUserDto(string Password);
    //public record SignUpDto(string Name, string Password);
}
