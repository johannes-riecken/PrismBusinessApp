

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class DelegateCommand<T> : DelegateCommandBase
    {
        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new DelegateCommand<T>(executeMethod);
        }

        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new DelegateCommand<T>(executeMethod, canExecuteMethod);
        }

        public bool CanExecute(T parameter)
        {
            return base.CanExecute(parameter);
        }

        public async Task Execute(T parameter)
        {
            await base.Execute(parameter);
        }


        private DelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, (o) => true)
        {
        }

        private DelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

    }

    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }

        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod)
        {
            return new DelegateCommand(executeMethod);
        }

        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }

        public async Task Execute()
        {
            await Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }

        private DelegateCommand(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {
        }

        private DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");
        }
    }

}
