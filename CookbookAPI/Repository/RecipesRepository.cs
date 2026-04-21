using AutoMapper;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;

namespace CookbookAPI.Repository
{
    public class RecipesRepository(IIngredientsRepository ingredientsRepository, IMapper mapper) : IRecipesRepository
    {
        private List<Recipe> _recipes = new List<Recipe>();
        public int AddRecipe(CreateRecipeDto createRecipeDto)
        {
            //var recipe = new Recipe()
            //{
            //    Id = GetNextRecipetId(),
            //    Name = createRecipeDto.Name.Trim(),
            //    Description = createRecipeDto.Description.Trim(),
            //    Ingredients = new List<IngredientInRecipe>(),
            //    Rating = new List<int>()
            //};

            var recipe = mapper.Map<Recipe>(createRecipeDto);
            recipe.Id = GetNextRecipetId();

            _recipes.Add(recipe);

            if (createRecipeDto.IngredientsInRecipeDto == null) return recipe.Id;

            foreach (var req in createRecipeDto.IngredientsInRecipeDto)
            {
                var foundIngredient = ingredientsRepository.GetIngredientById(req.IngredientId);

                if (foundIngredient != null)
                {
                    recipe.Ingredients!.Add(new IngredientInRecipe
                    {
                        IngredientId = foundIngredient.Id,
                        //Ingredient = foundIngredient,
                        Amount = req.Amount,
                        Units = req.Units,
                    });
                }
            }

            return recipe.Id;
        }

        public void DeleteRecipe(int id)
        {
            _recipes.Remove(GetRecipeById(id));
        }

        public RecipeVm GetRecipeVmById(int id)
        {
            return mapper.Map<RecipeVm>(GetRecipeById(id));
        }
        private Recipe GetRecipeById(int id)
        {
            return _recipes.FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);
        }

        public IReadOnlyList<RecipeVm> GetRecipes()
        {
            return mapper.Map<IReadOnlyList<RecipeVm>>(_recipes);
        }

        public void RateRecipeById(int id, int rate)
        {
            var recipe = GetRecipeById(id);
            recipe.Rating.Add(rate);
        }
        
        public void UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            var recipe = GetRecipeById(updateRecipeDto.Id);

            recipe.Name = updateRecipeDto.Name?.Trim() ?? recipe.Name;
            recipe.Description = updateRecipeDto.Description?.Trim() ?? recipe.Description;

            if (updateRecipeDto.IngredientsInRecipeDto == null) return;

            var incomingIngredientId = updateRecipeDto.IngredientsInRecipeDto.Select(x => x.IngredientId).ToList();
            recipe.Ingredients.RemoveAll(x => !incomingIngredientId.Contains(x.IngredientId));

            foreach (var req in updateRecipeDto.IngredientsInRecipeDto)
            {
                var existing = recipe.Ingredients.FirstOrDefault(x => x.IngredientId == req.IngredientId);
                if (existing.Amount != null)
                {
                    existing.Amount = req.Amount;
                    existing.Units = req.Units;
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
                            Amount = req.Amount,
                            Units = req.Units,
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