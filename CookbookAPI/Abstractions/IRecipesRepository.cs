using CookbookAPI.Models;
using CookbookAPI.Models.Dto;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesRepository
    {
        public void AddRecipe(string name, string? description, List<IngredientRequirement> ingredientRequirements);

        public void DeleteRecipe(int id);

        public void UpdateRecipe(int recipeId, string? name, string? description, List<IngredientRequirement>? ingredientRequirements);

        public Recipe GetRecipeById(int id);

        public IReadOnlyList<Recipe> GetRecipes();

        public void RateRecipeById(int recipeId, int rateNumber);
    }
}
