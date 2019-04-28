using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EifelMono.Fluent.Extensions
{
    public static class TimeSpanExtensions
    {   
        /// <summary>
        /// Creates a Token with timeout from CancellationTokenSource
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static CancellationToken AsToken(this TimeSpan thisValue)
            => new CancellationTokenSource(thisValue).Token;
    }
}
