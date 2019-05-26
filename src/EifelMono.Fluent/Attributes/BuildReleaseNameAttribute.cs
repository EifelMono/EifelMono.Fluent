using System;

namespace EifelMono.Fluent.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildReleaseNameAttribute : Attribute
    {
        public BuildReleaseNameAttribute(string value)
        {
            ReleaseName = value;
        }

        public string ReleaseName { get; }
    }
}
