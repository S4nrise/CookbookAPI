using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IIngredientsService
    {
        public Task<int> CreateIngredientAsync(string name, CancellationToken cancellationToken);
        public Task DeleteIngredientAsync(int id, CancellationToken cancellationToken);
        public Task<IReadOnlyList<IngredientVm>> GetAllIngredientsAsync(CancellationToken cancellationToken);
        public Task<Ingredient> GetIngredientByIdAsync(int id, CancellationToken cancellationToken);
    }
}