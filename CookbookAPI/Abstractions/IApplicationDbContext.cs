using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Recipe> Recipes { get; }
        public DbSet<Ingredient> Ingredients { get; }
        public DbSet<IngredientInRecipe> IngredientsInRecipes { get; }
        public DbSet<Rating> Rating { get; }
        public DbSet<User> Users { get; }
        public DbSet<JwtToken> JwtTokens { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }

        public int SaveChanges();
    }
}