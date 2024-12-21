using EventFlux;
using FlowValidate;
using Hsm.Application.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Hsm.Application.Extensions
{
    public static class ApplicationLayerExtension
    {
        public static IServiceCollection LoadApplicationLayerExtension(this IServiceCollection services)
        {
            services.AddEventBus(AssemblyReference.Assemblies);

            services.FluentVal(AssemblyReference.Assembly);

            services.AddScoped(typeof(GenericNotFoundFilter<>));

            return services;
        }
    }
}
