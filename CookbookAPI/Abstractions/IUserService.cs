using CookbookAPI.Contracts;

namespace CookbookAPI.Abstractions
{
    public interface IUserService
    {
        public Task<int> CreateUserAsync(SignUpDto createUserDto, CancellationToken cancellationToken);
        public Task DeleteUserAsync(int id, DeleteUserDto deleteUserDto, CancellationToken cancellationToken);
    }
}
