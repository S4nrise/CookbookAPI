using CookbookAPI.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController(
        IIngredientsService ingredientsRepository) : ControllerBase
    {

        [HttpPost("/AddIngredient")]
        public IActionResult AddIngredient(string name)
        {
            var ingredientId = ingredientsRepository.CreateIngredient(name.Trim());
            return CreatedAtAction("GetIngredientById", new {id= ingredientId }, ingredientId);
        }

        [HttpDelete("/DeleteIngredient/{id}")]
        public IActionResult DeleteIngredient(int id)
        {
            ingredientsRepository.DeleteIngredient(id);
            return NoContent();
        }

        [HttpGet("/AllIngredients")]
        public IActionResult GettAllIngredients() => Ok(ingredientsRepository.GetAllIngredients());

        [HttpGet("/GetIngredient/{id}")]
        public IActionResult GetIngredientById(int id) => Ok(ingredientsRepository.GetIngredientById(id));
    }
}
