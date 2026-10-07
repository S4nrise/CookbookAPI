using CookbookAPI.Models.Enums;

namespace CookbookAPI.Models
{
    public class IngredientInRecipe
    {
        public int IngredientId { get; set; }
        public int RecipeId { get; set; }
        public Ingredient Ingredient { get; set; }
        public Recipe Recipe { get; set; }
        public double Amount { get; set; }
        public Units Units {  get; set; }
    }
}
