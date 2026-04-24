using CookbookAPI.Abstractions;
using CookbookAPI.Database;
using CookbookAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.DI
{
    public static class DI
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwagger();
            services.AddInfrastructure(configuration);
            services.AddServices();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddAutoMapper(typeof(Program));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IIngredientsRepository, IngredientsRepository>();
            services.AddSingleton<IRecipesRepository, RecipesRepository>();
            services.AddSingleton<IRecipesService, RecipesService>();

            return services;
        }
    }
}
