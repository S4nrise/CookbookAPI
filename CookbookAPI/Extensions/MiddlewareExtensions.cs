using CookbookAPI.Middleware;

namespace CookbookAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UseCustomExceptionHandler>();
        }
    }
}
