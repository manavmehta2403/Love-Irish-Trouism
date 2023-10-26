using System.Windows;
using System.Windows.Controls;

namespace LIT.Views
{
    /// <summary>
    /// Interaction logic for SupplierPaymentSearchTab
    /// </summary>
    public partial class SupplierPaymentSearchTab : UserControl
    {
        private LIT.Old_LIT.MainWindow _parentWindow;

        public SupplierPaymentSearchTab()
        {
            InitializeComponent();
        }

        public SupplierPaymentSearchTab(LIT.Old_LIT.MainWindow ParentWindow)
        {
            InitializeComponent();
            _parentWindow = ParentWindow;
        }

        private Window popupWindow;

        public void Show()
        {
            if (popupWindow == null)
            {
                popupWindow = new Window
                {
                    Content = this,
                    Title = "Supplier Payment Search",
                    SizeToContent = SizeToContent.WidthAndHeight,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                };
                popupWindow.Closed += (sender, e) => { popupWindow = null; };
            }

            popupWindow.Show();
        }
    }
}
