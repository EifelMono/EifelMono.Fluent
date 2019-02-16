using System;

namespace EifelMono.Fluent
{
    public class FluentExActionResult<T>
    {
        public bool Fixed { get; set; }
        public T Data { get; set; }
    }
    public delegate FluentExActionResult<T> FluentExAction<T>(Exception ex, T data);
    public delegate FluentExActionResult<TOut> FluentExAction<TIn, TOut>(Exception ex, TIn data);
}
