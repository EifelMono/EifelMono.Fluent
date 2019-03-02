using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EifelMono.Fluent.Classes
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
