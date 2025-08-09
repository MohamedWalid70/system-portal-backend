using Microsoft.OpenApi.Models;

namespace SystemPortal.Api.Extensions
{
    internal static class ServiceExtensions
    {
        internal static IServiceCollection AddSwaggerGenWithAuth(this IServiceCollection services)
        {

            services.AddSwaggerGen( o =>

                {
                    o.CustomSchemaIds(id => id?.FullName?.Replace('+', '-'));

                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "JWT Authentication",
                        Description = "Enter your JWT token here",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                    };

                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                }
                            },
                            []
                        }
                    };

                    o.AddSecurityRequirement(securityRequirement);
                });

            return services;
        }
    }
}
