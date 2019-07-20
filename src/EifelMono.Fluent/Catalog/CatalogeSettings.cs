using System;
using System.Collections.Generic;

namespace EifelMono.Fluent.Cataloge
{
    public class CatalogeSettings
    {
        public string RootName { get; set; } = "";
        public HashSet<CatalogeFlag> Flags { get; internal set; } = new HashSet<CatalogeFlag>();
        public int Depth { get; set; } = 2;

        public bool IfEqualInToStringEmbracedWithBrackets { get; set; } = true;

        public List<Type> ToStringOnType { get; set; } = new List<Type>();

        public CatalogeSettings Clone()
            => new CatalogeSettings
            {
                RootName = RootName,
                Flags = new HashSet<CatalogeFlag>(Flags),
                Depth = Depth,
                ToStringOnType = new List<Type>(ToStringOnType)
            };

        public static List<Type> KnownToStringOnType { get; set; } = new List<Type>
        {
            typeof(DateTime),
            typeof(Version)
        };
    }

    public static class ConvertSettingsExtension
    {
        public static T SetRootName<T>(this T thisValue, string rootName) where T : CatalogeSettings
        {
            thisValue.RootName = rootName;
            return thisValue;
        }

        public static T SetDepth<T>(this T thisValue, int depth) where T : CatalogeSettings
        {
            thisValue.Depth = depth;
            return thisValue;
        }

        public static T SetFlags<T>(this T thisValue, HashSet<CatalogeFlag> flags) where T : CatalogeSettings
        {
            if (flags is null)
                throw new NullReferenceException();
            thisValue.Flags = flags;
            return thisValue;
        }

        public static T IncludeFlags<T>(this T thisValue, params CatalogeFlag[] flags) where T : CatalogeSettings
        {
            foreach (var flag in flags)
                if (!thisValue.Flags.Contains(flag))
                    thisValue.Flags.Add(flag);
            return thisValue;
        }

        public static T ExcludeFlags<T>(this T thisValue, params CatalogeFlag[] flags) where T : CatalogeSettings
        {
            foreach (var flag in flags)
                if (thisValue.Flags.Contains(flag))
                    thisValue.Flags.Remove(flag);
            return thisValue;
        }

        public static T AddToStringOnType<T>(this T thisValue, params Type[] types) where T : CatalogeSettings
        {
            foreach (var type in types)
                if (!thisValue.ToStringOnType.Contains(type))
                    thisValue.ToStringOnType.Add(type);
            return thisValue;
        }

        public static T AddKnownToStringOnType<T>(this T thisValue, params Type[] types) where T : CatalogeSettings
        {
            foreach (var type in CatalogeSettings.KnownToStringOnType)
                if (!thisValue.ToStringOnType.Contains(type))
                    thisValue.ToStringOnType.Add(type);
            foreach (var type in types)
                if (!thisValue.ToStringOnType.Contains(type))
                    thisValue.ToStringOnType.Add(type);
            return thisValue;
        }

        public static T RemoveToStringOnType<T>(this T thisValue, params Type[] types) where T : CatalogeSettings
        {
            foreach (var type in types)
                if (thisValue.ToStringOnType.Contains(type))
                    thisValue.ToStringOnType.Remove(type);
            return thisValue;
        }
        public static T ClearToStringOnType<T>(this T thisValue) where T : CatalogeSettings
        {
            thisValue.ToStringOnType.Clear();
            return thisValue;
        }


    }
}
