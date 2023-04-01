using Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case LockApiException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ArgumentNullException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case ArgumentException e:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case UnauthorizedAccessException e:
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(error?.Message);
                await response.WriteAsync(result);
            }
        }
    }
}
