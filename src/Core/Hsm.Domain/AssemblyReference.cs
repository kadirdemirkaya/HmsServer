using System.Reflection;

namespace Hsm.Domain
{
    public static class AssemblyReference
    {
        public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;

        public static readonly Assembly[] Assemblies = new[]
        {
            Assembly,
        };
    }
}
