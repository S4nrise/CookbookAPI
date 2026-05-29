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
        public int CreateIngredient(string name)
        {
            var ingredient = new Ingredient()
            {
                Name = name
            };

            dbContext.Ingredients.Add(ingredient);
            dbContext.SaveChanges();

            return ingredient.Id;
        }

        public void DeleteIngredient(int id)
        {
            var ingredient = GetIngredientById(id);
            dbContext.Ingredients.Remove(ingredient);
            dbContext.SaveChanges();
        }

        public IReadOnlyList<IngredientVm> GetAllIngredients()
        {
            var ingredients = dbContext.Ingredients.AsNoTracking().ToList();

            return mapper.Map<IReadOnlyList<IngredientVm>>(ingredients);
        }

        public Ingredient GetIngredientById(int id) => dbContext.Ingredients.AsNoTracking().FirstOrDefault(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
    }
}
