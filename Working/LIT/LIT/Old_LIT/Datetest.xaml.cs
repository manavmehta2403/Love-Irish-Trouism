using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for Datetest.xaml
    /// </summary>
    public partial class Datetest : Window
    {
        private ObservableCollection<SupplierServiceRatesDt> _SupplierSRatesDt;
        public ObservableCollection<SupplierServiceRatesDt> SupplierSRatesDt
        {
            get { return _SupplierSRatesDt ?? (_SupplierSRatesDt = new ObservableCollection<SupplierServiceRatesDt>()); }
            set
            {
                _SupplierSRatesDt = value;
            }
        }
        public Datetest()
        {
            InitializeComponent();
            this.DataContext = this;
            Addrates();
        }

        private void BtnRatesAdd_Click(object sender, RoutedEventArgs e)
        {
            Addrates();
        }

        private void Addrates()
        {
            SupplierServiceRatesDt ssRates;
            ssRates = new SupplierServiceRatesDt();
            ssRates.ValidFrom = DateTime.Now.Date;
            ssRates.IsActive = true;
            ssRates.SupplierServiceDetailsRateId = (Guid.NewGuid()).ToString();
            SupplierSRatesDt.Add(ssRates);
            dgtest.ItemsSource = SupplierSRatesDt.ToList();
        }
    }
}
