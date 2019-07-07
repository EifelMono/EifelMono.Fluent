using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EifelMono.Fluent.Extensions;
using EifelMono.Fluent.Flow;
using Newtonsoft.Json;

namespace EifelMono.Fluent.Changes
{
    public class ChangeCore
    {
        [JsonIgnore]
        private string _typeName = null;
        public string TypeName
        {
            get => _typeName ?? (_typeName = GetType().Name);
            set => _typeName = value;
        }

        public int Level { get; set; }
        public string Name { get; set; }

        private string _fullName = null;

        public string FullName
        {
            get => _fullName ?? (_fullName = GetFullName());
            set
            {
                if (_fullName != value)
                    _fullName = value;
            }
        }

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

            if (_fullName.IsNullOrEmpty())
                _fullName = GetFullName();
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
                while (parent?.Parent is object)
                    parent = parent.Parent;
                return parent;
            }
        }

        protected virtual void Notify(ChangeProperty changedProperty)
        {
            if (FindRootParent is var rootParent && rootParent is object)
                rootParent?.OnNotify?.Invoke(changedProperty);
        }

        protected bool _isNotifyEnabled = true;
        [JsonIgnore]
        public bool IsNotifyEnabled => _isNotifyEnabled;

        [JsonIgnore]
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

        protected virtual string Prefix
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

        public List<ChangeCore> GetChildrensTree()
            => GetChildrens(int.MaxValue);

        public List<ChangeCore> GetChildrens()
            => GetChildrens(2);

        protected List<ChangeCore> GetChildrens(int maxDepth)
        {
            int level = 0;
            var parent = this;
            while (parent.Parent is object)
            {
                level++;
                parent = parent.Parent;
            }
            return GetInternalChildrens(this, level, 0, maxDepth);
        }

        public List<ChangeCore> GetInternalChildrens(ChangeCore parent, int level, int depth, int maxDepth)
        {
            var result = new List<ChangeCore>();
            if (parent is null)
                return result;
            parent.Level = level;
            result.Add(parent);
            level++;
            depth++;
            if (depth < maxDepth)
                foreach (var propertyInfo in parent.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType.IsSubclassOf(typeof(ChangeCore))))
                {
                    var o = propertyInfo.GetValue(parent) as ChangeCore;
                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(ChangeClass)))
                        result.AddRange(GetInternalChildrens(o, level, depth, maxDepth));
                    if (propertyInfo.PropertyType.IsSubclassOf(typeof(ChangeProperty)))
                    {
                        o.Level = level;
                        result.Add(o);
                    }
                }
            return result;
        }

        public override string ToString()
            => $"{Prefix}:{FullName}";
    }
}
