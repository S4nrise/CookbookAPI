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
        public async Task<IActionResult> GetRecipesAsync([FromQuery] RecipeFilterDto recipeFilterDto, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(await recipeService.GetAllRecipesAsync(userId.Value, recipeFilterDto, cancellationToken));
        }

        [HttpGet("/GetRecipeById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetRecipeByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }

            return Ok(await recipeService.GetRecipeAsync(userId.Value, id, cancellationToken));
        }

        [HttpPost("/AddRecipe")]
        [Authorize]
        public async Task<IActionResult> AddRecipeAsync([FromBody] CreateRecipeDto createRecipeDto, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            var recipeId = await recipeService.CreateRecipeAsync(userId.Value, createRecipeDto, cancellationToken);

            return CreatedAtAction("GetRecipeById", new { id = recipeId }, recipeId);
        }

        [HttpPut("/UpdateRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateRecipeAsync(UpdateRecipeDto updateRecipeDto, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            await recipeService.UpdateRecipeAsync(userId.Value, updateRecipeDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("/DeleteRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteRecipeAsync(int id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            await recipeService.DeleteRecipeAsync(userId.Value, id, cancellationToken);

            return NoContent();
        }

        [HttpPost("/RateRecipe/{id}")]
        [Authorize]
        public async Task<IActionResult> RateRecipeByIdAsync(int id, RateRecipeDto rateRecipeDto, CancellationToken cancellationToken)
        {
            var userId = HttpContext.ExtractUserIdFromClaims();
            if (userId == null)
            {
                return Unauthorized();
            }
            await recipeService.RateRecipeAsync(userId.Value, id, rateRecipeDto, cancellationToken);

            return Ok();
        }
    }
}