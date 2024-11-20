using HyperhireWebAPI.Swagger.Operations;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;

namespace HyperhireWebAPI.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration["ApplicationSettings:IsUseSwagger"], out bool isUseSwagger);
        if (!isUseSwagger)
            return services;
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Description = "Type into the textbox: Bearer {your JWT token}."
            });
            //c.AddSecurityDefinition("X-ApiKey", new OpenApiSecurityScheme
            //{
            //    Name = "X-ApiKey",
            //    In = ParameterLocation.Header,
            //    Type = SecuritySchemeType.ApiKey,
            //    Description = "Type into the textbox: {your ClientName}:{your ClientSecret}",
            //});

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                      },
                        new List<string>()
                      }
                    });
            //c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            //      {
            //        {
            //          new OpenApiSecurityScheme
            //          {
            //            Reference = new OpenApiReference
            //            {
            //                Type = ReferenceType.SecurityScheme,
            //                Id = "X-ApiKey",

            //            }
            //          },
            //            new List<string>()
            //          }
            //        });
            //c.OperationFilter<RequiredXApiKeyHeaderFilter>();
            c.OperationFilter<AddSummaryFilter>();
        });
        return services;
    }
    public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app, IConfiguration configuration)
    {
        bool.TryParse(configuration["ApplicationSettings:IsUseSwagger"], out bool isUseSwagger);
        if (!isUseSwagger)
            return app;
        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName.ToUpperInvariant());
            }
        });

        return app;
    }
}