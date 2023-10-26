//using Microsoft.Windows.Controls;
using LITModels;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for SupplierService.xaml
    /// </summary>
    public partial class SupplierServiceUC : Window
    {
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        string SupplierFoldersettingurl = string.Empty;
        List<CommonValueCountrycity> ListofCountry = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofRegion = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofState = new List<CommonValueCountrycity>();
        List<CommonValueCountrycity> ListofCity = new List<CommonValueCountrycity>();
        List<SupplierServiceType> ListofSupplierServiceType { get; set; }
        private MainWindow _parentWindow;



        SupplierServiceModels objSuppservicemdl = new SupplierServiceModels();
        List<SupplierServiceModels> ListofSuppservice = new List<SupplierServiceModels>();
        List<SupplierServiceRatesDt> ListofSuppserviceRates = new List<SupplierServiceRatesDt>();
        List<SupplierServiceWarning> ListofSuppwarning = new List<SupplierServiceWarning>();
        List<SupplierServiceWarning> ListofServicewarning = new List<SupplierServiceWarning>();
        List<SupplierPricingOption> ListofPricingOption = new List<SupplierPricingOption>();
        List<CommonValueList> ListofGroupinfo = new List<CommonValueList>();
        List<Currencydetails> ListofCurrency = new List<Currencydetails>();
        List<Currencydetails> ListofCurrencyServiceidwise = new List<Currencydetails>();
        // public bool Chbhidenonactive_Checked { get; set; }

        DateTime NewStartDateVal;
        RefreshRates RefreshRatesVal;
        long bkgidval;

        Errorlog errobj = new Errorlog();
        DBConnectionEF DBconnEF = new DBConnectionEF();

        private ObservableCollection<SupplierPriceRateEdit> _PriceEditDt;
        public ObservableCollection<SupplierPriceRateEdit> PriceEditDt
        {
            get { return _PriceEditDt ?? (_PriceEditDt = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEditDt = value;
            }
        }

        public decimal nightdaysval = 0;
        public SupplierServiceUC()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public SupplierServiceUC(string loginusernameval, string SupplierIdval, string serviceid, long bkgid, DateTime? NewStartDate = null, decimal nightdays=0, RefreshRates refrates = null)
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername = loginusernameval;
            hdnSupplierid.Text = SupplierIdval;
            hdnserviceid.Text = serviceid;
            nightdaysval = nightdays;
            hdnloginusername.Text = loginusername;
            if (NewStartDate != null)
            {
                NewStartDateVal = (DateTime)NewStartDate;
                hdnNewStartDate.SelectedDate = NewStartDate;
            }
            if (refrates != null)
            {
                RefreshRatesVal = refrates;
            }
            if (bkgid > 0)
            {
                bkgidval = bkgid;
            }
            Supplierloadcmbvalues();
            ReteriveSupplierServices(SupplierIdval, serviceid);
            ReteriveSupplierServicesRates(serviceid);
            ReterivePricingOption();
            // this.Parent.CloseButtonClicked += btnSelectforbkg_Click;


        }

        public void Supplierloadcmbvalues()
        {
            try
            {
                CommonValueCountrycity objCVCC
                = new CommonValueCountrycity();

                SupplierFoldersettingurl = loadDropDownListValues.LoadFolderName("SupplierFolder");

                SupplierServiceType objSST
                   = new SupplierServiceType();
                ListofSupplierServiceType = new List<SupplierServiceType>();
                ListofSupplierServiceType = loadDropDownListValues.LoadSupplierServiceTypes();
                if (ListofSupplierServiceType != null && ListofSupplierServiceType.Count > 0)
                {
                    CmbSupplierServiceTypeUC.ItemsSource = ListofSupplierServiceType;
                    CmbSupplierServiceTypeUC.SelectedValuePath = "ServiceTypeID";
                    CmbSupplierServiceTypeUC.DisplayMemberPath = "ServiceTypeName";
                    if (ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault() != null)
                    {
                        CmbSupplierServiceTypeUC.SelectedValuePath = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault().ServiceTypeID;
                        //  objSST.Defaultselectedtype = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault();

                        // CmbserviceType.SelectedItemBinding = objSST.Defaultselectedtype.ServiceTypeID;
                    }
                }


                ListofGroupinfo = new List<CommonValueList>();
                ListofGroupinfo = loadDropDownListValues.LoadGroupinfo("Group Info");
                if (ListofGroupinfo != null && ListofGroupinfo.Count > 0)
                {
                    CmbChargesgroupinfoUC.ItemsSource = ListofGroupinfo;
                    CmbChargesgroupinfoUC.SelectedValuePath = "ValueField";
                    CmbChargesgroupinfoUC.DisplayMemberPath = "TextField";
                }

                ListofCurrency = new List<Currencydetails>();
                ListofCurrency = loadDropDownListValues.LoadCurrencyDetails();
                if (ListofCurrency != null && ListofCurrency.Count > 0)
                {
                    CmbCurrencyDetailsUC.ItemsSource = ListofCurrency;
                    CmbCurrencyDetailsUC.SelectedValuePath = "CurrencyID";
                    CmbCurrencyDetailsUC.DisplayMemberPath = "CurrencyName";

                    if (ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault() != null)
                    {
                        CmbCurrencyDetailsUC.SelectedValuePath = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().CurrencyID;
                    }
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        #region "Supplier Service Start"




        // List<SupplierServiceModels> ObjSSM=new List<SupplierServiceModels>();

        private ObservableCollection<SupplierServiceModels> _SupplierSM;
        public ObservableCollection<SupplierServiceModels> SupplierSM
        {
            get { return _SupplierSM ?? (_SupplierSM = new ObservableCollection<SupplierServiceModels>()); }
            set
            {
                _SupplierSM = value;
            }
        }
        //private List<ObservableCollection<SupplierServiceModels>> _lstSupplierSM;
        //public List<ObservableCollection<SupplierServiceModels>> lstSupplierSM
        //{
        //    get { return _lstSupplierSM ?? (_lstSupplierSM = new List<ObservableCollection<SupplierServiceModels>>()); }
        //    set
        //    {
        //        _lstSupplierSM = value;
        //    }
        //}

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
            dgSupplierServicesuc.ItemsSource = SupplierSM;

            dgSupplierServicesuc.Focus();
            dgSupplierServicesuc.BeginEdit();
            // dgSupplierServicesuc.CurrentCell = new System.Windows.Controls.DataGridCellInfo(
            // dgSupplierServicesuc.Items[SupplierSM.Count-1], dgSupplierServicesuc.Columns[3]);
        }

        public void saveupdateSupplierServices()
        {
            try
            {
                if (dgSupplierServicesuc.Items.Count > 0)
                {
                    if (SupplierSM.Where(m => m.ServiceName.ToString().Trim() == string.Empty).Count() > 0)
                    {
                        MessageBox.Show("Please provide a Service Name");
                        return;
                    }

                    foreach (SupplierServiceModels ssm in dgSupplierServicesuc.Items)
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServicesuc.SelectedItem;
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



        public void ReteriveSupplierServices(string SupplierIdval, string ServiceId = null)
        {
            try
            {
                if (SupplierIdval != "")
                {
                    // List<SupplierServiceModels> ListofSuppservice = new List<SupplierServiceModels>();

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
                        // lstSupplierSM.Add(SupplierSM);
                        // ListofSuppservice
                        if (chbhidenonactive.IsChecked == false)
                        {
                            dgSupplierServicesuc.ItemsSource = SupplierSM;
                            if (ServiceId != null)
                            {
                                int k = SupplierSM.Where(x => x.SupplierId == SupplierIdval).ToList().FindIndex(x => x.ServiceId == ServiceId.ToString());
                                if (k >= 0)
                                {
                                    CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesuc, k);
                                }
                            }
                        }
                        else
                        {
                            dgSupplierServicesuc.ItemsSource = SupplierSM.Where(x => x.IsActive == true).ToList();
                            if (ServiceId != null)
                            {
                                int k = SupplierSM.Where(x => x.SupplierId == SupplierIdval).ToList().FindIndex(x => x.ServiceId == ServiceId.ToString());
                                if (k >= 0)
                                {
                                    CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesuc, k);
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

        public void DeleteSupplierServices(SupplierServiceModels ssm)
        {
            try
            {
                if (dgSupplierServicesuc.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesuc.SelectedItem;
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
                        // dgSupplierServicesuc.ItemsSource = SupplierSM;
                        ReteriveSupplierServices(ssm.SupplierId);


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void BtnServiceAddUc_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }



        private void btnServiceDelete_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
                DeleteSupplierServices(ssmobj);

            }
        }

        //private void chbhidenonactive_Checked(object sender, RoutedEventArgs e)
        //{
        //    //if (Chbhidenonactive_Checked == true)
        //    //{
        //        if (dgSupplierServicesuc != null)
        //        {
        //          //  Chbhidenonactive_Checked = false;
        //            ReteriveSupplierServices(hdnSupplierid.Text.Trim());
        //            dgSupplierServicesuc.Columns[2].Visibility = Visibility.Hidden;
        //            chbhidenonactivePrice.IsChecked = chbhidenonactive.IsChecked;
        //            if (dgPricingoption != null)
        //            {
        //                ReterivePricingOption();
        //                dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
        //            }
        //    }
        //    //}
        //    //if (Chbhidenonactive_Checked == false) return;


        //}
        //private void chbhidenonactive_UnChecked(object sender, RoutedEventArgs e)
        //{
        //    //if (Chbhidenonactive_Checked == false)
        //    //{
        //        if (dgSupplierServicesuc != null)
        //        {
        //            //Chbhidenonactive_Checked = true;
        //            ReteriveSupplierServices(hdnSupplierid.Text.Trim());
        //            dgSupplierServicesuc.Columns[2].Visibility = Visibility.Visible;
        //            chbhidenonactivePrice.IsChecked = chbhidenonactive.IsChecked;
        //            if (dgPricingoption != null)
        //            {
        //                ReterivePricingOption();
        //                dgPricingoption.Columns[3].Visibility = Visibility.Visible;
        //            }
        //    }
        //    //}

        //    //if (Chbhidenonactive_Checked == true) return;
        //}

        #endregion "Supplier Service End"

        #region "Supplier Rates Start"

        private void ServiceRatesPageCall()
        {
            try
            {

                SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
                if (ssmobj.SupplierId.ToString().Trim() == string.Empty)
                {
                    MessageBox.Show("Please provide a Service Name");
                    return;
                }
                if (ssmobj.IsActive == false)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Selected Service is Non-Active, Do you really want to proceed?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        SupplierRatesCore.IsEnabled = true;
                        SupplierRatesCore.IsSelected = true;
                        lblserviceNameUC.Visibility = Visibility.Visible;
                        lblserviceNameUC.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                        lblRatesDateUC.Content = string.Empty;
                        ListofSuppserviceRates = null;
                        SupplierSRatesDt = null;
                        chbhideexpiredseasons.IsChecked = true;
                        chbhideexpiredseasonswarning.IsChecked = true;
                        ListofSuppwarning = null;
                        ListofServicewarning = null;
                        WarningDt = null;
                        ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        ReteriveWarningService(ssmobj.ServiceId);
                        ReteriveWarningSupplier(ssmobj.ServiceId);
                        ReterivePricingOption();
                        ReterivePriceEditRate();
                    }
                }
                else
                {

                    //SupplierRates.IsEnabled = true;
                    //SupplierRates.IsSelected = true;
                    lblserviceNameUC.Visibility = Visibility.Visible;
                    lblserviceNameUC.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                    lblRatesDateUC.Content = string.Empty;
                    ListofSuppserviceRates = null;
                    SupplierSRatesDt = null;
                    chbhideexpiredseasons.IsChecked = true;
                    chbhideexpiredseasonswarning.IsChecked = true;
                    ListofSuppwarning = null;
                    ListofServicewarning = null;
                    WarningDt = null;
                    ReteriveSupplierServicesRates(ssmobj.ServiceId);
                    ReteriveWarningService(ssmobj.ServiceId);
                    ReteriveWarningSupplier(ssmobj.ServiceId);
                    ReterivePricingOption();
                    ReterivePriceEditRate();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void btnServiceRates_Click(object sender, RoutedEventArgs e)
        {
            ServiceRatesPageCall();
        }

        private void dgSupplierServicesucDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if ((((System.Windows.Controls.DataGridCell)sender).Column.Header.ToString().ToLower() != "currency") && (((System.Windows.Controls.DataGridCell)sender).Column.Header.ToString().ToLower() != "charges"))
            //{
                ServiceRatesPageCall();
           // }
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
        private void AddRates()
        {
            SupplierServiceRatesDt ssRates;
            ssRates = new SupplierServiceRatesDt();
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
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


        private void BtnRatesAdd_Click(object sender, RoutedEventArgs e)
        {
            AddRates();
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
                        cmb.SelectedItem = dgSupplierServicesuc.SelectedItem;
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


        public void ReteriveSupplierServicesRates(string SupplierServiceIdval)
        {
            try
            {
                if (SupplierServiceIdval != "")
                {

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
                        RatecheckExpireactive(SupplierServiceIdval);
                    }
                    if (ListofSuppserviceRates == null || ListofSuppserviceRates.Count == 0)
                    {
                        dgSupplierServicesRatesUC.ItemsSource = null;
                    }
                    if (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).Count() > 0)
                    {
                        RatecheckExpireactive(SupplierServiceIdval);
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void DeleteSupplierServicesRates(SupplierServiceRatesDt ssm)
        {
            try
            {
                if (dgSupplierServicesRatesUC.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesRatesUC.SelectedItem;
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
        private void btnRatesDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
                DeleteSupplierServicesRates(ssmobjrate);

            }
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


        private void RatecheckExpireactive(string SupplierServiceIdval)
        {
            //if (chbhideexpiredseasons.IsChecked == true && chbhidenonactive.IsChecked == true)
            //{
            //    dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.IsActive == false).ToList();
            //}
            //else

            DateTime ist = ((DateTime)hdnNewStartDate.SelectedDate);
            DateTime isend = ((DateTime)hdnNewStartDate.SelectedDate).AddDays(Convert.ToInt32(nightdaysval));
            string ssrateid = string.Empty;
            List<Tuple<DateTime, DateTime>> dateRanges = null;
            dateRanges = new List<Tuple<DateTime, DateTime>>
            {


            };
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
            string servicetypeid = ((SQLDataAccessLayer.Models.SupplierServiceType)ssmobj.SelectedItem).ServiceTypeID;
            string servicetypename = DBconnEF.GetServicetypename(servicetypeid);
            DateTime? endate = null;
            if (!string.IsNullOrEmpty(servicetypename))
            {
                if (servicetypename.ToLower() == "accommodation")
                {
                    endate = ((DateTime)hdnNewStartDate.SelectedDate).AddDays(Convert.ToInt32(nightdaysval));
                }
                else
                {
                    endate = ((DateTime)hdnNewStartDate.SelectedDate);
                }
            }
            if (chbhideexpiredseasons.IsChecked == true)
            {
                dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false).ToList();

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
                            if (result1 != -1)
                            {
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesRatesUC, (k == 0) ? 0 : (k));
                                break;
                            }
                            k++;
                        }
                    }
                    else
                    {
                        CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesRatesUC, (result == 0) ? 0 : (result));

                    }
                }

            }
            //else if (chbhidenonactive.IsChecked == true)
            //{
            //    dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsActive == false).ToList();
            //}
            else
            {
                dgSupplierServicesRatesUC.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).ToList();
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
                                CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesRatesUC, (result1 == 0) ? 0 : (result1));
                                break;
                            }
                        }
                    }
                    else
                    {
                        CommonAndCalcuation.SelectRowByIndex(dgSupplierServicesRatesUC, (result == 0) ? 0 : (result));

                    }
                }
            }
        }


        private void chbhideexpiredseasons_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
            chbhideexpiredseasonswarning.IsChecked = chbhideexpiredseasons.IsChecked;
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
                MessageBox.Show("Please select Service");
                return;
            }
        }

        #endregion "Supplier Rates End"

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
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
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
                        cmb.SelectedItem = dgSupplierServicesuc.SelectedItem;
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
                    if (ListofServicewarning == null || ListofServicewarning.Count == 0)
                    {
                        dgServicesWarnings.ItemsSource = null;
                    }
                    if (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.Messagefor.ToString().ToLower() == "service" || x.Messagefor.ToString().ToLower() == "supplier")).Count() > 0)
                    {
                        WarningcheckExpireactive(SupplierServiceIdval);
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
                    cmb.SelectedItem = dgSupplierServicesRatesUC.SelectedItem;
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
        private void btnSerWarnDelete_Click(object sender, RoutedEventArgs e)
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
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
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
                        cmb.SelectedItem = dgSupplierServicesuc.SelectedItem;
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
                    if (ListofSuppwarning == null || ListofSuppwarning.Count == 0)
                    {
                        dgSuppWarnings.ItemsSource = null;
                    }
                    if (WarningDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.Messagefor.ToString().ToLower() == "supplier" || x.Messagefor.ToString().ToLower() == "service")).Count() > 0)
                    {
                        WarningcheckExpireactive(SupplierServiceIdval);
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

        private void btnSuppWarnDelete_Click(object sender, RoutedEventArgs e)
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
        //    SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
        //    ReteriveSupplierServicesRates(ssmobj.ServiceId);
        //    ReteriveWarningSupplier(ssmobj.ServiceId);
        //    ReteriveWarningService(ssmobj.ServiceId);
        //}

        private void chbhideexpiredseasonswarning_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
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
                SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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
                lblserviceNameUC.Visibility = Visibility.Visible;
                lblserviceNameUC.Content = "Service Name: " + ssmobj.ServiceName.ToString();

                lblRatesDateUC.Visibility = Visibility.Visible;
                lblRatesDateUC.Content = "Date: From: " + ssmobjrate.ValidFrom.ToShortDateString() + "  To: " + ssmobjrate.ValidTo.ToShortDateString();
                ReterivePricingOption();

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void btnPricing_Click(object sender, RoutedEventArgs e)
        {
            PricingClickPagecall();
        }
        private void dgSupplierServicesRatesUCDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PricingClickPagecall();
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

        private void AddPricingOption()
        {
            SupplierPricingOption ssPO = new SupplierPricingOption();
            //SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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


        public void ReterivePricingOption()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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
                            }
                        }
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    if (ListofPricingOption == null || ListofPricingOption.Count == 0)
                    {
                        dgPricingoption.ItemsSource = null;
                    }
                    if (PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() > 0)
                    {
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    if ((PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() == 0)
                            && (ListofPricingOption == null || ListofPricingOption.Count == 0))
                    {
                        dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId);
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


        public void ReterivePriceEditRate()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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

                    if (ListofPricingOption == null || ListofPricingOption.Count == 0)
                    {
                        dgPricingoption.ItemsSource = null;
                    }
                    if (PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() > 0)
                    {
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
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
                    SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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

        private void BtnPriceAdd_Click(object sender, RoutedEventArgs e)
        {

            // SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
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

        private void btnPricingOptionDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
                DeletePricingOption(objPO);
            }
        }

        private void PricingOptcheckExpireactive(string SupplierServiceIdval, string supprateid, bool supprateexpired)
        {
            try
            {
                //if (chbhideexpiredseasonsPrice.IsChecked == true && chbhidenonactivePrice.IsChecked == true)
                //{
                //    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList());
                //}
                //else if (chbhidenonactivePrice.IsChecked == true)
                //{
                //    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList();
                //}
                //else if (chbhideexpiredseasonsPrice.IsChecked == true)
                //{
                //    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && x.SupplierServiceDetailsRateId == supprateid).ToList();
                //}
                //else
                //{
                //    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
                //}

                dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        private void btnPriceEdit_Click(object sender, RoutedEventArgs e)
        {
            SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
            if (objPO != null)
            {
                if (!string.IsNullOrEmpty(objPO.PricingOptionName))
                {
                    SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, null,this);
                    wobj.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Please provide a option name");
                    return;
                }
            }

        }
        private void dgPricingoptionDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
            if (objPO != null)
            {
                if (!string.IsNullOrEmpty(objPO.PricingOptionName))
                {
                    SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, null, this);
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

        #endregion "Supplier Pricing End"

        //private void chbhidenonactivePrice_Click(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        if (dgSupplierServicesuc != null)
        //        {
        //           // chbhidenonactive.IsChecked = chbhidenonactivePrice.IsChecked;
        //            ReteriveSupplierServices(hdnSupplierid.Text.Trim());


        //            if (chbhidenonactivePrice.IsChecked == true)
        //            {
        //                dgSupplierServicesuc.Columns[2].Visibility = Visibility.Hidden;
        //                if (dgPricingoption != null)
        //                {
        //                    ReterivePricingOption();
        //                    dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
        //                }
        //            }
        //            if (chbhidenonactivePrice.IsChecked == false)
        //            {
        //                dgSupplierServicesuc.Columns[2].Visibility = Visibility.Visible;
        //                if (dgPricingoption != null)
        //                {
        //                    ReterivePricingOption();
        //                    dgPricingoption.Columns[3].Visibility = Visibility.Visible;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
        //    }
        //}

        private void chbhideexpiredseasonsPrice_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
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
                MessageBox.Show("Please select Service");
                return;
            }

        }

        private void chbhidenonactive_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgSupplierServicesuc != null)
                {
                    SupplierServiceModels ssmobj = dgSupplierServicesuc.SelectedItem as SupplierServiceModels;
                    // chbhidenonactivePrice.IsChecked = chbhidenonactive.IsChecked;


                    if (chbhidenonactive.IsChecked == false)
                    {
                        ReteriveSupplierServices(hdnSupplierid.Text.Trim());
                        if (ssmobj != null)
                        {
                            ReteriveWarningSupplier(ssmobj.ServiceId);
                            ReteriveWarningService(ssmobj.ServiceId);
                            ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        }
                        dgSupplierServicesuc.Columns[2].Visibility = Visibility.Visible;
                        if (dgPricingoption != null)
                        {
                            ReterivePricingOption();
                            ReterivePriceEditRate();
                            dgPricingoption.Columns[3].Visibility = Visibility.Visible;
                        }
                    }
                    if (chbhidenonactive.IsChecked == true)
                    {
                        ReteriveSupplierServices(hdnSupplierid.Text.Trim());
                        dgPricingoption.ItemsSource = null;
                        dgServicesWarnings.ItemsSource = null;
                        dgSupplierServicesRatesUC.ItemsSource = null;
                        dgSuppWarnings.ItemsSource = null;
                        dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
                        dgSupplierServicesuc.Columns[2].Visibility = Visibility.Hidden;
                        //if (ssmobj != null)
                        //{
                        //    ReteriveWarningSupplier(ssmobj.ServiceId);
                        //    ReteriveWarningService(ssmobj.ServiceId);
                        //    ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        //}

                        //if (dgPricingoption != null)
                        //{
                        //    ReterivePricingOption();
                        //    ReterivePriceEditRate();

                        //}
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
        private void msPenLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#579F00"));
        }

        private void msPenEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#79D10D"));
        }


        /* Event Handler for usercontrol*/

        public event EventHandler Btncancelclickevt;

        public event EventHandler BtnOKclickevtChld;
        private void btncancelforssc_Click(object sender, RoutedEventArgs e)
        {
            //EventHandler evthdl = Btncancelclickevt;
            //if(evthdl!=null)
            //    evthdl(sender,e);

            this.Close();

        }

        public void btnSelectforbkg_Click(object sender, RoutedEventArgs e)
        {
            if (validation() == 0)
            {
                System.Windows.MessageBox.Show("Please select pricing option");
                return;
            }
            else
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesRatesUC.SelectedItem as SupplierServiceRatesDt;
                if (ssRateobj != null && (hdnNewStartDate.SelectedDate != null && hdnNewStartDate.SelectedDate != DateTime.MinValue))
                {
                    string mess = "Selected rate is not valid for current booking date of " + hdnNewStartDate.SelectedDate.Value.ToShortDateString() + " \r\n\r\n Continue?";
                    string supmes = "You have made changes to this supplier" + "\r\n\r\n Do you want to save the changes?";
                    if ((ssRateobj.ValidTo < hdnNewStartDate.SelectedDate.Value && ssRateobj.ValidFrom < hdnNewStartDate.SelectedDate.Value)
                        || (ssRateobj.ValidTo > hdnNewStartDate.SelectedDate.Value && ssRateobj.ValidFrom > hdnNewStartDate.SelectedDate.Value))
                    {
                        MessageBoxResult msrs = MessageBox.Show(mess, "Message", MessageBoxButton.YesNo);
                        if (msrs == MessageBoxResult.Yes)
                        {
                            MessageBoxResult msrssupl = MessageBox.Show(supmes, "Message", MessageBoxButton.YesNo);
                            if (msrssupl == MessageBoxResult.Yes)
                            {
                                //hdnSupplierid.Text= 
                                recordmode = "Save";
                                saveupdateSupplierServices();
                                saveupdateSupplierServicesRates();
                                saveupdateWarningSupplier();
                                saveupdateWarningService();
                                saveupdatePricingOption();
                                if (PriceEditDt.Count > 0)
                                {
                                    saveupdatePriceEditRate();
                                }
                                var list = PricingOptionDt.Where(x => x.SupplierServiceId == ssRateobj.SupplierServiceId && x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList();
                                if (list != null)
                                {
                                    //foreach (SupplierPricingOption supl in list)
                                    //{
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetunit = list[0].NetPrice.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetUnitNotinSupptbl = true;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewGrossunit = list[0].GrossPrice.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewMarkupPercentage = list[0].MarkupPercentage.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().RefreshRateEditedflag = true;
                                    Image SuccessImage = new Image();
                                    BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                                    SuccessImage.Source = SuccessImagebitmapImage;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsgurl = SuccessImagebitmapImage;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsg = "";
                                    // }

                                }

                                this.Close();

                            }
                            else
                            {
                                var list = PricingOptionDt.Where(x => x.SupplierServiceId == ssRateobj.SupplierServiceId && x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList();
                                if (list != null)
                                {
                                    //foreach (SupplierPricingOption supl in list)
                                    //{
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetunit = list[0].NetPrice.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetUnitNotinSupptbl = true;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewGrossunit = list[0].GrossPrice.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewMarkupPercentage = list[0].MarkupPercentage.ToString();
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().RefreshRateEditedflag = true;
                                    Image SuccessImage = new Image();
                                    BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                                    SuccessImage.Source = SuccessImagebitmapImage;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsgurl = SuccessImagebitmapImage;
                                    RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsg = "";
                                    // }

                                }

                                this.Close();

                            }
                        }
                        else
                        {

                        }
                    }
                    else
                    {
                        MessageBoxResult msrssupl = MessageBox.Show(supmes, "Message", MessageBoxButton.YesNo);
                        if (msrssupl == MessageBoxResult.Yes)
                        {
                            //hdnSupplierid.Text= 
                            recordmode = "Save";
                            saveupdateSupplierServices();
                            saveupdateSupplierServicesRates();
                            saveupdateWarningSupplier();
                            saveupdateWarningService();
                            saveupdatePricingOption();
                            if (PriceEditDt.Count > 0)
                            {
                                saveupdatePriceEditRate();
                            }
                            var list = PricingOptionDt.Where(x => x.SupplierServiceId == ssRateobj.SupplierServiceId && x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList();
                            if (list != null)
                            {
                                //foreach (SupplierPricingOption supl in list)
                                //{
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetunit = list[0].NetPrice.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetUnitNotinSupptbl = true;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewGrossunit = list[0].GrossPrice.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewMarkupPercentage = list[0].MarkupPercentage.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().RefreshRateEditedflag = true;
                                Image SuccessImage = new Image();
                                BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                                SuccessImage.Source = SuccessImagebitmapImage;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsgurl = SuccessImagebitmapImage;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsg = "";
                                // }

                            }

                            this.Close();

                        }
                        else
                        {
                            var list = PricingOptionDt.Where(x => x.SupplierServiceId == ssRateobj.SupplierServiceId && x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList();
                            if (list != null)
                            {
                                //foreach (SupplierPricingOption supl in list)
                                //{
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetunit = list[0].NetPrice.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewNetUnitNotinSupptbl = true;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewGrossunit = list[0].GrossPrice.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().NewMarkupPercentage = list[0].MarkupPercentage.ToString();
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().RefreshRateEditedflag = true;
                                Image SuccessImage = new Image();
                                BitmapImage SuccessImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "check"), UriKind.Relative));
                                SuccessImage.Source = SuccessImagebitmapImage;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsgurl = SuccessImagebitmapImage;
                                RefreshRatesVal.BkingItemsRefreshrates.Where(x => x.BookingID == bkgidval).FirstOrDefault().Resultmsg = "";



                                // }

                            }

                            this.Close();

                        }
                    }
                }

            }
            // base.OnKeyDown(e);
            //MessageBox.Show("Key Down Fired!");


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
                errobj.WriteErrorLoginfo("SupplierServiceUC", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            return cnt;
        }

        private void Txtvalidfrom_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.DatePicker dtpic = (System.Windows.Controls.DatePicker)((System.Windows.FrameworkElement)e.Source);
            
            if (dtpic != null) {
                var tb = (Popup)dtpic.Template.FindName("PART_Popup", dtpic);
                tb.Placement=PlacementMode.Bottom;
            }
            //var tb = (DatePickerTextBox)Txtvalidfrom.Template.FindName("PART_TextBox", Txtvalidfrom);
            //if (tb != null)
            //{
            //    tb.PreviewMouseUp += (s, args) =>
            //    {
            //        tb.CaretIndex = 0;
            //    };
            //}

        }

        private void dgSupplierServicesRatesUC_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgSupplierServicesRatesUC.BeginEdit();
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


        /*  public void InvokeOnKeyDown(object sender,RoutedEventArgs e)
          {
              btnSelectforbkg_Click(sender,e);
          }
        */

    }
}
