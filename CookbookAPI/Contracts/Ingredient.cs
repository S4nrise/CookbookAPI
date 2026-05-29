using CookbookAPI.Models.Enums;

namespace CookbookAPI.Contracts
{
    public record CreateIngredientDto(string Name);
    public record IngredientVm(int Id, string Name);
    public record IngredientInRecipeDto(int Id, double Amount, Units Units);
    public record IngredientsInRecipeVm(string Name, double Amount, Units Units);
}
