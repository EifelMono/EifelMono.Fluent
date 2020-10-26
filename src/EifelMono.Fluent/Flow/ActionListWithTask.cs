using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EifelMono.Fluent.Flow
{
    public class ActionListWithTaskItem
    {
        public bool IsTask { get; set; } = false;
        public object VoidOrTaskAction { get; set; } = null;

        public static ActionListWithTaskItem CreateVoidAction(object voidAction)
            => new ActionListWithTaskItem { IsTask = false, VoidOrTaskAction = voidAction };
        public static ActionListWithTaskItem CreateTaskAction(object taskAction)
            => new ActionListWithTaskItem { IsTask = true, VoidOrTaskAction = taskAction };
    }

    public abstract class ActionListWithTaskCore
    {
        private object _lockItems = new object();
        protected List<ActionListWithTaskItem> Items { get; set; } = new List<ActionListWithTaskItem>();

        protected object AddFirst(ActionListWithTaskItem item)
        {
            lock (_lockItems)
            {
                Items.Insert(0, item);
            }
            return item;
        }

        protected object Add(ActionListWithTaskItem item)
        {
            lock (_lockItems)
            {
                Items.Add(item);
            }
            return item;
        }

        public object Remove(ActionListWithTaskItem item)
        {
            lock (_lockItems)
            {
                var removeItem = Items.Find(i => i.VoidOrTaskAction.Equals(item.VoidOrTaskAction));
                if (removeItem is { })
                    Items.Remove(removeItem);
            }
            return item;
        }

        public bool Contains(ActionListWithTaskItem item)
        {
            lock (_lockItems)
                return Items.Any(i => i.VoidOrTaskAction.Equals(item.VoidOrTaskAction));
        }

        public void Clear()
        {
            lock (_lockItems)
            {
                Items.Clear();
            }
        }

        public int Count
        {
            get
            {
                lock (_lockItems)
                    return Items.Count;
            }
        }

        protected List<ActionListWithTaskItem> Clone()
        {
            lock (_lockItems)
                return Items.Select(item => item).ToList();
        }

        protected ActionListWithTaskItem this[int index]
            => Clone()[index];

        protected (int Calls, int Errors) Invoke(Action<ActionListWithTaskItem> actionWithItem)
        {
            int calls = 0;
            int errors = 0;
            Clone().ForEach(item =>
            {
                try
                {
                    calls++;
                    actionWithItem?.Invoke(item);
                }
                catch
                {
                    errors++;
                }
            });
            return (calls, errors);
        }

        protected (int Calls, int Errors) InvokeParallel(Action<ActionListWithTaskItem> actionWithItem)
        {
            object sync = new object();
            int calls = 0;
            int errors = 0;
#if NETSTANDARD1_6
            Clone().ForEach(item =>
            {
                Task.Run(() =>
                {
                    try
                    {
                        lock (sync)
                            calls++;
                            actionWithItem?.Invoke(item);
                    }
                    catch
                    {
                        lock (sync)
                            errors++;
                    }
                });
            });
#else
            Parallel.ForEach(Clone(), item =>
            {
                try
                {
                    lock (sync)
                        calls++;
                    actionWithItem?.Invoke(item);
                }
                catch
                {
                    lock (sync)
                        errors++;
                }
            });
#endif
            return (calls, errors);
        }
    }

    public class ActionListWithTask : ActionListWithTaskCore
    {
        public Action AddFirst(ref Action voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<Task> AddFirst(ref Func<Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action Add(Action voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<Task> Add(Func<Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action Remove(Action voidAction)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<Task> Remove(Func<Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<Task>)?.Invoke();
            else
                (item.VoidOrTaskAction as Action)?.Invoke();
        }

        public (int Calls, int Errors) Invoke()
            => Invoke(item => InvokeAsVoidOrTask(item));
        public (int Calls, int Errors) InvokeParallel()
            => InvokeParallel(item => InvokeAsVoidOrTask(item));
    }

    public class ActionListWithTask<T1> : ActionListWithTaskCore
    {
        public Action<T1> AddFirst(Action<T1> voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, Task> AddFirst(Func<T1, Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1> Add(Action<T1> voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, Task> Add(Func<T1, Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1> Remove(Action<T1> action)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(action));
            return action;
        }

        public Func<T1, Task> Remove(Func<T1, Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action<T1> voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<T1, Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item, T1 arg1)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<T1, Task>)?.Invoke(arg1);
            else
                (item.VoidOrTaskAction as Action<T1>)?.Invoke(arg1);
        }

        public (int Calls, int Errors) Invoke(T1 arg1)
            => Invoke(item => InvokeAsVoidOrTask(item, arg1));
        public (int Calls, int Errors) InvokeParallel(T1 arg1)
            => InvokeParallel(item => InvokeAsVoidOrTask(item, arg1));
    }

    public class ActionListWithTask<T1, T2> : ActionListWithTaskCore
    {
        public Action<T1, T2> AddFirst(Action<T1, T2> voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, Task> AddFirst(Func<T1, T2, Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2> Add(Action<T1, T2> voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, Task> Add(Func<T1, T2, Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2> Remove(Action<T1, T2> action)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(action));
            return action;
        }

        public Func<T1, T2, Task> Remove(Func<T1, T2, Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action<T1, T2> voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<T1, T2, Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item, T1 arg1, T2 arg2)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<T1, T2, Task>)?.Invoke(arg1, arg2);
            else
                (item.VoidOrTaskAction as Action<T1, T2>)?.Invoke(arg1, arg2);
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2)
            => Invoke(item => InvokeAsVoidOrTask(item, arg1, arg2));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2)
            => InvokeParallel(item => InvokeAsVoidOrTask(item, arg1, arg2));
    }

    public class ActionListWithTask<T1, T2, T3> : ActionListWithTaskCore
    {
        public Action<T1, T2, T3> AddFirst(Action<T1, T2, T3> voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, Task> AddFirst(Func<T1, T2, T3, Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3> Add(Action<T1, T2, T3> voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, Task> Add(Func<T1, T2, T3, Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3> Remove(Action<T1, T2, T3> action)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(action));
            return action;
        }

        public Func<T1, T2, T3, Task> Remove(Func<T1, T2, T3, Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action<T1, T2, T3> voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<T1, T2, T3, Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item, T1 arg1, T2 arg2, T3 arg3)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<T1, T2, T3, Task>)?.Invoke(arg1, arg2, arg3);
            else
                (item.VoidOrTaskAction as Action<T1, T2, T3>)?.Invoke(arg1, arg2, arg3);
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3)
            => Invoke(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3)
            => InvokeParallel(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3));
    }

    public class ActionListWithTask<T1, T2, T3, T4> : ActionListWithTaskCore
    {
        public Action<T1, T2, T3, T4> AddFirst(Action<T1, T2, T3, T4> voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, T4, Task> AddFirst(Func<T1, T2, T3, T4, Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3, T4> Add(Action<T1, T2, T3, T4> voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, T4, Task> Add(Func<T1, T2, T3, T4, Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3, T4> Remove(Action<T1, T2, T3, T4> action)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(action));
            return action;
        }

        public Func<T1, T2, T3, T4, Task> Remove(Func<T1, T2, T3, T4, Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action<T1, T2, T3, T4> voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<T1, T2, T3, T4, Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<T1, T2, T3, T4, Task>)?.Invoke(arg1, arg2, arg3, arg4);
            else
                (item.VoidOrTaskAction as Action<T1, T2, T3, T4>)?.Invoke(arg1, arg2, arg3, arg4);
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => Invoke(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3, arg4));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            => InvokeParallel(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3, arg4));
    }

    public class ActionListWithTask<T1, T2, T3, T4, T5> : ActionListWithTaskCore
    {
        public Action<T1, T2, T3, T4, T5> AddFirst(Action<T1, T2, T3, T4, T5> voidAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, T4, T5, Task> AddFirst(Func<T1, T2, T3, T4, T5, Task> taskAction)
        {
            base.AddFirst(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3, T4, T5> Add(Action<T1, T2, T3, T4, T5> voidAction)
        {
            base.Add(ActionListWithTaskItem.CreateVoidAction(voidAction));
            return voidAction;
        }

        public Func<T1, T2, T3, T4, T5, Task> Add(Func<T1, T2, T3, T4, T5, Task> taskAction)
        {
            base.Add(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public Action<T1, T2, T3, T4, T5> Remove(Action<T1, T2, T3, T4, T5> action)
        {
            base.Remove(ActionListWithTaskItem.CreateVoidAction(action));
            return action;
        }

        public Func<T1, T2, T3, T4, T5, Task> Remove(Func<T1, T2, T3, T4, T5, Task> taskAction)
        {
            base.Remove(ActionListWithTaskItem.CreateTaskAction(taskAction));
            return taskAction;
        }

        public bool Contains(Action<T1, T2, T3, T4, T5> voidAction) => base.Contains(ActionListWithTaskItem.CreateVoidAction(voidAction));
        public bool Contains(Func<T1, T2, T3, T4, T5, Task> taskAction) => base.Contains(ActionListWithTaskItem.CreateTaskAction(taskAction));

        private void InvokeAsVoidOrTask(ActionListWithTaskItem item, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (item.IsTask)
                _ = (item.VoidOrTaskAction as Func<T1, T2, T3, T4, T5, Task>)?.Invoke(arg1, arg2, arg3, arg4, arg5);
            else
                (item.VoidOrTaskAction as Action<T1, T2, T3, T4, T5>)?.Invoke(arg1, arg2, arg3, arg4, arg5);
        }

        public (int Calls, int Errors) Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => Invoke(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3, arg4, arg5));
        public (int Calls, int Errors) InvokeParallel(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            => InvokeParallel(item => InvokeAsVoidOrTask(item, arg1, arg2, arg3, arg4, arg5));
    }
}
