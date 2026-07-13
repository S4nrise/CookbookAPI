using CookbookAPI.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController(
        IIngredientsService ingredientsService) : BaseController
    {

        [HttpPost("/AddIngredient")]
        public IActionResult AddIngredient(string name)
        {
            var ingredientId = ingredientsService.CreateIngredient(name.Trim());
            return CreatedAtAction("GetIngredientById", new {id= ingredientId }, ingredientId);
        }

        [HttpDelete("/DeleteIngredient/{id}")]
        public IActionResult DeleteIngredient(int id)
        {
            ingredientsService.DeleteIngredient(id);
            return NoContent();
        }

        [HttpGet("/AllIngredients")]
        public IActionResult GettAllIngredients() => Ok(ingredientsService.GetAllIngredients());

        [HttpGet("/GetIngredient/{id}")]
        public IActionResult GetIngredientById(int id) => Ok(ingredientsService.GetIngredientById(id));
    }
}
