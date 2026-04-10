namespace CookbookAPI.Models
{
    public class IngredientInRecipe
    {
        public int IngredientId { get; set; }
        public int RercpeId { get; set; }
        //public Ingredient Ingredient { get; set; }
        public double Amount { get; set; }
        public string Unit {  get; set; }
    }
}
