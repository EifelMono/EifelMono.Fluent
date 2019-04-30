using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Extensions
{
    public static class ListExtensions
    {
        public static T AddValueReturnValue<T>(this List<T> thisValue, T addValue)
        {
            thisValue.Add(addValue);
            return addValue;
        }
        public static List<T> AddValueReturnList<T>(this List<T> thisValue, T addValue)
        {
            thisValue.Add(addValue);
            return thisValue;
        }
        public static T RemoveValueReturnValue<T>(this List<T> thisValue, T removeValue)
        {
            if (thisValue.Contains(removeValue))
                thisValue.Remove(removeValue);
            return removeValue;
        }
        public static List<T> RemoveValueReturnList<T>(this List<T> thisValue, T removeValue)
        {
            if (thisValue.Contains(removeValue))
                thisValue.Remove(removeValue);
            return thisValue;
        }
    }
}
