﻿using System;

namespace EifelMono.Fluent
{
    public static class PipeExtensions
    {
        public static T Pipe<T>(this T thisValue, Action<T> action)
        {
            action(thisValue);
            return thisValue;
        }

        public static T Pipe<T>(this T thisValue, Func<T, T> action)
            => action(thisValue);

        public static Tout Pipe<Tin, Tout>(this Tin thisValue, Func<Tin, Tout> action)
            => action(thisValue);
    }
}
