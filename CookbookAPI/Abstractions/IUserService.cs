using CookbookAPI.Contracts;

namespace CookbookAPI.Abstractions
{
    public interface IUserService
    {
        public int CreateUser(SignUpDto createUserDto);
        public void DeleteUser(int id, DeleteUserDto deleteUserDto);
    }
}
