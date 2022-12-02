using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace OnlineShop.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("1.0.0", new OpenApiInfo
            {
                Version = "1.0.0",
                Title = "OnlineShop",
                Description = "Online shop app",
                Contact = new OpenApiContact { Name = "Centric IT Solutions" }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    System.Array.Empty<string>()
                }
            });
        });
        return services;
    }

    public static IApplicationBuilder UseCustomSwaggerUi(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/1.0.0/swagger.json", "OnlineShop");
            c.OAuthClientId("swagger-ui");
            c.OAuthClientSecret("swagger-ui-secret");
            c.OAuthRealm("swagger-ui-realm");
            c.OAuthAppName("Swagger UI");
        });
        return app;
    }
}