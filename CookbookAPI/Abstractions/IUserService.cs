using CookbookAPI.Contracts;

namespace CookbookAPI.Abstractions
{
    public interface IUserService
    {
        public int CreateUser(CreateUserDto createUserDto);
        public void DeleteUser(int id, DeleteUserDto deleteUserDto);
    }
}
