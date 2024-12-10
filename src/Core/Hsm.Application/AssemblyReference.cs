using Hsm.Application.Extensions;
using System.Reflection;

namespace Hsm.Application
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

        public static readonly Assembly[] Assemblies = new[]
        {
            Assembly,
            typeof(ApplicationLayerExtension).Assembly,
        };
    }
}
