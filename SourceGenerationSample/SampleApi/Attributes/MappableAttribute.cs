using System;

namespace SampleApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MappableAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Property)]
    public class MappableIgnoreAttribute : Attribute
    {

    }
}
