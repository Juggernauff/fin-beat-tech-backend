namespace FinBeat.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(
             ILogger<ErrorHandlingMiddleware> logger,
             RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
