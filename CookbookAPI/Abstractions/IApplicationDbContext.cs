using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Recipe> Recipes { get; }
        public DbSet<Ingredient> Ingredients { get; }
        public DbSet<IngredientInRecipe> IngredientsInRecipes { get; }

        public int SaveChanges();
    }
}