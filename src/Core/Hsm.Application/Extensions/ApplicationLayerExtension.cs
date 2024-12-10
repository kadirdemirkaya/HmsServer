using EventFlux;
using FlowValidate;
using Microsoft.Extensions.DependencyInjection;

namespace Hsm.Application.Extensions
{
    public static class ApplicationLayerExtension
    {
        public static IServiceCollection LoadApplicationLayerExtension(this IServiceCollection services)
        {
            services.AddEventBus(AssemblyReference.Assemblies);

            services.FluentVal(AssemblyReference.Assembly);

            return services;
        }
    }
}
