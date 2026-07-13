using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesService
    {
        public int CreateRecipe(int userId, CreateRecipeDto createRecipeDto);
        public void UpdateRecipe(UpdateRecipeDto updateRecipeDto);
        public void DeleteRecipe(int id);
        public RecipeVm GetRecipe(int id);
        public IReadOnlyList<RecipeVm> GetAllRecipes();
        public void RateRecipe(int id, RateRecipeDto rateRecipeDto);
    }
}
