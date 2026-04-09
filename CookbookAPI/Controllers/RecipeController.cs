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

        [HttpGet("/GetRecipeById")]
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

        [HttpPut("/UpdateRecipe")]
        public IActionResult UpdateRecipe(int recipeId,string? name, string? description, List<IngredientRequirement>? ingredientRequirements)
        {
            recipesRepository.UpdateRecipe(recipeId, name, description, ingredientRequirements);
            return Ok();
        }

        [HttpDelete("/DeleteRecipe")]
        public IActionResult DeleteRecipe(int recipeId)
        {
            recipesRepository.DeleteRecipe(recipeId);
            return Ok();
        }

        [HttpPost("/RateRecipe")]
        public IActionResult RateRecipeById(int recipeId, int rateNumber)
        {
            recipesRepository.RateRecipeById(recipeId, rateNumber);
            return Ok();
        }
    }
}