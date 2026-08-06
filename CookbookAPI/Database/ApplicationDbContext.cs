using CookbookAPI.Abstractions;
using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Database
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<JwtToken> JwtTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Program).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}