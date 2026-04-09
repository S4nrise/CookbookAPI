using CookbookAPI.Abstractions;
using CookbookAPI.Models;
using CookbookAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IngredientsController(
        IIngredientsRepository ingredientsRepository) : ControllerBase
    {

        [HttpPost("/AddIngredient")]
        public IActionResult AddIngredient(string name)
        {
            ingredientsRepository.AddIngredient(name.Trim());
            return Ok();
        }

        [HttpDelete("/DeleteIngredient/{id}")]
        public IActionResult DeleteIngredient(int id)
        {
            ingredientsRepository.DeleteIngredient(id);
            return Ok();
        }

        [HttpGet("/AllIngredients")]
        public IActionResult GettAllIngredients() => Ok(ingredientsRepository.GetAllIngredients());

        [HttpGet("/GetIngredient/{id}")]
        public IActionResult GetIngredientById(int id) => Ok(ingredientsRepository.GetIngredientById(id));
    }
}
