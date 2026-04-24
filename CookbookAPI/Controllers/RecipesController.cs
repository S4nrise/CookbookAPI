using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController(IRecipesService recipeService) : ControllerBase
    {
        [HttpGet("/Recipes")]
        public IActionResult GetRecipes()
        {
            return Ok(recipeService.GetAllRecipes());
        }

        [HttpGet("/GetRecipeById/{id}")]
        public IActionResult GetRecipeById(int id)
        {
            return Ok(recipeService.GetRecipe(id));
        }

        [HttpPost("/AddRecipe")]
        public IActionResult AddRecipe(CreateRecipeDto createRecipeDto)
        {
            var recipeId = recipeService.CreateRecipe(createRecipeDto);
            return CreatedAtAction("GetRecipeById", new { id = recipeId }, recipeId);
        }

        [HttpPut("/UpdateRecipe/{id}")]
        public IActionResult UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            recipeService.UpdateRecipe(updateRecipeDto);
            return NoContent();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            recipeService.DeleteRecipe(id);
            return NoContent();
        }

        [HttpPost("/RateRecipe/{id}")]
        public IActionResult RateRecipeById(int id, int rate)
        {
            recipeService.RateRecipe(id, rate);
            return NoContent();//Подумать, мб что-то стоит возвращать. 
        }
    }
}