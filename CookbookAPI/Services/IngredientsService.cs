using AutoMapper;
using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class IngredientsService(IApplicationDbContext dbContext, IMapper mapper) : IIngredientsService
    {
        public async Task<int> CreateIngredientAsync(string name, CancellationToken cancellationToken)
        {
            var ingredient = new Ingredient()
            {
                Name = name
            };

            dbContext.Ingredients.Add(ingredient);
            await dbContext.SaveChangesAsync();

            return ingredient.Id;
        }

        public async Task DeleteIngredientAsync(int id, CancellationToken cancellationToken)
        {
            var ingredient = await GetIngredientByIdAsync(id, cancellationToken);
            dbContext.Ingredients.Remove(ingredient);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<IngredientVm>> GetAllIngredientsAsync(CancellationToken cancellationToken)
        {
            var ingredients = await dbContext.Ingredients.AsNoTracking().ToListAsync();

            return mapper.Map<IReadOnlyList<IngredientVm>>(ingredients);
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id, CancellationToken cancellationToken)
            => await dbContext.Ingredients.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
    }
}
