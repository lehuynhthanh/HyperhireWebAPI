using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace HyperhireWebAPI.Swagger;

public class ConfigureSwaggerOptions : AbstractConfigureSwaggerOptions
{
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : base(provider, "Hyperhire API")
    {
    }
}