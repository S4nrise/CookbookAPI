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
        public async Task<IActionResult> AddIngredientAsync(string name, CancellationToken cancellationToken)
        {
            var ingredientId = await ingredientsService.CreateIngredientAsync(name.Trim(), cancellationToken);
            return CreatedAtAction("GetIngredientById", new {id= ingredientId }, ingredientId);
        }

        [HttpDelete("/DeleteIngredient/{id}")]
        public async Task<IActionResult> DeleteIngredientAsync(int id, CancellationToken cancellationToken)
        {
            await ingredientsService.DeleteIngredientAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("/AllIngredients")]
        public async Task<IActionResult> GettAllIngredientsAsync(CancellationToken cancellationToken) => Ok(await ingredientsService.GetAllIngredientsAsync(cancellationToken));

        [HttpGet("/GetIngredient/{id}")]
        public async Task<IActionResult> GetIngredientByIdAsync(int id, CancellationToken cancellationToken) => Ok(await ingredientsService.GetIngredientByIdAsync(id, cancellationToken));
    }
}
