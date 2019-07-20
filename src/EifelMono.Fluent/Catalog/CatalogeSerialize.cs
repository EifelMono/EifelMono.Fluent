using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private BindingFlags GetBindingFlags()
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
            return bindingFlags;
        }

        private IEnumerable<PropertyInfo> GetProperties(object @object)
            => Settings.Flags.Contains(CatalogeFlag.Properties)
                ? @object.GetType().GetProperties(GetBindingFlags())
                : Enumerable.Empty<PropertyInfo>();

        private IEnumerable<FieldInfo> GetFields(object @object)
            => Settings.Flags.Contains(CatalogeFlag.Fields)
                ? @object.GetType().GetFields(GetBindingFlags())
                : Enumerable.Empty<FieldInfo>();

        protected string ObjectToBracketsString(object o)
        {
            var result = o?.ToString() ?? "null";
            if (Settings.IfEqualInToStringEmbracedWithBrackets)
                if (result.Contains("="))
                    result = $"({result})";
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
                        _cataloge.AppendLine($"{name}{newName}={ObjectToBracketsString(o)}");
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
                    _cataloge.AppendLine($"{name}{newName}={ObjectToBracketsString(o)}");
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
                _cataloge.AppendLine($"{name}={ObjectToBracketsString(@object)}");
                return;
            }

            depth++;
            InternalSerializeProperties(@object, name, depth);
            InternalSerializeFields(@object, name, depth);
        }
    }
}
