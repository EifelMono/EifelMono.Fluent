using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Bindings
{
    public class BindingCollection<T> : ObservableCollection<T>, IOnPropertyChanged, IDisposable
    {

        public BindingCollection() : base()
        {
            CollectionChanged += BindingCollection_CollectionChanged;
        }

        public BindingCollection(IEnumerable<T> collection) : base(collection)
        {
            CollectionChanged += BindingCollection_CollectionChanged;
        }

        private void BindingCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                    BadgeAdded++;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                    BadgeRemoved++;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    BadgeAdded = 0;
                    BadgeRemoved = 0;
                    break;
            }
        }

        public void Dispose()
        {
            CollectionChanged -= BindingCollection_CollectionChanged;
        }

        public BindingCollection<T> AddItems(IEnumerable<T> addItems)
        {
            addItems.ForEach(item => Add(item));
            return this;
        }

        public new BindingCollection<T> ClearItems()
        {
            base.ClearItems();
            return this;
        }

        public void OnPropertyChanged(string propertyName)
            => OnPropertyChanged(new PropertyChangedEventArgs(propertyName));

        public int BadgeAdded { get; set; }
        public int BadgeRemoved { get; set; }

        public void BadgeReset()
        {
            BadgeAdded = 0;
            BadgeRemoved = 0;
        }
    }
}
