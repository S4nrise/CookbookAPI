using CookbookAPI.Abstractions;
using CookbookAPI.Exceptions;
using CookbookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CookbookAPI.Services
{
    public class IngredientsService(IApplicationDbContext dbContext) : IIngredientsService
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

        public IReadOnlyList<Ingredient> GetAllIngredients() => dbContext.Ingredients.AsNoTracking().ToList();

        public Ingredient GetIngredientById(int id) => dbContext.Ingredients.AsNoTracking().FirstOrDefault(x => x.Id == id) ?? throw new IngredientNotFoundException(id);
    }
}
