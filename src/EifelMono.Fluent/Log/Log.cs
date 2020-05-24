using System;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Log
{
    public static class Log
    {
        #region System.Diagnostics
        public static void SystemDebug(this object thisValue)
            => System.Diagnostics.Debug.WriteLine(thisValue.ToString());
        public static void SystemDebugDump(this object thisValue)
            => System.Diagnostics.Debug.WriteLine(thisValue.ToJson());
        #endregion

        #region LogProxy
        public static void SetLogProxy(ILogProxy logProxy)
        {
            LogProxy = logProxy;
        }
        private static ILogProxy LogProxy { get; set; }
        #endregion

        #region Log

        public static string LogDebug(this string thisValue)
        {
            LogProxy?.OnLogDebug(thisValue);
            return thisValue;
        }
        public static string LogTrace(this string thisValue)
        {
            LogProxy?.OnLogTrace(thisValue);
            return thisValue;
        }
        public static string LogWarning(this string thisValue)
        {
            LogProxy?.OnLogWarning(thisValue);
            return thisValue;
        }
        public static string LogError(this string thisValue)
        {
            LogProxy?.OnLogError(thisValue);
            return thisValue;
        }
        public static Exception LogException(this Exception thisValue, string message= null)
        {
            if (message is { })
                message.LogError();
            LogProxy?.OnLogException(thisValue);
            return thisValue;
        }
        public static Exception LogSafeException(this Exception thisValue)
        {
            LogProxy?.OnLogSafeException(thisValue);
            return thisValue;
        }
        #endregion
    }
}
