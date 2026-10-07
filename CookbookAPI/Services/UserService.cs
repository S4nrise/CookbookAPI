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
        public async Task<int> CreateUserAsync(SignUpDto createUserDto, CancellationToken cancellationToken)
        {
            var user = mapper.Map<User>(createUserDto);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            return user.Id;
        }

        public async Task DeleteUserAsync(int id, DeleteUserDto deleteUserDto, CancellationToken cancellationToken)
        {
            var deletedUser = await dbContext.Users
                .Where(user => user.Id == id)
                .ExecuteDeleteAsync();
            if (deletedUser == 0) throw new UserNotFoundException(id);
        }

        private User GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(user => user.Id == id) ?? throw new UserNotFoundException(id);
        }
    }
}