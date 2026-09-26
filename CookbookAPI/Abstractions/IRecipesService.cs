using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IRecipesService
    {
        public Task<int> CreateRecipeAsync(int userId, CreateRecipeDto createRecipeDto, CancellationToken cancellationToken);
        public Task UpdateRecipeAsync(int userId, UpdateRecipeDto updateRecipeDto, CancellationToken cancellationToken);
        public Task DeleteRecipeAsync(int userId, int id, CancellationToken cancellationToken);
        public Task<RecipeVm> GetRecipeAsync(int userId, int id, CancellationToken cancellationToken);
        public Task<IReadOnlyList<RecipeVm>> GetAllRecipesAsync(int userId, RecipeFilterDto recipeFilterDto, CancellationToken cancellationToken);
        public Task RateRecipeAsync(int userId, int id, RateRecipeDto rateRecipeDto, CancellationToken cancellationToken);
    }
}
