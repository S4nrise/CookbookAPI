using CookbookAPI.Abstractions;
using CookbookAPI.Models;

namespace CookbookAPI.Repository
{
    public class IngredientsRepository : IIngredientsRepository
    {
        private List<Ingredient> _ingredients = new();
        public void AddIngredient(string name)
        {
            var ingredient = new Ingredient() { Id = GetMaxIngredientId() + 1, Name = name }; //как лучше избавится от +1?
            _ingredients.Add(ingredient);
        }

        public void DeleteIngredient(int id)
        {
            _ingredients.Remove(_ingredients.First(x => x.Id == id));
        }

        public IReadOnlyList<Ingredient> GetAllIngredients() => _ingredients;

        public Ingredient GetIngredientById(int id) => _ingredients.First(x => x.Id == id) ?? throw new KeyNotFoundException($"Ingredient id - {id}, not found"); 

        private int GetMaxIngredientId()
        {
            return _ingredients.Count == 0 ? 0 : _ingredients.Max(x=>x.Id); 
        }  

        
    }
}
