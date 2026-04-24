using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recipe>().HasKey(x => x.Id);
            modelBuilder.Entity<Ingredient>().HasKey(x => x.Id);
            modelBuilder.Entity<IngredientInRecipe>().HasKey(x => new { x.IngredientId, x.RercpeId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
