namespace CookbookAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Password { get; set; }
        public List<Recipe> Recipes { get; set; } = [];
        public ICollection<Rating> RecipeRating { get; set; } = []; 
    }
}