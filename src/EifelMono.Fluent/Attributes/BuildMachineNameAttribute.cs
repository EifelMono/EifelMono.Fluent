using System;

namespace EifelMono.Fluent.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildMachineNameAttribute : Attribute
    {
        public BuildMachineNameAttribute(string value)
        {
            MachineName = value;
        }

        public string MachineName { get; }
    }
}
