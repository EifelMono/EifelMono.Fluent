using System;
using System.Collections.Generic;
using System.Text;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Classes
{
    public class ParentExist
    {
        public object ParentThis { get; protected set; }
    }

    public class ParentExist<T> : ParentExist where T : IExist
    {
        public new T ParentThis { get => (T)base.ParentThis; protected set => base.ParentThis = value; }

        public ParentExist(T parentThis)
        {
            ParentThis = parentThis;
        }

        public T Do(Action<T> action)
        {
            if (ParentThis.Exists)
                action?.Invoke(ParentThis);
            return ParentThis;
        }

        public T DoCatch(Action<T> action)
        {
            try
            {
                if (ParentThis.Exists)
                    action?.Invoke(ParentThis);
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return ParentThis;
        }
    }
}
