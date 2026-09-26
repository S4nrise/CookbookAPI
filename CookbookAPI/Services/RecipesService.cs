using AutoMapper;
using AutoMapper.QueryableExtensions;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;
using CookbookAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class RecipesService(IApplicationDbContext dbContext, IMapper mapper) : IRecipesService
    {
        public async Task<int> CreateRecipeAsync(int userId, CreateRecipeDto createRecipeDto, CancellationToken cancellationToken)
        {
            var recipe = mapper.Map<Recipe>(createRecipeDto);
            recipe.UserId = userId;

            await dbContext.Recipes.AddAsync(recipe);
            await dbContext.SaveChangesAsync();

            return recipe.Id;
        }

        public async Task DeleteRecipeAsync(int userId, int id, CancellationToken cancellationToken)
        {
            var deletedRecipe = await dbContext.Recipes
                .Where(x => x.Id == id && x.UserId == userId)
                .ExecuteDeleteAsync();
            if (deletedRecipe == 0) throw new RecipeNotFoundException(id);
        }

        public async Task<IReadOnlyList<RecipeVm>> GetAllRecipesAsync(int userId, RecipeFilterDto recipeFilterDto, CancellationToken cancellationToken)
        {
            IQueryable<Recipe> query = dbContext.Recipes.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(recipeFilterDto.SearchTerm))
            {
                var search = recipeFilterDto.SearchTerm.Trim().ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(recipeFilterDto.SearchTerm));
            }

            if (recipeFilterDto.Rating.HasValue)
            {
                query = query.Where(x => x.Rating.Any() && x.Rating.Average(r => r.Value) >= recipeFilterDto.Rating.Value);
            }

            if (recipeFilterDto.UserId is not null)
            {
                var author = recipeFilterDto.UserId;
                query = query.Where(x => x.UserId == author);
            }
            query = (recipeFilterDto.SortBy, recipeFilterDto.IsDescending) switch
            {
                (RecipeSortBy.Title, false) => query.OrderBy(x => x.Name),
                (RecipeSortBy.Title, true) => query.OrderByDescending(x => x.Name),

                (RecipeSortBy.Rating, false) => query.OrderBy(x => x.Rating.Any() ? x.Rating.Average(r => r.Value) : 0),
                (RecipeSortBy.Rating, true) => query.OrderByDescending(x => x.Rating.Any() ? x.Rating.Average(r => r.Value) : 0),
                
                _ => query.OrderByDescending(r => r.Id)
            };

            return await query.ProjectTo<RecipeVm>(mapper.ConfigurationProvider).ToListAsync();

            /*var recipe = dbContext.Recipes
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Rating)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .ToList();
            return mapper.Map<IReadOnlyList<RecipeVm>>(recipe);*/
        }

        public async Task<RecipeVm> GetRecipeAsync(int userId, int id, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes
                .AsNoTracking()
                .Include(x => x.Rating)
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id) ?? throw new RecipeNotFoundException(id);

            return mapper.Map<RecipeVm>(recipe);
        }

        public async Task RateRecipeAsync(int userId, int id, RateRecipeDto rateRecipeDto, CancellationToken cancellationToken)
        {
            var recipe = await dbContext.Recipes.FirstOrDefaultAsync(x => x.Id == id) ?? throw new RecipeNotFoundException(id);

            recipe.Rating.Add(new Rating
            {
                RecipeId = recipe.Id,
                UserId = rateRecipeDto.UserId,
                Value = rateRecipeDto.Value
            });

            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateRecipeAsync(int userId, UpdateRecipeDto updateRecipeDto, CancellationToken cancellationToken)
        {
            var recipe = await GetRecipeByIdWithTrackingAsync(updateRecipeDto.Id, userId, cancellationToken);

            recipe.Name = updateRecipeDto.Name?.Trim() ?? recipe.Name;
            recipe.Description = updateRecipeDto.Description?.Trim() ?? recipe.Description;

            if (updateRecipeDto.IngredientsInRecipeDto == null)
            {
                await dbContext.SaveChangesAsync();
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
                    var foundIngredient = GetIngredientByIdAsync(req.IngredientId, cancellationToken);

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

            await dbContext.SaveChangesAsync();
        }

        private async Task<Recipe> GetRecipeByIdWithTrackingAsync(int id, int userId, CancellationToken cancellationToken)
        {
            return await dbContext.Recipes
                .Include(x => x.Ingredients)
                .ThenInclude(x => x.Ingredient)
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId) ?? throw new RecipeNotFoundException(id);
        }

        private async Task<Ingredient> GetIngredientByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await dbContext.Ingredients.FirstOrDefaultAsync(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
        }
    }
}