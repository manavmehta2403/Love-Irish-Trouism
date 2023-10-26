using Prism.Mvvm;
using System;
using Prism.Ioc;
using System.Runtime.CompilerServices;

namespace LIT.Core.Mvvm
{
    public class PropertyChangeNotifier : BindableBase
    {
        private static NotificationService _notificationService;
        private bool _dataLoaded = false; // Flag to track whether data has been loaded

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
        protected static void ShowNotificationPopup(string propertyName, string customMessage = null)
        {
            // Build the notification message
            string notificationMessage = customMessage ?? $"{propertyName} has changed";

            // Show a notification with the custom message
            NotificationService.ShowNotification(notificationMessage);
        }

        protected virtual void OnPropertyChangedWithNotification<T>(ref T storage, T newValue, [CallerMemberName] string propertyName = null, string customMessage = null)
        {
            T oldValue = storage;
            SetProperty(ref storage, newValue, propertyName);
            PropertyChangedEvent?.Invoke(this, propertyName);

           // if (_dataLoaded && !Equals(oldValue, newValue))
           if(!Equals(oldValue, newValue))
            {
                // Build the notification message
                string notificationMessage = customMessage ?? $"{propertyName} has changed from '{oldValue}' to '{newValue}'";

                // Show a notification with the custom message
                NotificationService.AddNotification(notificationMessage);
            }
            else
            {
                // Data is now loaded
               // _dataLoaded = true;
            }
        }
        // Method to explicitly set the _dataLoaded flag to true when data is initially loaded
        public void SetDataLoaded()
        {
            _dataLoaded = true;
        }

        // Not to get the data for the first time
        protected virtual void OnPropertyChangedWithNotificationAfterLoading<T>(ref T storage, T newValue, [CallerMemberName] string propertyName = null, string customMessage = null)
        {
            T oldValue = storage;
            SetProperty(ref storage, newValue, propertyName);
            PropertyChangedEvent?.Invoke(this, propertyName);

            if (_dataLoaded && !Equals(oldValue, newValue))
            {
                // Build the notification message
                string notificationMessage = customMessage ?? $"{propertyName} has changed from '{oldValue}' to '{newValue}'";

                // Show a notification with the custom message
                NotificationService.AddNotification(notificationMessage);
            }
            else
            {
                // Data is now loaded
                _dataLoaded = true;
            }
        }
    }
}
