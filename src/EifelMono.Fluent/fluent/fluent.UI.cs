using System;

#pragma warning disable IDE1006 // Naming Styles
namespace EifelMono.Fluent
{

    public static partial class fluent
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
