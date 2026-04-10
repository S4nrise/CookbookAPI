using CookbookAPI.Dto;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesRepository
    {
        public int AddRecipe(string name, string? description, List<IngredientInRecipeDto> ingredientRequirements);

        public void DeleteRecipe(int id);

        public void UpdateRecipe(int recipeId, string? name, string? description, List<IngredientInRecipeDto>? ingredientRequirements);

        public Recipe GetRecipeById(int id);

        public IReadOnlyList<Recipe> GetRecipes();

        public void RateRecipeById(int id, int rate);
    }
}
