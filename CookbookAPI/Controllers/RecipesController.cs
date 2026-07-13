using CookbookAPI.Abstractions;
using CookbookAPI.Contracts;
using CookbookAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookbookAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecipesController(IRecipesService recipeService) : BaseController
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
        public IActionResult AddRecipe([FromBody] CreateRecipeDto createRecipeDto)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            var recipeId = recipeService.CreateRecipe(userId.Value, createRecipeDto);

            return CreatedAtAction("GetRecipeById", new { id = recipeId }, recipeId);
        }

        [HttpPut("/UpdateRecipe/{id}")]
        public IActionResult UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            recipeService.UpdateRecipe(updateRecipeDto);
            return NoContent();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        [Authorize(Policy = "PostsOwner")]
        public IActionResult DeleteRecipe(int id)
        {
            recipeService.DeleteRecipe(id);
            return NoContent();
        }

        [HttpPost("/RateRecipe/{id}")]
        public IActionResult RateRecipeById(int id, RateRecipeDto rateRecipeDto)
        {
            recipeService.RateRecipe(id, rateRecipeDto);
            return Ok();
        }
    }
}