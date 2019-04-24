#undef WithOperator
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EifelMono.Fluent.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EifelMono.Fluent.Flow
{
    public class WaitValue<T>
    {
        public WaitValue(T defaultValue = default)
        {
            _value = defaultValue;
        }
        [JsonIgnore]
        public ActionList<T> OnChange { get; protected set; } = new ActionList<T>();

        protected object _valueLockObject = new object();
        private T _value = default;

        public T Value
        {
            get
            {
                lock (_valueLockObject)
                    return _value;
            }
            set
            {
                bool changed = false;
                lock (_valueLockObject)
                {
                    if (changed = !Equals(_value, value))
                        lock (_valueLockObject)
                            _value = value;
                }
                if (changed)
                    OnChange.Invoke(value);
            }
        }

        public T SetValue(T value)
        {
            Value = value;
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitValue"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True=Wait finished, False=Canceled</returns>
        public async Task<bool> WaitValueAsync(T waitValue, CancellationToken cancellationToken = default)
        {
            var queue = new TaskCompletionQueuedSource<T>();
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                OnChange.Add(SubscribeOnChange);
                if (Equals(Value, waitValue))
                    return true;
                bool running = true;
                while (running)
                {
                    if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
                    {
                        if (Equals(result.Value, waitValue))
                            return true;
                    }
                    else
                        running = false;
                    if (cancellationToken.IsCancellationRequested)
                        running = false;
                }
                return false;
            }
            finally
            {
                OnChange.Remove(SubscribeOnChange);
            }
        }

        public Task<bool> WaitValueAsync(T value, TimeSpan timeSpan)
            => WaitValueAsync(value, new CancellationTokenSource(timeSpan).Token);

        public static implicit operator T(WaitValue<T> value)
            => value.Value;

#if WithOperator
        // Action List is not copied
        public static implicit operator WaitValue<T>(T value)
            => new WaitValue<T>(value);
#endif

        public override string ToString()
            => $"{Value}";
    }
    public class WaitEnumValue<T> : WaitValue<T> where T : Enum
    {
        public WaitEnumValue() { }
        public WaitEnumValue(T defaultValue) { Value = defaultValue; }

        private List<T> Values => fluent.Enum.Values<T>().ToList();

        [JsonConverter(typeof(StringEnumConverter))]
        public new T Value
        {
            get => base.Value;
            set => base.Value = value;
        }
        public T Next()
        {
            Value = Values.Next(Value);
            return Value;
        }

        public T Previous()
        {
            Value = Values.Previous(Value);
            return Value;
        }

        public T First()
        {
            Value = Values.First();
            return Value;
        }
        public T Last()
        {
            Value = Values.Last();
            return Value;
        }
        public override string ToString()
            => $"{Value.ToString()}";
    }
}
