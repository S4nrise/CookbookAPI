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
        public int CreateRecipe(int userId, CreateRecipeDto createRecipeDto)
        {
            var recipe = mapper.Map<Recipe>(createRecipeDto);
            recipe.UserId = userId;

            dbContext.Recipes.Add(recipe);
            dbContext.SaveChanges();

            return recipe.Id;
        }

        public void DeleteRecipe(int userId, int id)
        {
            var deletedRecipe = dbContext.Recipes
                .Where(x => x.Id == id && x.UserId == userId)
                .ExecuteDelete();
            if (deletedRecipe == 0) throw new RecipeNotFoundException(id);
        }

        public IReadOnlyList<RecipeVm> GetAllRecipes(int userId)//сортировка по рейтингу и фильтр по наименованию
        {
            var recipe = dbContext.Recipes
                .AsNoTracking()
                .Where(x=>x.UserId == userId)
                .Include(x=>x.Rating)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .ToList();
            return mapper.Map<IReadOnlyList<RecipeVm>>(recipe);
        }

        public RecipeVm GetRecipe(int userId, int id)
        {
            var recipe = dbContext.Recipes
                .AsNoTracking()
                .Include(x => x.Rating)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);

            return mapper.Map<RecipeVm>(recipe);
        }

        public void RateRecipe(int userId, int id, RateRecipeDto rateRecipeDto)
        {
            var recipe = dbContext.Recipes.FirstOrDefault(x => x.Id == id) ?? throw new RecipeNotFoundException(id);

            recipe.Rating.Add(new Rating
            {
                RecipeId = recipe.Id,
                UserId = rateRecipeDto.UserId,
                Value = rateRecipeDto.Value
            });

            dbContext.SaveChanges();
        }

        public void UpdateRecipe(int userId, UpdateRecipeDto updateRecipeDto)
        {
            var recipe = GetRecipeByIdWithTracking(updateRecipeDto.Id, userId);

            recipe.Name = updateRecipeDto.Name?.Trim() ?? recipe.Name;
            recipe.Description = updateRecipeDto.Description?.Trim() ?? recipe.Description;

            if (updateRecipeDto.IngredientsInRecipeDto == null)
            {
                dbContext.SaveChanges();
                return;
            }

            var incomingIngredientId = updateRecipeDto.IngredientsInRecipeDto.Select(x => x.IngredientId).ToList();
            recipe.Ingredients.RemoveAll(x => !incomingIngredientId.Contains(x.IngredientId));

            foreach (var req in updateRecipeDto.IngredientsInRecipeDto)
            {
                var existing = recipe.Ingredients.FirstOrDefault(x => x.IngredientId == req.IngredientId);
                if (existing is not null)
                {
                    existing.Amount = req.Amount;
                    existing.Units = req.Units;
                }
                else
                {
                    var foundIngredient = GetIngredientById(req.IngredientId);

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

            dbContext.SaveChanges();
        }

        private Recipe GetRecipeByIdWithTracking(int id, int userId)
        {
            return dbContext.Recipes
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefault(x => x.Id == id && x.UserId == userId) ?? throw new RecipeNotFoundException(id);
        }
        
        private Ingredient GetIngredientById(int id)
        {
            return dbContext.Ingredients.FirstOrDefault(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
        }
    }
}