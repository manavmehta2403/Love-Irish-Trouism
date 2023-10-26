using LIT.Modules.TabControl.ViewModels;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LIT.Modules.TabControl.Views
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ItineraryFollowUpTab : UserControl
    {
        // FollowupViewModel FUviewModel=new FollowupViewModel();
        public ItineraryFollowUpTabViewModel FollowupViewModel { get; set; }

        public ItineraryFollowUpTab()
        {
            InitializeComponent();

            // 
            // this.FollowupViewModel = new FollowupViewModel();

            //  dgFollowupTask.DataContext = this.FollowupViewModel.Folluptask;
            // dgFollowupTask.DataContext = FUviewModel.Folluptask;
        }

        private void dgFollowupTaskDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnflupEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void btnAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    AddItem();
        //   // this.FollowupViewModel.AddtaskCommand.Execute();
        //   // dgFollowupTask.DataContext = this.FollowupViewModel.Folluptask;
        //}

        //private ObservableCollection<FollowupViewModel> _Folluptask;
        //public ObservableCollection<FollowupViewModel> Folluptask
        //{
        //    get { return _Folluptask ?? (_Folluptask = new ObservableCollection<FollowupViewModel>()); }
        //    set
        //    {
        //        _Folluptask = value;
        //    }
        //}





    }

    public class EmptyCollectionToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as ICollection;
            if (collection != null && collection.Count == 0)
                return null;
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}