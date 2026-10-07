using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookbookAPI.Database.Configurations
{
    public class RecipeEntityConfigurations : IEntityTypeConfiguration<Recipe>
    {
        public void Configure(EntityTypeBuilder<Recipe> builder)
        {
            builder.HasKey(recipe => recipe.Id);
            builder.Property(recipe => recipe.Description)
                .HasMaxLength(256);
        }
    }
}