using System;
using System.Collections.Generic;
using System.Linq;
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
                var currentValue = Value;
                if (!Equals(currentValue, value))
                {
                    lock (_valueLockObject)
                        _value = value;
                    OnChange.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True=Wait finished, False=Canceled</returns>
        public async Task<bool> WaitValueAsync(T value, CancellationToken cancellationToken = default)
        {
            var queue = new TaskCompletionQueuedSource<T>();
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                OnChange.Add(SubscribeOnChange);
                if (Equals(Value, value))
                    return true;
                bool running = true;
                while (running)
                {
                    if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
                        return true;
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

        public static implicit operator WaitValue<T>(T value)
            => new WaitValue<T>(value);

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
