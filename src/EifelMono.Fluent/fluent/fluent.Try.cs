using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static void Try(Action tryAction, Action<Exception> catchAction = null, Action finallyAction= null)
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
    }
}
