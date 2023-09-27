

using System.Windows.Input;
using System;
using System.Threading.Tasks;

namespace Microsoft.Practices.Prism.StoreApps
{
    public abstract class DelegateCommandBase : ICommand
    {
        private readonly Func<object, Task> _executeMethod;
        private readonly Func<object, bool> _canExecuteMethod;

        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = (arg) => { executeMethod(arg); return Task.Delay(0); };
            _canExecuteMethod = canExecuteMethod;
        }

        protected DelegateCommandBase(Func<object,Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
                throw new ArgumentNullException("executeMethod");

            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }
        
        protected virtual void OnCanExecuteChanged()
        {
            var handlers = CanExecuteChanged;
            if (handlers != null)
            {
                handlers(this, EventArgs.Empty);
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged();
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        protected async Task Execute(object parameter)
        {
            await _executeMethod(parameter);  
        }

        protected bool CanExecute(object parameter)
        {
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        public event EventHandler CanExecuteChanged;

    }
}