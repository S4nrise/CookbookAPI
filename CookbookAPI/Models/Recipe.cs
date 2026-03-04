namespace CookbookAPI.Models
{
    public class Recipe
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        List<Ingredient> Ingredients { get; set; }
        double[] Rating { get; set; }
        User User { get; set; }
    }
}
