using System.Windows.Controls;

namespace LIT.Views
{
    /// <summary>
    /// Interaction logic for ItinerarySearch
    /// </summary>
    public partial class ItinerarySearchTab : UserControl
    {
        private LIT.Old_LIT.MainWindow _parentWindow;
        public ItinerarySearchTab()
        {
            InitializeComponent();
        }
        public ItinerarySearchTab(LIT.Old_LIT.MainWindow ParentWindow)
        {
            InitializeComponent();
            _parentWindow = ParentWindow;
        }
    }
}
