using CookbookAPI.Models;

namespace CookbookAPI.Abstractions
{
    public interface IIngredientsRepository
    {
        public int AddIngredient(string name);
        public void DeleteIngredient(int id);
        public IReadOnlyList<Ingredient> GetAllIngredients();
        public Ingredient GetIngredientById(int id);
    }
}