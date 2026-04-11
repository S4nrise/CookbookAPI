namespace CookbookAPI.Exceptions
{
    public class RecipeNotFoundException : Exception
    {
        public int Id { get; }
        public RecipeNotFoundException(int recipeId) : base($"Recipe id - {recipeId}, not found.")
        {
            Id = recipeId;
        }
    }
}