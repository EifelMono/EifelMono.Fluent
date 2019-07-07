using System;
using System.Runtime.CompilerServices;

namespace EifelMono.Fluent.Classes
{
    public class SafeTupleException : Exception
    {
        public SafeTupleException(SafeTuple safeTuple) : base($"HasOk={safeTuple?.HasOk ?? false}")
        {
            SafeTuple = safeTuple;
        }
        public SafeTuple SafeTuple { get; private set; }
    }
    public class SafeTuple
    {
        protected ITuple Tuple { get; set; }
        public SafeTuple(ITuple tuple)
        {
            Tuple = tuple;
        }

        public bool HasOk { get; set; } = false;
        public bool Ok { get; internal set; }
        public bool HasThisValue { get; set; } = false;
        public object ThisValue { get; internal set; }

        public bool IsSafeTuple => HasOk && HasThisValue;
    }

    public class SafeTuple<T> : SafeTuple
    {
        public SafeTuple(ITuple tuple, bool raiseOnError = true) : base(tuple)
        {
            if (tuple is null)
                throw new ArgumentNullException("tuple is null");
            if (Tuple.Length > 0)
                if (Tuple[0] is var ok && ok.GetType() == typeof(bool))
                {
                    Ok = (bool)ok;
                    HasOk = true;
                }
            if (Tuple.Length > 1)
                if (Tuple[^1] is var thisValue && thisValue.GetType() == typeof(T))
                {
                    ThisValue = (T)thisValue;
                    HasThisValue = true;
                }
            if (!IsSafeTuple && raiseOnError)
                throw new SafeTupleException(this);
        }

        public new T ThisValue { get => (T)base.ThisValue; private set => base.ThisValue = value; }
    }
}

