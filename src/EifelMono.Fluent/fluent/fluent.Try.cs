﻿using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{
    public static partial class fluent
    {
        public static void Try(Action tryAction, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            try
            {
                tryAction?.Invoke();
            }
            catch (Exception ex)
            {
                catchAction?.Invoke(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }
        public static T Try<T>(Func<T> tryAction, Func<Exception, T> catchAction = null, Action finallyAction = null)
        {
            try
            {
                return tryAction.Invoke();
            }
            catch (Exception ex)
            {
                if (catchAction is object)
                    return catchAction.Invoke(ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
            return default;
        }

        public static (bool Ok, T Value) TrySafe<T>(Func<T> func, Action<Exception> catchAction = null, Action finallyAction = null)
        {
            try
            {
                return (true, func.Invoke());
            }
            catch (Exception ex)
            {
                catchAction?.Invoke(ex);
                return (false, default);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }
    }
}
