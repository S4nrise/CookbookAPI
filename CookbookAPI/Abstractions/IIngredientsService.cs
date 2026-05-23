using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IIngredientsService
    {
        public int CreateIngredient(string name);
        public void DeleteIngredient(int id);
        public IReadOnlyList<Ingredient> GetAllIngredients();
        public Ingredient GetIngredientById(int id);
    }
}