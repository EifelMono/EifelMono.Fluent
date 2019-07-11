using System;
using System.Collections.Generic;
using EifelMono.Fluent.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EifelMono.Fluent.Changes
{
    public class ChangeProperty : ChangeCore
    {
        protected object LockObject = new object();
        public DateTime TimeStamp { get; set; } = DateTime.MinValue;
        protected object _Value;
        public object Value
        {
            get => _Value;
            set
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

        public void DoNotify(ChangeProperty changedProperty = default)
            => Notify(changedProperty ?? this);

        public void DisableNotify()
           => _isNotifyEnabled = false;
        public void EnableNotify(ChangeProperty changedProperty = default)
        {
            _isNotifyEnabled = true;
            DoNotify(changedProperty);
        }

        protected List<string> _Infos = new List<string>();
        public List<string> Infos
        {
            get => _Infos;
            set
            {
                _Infos = value;
                Notify(this);
            }
        }

        public string AddInfo(string info, int maxInfos = 10)
        {
            DisableNotify();
            try
            {
                Infos.Add(info);
                while (Infos.Count > maxInfos)
                    Infos.RemoveAt(0);
            }
            finally
            {
                EnableNotify();
            }
            return info;
        }

        [JsonIgnore]
        public string this[int i]
        {
            get
            {
                while (_Infos.Count <= i)
                    _Infos.Add("");
                return _Infos[i];
            }
            set
            {
                while (_Infos.Count <= i)
                    _Infos.Add("");
                _Infos[i] = value;
                DoNotify();
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
        public string ToChangeString(bool shortInfo = true, bool withInfos = true)
        {
            string result;
            var error = HasError ? $"Error {Error}" : "";
            if (shortInfo)
                result = $"{Name} = {LastValue?.ToString() ?? "Null"} => {Value?.ToString() ?? "Null"}"
                 + $" {SecondsToLast} seconds to last {error}";
            else
                result = $"{base.ToString()} at {TimeStamp} from {LastValue?.ToString() ?? "Null"} to {Value?.ToString() ?? "Null"}"
                 + $" seconds to last {SecondsToLast} from now {SecondsFromNow} {error}";
            if (withInfos)
                if (Infos.Count > 0)
                    result += Environment.NewLine + string.Join(Environment.NewLine, Infos);
            return result;
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
        public ChangeEnumProperty(T value = default) : base()
        {
            TypeName = typeof(T).Name;
            _Value = default(T);
            LastValue = default;
            Value = value;
            Exception = null;
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public new T Value { get => (T)base.Value; set => base.Value = value; }

        [JsonConverter(typeof(StringEnumConverter))]
        public new T LastValue { get => (T)base.LastValue; set => base.LastValue = value; }
    }

}
