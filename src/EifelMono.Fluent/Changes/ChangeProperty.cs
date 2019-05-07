using System;
using EifelMono.Fluent.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EifelMono.Fluent.Changes
{
    public abstract class ChangeProperty : ChangeCore
    {
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

        private Exception _exception;
        [JsonIgnore]
        public Exception Exception
        {
            get => _exception;
            set
            {
                _exception = value;
                Error = _exception?.Message ?? "";
            }
        }
        public string Error { get; set; }

        public bool HasError => Error.IsNotNullOrEmpty();

        public virtual void Reset()
        {
            _Value = default;
            TimeStamp = default;
            LastValue = default;
            LastTimeStamp = DateTime.MinValue;
            Exception = null;
        }

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
            var error = HasError ? $"Error {Error}" : "";
            if (shortInfo)
                return $"{Name} = {LastValue?.ToString() ?? "Null"} => {Value?.ToString() ?? "Null"}"
                 + $" {SecondsToLast} seconds to last {error}".Trim();
            else
                return $"{base.ToString()} at {TimeStamp} from {LastValue?.ToString() ?? "Null"} to {Value?.ToString() ?? "Null"}"
                 + $" seconds to last {SecondsToLast} from now {SecondsFromNow} {error}".Trim();
        }
    }

    public class ChangeProperty<T> : ChangeProperty
    {
        public ChangeProperty(T value = default) : base()
        {
            TypeName = typeof(T).Name;
            _Value = default(T);
            LastValue = default;
            Value = value;
            Exception = null;
        }

        public new T Value { get => (T)base.Value; set => base.Value = value; }

        public new T LastValue { get => (T)base.LastValue; set => base.LastValue = value; }

        public override void Reset()
        {
            base.Reset();
            _Value = default(T);
            LastValue = default;
        }
    }

    public class ChangeEnumProperty<T> : ChangeProperty<T> where T : Enum
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public new T Value { get => (T)base.Value; set => base.Value = value; }

        [JsonConverter(typeof(StringEnumConverter))]
        public new T LastValue { get => (T)base.LastValue; set => base.LastValue = value; }
    }
}
