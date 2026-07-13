using CookbookAPI.Models.Enums;

namespace CookbookAPI.Contracts
{
    public record CreateIngredientDto(string Name);
    public record IngredientVm(int Id, string Name);
    public record IngredientInRecipeDto(int IngredientId, double Amount, Units Units);
    public record IngredientsInRecipeVm(int IngredientId, string Name, double Amount, Units Units);
}