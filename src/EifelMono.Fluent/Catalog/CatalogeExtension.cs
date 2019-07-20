using System;

namespace EifelMono.Fluent.Cataloge
{
    public static class CatalogeExtension
    {
        public static CatalogeSettings DefaultSettings { get; set; } = new CatalogeSettings()
            .SetRootName("")
            .SetDepth(2)
            .IncludeFlags(CatalogeFlag.Properties, CatalogeFlag.Fields, CatalogeFlag.Public, CatalogeFlag.Instance);

        public static string ToCataloge(this object @object, Action<CatalogeSettings> action = null)
        {
            var settings = DefaultSettings;
            action?.Invoke(settings);
            return new CatalogeSerialize().Serialize(@object, settings);
        }

        public static string ToCatalogeAsString(this object @object, Action<CatalogeSettings> action = null)
        {
            var settings = DefaultSettings;
            action?.Invoke(settings);
            return new CatalogeSerialize().Serialize(@object, settings).Replace(Environment.NewLine, " ");
        }

        public static string ToCatalogeAsToString(this object @object, int depth = 2)
            => @object.ToCatalogeAsString(
                s => s.SetDepth(depth)
                        .AddKnownToStringOnType()
                        .ExcludeFlags(CatalogeFlag.NonPublic)
                );
    }
}

