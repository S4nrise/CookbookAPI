using AutoMapper;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class RecipesService(IApplicationDbContext dbContext, IMapper mapper) : IRecipesService
    {
        public int CreateRecipe(CreateRecipeDto createRecipeDto)
        {
            var recipe = mapper.Map<Recipe>(createRecipeDto);

            if (createRecipeDto.IngredientsInRecipeDto != null)
            {
                foreach (var req in createRecipeDto.IngredientsInRecipeDto)
                {
                    recipe.Ingredients!.Add(new IngredientInRecipe
                    {
                        IngredientId = GetIngredientById(req.Id).Id,//Жестко подумать
                        Amount = req.Amount,
                        Units = req.Units,
                    });
                }
            }

            dbContext.Recipes.Add(recipe);
            dbContext.SaveChanges();

            return recipe.Id;
        }

        public void DeleteRecipe(int id)
        {
            var deletedRecipe = dbContext.Recipes
                .Where(x => x.Id == id)
                .ExecuteDelete();
            if (deletedRecipe == 0) throw new RecipeNotFoundException(id);
        }

        public IReadOnlyList<RecipeVm> GetAllRecipes()
        {
            var recipe = dbContext.Recipes.AsNoTracking().Include(x => x.Ingredients).ThenInclude(x=>x.Ingredient).ToList();
            return mapper.Map<IReadOnlyList<RecipeVm>>(recipe);
        }

        public RecipeVm GetRecipe(int id)
        {
            var recipe = dbContext.Recipes
                .AsNoTracking()
                .Include(x=>x.Ingredients)
                .ThenInclude(x=>x.Ingredient)
                .FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);

            return mapper.Map<RecipeVm>(recipe);
        }

        private Recipe GetRecipeById(int id)
        {
            return dbContext.Recipes
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);
        }

        private Ingredient GetIngredientById(int id)
        {
            return dbContext.Ingredients.FirstOrDefault(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
        }

        public void RateRecipe(int id, int rate)
        {
            var recipe = GetRecipeById(id);
            recipe.Rating.Add(rate);

            dbContext.SaveChanges();
        }

        public void UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            var recipe = GetRecipeById(updateRecipeDto.Id);

            recipe.Name = updateRecipeDto.Name?.Trim() ?? recipe.Name;
            recipe.Description = updateRecipeDto.Description?.Trim() ?? recipe.Description;

            if (updateRecipeDto.IngredientsInRecipeDto == null)
            {
                //recipesRepository.UpdateRecipe(updateRecipeDto.Id, recipe);
                dbContext.SaveChanges();
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
                    var foundIngredient = GetIngredientById(req.Id);

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

            //recipesRepository.UpdateRecipe(updateRecipeDto.Id, recipe);
            dbContext.SaveChanges();
        }
    }
}