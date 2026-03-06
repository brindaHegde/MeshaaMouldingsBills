using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Factory.Common.Base
{
    /// <summary>
    /// This base classs implements ViewModelBase & INotifyDataErrorInfo interaces to handle
    /// property change notifications and error handling.
    /// </summary>
    public class ViewModelBaseValidator : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public bool HasErrors => (_propertyErrors != null && _propertyErrors.Count > 0) ? true : false;

        /// <summary>
        /// Event handler delegate for data error handling
        /// </summary>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged = delegate { };

        /// <summary>
        /// Method to handle the error notification
        /// </summary>
        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Method to add the errors to the error dictionary and raise the error notification.
        /// </summary>
        protected void AddError(string error, [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null) return;

            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors[propertyName] = new List<string>();
            }
            if (!_propertyErrors[propertyName].Contains(error))
            {
                _propertyErrors[propertyName].Add(error);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                InvokePropertyChanged(nameof(HasErrors));
            }
        }

        /// <summary>
        /// Method to clean up the errors and raise error notification and property changed event
        /// </summary>
        protected void ClearErrors([CallerMemberName] string propertyName = null)
        {
            if (propertyName is null) return;

            if (_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Remove(propertyName);
                OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName));
                InvokePropertyChanged(nameof(HasErrors));
            }
        }

        /// <summary>
        /// Method returns the list of errors.
        /// </summary>
        public IEnumerable GetErrors(string propertyName)
        {
            return (propertyName != null && _propertyErrors.ContainsKey(propertyName)) ?
                _propertyErrors[propertyName] : Enumerable.Empty<string>();
        }
    }
}
