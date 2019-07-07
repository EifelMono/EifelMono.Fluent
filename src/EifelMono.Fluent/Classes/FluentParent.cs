using System;
using EifelMono.Fluent.Log;

namespace EifelMono.Fluent.Classes
{
    public class FluentParent
    {
        public object ParentThis { get; protected set; }
        public FluentParent(object parentThis)
        {
            ParentThis = parentThis;
        }
    }

    public class FluentParent<T> : FluentParent 
    {
        public new T ParentThis { get => (T)base.ParentThis; protected set => base.ParentThis = value; }

        public FluentParent(T parentThis) : base(parentThis) { }
    }

    public abstract class FluentParentCondition<T> : FluentParent<T> 
    {
        protected Func<bool> ConditionFunc { get; set; }
        public FluentParentCondition(T parentThis, Func<bool> conditionFunc) : base(parentThis)
        {
            ConditionFunc = conditionFunc;
        }

        public bool Condition => ConditionFunc();

        public T Do(Action<T> action)
        {
            if (Condition)
                action?.Invoke(ParentThis);
            return ParentThis;
        }

        public T DoCatch(Action<T> action)
        {
            try
            {
                if (Condition)
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
