using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Classes;

namespace EifelMono.Fluent.Extensions
{
    public static class TriStateExtensions
    {
        public static bool IsUnkown(this TriState thisValue)
            => thisValue == TriState.Unkown;
        public static bool IsDefined(this TriState thisValue)
            => ! thisValue.IsUnkown();

        public static bool IsYes(this TriState thisValue)
            => thisValue == TriState.Yes;
        public static bool IsNo(this TriState thisValue)
            => thisValue == TriState.No;
    }
}
