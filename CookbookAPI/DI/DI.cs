using CookbookAPI.Abstractions;
using CookbookAPI.Database;
using CookbookAPI.Services;
using Microsoft.EntityFrameworkCore;

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
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(Program));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IIngredientsRepository, IngredientsRepository>();

            services.AddScoped<IRecipesRepository, RecipesRepository>();
            services.AddScoped<IRecipesService, RecipesService>();

            return services;
        }
    }
}
