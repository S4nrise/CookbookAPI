using AutoMapper;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Models;

namespace CookbookAPI.Services
{
    public class RecipesService(IRecipesRepository recipesRepository,
        IIngredientsRepository ingredientsRepository, IMapper mapper) : IRecipesService
    {
        public int CreateRecipe(CreateRecipeDto createRecipeDto)
        {
            var recipe = mapper.Map<Recipe>(createRecipeDto);

            if (createRecipeDto.IngredientsInRecipeDto == null)
            {
                return recipesRepository.AddRecipe(recipe);
            }

            foreach (var req in createRecipeDto.IngredientsInRecipeDto)
            {
                var foundIngredient = ingredientsRepository.GetIngredientById(req.Id);

                if (foundIngredient != null)
                {
                    recipe.Ingredients!.Add(new IngredientInRecipe
                    {
                        IngredientId = foundIngredient.Id,
                        Amount = req.Amount,
                        Units = req.Units,
                    });
                }
            }

            return recipesRepository.AddRecipe(recipe);
        }

        public void DeleteRecipe(int id) => recipesRepository.DeleteRecipe(id);

        public IReadOnlyList<RecipeVm> GetAllRecipes()
        {
            return mapper.Map<IReadOnlyList<RecipeVm>>(recipesRepository.GetRecipes());
        }

        public RecipeVm GetRecipe(int id)
        {
            return mapper.Map<RecipeVm>(recipesRepository.GetRecipeById(id));
        }

        public void RateRecipe(int id, int rate)
        {
            recipesRepository.RateRecipeById(id, rate);
        }

        public void UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            var recipe = recipesRepository.GetRecipeById(updateRecipeDto.Id);

            recipe.Name = updateRecipeDto.Name?.Trim() ?? recipe.Name;
            recipe.Description = updateRecipeDto.Description?.Trim() ?? recipe.Description;

            if (updateRecipeDto.IngredientsInRecipeDto == null)
            {
                recipesRepository.UpdateRecipe(updateRecipeDto.Id, recipe);
                return;
            }

            var incomingIngredientId = updateRecipeDto.IngredientsInRecipeDto.Select(x => x.Id).ToList();
            recipe.Ingredients.RemoveAll(x => !incomingIngredientId.Contains(x.IngredientId));

            foreach (var req in updateRecipeDto.IngredientsInRecipeDto)
            {
                var existing = recipe.Ingredients.FirstOrDefault(x => x.IngredientId == req.Id);
                if (existing.Amount != null)
                {
                    existing.Amount = req.Amount;
                    existing.Units = req.Units;
                }
                else
                {
                    var foundIngredient = ingredientsRepository.GetIngredientById(req.Id);

                    if (foundIngredient != null)
                    {
                        recipe.Ingredients.Add(new IngredientInRecipe
                        {
                            IngredientId = foundIngredient.Id,
                            Amount = req.Amount,
                            Units = req.Units,
                        });
                    }
                }
            }

            recipesRepository.UpdateRecipe(updateRecipeDto.Id, recipe);
        }
    }
}