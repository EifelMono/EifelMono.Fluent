using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Classes
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
