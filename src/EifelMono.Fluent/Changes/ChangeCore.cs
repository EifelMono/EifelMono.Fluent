using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EifelMono.Fluent.Flow;

namespace EifelMono.Fluent.Changes
{
    public class ChangeCore
    {
#if !NETSTANDARD1_6
        public string Name { get; set; }

        private string _FullName = null;

        public string FullName { get => _FullName ?? (_FullName = GetFullName()); }

        protected string GetFullName()
        {
            var fullName = Name;
            var parent = Parent;
            while (parent is object)
            {
                fullName = $"{parent.Name ?? ""}.{fullName}";
                parent = parent.Parent;
            }
            return fullName;
        }
        public ChangeCore()
        {
            SetParent(this);
        }

        protected void SetParent(ChangeCore parent)
        {
            foreach (var property in
                parent.GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(ChangeCore))))
            {
                if (property.Name != nameof(Parent))
                    if (property.GetValue(parent) is ChangeCore v)
                    {
                        v.Name = property.Name;
                        v.Parent = parent;
                        SetParent(v);
                    }
            }
        }
        protected ChangeCore Parent { get; set; } = null;

        protected ChangeCore FindRootParent
        {
            get
            {
                var parent = Parent;
                while (parent.Parent is object)
                    parent = parent.Parent;
                return parent;
            }
        }

        protected virtual void Notify(ChangeProperty changedProperty)
        {
            if (FindRootParent is var rootParent && rootParent is object)
                rootParent?.OnNotify?.Invoke(changedProperty);
        }

        public ActionList<ChangeProperty> OnNotify { get; set; } = new ActionList<ChangeProperty>();

        protected List<ChangeProperty> GetChangedProperties(ChangeCore parent)
        {
            var result = new List<ChangeProperty>();
            foreach (var property in
                parent.GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(ChangeCore))))
            {
                var v = property.GetValue(parent) as ChangeCore;
                if (v is ChangeProperty vp)
                    result.Add(vp);
                if (v is ChangeClass vc)
                    result.AddRange(vc.GetChangedProperties(v));
            }
            return result;
        }
        public List<ChangeProperty> ChangedProperties()
            => GetChangedProperties(this);

        protected string Prefix
        {
            get
            {
                var result = "";
                if (this is ChangeClass)
                    return "c";
                else if (this is ChangeProperty)
                    return "p";
                return result;
            }
        }
        public override string ToString()
            => $"{Prefix}:{FullName}";
#endif
    }
}
