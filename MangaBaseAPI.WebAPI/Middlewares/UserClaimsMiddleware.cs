using System.Security.Claims;

namespace MangaBaseAPI.WebAPI.Middlewares
{
    public class UserClaimsMiddleware : IMiddleware
    {
        //private readonly ILogger<UserClaimsMiddleware> _logger;

        //public UserClaimsMiddleware(ILogger<UserClaimsMiddleware> logger)
        //{
        //    _logger = logger;
        //}

        public async Task InvokeAsync(
            HttpContext context,
            RequestDelegate next)
        {
            // Check if the user is authenticated and the token is valid
            if (context.User.Identity?.IsAuthenticated == true) 
            {
                // Get and store user claims
                // User ID
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    context.Items["UserId"] = userId;
                }
            }

            await next(context);
        }
    }
}
