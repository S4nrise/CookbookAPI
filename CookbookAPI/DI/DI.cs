using CookbookAPI.Abstractions;
using CookbookAPI.Configuration;
using CookbookAPI.Database;
using CookbookAPI.Politics;
using CookbookAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using System.Text.Json.Serialization;

namespace CookbookAPI.DI
{
    public static class DI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddServices();

            return services;
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
            {
                var jwtOptions = configuration
                .GetRequiredSection(nameof(JwtOptions))
                .Get<JwtOptions>()!;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtOptions.Secret))
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var authService =
                        context.HttpContext.RequestServices.GetRequiredService<IAuthService>();

                        var userId = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                        if (userId is null
                        || context.SecurityToken.ValidTo < DateTime.UtcNow
                        || !authService.VerifyToken(int.Parse(userId), context.SecurityToken.UnsafeToString())
                        )
                        {
                            context.Fail("Unauthorized");
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                options.AddPolicy("PostsOwner", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(new PostOwnerRequirement());
                });
            });

            services.AddOptions<JwtOptions>()
                .Bind(configuration.GetRequiredSection(nameof(JwtOptions)))
                .ValidateDataAnnotations()
                .ValidateOnStart();
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();

            services.AddAutoMapper(typeof(Program));

            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

                if (hostEnvironment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging();
                    options.LogTo(Console.WriteLine, LogLevel.Information);
                }
            });

            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IIngredientsService, IngredientsService>();
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddValidatorsFromAssemblyContaining<Program>();

            return services;
        }
    }
}
