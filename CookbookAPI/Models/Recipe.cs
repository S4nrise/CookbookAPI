namespace CookbookAPI.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<IngredientInRecipe> Ingredients { get; set; } = [];
        public ICollection<Rating> Rating { get; set; } = [];
    }
}