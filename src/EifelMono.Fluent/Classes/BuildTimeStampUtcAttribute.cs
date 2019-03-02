using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace EifelMono.Fluent.Classes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class BuildTimeStampUtcAttribute : Attribute
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
