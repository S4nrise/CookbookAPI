using CookbookAPI.Abstractions;
using CookbookAPI.Models;
using CookbookAPI.Models.Dto;

namespace CookbookAPI.Repository
{
    public class RecipesRepository(IIngredientsRepository ingredientsRepository) : IRecipesRepository
    {
        private List<Recipe> _recipes = new List<Recipe>();
        public void AddRecipe(string name, string? description, List<IngredientRequirement> ingredientRequirements)
        {
            var recipe = new Recipe()
            {
                Id = _recipes.Count,
                Name = name.Trim(),
                Description = description?.Trim(),
                Ingredients = new List<IngredientInRecipe>()
            };

            _recipes.Add(recipe);

            if (ingredientRequirements == null) return;

            foreach (var req in ingredientRequirements)
            {
                var foundIngredient = ingredientsRepository.GetIngredientById(req.IngredientId);

                if (foundIngredient != null)
                {
                    recipe.Ingredients.Add(new IngredientInRecipe
                    {
                        IngredientId = foundIngredient.Id,
                        Ingredient = foundIngredient,
                        Amount = req.Amount
                    });
                }
            }
        }

        public void DeleteRecipe(int id)
        {
            _recipes.Remove(GetRecipeById(id));
        }

        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(x => x.Id == id) ?? throw new KeyNotFoundException($"recipe id - {id}, not found");//Сделать кастомные исключения
        }

        public IReadOnlyList<Recipe> GetRecipes()
        {
            return _recipes;
        }

        public void RateRecipeById(int recipeId, int rateNumber)
        {
            var recipe = GetRecipeById(recipeId);
            recipe.Rating.Add(rateNumber);
        }

        public void UpdateRecipe(int recipeId, string? name, string? description, List<IngredientRequirement>? ingredientRequirements)
        {
            var recipe = GetRecipeById(recipeId);

            recipe.Name = name?.Trim() ?? recipe.Name;
            recipe.Description = description?.Trim() ?? recipe.Description;

            if (ingredientRequirements == null) return;

            var incomingIngredientId = ingredientRequirements.Select(x => x.IngredientId).ToList();
            recipe.Ingredients.RemoveAll(x => !incomingIngredientId.Contains(x.IngredientId));

            foreach (var req in ingredientRequirements)
            {
                var existing = recipe.Ingredients.FirstOrDefault(x => x.IngredientId == req.IngredientId);
                if (existing.Amount != null)
                {
                    existing.Amount = req.Amount;
                }
                else
                {
                    var foundIngredient = ingredientsRepository.GetIngredientById(req.IngredientId);

                    if (foundIngredient != null)
                    {
                        recipe.Ingredients.Add(new IngredientInRecipe
                        {
                            IngredientId = foundIngredient.Id,
                            Ingredient = foundIngredient,
                            Amount = req.Amount
                        });
                    }
                }
            }
        }
    }
}