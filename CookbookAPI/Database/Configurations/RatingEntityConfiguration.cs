using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CookbookAPI.Database.Configurations
{
    public class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(x => new { x.UserId, x.RecipeId });

            builder.Property(x => x.Value)
                .IsRequired();

            builder.HasOne(rating => rating.User)
                .WithMany(user => user.RecipeRating)
                .HasForeignKey(rating => rating.UserId);

            builder.HasOne(rating => rating.Recipe)
                .WithMany(recipe => recipe.Rating)
                .HasForeignKey(rating => rating.RecipeId);
        }
    }
}
