using Microsoft.Extensions.Caching.Memory;

namespace FOT.WebApi.Middlewares;

/// <summary>
/// Replay protection middleware (idempotency key)
/// </summary>
/// <param name="next"></param>
/// <param name="cache"></param>
public class ReplayProtectionMiddleware(RequestDelegate next, IMemoryCache cache)
{
    private const string _headerName = "Replay-Nonce";

    /// <inheritdoc />
    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;

        if (method != HttpMethods.Post &&
            method != HttpMethods.Put &&
            method != HttpMethods.Patch)
        {
            await next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(_headerName, out var nonce))
        {
            Console.WriteLine("Replay-Nonce header missing");
            await next(context);
            return;
        }

        if (cache.TryGetValue(nonce.ToString(), out _))
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync("Request already received");
            return;
        }

        cache.Set(nonce.ToString(), true, TimeSpan.FromHours(1));
        await next(context);
    }
}
