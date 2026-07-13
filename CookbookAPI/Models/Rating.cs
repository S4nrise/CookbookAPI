namespace CookbookAPI.Models
{
    public class Rating
    {
        public int RecipeId { get; set; }
        public int UserId { get; set; }
        public Recipe Recipe { get; set; } = null!;
        public User User { get; set; } = null!;
        public int Value { get; set; }
    }
}