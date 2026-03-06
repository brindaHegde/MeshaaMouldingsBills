
using System;
using System.Windows.Input;

namespace Factory.Common
{
    public class RelayCommand : ICommand
    {
        Action _ExecuteMethod;
        Func<bool> _CanExecuteMethod;

        /// <summary>
        /// Constructor for command implementation
        /// </summary>
        public RelayCommand(Action executeMethod)
        {
            _ExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Overloaded Constructor for command implementation to handle both execute and canexecutemethods
        /// </summary>
        public RelayCommand(Action executeMethod, Func<bool> canExecuteMethod)
        {
            _ExecuteMethod = executeMethod;
            _CanExecuteMethod = canExecuteMethod;
        }

        /// <summary>
        /// Method to raise can execute changed event
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged(this, EventArgs.Empty);
        }

        #region ICommand 

        /// <summary>
        /// Method implements canexecute method of ICommand
        /// </summary>
        bool ICommand.CanExecute(object parameter)
        {
            if (_CanExecuteMethod != null)
            {
                return _CanExecuteMethod();
            }
            if (_ExecuteMethod != null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Property for CanExecuteChanged
        /// </summary>
        public event EventHandler CanExecuteChanged = delegate { };

        /// <summary>
        /// Method implements execute method of ICommand
        /// </summary>
        void ICommand.Execute(object parameter)
        {
            if (_ExecuteMethod != null)
            {
                _ExecuteMethod();
            }
        }
        #endregion
    }

}
