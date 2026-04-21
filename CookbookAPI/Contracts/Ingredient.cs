using CookbookAPI.Enums;

namespace CookbookAPI.Contracts
{
    public record CreateIngredientDto(string Name);
    public record IngredientVm(string Name);
    public record IngredientInRecipeDto(int IngredientId, double Amount, Units Units);
    public record IngredientsInRecipeVm(string Name, double Amount, Units Units);
}
