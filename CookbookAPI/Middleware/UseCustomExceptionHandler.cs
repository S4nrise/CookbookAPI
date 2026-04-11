using CookbookAPI.Exceptions;

namespace CookbookAPI.Middleware
{
    public class UseCustomExceptionHandler
    {
        private readonly RequestDelegate _next;

        public UseCustomExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (RecipeNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            catch (IngredientNotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
