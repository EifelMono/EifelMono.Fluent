using System;
using System.Collections.Generic;
using System.Text;

namespace EifelMono.Fluent.Log
{
    public interface ILogProxy
    {
        Action<string> OnLogDebug { get; set; }
        Action<string> OnLogTrace { get; set; }
        Action<string> OnLogWarning { get; set; }
        Action<string> OnLogError { get; set; }

        Action<Exception> OnLogException { get; set; }
    }
}
