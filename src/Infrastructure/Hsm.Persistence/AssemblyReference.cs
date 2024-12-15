using Hsm.Persistence.Extensions;
using System.Reflection;

namespace Hsm.Persistence
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

        public static readonly Assembly[] Assemblies = new[]
        {
            Assembly,
            typeof(PersistenceLayerExtension).Assembly,
        };
    }
}
