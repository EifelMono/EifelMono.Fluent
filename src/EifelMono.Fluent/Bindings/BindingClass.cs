using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EifelMono.Fluent.Bindings
{
    public class BindingClass : INotifyPropertyChanged, IOnPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
