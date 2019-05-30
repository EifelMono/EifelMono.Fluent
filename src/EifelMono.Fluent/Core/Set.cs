using System;
using System.Collections.Generic;
using System.Linq;
using EifelMono.Fluent.Extensions;

namespace EifelMono.Fluent.Core
{
    public class Set<T> where T : Enum
    {
        public Set(params T[] items)
            => Include(items);

        public List<T> Items { get; set; } = new List<T>();

        public Set<T> Include(IEnumerable<T> items)
        {
            lock (Items)
                foreach (var item in items)
                    if (!Items.Contains(item))
                        Items.Add(item);
            return this;
        }

        public Set<T> Include(params T[] items)
            => Include((IEnumerable<T>)items);

        public Set<T> Include(Set<T> set)
            => Include(set.Items);

        public Set<T> Exclude(IEnumerable<T> items)
        {
            lock (Items)
                foreach (var item in items)
                    if (Items.Contains(item))
                        Items.Remove(item);
            return this;
        }

        public Set<T> Exclude(params T[] items)
            => Exclude((IEnumerable<T>)items);
        public Set<T> Exclude(Set<T> set)
            => Exclude(set.Items);

        public bool Has(IEnumerable<T> items)
        {
            if (items is null)
                return false;
            var containsCount = 0;
            lock (Items)
            {
                foreach (var item in items)
                    if (Items.Contains(item))
                        containsCount++;
            }
            return containsCount == items.Count();
        }

        public bool Has(params T[] items)
            => Has((IEnumerable<T>)items);
        public bool Has(Set<T> set)
            => Has(set.Items);

        public bool Is(IEnumerable<T> items)
        {
            lock (Items)
            {
                if (items is null)
                    return false;
                if (Items.Count != items.Count())
                    return false;
                return Has(items);
            }
        }

        public bool Is(params T[] items)
            => Is((IEnumerable<T>)items);

        public bool Is(Set<T> set)
            => Is(set.Items);

        public bool IsEmpty()
        {
            lock (Items)
                return Items.Count == 0;
        }

        public Set<T> Fill()
        {
            lock (Items)
                Items = fluent.Enum.Values<T>().ToList();
            return this;
        }

        public Set<T> Clear()
        {
            lock (Items)
                Items.Clear();
            return this;
        }

        public Set<T> Invert()
        {
            lock (Items)
            {
                foreach (var item in fluent.Enum.Values<T>())
                    if (Items.Contains(item))
                        Items.Remove(item);
                    else
                        Items.Add(item);
            }
            return this;
        }

        public static Set<T> operator +(Set<T> value1, Set<T> value2)
            => value1.Include(value2);
        public static Set<T> operator +(Set<T> value1, T value2)
            => value1.Include(value2);
        public static Set<T> operator -(Set<T> value1, Set<T> value2)
            => value1.Exclude(value2);
        public static Set<T> operator -(Set<T> value1, T value2)
            => value1.Exclude(value2);

        public override string ToString()
            => $"{Items.Count} {Items.ToJoinString(",")}";
    }
}
