using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController(IRecipesRepository recipesRepository, IIngredientsRepository ingredientsRepository) : ControllerBase
    {
        [HttpGet("/Recipes")]
        public IActionResult GetRecipes()
        {
            return Ok(recipesRepository.GetRecipes());
        }

        [HttpGet("/GetRecipeById/{id}")]
        public IActionResult GetRecipeById(int id)
        {
            return Ok(recipesRepository.GetRecipeVmById(id));
        }

        [HttpPost("/AddRecipe")]
        public IActionResult AddRecipe(CreateRecipeDto createRecipeDto)
        {
            var recipeId = recipesRepository.AddRecipe(createRecipeDto);
            return CreatedAtAction("GetRecipeById", new { id = recipeId }, recipeId);
        }

        [HttpPut("/UpdateRecipe/{id}")]
        public IActionResult UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            recipesRepository.UpdateRecipe(updateRecipeDto);
            return NoContent();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            recipesRepository.DeleteRecipe(id);
            return NoContent();
        }

        [HttpPost("/RateRecipe/{id}")]
        public IActionResult RateRecipeById(int id, int rate)
        {
            recipesRepository.RateRecipeById(id, rate);
            return NoContent();//Подумать, мб что-то стоит возвращать. 
        }
    }
}