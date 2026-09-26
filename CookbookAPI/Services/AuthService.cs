using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Models;
using CookbookAPI.Utils;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class AuthService(
        IApplicationDbContext dbContext,
        IJwtTokenGenerator jwtTokenGenerator) : IAuthService
    {
        public async Task<LogInResponse?> LogInAsync(LoginUserDto loginUserDto, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Name == loginUserDto.Name);

            if (user is null)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(user.Password, loginUserDto.Password))
                return null;

            var (jwt, refresh) = await UpdateTokenAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync();

            return CreateResponse(jwt, refresh);
        }

        public async Task<bool> LogOutAsync(int userId, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
            if (user is null)
            {
                return false;
            }

            var token = await dbContext.JwtTokens.FirstOrDefaultAsync(token => token.UserId == userId);
            if (token is null)
            {
                return false;
            }

            dbContext.JwtTokens.Remove(token);
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<LogInResponse> SignUpAsync(SignUpDto createUserDtodto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Name = createUserDtodto.Name,
                Password = PasswordHasher.HashPassword(createUserDtodto.Password),
            };

            await dbContext.Users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var (jwt, refresh) = await UpdateTokenAsync(user, cancellationToken);

            await dbContext.SaveChangesAsync();

            return CreateResponse(jwt, refresh);
        }

        public async Task<bool> VerifyTokenAsync(int userId, string token, CancellationToken cancellationToken)
        {
            var jwtToken = await dbContext.JwtTokens.FirstOrDefaultAsync(token => token.UserId == userId);
            if (jwtToken is null)
                return false;

            return jwtToken.Token == token && jwtToken.ExpiresAt > DateTime.UtcNow;
        }

        public async Task<LogInResponse?> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var existingRefreshToken = await dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

            if (existingRefreshToken is null)
                return null;

            var (jwt, refresh) = await UpdateTokenAsync(existingRefreshToken.User, cancellationToken);

            await dbContext.SaveChangesAsync();

            return CreateResponse(jwt, refresh);
        }

        public async Task RevokeAsync(string refreshToken, CancellationToken cancellationToken)
        {
            var existingRefreshToken = await dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.ExpiresAt > DateTime.UtcNow);

            if (existingRefreshToken is null)
                return;

            dbContext.RefreshTokens.Remove(existingRefreshToken);
            await dbContext.SaveChangesAsync();
        }

        private async Task<(JwtToken Jwt, RefreshToken Refresh)> UpdateTokenAsync(User user, CancellationToken cancellationToken)
        {
            var token = jwtTokenGenerator.GenerateJwtToken(user);
            var oldToken = await dbContext.JwtTokens.FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (oldToken is not null)
            {
                dbContext.JwtTokens.Remove(oldToken);
            }
            await dbContext.JwtTokens.AddAsync(token);

            var refreshToken = jwtTokenGenerator.GetRefreshToken(user.Id);
            await dbContext.RefreshTokens.AddAsync(refreshToken);

            return (token, refreshToken);
        }

        private static LogInResponse CreateResponse(JwtToken jwtToken, RefreshToken refreshToken)
            => new(jwtToken.UserId, jwtToken.Token, refreshToken.Token);
    }
}
