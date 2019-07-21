using System;

namespace EifelMono.Fluent.Cataloge
{
    public static class CatalogeExtension
    {
        public static CatalogeSettings DefaultSettings { get; set; } = new CatalogeSettings()
            .SetRootName("")
            .SetDepth(1)
            .IncludeFlags(
                CatalogeFlag.Properties,
                CatalogeFlag.Fields,
                CatalogeFlag.Public,
                CatalogeFlag.Instance)
            .AddKnownToStringOnType();

        public static string ToCataloge(this object @object, Action<CatalogeSettings> action = null)
        {
            var settings = DefaultSettings;
            action?.Invoke(settings);
            return new CatalogeSerialize().Serialize(@object, settings) ?? "";
        }

        public static string ToCatalogeString(this object @object, Action<CatalogeSettings> action = null)
            => @object.ToCataloge(action).Replace(Environment.NewLine, " ");
        public static string ToCatalogeString(this object @object, int depth)
               => @object.ToCatalogeString(
                   s => s.SetDepth(depth));

        [Obsolete("use ToCatalogeString")]
        public static string ToCatalogeToString(this object @object, int depth = 1)
               => @object.ToCatalogeString(depth);
    }
}

