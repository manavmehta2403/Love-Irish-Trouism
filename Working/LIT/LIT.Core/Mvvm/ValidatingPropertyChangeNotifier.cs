using Prism.Mvvm;
using System;
using Prism.Ioc;
using System.Runtime.CompilerServices;

namespace LIT.Core.Mvvm
{
    public class ValidatingPropertyChangeNotifier<T> : DomainObject<T> where T : DomainObject<T>
    {
        private static NotificationService _notificationService;

        // Ensure the notification service is resolved only once
        private static NotificationService NotificationService
        {
            get
            {
                _notificationService ??= (NotificationService)ContainerLocator.Container.Resolve(typeof(NotificationService));
                return _notificationService;
            }
        }

        public event EventHandler<string> PropertyChangedEvent;

        protected virtual void OnPropertyChanged<TValue>(ref TValue storage, TValue value, [CallerMemberName] string propertyName = null)
        {
            SetPropertyWithValidation(ref storage, value, propertyName);
            PropertyChangedEvent?.Invoke(this, propertyName);

            // Show a notification when a property changes
            NotificationService.ShowNotification($"{propertyName} has changed to {value}");
        }
    }
}
