using CookbookAPI.Abstractions;
using CookbookAPI.Dto;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;

namespace CookbookAPI.Repository
{
    public class RecipesRepository(IIngredientsRepository ingredientsRepository) : IRecipesRepository
    {
        private List<Recipe> _recipes = new List<Recipe>();
        public int AddRecipe(string name, string? description, List<IngredientInRecipeDto> ingredientRequirements)
        {
            var recipe = new Recipe()
            {
                Id = GetNextRecipetId(),
                Name = name.Trim(),
                Description = description?.Trim(),
                Ingredients = new List<IngredientInRecipe>(),
                Rating = new List<int>()
            };

            _recipes.Add(recipe);

            if (ingredientRequirements == null) return recipe.Id;

            foreach (var req in ingredientRequirements)
            {
                var foundIngredient = ingredientsRepository.GetIngredientById(req.IngredientId);

                if (foundIngredient != null)
                {
                    recipe.Ingredients.Add(new IngredientInRecipe
                    {
                        IngredientId = foundIngredient.Id,
                        //Ingredient = foundIngredient,
                        Amount = req.Amount
                    });
                }
            }

            return recipe.Id;
        }

        public void DeleteRecipe(int id)
        {
            _recipes.Remove(GetRecipeById(id));
        }

        public Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);
        }

        public IReadOnlyList<Recipe> GetRecipes()
        {
            return _recipes;
        }

        public void RateRecipeById(int id, int rate)
        {
            var recipe = GetRecipeById(id);
            recipe.Rating.Add(rate);
        }

        public void UpdateRecipe(int recipeId, string? name, string? description, List<IngredientInRecipeDto>? ingredientRequirements)
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
                            //Ingredient = foundIngredient,
                            Amount = req.Amount
                        });
                    }
                }
            }
        }
        private int GetNextRecipetId()
        {
            return _recipes.Count == 0 ? 0 : _recipes.Max(x => x.Id) + 1;
        }
    }
}