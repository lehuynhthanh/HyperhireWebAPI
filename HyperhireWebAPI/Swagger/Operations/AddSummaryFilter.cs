using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HyperhireWebAPI.Swagger.Operations;

public class AddSummaryFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        operation.Summary = $"{context.MethodInfo.GetBaseDefinition().Name} - {operation.Summary}";
    }
}
