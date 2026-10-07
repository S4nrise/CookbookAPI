using CookbookAPI.Models;
using CookbookAPI.Models.Enums;

namespace CookbookAPI.Contracts
{
    public record CreateRecipeDto(string Name, string? Description, List<IngredientInRecipeDto>? IngredientsInRecipeDto);
    public record UpdateRecipeDto(int Id, string? Name, string? Description, List<IngredientInRecipeDto>? IngredientsInRecipeDto);
    public record RateRecipeDto(int UserId, int Value);
    public record RecipeFilterDto(string? SearchTerm, int? Rating, int? UserId,
        RecipeSortBy SortBy = RecipeSortBy.Title,
        bool IsDescending = false);
    public record RecipeVm(int Id, string Name, string? Description, List<IngredientsInRecipeVm> Ingredients, double Rating, int UserId);
}