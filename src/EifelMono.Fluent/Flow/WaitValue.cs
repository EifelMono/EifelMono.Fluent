#undef WithOperator
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

        #region WaitAsync
        public async Task<bool> WaitAsync(CancellationToken cancellationToken = default)
        {
            var queue = new TaskCompletionQueuedSource<T>();
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                OnChange.Add(SubscribeOnChange);
                if (await queue.WaitValueAsync(cancellationToken).ConfigureAwait(false) is var result && result.Ok)
                    Value = result.Value;
                return result.Ok;
            }
            finally
            {
                OnChange.Remove(SubscribeOnChange);
            }
        }

        public async Task<bool> WaitAsync(CancellationContainer cancellationContainer)
        {
            try
            {
                return await WaitAsync(cancellationContainer.Token).ConfigureAwait(false);
            }
            finally
            {
                cancellationContainer.DisposeAfterAction();
            }
        }

        public Task<bool> WaitAsync(TimeSpan timeSpan)
            => WaitAsync(timeSpan.AsToken());
        #endregion

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

        public async Task<bool> WaitValueAsync(T[] waitValues, CancellationContainer cancellationContainer)
        {
            try
            {
                return await WaitValueAsync(waitValues, cancellationContainer.Token).ConfigureAwait(false);
            }
            finally
            {
                cancellationContainer?.DisposeAfterAction();
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

        public Task<bool> WaitValueAsync(T waitValue1, CancellationContainer cancellationContainer)
          => WaitValueAsync(new T[] { waitValue1 }, cancellationContainer);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, CancellationContainer cancellationContainer)
            => WaitValueAsync(new T[] { waitValue1, waitValue2 }, cancellationContainer);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, CancellationContainer cancellationContainer)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationContainer);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationContainer cancellationContainer)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationContainer);
        public Task<bool> WaitValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationContainer cancellationContainer)
            => WaitValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationContainer);

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

        public async Task<bool> WaitValuesAsync(T[] waitValues, CancellationContainer cancellationContainer)
        {
            try
            {
                return await WaitValuesAsync(waitValues, cancellationContainer.Token).ConfigureAwait(false);
            }
            finally
            {
                cancellationContainer?.DisposeAfterAction();
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

        public Task<bool> WaitValuesAsync(T waitValue1, CancellationContainer cancellationContainer)
            => WaitValuesAsync(new T[] { waitValue1 }, cancellationContainer);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, CancellationContainer cancellationContainer)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2 }, cancellationContainer);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, CancellationContainer cancellationContainer)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationContainer);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationContainer cancellationContainer)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationContainer);
        public Task<bool> WaitValuesAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationContainer cancellationContainer)
            => WaitValuesAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationContainer);
        #endregion

        #region WaitNotValueAsync
        public async Task<bool> WaitNotValueAsync(T[] waitNotValues, CancellationToken cancellationToken = default)
        {
            if (waitNotValues is null || waitNotValues.Length == 0)
                return false;
            var queue = new TaskCompletionQueuedSource<T>();
            if (!waitNotValues.Contains(Value))
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
                        if (!waitNotValues.Contains(result.Value))
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

        public async Task<bool> WaitNotValueAsync(T[] waitNotValues, CancellationContainer cancellationContainer)
        {
            try
            {
                return await WaitNotValueAsync(waitNotValues, cancellationContainer.Token).ConfigureAwait(false);
            }
            finally
            {
                cancellationContainer?.DisposeAfterAction();
            }
        }

        public Task<bool> WaitNotValueAsync(T waitValue1, CancellationToken cancellationToken = default)
        => WaitNotValueAsync(new T[] { waitValue1 }, cancellationToken);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, CancellationToken cancellationToken = default)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2 }, cancellationToken);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, CancellationToken cancellationToken = default)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationToken);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationToken cancellationToken = default)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationToken);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationToken cancellationToken = default)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationToken);

        public Task<bool> WaitNotValueAsync(T[] waitValues, TimeSpan timeSpan)
          => WaitNotValueAsync(waitValues, timeSpan.AsToken());
        public Task<bool> WaitNotValueAsync(T waitValue1, TimeSpan timeSpane)
            => WaitNotValueAsync(waitValue1, timeSpane.AsToken());
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, TimeSpan timeSpane)
            => WaitNotValueAsync(waitValue1, waitValue2, timeSpane.AsToken());
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, TimeSpan timeSpane)
            => WaitNotValueAsync(waitValue1, waitValue2, waitValue3, timeSpane.AsToken());
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, TimeSpan timeSpane)
            => WaitNotValueAsync(waitValue1, waitValue2, waitValue3, waitValue4, timeSpane.AsToken());
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, TimeSpan timeSpane)
            => WaitNotValueAsync(waitValue1, waitValue2, waitValue3, waitValue4, waitValue5, timeSpane.AsToken());

        public Task<bool> WaitNotValueAsync(T waitValue1, CancellationContainer cancellationContainer)
            => WaitNotValueAsync(new T[] { waitValue1 }, cancellationContainer);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, CancellationContainer cancellationContainer)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2 }, cancellationContainer);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, CancellationContainer cancellationContainer)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3 }, cancellationContainer);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, CancellationContainer cancellationContainer)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4 }, cancellationContainer);
        public Task<bool> WaitNotValueAsync(T waitValue1, T waitValue2, T waitValue3, T waitValue4, T waitValue5, CancellationContainer cancellationContainer)
            => WaitNotValueAsync(new T[] { waitValue1, waitValue2, waitValue3, waitValue4, waitValue5 }, cancellationContainer);

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
