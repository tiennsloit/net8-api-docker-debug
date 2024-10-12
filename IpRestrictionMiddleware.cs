public class IpRestrictionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _allowedIps;

    public IpRestrictionMiddleware(RequestDelegate next, string[] allowedIps)
    {
        _next = next;
        _allowedIps = allowedIps;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress?.ToString();

        if (!_allowedIps.Contains(remoteIp))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("Forbidden");
            return;
        }

        await _next(context);
    }
}
