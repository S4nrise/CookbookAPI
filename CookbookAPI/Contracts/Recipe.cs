using CookbookAPI.Models;

namespace CookbookAPI.Contracts
{
    public record CreateRecipeDto(string Name, string? Description, List<IngredientInRecipeDto>? IngredientsInRecipeDto);
    public record UpdateRecipeDto(int Id, string? Name, string? Description, List<IngredientInRecipeDto>? IngredientsInRecipeDto);
    public record RecipeVm(int Id, string Name, string? Description, List<IngredientInRecipe> Ingredients);
}
