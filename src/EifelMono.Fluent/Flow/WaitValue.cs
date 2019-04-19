using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Flow
{
    public class WaitValue<T>
    {
        private readonly object _onChangeLockObject = new object();
        public event Action<T> OnChange;

        private T _value = default;
        public T Value
        {
            get => _value;
            set
            {
                if (!Equals(_value, value))
                {
                    _value = value;
                    lock (_onChangeLockObject)
                        OnChange?.Invoke(value);
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
            if (Equals(Value, value))
                return true;
            var queue = new TaskCompletionQueuedSource<T>();
            void SubscribeOnChange(T newValue) => queue.NewData(newValue);
            try
            {
                lock (_onChangeLockObject)
                    OnChange += SubscribeOnChange;
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
                lock (_onChangeLockObject)
                    OnChange -= SubscribeOnChange;
            }
        }

        public Task<bool> WaitValueAsync(T value, TimeSpan timeSpan)
            => WaitValueAsync(value, new CancellationTokenSource(timeSpan).Token);

        public override string ToString()
            => $"{Value}";
    }
    public class WaitEnumValue<T> : WaitValue<T> where T : Enum
    {
        public WaitEnumValue() { }
        public WaitEnumValue(T defaultValue) { Value = defaultValue; }

        private List<T> Values => fluent.Enum.Values<T>().ToList();
        public T Next()
        {
            var index = Values.IndexOf(Value);
            if (index < Values.Count - 1)
                Value = Values[++index];
            return Value;
        }
        public override string ToString()
            => $"{Value.ToString()}";
    }
}
