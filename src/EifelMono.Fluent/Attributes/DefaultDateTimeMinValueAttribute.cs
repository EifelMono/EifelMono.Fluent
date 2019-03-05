using System;
using System.ComponentModel;

namespace EifelMono.Fluent.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DefaultDateTimeMinValueAttribute : DefaultValueAttribute
    {

        public DefaultDateTimeMinValueAttribute() : base(DateTime.MinValue)
        {
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return true;
            if ((DateTime)obj == DateTime.MinValue)
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
