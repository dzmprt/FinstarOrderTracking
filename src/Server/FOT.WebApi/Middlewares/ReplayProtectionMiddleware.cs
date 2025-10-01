using Microsoft.Extensions.Caching.Memory;

namespace FOT.WebApi.Middlewares;

public class ReplayProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private const string HeaderName = "Replay-Nonce";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="next"></param>
    /// <param name="cache"></param>
    public ReplayProtectionMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;

        if (method != HttpMethods.Post &&
            method != HttpMethods.Put &&
            method != HttpMethods.Patch)
        {
            await _next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue(HeaderName, out var nonce))
        {
            Console.WriteLine("Replay-Nonce header missing");
            await _next(context);
            return;
        }

        if (_cache.TryGetValue(nonce.ToString(), out _))
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync("Request already received");
            return;
        }

        _cache.Set(nonce.ToString(), true, TimeSpan.FromHours(1));
        await _next(context);
    }
}