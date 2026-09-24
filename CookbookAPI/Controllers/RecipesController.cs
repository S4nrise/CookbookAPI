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
        [Authorize]
        public IActionResult GetRecipes([FromQuery] RecipeFilterDto recipeFilterDto)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(recipeService.GetAllRecipes(userId.Value, recipeFilterDto));
        }

        [HttpGet("/GetRecipeById/{id}")]
        [Authorize]
        public IActionResult GetRecipeById(int id)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(recipeService.GetRecipe(userId.Value, id));
        }

        [HttpPost("/AddRecipe")]
        [Authorize]
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
        [Authorize]
        public IActionResult UpdateRecipe(UpdateRecipeDto updateRecipeDto)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            recipeService.UpdateRecipe(userId.Value, updateRecipeDto);
            return NoContent();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        [Authorize]
        public IActionResult DeleteRecipe(int id)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            recipeService.DeleteRecipe(userId.Value, id);
            
            return NoContent();
        }

        [HttpPost("/RateRecipe/{id}")]
        [Authorize]
        public IActionResult RateRecipeById(int id, RateRecipeDto rateRecipeDto)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            recipeService.RateRecipe(userId.Value, id, rateRecipeDto);
            
            return Ok();
        }
    }
}