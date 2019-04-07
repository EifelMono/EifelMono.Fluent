using System;

namespace EifelMono.Fluent
{
#pragma warning disable IDE1006 // Naming Styles
    public static partial class fluent
#pragma warning restore IDE1006 // Naming Styles
    {
        public static Action<Action> OnInvokeInMainThread { get; set; } = null;

        /// <summary>
        /// OnInvokeInMainThread must be implemented
        /// WPF: OnInvokeInMainThread= (action) => Application.Current.Dispatcher.Invoke(action)
        /// Forms: ?
        /// </summary>
        /// <param name="action"></param>
        public static void InvokeInMainThread(Action action)
        {
            // System.Windows.Threading.DispatcherPriority.Dispatcher.Invoke(action);
            OnInvokeInMainThread?.Invoke(action);
        }
    }
}
