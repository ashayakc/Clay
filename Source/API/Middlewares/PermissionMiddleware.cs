using API.Constants;

namespace API.Middlewares
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.HasValue && path.Value.Contains("/audits"))
            {
                var isAdminClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type.Equals(Jwt.IS_ADMIN));
                if (isAdminClaim == null || (isAdminClaim != null && isAdminClaim.Value == "0"))
                {
                    httpContext.Response.StatusCode = 403;
                    await httpContext.Response.WriteAsync("Insufficient permission");
                    return;
                }
            }
            await _next(httpContext);
        }
    }
}
