using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LIT.Core.Controls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool _isPasswordChanging;

        /// <summary>
        /// Password dependency property
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    PasswordPropertyChanged, null, false, UpdateSourceTrigger.PropertyChanged));

        /// <summary>
        /// Password property changed callback
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox passwordBox)
            {
                passwordBox.UpdatePassword();
            }
        }

        /// <summary>
        /// Password property
        /// </summary>
        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        /// <summary>
        /// ctor for BindablePasswordBox
        /// </summary>
        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This method is called when the password box's password changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _isPasswordChanging = true;
            Password = passwordBox.Password;
            _isPasswordChanging = false;
        }

        /// <summary>
        /// This method is called when the password property is changed.
        /// </summary>
        private void UpdatePassword()
        {
            if (!_isPasswordChanging)
            {
                passwordBox.Password = Password;
            }
        }
    }
}