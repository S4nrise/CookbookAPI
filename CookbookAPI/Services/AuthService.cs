using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Models;
using CookbookAPI.Utils;

namespace CookbookAPI.Services
{
    public class AuthService(
        IApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator) : IAuthService
    {
        public JwtTokenVm? LogIn(LoginUserDto loginUserDto)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Name == loginUserDto.Name);

            if (user is null)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(user.Password, loginUserDto.Password))
                return null;

            var token = UpdateToken(user);
            dbContext.SaveChanges();

            return token?.ToJwtTokenVm();
        }

        public bool LogOut(int userId)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Id == userId);
            if (user is null)
            {
                return false;
            }

            var token = dbContext.JwtTokens.FirstOrDefault(token => token.UserId == userId);
            if (token is null)
            {
                return false;
            }

            dbContext.JwtTokens.Remove(token);
            dbContext.SaveChanges();

            return true;
        }

        public JwtTokenVm SignUp(CreateUserDto createUserDtodto)
        {
            var user = new User
            {
                Name = createUserDtodto.Name,
                Password = PasswordHasher.HashPassword(createUserDtodto.Password),
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var token = UpdateToken(user);

            dbContext.SaveChanges();

            return token.ToJwtTokenVm();
        }

        public bool VerifyToken(int userId, string token)
        {
            var jwtToken = dbContext.JwtTokens.FirstOrDefault(token => token.UserId == userId);
            if (jwtToken is null) 
                return false;

            return jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;
        }

        private JwtToken UpdateToken(User user)
        {
            var token = jwtTokenGenerator.GenerateJwtToken(user);
            var oldToken = dbContext.JwtTokens.FirstOrDefault(t => t.UserId == user.Id);

            if (oldToken is not null)
            {
                dbContext.JwtTokens.Remove(oldToken);
            }
            dbContext.JwtTokens.Add(token);

            return token;
        }
    }
}
