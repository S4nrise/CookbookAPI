namespace CookbookAPI.Exceptions
{
    public class RecipeNotFoundException : Exception
    {
        public int Value { get; }
        public RecipeNotFoundException(int recipeId) : base($"Recipe id - {recipeId}, not found.")
        {
            Value = recipeId;
        }
    }
}