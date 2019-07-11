using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EifelMono.Fluent.Cataloge
{
    public class CatalogeSerialize
    {
        private CatalogeSettings Settings { get; set; }

        private readonly StringBuilder _cataloge = new StringBuilder();
        public string Serialize(object @object, CatalogeSettings settings)
        {
            Settings = settings.Clone();
            Settings.RootName ??= "";
            InternalSerialize(@object, Settings.RootName, 0);
            return _cataloge.ToString();
        }

        private List<PropertyInfo> GetProperties(object @object)
        {
            var result = new List<PropertyInfo>();

            if (Settings.Flags.Contains(CatalogeFlag.Properties))
            {
                BindingFlags bindingFlags = default;
                if (Settings.Flags.Contains(CatalogeFlag.Public))
                    bindingFlags |= BindingFlags.Public;
                if (Settings.Flags.Contains(CatalogeFlag.NonPublic))
                    bindingFlags |= BindingFlags.NonPublic;
                if (Settings.Flags.Contains(CatalogeFlag.Instance))
                    bindingFlags |= BindingFlags.Instance;
                if (Settings.Flags.Contains(CatalogeFlag.Static))
                    bindingFlags |= BindingFlags.Static;
                foreach (var property in @object.GetType().GetProperties(bindingFlags))
                    result.Add(property);
            }
            return result;
        }

        private List<FieldInfo> GetFields(object @object)
        {
            var result = new List<FieldInfo>();

            if (Settings.Flags.Contains(CatalogeFlag.Fields))
            {
                BindingFlags bindingFlags = default;
                if (Settings.Flags.Contains(CatalogeFlag.Public))
                    bindingFlags |= BindingFlags.Public;
                if (Settings.Flags.Contains(CatalogeFlag.NonPublic))
                    bindingFlags |= BindingFlags.NonPublic;
                if (Settings.Flags.Contains(CatalogeFlag.Instance))
                    bindingFlags |= BindingFlags.Instance;
                if (Settings.Flags.Contains(CatalogeFlag.Static))
                    bindingFlags |= BindingFlags.Static;
                foreach (var field in @object.GetType().GetFields(bindingFlags))
                    result.Add(field);
            }
            return result;
        }

        protected void InternalSerializeProperties(object @object, string name, int depth)
        {
            foreach (var property in GetProperties(@object))
            {
                var newName = string.IsNullOrEmpty(property.Name) ? "" : $".{property.Name}";
                if (property.GetGetMethod() is object)
                {
                    var o = property.GetValue(@object);
                    if (Settings.ToStringOnType.Contains(property.PropertyType))
                        _cataloge.AppendLine($"{name}{newName}={o?.ToString() ?? ""}");
                    else
                    {
                        var i = 0;
                        if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                            if (o is IEnumerable)
                                foreach (var item in o as IEnumerable)
                                    InternalSerialize(item, $"{name}.{property.Name}[{i++}]", depth);
                            else
                                InternalSerialize(o, $"{name}.{property.Name}", depth + 1);
                        else
                            InternalSerialize(o, $"{name}{newName}", depth + 1);
                    }
                }
                else
                    _cataloge.AppendLine($"{name}.{newName}=not readable");
            }
        }


        protected void InternalSerializeFields(object @object, string name, int depth)
        {
            foreach (var field in GetFields(@object))
            {
                var o = field.GetValue(@object);
                var newName = string.IsNullOrEmpty(field.Name) ? "" : $".{field.Name}";
                if (Settings.ToStringOnType.Contains(field.FieldType))
                    _cataloge.AppendLine($"{name}{newName}={o?.ToString() ?? ""}");
                else
                {
                    var i = 0;
                    if (field.FieldType.IsClass && field.FieldType != typeof(string))
                    {
                        if (o is IEnumerable)
                            foreach (var item in o as IEnumerable)
                                InternalSerialize(item, $"{name}.{field.Name}[{i++}]", depth);
                        else
                            InternalSerialize(o, $"{name}.{field.Name}", depth + 1);
                    }
                    else
                        InternalSerialize(o, $"{name}{newName}", depth + 1);
                }
            }
        }

        protected void InternalSerialize(object @object, string name, int depth)
        {
            if (@object is null)
            {
                name = name.StartsWith(".") ? name : $".{name}";
                _cataloge.AppendLine($"{name}=(null)");
                return;
            }
            var type = @object.GetType();
            if (depth > Settings.Depth || type.IsPrimitive || type.IsEnum || type == typeof(string))
            {
                name = name.StartsWith(".") ? name : $".{name}";
                if (depth > Settings.Depth)
                    _cataloge.AppendLine($"{name}=({@object.ToString()})");
                else
                    _cataloge.AppendLine($"{name}={@object.ToString()}");
                return;
            }

            depth++;
            if (Settings.Flags.Contains(CatalogeFlag.Properties))
                InternalSerializeProperties(@object, name, depth);
            if (Settings.Flags.Contains(CatalogeFlag.Fields))
                InternalSerializeFields(@object, name, depth);
        }
    }
}
