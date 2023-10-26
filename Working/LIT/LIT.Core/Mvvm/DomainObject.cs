using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LIT.Core.Mvvm
{
    /// <summary>
    /// Base class for domain objects with validation and data error reporting.
    /// </summary>
    public abstract class DomainObject<T> : BindableBase, INotifyDataErrorInfo where T : DomainObject<T>
    {
        private readonly ErrorsContainer<ValidationResult> errorsContainer;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => errorsContainer.HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return errorsContainer.GetErrors(propertyName);
        }

        protected DomainObject()
        {
            errorsContainer = new ErrorsContainer<ValidationResult>(RaiseErrorsChanged);
        }

        protected void RaiseErrorsChanged([CallerMemberName] string propertyName = null)
        {
            errorsContainer.ClearErrors(propertyName);
            OnErrorsChanged(propertyName);
        }

        private void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected bool SetPropertyWithValidation<TValue>(ref TValue storage, TValue value, [CallerMemberName] string propertyName = null)
        {
            return SetProperty(ref storage, value, () => ValidateProperty(value, propertyName), propertyName);
        }

        // Helper method to validate properties using data annotations
        private void ValidateProperty(object value, string propertyName)
        {
            var validationContext = new ValidationContext(this, null, null) { MemberName = propertyName };
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateProperty(value, validationContext, validationResults))
            {
                errorsContainer.SetErrors(propertyName, validationResults);
            }
        }
    }
}