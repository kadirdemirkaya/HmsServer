using Hsm.Api.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Hsm.Api.Extensions
{
    public static class ApiLayerExtension
    {
        public static IServiceCollection LoadApiLayerExtension(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder LoadApiLayerApplicationExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseAuthorization();

            return app;
        }
    }
}
