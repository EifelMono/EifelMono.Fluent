using System;

namespace EifelMono.Fluent.Changes
{
    public class ChangeProperty : ChangeCore
    {
#if !NETSTANDARD1_6
        protected object LockObject = new object();
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;
        protected object _Value;
        public object Value
        {
            get => _Value; set
            {
                bool changed = false;
                lock (LockObject)
                {
                    if (changed = !Equals(_Value, value))
                    {
                        LastValue = _Value;
                        LastTimeStamp = TimeStamp;
                        TimeStamp = DateTime.Now;
                        _Value = value;
                    }
                }
                if (changed)
                    Notify(this);
            }
        }

        public object LastValue { get; set; }
        public DateTime LastTimeStamp { get; set; } = DateTime.MinValue;

        public long SecondsToLast
        {
            get
            {
                if (LastTimeStamp == DateTime.MinValue)
                    return 0;
                if (TimeStamp == DateTime.MinValue)
                    return 0;
                return (long)(TimeStamp - LastTimeStamp).TotalSeconds;
            }
        }
        public long SecondsFromNow
        {
            get
            {
                if (TimeStamp == DateTime.MinValue)
                    return 0;
                return (long)(DateTime.Now - TimeStamp).TotalSeconds;
            }
        }


        public override string ToString()
            => $"{base.ToString()} = {Value?.ToString() ?? "Null"} at {TimeStamp}";
        public string ToChangeString(bool shortInfo = true)
        {
            if (shortInfo)
                return $"{Name} = {LastValue?.ToString() ?? "Null"} => {Value?.ToString() ?? "Null"}"
                 + $" {SecondsToLast} seconds to last";
            else
                return $"{base.ToString()} at {TimeStamp} from {LastValue?.ToString() ?? "Null"} to {Value?.ToString() ?? "Null"}"
                 + $" seconds to last {SecondsToLast} from now {SecondsFromNow}";
        }
    }

    public class ChangeProperty<T> : ChangeProperty
    {
        public ChangeProperty(T value = default) : base()
        {
            _Value = default(T);
            LastValue = default;
            Value = value;
        }

        public new T Value { get => (T)base.Value; set => base.Value = value; }

        public new T LastValue { get => (T)base.LastValue; set => base.LastValue = value; }
#endif
    }
}
