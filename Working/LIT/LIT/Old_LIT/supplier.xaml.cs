using Azure;
using Azure.Core;
using LITModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
//using Microsoft.Windows.Controls;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static LIT.Old_LIT.TreeViewCreation;
using static System.Net.Mime.MediaTypeNames;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for Supplier.xaml
    /// </summary>
    public partial class Supplier : UserControl
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

        List<CommunicationTypeStatus> ListofSupplierCommunContentType { get; set; }
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
        List<SupplierCommunicationNotes> ListofSuppcommunicationnotes = new List<SupplierCommunicationNotes>();
        List<SupplierCommunicationContentdata> ListofSuppcommunicationContent = new List<SupplierCommunicationContentdata>();

        // public bool Chbhidenonactive_Checked { get; set; }

        Errorlog errobj = new Errorlog();
        public bool Datasaveresultsupplier = false;

        private ObservableCollection<SupplierPriceRateEdit> _PriceEditDt;
        public ObservableCollection<SupplierPriceRateEdit> PriceEditDt
        {
            get { return _PriceEditDt ?? (_PriceEditDt = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEditDt = value;
            }
        }
        string rootfilepathItinval = string.Empty;
        public Supplier()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Supplier(string username, MainWindow ParentWindow, string Supplierid = "", string rootfilepathsup = "")
        {
            InitializeComponent();
            this.DataContext = this;
            loginusername = username.Trim();
            if (ParentWindow != null)
            {
                _parentWindow = ParentWindow;
            }
            rootfilepathItinval = (!string.IsNullOrEmpty(rootfilepathsup)) ? rootfilepathsup : "";
            ChbSupplieractive.IsChecked = true;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            Supplierloadcmbvalues();
            // Chbhidenonactive_Checked = true;
            //TxtSupplierName.Focus();



            if (!string.IsNullOrEmpty(Supplierid))
            {
                recordmode = "Edit";
                hdnSuppliermode.Text = "Edit";
                hdnSupplierid.Text = Supplierid.ToString();
                TxtSupplierID.Text = Supplierid;
                ReteriveSupplierValues(Supplierid);
            }
            else
            {
                recordmode = "Save";
                hdnSuppliermode.Text = "Save";
                TxtSupplierID.Text = (Guid.NewGuid()).ToString();
                hdnSupplierid.Text = TxtSupplierID.Text;
                string autoid = loadDropDownListValues.LoadSupplierAutoNumber();//objitdal.LoadSingleValues();
                if (!string.IsNullOrEmpty(autoid))
                {
                    hdnSupplierAutoId.Text = autoid;
                    TxtSupplierID.Text = autoid;
                }
            }
            TxtblsuppNamewithpath.Text = rootfilepathItinval.Trim() + TxtSupplierName.Text.Trim();
        }


        #region "Supplier Core Information Start"
        public void BtnSupplierSave_Click(object sender, RoutedEventArgs e)
        {
            string valmsg = Suppliervalidation();
            if (valmsg == string.Empty)
            {
                saveupdateSupplier();
                saveupdateSupplierServices();
                saveupdateSupplierServicesRates();
                saveupdateWarningSupplier();

                saveupdateWarningService();
                saveupdatePricingOption();
                if (PriceEditDt.Count > 0)
                {
                    saveupdatePriceEditRate();
                }

                saveupdateSupplierCommunicationNote();
                saveupdateSupplierCommunicationContentValues();
                if (!string.IsNullOrEmpty(hdnSupplierid.Text))
                {
                    ReteriveSupplierValues(hdnSupplierid.Text);
                }
                string rootpathval = string.Empty;
                if (rootfilepathItinval.Trim().ToLower() == "supplier\\")
                { rootpathval = rootfilepathItinval; }
                else if (rootfilepathItinval.Trim().ToLower() == "supplier\\new supplier")
                { rootpathval = rootfilepathItinval.Replace("New Supplier", ""); }
                else
                {
                    rootpathval = rootfilepathItinval;//.Remove(rootfilepathItinval.LastIndexOf("\\"));
                }
                TxtblsuppNamewithpath.Text = rootpathval + TxtSupplierName.Text.Trim();
                TreeviewAccordion tracc = new TreeviewAccordion();
                SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                if (snvm != null)
                {

                    snvm.IsSelectedsupplier = true;
                    snvm.IsExpandedsupplier = true;
                    if (snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).ToList().Count > 0)
                    {
                        snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).FirstOrDefault().IsSelectedsupplier = true;
                        snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).FirstOrDefault().IsExpandedsupplier = true;
                    }

                }

                // (new System.Collections.Generic.CollectionDebugView<LIT.SupplierNodeViewModel>(tracc.SupplierTreeViewModel.SupplierItems).Items[0]).IsSelectedsupplier = true;
                _parentWindow.trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;
                //_parentWindow.trviewsupplier.ItemContainerStyle.



            }
            else
            {
                MessageBox.Show(valmsg);
            }
        }

        private void BtnSupplierSave_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnSupplierSave.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void BtnSupplierSave_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnSupplierSave.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF579F00"));
        }

        private void BtnSupplierSaveandclose_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnSupplierSaveandclose.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF579F00"));
        }

        private void BtnSupplierSaveandclose_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnSupplierSaveandclose.Foreground = new SolidColorBrush(Colors.Black);
        }

        public void BtnSupplierSaveandclose_Click(object sender, RoutedEventArgs e)
        {

            string valmsg = Suppliervalidation();
            if (valmsg == string.Empty)
            {
                saveupdateSupplier();
                saveupdateSupplierServices();
                saveupdateSupplierServicesRates();
                saveupdateWarningSupplier();
                saveupdateWarningService();
                saveupdatePricingOption();
                if (PriceEditDt.Count > 0)
                {
                    saveupdatePriceEditRate();
                }
                saveupdateSupplierCommunicationNote();
                saveupdateSupplierCommunicationContentValues();
                Datasaveresultsupplier = true;
                TreeviewAccordion tracc = new TreeviewAccordion();

                SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                if (snvm != null)
                {

                    snvm.IsSelectedsupplier = true;
                    snvm.IsExpandedsupplier = true;
                    if (snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).ToList().Count > 0)
                    {
                        snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).FirstOrDefault().IsSelectedsupplier = false;
                        snvm.SupplierChildren.Where(x => x.SupplierId == hdnSupplierid.Text).FirstOrDefault().IsExpandedsupplier = true;
                    }

                }
                _parentWindow.trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;

                //NavigationService.Navigate(null);
            }
            else
            {
                MessageBox.Show(valmsg);
            }



        }

        public string Suppliervalidation()
        {

            string ret = string.Empty;
            #region "Validation for Itinerary tab start"
            if (TxtSupplierName.Text.Length == 0)
            {
                ret = "Please provide the supplier Name";
                // TxtSupplierName.Focus();
                return ret;
            }
            if (TxtEmail.Text.Length == 0)
            {
                ret = "Please provide an Email Address";
                TxtEmail.Focus();
                return ret;
            }

            if (TxtEmail.Text.Length > 0)
            {
                if ((!Regex.IsMatch(TxtEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")))
                {
                    ret = "Please provide a valid Email Address";
                    TxtEmail.Focus();
                    return ret;
                }
            }

            //if (decimal.TryParse(DepositAmount.Text, out decimal depositAmountDecimal))
            //{

            //}
            //else
            //{
            //    ret = "enter valid deposit amount";
            //    DepositAmount.Focus();
            //    return ret;
            //}
            //if (int.TryParse(PaymentTerms.Text, out int TermsDays))
            //{

            //}
            //else
            //{
            //    ret = "enter valid payment terms in days";
            //    PaymentTerms.Focus();
            //    return ret;
            //}
            //if (TxtPhone.Text.Length > 0)
            //{
            //    if ((!Regex.IsMatch(TxtPhone.Text, "^[0-9.\\-]+$")))
            //    {
            //        ret = "Please provide a valid Phone";
            //        TxtPhone.Focus();
            //        return ret;
            //    }
            //}

            //string allowedchar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.@ ";


            // var regex = new Regex(@"[^a-zA-Z-0-9-.]");
            // return !regex.IsMatch(Text);

            //if (Regex.IsMatch(TxtSupplierName.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the supplier name are allowed";
            //    TxtSupplierName.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtCustomCode.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the Custom code are allowed";
            //    TxtCustomCode.Focus();
            //    return ret;
            //}
            
            //else if (Regex.IsMatch(TxtWebsite.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the website are allowed";
            //    TxtWebsite.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtPhone.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the phone are allowed";
            //    TxtPhone.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtFax.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the fax are allowed";
            //    TxtFax.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtHosts.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the hosts are allowed";
            //    TxtHosts.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtFreeph.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the free ph are allowed";
            //    TxtFreeph.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtMobile.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the mobile are allowed";
            //    TxtMobile.Focus();
            //    return ret;
            //}
            //else if (Regex.IsMatch(TxtPostCode.Text.ToString(), @"[\^*|\"":<>[\]{}`\\()';@&$]"))
            //{
            //    ret = "No special characters on the post code are allowed";
            //    TxtPostCode.Focus();
            //    return ret;
            //}
            //else if (!(txt.Text.ToString()))
            //{
            //    ret = "No special characters on the supplier name are allowed";
            //    TxtSupplierName.Focus();
            //    return ret;
            //}
            if (dgSupplierServices.Items.Count > 0)
            {
                if (SupplierSM.Where(m => m.ServiceName.ToString().Trim() == string.Empty).Count() > 0)
                {
                    ret = "Please provide a Service Name";
                }
            }

            #endregion "Validation for supplier tab end"
            return ret;
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


        public void Supplierloadcmbvalues()
        {
            try
            {
                CommonValueCountrycity objCVCC
                   = new CommonValueCountrycity();

                ListofCountry = loadDropDownListValues.LoadCommonValuesCountry("Country", objCVCC);
                if (ListofCountry != null && ListofCountry.Count > 0)
                {
                    CmbCountry.ItemsSource = ListofCountry;
                    CmbCountry.SelectedValuePath = "CountryId";
                    CmbCountry.DisplayMemberPath = "CountryName";
                }

                //ListofCity = loadDropDownListValues.LoadCommonValuesCity("City", objCVCC);
                //if (ListofCity != null && ListofCity.Count > 0)
                //{
                //    CmbCity.ItemsSource = ListofCity;
                //    CmbCity.SelectedValuePath = "CityId";
                //    CmbCity.DisplayMemberPath = "CityName";
                //}

                SupplierFoldersettingurl = loadDropDownListValues.LoadFolderName("SupplierFolder");

                SupplierServiceType objSST
                   = new SupplierServiceType();
                ListofSupplierServiceType = new List<SupplierServiceType>();
                ListofSupplierServiceType = loadDropDownListValues.LoadSupplierServiceTypes();
                if (ListofSupplierServiceType != null && ListofSupplierServiceType.Count > 0)
                {
                    CmbserviceType.ItemsSource = ListofSupplierServiceType;
                    CmbserviceType.SelectedValuePath = "ServiceTypeID";
                    CmbserviceType.DisplayMemberPath = "ServiceTypeName";
                    if (ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault() != null)
                    {
                        CmbserviceType.SelectedValuePath = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault().ServiceTypeID;
                        //  objSST.Defaultselectedtype = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault();

                        // CmbserviceType.SelectedItemBinding = objSST.Defaultselectedtype.ServiceTypeID;
                    }
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

                    if (ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault() != null)
                    {
                        CmbCurrencyDetails.SelectedValuePath = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().CurrencyID;
                    }
                }




                ListofSupplierCommunContentType = new List<CommunicationTypeStatus>();
                ListofSupplierCommunContentType = loadDropDownListValues.LoadCommunicationTypes();
                if (ListofSupplierCommunContentType != null && ListofSupplierCommunContentType.Count > 0)
                {
                    CmbSuppcommunContentType.ItemsSource = ListofSupplierCommunContentType;
                    CmbSuppcommunContentType.SelectedValuePath = "TypeID";
                    CmbSuppcommunContentType.DisplayMemberPath = "TypeName";
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        public void saveupdateSupplier()
        {
            try
            {
                SupplierModels objsuppm = new SupplierModels();
                /*  if (decimal.TryParse(DepositAmount.Text, out decimal depositAmountDecimal))
                  {

                  }
                  else
                  {
                      MessageBox.Show("enter valid deposit amount");
                      return;
                  }
                  if (int.TryParse(PaymentTerms.Text, out int TermsDays))
                  {

                  }
                  else
                  {
                      MessageBox.Show("enter valid payment terms in days");
                      return;
                  }*/
                if (!string.IsNullOrEmpty(DepositAmount.Text))
                {
                    objsuppm.SupplierPaymentDepositAmount = Convert.ToDecimal(DepositAmount.Text);
                }
                else { objsuppm.SupplierPaymentDepositAmount = 0; }
                if (!string.IsNullOrEmpty(PaymentTerms.Text))
                {
                    objsuppm.SupplierPaymentTermsindays = Convert.ToInt32(PaymentTerms.Text);
                }
                else
                {
                    objsuppm.SupplierPaymentTermsindays = 0;
                }
                objsuppm.SupplierId = hdnSupplierid.Text.Trim();
                objsuppm.SupplierAutoId = Convert.ToInt64(hdnSupplierAutoId.Text.Trim());
                objsuppm.SupplierName = TxtSupplierName.Text.Trim();
                objsuppm.Hosts = TxtHosts.Text.Trim();
                objsuppm.IsSupplierActive = (ChbSupplieractive.IsChecked == true) ? true : false;
                objsuppm.CustomCode = TxtCustomCode.Text.Trim();
                objsuppm.Street = TxtStreet.Text.Trim();
                objsuppm.City = (CmbCity.SelectedValue == null) ? Guid.Empty.ToString() : CmbCity.SelectedValue.ToString();
                objsuppm.State = (CmbState.SelectedValue == null) ? Guid.Empty.ToString() : CmbState.SelectedValue.ToString();
                objsuppm.Country = (CmbCountry.SelectedValue == null) ? Guid.Empty.ToString() : CmbCountry.SelectedValue.ToString();
                objsuppm.Region = (CmbRegion.SelectedValue == null) ? Guid.Empty.ToString() : CmbRegion.SelectedValue.ToString();
                objsuppm.Postcode = TxtPostCode.Text.Trim();
                objsuppm.Phone = TxtPhone.Text.Trim();
                objsuppm.Mobile = TxtMobile.Text.Trim();
                objsuppm.FreePh = TxtFreeph.Text.Trim();
                objsuppm.Fax = TxtFax.Text.Trim();
                objsuppm.Email = TxtEmail.Text.Trim();
                objsuppm.Website = TxtWebsite.Text.Trim();
                objsuppm.PostalAddress = TxtPostalAddress.Text.Trim();
                objsuppm.SupplierComments = TxtComments.Text.Trim();
                objsuppm.SupplierDescription = TxtDescription.Text.Trim();
                objsuppm.SupplierFolderPath = SupplierFoldersettingurl;
                string rootpath = string.Empty;
                if (!string.IsNullOrEmpty(rootfilepathItinval.Trim()))
                {
                    if (rootfilepathItinval.Trim().ToLower() == "supplier\\")
                    { rootpath = rootfilepathItinval; }
                    else if (rootfilepathItinval.Trim().ToLower() == "supplier\\new supplier")
                    { rootpath = rootfilepathItinval.Replace("New Supplier", ""); }
                    else
                    {
                        rootpath = rootfilepathItinval.Remove(rootfilepathItinval.LastIndexOf("\\"));
                    }
                }
                objsuppm.SupplierFolderPath = rootpath;



                string purpose = string.Empty;
                if (recordmode.ToString().ToLower() == "edit")
                {
                    purpose = "I";
                    objsuppm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    objsuppm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    objsuppm.DeletedBy = Guid.Empty.ToString();
                }
                else if (recordmode.ToString().ToLower() == "save")
                {
                    purpose = "I";
                    objsuppm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    objsuppm.ModifiedBy = Guid.Empty.ToString();
                    objsuppm.DeletedBy = Guid.Empty.ToString();
                }
                else if (recordmode.ToString().ToLower() == "delete")
                {
                    purpose = "D";
                    objsuppm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    objsuppm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    objsuppm.IsDeleted = true;
                    objsuppm.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                }
                string objret = objsupdal.SaveUpdateSupplier(purpose, objsuppm);
                if (!string.IsNullOrEmpty(objret))
                {
                    if (objret.ToString().ToLower() == "1")
                    {
                        MessageBox.Show("Supplier saved successfully");
                    }


                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        public void ReteriveSupplierValues(string SupplierIdval)
        {
            try
            {
                if (SupplierIdval != "")
                {
                    List<SupplierModels> listsupplier = new List<SupplierModels>();

                    listsupplier = objsupdal.SupplierRetrive("FIR", Guid.Parse(SupplierIdval));
                    if (listsupplier != null && listsupplier.Count > 0)
                    {
                        if (listsupplier.Count > 0)
                        {
                            PaymentTerms.Text = listsupplier[0].SupplierPaymentTermsindays.ToString();
                            DepositAmount.Text = listsupplier[0].SupplierPaymentDepositAmount.ToString();
                            hdnSupplierid.Text = listsupplier[0].SupplierId;
                            TxtSupplierID.Text = listsupplier[0].SupplierAutoId.ToString();
                            TxtSupplierName.Text = listsupplier[0].SupplierName;
                            hdnSupplierAutoId.Text = listsupplier[0].SupplierAutoId.ToString();
                            ChbSupplieractive.IsChecked = (listsupplier[0].IsSupplierActive) ? true : false;
                            TxtHosts.Text = listsupplier[0].Hosts;
                            TxtCustomCode.Text = listsupplier[0].CustomCode;
                            TxtStreet.Text = listsupplier[0].Street;
                            string Countryid = (listsupplier[0].Country == "" || listsupplier[0].Country == "00000000-0000-0000-0000-000000000000") ? "" : listsupplier[0].Country;
                            if (!string.IsNullOrEmpty(Countryid))
                            {
                                CmbCountry.SelectedValue = (!string.IsNullOrEmpty(Countryid)) ? Countryid : null;
                                cmbstateload(Guid.Parse(Countryid));

                                string Stateid = (listsupplier[0].State == "" || listsupplier[0].State == "00000000-0000-0000-0000-000000000000") ? "" : listsupplier[0].State;
                                if (!string.IsNullOrEmpty(Stateid))
                                {
                                    CmbState.SelectedValue = Stateid;
                                    cmbRegionload(Guid.Parse(Stateid));
                                }
                                else
                                {
                                    CmbState.SelectedValue = null;
                                }
                                string Regionid = (listsupplier[0].Region == "" || listsupplier[0].Region == "00000000-0000-0000-0000-000000000000") ? "" : listsupplier[0].Region;
                                if (!string.IsNullOrEmpty(Regionid))
                                {
                                    CmbRegion.SelectedValue = Regionid;
                                    cmbCityload(Guid.Parse(Regionid));
                                }
                                else
                                {
                                    CmbRegion.SelectedValue = null;
                                }
                                CmbCity.SelectedValue = (listsupplier[0].City == "" || listsupplier[0].City == "00000000-0000-0000-0000-000000000000") ? null : listsupplier[0].City;
                            }
                            else
                            {
                                CmbCountry.SelectedValue = null;
                                CmbState.SelectedValue = null;
                                CmbRegion.SelectedValue = null;
                                CmbCity.SelectedValue = null;
                            }
                            TxtPostCode.Text = listsupplier[0].Postcode;
                            TxtPhone.Text = listsupplier[0].Phone;
                            TxtFreeph.Text = listsupplier[0].FreePh;
                            TxtMobile.Text = listsupplier[0].Mobile;
                            TxtEmail.Text = listsupplier[0].Email;
                            TxtFax.Text = listsupplier[0].Fax;
                            TxtWebsite.Text = listsupplier[0].Website;
                            TxtPostalAddress.Text = listsupplier[0].PostalAddress;
                            TxtComments.Text = listsupplier[0].SupplierComments;
                            TxtDescription.Text = listsupplier[0].SupplierDescription;
                            TxtSupplierFolderPath.Text = listsupplier[0].SupplierFolderPath;

                            //objitm.CreatedBy = dsItineraryRetr.Tables[0].Rows[0]["CreatedBy"].ToString();
                            //objitm.IsDeleted = dsItineraryRetr.Tables[0].Rows[0]["IsDeleted"].ToString();


                        }

                    }

                    ReteriveSupplierServices(SupplierIdval);
                    ReteriveSupplierCommunicationnotes(SupplierIdval);
                    ReteriveSupplierservicemenu(SupplierIdval);
                    ReteriveSupplierCommunicationContent();

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void cmbstateload(Guid countryid)
        {
            try
            {
                CommonValueCountrycity objCVCC
                = new CommonValueCountrycity();

                objCVCC.CountryId = countryid;
                ListofState = loadDropDownListValues.LoadCommonValuesState("State", objCVCC);
                if (ListofState != null && ListofState.Count > 0)
                {
                    CmbState.ItemsSource = ListofState;
                    CmbState.SelectedValuePath = "StatesId";
                    CmbState.DisplayMemberPath = "StatesName";
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void cmbRegionload(Guid Stateid)
        {
            try
            {
                CommonValueCountrycity objCVCC
                 = new CommonValueCountrycity();

                objCVCC.StatesId = Stateid;
                ListofRegion = loadDropDownListValues.LoadCommonValuesRegion("Region", objCVCC);
                if (ListofRegion != null && ListofRegion.Count > 0)
                {
                    CmbRegion.ItemsSource = ListofRegion;
                    CmbRegion.SelectedValuePath = "RegionId";
                    CmbRegion.DisplayMemberPath = "RegionName";
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void cmbCityload(Guid Regionid)
        {
            try
            {
                CommonValueCountrycity objCVCC
                 = new CommonValueCountrycity();

                objCVCC.RegionId = Regionid;
                ListofCity = loadDropDownListValues.LoadCommonValuesCity("CitywithRegionid", objCVCC);
                if (ListofCity != null && ListofCity.Count > 0)
                {
                    CmbCity.ItemsSource = ListofCity;
                    CmbCity.SelectedValuePath = "CityId";
                    CmbCity.DisplayMemberPath = "CityName";
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void CmbCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbCountry.SelectedValue != null)
            {
                CmbState.ItemsSource = null;
                CmbRegion.ItemsSource = null;
                CmbCity.ItemsSource = null;
                cmbstateload((CmbCountry.SelectedValue != null) ? Guid.Parse(CmbCountry.SelectedValue.ToString()) : Guid.Empty);
            }
        }

        private void CmbState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbState.SelectedValue != null)
            {
                CmbRegion.ItemsSource = null;
                CmbCity.ItemsSource = null;
                cmbRegionload((CmbState.SelectedValue != null) ? Guid.Parse(CmbState.SelectedValue.ToString()) : Guid.Empty);
            }
        }

        private void CmbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbRegion.SelectedValue != null)
            {
                CmbCity.ItemsSource = null;
                cmbCityload((CmbRegion.SelectedValue != null) ? Guid.Parse(CmbRegion.SelectedValue.ToString()) : Guid.Empty);
            }
        }
        #endregion "Supplier Core Information End"

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
            ssm.ServiceName = "";// "New Service" + " (" + (SupplierSM.Count + 1) + ")";

            ssm.ServiceId = (Guid.NewGuid()).ToString();
            ssm.SelectedItem = ListofSupplierServiceType.Where(x => x.IsDefault == true).FirstOrDefault();
            ssm.SelectedItemCurrency = ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault();
            ssm.IsActive = true;
            SupplierSM.Add(ssm);
            // lstSupplierSM.Add(SupplierSM);
            dgSupplierServices.ItemsSource = SupplierSM;

            dgSupplierServices.Focus();
            dgSupplierServices.BeginEdit();

            OldDataGridUtility.FocusLastEditableCellAndEdit(dgSupplierServices);
            //int rowIndex = SupplierSM.Count - 1;
            //dgSupplierServices.ScrollIntoView(dgSupplierServices.Items[rowIndex]);
            //DataGridRow row = (DataGridRow)dgSupplierServices.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            //if (row != null)
            //{
            //    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            //    row.IsSelected = true;
            //}

            // dgSupplierServices.CurrentCell = new System.Windows.Controls.DataGridCellInfo(
            // dgSupplierServices.Items[SupplierSM.Count-1], dgSupplierServices.Columns[3]);
        }

        public void saveupdateSupplierServices()
        {
            try
            {
                if (dgSupplierServices.Items.Count > 0)
                {
                    if (SupplierSM.Where(m => m.ServiceName.ToString().Trim() == string.Empty).Count() > 0)
                    {
                        MessageBox.Show("Please provide a Service Name");
                        return;
                    }

                    foreach (SupplierServiceModels ssm in dgSupplierServices.Items)
                    {
                        ComboBox cmb = new ComboBox();
                        cmb.SelectedItem = dgSupplierServices.SelectedItem;
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }



        public void ReteriveSupplierServices(string SupplierIdval)
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
                            dgSupplierServices.ItemsSource = SupplierSM;
                        }
                        else
                        {
                            dgSupplierServices.ItemsSource = SupplierSM.Where(x => x.IsActive == true).ToList();
                        }

                    }
                    if ((ListofSuppservice == null || ListofSuppservice.Count == 0) && (SupplierSM.Count == 0))
                    {
                        dgSupplierServices.ItemsSource = SupplierSM;
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void DeleteSupplierServices(SupplierServiceModels ssm)
        {
            try
            {
                if (dgSupplierServices.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServices.SelectedItem;
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
                        // dgSupplierServices.ItemsSource = SupplierSM;
                        ReteriveSupplierServices(ssm.SupplierId);


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void BtnServiceAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItem();
        }



        private void btnServiceDelete_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;

                DeleteSupplierServices(ssmobj);

            }
        }

        //private void chbhidenonactive_Checked(object sender, RoutedEventArgs e)
        //{
        //    //if (Chbhidenonactive_Checked == true)
        //    //{
        //        if (dgSupplierServices != null)
        //        {
        //          //  Chbhidenonactive_Checked = false;
        //            ReteriveSupplierServices(hdnSupplierid.Text.Trim());
        //            dgSupplierServices.Columns[2].Visibility = Visibility.Hidden;
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
        //        if (dgSupplierServices != null)
        //        {
        //            //Chbhidenonactive_Checked = true;
        //            ReteriveSupplierServices(hdnSupplierid.Text.Trim());
        //            dgSupplierServices.Columns[2].Visibility = Visibility.Visible;
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

                SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
                if (ssmobj.SupplierId.ToString().Trim() == string.Empty)
                {
                    MessageBox.Show("Please provide a Service Name");
                    return;
                }
                if (ssmobj.ServiceName.ToString().Trim() == string.Empty)
                {
                    MessageBox.Show("Please provide a Service Name");
                    return;
                }
                if (ssmobj.IsActive == false)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("Selected Service is Non-Active, Do you really want to proceed?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {

                        SupplierRates.IsEnabled = true;
                        SupplierRates.IsSelected = true;
                        lblserviceName.Visibility = Visibility.Visible;
                        lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                        lblRatesDate.Content = string.Empty;
                        // ListofSuppserviceRates = null;
                        // SupplierSRatesDt = null;
                        chbhideexpiredseasons.IsChecked = true;
                        chbhideexpiredseasonswarning.IsChecked = true;
                        // ListofSuppwarning = null;
                        // ListofServicewarning = null;
                        // WarningDt = null;
                        ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        ReteriveWarningService(ssmobj.ServiceId);
                        ReteriveWarningSupplier(ssmobj.ServiceId);
                        ReterivePricingOption();
                        ReterivePriceEditRate();
                    }
                }
                else
                {

                    SupplierRates.IsEnabled = true;
                    SupplierRates.IsSelected = true;
                    lblserviceName.Visibility = Visibility.Visible;
                    lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();
                    lblRatesDate.Content = string.Empty;
                    // ListofSuppserviceRates = null;
                    // SupplierSRatesDt = null;
                    chbhideexpiredseasons.IsChecked = true;
                    chbhideexpiredseasonswarning.IsChecked = true;
                    // ListofSuppwarning = null;
                    // ListofServicewarning = null;
                    // WarningDt = null;
                    ReteriveSupplierServicesRates(ssmobj.ServiceId);
                    ReteriveWarningService(ssmobj.ServiceId);
                    ReteriveWarningSupplier(ssmobj.ServiceId);
                    ReterivePricingOption();
                    ReterivePriceEditRate();
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void btnServiceRates_Click(object sender, RoutedEventArgs e)
        {
            ServiceRatesPageCall();
        }

        private void dgSupplierServicesDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if ((((System.Windows.Controls.DataGridCell)sender).Column.Header.ToString().ToLower() != "currency") && (((System.Windows.Controls.DataGridCell)sender).Column.Header.ToString().ToLower() != "charges"))
            //{
                SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
                ServiceRatesPageCall();
            //}
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
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            ssRates.SupplierServiceId = ssmobj.ServiceId.ToString();
            ssRates.ValidFrom = DateTime.Now.Date;
            ssRates.ValidTo = DateTime.Now.Date;
            ssRates.IsActive = true;
            //ssRates.StrValidFrom = DateTime.Today.ToShortDateString();
            //ssRates.StrValidTo = DateTime.Today.ToShortDateString();
            ssRates.SupplierServiceDetailsRateId = (Guid.NewGuid()).ToString();
            //DatePicker txtvalidfromdt = (DatePicker)dgSupplierServicesRates.Items[2];
            //txtvalidfromdt.SelectedDate = DateTime.Now;

            if (SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
            {
                // SupplierSRatesDt = null;
            }

            SupplierSRatesDt.Add(ssRates);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
            RatecheckExpireactive(ssmobj.ServiceId);
        }


        private void BtnRatesAdd_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
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
                // if (dgSupplierServicesRates.Items.Count > 0)
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
                        cmb.SelectedItem = dgSupplierServices.SelectedItem;
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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

                    if (SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).Count() > 0)
                    {
                        RatecheckExpireactive(SupplierServiceIdval);
                    }
                    else
                    {
                        RatecheckExpireactive(SupplierServiceIdval);
                    }
                    if ((ListofSuppserviceRates == null || ListofSuppserviceRates.Count == 0) && (SupplierSRatesDt.Count==0))
                    {
                        dgSupplierServicesRates.ItemsSource = SupplierSRatesDt;
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void DeleteSupplierServicesRates(SupplierServiceRatesDt ssm)
        {
            try
            {
                if (dgSupplierServicesRates.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesRates.SelectedItem;
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
                        //dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveSupplierServicesRates(ssm.SupplierServiceId);

                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void btnRatesDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
            //    dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false && x.IsActive == false).ToList();
            //}
            //else
            if (chbhideexpiredseasons.IsChecked == true)
            {
                dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsExpired == false).ToList();
            }
            //else if (chbhidenonactive.IsChecked == true)
            //{
            //    dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.IsActive == false).ToList();
            //}
            else
            {
                dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == SupplierServiceIdval).ToList();
            }
        }


        private void chbhideexpiredseasons_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            chbhideexpiredseasonswarning.IsChecked = chbhideexpiredseasons.IsChecked;
            chbhideexpiredseasonsPrice.IsChecked = chbhideexpiredseasons.IsChecked;
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
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            sswarning.SupplierServiceId = ssmobj.ServiceId.ToString();
            sswarning.ValidFromwarning = DateTime.Now.Date;
            sswarning.ValidTowarning = DateTime.Now.Date;
            sswarning.Messagefor = "Service";
            sswarning.SupplierServiceDetailsWarningID = (Guid.NewGuid()).ToString();
            sswarning.WarDescription = string.Empty;
            sswarning.IsActive = true;

            if (WarningDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
            {
                // WarningDt = null;
            }

            WarningDt.Add(sswarning);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
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
                        cmb.SelectedItem = dgSupplierServices.SelectedItem;
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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
                    else 
                    {
                        if (WarningDt.Count() > 0)
                        {
                            WarningcheckExpireactive(SupplierServiceIdval);
                        }
                    }
                    if ((ListofServicewarning == null || ListofServicewarning.Count == 0) && (WarningDt.Count==0))
                    {
                        dgServicesWarnings.ItemsSource = WarningDt;
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void DeletewarningService(SupplierServiceWarning ssm)
        {
            try
            {
                if (dgServicesWarnings.Items.Count > 0)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.SelectedItem = dgSupplierServicesRates.SelectedItem;
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
                        //dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveWarningService(ssm.SupplierServiceId);
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            sswarning.SupplierServiceId = ssmobj.ServiceId.ToString();
            sswarning.ValidFromwarning = DateTime.Now.Date;
            sswarning.ValidTowarning = DateTime.Now.Date;
            sswarning.Messagefor = "Supplier";
            sswarning.WarDescription = string.Empty;
            sswarning.SupplierServiceDetailsWarningID = (Guid.NewGuid()).ToString();
            sswarning.IsActive = true;

            if (WarningDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId).Count() == 0)
            {
                // WarningDt = null;
            }

            WarningDt.Add(sswarning);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
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
                        cmb.SelectedItem = dgSupplierServices.SelectedItem;
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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
                    else
                    {
                        if (WarningDt.Count() > 0)
                        {
                            WarningcheckExpireactive(SupplierServiceIdval);
                        }
                    }
                    if ((ListofSuppwarning == null || ListofSuppwarning.Count == 0) && (WarningDt.Count==0))
                    {
                        dgSuppWarnings.ItemsSource = WarningDt;
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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
                        //dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;
                        ReteriveWarningSupplier(ssm.SupplierServiceId);
                        ReterivePricingOption();
                        ReterivePriceEditRate();
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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
        //    SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
        //    ReteriveSupplierServicesRates(ssmobj.ServiceId);
        //    ReteriveWarningSupplier(ssmobj.ServiceId);
        //    ReteriveWarningService(ssmobj.ServiceId);
        //}

        private void chbhideexpiredseasonswarning_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            chbhideexpiredseasons.IsChecked = chbhideexpiredseasonswarning.IsChecked;
            chbhideexpiredseasonsPrice.IsChecked = chbhideexpiredseasonswarning.IsChecked;
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
                SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
                SupplierServiceRatesDt ssmobjrate = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
                SupplierPricingOptions.IsEnabled = true;
                SupplierPricingOptions.IsSelected = true;
                lblserviceName.Visibility = Visibility.Visible;
                lblserviceName.Content = "Service Name: " + ssmobj.ServiceName.ToString();

                lblRatesDate.Visibility = Visibility.Visible;

                lblRatesDate.Content = "Date: From: " + ssmobjrate.ValidFrom.ToShortDateString() + "  To: " + ssmobjrate.ValidTo.ToShortDateString();
                ReterivePricingOption();

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        private void btnPricing_Click(object sender, RoutedEventArgs e)
        {
            PricingClickPagecall();
        }
        private void dgSupplierServicesRatesDataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)
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
            //SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
            ssPO.SupplierServiceId = ssRateobj.SupplierServiceId.ToString();
            ssPO.SupplierServiceDetailsRateId = ssRateobj.SupplierServiceDetailsRateId.ToString();
            ssPO.PricingOptionName = ""; //"New Option (" + (PricingOptionDt.Count + 1) + ")";
            ssPO.PriceType = string.Empty;
            ssPO.PriceIsDefault = false;
            ssPO.NetPrice = 0;
            ssPO.MarkupPercentage = 0;
            ssPO.GrossPrice = 0;
            ssPO.CommissionPercentage = 0;
            ssPO.PricingOptionId = (Guid.NewGuid()).ToString();
            ssPO.PriceIsActive = true;

            if (PricingOptionDt.Where(m => m.SupplierServiceId == ssRateobj.SupplierServiceId).Count() == 0)
            {
                // PricingOptionDt = null;
            }
            ListofCurrencyServiceidwise = loadDropDownListValues.CurrencyinfoReterive(ssRateobj.SupplierServiceId.ToString());
            if (ListofCurrencyServiceidwise.Count > 0)
            {
                ssPO.CurrencyName = ListofCurrencyServiceidwise[0].CurrencyName;
                ssPO.CurrencyDisplayFormat = ListofCurrencyServiceidwise[0].DisplayFormat;
            }
            else
            {
                if (ListofCurrencyServiceidwise.Count == 0)
                {
                    var cur =
                         ((SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault() != null) ?
                         (SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault().SelectedItemCurrency : null;

                    if (cur != null)
                    {
                        ssPO.CurrencyName = (((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName : string.Empty;
                        ssPO.CurrencyDisplayFormat = (((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat : string.Empty;
                    }
                }
            }
            PricingOptionDt.Add(ssPO);
            // lstSupplierSM.Add(SupplierSM);
            // dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(m => m.SupplierServiceId == ssmobj.ServiceId);
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }


        public void ReterivePricingOption()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
                                PricingOptionDt.Add(sup);
                            }
                        }
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }

                    if (PricingOptionDt.Where(x => x.SupplierServiceDetailsRateId == ssRateobj.SupplierServiceDetailsRateId && x.SupplierServiceId == ssRateobj.SupplierServiceId).Count() > 0)
                    {
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    else
                    {
                        if (PricingOptionDt.Count > 0)
                            PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);

                    }
                    if ((ListofPricingOption == null || ListofPricingOption.Count == 0) && (PricingOptionDt.Count == 0))    
                    {
                        dgPricingoption.ItemsSource = PricingOptionDt;
                    }
                }
                else
                {
                    dgPricingoption.ItemsSource = null;
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }


        public void ReterivePriceEditRate()
        {
            try
            {
                SupplierServiceRatesDt ssRateobj = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
                    else
                    {
                        if (PricingOptionDt.Count()>0)
                        PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    if ((ListofPricingOption == null || ListofPricingOption.Count == 0) && (PricingOptionDt.Count==0))
                    {
                        dgPricingoption.ItemsSource = PricingOptionDt;
                    }
                }
                else
                {
                    dgPricingoption.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }
        public void DeletePricingOption(SupplierPricingOption ssm)
        {
            try
            {
                if (PricingOptionDt.Count > 0)
                {
                    SupplierServiceRatesDt ssRateobj = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
                        ReterivePricingOption();
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void BtnPriceAdd_Click(object sender, RoutedEventArgs e)
        {

            // SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            SupplierServiceRatesDt ssRateobj = dgSupplierServicesRates.SelectedItem as SupplierServiceRatesDt;
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
                if (chbhideexpiredseasonsPrice.IsChecked == true && chbhidenonactivePrice.IsChecked == true)
                {
                    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList());
                }
                else if (chbhidenonactivePrice.IsChecked == true)
                {
                    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && (x.PriceIsActive == true) && x.SupplierServiceDetailsRateId == supprateid).ToList();
                }
                else if (chbhideexpiredseasonsPrice.IsChecked == true)
                {
                    dgPricingoption.ItemsSource = PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && supprateexpired == false && x.SupplierServiceDetailsRateId == supprateid).ToList();
                }
                else
                {
                    dgPricingoption.ItemsSource = (PricingOptionDt.Where(x => x.SupplierServiceId == SupplierServiceIdval && x.SupplierServiceDetailsRateId == supprateid && (x.PriceIsActive == false || x.PriceIsActive == true)).ToList());
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        private void btnPriceEdit_Click(object sender, RoutedEventArgs e)
        {
            SupplierPricingOption objPO = dgPricingoption.SelectedItem as SupplierPricingOption;
            if (objPO != null)
            {
                if (!string.IsNullOrEmpty(objPO.PricingOptionName))
                {
                    SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, this);
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
                    SuppPricingOptionTemplate wobj = new SuppPricingOptionTemplate(objPO, loginusername, this);
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        #endregion "Supplier Pricing End"

        private void dgServicesWarnings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void msPenLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#579F00"));
        }

        private void msPenEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#79D10D"));
        }

        private void chbhidenonactivePrice_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (dgSupplierServices != null)
                {
                    chbhidenonactive.IsChecked = chbhidenonactivePrice.IsChecked;
                    ReteriveSupplierServices(hdnSupplierid.Text.Trim());


                    if (chbhidenonactivePrice.IsChecked == true)
                    {
                        dgSupplierServices.Columns[2].Visibility = Visibility.Hidden;
                        if (dgPricingoption != null)
                        {
                            ReterivePricingOption();
                            dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
                        }
                    }
                    if (chbhidenonactivePrice.IsChecked == false)
                    {
                        dgSupplierServices.Columns[2].Visibility = Visibility.Visible;
                        if (dgPricingoption != null)
                        {
                            ReterivePricingOption();
                            dgPricingoption.Columns[3].Visibility = Visibility.Visible;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void chbhideexpiredseasonsPrice_Click(object sender, RoutedEventArgs e)
        {
            SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
            chbhideexpiredseasons.IsChecked = chbhideexpiredseasonswarning.IsChecked;
            chbhideexpiredseasonsPrice.IsChecked = chbhideexpiredseasonswarning.IsChecked;
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
                if (dgSupplierServices != null)
                {
                    SupplierServiceModels ssmobj = dgSupplierServices.SelectedItem as SupplierServiceModels;
                    chbhidenonactivePrice.IsChecked = chbhidenonactive.IsChecked;


                    if (chbhidenonactive.IsChecked == false)
                    {
                        ReteriveSupplierServices(hdnSupplierid.Text.Trim());
                        if (ssmobj != null)
                        {
                            ReteriveWarningSupplier(ssmobj.ServiceId);
                            ReteriveWarningService(ssmobj.ServiceId);
                            ReteriveSupplierServicesRates(ssmobj.ServiceId);
                        }
                        dgSupplierServices.Columns[2].Visibility = Visibility.Visible;
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
                        dgSupplierServicesRates.ItemsSource = null;
                        dgSuppWarnings.ItemsSource = null;
                        dgPricingoption.Columns[3].Visibility = Visibility.Hidden;
                        dgSupplierServices.Columns[2].Visibility = Visibility.Hidden;
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
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }



        private void TxtSupplierName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           // e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        }

        private void TxtHosts_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationClass.IsAlphaNumericDot(e.Text);
        }

        private void TxtCustomCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        }

        private void TxtPostCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        }







        //private void TxtPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        //}

        //private void TxtFreeph_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        //}

        //private void TxtMobile_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !ValidationClass.IsNumeric(e.Text);
        //}

        //private void TxtFax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !ValidationClass.IsAlphaNumeric(e.Text);
        //}

        //private void TxtWebsite_PreviewTextInput(object sender, TextCompositionEventArgs e)
        //{
        //    e.Handled = !ValidationClass.IsAlphaNumericDot(e.Text);
        //}



        /* Supplier Communication Start here */

        private ObservableCollection<SupplierCommunicationNotes> _SupplierCommNote;
        public ObservableCollection<SupplierCommunicationNotes> SupplierCommNote
        {
            get { return _SupplierCommNote ?? (_SupplierCommNote = new ObservableCollection<SupplierCommunicationNotes>()); }
            set
            {
                _SupplierCommNote = value;
            }
        }

        private ObservableCollection<supplierservicemenulist> _SupplierserviceMenu;
        public ObservableCollection<supplierservicemenulist> SupplierserviceMenu
        {
            get { return _SupplierserviceMenu ?? (_SupplierserviceMenu = new ObservableCollection<supplierservicemenulist>()); }
            set
            {
                _SupplierserviceMenu = value;
            }
        }

        private ObservableCollection<SupplierCommunicationContentdata> _SuppCommContentDatainfo;
        public ObservableCollection<SupplierCommunicationContentdata> SuppCommContentDatainfo
        {
            get { return _SuppCommContentDatainfo ?? (_SuppCommContentDatainfo = new ObservableCollection<SupplierCommunicationContentdata>()); }
            set
            {
                _SuppCommContentDatainfo = value;
            }
        }
        private void AddItemforCommunicationNote()
        {
            SupplierCommunicationNotes scns;
            scns = new SupplierCommunicationNotes();
            scns.SupplierId = hdnSupplierid.Text;
            scns.Notesinfo = "";//"Enter Note here" + " (" + (SupplierCommNote.Count + 1) + ")";
            scns.NotesinfoID = (Guid.NewGuid()).ToString();
            scns.Autoselected = false;
            SupplierCommNote.Add(scns);
            DgcommunicationNote.ItemsSource = SupplierCommNote;

            DgcommunicationNote.Focus();
            DgcommunicationNote.BeginEdit();
        }

        public void saveupdateSupplierCommunicationNote()
        {
            try
            {
                foreach (SupplierCommunicationNotes scns in DgcommunicationNote.Items)
                {
                    SupplierCommunicationNotes objSCNs = new SupplierCommunicationNotes();
                    objSCNs.NotesinfoID = scns.NotesinfoID;
                    objSCNs.Notesinfo = scns.Notesinfo;
                    objSCNs.SupplierId = scns.SupplierId;
                    objSCNs.Autoselected = scns.Autoselected;

                    string purpose = string.Empty;
                    if (recordmode.ToString().ToLower() == "edit")
                    {
                        purpose = "I";
                        objSCNs.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        objSCNs.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        objSCNs.DeletedBy = Guid.Empty.ToString();
                    }
                    else if (recordmode.ToString().ToLower() == "save")
                    {
                        purpose = "I";
                        objSCNs.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        objSCNs.ModifiedBy = Guid.Empty.ToString();
                        objSCNs.DeletedBy = Guid.Empty.ToString();
                    }
                    else if (recordmode.ToString().ToLower() == "delete")
                    {
                        purpose = "D";
                        objSCNs.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        objSCNs.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        objSCNs.IsDeleted = true;
                        objSCNs.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                    }
                    string objretcn = objsupdal.SaveUpdateSupplierCommunicationNotes(purpose, objSCNs);
                    if (!string.IsNullOrEmpty(objretcn))
                    {
                        if (objretcn.ToString().ToLower() == "1")
                        {
                            //MessageBox.Show("Supplier saved successfully");
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void btnCommunicationnoteDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierCommunicationNotes scnsobj = DgcommunicationNote.SelectedItem as SupplierCommunicationNotes;
                DeleteCommunicationNotes(scnsobj);

            }

        }
        private void DeleteCommunicationNotes(SupplierCommunicationNotes scnsobj)
        {
            try
            {
                if (DgcommunicationNote.Items.Count > 0)
                {

                    SupplierCommunicationNotes objscns = new SupplierCommunicationNotes();
                    objscns.SupplierId = scnsobj.SupplierId;
                    objscns.NotesinfoID = scnsobj.NotesinfoID;
                    objscns.IsDeleted = true;
                    objscns.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierCommunicationNotes(objscns);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                        {
                            MessageBox.Show("Supplier Communication Note Deleted successfully");
                            SupplierCommNote.Remove(SupplierCommNote.Where(m => m.NotesinfoID == objscns.NotesinfoID).FirstOrDefault());
                        }
                        else if (objret.ToString().ToLower() == "-1")
                        {
                            if (SupplierCommNote.Where(m => m.NotesinfoID == objscns.NotesinfoID).FirstOrDefault() != null)
                            {
                                MessageBox.Show("Supplier  Communication Note  Deleted successfully");
                                SupplierCommNote.Remove(SupplierCommNote.Where(m => m.NotesinfoID == objscns.NotesinfoID).FirstOrDefault());
                            }
                        }
                        ReteriveSupplierCommunicationnotes(objscns.SupplierId);


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void ReteriveSupplierCommunicationnotes(string SupplierIdval)
        {
            try
            {
                if (SupplierIdval != "")
                {
                    ListofSuppcommunicationnotes = objsupdal.SupplierCommunicationNoteRetrive(Guid.Parse(SupplierIdval));
                    if (ListofSuppcommunicationnotes != null && ListofSuppcommunicationnotes.Count > 0)
                    {
                        foreach (SupplierCommunicationNotes scn in ListofSuppcommunicationnotes)
                        {
                            if (SupplierCommNote.Where(x => x.NotesinfoID == scn.NotesinfoID).Count() == 0)
                            {
                                SupplierCommNote.Add(scn);
                            }
                        }
                        DgcommunicationNote.ItemsSource = SupplierCommNote;
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void BtnCommunicationNoteAdd_Click(object sender, RoutedEventArgs e)
        {
            AddItemforCommunicationNote();
        }


        public void ReteriveSupplierservicemenu(string SupplierIdval)
        {
            try
            {
                if (SupplierIdval != "")
                {
                    List<supplierservicemenulist> lstssm = new List<supplierservicemenulist>();
                    lstssm = objsupdal.SupplierServiceMenuRetrive(Guid.Parse(SupplierIdval));
                    if (lstssm != null && lstssm.Count > 0)
                    {
                        MenuItem mnuIn = new MenuItem();
                        foreach (supplierservicemenulist scn in lstssm)
                        {
                            if (SupplierserviceMenu.Where(x => x.ID == scn.ID).Count() == 0)
                            {
                                SupplierserviceMenu.Add(scn);
                                mnuIn.Header = scn.ServiceName;
                                mnuIn.Uid = scn.SupplierServiceID;
                                mnuadditms.Items.Add(mnuIn.Header);
                            }
                        }
                        mnuadditms.Click += Mnuadditms_Click;

                        // MnuSuppSer.ItemsSource = SupplierserviceMenu;
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void Mnuadditms_Click(object sender, RoutedEventArgs e)
        {
            string contentfor = ((System.Windows.Controls.HeaderedItemsControl)e.OriginalSource).Header.ToString();
            string contentfordata = string.Empty;
            string ServiceID = string.Empty;
            if (!string.IsNullOrEmpty(contentfor))
            {
                if (SupplierserviceMenu.Where(x => x.ServiceName == contentfor).Count() > 0)
                {
                    ServiceID = SupplierserviceMenu.Where(x => x.ServiceName == contentfor).FirstOrDefault().SupplierServiceID;
                }
                else
                {
                    ServiceID = Guid.Empty.ToString();
                }
                contentfordata = contentfor.Split(":")[1];
            }
            if (SuppCommContentDatainfo.Count > 0)
            {
                SuppCommContentDatainfo.ToList().ForEach(x => x.IsLastadded = false);
            }
            SupplierCommunicationContent win = new SupplierCommunicationContent(contentfordata, hdnSupplierid.Text, ServiceID, this);
            win.ShowDialog();
            // throw new NotImplementedException();
        }

        public void ReteriveSupplierCommunicationContent()
        {
            try
            {
                if (hdnSupplierid.Text != "")
                {
                    if (SuppCommContentDatainfo.Count == 0)
                    {
                        ListofSuppcommunicationContent = objsupdal.SupplierCommunicationContentRetrive(Guid.Parse(hdnSupplierid.Text));
                        if (ListofSuppcommunicationContent != null && ListofSuppcommunicationContent.Count > 0)
                        {
                            foreach (SupplierCommunicationContentdata scn in ListofSuppcommunicationContent)
                            {
                                if (SuppCommContentDatainfo.Where(x => x.ContentID == scn.ContentID).Count() == 0)
                                {
                                    scn.SelectedItemcontentype = ListofSupplierCommunContentType.Where(x => x.TypeID == scn.ContentType).FirstOrDefault();
                                    scn.OnlineImage = System.IO.Path.GetFileName(scn.OnlineImage);
                                    scn.ReportImage = System.IO.Path.GetFileName(scn.ReportImage);
                                    SuppCommContentDatainfo.Add(scn);
                                }
                            }

                        }
                        DgcommunicationContent.ItemsSource = SuppCommContentDatainfo;
                        if (SuppCommContentDatainfo.Where(x => x.IsLastadded == true).ToList().Count() > 0)
                        {
                            SupplierCommunicationContentdata objsccd = SuppCommContentDatainfo.Where(x => x.IsselectedContent == true).ToList()[0];
                            if (objsccd != null)
                            {
                                DgcommunicationContent.SelectedItem = objsccd;
                                txtcontentnamevalue.Focus();
                            }
                        }
                    }
                    else
                    {

                        DgcommunicationContent.ItemsSource = SuppCommContentDatainfo;
                        if (SuppCommContentDatainfo.Where(x => x.IsselectedContent == true).ToList().Count() > 0)
                        {
                            SupplierCommunicationContentdata objsccd = SuppCommContentDatainfo.Where(x => x.IsselectedContent == true).ToList()[0];
                            if (objsccd != null) { selectedrecord(objsccd); }
                        }
                        if (SuppCommContentDatainfo.Where(x => x.IsLastadded == true).ToList().Count() > 0)
                        {
                            SupplierCommunicationContentdata objsccd = SuppCommContentDatainfo.Where(x => x.IsLastadded == true).ToList()[0];
                            if (objsccd != null)
                            {
                                DgcommunicationContent.SelectedItem = objsccd;
                                txtcontentnamevalue.Focus();

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        private void BtnReportimage_Click(object sender, RoutedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png)|*.jpg; *.jpeg; *.gif; *.png";
                bool? res = fileDialog.ShowDialog();
                if (res.HasValue && res.Value)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                    // MessageBox.Show(sr.ReadToEnd());

                    TxtReportImageFileupload.Text = System.IO.Path.GetFileName(fileDialog.FileName);
                    SuppCommContentDatainfo[index].ReportImage = fileDialog.FileName;
                    sr.Close();
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a communication content");
                return;
            }

        }

        private void btnOtherImages_Click(object sender, RoutedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.png)|*.jpg; *.jpeg; *.gif; *.png";
                bool? res = fileDialog.ShowDialog();
                if (res.HasValue && res.Value)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileDialog.FileName);
                    // MessageBox.Show(sr.ReadToEnd());

                    TxtOthernimgFileupload.Text = System.IO.Path.GetFileName(fileDialog.FileName);
                    SuppCommContentDatainfo[index].OnlineImage = fileDialog.FileName;
                    sr.Close();
                }
            }
        }

        private void Txtbodyhtml_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                SuppCommContentDatainfo[index].BodyHtml = Txtbodyhtml.Text.Trim();

            }
        }
        private void txtheadingval_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                SuppCommContentDatainfo[index].Heading = txtheadingval.Text.Trim();

            }
        }

        private static String GetDestinationPath(string filename, string foldername)
        {
            DBConnectionEF dbconEF = new DBConnectionEF();
            string path = string.Empty;
            path=dbconEF.GetImagePDFTHtmlFolderPath();
            if (!string.IsNullOrEmpty(path))
            {
                //String appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
                string appStartPath = path;
                appStartPath = String.Format(appStartPath + "\\{0}\\" + filename, foldername);
                return appStartPath;
            }
            return path;
        }


        private void saveupdateSupplierCommunicationContentValues()
        {
            if (SuppCommContentDatainfo.Count > 0)
            {
                string ftpUrl = string.Empty; string userName = string.Empty; string password = string.Empty;
                CommonAndCalcuation CAC = new CommonAndCalcuation();
                List<FTPServerdetails> ftpserdet = new List<FTPServerdetails>();
                ftpserdet = CAC.ReteriveFTPDetails();
                ftpUrl = ftpserdet[0].FTPUrl;
                userName = ftpserdet[0].FTPUsername;
                password = ftpserdet[0].FTPPassword;

                foreach (SupplierCommunicationContentdata sccd in DgcommunicationContent.Items)
                {

                    try
                    {
                        SupplierCommunicationContentdata objSCdds = new SupplierCommunicationContentdata();
                        objSCdds.ContentID = sccd.ContentID;
                        objSCdds.ContentName = sccd.ContentName;
                        objSCdds.ContentFor = sccd.ContentFor;
                        objSCdds.ContentType = ((SQLDataAccessLayer.Models.CommunicationTypeStatus)sccd.SelectedItemcontentype != null) ? ((SQLDataAccessLayer.Models.CommunicationTypeStatus)sccd.SelectedItemcontentype).TypeID : Guid.Empty.ToString();
                        
                        string OnlineImagepath = string.Empty;
                        string ReportImagepath = string.Empty;
                        if (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault() != null)
                        {
                            objSCdds.SupplierID = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().SupplierID != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().SupplierID : Guid.Empty.ToString();
                            objSCdds.Heading = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().Heading != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().Heading : string.Empty;
                            ReportImagepath = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().ReportImage != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().ReportImage : string.Empty;
                            OnlineImagepath = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().OnlineImage != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().OnlineImage : string.Empty;
                            objSCdds.ServiceID = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().ServiceID != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().ServiceID : Guid.Empty.ToString();
                            objSCdds.BodyHtml = (SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().BodyHtml != null) ? SuppCommContentDatainfo.Where(x => x.ContentID == sccd.ContentID).FirstOrDefault().BodyHtml : string.Empty;
                        }
                        else
                        {
                            objSCdds.SupplierID=Guid.Empty.ToString();
                            objSCdds.Heading = string.Empty;
                            ReportImagepath = string.Empty;
                            OnlineImagepath = string.Empty;
                            objSCdds.ServiceID= Guid.Empty.ToString();
                            objSCdds.BodyHtml = string.Empty;
                        }

                        if (!string.IsNullOrEmpty(ReportImagepath))
                        {
                            string ReportImagename = System.IO.Path.GetFileName(ReportImagepath);
                            string RptdestinationPath = GetDestinationPath(ReportImagename, "ReportImages");

                            if (!File.Exists(RptdestinationPath))
                            {
                                File.Copy(ReportImagepath, RptdestinationPath, true);
                            }
                            //string RptdestinationPath = string.Empty;
                            //string Filename = System.IO.Path.GetFileName(ReportImagepath);
                            //RptdestinationPath = ftpUrl + "\\" + "SupplierCommunication\\ReportImages" + "\\" + Filename;
                            //// fileuploadatFTPserver(ReportImagepath, "SupplierCommunication\\ReportImages");
                            //if (!CheckfileExistatFTPserverNoTask(ReportImagepath, "SupplierCommunication\\ReportImages"))
                            //{
                            //    fileuploadatFTPserverNoTask(ReportImagepath, "SupplierCommunication\\ReportImages");
                            //}
                            objSCdds.ReportImage = RptdestinationPath;


                        }
                        else { objSCdds.ReportImage = string.Empty; }
                        

                        if (!string.IsNullOrEmpty(OnlineImagepath))
                        {

                            string OnlineImagename = System.IO.Path.GetFileName(OnlineImagepath);
                            string OnlinedestinationPath = GetDestinationPath(OnlineImagename, "OnlineImages");
                            if (!File.Exists(OnlinedestinationPath))
                            {
                                File.Copy(OnlineImagepath, OnlinedestinationPath, true);
                            }

                            //string OnlinedestinationPath = string.Empty;

                            //string Filename = System.IO.Path.GetFileName(OnlineImagepath);
                            //OnlinedestinationPath = ftpUrl + "\\" + "SupplierCommunication\\OnlineImages" + "\\" + Filename;
                            //// fileuploadatFTPserver(OnlineImagepath, "SupplierCommunication\\OnlineImages");
                            //if (!CheckfileExistatFTPserverNoTask(Filename, OnlinedestinationPath))
                            //{
                            //    fileuploadatFTPserverNoTask(OnlineImagepath, "SupplierCommunication\\OnlineImages");
                            //}
                            objSCdds.OnlineImage = OnlinedestinationPath;
                        }
                        else { objSCdds.OnlineImage = string.Empty; }
                        string purpose = string.Empty;
                        if (recordmode.ToString().ToLower() == "edit")
                        {
                            purpose = "I";
                            objSCdds.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objSCdds.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objSCdds.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "save")
                        {
                            purpose = "I";
                            objSCdds.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objSCdds.ModifiedBy = Guid.Empty.ToString();
                            objSCdds.DeletedBy = Guid.Empty.ToString();
                        }
                        else if (recordmode.ToString().ToLower() == "delete")
                        {
                            purpose = "D";
                            objSCdds.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objSCdds.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                            objSCdds.IsDeleted = true;
                            objSCdds.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
                        }
                        string objretcn = objsupdal.SaveUpdateSupplierCommunicationContent(purpose, objSCdds);
                        if (!string.IsNullOrEmpty(objretcn))
                        {
                            if (objretcn.ToString().ToLower() == "1")
                            {
                                //MessageBox.Show("Supplier saved successfully");
                            }


                        }
                    }

                    catch (Exception ex)
                    {
                        errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                    }
                }

            }
        }



        private void btnSuppcommcontdelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                SupplierCommunicationContentdata sccsobj = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
                DeleteSupplierCommunicationcontent(sccsobj);

            }
        }

        public void DeleteSupplierCommunicationcontent(SupplierCommunicationContentdata scc)
        {
            try
            {
                if (SuppCommContentDatainfo.Count > 0)
                {
                    SupplierCommunicationContentdata objsuppComcont = new SupplierCommunicationContentdata();
                    objsuppComcont.ContentID = scc.ContentID;
                    objsuppComcont.SupplierID = scc.SupplierID;
                    objsuppComcont.IsDeleted = true;
                    objsuppComcont.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeleteSupplierCommunicationContent(objsuppComcont);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                        {
                            MessageBox.Show("Supplier Communication Deleted successfully");
                            SuppCommContentDatainfo.Remove(SuppCommContentDatainfo.Where(m => m.ContentID == scc.ContentID).FirstOrDefault());
                            // dgSupplierServices.ItemsSource = SupplierSM;
                        }
                        else if (objret.ToString().ToLower() == "-1")
                        {
                            if (SuppCommContentDatainfo.Where(m => m.ContentID == scc.ContentID).FirstOrDefault() != null)
                            {
                                MessageBox.Show("Supplier Communication Deleted successfully");
                                SuppCommContentDatainfo.Remove(SuppCommContentDatainfo.Where(m => m.ContentID == scc.ContentID).FirstOrDefault());
                            }
                        }

                        clearcontrols();
                        ReteriveSupplierCommunicationContent();


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }


        private void clearcontrols()
        {
            txtcontentnamevalue.Text = "";
            txtheadingval.Text = "";
            Txtbodyhtml.Text = "";
            TxtReportImageFileupload.Text = "";
            TxtOthernimgFileupload.Text = "";
        }
        private void selectedrecord(SupplierCommunicationContentdata objsccdval)
        {
            SupplierCommunicationContentdata objsccd = objsccdval;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                txtcontentnamevalue.Text = SuppCommContentDatainfo.ToList()[index].ContentName;
                txtheadingval.Text = SuppCommContentDatainfo.ToList()[index].Heading;
                Txtbodyhtml.Text = SuppCommContentDatainfo.ToList()[index].BodyHtml;
                TxtReportImageFileupload.Text = SuppCommContentDatainfo.ToList()[index].ReportImage;
                TxtOthernimgFileupload.Text = SuppCommContentDatainfo.ToList()[index].OnlineImage;

            }
        }
        private void DgcommunicationContent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null) { selectedrecord(objsccd); }

        }

        private void txtcontentnamevalue_LostFocus(object sender, RoutedEventArgs e)
        {
            SupplierCommunicationContentdata objsccd = DgcommunicationContent.SelectedItem as SupplierCommunicationContentdata;
            if (objsccd != null)
            {
                int index = SuppCommContentDatainfo.ToList().FindIndex(x => x.ContentID == objsccd.ContentID);
                SuppCommContentDatainfo[index].ContentName = txtcontentnamevalue.Text.Trim();

            }
        }

        private void txtblconentname_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (((System.Windows.FrameworkElement)sender) != null)
            {
                string texttooltip = ((System.Windows.Controls.TextBlock)e.OriginalSource).Text;
                ((System.Windows.FrameworkElement)sender).ToolTip = texttooltip;
            }
        }

        private void dgSupplierServices_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgSupplierServices.BeginEdit();
        }

        private void dgSupplierServicesRates_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgSupplierServices.BeginEdit();
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

        private void dgPricingoption_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgPricingoption.BeginEdit();

        }

        /* Supplier Communication End here */


     /*   async Task<FtpStatusCode> fileuploadatFTPserver(string filePath, string location)
        {

            FtpStatusCode ftpstacod = 0;
            string ftpUrl = string.Empty; string userName = string.Empty; string password = string.Empty;
            try
            {
                CommonAndCalcuation CAC = new CommonAndCalcuation();
                List<FTPServerdetails> ftpserdet = new List<FTPServerdetails>();
                ftpserdet = CAC.ReteriveFTPDetails();
                ftpUrl = ftpserdet[0].FTPUrl;
                userName = ftpserdet[0].FTPUsername;
                password = ftpserdet[0].FTPPassword;
                string Filename = System.IO.Path.GetFileName(filePath);
                //FTPfilepath = ftpUrl + "\\" + location + "\\" + Filename;
               // FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);               

                //if (ftpstacod != FtpStatusCode.ClosingData)
                //{
                    FtpWebRequest requestcr = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);
                    requestcr.Method = WebRequestMethods.Ftp.UploadFile;
                    //request.Method = WebRequestMethods.Ftp.AppendFile;

                    requestcr.Credentials = new NetworkCredential(userName, password);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (Stream requestStream = requestcr.GetRequestStream())
                    {
                        await fileStream.CopyToAsync(requestStream);
                    }

                    using (FtpWebResponse response = (FtpWebResponse)await requestcr.GetResponseAsync())
                    {
                        ftpstacod = response.StatusCode;
                    }
               // }
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            catch (Exception ex) { errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ""); }

            return ftpstacod;
        }

        async Task<FtpStatusCode> CheckfileExistatFTPserver(string filePath, string location)
        {

            FtpStatusCode ftpstacod = 0;
            string ftpUrl = string.Empty; string userName = string.Empty; string password = string.Empty;
            try
            {
                CommonAndCalcuation CAC = new CommonAndCalcuation();
                List<FTPServerdetails> ftpserdet = new List<FTPServerdetails>();
                ftpserdet = CAC.ReteriveFTPDetails();
                ftpUrl = ftpserdet[0].FTPUrl;
                userName = ftpserdet[0].FTPUsername;
                password = ftpserdet[0].FTPPassword;
                string Filename = System.IO.Path.GetFileName(filePath);
                //FTPfilepath = ftpUrl + "\\" + location + "\\" + Filename;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);
                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                request.Credentials = new NetworkCredential(userName, password);
                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    ftpstacod = response.StatusCode;
                }               
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription; 
                ftpstacod = ((FtpWebResponse)ex.Response).StatusCode;
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            catch (Exception ex) { errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ""); }

            return ftpstacod;
        }

        */

        public string fileuploadatFTPserverNoTask(string filePath, string location)
        {

            FtpStatusCode ftpstacod = 0;
            string ftpUrl = string.Empty; string userName = string.Empty; string password = string.Empty;
            try
            {
                CommonAndCalcuation CAC = new CommonAndCalcuation();
                List<FTPServerdetails> ftpserdet = new List<FTPServerdetails>();
                ftpserdet = CAC.ReteriveFTPDetails();
                ftpUrl = ftpserdet[0].FTPUrl;
                userName = ftpserdet[0].FTPUsername;
                password = ftpserdet[0].FTPPassword;
                string Filename = System.IO.Path.GetFileName(filePath);
                //FTPfilepath = ftpUrl + "\\" + location + "\\" + Filename;
                // FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);               

                //if (ftpstacod != FtpStatusCode.ClosingData)
                //{
                FtpWebRequest requestcr = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);
                requestcr.Method = WebRequestMethods.Ftp.UploadFile;
                //request.Method = WebRequestMethods.Ftp.AppendFile;

                requestcr.Credentials = new NetworkCredential(userName, password);
                StreamReader sourceStream = new StreamReader(filePath);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                requestcr.ContentLength = fileContents.Length;
                Stream requestStream = requestcr.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)requestcr.GetResponse();
                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);
            }
            catch (WebException ex)
            {
                String status = ((FtpWebResponse)ex.Response).StatusDescription;
                errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            catch (Exception ex) { errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ""); }

            return ftpstacod.ToString();
        }

        public bool CheckfileExistatFTPserverNoTask(string filePath, string location)
        {
              string ftpUrl = string.Empty; string userName = string.Empty; string password = string.Empty;
            
                CommonAndCalcuation CAC = new CommonAndCalcuation();
                List<FTPServerdetails> ftpserdet = new List<FTPServerdetails>();
                ftpserdet = CAC.ReteriveFTPDetails();
                ftpUrl = ftpserdet[0].FTPUrl;
                userName = ftpserdet[0].FTPUsername;
                password = ftpserdet[0].FTPPassword;
                string Filename = System.IO.Path.GetFileName(filePath);
               
                FtpWebRequest requestcheck = (FtpWebRequest)WebRequest.Create(ftpUrl + "\\" + location + "\\" + Filename);
                requestcheck.Credentials = new NetworkCredential(userName, password);
                requestcheck.Method = WebRequestMethods.Ftp.GetFileSize;

                try
                {
                    FtpWebResponse response = (FtpWebResponse)requestcheck.GetResponse();
                    return true;
                }               
                catch (WebException ex)
                {
                    String status = ((FtpWebResponse)ex.Response).StatusDescription; FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                        return false;
                }
                catch (Exception ex) { errobj.WriteErrorLoginfo("Supplier", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ""); }

                return false;
            
           
             
        }

        private void PaymentTerms_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "0" || textBox.Text == "0.00")
            {
                textBox.Text = "";
            }

        }

        private void PaymentTerms_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "")
            {
                //textBox.Text = "0";
                textBox.Text = "";
            }

        }

        private void DepositAmount_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "")
            {
                // textBox.Text = "0";
                textBox.Text = "";
            }

        }

        private void DepositAmount_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "0" || textBox.Text == "0.00")
            {
                textBox.Text = "";
            }

        }
    }
    //public static class FocusFields
    //{
    //    public static void BeginInvoke<T>(this T element, Action<T> action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle) where T : UIElement
    //    {
    //        element.Dispatcher.BeginInvoke(priority, action);
    //    }
    //}


}
