namespace ModelMapper
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PropertyMappingAttribute : Attribute
    {
        public string MappedName { get; }

        public PropertyMappingAttribute(string mappedName)
        {
            MappedName = mappedName;
        }
    }
}
