using CookbookAPI.Abstractions;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;

namespace CookbookAPI.Services
{
    public class RecipesRepository : IRecipesRepository
    {
        private List<Recipe> _recipes = new List<Recipe>();
        public int AddRecipe(Recipe recipe)
        {
            _recipes.Add(recipe);

            return recipe.Id;
        }

        public void DeleteRecipe(int id) => _recipes.Remove(GetRecipeById(id));

        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);
        }

        public IReadOnlyList<Recipe> GetRecipes() => _recipes;

        public void RateRecipeById(int id, int rate)
        {
            var recipe = GetRecipeById(id);
            recipe.Rating.Add(rate);
        }
        
        public void UpdateRecipe(int id, Recipe recipe)
        {
            var oldRecipe = _recipes.FirstOrDefault(x => x.Id==id);
            _recipes.Remove(oldRecipe);
            _recipes.Add(recipe);
        }
    }
}