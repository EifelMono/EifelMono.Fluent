using System;
using EifelMono.Fluent.Classes;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Extensions
{
    public static class IFluentExtensions
    {
        #region IFluentExists
        public static T IfExistDo<T>(this T thisValue, Action<T> tryAaction) where T : IFluentExists
        {
            if (thisValue.Exists)
                tryAaction?.Invoke(thisValue);
            return thisValue;
        }
        public static T IfExistDoCatch<T>(this T thisValue, Action<T> tryAaction, Action<T, Exception> catchAction= null) where T : IFluentExists
        {
            try
            {
                if (thisValue.Exists)
                    tryAaction?.Invoke(thisValue);
            }
            catch (Exception ex)
            {
                ex.LogException();
                catchAction?.Invoke(thisValue, ex);
            }
            return thisValue;
        }

        public static T IfNotExistDo<T>(this T thisValue, Action<T> tryAaction) where T : IFluentExists
        {
            if (!thisValue.Exists)
                tryAaction?.Invoke(thisValue);
            return thisValue;
        }
        public static T IfNotExistDoCatch<T>(this T thisValue, Action<T> action, Action<T, Exception> catchAction = null) where T : IFluentExists
        {
            try
            {
                if (!thisValue.Exists)
                    action?.Invoke(thisValue);
            }
            catch (Exception ex)
            {
                ex.LogException();
                catchAction?.Invoke(thisValue, ex);
            }
            return thisValue;
        }
        #endregion

        #region IFluentCondition
        public static T IfFluentDo<T>(this T thisValue, Action<T> tryAaction) where T : IFluentCondition
        {
            if (thisValue.FluentCondition)
                tryAaction?.Invoke(thisValue);
            return thisValue;
        }
        public static T IfFluentDoCatch<T>(this T thisValue, Action<T> tryAaction, Action<T, Exception> catchAction = null) where T : IFluentCondition
        {
            try
            {
                if (thisValue.FluentCondition)
                    tryAaction?.Invoke(thisValue);
            }
            catch (Exception ex)
            {
                ex.LogException();
                catchAction?.Invoke(thisValue, ex);
            }
            return thisValue;
        }

        public static T IfNotFluentDo<T>(this T thisValue, Action<T> tryAaction) where T : IFluentCondition
        {
            if (!thisValue.FluentCondition)
                tryAaction?.Invoke(thisValue);
            return thisValue;
        }
        public static T IfNotFluentDoCatch<T>(this T thisValue, Action<T> tryAaction, Action<T, Exception> catchAction = null) where T : IFluentCondition
        {
            try
            {
                if (!thisValue.FluentCondition)
                    tryAaction?.Invoke(thisValue);
            }
            catch (Exception ex)
            {
                ex.LogException();
                catchAction?.Invoke(thisValue, ex);
            }
            return thisValue;
        }
        #endregion
    }
}
