using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EifelMono.Fluent.Extensions
{
    public static class TimeSpanExtensions
    {   
        /// <summary>
        /// Creates a Token for timeout from CancellationTokenSource
        /// </summary>
        /// <param name="thisValue"></param>
        /// <returns></returns>
        public static CancellationToken CreateToken(this TimeSpan thisValue)
            => new CancellationTokenSource(thisValue).Token;
    }
}
