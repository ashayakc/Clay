using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Middlewares
{
    public class ApiOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (!operation.Tags.Any(x => x.Name == "Users"))
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Required = true,
                    Description = "Application access token. <p>Ex: Bearer <app_JWT_token></p>",
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                });
            }
        }
    }
}
