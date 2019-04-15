using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Extensions
{
    public static partial class CharExtensions
    {
        public static bool IsDigit(this char thisValue)
            => char.IsDigit(thisValue);
    }
}
