namespace Hsm.Api.Extensions
{
    public static class ApiLayerExtension
    {
        public static IServiceCollection LoadApiLayerExtension(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            return services;
        }

        public static IApplicationBuilder LoadApiLayerApplicationExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            return app;
        }
    }
}
