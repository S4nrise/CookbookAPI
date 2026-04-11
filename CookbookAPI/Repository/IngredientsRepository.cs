using CookbookAPI.Abstractions;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;

namespace CookbookAPI.Repository
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private List<Ingredient> _ingredients = new();
        public int AddIngredient(string name)
        {
            var ingredient = new Ingredient() { Id = GetNextIngredientId(), Name = name };
            _ingredients.Add(ingredient);

            return ingredient.Id;
        }

        public void DeleteIngredient(int id)
        {
            var ingredient = GetIngredientById(id);
            _ingredients.Remove(ingredient);
        }

        public IReadOnlyList<Ingredient> GetAllIngredients() => _ingredients;

        public Ingredient GetIngredientById(int id) => _ingredients.FirstOrDefault(x => x.Id == id) ?? throw new IngredientNotFoundException(id);

        private int GetNextIngredientId()
        {
            return _ingredients.Count == 0 ? 0 : _ingredients.Max(x => x.Id) + 1;
        }
    }
}
