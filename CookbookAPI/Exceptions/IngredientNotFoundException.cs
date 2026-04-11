namespace CookbookAPI.Exceptions
{
    public class IngredientNotFoundException : Exception
    {
        public int Id { get; }
        public IngredientNotFoundException(int id) : base ($"Ingredient id - {id}, not found.")
        {
            Id = id;
        }
    }
}
