using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace EifelMono.Fluent.Bindings
{
    public static class BindingExtensions
    {
#pragma warning disable IDE0060 // Remove unused parameter
        #region ICommand BindingCommand
        public static ICommand Command(this IOnPropertyChanged thisValue, ref ICommand backingField, Action execute)
            => BindingCommand.Create(ref backingField, execute);
        public static ICommand Command(this IOnPropertyChanged thisValue, ref ICommand backingField, Action<object> execute)
            => BindingCommand.Create(ref backingField, execute);
        #endregion

        #region Property
        public static void PropertiesRefresh(this IOnPropertyChanged thisValue)
            => thisValue.OnPropertyChanged(string.Empty);
        public static T PropertyGet<T>(this IOnPropertyChanged thisValue, ref T backingField, [CallerMemberName]string propertyName = "")
            => backingField;

        public static T PropertySet<T>(this IOnPropertyChanged thisValue, ref T backingField, T newValue, [CallerMemberName]string propertyName = "", params string[] additionalPropertyNames)
        {
            if (!EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                backingField = newValue;
                thisValue.OnPropertyChanged(propertyName);
                foreach (var additionalPropertyName in additionalPropertyNames)
                    thisValue.OnPropertyChanged(additionalPropertyName);
            }
            return backingField;
        }


        #endregion
#pragma warning restore IDE0060 // Remove unused parameter
    }
}
