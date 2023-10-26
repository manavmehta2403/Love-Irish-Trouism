using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
using LITModels;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;


namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for SuppPricingOptionTemplate.xaml
    /// </summary>
    public partial class SuppPricingOptionTemplate : Window
    {

        SupplierPricingOption objPO;
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        List<SupplierPriceRateEdit> ListofPricedit = new List<SupplierPriceRateEdit>();
        Errorlog errobj = new Errorlog();
        Supplier Parentwindow;
        SupplierServiceUC ParentwindowUC;
        BookingSupplierSearch parentbkss;
        List<Currencydetails> ListofCurrency = new List<Currencydetails>();
        DBConnectionEF DBconnEF = new DBConnectionEF();
        // SupplierPriceRateEdit objPriEd=new SupplierPriceRateEdit();
        public SuppPricingOptionTemplate()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public SuppPricingOptionTemplate(SupplierPricingOption supo, string loginusername, Supplier suppwindow = null, SupplierServiceUC Suppuc = null, BookingSupplierSearch bkss = null)
        {
            InitializeComponent();
            this.DataContext = this;
            objPO = supo;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            if (suppwindow != null)
            {
                Parentwindow = suppwindow;
                PriceEditDtfull = suppwindow.PriceEditDt;
                var observablecollection = new ObservableCollection<SupplierPriceRateEdit>(suppwindow.PriceEditDt.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList());
                PriceEdit = observablecollection;
                if (PriceEdit.Count == 0)
                {
                    RbNetMark.IsChecked = true;
                }
            }
            if (Suppuc != null)
            {
                ParentwindowUC = Suppuc;
                PriceEditDtfull = Suppuc.PriceEditDt;
                var observablecollection = new ObservableCollection<SupplierPriceRateEdit>(Suppuc.PriceEditDt.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList());
                PriceEdit = observablecollection;
                if (PriceEdit.Count == 0)
                {
                    RbNetMark.IsChecked = true;
                }

            }
            if (bkss != null)
            {
                parentbkss = bkss;
                PriceEditDtfull = bkss.PriceEditDt;
                var observablecollection = new ObservableCollection<SupplierPriceRateEdit>(bkss.PriceEditDt.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList());
                PriceEdit = observablecollection;
                if (PriceEdit.Count == 0)
                {
                    RbNetMark.IsChecked = true;
                }

            }


            ListofCurrency = loadDropDownListValues.CurrencyinfoReterive(objPO.SupplierServiceId);

            ReterivePricingEditRate();
            if (suppwindow == null && Suppuc == null && bkss == null)
            {
                AllControlsReadonly();
                Roundingcheckboxenabledisable("disable");

            }
        }





        private ObservableCollection<SupplierPriceRateEdit> _PriceEditDtfull;
        public ObservableCollection<SupplierPriceRateEdit> PriceEditDtfull
        {
            get { return _PriceEditDtfull ?? (_PriceEditDtfull = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEditDtfull = value;
            }
        }


        private ObservableCollection<SupplierPriceRateEdit> _PriceEdit;
        public ObservableCollection<SupplierPriceRateEdit> PriceEdit
        {
            get { return _PriceEdit ?? (_PriceEdit = new ObservableCollection<SupplierPriceRateEdit>()); }
            set
            {
                _PriceEdit = value;
            }
        }

        private void AddPricingOption()
        {
            SupplierPriceRateEdit ssPO = new SupplierPriceRateEdit();
            ssPO.SupplierServiceId = objPO.SupplierServiceId.ToString();
            ssPO.NetPrice = 0;
            ssPO.MarkupPercentage =(decimal)DBconnEF.GetSupplierMargin();
            ssPO.GrossPrice = 0;
            ssPO.CommissionPercentage = 0;
            ssPO.PriceEditRateId = (Guid.NewGuid()).ToString();
            ssPO.PricingOptionId = objPO.PricingOptionId.ToString();

            if (PriceEditDtfull.Where(m => m.SupplierServiceId == ssPO.SupplierServiceId).Count() == 0)
            {
                //PriceEdit = null;
                //PriceEditDtfull = null;

            }
            if (ListofCurrency.Count > 0)
            {
                ssPO.CurrencyName = ListofCurrency[0].CurrencyName;
                ssPO.CurrencyDisplayFormat = ListofCurrency[0].DisplayFormat;
            }
            else
            {
                if (ListofCurrency.Count == 0)
                {
                    if (Parentwindow != null)
                    {
                        var cur =
                             ((Parentwindow.SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault() != null) ?
                             (Parentwindow.SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault().SelectedItemCurrency : null;

                        if (cur != null)
                        {
                            ssPO.CurrencyName = (((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName : string.Empty;
                            ssPO.CurrencyDisplayFormat = (((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat : string.Empty;
                        }
                    }
                    else if (parentbkss != null)
                    {
                        var cur =
                             ((parentbkss.SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault() != null) ?
                             (parentbkss.SupplierSM).Where(x => x.ServiceId == ssPO.SupplierServiceId).FirstOrDefault().SelectedItemCurrency : null;

                        if (cur != null)
                        {
                            ssPO.CurrencyName = (((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).CurrencyName : string.Empty;
                            ssPO.CurrencyDisplayFormat = (((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat != null) ? ((SQLDataAccessLayer.Models.Currencydetails)cur).DisplayFormat : string.Empty;
                        }
                    }
                }
            }
            PriceEdit.Add(ssPO);
            PriceEditDtfull.Add(ssPO);
            if (PriceEditDtfull.Where(m => m.SupplierServiceId == ssPO.SupplierServiceId && m.PricingOptionId == ssPO.PricingOptionId).Count() == 1)
            {
                DefaultSelect();
            }
            dgPricingRateEdit.ItemsSource = PriceEdit;


        }

        public void saveupdatePricingOption()
        {
            try
            {
                if (dgPricingRateEdit.Items.Count > 0)
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
                    foreach (SupplierPriceRateEdit sspo in dgPricingRateEdit.Items)
                    {
                        SupplierPriceRateEdit objPriceRateEdit = new SupplierPriceRateEdit();
                        objPriceRateEdit.PricingOptionId = sspo.PricingOptionId;
                        objPriceRateEdit.PriceEditRateId = sspo.PriceEditRateId;
                        objPriceRateEdit.ChooseEditOption = sspo.ChooseEditOption;
                        objPriceRateEdit.NetPrice = sspo.NetPrice;
                        objPriceRateEdit.MarkupPercentage = sspo.MarkupPercentage;
                        objPriceRateEdit.GrossPrice = sspo.GrossPrice;
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
                        objPriceRateEdit.Rounding = sspo.Rounding;
                        objPriceRateEdit.SupplierServiceId = sspo.SupplierServiceId;
                        objPriceRateEdit.PriceEditIsActive = sspo.PriceEditIsActive;

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
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void AllControlsReadonly()
        {
            RbGrossCommission.IsEnabled = false;
            RbNetGross.IsEnabled = false;
            RbNetMark.IsEnabled = false;
            dgPricingRateEdit.IsEnabled = false;
            BtnAddPrice.IsEnabled = false;
            BtnAddPrice.Visibility = Visibility.Collapsed;
        }
        public void ReterivePricingEditRate()
        {
            try
            {
                // SupplierPriceRateEdit sspredobj = dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
                if (PriceEdit != null)
                {
                    ListofPricedit = objsupdal.PriceEditRateRetrive(Guid.Parse(objPO.SupplierServiceId),
                         Guid.Parse(objPO.PricingOptionId), false);
                    if (ListofPricedit != null && ListofPricedit.Count > 0)
                    {
                        foreach (SupplierPriceRateEdit sup in ListofPricedit)
                        {
                            if (PriceEdit.Where(x => x.PricingOptionId == sup.PricingOptionId && x.PriceEditRateId == sup.PriceEditRateId).Count() == 0)
                            {
                                if (ListofCurrency.Count > 0)
                                {
                                    sup.CurrencyName = ListofCurrency[0].CurrencyName;
                                    sup.CurrencyDisplayFormat = ListofCurrency[0].DisplayFormat;
                                }

                                PriceEdit.Add(sup);
                                PriceEditDtfull.Add(sup);
                            }
                        }
                        // PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, sspredobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);
                    }
                    if (ListofPricedit == null || ListofPricedit.Count == 0)
                    {
                        dgPricingRateEdit.ItemsSource = null;
                    }
                    dgPricingRateEdit.ItemsSource = PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList();
                }

                if (dgPricingRateEdit.Items.Count > 0)
                {
                    foreach (SupplierPriceRateEdit sup in dgPricingRateEdit.Items)
                    {
                        if (sup.PricingOptionId == objPO.PricingOptionId)
                        {
                            Chbru1.IsChecked = (sup.Rounding == 1) ? true : false;
                            Chbru2.IsChecked = (sup.Rounding == 5) ? true : false;
                            Chbru3.IsChecked = (sup.Rounding == 10) ? true : false;

                            RbNetMark.IsChecked = (sup.ChooseEditOption == 1) ? true : false;
                            RbNetGross.IsChecked = (sup.ChooseEditOption == 2) ? true : false;
                            RbGrossCommission.IsChecked = (sup.ChooseEditOption == 3) ? true : false;
                        }
                    }
                    if (RbNetMark.IsChecked == true) { Roundingcheckboxenabledisable("enable"); }
                    else
                    {
                        Roundingcheckboxenabledisable("disable");
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }



        public void DeletePriceEditRate(SupplierPriceRateEdit ssm)
        {
            try
            {
                if (dgPricingRateEdit.Items.Count > 0)
                {

                    SupplierPriceRateEdit objpried = new SupplierPriceRateEdit();

                    objpried.PriceEditRateId = ssm.PriceEditRateId;
                    objpried.PricingOptionId = ssm.PricingOptionId;
                    objpried.IsDeleted = true;
                    objpried.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objsupdal.DeletePriceEditRate(objpried);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        // if (objret.ToString().ToLower() == "1")
                        // MessageBox.Show("Supplier Warning Deleted successfully");
                        PriceEdit.Remove(PriceEdit.Where(m => m.PricingOptionId == ssm.PricingOptionId && m.PriceEditRateId == ssm.PriceEditRateId).FirstOrDefault());
                        PriceEditDtfull.Remove(PriceEditDtfull.Where(m => m.PricingOptionId == ssm.PricingOptionId && m.PriceEditRateId == ssm.PriceEditRateId).FirstOrDefault());
                        //dgPricingRateEdit.ItemsSource = PriceEditDt;
                        ReterivePricingEditRate();
                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void BtnAddPrice_Click(object sender, RoutedEventArgs e)
        {
            AddPricingOption();
        }

        private void btnPriceEditRateDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (((System.Windows.FrameworkElement)sender).DataContext != null)
                {
                    SupplierPriceRateEdit objPR = (SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext; //dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
                    DeletePriceEditRate(objPR);
                }
            }

        }
        private void msEnter(object sender, MouseEventArgs e)
        {

            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#C90A37"));
        }

        private void msLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF003D"));
        }

        private void RbNetMark_Click(object sender, RoutedEventArgs e)
        {
            Roundingcheckboxenabledisable("enable");

        }

        private void RbNetGross_Click(object sender, RoutedEventArgs e)
        {
            Roundingcheckboxenabledisable("disable");
        }

        private void RbGrossCommission_Click(object sender, RoutedEventArgs e)
        {
            Roundingcheckboxenabledisable("disable");
        }

        private void Roundingcheckboxenabledisable(string opt)
        {
            if (opt.ToString().ToLower() == "disable")
            {
                Chbru1.IsEnabled = false;
                Chbru2.IsEnabled = false;
                Chbru3.IsEnabled = false;
            }
            if (opt.ToString().ToLower() == "enable")
            {
                Chbru1.IsEnabled = true;
                Chbru2.IsEnabled = true;
                Chbru3.IsEnabled = true;
            }
        }
        private void Chbru3_Click(object sender, RoutedEventArgs e)
        {
            Chbru1.IsChecked = false;
            Chbru2.IsChecked = false;
            if (Chbru3.IsChecked == true)
            {
                Roundingcalucation(10);
            }
            else if (Chbru1.IsChecked == false && Chbru2.IsChecked == false &&
            Chbru3.IsChecked == false)
            {
                Roundingcalucation(0);
            }
        }

        private void Chbru2_Click(object sender, RoutedEventArgs e)
        {
            Chbru1.IsChecked = false;
            Chbru3.IsChecked = false;
            if (Chbru2.IsChecked == true)
            {
                Roundingcalucation(5);
            }
            else if (Chbru1.IsChecked == false && Chbru2.IsChecked == false &&
            Chbru3.IsChecked == false)
            {
                Roundingcalucation(0);
            }
        }
        private void Chbru1_Click(object sender, RoutedEventArgs e)
        {
            Chbru2.IsChecked = false;
            Chbru3.IsChecked = false;

            if (Chbru1.IsChecked == true)
            {
                Roundingcalucation(1);
            }
            else if (Chbru1.IsChecked == false && Chbru2.IsChecked == false &&
            Chbru3.IsChecked == false)
            {
                Roundingcalucation(0);
            }
        }

        private void Roundingcalucation(int roundvalue)
        {
            if (dgPricingRateEdit.Items.Count > 0)
            {
                foreach (SupplierPriceRateEdit sup in PriceEdit)
                {
                    if (sup.PricingOptionId == objPO.PricingOptionId)
                    {
                        sup.Rounding = roundvalue;
                    }
                }
                dgPricingRateEdit.Items.Refresh();
                dgPricingRateEdit.ItemsSource = PriceEdit;
            }
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        } 


        private void CommandBinding_CloseWindowExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            foreach (SupplierPriceRateEdit sup in dgPricingRateEdit.Items)
            {
                if (sup.PricingOptionId == objPO.PricingOptionId)
                {
                    if (Chbru1.IsChecked == true)
                    {
                        sup.Rounding = 1;
                    }
                    if (Chbru2.IsChecked == true)
                    {
                        sup.Rounding = 5;
                    }
                    if (Chbru3.IsChecked == true)
                    {
                        sup.Rounding = 10;
                    }

                    if (RbNetMark.IsChecked == true)
                    {
                        sup.ChooseEditOption = 1;
                    }
                    if (RbNetGross.IsChecked == true)
                    {
                        sup.ChooseEditOption = 2;
                    }
                    if (RbGrossCommission.IsChecked == true)
                    {
                        sup.ChooseEditOption = 3;
                    }

                }

            }
            // DefaultSelect();
            if (Parentwindow != null)
            {
                Parentwindow.PriceEditDt = PriceEditDtfull;
                Parentwindow.ReterivePriceEditRate();
            }
            if (ParentwindowUC != null)
            {
                ParentwindowUC.PriceEditDt = PriceEditDtfull;
                ParentwindowUC.ReterivePriceEditRate();
            }
            if (parentbkss != null)
            {
                parentbkss.PriceEditDt = PriceEditDtfull;
                parentbkss.ReterivePriceEditRate();
            }
            SystemCommands.CloseWindow(this);
        }

        private void DefaultSelect()
        {
            int cnt = PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).Count();
            if (cnt > 0)
            {
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Monday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Monday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Tuesday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Tuesday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Wednesday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Wednesday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Thursday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Thursday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Friday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Friday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Saturday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Saturday = true;
                }
                if (cnt == PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().Where(x => x.Sunday == false).Count())
                {
                    PriceEditDtfull.Where(x => x.PricingOptionId == objPO.PricingOptionId).FirstOrDefault().Sunday = true;
                }
            }
        }

        private void MonOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Monday = true; } else { x.Monday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }
        private void TueOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Tuesday = true; } else { x.Tuesday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }
        private void WedOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Wednesday = true; } else { x.Wednesday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;

        }
        private void ThuOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Thursday = true; } else { x.Thursday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }
        private void FriOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Friday = true; } else { x.Friday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }
        private void SatOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Saturday = true; } else { x.Saturday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }
        private void SunOnChecked(object sender, RoutedEventArgs e)
        {
            string PricerateEditid = string.Empty;
            PricerateEditid = ((SQLDataAccessLayer.Models.SupplierPriceRateEdit)((System.Windows.FrameworkElement)sender).DataContext).PriceEditRateId;
            PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList().ForEach(x => { if (x.PriceEditRateId == PricerateEditid) { x.Sunday = true; } else { x.Sunday = false; } });
            dgPricingRateEdit.ItemsSource = PriceEdit;
        }

        //private void dgPricingRateEdit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //{
        //    decimal netprice = 0;
        //    decimal Grossprice = 0;
        //    decimal markup=0;
        //    string PricerateEditid = string.Empty;


        //    if (e.EditAction==DataGridEditAction.Commit)
        //    {
        //        var column = e.Column as DataGridBoundColumn;
        //        int rowIndex = e.Row.GetIndex();
        //        if (column != null)
        //        {
        //            var bindingPath = (column.Binding as Binding).Path.Path;
        //            if (bindingPath.ToLower() == "netprice")
        //            {                      
        //                var el = e.EditingElement as TextBox;
        //                netprice = Convert.ToDecimal(!string.IsNullOrEmpty(el.Text)?el.Text:0);
        //            }
        //            if (bindingPath.ToLower() == "markuppercentage")
        //            {                        
        //                var mkpval = e.EditingElement as TextBox;

        //                markup = Convert.ToDecimal(!string.IsNullOrEmpty(mkpval.Text)? mkpval.Text:0);
        //            }
        //            if (bindingPath.ToLower() == "priceeditrateId")
        //            {                        
        //                var predratid = e.EditingElement as TextBox;
        //                PricerateEditid = (!string.IsNullOrEmpty(predratid.Text)? predratid.Text:string.Empty);
        //            }
        //        }
        //    }
        //    Grosscalcuation(netprice, markup, PricerateEditid);


        //}

        private void Grosscalcuation(decimal netprice, decimal markup, string PricerateEditid)
        {
            string grossprice = (netprice + ((netprice * markup) / 100)).ToString();
            //PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId && x.PriceEditRateId == PricerateEditid).FirstOrDefault().GrossPrice =Convert.ToDecimal(grossprice);


        }

        private void dgPricingRateEdit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.OriginalSource.GetType().Name.ToLower() == "textbox")
            {
                string typedtext = ((System.Windows.Controls.TextBox)e.OriginalSource).Text + e.Text;
                e.Handled = !ValidationClass.IsNumericDotwith2decimal(typedtext);
            }
        }

        private void dgPricingRateEdit_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgPricingRateEdit.BeginEdit();
        }

        private void Txtnetprice_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "0.00" || textBox.Text == "0")
                {
                    textBox.Text = string.Empty;
                }
            }
        }

        //private void dgPricingRateEdit_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        //{
        //    if (e.AddedCells.Count == 0) return;
        //    var currentCell = e.AddedCells[0];
        //    if (currentCell.Column ==
        //         dgPricingRateEdit.Columns[1])
        //    {
        //        dgPricingRateEdit.BeginEdit();
        //    }
        //}
    }

    public class CultureFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter,
            CultureInfo culture)
        {
            var value = values[0];
            var format = values[1] as String;
            var targetCulture = values[2] as string;
            if (targetCulture != null)
            {
                return string.Format(new System.Globalization.CultureInfo(targetCulture),
                    format, value);
            }
            //else
            //{
            //    return string.Format(new System.Globalization.CultureInfo("en-IN"),
            //                        format, value);
            //}
            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object
            parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
