using CookbookAPI.Abstractions;
using CookbookAPI.Database;
using CookbookAPI.Services;
using Microsoft.EntityFrameworkCore;
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

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Program));
            services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IIngredientsService, IngredientsService>();

            services.AddScoped<IRecipesRepository, RecipesRepository>();
            services.AddScoped<IRecipesService, RecipesService>();

            return services;
        }
    }
}
