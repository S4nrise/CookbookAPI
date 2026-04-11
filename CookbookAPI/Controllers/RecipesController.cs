using CookbookAPI.Abstractions;
using CookbookAPI.Dto;
using CookbookAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController(IRecipesRepository recipesRepository) : ControllerBase
    {
        [HttpGet("/Recipes")]
        public IActionResult GetRecipes()
        {
            return Ok(recipesRepository.GetRecipes());
        }

        [HttpGet("/GetRecipeById/{id}")]
        public IActionResult GetRecipeById(int id)
        {
            return Ok(recipesRepository.GetRecipeById(id));
        }

        [HttpPost("/AddRecipe")]
        public IActionResult AddRecipe(string name, string? description, List<IngredientInRecipeDto>? ingredientRequirements )
        {
            var recipeId = recipesRepository.AddRecipe(name, description, ingredientRequirements);
            return CreatedAtAction("GetRecipeById", new { id = recipeId }, recipeId);
        }

        [HttpPut("/UpdateRecipe/{id}")]
        public IActionResult UpdateRecipe(int id,string? name, string? description, List<IngredientInRecipeDto>? ingredientRequirements)
        {
            recipesRepository.UpdateRecipe(id, name, description, ingredientRequirements);
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