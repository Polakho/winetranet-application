using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace winetranet_application.ViewModel
{
    class ViewModelCommand : ICommand
    {
        // Fields
        public readonly Action<object> _executeAction;

        public readonly Predicate<object> _canExecuteAction;

        // Constructors
        public ViewModelCommand(object v, Action<object> executeAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = null;
        }

        public ViewModelCommand(Action<object> executeAction, Predicate<object> canExecuteAction)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        // Events
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value;  }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private void CommandManager_RequerySuggested(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        // Methods
        public bool CanExecute(object? parameter)
        {
            return _canExecuteAction == null ? true : _canExecuteAction(parameter);

        }

        public void Execute(object? parameter)
        {
            _executeAction(parameter);
        }
    }
}
