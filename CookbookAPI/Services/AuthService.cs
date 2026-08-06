using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Models;
using CookbookAPI.Utils;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class AuthService(
        IApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator) : IAuthService
    {
        public LogInResponse? LogIn(LoginUserDto loginUserDto)
        {
            var user = dbContext.Users.FirstOrDefault(user => user.Name == loginUserDto.Name);

            if (user is null)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(user.Password, loginUserDto.Password))
                return null;

            var (jwt, refresh) = UpdateToken(user);
            dbContext.SaveChanges();

            return CreateResponse(jwt, refresh);
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

        public LogInResponse SignUp(CreateUserDto createUserDtodto)
        {
            var user = new User
            {
                Name = createUserDtodto.Name,
                Password = PasswordHasher.HashPassword(createUserDtodto.Password),
            };

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var (jwt, refresh) = UpdateToken(user);

            dbContext.SaveChanges();

            return CreateResponse(jwt, refresh);
        }

        public bool VerifyToken(int userId, string token)
        {
            var jwtToken = dbContext.JwtTokens.FirstOrDefault(token => token.UserId == userId);
            if (jwtToken is null)
                return false;

            return jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;
        }

        public LogInResponse? Refresh(string refreshToken)
        {
            var existingRefreshToken = dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefault(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

            if (existingRefreshToken is null)
                return null;

            var (jwt, refresh) = UpdateToken(existingRefreshToken.User);

            dbContext.SaveChanges();

            return CreateResponse(jwt, refresh);
        }

        public void Revoke(string refreshToken)
        {
            var existingRefreshToken = dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefault(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

            if (existingRefreshToken is null)
                return;

            dbContext.RefreshTokens.Remove(existingRefreshToken);
            dbContext.SaveChanges();
        }

        private (JwtToken Jwt, RefreshToken Refresh) UpdateToken(User user)
        {
            var token = jwtTokenGenerator.GenerateJwtToken(user);
            var oldToken = dbContext.JwtTokens.FirstOrDefault(t => t.UserId == user.Id);

            if (oldToken is not null)
            {
                dbContext.JwtTokens.Remove(oldToken);
            }
            dbContext.JwtTokens.Add(token);

            var refreshToken = jwtTokenGenerator.GetRefreshToken(user.Id);
            dbContext.RefreshTokens.Add(refreshToken);

            return (token, refreshToken);
        }

        private static LogInResponse CreateResponse(JwtToken jwtToken, RefreshToken refreshToken)
            => new(jwtToken.UserId, jwtToken.Token, refreshToken.Token);
    }
}
