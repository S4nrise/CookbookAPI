using CookbookAPI.Abstractions;
using CookbookAPI.Models;
using CookbookAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipeController(IRecipesRepository recipesRepository) : ControllerBase
    {
        [HttpGet("/Recipes")]
        public IActionResult GetRecipes()
        {
            return Ok(recipesRepository.GetRecipes());
        }

        [HttpGet("/GetRecipeById/{id}")]
        public IActionResult GetRecipeById(int id)
        {
            var recipe = recipesRepository.GetRecipeById(id);
            return Ok(recipe);
        }

        [HttpPost("/AddRecipe")]
        public IActionResult AddRecipe(string name, string? description, List<IngredientRequirement>? ingredientRequirements )
        {
            recipesRepository.AddRecipe(name, description, ingredientRequirements);
            return Ok();
        }

        [HttpPut("/UpdateRecipe/{id}")]
        public IActionResult UpdateRecipe(int id,string? name, string? description, List<IngredientRequirement>? ingredientRequirements)
        {
            recipesRepository.UpdateRecipe(id, name, description, ingredientRequirements);
            return Ok();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        public IActionResult DeleteRecipe(int id)
        {
            recipesRepository.DeleteRecipe(id);
            return Ok();
        }

        [HttpPost("/RateRecipe/{id}")]
        public IActionResult RateRecipeById(int id, int rateNumber)
        {
            recipesRepository.RateRecipeById(id, rateNumber);
            return Ok();
        }
    }
}