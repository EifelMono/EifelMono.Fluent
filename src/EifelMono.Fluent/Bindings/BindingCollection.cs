using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Bindings
{
    public class BindingCollection<T> : ObservableCollection<T>, IOnPropertyChanged
    {
        public BindingCollection<T> AddItems(IEnumerable<T> addItems)
        {
            addItems.ForEach(item => Items.Add(item));
            return this;
        }

        public void OnPropertyChanged(string propertyName)
            => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }
}
