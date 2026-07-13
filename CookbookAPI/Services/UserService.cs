using AutoMapper;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class UserService(IApplicationDbContext dbContext, IMapper mapper) : IUserService
    {
        public int CreateUser(CreateUserDto createUserDto)
        {
            var user = mapper.Map<User>(createUserDto);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return user.Id;
        }

        public void DeleteUser(int id, DeleteUserDto deleteUserDto)
        {
            var deletedUser = dbContext.Users
                .Where(user => user.Id == id)
                .ExecuteDelete();
            if (deletedUser == 0) throw new UserNotFoundException(id);
        }

        private User GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(user => user.Id == id) ?? throw new UserNotFoundException(id);
        }
    }
}