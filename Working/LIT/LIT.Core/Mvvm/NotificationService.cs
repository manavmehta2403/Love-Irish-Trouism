using LIT.Core.Controls;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LIT.Core.Mvvm
{
    public class NotificationItem
    {
        public Guid NotificationId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }

    public class NotificationService
    {
        private readonly List<Popup> _popups;
        private const int NotificationDisplayTimeMilliseconds = int.MaxValue; // 24 days 

        public ObservableCollection<NotificationItem> Notifications { get; } = new ObservableCollection<NotificationItem>();

        public NotificationService()
        {
            _popups = new List<Popup>();
        }

        public void ShowNotification(string message, string customMessage = null)
        {
            string notificationMessage = customMessage ?? message;
            DateTime timestamp = DateTime.Now;
            string formattedTimestamp = timestamp.ToString("dd/MM/yyyy HH:mm");
            var notificationItem = new NotificationItem { Message = notificationMessage, Timestamp = formattedTimestamp };

            var notificationPopup = CreateNotificationPopup(notificationItem);
            var popup = CreatePopup(notificationPopup);

            ClosePopupAfterDelay(popup);
        }

        public void AddNotification(string message, string customMessage = null)
        {
            // Play a beep sound
            SystemSounds.Beep.Play();

            string notificationMessage = customMessage ?? message;
            DateTime timestamp = DateTime.Now;
            string formattedTimestamp = timestamp.ToString("dd/MM/yyyy HH:mm");
            var notificationItem = new NotificationItem { Message = notificationMessage, Timestamp = formattedTimestamp };

            Application.Current.Dispatcher.Invoke(() =>
            {
                Notifications.Add(notificationItem);
            });

            //Notifications.Add(notificationItem);
            var notificationPopup = CreateNotificationPopup(notificationItem);
            var popup = CreatePopup(notificationPopup);
            ShowNotification(message, customMessage);
            SubscribeToCloseButtonClick(popup, notificationPopup);

            _popups.Add(popup);
            popup.IsOpen = true;

            // Keep the popup open when it loses focus
            popup.LostFocus += (sender, e) =>
            {
                // Check if the related target is null or not a descendant of the popup
                if (e.OriginalSource == null || !((UIElement)e.OriginalSource).IsDescendantOf(notificationPopup))
                {
                    popup.Focus();
                }
            };

            ClosePopupAfterDelay(popup);
        }

        public void MarkNotificationAsRead(Guid notificationId, Guid userId)
        {
            // Call your DAL method to mark the notification as read in the database
            var notificationDal = new NotificationDal();
            notificationDal.MarkNotificationAsRead(notificationId, userId);

            // Optionally, you can update the UI to reflect that the notification is marked as read
            var notificationItem = Notifications.FirstOrDefault(n => n.NotificationId == notificationId);
        }

        public void ClearAllNotifications()
        {
            Notifications.Clear();
        }

        public void InitializeBackgroundTask(Guid userId)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        List<Notification> newNotifications = await FetchNotificationsAsync(userId);

                        // Marshal UI-related code to the UI thread
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (newNotifications.Count > 0)
                            {
                                foreach (var notification in newNotifications)
                                {
                                    //Booking "Confirmed" for ##Booking Name## by ##Supplier Name##
                                    //Booking "Declined" for ##Booking Name## by ##Supplier Name##
                                    //Booking "Confirmed" but price to be corrected for ##Booking Name## by ##Supplier Name##
                                    string message = (bool)notification.IsPriceChanged ?
                                    $"Booking {notification.NotificationTitle} but price to be corrected for {notification.BookingName} by {notification.SupplierName}"
                                    : $"Booking {notification.NotificationTitle} for {notification.BookingName} by {notification.SupplierName}";
                                    AddNotification(notification.NotificationTitle, $"{message}");
                                    MarkNotificationAsRead(notification.BubbleNotificationId, notification.TargetUserId);
                                }
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions here, log them or take appropriate action
                    }

                    await Task.Delay(TimeSpan.FromMinutes(1)); // Fetch notifications every 1 minute
                }
            });
        }



        private static NotificationPopup CreateNotificationPopup(NotificationItem notificationItem)
        {
            return new NotificationPopup(notificationItem);
        }

        private static Popup CreatePopup(NotificationPopup notificationPopup)
        {
            var popup = new Popup
            {
                Placement = PlacementMode.RelativePoint,
                PlacementTarget = Application.Current.MainWindow,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade,
                StaysOpen = true,
                Child = notificationPopup
            };

            return popup;
        }

        private void SubscribeToCloseButtonClick(Popup popup, NotificationPopup notificationPopup)
        {
            notificationPopup.CloseButton.Click += (sender, args) =>
            {
                popup.IsOpen = false;
                _popups.Remove(popup);
                Notifications.Remove(notificationPopup.NotificationItem);
            };
        }

        private async Task<List<Notification>> FetchNotificationsAsync(Guid userId)
        {
            // Use the NotificationDal to fetch notifications from the database
            var notificationDal = new NotificationDal();
            return await Task.Run(() => notificationDal.GetNotifications(userId));
        }

        public void ClosePopupsOnApplicationExit()
        {
            foreach (var popup in _popups.ToList())
            {
                popup.IsOpen = false;
                _popups.Remove(popup);

                Notifications.Remove(((NotificationPopup)popup.Child).NotificationItem);
            }
        }

        private void ClosePopupAfterDelay(Popup popup)
        {
            Task.Delay(NotificationDisplayTimeMilliseconds).ContinueWith(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    popup.IsOpen = false;
                    _popups.Remove(popup);
                    Notifications.Remove(((NotificationPopup)popup.Child).NotificationItem);
                });
            });
        }
    }
}
