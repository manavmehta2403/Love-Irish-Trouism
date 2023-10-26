using LIT.Core.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LIT.Core.Controls
{
    /// <summary>
    /// Interaction logic for NotificationPopup
    /// </summary>
    public partial class NotificationPopup : UserControl
    {
        public NotificationPopup()
        {
            InitializeComponent();
        }
        public NotificationPopup(NotificationItem notificationItem)
        {
            InitializeComponent();
            Dispatcher.Invoke(() =>
            {
                MessageTextBlock.Text = notificationItem.Message; // Set the message text from the NotificationItem
            });
           
        }

        public NotificationItem NotificationItem { get; internal set; }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            // Close or dismiss the notification popup here
            if (Parent is Popup popup)
            {
                popup.IsOpen = false;
                // Optionally, remove the popup from the collection or perform other actions
            }
        }
    }
}
