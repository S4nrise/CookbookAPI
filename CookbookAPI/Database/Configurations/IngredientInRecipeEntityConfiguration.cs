using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookbookAPI.Database.Configurations
{
    public class IngredientInRecipeEntityConfiguration : IEntityTypeConfiguration<IngredientInRecipe>
    {
        public void Configure(EntityTypeBuilder<IngredientInRecipe> builder)
        {
            builder.HasKey(x => new { x.IngredientId, x.RecipeId });

            builder.HasOne(x => x.Recipe).WithMany(x => x.Ingredients).HasForeignKey(x => x.RecipeId);
            builder.HasOne(x => x.Ingredient).WithMany(x => x.Recipes).HasForeignKey(x => x.IngredientId);
        }
    }
}
