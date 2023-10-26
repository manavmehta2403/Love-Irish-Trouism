using LITModels;
using SQLDataAccessLayer.DAL;
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
    /// Interaction logic for BookingMargin.xaml
    /// </summary>
    public partial class BookingMargin : Window
    {
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        DBConnectionEF DBconnEF = new DBConnectionEF();
        CommonAndCalcuation CommonValues = new CommonAndCalcuation();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        ItineraryWindow IWparentwindowdt;       
        Errorlog errobj = new Errorlog();
        public BookingMargin()
        {
            InitializeComponent();
        }
        public BookingMargin(string username, ItineraryWindow IWparentwindow)
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername = username.Trim();
            BookingItemsitindata = IWparentwindow.BookingItemsitin;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            IWparentwindowdt = IWparentwindow;           
        }

        private ObservableCollection<BookingItems> _BookingItemsitindata;
        public ObservableCollection<BookingItems> BookingItemsitindata
        {
            get { return _BookingItemsitindata ?? (_BookingItemsitindata = new ObservableCollection<BookingItems>()); }
            set
            {
                _BookingItemsitindata = value;
            }
        }
        private void btnMarginOk_Click(object sender, RoutedEventArgs e)
        {
            decimal Grossadjmarginval=Convert.ToDecimal((!string.IsNullOrEmpty(txtOverrideAllmargin.Text))?txtOverrideAllmargin.Text:0);
            foreach (BookingItems items in BookingItemsitindata)
            {
                items.GrossAdj= (decimal)DBconnEF.GrossAdjCalculation(items.Grossfinal, Grossadjmarginval);
            }
            IWparentwindowdt.txtmarginpercenOverrideall.Text=Grossadjmarginval.ToString("0.00")+" %";
            IWparentwindowdt.txtMargingrossprice.Text = IWparentwindowdt.itincurformat + "  " + (BookingItemsitindata.Sum(x => x.GrossAdj)).ToString("0.00");
            IWparentwindowdt.BookingItemsitin = BookingItemsitindata;
            IWparentwindowdt.ReteriveBookingItems();
            this.Close();
        }

        private void btnMarginCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
