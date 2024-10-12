public class IpRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IpRestrictionMiddleware> _logger;

    public IpRestrictionMiddleware(RequestDelegate next, ILogger<IpRestrictionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var _allowedIps = new[] { "35.208.237.59", "35.208.95.118", "::1" };
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();
        _logger.LogInformation($"remoteIP:{remoteIp}");
        if (!_allowedIps.Contains(remoteIp))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }
}
