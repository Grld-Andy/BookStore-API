using System.Net;
using Microsoft.AspNetCore.Mvc.Localization;

namespace BookStore.Middlewares;

public class GlobalErrorHandler
{

    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public GlobalErrorHandler(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}