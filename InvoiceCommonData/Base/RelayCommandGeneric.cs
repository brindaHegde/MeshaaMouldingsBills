using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Factory.Common.Base
{
    public class RelayCommand<T> : ICommand
    {
        Action<T> _ExecuteMethod;
        Func<T, bool> _CanExecuteMethod;

        /// <summary>
        /// Constructor for command implementation
        /// </summary>
        public RelayCommand(Action<T> executeMethod)
        {
            _ExecuteMethod = executeMethod;
        }

        /// <summary>
        /// Overloaded Constructor for command implementation to handle both execute and canexecutemethods
        /// </summary>
        public RelayCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
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
                T param = (T)parameter;
                return _CanExecuteMethod(param);
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
                T param = (T)parameter;
                _ExecuteMethod(param);
            }
        }
        #endregion
    }
}
