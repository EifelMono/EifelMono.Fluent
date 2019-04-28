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
        //public async Task<bool> WaitValueAsync(T waitValue, CancellationToken cancellationToken = default)
        //{
        //    var queue = new TaskCompletionQueuedSource<T>();
        //    void SubscribeOnChange(T newValue) => queue.NewData(newValue);
        //    try
        //    {
        //        OnChange.Add(SubscribeOnChange);
        //        if (Equals(Value, waitValue))
        //            return true;
        //        bool running = true;
        //        while (running)
        //        {
        //            if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
        //            {
        //                if (Equals(result.Value, waitValue))
        //                    return true;
        //            }
        //            else
        //                running = false;
        //            if (cancellationToken.IsCancellationRequested)
        //                running = false;
        //        }
        //        return false;
        //    }
        //    finally
        //    {
        //        OnChange.Remove(SubscribeOnChange);
        //    }
        //}

        #region WaitValueAsync
        public async Task<bool> WaitValueAsync(T[] waitValues, CancellationToken cancellationToken = default)
        {
            if (waitValues is null || waitValues.Length == 0)
                return false;
            var queue = new TaskCompletionQueuedSource<T>();
            if (waitValues.Contains(Value))
                return true;
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                OnChange.Add(SubscribeOnChange);
                bool running = true;
                while (running)
                {
                    if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
                    {
                        if (waitValues.Contains(result.Value))
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

        public Task<bool> WaitValueAsync(T waitValue1, CancellationToken cancellationToken = default)
            => WaitValueAsync(new T[] { waitValue1 }, cancellationToken);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, CancellationToken cancellationToken = default)
            => WaitValueAsync(new T[] { waitValue1, waitValue2 }, cancellationToken);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, CancellationToken cancellationToken = default)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationToken);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationToken cancellationToken = default)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationToken);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationToken cancellationToken = default)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationToken);

        public Task<bool> WaitValueAsync(T[] waitValues, TimeSpan timeSpan)
          => WaitValueAsync(waitValues, timeSpan.AsToken());
        public Task<bool> WaitValueAsync(T waitValue1, TimeSpan timeSpane)
            => WaitValueAsync(waitValue1, timeSpane.AsToken());
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, TimeSpan timeSpane)
            => WaitValueAsync(waitValue1, waitValue2, timeSpane.AsToken());
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, TimeSpan timeSpane)
            => WaitValueAsync(waitValue1, waitValue2, waitValue3, timeSpane.AsToken());
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, TimeSpan timeSpane)
            => WaitValueAsync(waitValue1, waitValue2, waitValue3, waitValue4, timeSpane.AsToken());
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, TimeSpan timeSpane)
            => WaitValueAsync(waitValue1, waitValue2, waitValue3, waitValue4, waitValue5, timeSpane.AsToken());

        #endregion

        #region WaitValuesAsync
        public async Task<bool> WaitValuesAsync(T[] waitValues, CancellationToken cancellationToken = default)
        {
            if (waitValues is null || waitValues.Length == 0)
                return false;
            var waitValuesDictionary = waitValues.ToDictionary(v => v, v => false);
            if (waitValuesDictionary.ContainsKey(Value))
                waitValuesDictionary[Value] = true;
            if (waitValuesDictionary.Where(v => v.Value == false).Count() == 0)
                return true;

            var queue = new TaskCompletionQueuedSource<T>();
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                OnChange.Add(SubscribeOnChange);
                bool running = true;
                while (running)
                {
                    if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
                    {
                        if (waitValuesDictionary.ContainsKey(result.Value))
                            waitValuesDictionary[result.Value] = true;
                        if (waitValuesDictionary.Where(v => v.Value == false).Count() == 0)
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

        public Task<bool> WaitValuesAsync(T waitValue1, CancellationToken cancellationToken = default)
            => WaitValuesAsync(new T[] { waitValue1 }, cancellationToken);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, CancellationToken cancellationToken = default)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2 }, cancellationToken);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, CancellationToken cancellationToken = default)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationToken);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationToken cancellationToken = default)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationToken);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationToken cancellationToken = default)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationToken);

        public Task<bool> WaitValuesAsync(T[] waitValues, TimeSpan timeSpan)
             => WaitValuesAsync(waitValues, timeSpan.AsToken());
        public Task<bool> WaitValuesAsync(T waitValue1, TimeSpan timeSpan)
         => WaitValuesAsync(waitValue1, timeSpan.AsToken());
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, TimeSpan timeSpan)
            => WaitValuesAsync(waitValue1, waitValue2, timeSpan.AsToken());
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, TimeSpan timeSpan)
            => WaitValuesAsync(waitValue1, waitValue2, waitValue3, timeSpan.AsToken());
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, TimeSpan timeSpan)
            => WaitValuesAsync(waitValue1, waitValue2, waitValue3, waitValue4, timeSpan.AsToken());
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, TimeSpan timeSpan)
            => WaitValuesAsync(waitValue1, waitValue2, waitValue3, waitValue4, waitValue5, timeSpan.AsToken());
        #endregion


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
