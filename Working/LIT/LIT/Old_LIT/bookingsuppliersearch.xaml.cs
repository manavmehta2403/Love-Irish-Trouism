//using Microsoft.Windows.Controls;
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
using static SQLDataAccessLayer.Models.BookingModel;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using LITModels;
using LITModels.LITModels.Models;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for BookingSupplierSearch.xaml
    /// </summary>
    public partial class BookingSupplierSearch : Window
    {
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        DBConnectionEF DBconnEF = new DBConnectionEF();
        CommonAndCalcuation CommonValues = new CommonAndCalcuation();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        string PaymentDueDatevalue = string.Empty;
        List<CommonValueCountrycity> ListofRegion = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofCity = new List<CommonValueCountrycity>();
        List<SupplierServiceType> ListofSupplierServiceType { get; set; }

        SupplierServiceModels objSuppservicemdl = new SupplierServiceModels();
        List<SupplierServiceModels> ListofSuppservice = new List<SupplierServiceModels>();
        List<SupplierServiceRatesDt> ListofSuppserviceRates = new List<SupplierServiceRatesDt>();
        List<SupplierPricingOption> ListofPricingOption = new List<SupplierPricingOption>();
        List<SupplierServiceWarning> ListofSuppwarning = new List<SupplierServiceWarning>();
        List<SupplierServiceWarning> ListofServicewarning = new List<SupplierServiceWarning>();

        List<CommonValueList> ListofGroupinfo = new List<CommonValueList>();
        List<Currencydetails> ListofCurrency = new List<Currencydetails>();
        List<SupplierPriceRateEdit> ListofPricedit = new List<SupplierPriceRateEdit>();
        List<Currencydetails> ListofCurrencyServiceidwise = new List<Currencydetails>();

        ItineraryWindow IWparentwindowdt;
        string SelectedSupplierID = string.Empty;
        List<BookingSupplierServiceModels> listBkgsupplierserv = new List<BookingSupplierServiceModels>();
        Errorlog errobj = new Errorlog();
        public BookingSupplierSearch()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public BookingSupplierSearch(string username, ItineraryWindow IWparentwindow, string SelectedSupplierIDval = null)
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername = username.Trim();
            TxtSupplierName.Focus();
            BookingItems = IWparentwindow.BookingItemsitin;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            IWparentwindowdt = IWparentwindow;
            SelectedSupplierID = SelectedSupplierIDval;
            Loadcmbvalues();
            LoadSupplierService();
        }



        /* ObservableCollection start */
        private ObservableCollection<SelectedSupplierBooking> _SupplierSelectedbookingSM;
        public ObservableCollection<SelectedSupplierBooking> SupplierSelectedbookingSM
        {
            get { return _SupplierSelectedbookingSM ?? (_SupplierSelectedbookingSM = new ObservableCollection<SelectedSupplierBooking>()); }
            set
            {
                _SupplierSelectedbookingSM = value;
            }
        }

        private ObservableCollection<BookingItems> _BookingItems;
        public ObservableCollection<BookingItems> BookingItems
        {
            get { return _BookingItems ?? (_BookingItems = new ObservableCollection<BookingItems>()); }
            set
            {
                _BookingItems = value;
            }
        }

        private ObservableCollection<SupplierServiceModels> _SupplierSM;
        public ObservableCollection<SupplierServiceModels> SupplierSM
        {
            get { return _SupplierSM ?? (_SupplierSM = new ObservableCollection<SupplierServiceModels>()); }
            set
            {
                _SupplierSM = value;
            }
        }

        private ObservableCollection<SupplierServiceRatesDt> _SupplierSRatesDt;
        public ObservableCollection<SupplierServiceRatesDt> SupplierSRatesDt
        {
            get { return _SupplierSRatesDt ?? (_SupplierSRatesDt = new ObservableCollection<SupplierServiceRatesDt>()); }
            set
            {
                _SupplierSRatesDt = value;
            }
        }

        private ObservableCollection<SupplierPricingOption> _PricingOptionDt;
        public ObservableCollection<SupplierPricingOption> PricingOptionDt
        {
            get { return _PricingOptionDt ?? (_PricingOptionDt = new ObservableCollection<SupplierPricingOption>()); }
            set
            {
                _PricingOptionDt = value;
            }
        }

        private ObservableCollection<SupplierPriceRateEdit> _PriceEditDt;
        public ObservableCollection<SupplierPriceRateEdit> PriceEditDt
        {
            get { return _PriceEditDt ?? (_PriceEditDt = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEditDt = value;
            }
        }

        /* ObservableCollection end */
        private void Loadcmbvalues()
        {

            try
            {
                CommonValueCountrycity objCVCC
                 = new CommonValueCountrycity();

                ListofRegion = loadDropDownListValues.LoadCommonValuesRegion("Regionnostateid", objCVCC);
                if (ListofRegion != null && ListofRegion.Count > 0)
                {
                    CmbRegion.ItemsSource = ListofRegion;
                    CmbRegion.SelectedValuePath = "RegionId";
                    CmbRegion.DisplayMemberPath = "RegionName";

                }

                ListofCity = loadDropDownListValues.LoadCommonValuesCity("City", objCVCC);
                if (ListofCity != null && ListofCity.Count > 0)
                {
                    CmbCity.ItemsSource = ListofCity;
                    CmbCity.SelectedValuePath = "CityId";
                    CmbCity.DisplayMemberPath = "CityName";
                }


                SupplierServiceType objSST
                   = new SupplierServiceType();
                ListofSupplierServiceType = new List<SupplierServiceType>();
                ListofSupplierServiceType = loadDropDownListValues.LoadSupplierServiceTypes();
                if (ListofSupplierServiceType != null && ListofSupplierServiceType.Count > 0)
                {
                    CmbServiceType.ItemsSource = ListofSupplierServiceType;
                    CmbServiceType.SelectedValuePath = "ServiceTypeID";
                    CmbServiceType.DisplayMemberPath = "ServiceTypeName";

                    CmbSupplierServiceType.ItemsSource = ListofSupplierServiceType;
                    CmbSupplierServiceType.SelectedValuePath = "ServiceTypeID";
                    CmbSupplierServiceType.DisplayMemberPath = "ServiceTypeName";
                }

                ListofGroupinfo = new List<CommonValueList>();
                ListofGroupinfo = loadDropDownListValues.LoadGroupinfo("Group Info");
                if (ListofGroupinfo != null && ListofGroupinfo.Count > 0)
                {
                    CmbChargesgroupinfo.ItemsSource = ListofGroupinfo;
                    CmbChargesgroupinfo.SelectedValuePath = "ValueField";
                    CmbChargesgroupinfo.DisplayMemberPath = "TextField";
                }

                ListofCurrency = new List<Currencydetails>();
                ListofCurrency = loadDropDownListValues.LoadCurrencyDetails();
                if (ListofCurrency != null && ListofCurrency.Count > 0)
                {
                    CmbCurrencyDetails.ItemsSource = ListofCurrency;
                    CmbCurrencyDetails.SelectedValuePath = "CurrencyID";
                    CmbCurrencyDetails.DisplayMemberPath = "CurrencyName";
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void clearvalues()
        {
            CmbServiceType.SelectedValue = null;
            CmbCity.SelectedValue = null;
            CmbRegion.SelectedValue = null;
            TxtSupplierName.Text = string.Empty;
        }

        private void LoadSupplierService()
        {
            try
            {
                string SupplierName = string.Empty; Guid City, Region, ServiceType, SelectedSupplieridval;
                SupplierName = TxtSupplierName.Text.Trim();
                City = (CmbCity.SelectedValue == null) ? Guid.Empty : Guid.Parse(CmbCity.SelectedValue.ToString());
                ServiceType = (CmbServiceType.SelectedValue == null) ? Guid.Empty : Guid.Parse(CmbServiceType.SelectedValue.ToString());
                Region = (CmbRegion.SelectedValue == null) ? Guid.Empty : Guid.Parse(CmbRegion.SelectedValue.ToString());
                SelectedSupplieridval = ((string.IsNullOrEmpty(SelectedSupplierID))) ? Guid.Empty : Guid.Parse(SelectedSupplierID.ToString());


                listBkgsupplierserv = objsupdal.BookingSupplierServiceRetrive(SupplierName, ServiceType, City, Region, SelectedSupplieridval);
                if (listBkgsupplierserv != null && listBkgsupplierserv.Count > 0)
                {
                    if (listBkgsupplierserv.Count > 0)
                    {
                        dgBkgSupplierService.ItemsSource = listBkgsupplierserv;
                    }
                    else
                    {
                        dgBkgSupplierService.ItemsSource = null;
                    }
                }
                else
                {
                    dgBkgSupplierService.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LoadSupplierService();
        }

        private void TxtSupplierName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            LoadSupplierService();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearvalues();
        }

        private void SelectSupplierServicecall()
        {
            BookingSupplierServiceModels ssmbkg = dgBkgSupplierService.SelectedItem as BookingSupplierServiceModels;
            if (ssmbkg != null)
            {
                TbSelectforbooking.IsEnabled = true;
                TbSelectforbooking.IsSelected = true;
                TbSuppServiceSrch.IsSelected = false;
                tbcntBkgsrch.SelectedIndex = 1;
                hdnSupplierid.Text = ssmbkg.SupplierID;
                ReteriveSupplierServices(ssmbkg.SupplierID, ssmbkg.ServiceID);
                ServiceRateCallpage();
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a Supplier and Service, then click on \"Select\" button...!");
                return;
            }
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SelectSupplierServicecall();
        }
        private void dgBkgSupplierServiceDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TbSelectforbooking.IsEnabled = true;
            TbSelectforbooking.IsSelected = true;

            SelectSupplierServicecall();
            tbcntBkgsrch.SelectedIndex = 1;
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        /* Tab 2 code start       */


        private void msPenLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#579F00"));
        }

        private void msPenEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#79D10D"));
        }
        private void msEnter(object sender, MouseEventArgs e)
        {

            ((Button)sender).Background = (Brush)(new BrushConverter().ConvertFrom("#FF3C6A05"));
        }

        private void msLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Background = (Brush)(new BrushConverter().ConvertFrom("#579F00"));
        }

        private void msCrossEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#C90A37"));
        }

        private void msCrossLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF003D"));
        }


        private void dgSupplierServicesBkssDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ServiceRateCallpage();

            e.Handled = true;
        }

        private void ServiceRateCallpage()
        {
            try
            {
                SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                if (ssmobj != null)
                {
                    if (ssmobj.ServiceName.ToString().Trim() == string.Empty)
                    {
                        System.Windows.MessageBox.Show("Please provide a Service Name");
                        return;
                    }
                    if (ssmobj.IsActive == false)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Selected Service is Non-Active, Do you really want to proceed?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {

                            //SupplierRates.IsEnabled = true;
                            // SupplierRates.IsSelected = true;
                            lblserviceName.Visibility = Visibility.Visible;
                            lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                            ListofSuppserviceRates = null;
                            SupplierSRatesDt = null;
                            PricingOptionDt = null;
                            chbhideexpiredseasons.IsChecked = true;
                            ReteriveSupplierServicesRates(ssmobj.ServiceId);
                            PricingCallpage();

                        }
                    }
                    else
                    {

                        //SupplierRates.IsEnabled = true;
                        //SupplierRates.IsSelected = true;
                        lblserviceName.Visibility = Visibility.Visible;
                        lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                        ListofSuppserviceRates = null;
                        SupplierSRatesDt = null;
                        PricingOptionDt = null;
                        chbhideexpiredseasons.IsChecked = true;
                        ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        PricingCallpage();

                    }
                }
                else
                {
                    dgSupplierServicesBkssRates.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void btnServiceRatesbkss_Click(object sender, RoutedEventArgs e)
        {
            ServiceRateCallpage();
        }

        private void PricingCallpage()
        {
            try
            {
                SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                if (ssmobj == null)
                {
                    System.Windows.MessageBox.Show("Please select Service");
                    return;
                }
                if (ssmobj.ServiceName.ToString().Trim() == string.Empty)
                {
                    System.Windows.MessageBox.Show("Please provide a Service Name");
                    return;
                }
                //SupplierPricingOptions.IsEnabled = true;
                // SupplierPricingOptions.IsSelected = true;
                lblserviceName.Visibility = Visibility.Visible;
                lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();

                if (ssmobjrate != null)
                {
                    lblRatesDate.Visibility = Visibility.Visible;
                    lblRatesDate.Content = "From: " + ssmobjrate.ValidFrom.ToShortDateString() + "  To: " + ssmobjrate.ValidTo.ToShortDateString();
                    ReterivePricingOption();
                }
                else
                {
                    dgPricingoption.ItemsSource = null;
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void btnPricingbkss_Click(object sender, RoutedEventArgs e)
        {
            PricingCallpage();
        }
        private void dgSupplierServicesBkssRatesDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PricingCallpage();
            e.Handled = true;
        }
        private void chbhidenonactive_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgSupplierServicesBkss != null)
                {
                    SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                    //chbhidenonactivePrice.IsChecked = chbhidenonactive.IsChecked;


                    if (chbhidenonactive.IsChecked == false)
                    {

                        if (ssmobj != null)
                        {
                            ReteriveSupplierServices(ssmobj.SupplierId);
                            ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        }
                        dgSupplierServicesBkss.Columns[2].Visibility = Visibility.Visible;
                        if (dgPricingoption != null)
                        {
                            ReterivePricingOption();

                            dgPricingoption.Columns[4].Visibility = Visibility.Visible;
                        }
                    }
                    if (chbhidenonactive.IsChecked == true)
                    {
                        if (ssmobj != null)
                        {
                            ReteriveSupplierServices(ssmobj.SupplierId);
                            ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        }
                        dgPricingoption.ItemsSource = null;
                        //dgSupplierServicesBkssRates.ItemsSource = null;
                        dgSupplierServicesBkss.Columns[2].Visibility = Visibility.Hidden;
                        dgPricingoption.Columns[4].Visibility = Visibility.Hidden;
                        //if (ssmobj != null)
                        //{
                        //    ReteriveWarningSupplier(ssmobj.ServiceId);
                        //    ReteriveWarningService(ssmobj.ServiceId);
                        //    ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        //}
                        //dgSupplierServicesBkss.Columns[2].Visibility = Visibility.Hidden;                    
                        //if (dgPricingoption != null)
                        //{
                        //    ReterivePricingOption();
                        //    ReterivePriceEditRate();
                        //    
                        //}
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        //private void chbhidenonactivePrice_Click(object sender, RoutedEventArgs e)
        //{

        //    try { 
        //    if (dgSupplierServicesBkss != null)
        //    {
        //        chbhidenonactive.IsChecked = chbhidenonactivePrice.IsChecked;
        //        ReteriveSupplierServices(hdnSupplierid.Text.Trim());


        //        if (chbhidenonactivePrice.IsChecked == true)
        //        {
        //            dgSupplierServicesBkss.Columns[2].Visibility = Visibility.Hidden;
        //            if (dgPricingoption != null)
        //            {
        //                ReterivePricingOption();
        //                dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
        //            }
        //        }
        //        if (chbhidenonactivePrice.IsChecked == false)
        //        {
        //            dgSupplierServicesBkss.Columns[2].Visibility = Visibility.Visible;
        //            if (dgPricingoption != null)
        //            {
        //                ReterivePricingOption();
        //                dgPricingoption.Columns[3].Visibility = Visibility.Visible;
        //            }
        //        }
        //    }
        //    }
        //    catch (Exception ex)
        //    {
        //        errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
        //    }
        //}

        //private void chbhideexpiredseasonsPrice_Click(object sender, RoutedEventArgs e)
        //{
        //    SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;            
        //    if (ssmobj != null)
        //    {
        //        ReteriveSupplierServicesRates(ssmobj.ServiceId);
        //        ReterivePricingOption();
        //    }
        //    else
        //    {
        //        System.Windows.MessageBox.Show("Please select Service");
        //        return;
        //    }

        //}
        private void chbhideexpiredseasons_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            //chbhideexpiredseasonswarning.IsChecked = chbhideexpiredseasons.IsChecked;
            //chbhideexpiredseasonsPrice.IsChecked = chbhideexpiredseasons.IsChecked;
            if (ssmobj != null)
            {
                ReteriveSupplierServicesRates(ssmobj.ServiceId);
                ReteriveWarningSupplier(ssmobj.ServiceId);
                ReteriveWarningService(ssmobj.ServiceId);
                ReterivePricingOption();
                ReterivePriceEditRate();
            }
            else
            {
                System.Windows.MessageBox.Show("Please select Service");
                return;
            }
        }


        public void ReteriveSupplierServices(string SupplierIdval, string ServiceId = null)
        {
            try
            {
                if (SupplierIdval != "")
                {
                    // List<SupplierServiceModels> ListofSuppservice = new List<SupplierServiceModels>();
                    //ListofSuppservice = null;
                    //SupplierSM = null;
                    //dgSupplierServicesBkss.ItemsSource = null;
                    ListofSuppservice = objsupdal.SupplierServiceRetrive(Guid.Parse(SupplierIdval));
                    if (ListofSuppservice != null && ListofSuppservice.Count > 0)
                    {
                        foreach (SupplierServiceModels sup in ListofSuppservice)
                        {
                            if (SupplierSM.Where(x => x.ServiceId == sup.ServiceId).Count() == 0)
                            {
                                sup.SelectedItem = ListofSupplierServiceType.Where(x => x.ServiceTypeID == sup.Type).FirstOrDefault();
                                sup.SelectedItemCurrency = ListofCurrency.Where(x => x.CurrencyID == sup.Currency).FirstOrDefault();
                                sup.SelectedItemGroupInfo = ListofGroupinfo.Where(x => x.ValueField.ToString() == sup.Groupinfo).FirstOrDefault();
                                
                                SupplierSM.Add(sup);
                            }

                        }
                        // SupplierSM.ToList().ForEach(x => { x.SelectedItemRowvalue = null; x.SelectedItemRow = 0; });
                        //int k = SupplierSM.Where(x => x.SupplierId == SupplierIdval).ToList().FindIndex(x => x.ServiceId == ServiceId.ToString());
                        //if (k > 0)
                        //{
                        //    SelectRowByIndex(dgSupplierServicesBkss, k);
                        //    SupplierSM.Where(x => x.SupplierId == SupplierIdval && x.ServiceId == ServiceId.ToString()).FirstOrDefault().SelectedItemRow = k;
                        //    SupplierSM.Where(x => x.SupplierId == SupplierIdval && x.ServiceId == ServiceId.ToString()).FirstOrDefault().SelectedItemRowvalue = SupplierSM[k];
                        //}

                        // lstSupplierSM.Add(SupplierSM);
                        // ListofSuppservice

                        
                        if (chbhidenonactive.IsChecked == false)
                        {
                            dgSupplierServicesBkss.ItemsSource = SupplierSM.Where(x => x.SupplierId == SupplierIdval).ToList();
                            int k = SupplierSM.Where(x => x.SupplierId == SupplierIdval).ToList().FindIndex(x => x.ServiceId == ServiceId.ToString());
                            if (k >= 0)
                            {
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkss, k);
                            }
                        }
                        else
                        {
                            
                            dgSupplierServicesBkss.ItemsSource = SupplierSM.Where(x => x.IsActive == true && x.SupplierId == SupplierIdval).ToList();
                            int k = SupplierSM.Where(x => x.IsActive == true && x.SupplierId == SupplierIdval).ToList().FindIndex(x => x.ServiceId == ServiceId.ToString());
                            if (k >= 0)
                            {
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkss, k);
                            }
                        }

                    }
                    if ((ListofSuppservice == null || ListofSuppservice.Count == 0) && SupplierSM.Count==0)
                    {
                        dgSupplierServicesBkss.ItemsSource = SupplierSM;
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

     
        public void ReteriveSupplierServicesRates(string SupplierServiceIdval)
        {
            try
            {
                if (SupplierServiceIdval != "")
                {
                    //ListofSuppserviceRates = null;
                    //SupplierSRatesDt = null;
                    //dgSupplierServicesBkssRates.ItemsSource = null;                   
                    ListofSuppserviceRates = objsupdal.SupplierServiceRateDtRetrive(Guid.Parse(SupplierServiceIdval));
                    if (ListofSuppserviceRates != null && ListofSuppserviceRates.Count > 0)
                    {
                        foreach (SupplierServiceRatesDt sup in ListofSuppserviceRates)
                        {
                            if (SupplierSRatesDt.Where(x => x.SupplierServiceDetailsRateId == sup.SupplierServiceDetailsRateId).Count() == 0)
                            {
                                SupplierSRatesDt.Add(sup);
                            }
                        }
                        //RatecheckExpireactive(SupplierServiceIdval);
                    }
                   
                    if ((ListofSuppserviceRates != null && ListofSuppserviceRates.Count > 0) || (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).Count() > 0))
                    {
                        RatecheckExpireactive(SupplierServiceIdval);
                    }
                    if ((ListofSuppserviceRates == null || ListofSuppserviceRates.Count == 0) && (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).Count() == 0))
                    {
                        dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceDetailsRateId == SupplierServiceIdval).ToList();
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        public void ReterivePricingOption()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                //ListofPricingOption = null;
                //PricingOptionDt = null;
                //dgPricingoption.ItemsSource = null;
                if (ssRateobj != null)
                {

                    ListofPricingOption = objsupdal.PricingOptionRetrive(Guid.Parse(ssRateobj.SupplierServiceId), Guid.Parse(ssRateobj.SupplierServiceDetailsRateId));
                    ListofCurrencyServiceidwise = loadDropDownListValues.CurrencyinfoReterive(ssRateobj.SupplierServiceId.ToString());
                    if (ListofPricingOption != null && ListofPricingOption.Count > 0)
                    {
                        foreach (SupplierPricingOption sup in ListofPricingOption)
                        {
                            if (PricingOptionDt.Where(x => x.PricingOptionId == sup.PricingOptionId).Count() == 0)
                            {
                                if (ListofCurrencyServiceidwise.Count > 0)
                                {
                                    sup.CurrencyName = ListofCurrencyServiceidwise[0].CurrencyName;
                                    sup.CurrencyDisplayFormat = ListofCurrencyServiceidwise[0].DisplayFormat;
                                }
                                sup.selectedforbkg = false;
                                PricingOptionDt.Add(sup);
                                //ReterivePricingEditRate(sup.SupplierServiceId, sup.PricingOptionId);
                            }
                        }
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }

                    if (PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() > 0)
                    {
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                   if ((PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count()==0)
                            && (ListofPricingOption == null || ListofPricingOption.Count == 0))
                    {
                        dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId);
                    }
                    if ((ListofPricingOption == null || ListofPricingOption.Count == 0) && (PricingOptionDt.Count==0 || PricingOptionDt == null))
                    {
                        dgPricingoption.ItemsSource = PricingOptionDt;
                    }
                }

                //if ((ListofPricingOption == null || ListofPricingOption.Count == 0) && (PricingOptionDt == null || PricingOptionDt.Count == 0))
                //{
                //    dgPricingoption.ItemsSource = null;
                //}
                else if (ssRateobj == null || PricingOptionDt == null || PricingOptionDt.Count == 0)
                {
                    dgPricingoption.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }


        }

        private void RatecheckExpireactive(string SupplierServiceIdval)
        {
            DateTime ist = (DateTime)IWparentwindowdt.TxtItinerarystartdt.SelectedDate;
            DateTime isend = (DateTime)IWparentwindowdt.TxtItineraryEndDt.SelectedDate;
            string ssrateid = string.Empty;
            List<Tuple<DateTime, DateTime>> dateRanges = null;
            dateRanges = new List<Tuple<DateTime, DateTime>>
            {


            };
            //if (chbhideexpiredseasons.IsChecked == true && chbhidenonactive.IsChecked == true)
            //{
            //    dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.IsActive == false).ToList();
            //}
            //else 
            if (chbhideexpiredseasons.IsChecked == true)
            {
                dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false).ToList();
                if (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false).ToList().Count() > 0)
                {
                    foreach (var dt in SupplierSRatesDt)
                    {

                        dateRanges.Add(Tuple.Create(dt.ValidFrom, dt.ValidTo));

                    }
                    //var result = dateRanges.FirstOrDefault(range => range.Item1 >= ist && range.Item2 <= isend);
                    //var result1 = dateRanges.FirstOrDefault(range => range.Item1 <= ist && range.Item2 >= isend);
                    int k = 0;
                    var result = dateRanges.FindIndex(range => range.Item1 <= ist && range.Item2 >= isend);
                    if (result == -1)
                    {
                        dateRanges.Clear();
                        dateRanges.Add(Tuple.Create(ist, isend));
                        foreach (var dt in SupplierSRatesDt)
                        {
                            
                            var result1 = dateRanges.FindIndex(range => range.Item1 <= dt.ValidFrom && range.Item2 >= dt.ValidTo);
                            if (result1 !=-1)
                            {                              
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkssRates, (k == 0) ? 0 : (k));
                                break;
                            }
                            k++;
                        }
                    }
                    else 
                    {
                        CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkssRates, (result == 0) ? 0 : (result));

                    }
                }
            }
            //else if (chbhidenonactive.IsChecked == true)
            //{
            //    dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsActive == false).ToList();
            //}
            else
            {
               
                dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).ToList();
                if (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).ToList().Count() > 0)
                {
                    foreach (var dt in SupplierSRatesDt)
                    {

                        dateRanges.Add(Tuple.Create(dt.ValidFrom, dt.ValidTo));

                    }
                    //var result = dateRanges.FirstOrDefault(range => range.Item1 >= ist && range.Item2 <= isend);
                    //var result1 = dateRanges.FirstOrDefault(range => range.Item1 <= ist && range.Item2 >= isend);

                    var result = dateRanges.FindIndex(range => range.Item1 >= ist && range.Item2 >= isend);
                    if (result == -1)
                    {
                        dateRanges.Clear();
                        dateRanges.Add(Tuple.Create(ist, isend));
                        foreach (var dt in SupplierSRatesDt)
                        {
                            var result1 = dateRanges.FindIndex(range => range.Item1 <= dt.ValidFrom && range.Item2 >= dt.ValidTo);
                            if (result1 != -1)
                            {
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkssRates, (result1 == 0) ? 0 : (result1));
                                break;
                            }
                        }
                    }
                    else
                    {
                        CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkssRates, (result == 0) ? 0 : (result));

                    }
                }
                //foreach (var x in SupplierSRatesDt)
                //{
                //    k++;
                //    //if ((x.ValidFrom >= ist && x.ValidTo >= ist) && (x.ValidFrom <= isend && x.ValidTo <= isend))
                //    // if ((x.ValidFrom >= ist && x.ValidTo >= ist) && (x.ValidFrom <= isend && x.ValidTo >= isend))

                //    // if ((x.ValidFrom <= isend && x.ValidTo >= isend) && (x.ValidFrom >= isend && x.ValidTo >= isend))
                //    //if((x.ValidFrom >= isend && x.ValidTo >= isend))
                //    // {
                //    //     if (x.ValidFrom >= ist && x.ValidTo >= ist)
                //    //     {
                //    //         ssrateid = x.SupplierServiceDetailsRateId;
                //    //         s = k;
                //    //         break;
                //    //     }                        
                //    // }

                //    if ((isend >= x.ValidFrom && isend>=x.ValidTo))
                //    {
                //        if (ist>=x.ValidFrom&& ist>=x.ValidTo)
                //        {
                //            ssrateid = x.SupplierServiceDetailsRateId;
                //            s = k;
                //            break;
                //        }
                //    }
                //}
                //if ((s - 1) >= 0)
                //{
                //    CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesBkssRates, s-1);
                //}


            }
        }

        private void PricingOptcheckExpireactive(string SupplierServiceIdval, string supprateid, bool supprateexpired)
        {
            if (chbhideexpiredseasons.IsChecked == true && chbhidenonactive.IsChecked == true)
            {
                dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList());
            }
            else if (chbhidenonactive.IsChecked == true)
            {
                dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList();
            }
            else if (chbhideexpiredseasons.IsChecked == true)
            {
                dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && x.SupplierServiceDetailsRateId == supprateid).ToList();
            }
            else
            {
                dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
            }
        }

        private void Selectforbooking()
        {
            try
            {
                if (dgPricingoption.Items.Count > 0)
                {

                    BookingSupplierServiceModels objBss = new BookingSupplierServiceModels();

                    dgBookingselected.ItemsSource = null;
                    SupplierSelectedbookingSM = null;
                    string suppliername = string.Empty;
                    foreach (SupplierPricingOption item in dgPricingoption.Items)
                    {
                        if (PricingOptionDt.Where(x => x.SupplierServiceId == item.SupplierServiceId && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg != null)
                        {
                            if ((bool)PricingOptionDt.Where(x => x.SupplierServiceId == item.SupplierServiceId && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg == true)
                            {
                                SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                                SelectedSupplierBooking objSSB = new SelectedSupplierBooking();
                                if (ssmobj != null)
                                {
                                    //objSSB.BookingName = listBkgsupplierserv.Where(x => x.ServiceID == item.SupplierServiceId).FirstOrDefault().ServiceName;
                                    suppliername = DBconnEF.GetSupplierName(ssmobj.SupplierId);

                                   
                                    //var SupplierID = listBkgsupplierserv.Where(x => x.ServiceID == item.SupplierServiceId).FirstOrDefault().SupplierID;
                                    objSSB.BookingName = suppliername;

                                    // suppliername = ssmobj.ServiceName.ToString();
                                    objSSB.ItemsDescription = ssmobj.ServiceName.ToString() + ", " + item.PricingOptionName;
                                    objSSB.SupplierID = ssmobj.SupplierId.ToString();
                                }
                                else
                                {
                                    objSSB.BookingName = string.Empty;
                                    suppliername = string.Empty;
                                    objSSB.ItemsDescription = suppliername.ToString() + ", " + item.PricingOptionName;
                                    objSSB.SupplierID = string.Empty;
                                }
                                objSSB.ServiceID = item.SupplierServiceId.ToString();
                                objSSB.ServiceTypeID= ((SQLDataAccessLayer.Models.SupplierServiceType)ssmobj.SelectedItem).ServiceTypeID;
                                if (IWparentwindowdt.BookingItemsitin.Count > 0)
                                {
                                   // if(objSSB.ServiceTypeID)
                                    objSSB.BookingStartDate = (IWparentwindowdt.BookingItemsitin.Max(x => x.Enddate) != null) ? IWparentwindowdt.BookingItemsitin.Max(x => x.Enddate) : Convert.ToDateTime(IWparentwindowdt.TxtItinerarystartdt.ToString());
                                }
                                else if (IWparentwindowdt.BookingItemsitin.Count == 0)
                                {
                                    if (IWparentwindowdt.TxtItinerarystartdt.SelectedDate != null)
                                    {
                                        objSSB.BookingStartDate = Convert.ToDateTime(IWparentwindowdt.TxtItinerarystartdt.ToString());
                                    }
                                    else
                                    {
                                        System.Windows.MessageBox.Show("Itinerary Start Date should not be empty");
                                        return;
                                    }
                                }
                                if (!string.IsNullOrEmpty(suppliername))
                                {
                                    if (suppliername.ToLower() == "hertz rent a car" || suppliername.ToLower() == "hertz rent a car - scotland"
                                        || suppliername.ToLower() == "hertz rent a car- belfast airport")
                                        {
                                        objSSB.BookingStartDate = null;
                                        }
                                }
                                ReterivePricingEditRate(item.SupplierServiceId, item.PricingOptionId);
                                //objSSB.BookingStartDate = Convert.ToDateTime(IWparentwindowdt.TxtItinerarystartdt.ToString());
                                objSSB.BookingID = (SupplierSelectedbookingSM.Count + 1);
                                objSSB.PricingOptionId = item.PricingOptionId;
                                SupplierSelectedbookingSM.Add(objSSB);
                            }
                        }
                    }
                    dgBookingselected.ItemsSource = SupplierSelectedbookingSM;
                    SelectedBooking.IsSelected = true;
                    dgBookingselected.IsEnabled = true;
                    if (!string.IsNullOrEmpty(suppliername))
                    {
                        txtAdditems.Text = suppliername;
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void btnSelectforbkg_Click(object sender, RoutedEventArgs e)
        {
            if (validation() == 0)
            {
                System.Windows.MessageBox.Show("Please select pricing option");
                return;
            }
            Selectforbooking();
        }
        private void dgPricingoptionDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (validation() == 0)
            //{
            //    System.Windows.MessageBox.Show("Please select pricing option");
            //    return;
            //}
            //Selectforbooking();
            //e.Handled = true;
            SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
            SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, null, null, this);
            wobj.ShowDialog();

        }

        private int validation()
        {
            int cnt = 0;
            try
            {


                foreach (SupplierPricingOption item in dgPricingoption.Items)
                {
                    if (PricingOptionDt.Where(x => x.SupplierServiceId == item.SupplierServiceId && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg != null)
                    {
                        if ((bool)PricingOptionDt.Where(x => x.SupplierServiceId == item.SupplierServiceId && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg == true)
                        {
                            cnt = cnt + 1;
                            break;
                        }
                    }
                }
                if (cnt == 0)
                {
                    cnt = 0;
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return cnt;
        }
        private void btnCancelend_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dgBookingselected.Items.Count > 0)
                {

                    string maxbookingid = string.Empty;
                    foreach (SelectedSupplierBooking itemdt in dgBookingselected.Items)
                    {
                        if (string.IsNullOrEmpty(itemdt.NightDays))
                        {
                            System.Windows.MessageBox.Show("Please provide a Night/Days for " + itemdt.ItemsDescription.ToString());
                            return;
                        }
                        if (!int.TryParse(itemdt.NightDays, out int result))
                        {
                            System.Windows.MessageBox.Show("Please provide a valid Night/Days for");
                            return;
                        }
                        if (string.IsNullOrEmpty(itemdt.Quantity))
                        {
                            System.Windows.MessageBox.Show("Please provide a Quantity for " + itemdt.ItemsDescription.ToString());
                            return;
                        }
                        if (string.IsNullOrEmpty(itemdt.Quantity))
                        {
                            System.Windows.MessageBox.Show("Please provide a Quantity for " + itemdt.ItemsDescription.ToString());
                            return;
                        }
                        if (!int.TryParse(itemdt.Quantity, out int result1))
                        {
                            System.Windows.MessageBox.Show("Please provide valid numeric value on Quantity");
                            return;
                        }
                        if (itemdt.BookingStartDate == null)
                        {

                            System.Windows.MessageBox.Show("Please provide a Start date");
                            return;
                        }
                        string servicetypename = DBconnEF.GetServicetypename(itemdt.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
                            {
                                
                            }
                            else
                            {
                                int days = (itemdt.NightDays != null) ? Convert.ToInt32(itemdt.NightDays) : 0;
                                if (days > 1)
                                {
                                    System.Windows.MessageBox.Show("Please provide one day for this service type");
                                    return;
                                }
                            }
                        }


                    }
                    foreach (SelectedSupplierBooking item in dgBookingselected.Items)
                    {
                        //if (SupplierSelectedbookingSM.Where(x => x.ServiceID == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkgitem != null)
                        //{
                        // if ((bool)SupplierSelectedbookingSM.Where(x => x.ServiceID == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkgitem == true)
                        // {
                        BookingItems objBI = new BookingItems();
                        SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;

                        List<SupplierModels> supplierModels = objsupdal.SupplierRetrive("FIR", Guid.Parse(item.SupplierID));
                        if (supplierModels != null)
                        {
                            PaymentDueDatevalue = supplierModels.First().SupplierPaymentTermsindays.ToString();
                            objBI.SupplierPaymentTermsindays = supplierModels.First().SupplierPaymentTermsindays;
                            objBI.SupplierPaymentDepositAmount = supplierModels.First().SupplierPaymentDepositAmount;
                        }

                        if (ssmobj != null)
                        {
                            string suppliername = string.Empty;
                            suppliername = DBconnEF.GetSupplierName(ssmobj.SupplierId);

                            objBI.BookingName = suppliername;
                            objBI.SupplierID = ssmobj.SupplierId;
                        }
                        else
                        {
                            objBI.BookingName = string.Empty;
                            objBI.SupplierID = string.Empty;

                        }


                        objBI.ItemDescription = item.ItemsDescription;

                        objBI.ItineraryID = IWparentwindowdt.hdnitineraryid.Text;
                        //objBI.ServiceTypeID = item.ServiceTypeID.ToString();
                        objBI.ServiceID = item.ServiceID.ToString();
                      

                        
                        maxbookingid = DBconnEF.GetMaxBookingid();
                        if (BookingItems.Count > 0)
                        {
                            objBI.BookingAutoID = ((BookingItems.Max(x => x.BookingID)) + 1);
                            objBI.BookingID = ((BookingItems.Max(x => x.BookingID)) + 1);
                        }
                        else
                        {
                            objBI.BookingAutoID = (!string.IsNullOrEmpty(maxbookingid)) ? Convert.ToInt64(maxbookingid) + 1 : 0;
                            objBI.BookingID = (!string.IsNullOrEmpty(maxbookingid)) ? Convert.ToInt64(maxbookingid) + 1 : 0;
                        }

                        objBI.AgentCommission = "0.00";
                        objBI.AgentCommissionPercentage = string.Empty;

                        if (ssmobj != null)
                        {
                            if (((SQLDataAccessLayer.Models.Currencydetails)ssmobj.SelectedItemCurrency != null))
                            {
                                objBI.BkgCurrencyName = ((SQLDataAccessLayer.Models.Currencydetails)ssmobj.SelectedItemCurrency).CurrencyName;
                                objBI.BkgCurrencyID = ((SQLDataAccessLayer.Models.Currencydetails)ssmobj.SelectedItemCurrency).CurrencyID;
                                objBI.BkgCurDisplayFormat = ((SQLDataAccessLayer.Models.Currencydetails)ssmobj.SelectedItemCurrency).DisplayFormat;
                            }
                            else
                            {
                                objBI.BkgCurrencyName = string.Empty;
                                objBI.BkgCurrencyID = string.Empty;
                                objBI.BkgCurDisplayFormat = string.Empty;
                            }
                            objBI.Type = ((SQLDataAccessLayer.Models.SupplierServiceType)ssmobj.SelectedItem).ServiceTypeName;
                            objBI.ServiceTypeID = ((SQLDataAccessLayer.Models.SupplierServiceType)ssmobj.SelectedItem).ServiceTypeID;
                        }
                        else
                        {
                            objBI.BkgCurrencyName = string.Empty;
                            objBI.BkgCurrencyID = string.Empty;
                            objBI.Type = string.Empty;
                            objBI.ServiceTypeID = string.Empty;
                            objBI.BkgCurDisplayFormat = string.Empty;
                        }
                        BookingSupplierServiceModels bkgssm = dgBkgSupplierService.SelectedItem as BookingSupplierServiceModels;
                        if (bkgssm != null)
                        {
                            objBI.City = bkgssm.CityName;
                            objBI.Region = bkgssm.RegionName;
                            objBI.RegionID = bkgssm.RegionID;
                            objBI.CityID = bkgssm.CityID;
                        }
                        objBI.StartDate = (DateTime)item.BookingStartDate;
                        // objBI.StartTimedt = (item.BookingStartTime.Hours==0 && item.BookingStartTime.Minutes==0)? DateTime.Now: item.BookingStartTime. ;
                        if (item.BookingStartTime.Hours == 0 && item.BookingStartTime.Minutes == 0)
                        {
                            objBI.StartTime = DateTime.Now.ToShortTimeString();
                        }
                        else
                        {
                            objBI.StartTime = ((item.BookingStartTime.Hours < 9) ? ("0" + item.BookingStartTime.Hours.ToString()) : (item.BookingStartTime.Hours.ToString())) + ":" +
                                ((item.BookingStartTime.Minutes < 9) ? ("0" + (item.BookingStartTime.Minutes.ToString())) : (item.BookingStartTime.Minutes.ToString())).ToString(); //(item.BookingStartTime.ToShortTimeString()).ToString();
                        }
                        objBI.Comments = string.Empty;
                        objBI.Day = (((DateTime)item.BookingStartDate).DayOfWeek).ToString();
                        string servicetypename = DBconnEF.GetServicetypename(objBI.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
                            {
                                objBI.Enddate = ((DateTime)item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays));
                            }
                            else
                            {
                                objBI.Enddate = ((DateTime)item.BookingStartDate);
                            }
                        }

                        //objBI.Enddate = ((DateTime)item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays));
                        objBI.EndTime = objBI.StartTime;//(item.BookingStartTime.Hours + ":" + item.BookingStartTime.Minutes).ToString();//(item.BookingStartTime.ToShortTimeString()).ToString();
                        objBI.ExchRate = "1.00";
                        objBI.NtsDays = Convert.ToInt32(item.NightDays);
                        objBI.Qty = Convert.ToInt32(item.Quantity);

                        if (PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg != null)
                        {

                            objBI.PricingRateID = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().SupplierServiceDetailsRateId;
                            {

                            }
                        }

                        decimal Grossvalue = 0, netvalue = 0;
                        string markuppercen = string.Empty, commissionpercen = string.Empty;
                        int cnt = Convert.ToInt32(item.NightDays);
                        string dayofweek = string.Empty;
                        decimal grossprice = 0; decimal netprice = 0; decimal grosspricetotal = 0; decimal netpricetotal = 0,grossadjtotal=0;
                        string[] strgrossvaluearr = new string[cnt];
                        string[] strnetvaluearr = new string[cnt];
                        string strgrossvalue = string.Empty;
                        string strnetvalue = string.Empty;
                        ReterivePricingEditRate(item.ServiceID, item.PricingOptionId);
                        var listsupdt = SupplierSRatesDt.Where(x => (x.ValidFrom <= (DateTime)item.BookingStartDate || x.ValidTo >= ((DateTime)item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays))) && x.SupplierServiceId == item.ServiceID && x.SupplierServiceDetailsRateId == objBI.PricingRateID).ToList();
                        if (listsupdt.Count > 0)
                        {


                            if (PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg != null)
                            {

                                objBI.PricingRateID = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().SupplierServiceDetailsRateId;

                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId).Count() > 0)
                                {

                                    dayofweek = (((DateTime)item.BookingStartDate).DayOfWeek).ToString();
                                    for (int i = 0; i < cnt; i++)
                                    {

                                        switch (dayofweek.ToString().ToLower())
                                        {

                                            case "sunday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault().NetPrice;
                                                    // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID), Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "monday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault().NetPrice;
                                                    // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "tuesday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault().NetPrice;
                                                    // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "wednesday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault().NetPrice;
                                                    // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "thursday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault().NetPrice;
                                                    //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "friday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault().NetPrice;
                                                    //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    objBI.GrossAdj = (decimal)DBconnEF.GrossAdjCalculation(grossprice);
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                            case "saturday":
                                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault() != null)
                                                {
                                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault().GrossPrice;
                                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault().NetPrice;
                                                    //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                    grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                    netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                                    strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                    strnetvaluearr[i] = netprice.ToString("0.00");
                                                    
                                                    bool test = DBconnEF.Validateserviceavailable(Guid.Parse(item.SupplierID), Guid.Parse(item.ServiceID), Guid.Parse(objBI.PricingRateID),
                                                        Guid.Parse(item.PricingOptionId), Guid.Parse(PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault().PriceEditRateId));
                                                    if (!test)
                                                    {
                                                        objBI.NewNetUnitNotinSupptbl = true;
                                                    }
                                                }
                                                break;
                                        }
                                        dayofweek = ((((DateTime)item.BookingStartDate).AddDays(i + 1)).DayOfWeek).ToString();
                                    }
                                    //sup.NetPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId &&  >= startDate.Date && a.Start.Date <= endDate).FirstOrDefault().NetPrice;
                                    //sup.MarkupPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().MarkupPercentage;
                                    //sup.GrossPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().GrossPrice;
                                    //sup.CommissionPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().CommissionPercentage;

                                }
                                // Grossvalue = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().GrossPrice;
                                //netvalue=PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().NetPrice;
                                markuppercen = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().MarkupPercentage.ToString();
                                commissionpercen = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().CommissionPercentage.ToString();
                                objBI.Grossunit = (strgrossvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strgrossvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strgrossvaluearr.Distinct().OfType<string>().ToArray());
                                objBI.GrossAdj = grossadjtotal;
                                objBI.Grossfinal = grosspricetotal;//Grossvalue * objBI.Qty* objBI.NtsDays;
                                objBI.Grosstotal = grosspricetotal;//Grossvalue * objBI.Qty * objBI.NtsDays;

                                objBI.Netfinal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 
                                objBI.Nettotal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 


                                objBI.Netunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());

                                objBI.MarkupPercentage = markuppercen;
                                objBI.CommissionPercentage = commissionpercen;



                            }
                        }
                        else
                        {
                            if (PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().selectedforbkg != null)
                            {

                                objBI.PricingRateID = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().SupplierServiceDetailsRateId;

                                if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId).Count() > 0)
                                {

                                    /* dayofweek = ((item.BookingStartDate).DayOfWeek).ToString();
                                     for (int i = 0; i < cnt; i++)
                                     {

                                         switch (dayofweek.ToString().ToLower())
                                         {

                                             case "sunday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Sunday == true).FirstOrDefault().NetPrice;
                                                     // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "monday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Monday == true).FirstOrDefault().NetPrice;
                                                     // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "tuesday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Tuesday == true).FirstOrDefault().NetPrice;
                                                     // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "wednesday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Wednesday == true).FirstOrDefault().NetPrice;
                                                     // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "thursday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Thursday == true).FirstOrDefault().NetPrice;
                                                     //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "friday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Friday == true).FirstOrDefault().NetPrice;
                                                     //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                             case "saturday":
                                                 if (PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault() != null)
                                                 {
                                                     grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault().GrossPrice;
                                                     netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId && x.Saturday == true).FirstOrDefault().NetPrice;
                                                     //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
                                                     grosspricetotal = grosspricetotal + (grossprice * objBI.Qty);
                                                     netpricetotal = netpricetotal + (netprice * objBI.Qty);
                                                     strgrossvaluearr[i] = grossprice.ToString("0.00");
                                                     strnetvaluearr[i] = netprice.ToString("0.00");
                                                 }
                                                 break;
                                         }
                                         dayofweek = ((item.BookingStartDate.AddDays(i + 1)).DayOfWeek).ToString();
                                     }
                                     //sup.NetPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId &&  >= startDate.Date && a.Start.Date <= endDate).FirstOrDefault().NetPrice;
                                     //sup.MarkupPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().MarkupPercentage;
                                     //sup.GrossPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().GrossPrice;
                                     //sup.CommissionPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().CommissionPercentage;
                                     */

                                    grossprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId).FirstOrDefault().GrossPrice;
                                    netprice = PriceEditDt.Where(x => x.PricingOptionId == item.PricingOptionId).FirstOrDefault().NetPrice;
                                    grosspricetotal = grossprice * objBI.Qty * objBI.NtsDays;
                                    netpricetotal = netprice * objBI.Qty * objBI.NtsDays;
                                    grossadjtotal = grossadjtotal + ((decimal)DBconnEF.GrossAdjCalculation(grossprice));
                                    if (strgrossvaluearr.Count() == 0)
                                    {
                                        strgrossvaluearr = new string[1];
                                        strnetvaluearr = new string[1];
                                        strgrossvaluearr[0] = grossprice.ToString("0.00");
                                        strnetvaluearr[0] = netprice.ToString("0.00");
                                    }
                                    else
                                    {
                                        strgrossvaluearr[0] = grossprice.ToString("0.00");
                                        strnetvaluearr[0] = netprice.ToString("0.00");
                                    }



                                }


                                markuppercen = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().MarkupPercentage.ToString();
                                commissionpercen = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().CommissionPercentage.ToString();

                                objBI.GrossAdj = grossadjtotal;
                                objBI.Grossfinal = grosspricetotal;//Grossvalue * objBI.Qty* objBI.NtsDays;
                                objBI.Grosstotal = grosspricetotal;//Grossvalue * objBI.Qty * objBI.NtsDays;

                                objBI.Netfinal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 
                                objBI.Nettotal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 

                                if (cnt == 0)
                                {
                                    Grossvalue = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().GrossPrice;
                                    netvalue = PricingOptionDt.Where(x => x.SupplierServiceId == item.ServiceID && x.PricingOptionId == item.PricingOptionId).FirstOrDefault().NetPrice;


                                    objBI.Grossunit = Grossvalue.ToString(); //(strgrossvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join(",", strgrossvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strgrossvaluearr.Distinct().OfType<string>().ToArray());
                                    objBI.Netunit = netvalue.ToString();// (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join(",", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());
                                }
                                else
                                {
                                    objBI.Grossunit = (strgrossvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strgrossvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strgrossvaluearr.Distinct().OfType<string>().ToArray());
                                    objBI.Netunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join("; ", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());

                                }

                                objBI.MarkupPercentage = markuppercen;
                                objBI.CommissionPercentage = commissionpercen;



                            }
                        }


                        objBI.Invoiced = false;
                        objBI.ItinCurrency = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().CurrencyName;
                        objBI.ItinCurrencyID = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().CurrencyID;
                        objBI.ItinCurDisplayFormat = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().DisplayFormat;
                        objBI.ChangeCurrencyFormat = objBI.BkgCurDisplayFormat;


                        Tuple<decimal, decimal> curr = CommonValues.CalculateItinearycurrency(objBI.ItinCurrencyID, objBI.BkgCurrencyID);
                        if (curr != null)
                        {
                            if (objBI.ItinCurrency != objBI.BkgCurrencyName && objBI.ItinCurrencyID != objBI.BkgCurrencyID)
                            {
                                if (objBI.Grosstotal > 0 && objBI.Nettotal > 0 && curr.Item1 > 0 && curr.Item2 > 0)
                                {
                                    objBI.BkgNettotal = objBI.Nettotal;
                                    objBI.BkgNetfinal = objBI.Netfinal;
                                    objBI.BkgGrosstotal = objBI.Grosstotal;
                                    objBI.BkgGrossfinal = objBI.Grossfinal;

                                    objBI.Nettotal = curr.Item2 * objBI.Nettotal;
                                    objBI.Netfinal = curr.Item2 * objBI.Netfinal;
                                    objBI.Grosstotal = curr.Item2 * objBI.Grosstotal;
                                    objBI.Grossfinal = curr.Item2 * objBI.Grossfinal;

                                    objBI.ChangeCurrencyID = objBI.BkgCurrencyID;


                                }
                            }
                            else
                            {
                                objBI.BkgNettotal = curr.Item1 * objBI.Nettotal;
                                objBI.BkgNetfinal = curr.Item1 * objBI.Netfinal;
                                objBI.BkgGrosstotal = curr.Item1 * objBI.Grosstotal;
                                objBI.BkgGrossfinal = curr.Item1 * objBI.Grossfinal;
                                if (BookingItems.Select(x => x.BkgCurrencyID).Distinct().Count() > 1)
                                {

                                    List<Currencydetail> Listcur = new List<Currencydetail>();
                                    Listcur = DBconnEF.Currencydispalyformat();
                                    if (Listcur != null && Listcur.Count > 0)
                                    {
                                        if (Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true) != null)
                                        {
                                            objBI.ItinCurDisplayFormat = Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;
                                            objBI.ItinCurrencyID = Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().CurrencyId.ToString();
                                        }
                                        if (Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true) != null)
                                        {
                                            objBI.ChangeCurrencyFormat = Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;
                                            objBI.ChangeCurrencyID = Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().CurrencyId.ToString();
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {
                            objBI.BkgNettotal = objBI.Nettotal;
                            objBI.BkgNetfinal = objBI.Netfinal;
                            objBI.BkgGrosstotal = objBI.Grosstotal;
                            objBI.BkgGrossfinal = objBI.Grossfinal;
                            List<Currencydetail> Listcur = new List<Currencydetail>();
                            Listcur = DBconnEF.Currencydispalyformat();
                            if (Listcur != null && Listcur.Count > 0)
                            {
                                if (Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true) != null)
                                {
                                    objBI.ItinCurDisplayFormat = Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;
                                    objBI.ItinCurrencyID = Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().CurrencyId.ToString();
                                }
                                else if (Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true) != null)
                                {
                                    objBI.ChangeCurrencyFormat = Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;
                                    objBI.ChangeCurrencyID = Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().CurrencyId.ToString();
                                }

                            }
                        }



                        if (!string.IsNullOrEmpty(PaymentDueDatevalue))
                        {
                            objBI.PaymentDueDate = ((DateTime)  item.BookingStartDate).AddDays(-(Convert.ToInt32(PaymentDueDatevalue)));
                        }
                        objBI.Ref = string.Empty;

                        objBI.ServiceName = ssmobj.ServiceName;
                        objBI.Status = string.Empty;
                        objBI.PricingOptionId = item.PricingOptionId;
                        BookingItems.Add(objBI);


                        //   }
                        //}
                    }
                    IWparentwindowdt.BookingItemsitin = BookingItems;
                    if (BookingItems.Select(x => x.BkgCurrencyName).Distinct().Contains("Pound Sterling") == true)
                    {
                        IWparentwindowdt.visiblesterlingcolumn();
                    }
                    else
                    {
                        IWparentwindowdt.hidesterlingcolumn();
                    }
                    IWparentwindowdt.totalcalculation();
                    IWparentwindowdt.dgItinBooking.ItemsSource = BookingItems;
                    this.Close();
                }
                else
                {
                    System.Windows.MessageBox.Show("Please select at least one supplier for the booking...!");
                    return;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        
        private void dgBookingselected_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == System.Windows.Controls.DataGridEditAction.Commit)
            {
                var column = e.Column as System.Windows.Controls.DataGridBoundColumn;
                if (column != null && column.Binding != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "NightDays")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var el = e.EditingElement as TextBox;
                        if (string.IsNullOrEmpty(el.Text))
                        {
                            System.Windows.MessageBox.Show("Please provide a numeric value on Nt/Day");
                            return;
                        }
                        if (ValidationClass.IsNumeric(el.Text) == false)
                        {
                            System.Windows.MessageBox.Show("Please enter only numeric value on Nt/Day");
                            return;
                        }
                        if (!int.TryParse(el.Text,out int result))
                        {
                            System.Windows.MessageBox.Show("Please enter valid numeric value on Nt/Day");
                            return;
                        }
                        
                        SelectedSupplierBooking objssB = (SelectedSupplierBooking)(e.Row.DataContext);

                        string servicetypename = DBconnEF.GetServicetypename(objssB.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
                            {
                                //objBI.Enddate = ((DateTime)item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays));
                            }
                            else
                            {
                                int days = (el != null) ? Convert.ToInt32(el.Text) : 0;
                                if(days>1)
                                {
                                    string ermsg = "Please provide one day for this service" + objssB.ItemsDescription;
                                    System.Windows.MessageBox.Show(ermsg);
                                    el.Text = string.Empty;
                                    el.Focus();
                                    el.Focusable= true;
                                    return;
                                }
                            }
                        }
                        if (objssB != null)
                        {
                            if (!string.IsNullOrEmpty(el.Text))
                            {
                                int days = (el != null) ? Convert.ToInt32(el.Text) : 0;
                                if (days > 0 && objssB.BookingStartDate!=null)                                    
                                    ReteriveWarningSupplier(objssB.ServiceID, ((DateTime)objssB.BookingStartDate), ((DateTime)objssB.BookingStartDate).AddDays(days));
                            }
                        }

                    }
                    if (bindingPath == "Quantity")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var elqty = e.EditingElement as TextBox;
                        if (string.IsNullOrEmpty(elqty.Text))
                        {
                            System.Windows.MessageBox.Show("Please provide a numeric value on Quantity");
                            return;
                        }
                        if (ValidationClass.IsNumeric(elqty.Text) == false)
                        {
                            System.Windows.MessageBox.Show("Please enter only numeric value on Quantity");
                            return;
                        }
                        if (!int.TryParse(elqty.Text, out int result))
                        {
                            System.Windows.MessageBox.Show("Please enter valid numeric value on Quantity");
                            return;
                        }
                    }
                }
            }
        }

        public void ReteriveWarningSupplier(string SupplierServiceIdval, DateTime dtfrom, DateTime dtto)
        {
            try
            {
                if (SupplierServiceIdval != "")
                {

                    string servicewarning = string.Empty;
                    string supplierwarning = string.Empty;
                    Tuple<string, string> warreslt = CommonValues.ReteriveWarnings(SupplierServiceIdval, dtfrom, dtto);
                    if (warreslt != null)
                    {
                        servicewarning = warreslt.Item1;
                        supplierwarning = warreslt.Item2;

                        if (!string.IsNullOrEmpty(servicewarning))
                        {
                            MessageBoxResult mesbox = System.Windows.MessageBox.Show(servicewarning, "Service Warning Message", System.Windows.MessageBoxButton.YesNo);
                            if (mesbox == MessageBoxResult.Yes)
                            {
                            }
                        }
                        if (!string.IsNullOrEmpty(supplierwarning))
                        {
                            MessageBoxResult messupbox = System.Windows.MessageBox.Show(supplierwarning, "Supplier Warning Message", System.Windows.MessageBoxButton.YesNo);
                            if (messupbox == MessageBoxResult.Yes)
                            {
                            }
                        }

                    }

                    if (PriceEditDt.Count > 1)
                    {
                        MessageBoxResult mesbox = System.Windows.MessageBox.Show("There is different rate available for the selected number of days", "Service Warning Message", System.Windows.MessageBoxButton.OK);
                        if (mesbox == MessageBoxResult.Yes)
                        {
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }


        public void DeleteselectedBooking(SelectedSupplierBooking objSSB)
        {
            try
            {
                if (objSSB != null)
                {

                    SupplierSelectedbookingSM.Remove(SupplierSelectedbookingSM.Where(m => m.SupplierID == objSSB.SupplierID && m.ServiceID == objSSB.ServiceID
                    && m.BookingID == objSSB.BookingID && m.PricingOptionId == objSSB.PricingOptionId).FirstOrDefault());
                    //string objret = objsupdal.DeletePricingOption(objsupppropt);
                    //if (!string.IsNullOrEmpty(objret))
                    //{
                    //    if (objret.ToString().ToLower() == "1")
                    //        MessageBox.Show("Supplier Pricing option Deleted successfully");
                    //    PricingOptionDt.Remove(PricingOptionDt.Where(m => m.PricingOptionId == ssm.PricingOptionId).FirstOrDefault());
                    //    //dgSupplierServicesBkssRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                    //    //PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    //    ReterivePricingOption();
                    //}

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void btnSelectedBookingDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    SelectedSupplierBooking objSSB = dgBookingselected.SelectedItem as SelectedSupplierBooking;
                    DeleteselectedBooking(objSSB);
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingSupplierSearch", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void ReterivePricingEditRate(string SupplierServiceId, string PricingOptionId)
        {
            try
            {
                // SupplierPriceRateEdit sspredobj = dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
                if (PriceEditDt != null)
                {
                    //ListofPricedit = null;
                    //PriceEditDt = null;
                    ListofPricedit = objsupdal.PriceEditRateRetrive(Guid.Parse(SupplierServiceId),
                         Guid.Parse(PricingOptionId), false);
                    if (ListofPricedit != null && ListofPricedit.Count > 0)
                    {
                        foreach (SupplierPriceRateEdit sup in ListofPricedit)
                        {
                            if (PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId && x.PriceEditRateId == sup.PriceEditRateId).Count() == 0)
                            {
                                PriceEditDt.Add(sup);
                            }
                        }
                    }
                    if ((ListofPricedit == null || ListofPricedit.Count == 0) && (PriceEditDt.Count==0 || PriceEditDt==null))
                    {
                        // dgPricingRateEdit.ItemsSource = null;
                    }
                    // dgPricingRateEdit.ItemsSource = PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList();
                }


            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void dgBookingselected_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgBookingselected.BeginEdit();
        }

        private void btnViewallSearch_Click(object sender, RoutedEventArgs e)
        {
            clearvalues();
            LoadSupplierService();
        }



        private void AddItem()
        {
            SupplierServiceModels ssm;
            ssm = new SupplierServiceModels();
            ssm.SupplierId = hdnSupplierid.Text;
            ssm.ServiceName = string.Empty;//"New Service" + " (" + (SupplierSM.Count + 1) + ")";
            ssm.ServiceId = (Guid.NewGuid()).ToString();
            ssm.SelectedItem = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault();
            ssm.SelectedItemCurrency = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault();
            ssm.IsActive = true;
            //ssm.SupplierServiceRecordflag = true;
            SupplierSM.Add(ssm);
            // lstSupplierSM.Add(SupplierSM);
            dgSupplierServicesBkss.ItemsSource = SupplierSM;

            dgSupplierServicesBkss.Focus();
            dgSupplierServicesBkss.BeginEdit();
            // dgSupplierServicesBkss.CurrentCell = new System.Windows.Controls.DataGridCellInfo(
            // dgSupplierServicesBkss.Items[SupplierSM.Count-1], dgSupplierServices.Columns[3]);
        }

        public void saveupdateSupplierServices()
        {
            try
            {
                if (dgSupplierServicesBkss.Items.Count > 0)
                {
                    if (SupplierSM.Where(m => m.ServiceName.ToString().Trim() == string.Empty).Count() > 0)
                    {
                        MessageBox.Show("Please provide a Service Name");
                        return;
                    }

                    foreach (SupplierServiceModels ssm in dgSupplierServicesBkss.Items)
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServicesBkss.SelectedItem;
                        SupplierServiceModels objsuppServ = new SupplierServiceModels();
                        objsuppServ.SupplierId = ssm.SupplierId;
                        objsuppServ.ServiceId = ssm.ServiceId;
                        objsuppServ.ServiceName = ssm.ServiceName;
                        objsuppServ.IsActive = ssm.IsActive;
                        objsuppServ.Type = ((SQLDataAccessLayer.Models.SupplierServiceType)ssm.SelectedItem).ServiceTypeID;
                        objsuppServ.Currency = (((SQLDataAccessLayer.Models.Currencydetails)ssm.SelectedItemCurrency) != null) ? ((SQLDataAccessLayer.Models.Currencydetails)ssm.SelectedItemCurrency).CurrencyID : Guid.Empty.ToString();
                        objsuppServ.Groupinfo = (((SQLDataAccessLayer.Models.CommonValueList)ssm.SelectedItemGroupInfo) != null) ? ((SQLDataAccessLayer.Models.CommonValueList)ssm.SelectedItemGroupInfo).ValueField.ToString() : Guid.Empty.ToString();
                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objsuppServ.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServ.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServ.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objsuppServ.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServ.ModifiedBy = Guid.Empty.ToString();
                            objsuppServ.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objsuppServ.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServ.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServ.IsDeleted = true;
                            objsuppServ.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objret = objsupdal.SaveUpdateSupplierService(purpose, objsuppServ);
                        if (!string.IsNullOrEmpty(objret))
                        {
                            if (objret.ToString().ToLower() == "1")
                            {
                                //   MessageBox.Show("Supplier Service saved successfully");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        public void DeleteSupplierServices(SupplierServiceModels ssm)
        {
            try
            {
                if (dgSupplierServicesBkss.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesBkss.SelectedItem;
                    SupplierServiceModels objsuppServ = new SupplierServiceModels();
                    objsuppServ.SupplierId = ssm.SupplierId;
                    objsuppServ.ServiceId = ssm.ServiceId;
                    objsuppServ.IsDeleted = true;
                    objsuppServ.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierService(objsuppServ);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                            MessageBox.Show("Supplier Service Deleted successfully");
                        SupplierSM.Remove(SupplierSM.Where(m => m.ServiceId == ssm.ServiceId).FirstOrDefault());
                        // dgSupplierServicesBkssuc.ItemsSource = SupplierSM;
                        ReteriveSupplierServices(ssm.SupplierId);


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void BtnServicebkssAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }



        private void btnServiceBkssDelete_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                DeleteSupplierServices(ssmobj);

            }
        }


        private void AddRates()
        {
            SupplierServiceRatesDt ssRates;
            ssRates = new SupplierServiceRatesDt();
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            ssRates.SupplierServiceId = ssmobj.ServiceId.ToString();
            ssRates.ValidFrom = DateTime.Now.Date;
            ssRates.ValidTo = DateTime.Now.Date;
            ssRates.IsActive = true;
            //ssRates.StrValidFrom = DateTime.Today.ToShortDateString();
            //ssRates.StrValidTo = DateTime.Today.ToShortDateString();
            ssRates.SupplierServiceDetailsRateId = (Guid.NewGuid()).ToString();
            //DatePicker txtvalidfromdt = (DatePicker)dgSupplierServicesRatesUC.Items[2];
            //txtvalidfromdt.SelectedDate = DateTime.Now;

            if (SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
            {
                SupplierSRatesDt = null;
            }

            SupplierSRatesDt.Add(ssRates);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
            RatecheckExpireactive(ssmobj.ServiceId);
        }


        private void BtnRatesAddBkss_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            if (ssmobj != null)
            {
                AddRates();
            }
            else
            {
                MessageBox.Show("Please select a service");
            }
        }
        public void saveupdateSupplierServicesRates()
        {
            try
            {
                // if (dgSupplierServicesRatesUC.Items.Count > 0)
                if (SupplierSRatesDt.Count > 0)
                {
                    foreach (SupplierServiceRatesDt ssm in SupplierSRatesDt)
                    {
                        var strmsg = Suppliervalidationvaliddate(ssm.ValidFrom, ssm.ValidTo, "SupplierRates");
                        if (!string.IsNullOrEmpty(strmsg))
                        {
                            MessageBox.Show(strmsg);
                            return;
                        }
                    }

                    foreach (SupplierServiceRatesDt ssm in SupplierSRatesDt)
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServicesBkss.SelectedItem;
                        SupplierServiceRatesDt objsuppServRate = new SupplierServiceRatesDt();
                        objsuppServRate.ValidFrom = ssm.ValidFrom;
                        objsuppServRate.ValidTo = ssm.ValidTo;
                        objsuppServRate.SupplierServiceId = ssm.SupplierServiceId;
                        objsuppServRate.SupplierServiceDetailsRateId = ssm.SupplierServiceDetailsRateId;
                        objsuppServRate.IsActive = ssm.IsActive;
                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objsuppServRate.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServRate.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServRate.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objsuppServRate.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServRate.ModifiedBy = Guid.Empty.ToString();
                            objsuppServRate.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objsuppServRate.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServRate.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppServRate.IsDeleted = true;
                            objsuppServRate.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objret = objsupdal.SaveUpdateSupplierServiceRateDt(purpose, objsuppServRate);
                        if (!string.IsNullOrEmpty(objret))
                        {
                            if (objret.ToString().ToLower() == "1")
                            {
                                // MessageBox.Show("Supplier Service Rates saved successfully");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }


        public string Suppliervalidationvaliddate(DateTime ValidFrom, DateTime ValidTo, string frommsg)
        {
            string valmsg = string.Empty;
            if (ValidFrom > ValidTo)
            {
                if (frommsg.ToString().ToLower() == "supplierwarning")
                {
                    valmsg = "The Supplier Warning Valid To Date needs to be later than the  Valid From Date. Please change the dates as needed";
                }
                if (frommsg.ToString().ToLower() == "servicewarning")
                {
                    valmsg = "The Service Warning Valid To Date needs to be later than the  Valid From Date. Please change the dates as needed";
                }
                if (frommsg.ToString().ToLower() == "supplierrates")
                {
                    valmsg = "The Supplier Rates Valid To Date needs to be later than the  Valid From Date. Please change the dates as needed";
                }

            }
            return valmsg;
        }
        public void DeleteSupplierServicesRates(SupplierServiceRatesDt ssm)
        {
            try
            {
                if (dgSupplierServicesBkssRates.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesBkssRates.SelectedItem;
                    SupplierServiceRatesDt objsuppServRates = new SupplierServiceRatesDt();
                    objsuppServRates.SupplierServiceId = ssm.SupplierServiceId;
                    objsuppServRates.SupplierServiceDetailsRateId = ssm.SupplierServiceDetailsRateId;
                    objsuppServRates.IsDeleted = true;
                    objsuppServRates.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierServiceRateDt(objsuppServRates);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                        {
                            // MessageBox.Show("Supplier Service Rates Deleted successfully");
                        }
                        SupplierSRatesDt.Remove(SupplierSRatesDt.Where(m => m.SupplierServiceDetailsRateId == ssm.SupplierServiceDetailsRateId).FirstOrDefault());
                        //dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveSupplierServicesRates(ssm.SupplierServiceId);

                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void btnRatesDeletebkss_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                DeleteSupplierServicesRates(ssmobjrate);

            }
        }


        #region "Supplier Warning Start"
        private ObservableCollection<SupplierServiceWarning> _WarningDt;
        public ObservableCollection<SupplierServiceWarning> WarningDt
        {
            get { return _WarningDt ?? (_WarningDt = new ObservableCollection<SupplierServiceWarning>()); }
            set
            {
                _WarningDt = value;
            }
        }

        /* Warning Service Start */
        private void AddServiceWarning()
        {
            SupplierServiceWarning sswarning;
            sswarning = new SupplierServiceWarning();
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            if (ssmobj != null)
            {
                sswarning.SupplierServiceId = ssmobj.ServiceId.ToString();
                sswarning.ValidFromwarning = DateTime.Now.Date;
                sswarning.ValidTowarning = DateTime.Now.Date;
                sswarning.Messagefor = "Service";
                sswarning.SupplierServiceDetailsWarningID = (Guid.NewGuid()).ToString();
                sswarning.WarDescription = string.Empty;
                sswarning.IsActive = true;

                if (WarningDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
                {
                    WarningDt = null;
                }

                WarningDt.Add(sswarning);
                // lstSupplierSM.Add(SupplierSM);
                // dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
                WarningcheckExpireactive(ssmobj.ServiceId);
            }
            else
            {
                MessageBox.Show("Please select a service");
                return;
            }

        }

        private void WarningcheckExpireactive(string SupplierServiceIdval)
        {
            //if (chbhideexpiredseasonswarning.IsChecked == true && chbhidenonactive.IsChecked == true)
            //{
            //    dgServicesWarnings.ItemsSource = (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.IsActive == false && x.Messagefor.ToString().ToLower() == "service").ToList());
            //    dgSuppWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.IsActive == false && x.Messagefor.ToString().ToLower() == "supplier").ToList();
            //}
            //else
            if (chbhideexpiredseasonswarning.IsChecked == true)
            {
                dgServicesWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.Messagefor.ToString().ToLower() == "service").ToList();
                dgSuppWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.Messagefor.ToString().ToLower() == "supplier").ToList();
            }
            //else if (chbhidenonactive.IsChecked == true)
            //{
            //    dgServicesWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsActive == false && x.Messagefor.ToString().ToLower() == "service").ToList();
            //    dgSuppWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsActive == false && x.Messagefor.ToString().ToLower() == "supplier").ToList();
            //}
            else
            {
                dgServicesWarnings.ItemsSource = (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.Messagefor.ToString().ToLower() == "service").ToList());
                dgSuppWarnings.ItemsSource = WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.Messagefor.ToString().ToLower() == "supplier").ToList();
            }
        }

        public void saveupdateWarningService()
        {
            try
            {
                // if (dgServicesWarnings.Items.Count > 0)
                if (WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "service").ToList().Count > 0)
                {
                    foreach (SupplierServiceWarning ssmw in WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "service").ToList())
                    {
                        var strmsg = Suppliervalidationvaliddate(ssmw.ValidFromwarning, ssmw.ValidTowarning, "servicewarning");
                        if (!string.IsNullOrEmpty(strmsg))
                        {
                            MessageBox.Show(strmsg);
                            return;
                        }
                    }
                    foreach (SupplierServiceWarning ssm in WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "service").ToList())
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServicesBkss.SelectedItem;
                        SupplierServiceWarning objServwarn = new SupplierServiceWarning();
                        objServwarn.ValidFromwarning = ssm.ValidFromwarning;
                        objServwarn.ValidTowarning = ssm.ValidTowarning;
                        objServwarn.SupplierServiceId = ssm.SupplierServiceId;
                        objServwarn.SupplierServiceDetailsWarningID = ssm.SupplierServiceDetailsWarningID;
                        objServwarn.Messagefor = "Service";
                        objServwarn.WarDescription = ssm.WarDescription;
                        objServwarn.IsActive = ssm.IsActive;

                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objServwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objServwarn.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objServwarn.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objServwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objServwarn.ModifiedBy = Guid.Empty.ToString();
                            objServwarn.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objServwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objServwarn.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objServwarn.IsDeleted = true;
                            objServwarn.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objret = objsupdal.SaveUpdateSupplierServiceWarning(purpose, objServwarn);
                        if (!string.IsNullOrEmpty(objret))
                        {
                            if (objret.ToString().ToLower() == "1")
                            {
                                //MessageBox.Show("Service Warning saved successfully");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }


        public void ReteriveWarningService(string SupplierServiceIdval)
        {
            try
            {
                if (SupplierServiceIdval != "")
                {

                    ListofServicewarning = objsupdal.SupplierServiceWarningRetrive(Guid.Parse(SupplierServiceIdval));
                    if (ListofServicewarning != null && ListofServicewarning.Count > 0)
                    {
                        foreach (SupplierServiceWarning sup in ListofServicewarning)
                        {
                            if (WarningDt.Where(x => x.SupplierServiceDetailsWarningID == sup.SupplierServiceDetailsWarningID && (x.Messagefor.ToString().ToLower() == "service" || x.Messagefor.ToString().ToLower() == "supplier")).Count() == 0)
                            {
                                WarningDt.Add(sup);
                            }
                        }
                        WarningcheckExpireactive(SupplierServiceIdval);
                    }

                    if (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.Messagefor.ToString().ToLower() == "service" || x.Messagefor.ToString().ToLower() == "supplier")).Count() > 0)
                    {
                        WarningcheckExpireactive(SupplierServiceIdval);
                    }
                    if ((ListofServicewarning == null || ListofServicewarning.Count == 0) && (WarningDt==null|| WarningDt.Count==0))
                    {
                        dgServicesWarnings.ItemsSource = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void DeletewarningService(SupplierServiceWarning ssm)
        {
            try
            {
                if (dgServicesWarnings.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesBkssRates.SelectedItem;
                    SupplierServiceWarning objsuppServRates = new SupplierServiceWarning();
                    objsuppServRates.SupplierServiceId = ssm.SupplierServiceId;
                    objsuppServRates.SupplierServiceDetailsWarningID = ssm.SupplierServiceDetailsWarningID;
                    objsuppServRates.IsDeleted = true;
                    objsuppServRates.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierServiceWarning(objsuppServRates);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                            MessageBox.Show("Service Warning Deleted successfully");
                        WarningDt.Remove(WarningDt.Where(m => m.SupplierServiceDetailsWarningID == ssm.SupplierServiceDetailsWarningID && m.Messagefor.ToString().ToLower() == "service").FirstOrDefault());
                        //dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveWarningService(ssm.SupplierServiceId);
                    }

                }
                else
                {
                    ReteriveWarningService(ssm.SupplierServiceId);
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void BtnServWarningAdd_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWarning();
        }
        private void btnSerWarnDeletebkss_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceWarning ssmobjrate = dgServicesWarnings.SelectedItem as SupplierServiceWarning;
                DeletewarningService(ssmobjrate);
            }
        }
        /* Warning Service End */

        /* Warning supplier start */

        private void AddSupplierWarning()
        {
            SupplierServiceWarning sswarning;
            sswarning = new SupplierServiceWarning();
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            if (ssmobj != null)
            {
                sswarning.SupplierServiceId = ssmobj.ServiceId.ToString();
                sswarning.ValidFromwarning = DateTime.Now.Date;
                sswarning.ValidTowarning = DateTime.Now.Date;
                sswarning.Messagefor = "Supplier";
                sswarning.WarDescription = string.Empty;
                sswarning.SupplierServiceDetailsWarningID = (Guid.NewGuid()).ToString();
                sswarning.IsActive = true;

                if (WarningDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
                {
                    WarningDt = null;
                }

                WarningDt.Add(sswarning);
                // lstSupplierSM.Add(SupplierSM);
                // dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
                WarningcheckExpireactive(ssmobj.ServiceId);
            }
            else
            {
                MessageBox.Show("Please select a service");
                return;
            }

        }

        public void saveupdateWarningSupplier()
        {
            try
            {
                if (WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "supplier").ToList().Count > 0)
                {
                    foreach (SupplierServiceWarning ssmw in WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "supplier").ToList())
                    {
                        var strmsg = Suppliervalidationvaliddate(ssmw.ValidFromwarning, ssmw.ValidTowarning, "supplierwarning");
                        if (!string.IsNullOrEmpty(strmsg))
                        {
                            MessageBox.Show(strmsg);
                            return;
                        }
                    }
                    foreach (SupplierServiceWarning ssm in WarningDt.Where(x => x.Messagefor.ToString().ToLower() == "supplier").ToList())
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServicesBkss.SelectedItem;
                        SupplierServiceWarning objsuppwarn = new SupplierServiceWarning();
                        objsuppwarn.ValidFromwarning = ssm.ValidFromwarning;
                        objsuppwarn.ValidTowarning = ssm.ValidTowarning;
                        objsuppwarn.SupplierServiceId = ssm.SupplierServiceId;
                        objsuppwarn.SupplierServiceDetailsWarningID = ssm.SupplierServiceDetailsWarningID;
                        objsuppwarn.Messagefor = "Supplier";
                        objsuppwarn.WarDescription = ssm.WarDescription;
                        objsuppwarn.IsActive = ssm.IsActive;

                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objsuppwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppwarn.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppwarn.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objsuppwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppwarn.ModifiedBy = Guid.Empty.ToString();
                            objsuppwarn.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objsuppwarn.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppwarn.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objsuppwarn.IsDeleted = true;
                            objsuppwarn.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objret = objsupdal.SaveUpdateSupplierServiceWarning(purpose, objsuppwarn);
                        if (!string.IsNullOrEmpty(objret))
                        {
                            if (objret.ToString().ToLower() == "1")
                            {
                                // MessageBox.Show("Supplier warning saved successfully");
                            }


                        }
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }


        public void ReteriveWarningSupplier(string SupplierServiceIdval)
        {
            try
            {
                if (SupplierServiceIdval != "")
                {

                    ListofSuppwarning = objsupdal.SupplierServiceWarningRetrive(Guid.Parse(SupplierServiceIdval));
                    if (ListofSuppwarning != null && ListofSuppwarning.Count > 0)
                    {
                        foreach (SupplierServiceWarning sup in ListofSuppwarning)
                        {
                            if (WarningDt.Where(x => x.SupplierServiceDetailsWarningID == sup.SupplierServiceDetailsWarningID && (x.Messagefor.ToString().ToLower() == "supplier" || x.Messagefor.ToString().ToLower() == "service")).Count() == 0)
                            {
                                WarningDt.Add(sup);
                            }
                        }
                        WarningcheckExpireactive(SupplierServiceIdval);
                    }

                    if (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.Messagefor.ToString().ToLower() == "supplier" || x.Messagefor.ToString().ToLower() == "service")).Count() > 0)
                    {
                        WarningcheckExpireactive(SupplierServiceIdval);
                    }
                    if ((ListofSuppwarning == null || ListofSuppwarning.Count == 0) && (WarningDt==null || WarningDt.Count==0))
                    {
                        dgSuppWarnings.ItemsSource = null;
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        public void DeletewarningSupplier(SupplierServiceWarning ssm)
        {
            try
            {
                if (dgSuppWarnings.Items.Count > 0)
                {
                    SupplierServiceWarning objsuppwarn = new SupplierServiceWarning();
                    objsuppwarn.SupplierServiceId = ssm.SupplierServiceId;
                    objsuppwarn.SupplierServiceDetailsWarningID = ssm.SupplierServiceDetailsWarningID;
                    objsuppwarn.IsDeleted = true;
                    objsuppwarn.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierServiceWarning(objsuppwarn);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                            MessageBox.Show("Supplier Warning Deleted successfully");
                        WarningDt.Remove(WarningDt.Where(m => m.SupplierServiceDetailsWarningID == ssm.SupplierServiceDetailsWarningID && m.Messagefor.ToString().ToLower() == "supplier").FirstOrDefault());
                        //dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveWarningSupplier(ssm.SupplierServiceId);
                        ReterivePricingOption();
                        ReterivePriceEditRate();
                    }

                }
                else {
                    ReteriveWarningSupplier(ssm.SupplierServiceId);
                    ReterivePricingOption();
                    ReterivePriceEditRate();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void BtnSuppWarningAdd_Click(object sender, RoutedEventArgs e)
        {
            AddSupplierWarning();
        }

        private void btnSuppWarnDeletebkss_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceWarning ssmobjrate = dgSuppWarnings.SelectedItem as SupplierServiceWarning;
                DeletewarningSupplier(ssmobjrate);
            }
        }


        /* Warning supplier end */
        //private void chbhidenonactivewarning_Click(object sender, RoutedEventArgs e)
        //{
        //    SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
        //    ReteriveSupplierServicesRates(ssmobj.ServiceId);
        //    ReteriveWarningSupplier(ssmobj.ServiceId);
        //    ReteriveWarningService(ssmobj.ServiceId);
        //}

        private void chbhideexpiredseasonswarning_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            chbhideexpiredseasons.IsChecked = chbhideexpiredseasonswarning.IsChecked;
            //chbhideexpiredseasonsPrice.IsChecked = chbhideexpiredseasonswarning.IsChecked;
            if (ssmobj != null)
            {
                ReteriveWarningSupplier(ssmobj.ServiceId);
                ReteriveWarningService(ssmobj.ServiceId);
                ReteriveSupplierServicesRates(ssmobj.ServiceId);
                ReterivePricingOption();
                ReterivePriceEditRate();
            }
            else
            {
                MessageBox.Show("Please select service");
                return;
            }
        }

        #endregion "Supplier Warning End"

        #region "Supplier Pricing Start"

        private void PricingClickPagecall()
        {
            try
            {
                SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                if (ssmobj == null)
                {
                    MessageBox.Show("Please select a service");
                    return;
                }
                if (ssmobj.ServiceName.ToString().Trim() == string.Empty)
                {
                    MessageBox.Show("Please provide a Service Name");
                    return;
                }
                // SupplierPricingOptions.IsEnabled = true;
                // SupplierPricingOptions.IsSelected = true;
                lblserviceName.Visibility = Visibility.Visible;
                lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();

                lblRatesDate.Visibility = Visibility.Visible;
                lblRatesDate.Content = "Date: From: " + ssmobjrate.ValidFrom.ToShortDateString() + "  To: " + ssmobjrate.ValidTo.ToShortDateString();
                ReterivePricingOption();

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void dgSupplierServicesRatesUCDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PricingClickPagecall();
        }


        private void AddPricingOption()
        {
            SupplierPricingOption ssPO = new SupplierPricingOption();
            //SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
            ssPO.SupplierServiceId = ssRateobj.SupplierServiceId.ToString();
            ssPO.SupplierServiceDetailsRateId = ssRateobj.SupplierServiceDetailsRateId.ToString();
            ssPO.PricingOptionName = string.Empty;//"New Option (" + (PricingOptionDt.Count + 1) + ")";
            ssPO.PriceType = string.Empty;
            ssPO.PriceIsDefault = false;
            ssPO.NetPrice = 0;
            ssPO.MarkupPercentage = 0;
            ssPO.GrossPrice = 0;
            ssPO.CommissionPercentage = 0;
            ssPO.PricingOptionId = (Guid.NewGuid()).ToString();
            ssPO.PriceIsActive = true;
            ssPO.selectedforbkg = false;

            if (PricingOptionDt.Where(m => m.SupplierServiceId == ssRateobj.SupplierServiceId).Count() == 0)
            {
                PricingOptionDt = null;
            }
            ListofCurrencyServiceidwise = loadDropDownListValues.CurrencyinfoReterive(ssRateobj.SupplierServiceId.ToString());
            if (ListofCurrencyServiceidwise.Count > 0)
            {
                ssPO.CurrencyName = ListofCurrencyServiceidwise[0].CurrencyName;
                ssPO.CurrencyDisplayFormat = ListofCurrencyServiceidwise[0].DisplayFormat;
            }
            PricingOptionDt.Add(ssPO);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
            PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
            dgPricingoption.Focus();
            dgPricingoption.BeginEdit();

            //dgPricingoption.CurrentCell = new System.Windows.Controls.DataGridCellInfo(
            //dgPricingoption.Items[PricingOptionDt.Count - 1], dgPricingoption.Columns[4]);
            // ReterivePricingOption();
        }

        public void saveupdatePricingOption()
        {
            try
            {
                //if (dgPricingoption.Items.Count > 0)
                if (PricingOptionDt.Count > 0)
                {
                    //foreach (SupplierPricingOption ssmw in dgPricingoption.Items)
                    //{
                    //    var strmsg = Suppliervalidationvaliddate(ssmw.ValidFromwarning, ssmw.ValidTowarning, "supplierwarning");
                    //    if (!string.IsNullOrEmpty(strmsg))
                    //    {
                    //        MessageBox.Show(strmsg);
                    //        return;
                    //    }
                    //}
                    foreach (SupplierPricingOption sspo in PricingOptionDt)
                    {
                        SupplierPricingOption objpriceopt = new SupplierPricingOption();
                        objpriceopt.PricingOptionId = sspo.PricingOptionId;
                        objpriceopt.PricingOptionName = sspo.PricingOptionName;
                        objpriceopt.NetPrice = decimal.Round(sspo.NetPrice, 2, MidpointRounding.AwayFromZero);
                        objpriceopt.MarkupPercentage = decimal.Round(sspo.MarkupPercentage, 2, MidpointRounding.AwayFromZero);
                        objpriceopt.GrossPrice = decimal.Round(sspo.GrossPrice, 2, MidpointRounding.AwayFromZero);
                        objpriceopt.CommissionPercentage = sspo.CommissionPercentage;
                        objpriceopt.PriceType = ((!string.IsNullOrEmpty(sspo.PriceType)) ? Guid.Parse(sspo.PriceType) : Guid.Empty).ToString();
                        objpriceopt.PriceIsDefault = sspo.PriceIsDefault;
                        objpriceopt.PriceIsActive = sspo.PriceIsActive;

                        objpriceopt.SupplierServiceDetailsRateId = sspo.SupplierServiceDetailsRateId;
                        objpriceopt.SupplierServiceId = sspo.SupplierServiceId;

                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objpriceopt.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objpriceopt.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objpriceopt.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objpriceopt.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objpriceopt.ModifiedBy = Guid.Empty.ToString();
                            objpriceopt.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objpriceopt.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objpriceopt.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objpriceopt.IsDeleted = true;
                            objpriceopt.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objret = objsupdal.SaveUpdatePricingoption(purpose, objpriceopt);
                        if (!string.IsNullOrEmpty(objret))
                        {
                            if (objret.ToString().ToLower() == "1")
                            {
                                // MessageBox.Show("Supplier warning saved successfully");
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }




        public void ReterivePriceEditRate()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                if (ssRateobj != null)
                {
                    SupplierPriceRateEdit supplierPriceRateEdit = new SupplierPriceRateEdit();

                    // ListofPricingOption = objsupdal.PricingOptionRetrive(Guid.Parse(ssRateobj.SupplierServiceId), Guid.Parse(ssRateobj.SupplierServiceDetailsRateId));
                    if (PriceEditDt != null && PriceEditDt.Count > 0)
                    {
                        foreach (SupplierPricingOption sup in dgPricingoption.Items)
                        {
                            if (PricingOptionDt.Where(x => x.PricingOptionId == sup.PricingOptionId).Count() > 0)
                            {
                                if (PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).Count() > 0)
                                {
                                    sup.NetPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().NetPrice;
                                    sup.MarkupPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().MarkupPercentage;
                                    sup.GrossPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().GrossPrice;
                                    sup.CommissionPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().CommissionPercentage;

                                }
                            }
                            else
                            {
                                if (PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).Count() > 0)
                                {
                                    sup.NetPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().NetPrice;
                                    sup.MarkupPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().MarkupPercentage;
                                    sup.GrossPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().GrossPrice;
                                    sup.CommissionPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().CommissionPercentage;

                                }
                                //PricingOptionDt.Add(sup);
                            }

                        }
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }


                    if (PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() > 0)
                    {
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    if ((ListofPricingOption == null || ListofPricingOption.Count == 0 )&&(PricingOptionDt.Count==0 || PricingOptionDt==null))
                    {
                        dgPricingoption.ItemsSource = null;
                    }
                }
                else
                {
                    dgPricingoption.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        public void DeletePricingOption(SupplierPricingOption ssm)
        {
            try
            {
                if (dgPricingoption.Items.Count > 0)
                {
                    SupplierServiceRatesDt ssRateobj = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
                    SupplierPricingOption objsupppropt = new SupplierPricingOption();
                    objsupppropt.SupplierServiceId = ssm.SupplierServiceId;
                    objsupppropt.SupplierServiceDetailsRateId = ssm.SupplierServiceDetailsRateId;
                    objsupppropt.PricingOptionId = ssm.PricingOptionId;
                    objsupppropt.IsDeleted = true;
                    objsupppropt.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeletePricingOption(objsupppropt);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                            MessageBox.Show("Supplier Pricing option Deleted successfully");
                        PricingOptionDt.Remove(PricingOptionDt.Where(m => m.PricingOptionId == ssm.PricingOptionId).FirstOrDefault());
                        //dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        //PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                        ReterivePricingOption();
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void BtnPriceAddbkss_Click(object sender, RoutedEventArgs e)
        {

            // SupplierServiceModels ssmobj = dgSupplierServicesBkss.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesBkssRates.SelectedItem as SupplierServiceRatesDt;
            if (ssRateobj != null)
            {
                AddPricingOption();
            }
            else
            {
                MessageBox.Show("Please Select Supplier Service Rates");
                return;
            }
        }

        private void btnPricingOptionDeletebkss_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
                DeletePricingOption(objPO);
            }
        }

        //private void PricingOptcheckExpireactive(string SupplierServiceIdval, string supprateid, bool supprateexpired)
        //{
        //    try
        //    {
        //        //if (chbhideexpiredseasonsPrice.IsChecked == true && chbhidenonactivePrice.IsChecked == true)
        //        //{
        //        //    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList());
        //        //}
        //        //else if (chbhidenonactivePrice.IsChecked == true)
        //        //{
        //        //    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList();
        //        //}
        //        //else if (chbhideexpiredseasonsPrice.IsChecked == true)
        //        //{
        //        //    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && x.SupplierServiceDetailsRateId == supprateid).ToList();
        //        //}
        //        //else
        //        //{
        //        //    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
        //        //}

        //        dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
        //    }
        //    catch (Exception ex)
        //    {
        //        errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
        //    }
        //}



        private void btnPriceEditbkss_Click(object sender, RoutedEventArgs e)
        {
            SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
            if (objPO != null)
            {
                if (!string.IsNullOrEmpty(objPO.PricingOptionName))
                {
                    SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, null, null, this);
                    wobj.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Please provide a option name");
                    return;
                }
            }

        }

        public void saveupdatePriceEditRate()
        {
            try
            {
                if (PriceEditDt.Count > 0)
                {
                    //foreach (SupplierPricingOption ssmw in dgPricingoption.Items)
                    //{
                    //    var strmsg = Suppliervalidationvaliddate(ssmw.ValidFromwarning, ssmw.ValidTowarning, "supplierwarning");
                    //    if (!string.IsNullOrEmpty(strmsg))
                    //    {
                    //        MessageBox.Show(strmsg);
                    //        return;
                    //    }
                    //}

                    foreach (SupplierPriceRateEdit sspo in PriceEditDt)
                    {
                        SupplierPriceRateEdit objPriceRateEdit = new SupplierPriceRateEdit();
                        objPriceRateEdit.PricingOptionId = sspo.PricingOptionId;
                        objPriceRateEdit.PriceEditRateId = sspo.PriceEditRateId;

                        objPriceRateEdit.NetPrice = decimal.Round(sspo.NetPrice, 2, MidpointRounding.AwayFromZero);
                        objPriceRateEdit.MarkupPercentage = decimal.Round(sspo.MarkupPercentage, 2, MidpointRounding.AwayFromZero);
                        objPriceRateEdit.GrossPrice = decimal.Round(sspo.GrossPrice, 2, MidpointRounding.AwayFromZero);
                        objPriceRateEdit.CommissionPercentage = sspo.CommissionPercentage;
                        objPriceRateEdit.SupplierServiceId = sspo.SupplierServiceId;
                        objPriceRateEdit.Monday = sspo.Monday;
                        objPriceRateEdit.Tuesday = sspo.Tuesday;
                        objPriceRateEdit.Wednesday = sspo.Wednesday;
                        objPriceRateEdit.Thursday = sspo.Thursday;
                        objPriceRateEdit.Friday = sspo.Friday;
                        objPriceRateEdit.Saturday = sspo.Saturday;
                        objPriceRateEdit.Sunday = sspo.Sunday;
                        // objPriceRateEdit.RatevalidFromDay = sspo.RatevalidFromDay;
                        // objPriceRateEdit.RatevalidToDay = sspo.RatevalidToDay;                    
                        objPriceRateEdit.SupplierServiceId = sspo.SupplierServiceId;
                        objPriceRateEdit.PriceEditIsActive = sspo.PriceEditIsActive;
                        objPriceRateEdit.ChooseEditOption = sspo.ChooseEditOption;
                        objPriceRateEdit.Rounding = sspo.Rounding;
                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objPriceRateEdit.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objPriceRateEdit.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objPriceRateEdit.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objPriceRateEdit.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objPriceRateEdit.ModifiedBy = Guid.Empty.ToString();
                            objPriceRateEdit.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objPriceRateEdit.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objPriceRateEdit.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objPriceRateEdit.IsDeleted = true;
                            objPriceRateEdit.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        if (objPriceRateEdit.NetPrice > 0 && objPriceRateEdit.GrossPrice > 0)
                        {
                            string objret = objsupdal.SaveUpdatePriceEditRate(purpose, objPriceRateEdit);
                            if (!string.IsNullOrEmpty(objret))
                            {
                                if (objret.ToString().ToLower() == "1")
                                {
                                    // MessageBox.Show("Supplier warning saved successfully");
                                }


                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void dgSupplierServicesBkssRates_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgSupplierServicesBkssRates.BeginEdit();
        }

        private void dgServicesWarnings_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgServicesWarnings.BeginEdit();
        }

        private void dgSuppWarnings_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgSuppWarnings.BeginEdit();
        }


        #endregion "Supplier Pricing End"




        /* Tab 2 code end */
    }
}
