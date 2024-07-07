namespace EntityFrameworkService.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    public class CookieSignatureMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CookieSignatureMiddleware> _logger;

        public CookieSignatureMiddleware(RequestDelegate next, ILogger<CookieSignatureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("acctoken", out var jwt))
            {
                if (context.Request.Headers.TryGetValue("Authorization", out var authorization))
                {
                    context.Request.Headers["Authorization"] = $"{authorization} {jwt}";
                    _logger.LogDebug("Cookie jwt signature appended to the authorization header");
                }
                else
                {
                    context.Request.Headers.Add("Authorization", $"Bearer {jwt}");
                }
            }

            await _next(context);
        }
    }
}
