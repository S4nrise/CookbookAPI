using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesRepository
    {
        public int AddRecipe(CreateRecipeDto createRecipeDto);

        public void DeleteRecipe(int id);

        public void UpdateRecipe(UpdateRecipeDto updateRecipeDto);

        public RecipeVm GetRecipeVmById(int id);

        public IReadOnlyList<RecipeVm> GetRecipes();

        public void RateRecipeById(int id, int rate);
    }
}
