using E_Commerce.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

namespace E_Commerce.Extensions
{
    public static class WebApiServiceExtentions
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            var frontUrl = configuration.GetSection("URLS")["FrontUrl"];
            services.AddCors(options=>
            options.AddPolicy("CorsPolicy", builder=>
            {
                builder.AllowAnyHeader()
                .WithOrigins(frontUrl);
            }));
            services.AddSwaggerGen(option =>
             {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            }); services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
            });
            return services;
        }
    }
}
