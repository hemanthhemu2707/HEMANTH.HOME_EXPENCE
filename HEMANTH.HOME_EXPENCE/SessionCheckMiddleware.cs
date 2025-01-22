public class SessionCheckMiddleware
{
    private readonly RequestDelegate _next;

    public SessionCheckMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value?.ToLower();
        if (path?.Contains("/login/login") == true)
        {
            await _next(httpContext);
            return;
        }
        if (path?.Contains("/login/registeruser") == true)
        {
            await _next(httpContext);
            return;
        }
        if (path?.Contains("/login/forgotpassword") == true)
        {
            await _next(httpContext);
            return;
        }
        var userId = httpContext.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            // If the session has expired, set the session expired message
            httpContext.Session.SetString("SessionExpiredMessage", "Your session has expired. Please log in again.");

            // Redirect to the login page
            httpContext.Response.Redirect("../Login/Login");
            return;
        }

        // Continue processing the request
        await _next(httpContext);
    }
}
