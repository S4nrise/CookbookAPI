using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesService
    {
        public int CreateRecipe(int userId, CreateRecipeDto createRecipeDto);
        public void UpdateRecipe(int userId, UpdateRecipeDto updateRecipeDto);
        public void DeleteRecipe(int userId, int id);
        public RecipeVm GetRecipe(int userId, int id);
        public IReadOnlyList<RecipeVm> GetAllRecipes(int userId, RecipeFilterDto recipeFilterDto);
        public void RateRecipe(int userId, int id, RateRecipeDto rateRecipeDto);
    }
}
