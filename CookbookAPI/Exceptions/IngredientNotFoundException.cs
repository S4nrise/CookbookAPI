namespace CookbookAPI.Exceptions
{
    public class IngredientNotFoundException : Exception
    {
        public int Value { get; }
        public IngredientNotFoundException(int id) : base ($"Ingredient id - {id}, not found.")
        {
            Value = id;
        }
    }
}
