using System;
using System.Globalization;

namespace EifelMono.Fluent.Attributes
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class BuildTimeStampUtcAttribute : Attribute
    {
        public BuildTimeStampUtcAttribute(string value)
        {
            try
            {
                DateTime = DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None);
            }
            catch
            {
                DateTime = DateTime.MinValue;
            }
        }

        public DateTime DateTime { get; }
    }
}
