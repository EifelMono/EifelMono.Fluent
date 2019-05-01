using System.Collections.Generic;

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

        public static IEnumerable<T> AddValuesReturnValues<T>(this List<T> thisValue, IEnumerable<T> addValues)
        {
            thisValue.AddRange(addValues);
            return addValues;
        }

        public static IEnumerable<T> AddValuesReturnValues<T>(this List<T> thisValue, params T[] addValues)
            => thisValue.AddValuesReturnValues(addValues as IEnumerable<T>);

        public static IEnumerable<T> AddValuesReturnList<T>(this List<T> thisValue, IEnumerable<T> addValues)
        {
            thisValue.AddRange(addValues);
            return thisValue;
        }

        public static IEnumerable<T> AddValuesReturnList<T>(this List<T> thisValue, params T[] addValues)
            => thisValue.AddValuesReturnList(addValues as IEnumerable<T>);

        public static void RemoveRange<T>(this List<T> thisValue, IEnumerable<T> removeValues)
        {
            foreach (var removeValue in removeValues)
                if (thisValue.Contains(removeValue))
                    thisValue.Remove(removeValue);
        }
        public static IEnumerable<T> RemoveValuesReturnValues<T>(this List<T> thisValue, IEnumerable<T> removeValues)
        {
            thisValue.RemoveRange(removeValues);
            return removeValues;
        }

        public static IEnumerable<T> RemoveValuesReturnValues<T>(this List<T> thisValue, params T[] removeValues)
            => thisValue.RemoveValuesReturnValues(removeValues as IEnumerable<T>);

        public static IEnumerable<T> RemoveValuesReturnList<T>(this List<T> thisValue, IEnumerable<T> removeValues)
        {
            thisValue.RemoveRange(removeValues);
            return thisValue;
        }

        public static IEnumerable<T> RemoveValuesReturnList<T>(this List<T> thisValue, params T[] values)
            => thisValue.RemoveValuesReturnList(values as IEnumerable<T>);
    }
}
