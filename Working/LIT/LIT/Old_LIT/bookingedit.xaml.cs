using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LIT.Modules.TabControl.Commands;
using LIT.Modules.TabControl.ViewModels;
using LITModels;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for BookingEdit.xaml
    /// </summary>
    public partial class Bookingedit : Window
    {

        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        CommonAndCalcuation CommonValues = new CommonAndCalcuation();
        DBConnectionEF DBconnEF = new DBConnectionEF();

        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string recordmode = string.Empty;
        private ItineraryWindow IwParWindow;
        List<SupplierPriceRateEdit> ListofPricedit = new List<SupplierPriceRateEdit>();

        List<BookingItems> ListofSelectedBookingItems = new List<BookingItems>();
        List<BkRequestStatus> ListofRequestStatusedit = new List<BkRequestStatus>();
        List<bookingitemlist> Listofbookingitemlist=new List<bookingitemlist>();
        Errorlog errobj = new Errorlog();

        public ItineraryFollowUpTabViewModel FollowupViewModel { get; set; }
        public LIT.Modules.TabControl.Commands.ItineraryFollowUpTabCommand Followupcommd { get; set; }

        BookingItems objbkitms;


        List<Userdetails> ListUserdet = new List<Userdetails>();

        public Bookingedit()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public Bookingedit(string username, ItineraryWindow ParentWindow, BookingItems objbkitems)
        {
            InitializeComponent();
            this.DataContext = this;

            loginusername = username.Trim();
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            IwParWindow = ParentWindow;
            this.objbkitms = objbkitems;
            BookingItemsitinEditfull = ParentWindow.BookingItemsitin;
            // BookingItemsRefreshrates = ParentWindow.BookingItemsitin;
            var observablecollection = new ObservableCollection<BookingItems>(ParentWindow.BookingItemsitin.Where(x => x.BookingID == objbkitems.BookingID).ToList());
            BookingItemsitinEdit = observablecollection;

            this.FollowupViewModel = new ItineraryFollowUpTabViewModel();
            this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
            this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
            this.FollowupViewModel.Loginuserid = loginuserid;
            this.Followupcommd = new ItineraryFollowUpTabCommand(FollowupViewModel);
            this.Followupcommd.RetrieveCommand.Execute();
            ListUserdet = loadDropDownListValues.LoadUserDropDownlist("User");
            Folloupcntrl.DataContext = this.FollowupViewModel;
            //Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x=>x.Bookingid==this.objbkitms.BookingID).ToList();
            LoadComboValues();
            LoadBookingEditGrid(objbkitms.BookingID);
        }


        private ObservableCollection<BookingItems> _BookingItemsitinEdit;
        public ObservableCollection<BookingItems> BookingItemsitinEdit
        {
            get { return _BookingItemsitinEdit ?? (_BookingItemsitinEdit = new ObservableCollection<BookingItems>()); }
            set
            {
                _BookingItemsitinEdit = value;
            }
        }

        private ObservableCollection<BookingItems> _BookingItemsitinEditfull;
        public ObservableCollection<BookingItems> BookingItemsitinEditfull
        {
            get { return _BookingItemsitinEditfull ?? (_BookingItemsitinEditfull = new ObservableCollection<BookingItems>()); }
            set
            {
                _BookingItemsitinEditfull = value;
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


        //Refresh Rate _BookingItemsRefreshrates
        //private ObservableCollection<BookingItems> _BookingItemsRefreshrates;
        //public ObservableCollection<BookingItems> BookingItemsRefreshrates
        //{
        //    get { return _BookingItemsRefreshrates ?? (_BookingItemsRefreshrates = new ObservableCollection<BookingItems>()); }
        //    set
        //    {
        //        _BookingItemsRefreshrates = value;
        //    }
        //}
        private void LoadComboValues()
        {
            try
            {
                ListofRequestStatusedit = loadDropDownListValues.LoadRequestStatus();
                if (ListofRequestStatusedit != null && ListofRequestStatusedit.Count > 0)
                {
                    CmbRequestStatus.ItemsSource = ListofRequestStatusedit;
                    CmbRequestStatus.SelectedValuePath = "RequestStatusID";
                    CmbRequestStatus.DisplayMemberPath = "RequestStatusName";

                    CmbRequestStatuslist.ItemsSource = ListofRequestStatusedit;
                    CmbRequestStatuslist.SelectedValuePath = "RequestStatusID";
                    CmbRequestStatuslist.DisplayMemberPath = "RequestStatusName";
                    CmbRequestStatuslist.SelectedValue = (((SQLDataAccessLayer.Models.BkRequestStatus)BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus) != null) ?

                        ((SQLDataAccessLayer.Models.BkRequestStatus)BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus).RequestStatusID.ToString() : Guid.Empty.ToString();
                    CmbRequestStatus.SelectedValuePath = (((SQLDataAccessLayer.Models.BkRequestStatus)BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus) != null) ?
                        ((SQLDataAccessLayer.Models.BkRequestStatus)BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus).RequestStatusID.ToString() : Guid.Empty.ToString();
                }

                if (BookingItemsitinEditfull != null && BookingItemsitinEditfull.Count > 0)
                {
                    Listofbookingitemlist = new List<bookingitemlist>();
                    
                    foreach( BookingItems objbk in BookingItemsitinEditfull)
                    {
                        bookingitemlist bklist = new bookingitemlist();
                        bklist.BookingID = objbk.BookingID;
                        bklist.BookingName = objbk.BookingName;
                        Listofbookingitemlist.Add(bklist);
                    }
                    
                    CmbBookingItemsList.ItemsSource = Listofbookingitemlist;
                    CmbBookingItemsList.SelectedValuePath = "BookingID";
                    CmbBookingItemsList.DisplayMemberPath = "BookingName";
                    // CmbBookingItemsList.SelectedValuePath = BookingItemsitinEdit.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().BookingName;
                    CmbBookingItemsList.SelectedValue = BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().BookingID.ToString();

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingEdit", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private string splitTime(string time)
        {
            string[] parts = time.Split(':');
            return string.Join(":", parts[0], parts[1]);
        }

        public void LoadBookingEditGrid(long selectedvalue)
        {
            try
            {
                if (selectedvalue > 0 && (BookingItemsitinEditfull != null && BookingItemsitinEditfull.Count > 0))
                {
                    dgBookingedited.ItemsSource = BookingItemsitinEditfull.Where(x => x.BookingID == selectedvalue).ToList();
                    ListofSelectedBookingItems = BookingItemsitinEditfull.Where(x => x.BookingID == selectedvalue).ToList();
                    objbkitms = ListofSelectedBookingItems.FirstOrDefault();
                    var observablecollection = new ObservableCollection<BookingItems>(BookingItemsitinEditfull.Where(x => x.BookingID == selectedvalue).ToList());
                    BookingItemsitinEdit = observablecollection;

                    foreach (BookingItems itmbk in ListofSelectedBookingItems)
                    {
                        txtbookingname.Text = (!string.IsNullOrEmpty(itmbk.BookingName)) ? itmbk.BookingName : string.Empty;
                        txtbookingID.Text = (itmbk.BookingID != null) ? itmbk.BookingID.ToString() : string.Empty;
                        txtbookingAutoID.Text = (itmbk.BookingAutoID != null) ? itmbk.BookingAutoID.ToString() : string.Empty;
                        txtitemname.Text = (!string.IsNullOrEmpty(itmbk.ItemDescription)) ? itmbk.ItemDescription : string.Empty;
                        TxtCheckinTime.Text = (itmbk.StartTime != null && itmbk.StartTime.Length == 5) ? itmbk.StartTime.ToString() : itmbk.StartTime == "00:00" ? "00:00": splitTime(itmbk.StartTime);
                        TxtCheckoutTime.Text = (itmbk.EndTime != null && itmbk.EndTime.Length == 5) ? itmbk.EndTime.ToString() : itmbk.EndTime == "00:00" ? "00:00" : splitTime(itmbk.EndTime);
                        TxtCheckindate.SelectedDate = (itmbk.StartDate != null) ? itmbk.StartDate : null;

                        TxtCheckoutDate.SelectedDate = (itmbk.Enddate != null) ? itmbk.Enddate : null;

                        txtnights.Text = (itmbk.NtsDays != null) ? itmbk.NtsDays.ToString() : string.Empty;
                        TxtQuantity.Text = (itmbk.Qty != null) ? itmbk.Qty.ToString() : string.Empty;
                        CmbRequestStatuslist.SelectedValue = ((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus) != null) ?
                            ((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitinEditfull.Where(x => x.BookingID == objbkitms.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID : Guid.Empty.ToString();
                        TxtSupplierref.Text = (!string.IsNullOrEmpty(itmbk.Ref)) ? itmbk.Ref : string.Empty;

                        string grossunit = string.Empty;
                        string netunit = string.Empty;
                        if (!string.IsNullOrEmpty(itmbk.Grossunit))
                        {
                            grossunit = itmbk.Grossunit.Contains(',') ? itmbk.Grossunit.Split(',')[0].ToString() : itmbk.Grossunit.ToString();
                        }
                        if (!string.IsNullOrEmpty(itmbk.Netunit))
                        {
                            netunit = itmbk.Netunit.Contains(',') ? itmbk.Netunit.Split(',')[0].ToString() : itmbk.Netunit.ToString();

                        }

                        lblGrossvalue.Content = itmbk.BkgCurDisplayFormat + " " + grossunit;
                        lblmarkupvalue.Content = (!string.IsNullOrEmpty(itmbk.MarkupPercentage)) ? itmbk.MarkupPercentage + "%" : string.Empty;
                        lblCommissionvalue.Content = (!string.IsNullOrEmpty(itmbk.CommissionPercentage)) ? itmbk.CommissionPercentage : string.Empty;
                        lblnetvalue.Content = itmbk.BkgCurDisplayFormat + " " + netunit;
                        lblTermsvalue.Content = (!string.IsNullOrEmpty(itmbk.PaymentTerms)) ? itmbk.PaymentTerms : string.Empty;
                        OverridePaymentTerm.Text = (!string.IsNullOrEmpty(itmbk.SupplierPaymentTermsOverrideindays.ToString())) ? itmbk.SupplierPaymentTermsOverrideindays.ToString() : string.Empty;
                        DepositeAmountOverrideValue.Text = (!string.IsNullOrEmpty(itmbk.SupplierPaymentOverrideDepositAmount.ToString())) ? itmbk.SupplierPaymentOverrideDepositAmount.ToString() : string.Empty;
                        Termsvalue.Content =itmbk.SupplierPaymentTermsindays.ToString();
                        DepositeAmountvalue.Content = itmbk.SupplierPaymentDepositAmount.ToString();

                        //lblGrossTotalcur.Text = itmbk.ItinCurDisplayFormat;
                        //lblNetTotalcur.Text = itmbk.ItinCurDisplayFormat;

                        TxtBooking.Text = itmbk.BookingNote;
                        TxtVoucher.Text = itmbk.VoucherNote;
                        TxtPrivateMsg.Text = itmbk.Privatemsg;

                    }
                    lbltotalrecord.Content = BookingItemsitinEditfull.Count.ToString();
                    txtpagerecord.Text = Convert.ToInt32(BookingItemsitinEditfull.IndexOf(objbkitms) + 1).ToString();

                    totalcalculation();
                    BookingItemsitinEditfull.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = false;
                    });

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingEdit", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void CmbBookingItemsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBookingItemsList.SelectedValue != null)
            {
                LoadBookingEditGrid((CmbBookingItemsList.SelectedValue != null) ? Convert.ToInt64(CmbBookingItemsList.SelectedValue) : 0);
                this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
                this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
                this.FollowupViewModel.Loginuserid = loginuserid;
                // this.Followupcommd = new FollowUpCommand(FollowupViewModel);
                this.Followupcommd.RetrieveCommand.Execute();
            }
        }

        private void btnBookingEditDelete_Click(object sender, RoutedEventArgs e)
        {
            //BookingItems objBI = dgBookingedited.SelectedItem as BookingItems;
            if (((System.Windows.FrameworkElement)sender).DataContext != null)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (((System.Windows.FrameworkElement)sender).DataContext != null)
                    {
                        BookingItems objBI = (BookingItems)((System.Windows.FrameworkElement)sender).DataContext; //dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
                        DeleteBookingItemsEdit(objBI);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Please select booking item");
                return;
            }
        }

        private void DeleteBookingItemsEdit(BookingItems ObjBidel)
        {
            try
            {
                int ind = 0;
                ind = Convert.ToInt32(BookingItemsitinEdit.IndexOf(objbkitms));

                if (dgBookingedited.Items.Count > 0)
                {

                    BookingItems objBItdel = new BookingItems();
                    objBItdel.ItineraryID = ObjBidel.ItineraryID;
                    objBItdel.BookingID = ObjBidel.BookingID;
                    objBItdel.SupplierID = ObjBidel.SupplierID;
                    objBItdel.ServiceID = ObjBidel.ServiceID;
                    objBItdel.PricingRateID = ObjBidel.PricingRateID;
                    objBItdel.PricingOptionId = ObjBidel.PricingOptionId;
                    objBItdel.IsDeleted = true;
                    objBItdel.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    string objret = objitdal.DeleteBookingItems(objBItdel);
                    if (!string.IsNullOrEmpty(objret))
                    {
                        if (objret.ToString().ToLower() == "1")
                            MessageBox.Show("Booking Item Deleted successfully");
                        BookingItemsitinEdit.Remove(BookingItemsitinEdit.Where(m => m.BookingID == ObjBidel.BookingID
                        && m.ItineraryID == ObjBidel.ItineraryID && m.SupplierID == ObjBidel.SupplierID &&
                        m.ServiceID == ObjBidel.ServiceID).FirstOrDefault());
                        BookingItemsitinEditfull.Remove(BookingItemsitinEditfull.Where(m => m.BookingID == ObjBidel.BookingID
                        && m.ItineraryID == ObjBidel.ItineraryID && m.SupplierID == ObjBidel.SupplierID &&
                        m.ServiceID == ObjBidel.ServiceID).FirstOrDefault());
                        if (ind >= BookingItemsitinEditfull.Count)
                        {
                            objbkitms = BookingItemsitinEditfull[ind - 1];
                            LoadBookingEditGrid(objbkitms.BookingID);
                        }
                        else if (ind <= BookingItemsitinEditfull.Count)
                        {
                            objbkitms = BookingItemsitinEditfull[ind];
                            LoadBookingEditGrid(objbkitms.BookingID);
                        }
                        // dgBookingedited.ItemsSource = BookingItemsitinEdit;
                        IwParWindow.BookingItemsitin = BookingItemsitinEditfull;
                        IwParWindow.dgItinBooking.ItemsSource = BookingItemsitinEditfull;


                    }

                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingEdit", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
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

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ind = 0;
                ind = Convert.ToInt32(BookingItemsitinEditfull.IndexOf(objbkitms) + 1);
                if (ind != BookingItemsitinEditfull.Count)
                {
                    if (BookingItemsitinEditfull.Count > ind)
                    {
                        objbkitms = BookingItemsitinEditfull[ind];
                        if (objbkitms != null)
                        {
                            BookingItemsitinEdit.Clear();
                            BookingItemsitinEdit.Add(objbkitms);
                            CmbBookingItemsList.SelectedIndex = ind;
                            //objbkitms.IsRefreshed = true;
                            LoadBookingEditGrid(objbkitms.BookingID);
                            this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
                            this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
                            this.FollowupViewModel.Loginuserid = loginuserid;
                            // this.Followupcommd = new FollowUpCommand(FollowupViewModel);
                            this.Followupcommd.RetrieveCommand.Execute();
                            //Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x => x.Bookingid == this.objbkitms.BookingID).ToList();
                            // Folloupcntrl.DataContext = this.FollowupViewModel;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("BookingEdit", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            int ind = 0;
            ind = Convert.ToInt32(BookingItemsitinEditfull.IndexOf(objbkitms));
            if (ind != 0)
            {
                if (BookingItemsitinEditfull.Count > ind - 1)
                {
                    objbkitms = BookingItemsitinEditfull[ind - 1];
                    if (objbkitms != null)
                    {
                        BookingItemsitinEdit.Clear();
                        BookingItemsitinEdit.Add(objbkitms);
                        CmbBookingItemsList.SelectedIndex = ind - 1;
                        //objbkitms.IsRefreshed = true;
                        LoadBookingEditGrid(objbkitms.BookingID);
                        // this.FollowupViewModel = new FollowupViewModel();
                        this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
                        this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
                        this.FollowupViewModel.Loginuserid = loginuserid;
                        // this.Followupcommd = new FollowUpCommand(FollowupViewModel);
                        this.Followupcommd.RetrieveCommand.Execute();
                        //Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x => x.Bookingid == objbkitms.BookingID).ToList();
                    }

                }
            }
        }

        private void btnfirst_Click(object sender, RoutedEventArgs e)
        {
            int ind = 0;
            objbkitms = BookingItemsitinEditfull[ind];
            if (objbkitms != null)
            {
                BookingItemsitinEdit.Clear();
                BookingItemsitinEdit.Add(objbkitms);
                CmbBookingItemsList.SelectedIndex = ind;
                //objbkitms.IsRefreshed = true;
                LoadBookingEditGrid(objbkitms.BookingID);
                // this.FollowupViewModel = new FollowupViewModel();
                this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
                this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
                this.FollowupViewModel.Loginuserid = loginuserid;
                // this.Followupcommd = new FollowUpCommand(FollowupViewModel);
                this.Followupcommd.RetrieveCommand.Execute();
                //Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x => x.Bookingid == this.objbkitms.BookingID).ToList();
            }

        }

        private void btnLast_Click(object sender, RoutedEventArgs e)
        {
            int ind = BookingItemsitinEditfull.Count - 1;
            objbkitms = BookingItemsitinEditfull[ind];
            if (objbkitms != null)
            {
                BookingItemsitinEdit.Clear();
                BookingItemsitinEdit.Add(objbkitms);
                CmbBookingItemsList.SelectedIndex = ind;
                // objbkitms.IsRefreshed = true;
                LoadBookingEditGrid(objbkitms.BookingID);
                // this.FollowupViewModel = new FollowupViewModel();
                this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
                this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
                this.FollowupViewModel.Loginuserid = loginuserid;
                // this.Followupcommd = new FollowUpCommand(FollowupViewModel);
                this.Followupcommd.RetrieveCommand.Execute();
                // Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x => x.Bookingid == this.objbkitms.BookingID).ToList();
            }
        }

        private void BtnBookingEditAdd_Click(object sender, RoutedEventArgs e)
        {
            BookingSupplierSearch wnbkadd = new BookingSupplierSearch(loginusername, IwParWindow);
            wnbkadd.ShowDialog();
            LoadBookingEditGrid(objbkitms.BookingID);
        }

        private void dgBookingedited_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == System.Windows.Controls.DataGridEditAction.Commit)
            {
                var column = e.Column as System.Windows.Controls.DataGridBoundColumn;


                if (column != null && column.Binding != null)
                {
                    BookingItems objBIt = (BookingItems)(e.Row.DataContext);
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    if (bindingPath == "NtsDays")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var elnt = e.EditingElement as TextBox;

                        if (string.IsNullOrEmpty(elnt.Text))
                        {
                            System.Windows.MessageBox.Show("Please provide Nt/Days");
                            return;
                        }
                        if (ValidationClass.IsNumeric(elnt.Text) == false)
                        {
                            System.Windows.MessageBox.Show("Nt/Days allow only numeric");
                            return;
                        }

                        if (objBIt != null)
                        {

                            string servicetypenameval = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                            if (!string.IsNullOrEmpty(servicetypenameval))
                            {
                                if (servicetypenameval.ToLower() == "accommodation")
                                {
                                    //objBI.Enddate = ((DateTime)item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays));
                                }
                                else
                                {
                                    int days = (elnt != null) ? Convert.ToInt32(elnt.Text) : 0;
                                    if (days > 1)
                                    {
                                        string ermsg = "Please provide one day for this service" + objBIt.ItemDescription;
                                        System.Windows.MessageBox.Show(ermsg);
                                        elnt.Text = string.Empty;                                        
                                        elnt.Focusable = true;
                                        elnt.Focus();
                                        return;
                                    }
                                }
                            }

                            objBIt.NtsDays = Convert.ToInt32(elnt.Text);
                            if (objBIt.Qty != Convert.ToDecimal(TxtQuantity.Text) || objBIt.NtsDays != Convert.ToInt32(txtnights.Text))
                            {
                                txtnights.Text = elnt.Text;
                                CommonValues.Grossnetcalculation(objBIt);
                                LoadBookingEditGrid(objBIt.BookingID);
                                if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
                                {
                                    string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                                    if (!string.IsNullOrEmpty(servicetypename))
                                    {
                                        if (servicetypename.ToLower() == "accommodation")
                                        {
                                            objBIt.Enddate = ((DateTime)objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                        }
                                        else
                                        {
                                            objBIt.Enddate = ((DateTime)objBIt.StartDate);
                                        }
                                    }
                                    // objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;                                   
                                   // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                    //LoadBookingEditGrid(objBIt.BookingID);
                                }
                            }
                            else if(((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
                            {
                                CommonValues.Grossnetcalculation(objBIt);
                                LoadBookingEditGrid(objBIt.BookingID);
                            }
                        }

                    }
                    if (bindingPath == "Qty")
                    {
                        int rowIndex = e.Row.GetIndex();
                        var elqty = e.EditingElement as TextBox;
                        if (string.IsNullOrEmpty(elqty.Text))
                        {
                            System.Windows.MessageBox.Show("Please provide Quantity");
                            return;
                        }
                        if (ValidationClass.IsNumeric(elqty.Text) == false)
                        {
                            System.Windows.MessageBox.Show("Quantity allow only numeric");
                            return;
                        }
                        if (objBIt != null)
                        {
                            
                            objBIt.Qty = Convert.ToInt32(elqty.Text);
                            if (objBIt.Qty != Convert.ToDecimal(TxtQuantity.Text) || objBIt.NtsDays != Convert.ToInt32(txtnights.Text))
                            {
                                TxtQuantity.Text = elqty.Text;
                                CommonValues.Grossnetcalculation(objBIt);
                                LoadBookingEditGrid(objBIt.BookingID);
                            }                            
                        }

                    }

                }
            }
        }


        private void ChbInvoicedChecked(object sender, RoutedEventArgs e)
        {
            long BookingID = 0;
            BookingID = ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).BookingID;

            if (BookingItemsitinEdit.Where(x => x.BookingID == BookingID).FirstOrDefault() != null)
            {
                BookingItemsitinEdit.Where(x => x.BookingID == BookingID).FirstOrDefault().Invoiced = true;
            }
            dgBookingedited.ItemsSource = BookingItemsitinEdit;

        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //IwParWindow.BookingItemsitin = null;
            //IwParWindow.CmbRequestStatusitin.ItemsSource = ListofRequestStatusedit;
            //foreach (BookingItems item in dgBookingedited.Items)
            //{
            //    var Requestid = ((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitinEditfull.Where(x => x.BookingID == item.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID;
            //    if (!String.IsNullOrEmpty(Requestid))
            //    {
            //        item.SelectedItemRequstStatus = ListofRequestStatusedit.Where(x => x.RequestStatusID == Requestid).FirstOrDefault();
            //        BookingItemsitinEditfull.Where(x => x.BookingID == item.BookingID).FirstOrDefault().SelectedItemRequstStatus=item.SelectedItemRequstStatus;
            //    }
            //}

            // IwParWindow.dgItinBooking.ItemsSource = BookingItemsitinEditfull;


            IwParWindow.BookingItemsitin = BookingItemsitinEditfull;
            IwParWindow.ReteriveBookingItems();

            // Prevent the error sound by disabling the window's sound effects

            Dispatcher.Invoke(() =>
            {
                // Prevent the error sound by disabling the window's sound effects

                // Close the window
                this.Close();

                SystemCommands.CloseWindow(this);
            });




        }

        private void txtnights_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ValidationClass.IsNumeric(TxtQuantity.Text) == false)
            {
                System.Windows.MessageBox.Show("Quantity allow only numeric");
                return;
            }
            if (ValidationClass.IsNumeric(txtnights.Text) == false)
            {
                System.Windows.MessageBox.Show("Nts/Days allow only numeric");
                return;
            }
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtQuantity.Text.Length > 0 && (!string.IsNullOrEmpty(TxtQuantity.Text)) && txtnights.Text.Length > 0 && (!string.IsNullOrEmpty(txtnights.Text)))
            {
                objBIt.Qty = Convert.ToInt32(TxtQuantity.Text);
                objBIt.NtsDays = Convert.ToInt32(txtnights.Text);
                string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                if (!string.IsNullOrEmpty(servicetypename))
                {
                    if (servicetypename.ToLower() == "accommodation")
                    {
                        //objBIt.Enddate = ((DateTime)objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                    }
                    else
                    {
                        int days = (txtnights.Text != null) ? Convert.ToInt32(txtnights.Text) : 0;
                        if (days > 1)
                        {
                            string ermsg = "Please provide one day for this service" + objBIt.ItemDescription;
                            System.Windows.MessageBox.Show(ermsg);
                            txtnights.Text = string.Empty;
                            txtnights.Focus();
                            txtnights.Focusable = true;
                            return;
                        }
                        //objBIt.Enddate = ((DateTime)objBIt.StartDate); ;
                    }
                }
                CommonValues.Grossnetcalculation(objBIt);
                // LoadBookingEditGrid(objBIt.BookingID);
                if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)) && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
                {
                    // objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
                    //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                    
                    if (!string.IsNullOrEmpty(servicetypename))
                    {
                        if (servicetypename.ToLower() == "accommodation")
                        {
                            objBIt.Enddate = ((DateTime)objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                        }
                        else
                        {
                            objBIt.Enddate = ((DateTime)objBIt.StartDate); ;
                        }
                    }
                    LoadBookingEditGrid(objBIt.BookingID);
                }
            }
        }

        //private void Grossnetcalculation(BookingItems objbkt)
        //{
        //    decimal Grossvalue = 0, netvalue = 0;
        //    string markuppercen = string.Empty, commissionpercen = string.Empty;
        //    int cnt = Convert.ToInt32(objbkt.NtsDays);
        //    string dayofweek = string.Empty;
        //    decimal grossprice = 0; decimal netprice = 0; decimal grosspricetotal = 0; decimal netpricetotal = 0;
        //    string strgrossvalue = string.Empty;
        //    string strnetvalue = string.Empty;
        //    string[] strgrossvaluearr = new string[cnt];
        //    string[] strnetvaluearr = new string[cnt];
        //    ReterivePricingEditRate(objbkt.ServiceID, objbkt.PricingOptionId);
        //    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId).Count() > 0)
        //    {
        //        //var listsupdt = SupplierSRatesDt.Where(x => x.ValidFrom <= item.BookingStartDate && x.ValidTo >= (DateTime)(item.BookingStartDate).AddDays(Convert.ToInt32(item.NightDays)) && x.SupplierServiceId == item.ServiceID && x.SupplierServiceDetailsRateId == objBI.PricingRateID).ToList();

        //        dayofweek = ((objbkt.StartDate).DayOfWeek).ToString();
        //        for (int i = 0; i < cnt; i++)
        //        {

        //            switch (dayofweek.ToString().ToLower())
        //            {

        //                case "sunday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Sunday == true).FirstOrDefault().NetPrice;
        //                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "monday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Monday == true).FirstOrDefault().NetPrice;
        //                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "tuesday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Tuesday == true).FirstOrDefault().NetPrice;
        //                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "wednesday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Wednesday == true).FirstOrDefault().NetPrice;
        //                        // strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "thursday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Thursday == true).FirstOrDefault().NetPrice;
        //                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "friday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Friday == true).FirstOrDefault().NetPrice;
        //                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //                case "saturday":
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault() != null)
        //                    {
        //                        grossprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault().GrossPrice;
        //                        netprice = PriceEditDt.Where(x => x.PricingOptionId == objbkt.PricingOptionId && x.Saturday == true).FirstOrDefault().NetPrice;
        //                        //strgrossvalue = strgrossvalue + "," + grossprice.ToString();
        //                        grosspricetotal = grosspricetotal + (grossprice * objbkt.Qty);
        //                        netpricetotal = netpricetotal + (netprice * objbkt.Qty);
        //                        strgrossvaluearr[i] = grossprice.ToString("0.00");
        //                        strnetvaluearr[i] = netprice.ToString("0.00");
        //                    }
        //                    break;
        //            }
        //            dayofweek = ((objbkt.StartDate.AddDays(i + 1)).DayOfWeek).ToString();
        //        }
        //        //sup.NetPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId &&  >= startDate.Date && a.Start.Date <= endDate).FirstOrDefault().NetPrice;
        //        //sup.MarkupPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().MarkupPercentage;
        //        //sup.GrossPrice = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().GrossPrice;
        //        //sup.CommissionPercentage = PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId).FirstOrDefault().CommissionPercentage;

        //    }

        //    /* decimal Grossvalue = 0, netvalue = 0;
        //    BookingItems objBIt = (BookingItems)objbkitms;
        //    Grossvalue = BookingItemsitinEditfull.Where(x => x.BookingID == objBIt.BookingID && x.ItineraryID == objBIt.ItineraryID).FirstOrDefault().Grossunit;
        //    netvalue = BookingItemsitinEditfull.Where(x => x.BookingID == objBIt.BookingID && x.ItineraryID == objBIt.ItineraryID).FirstOrDefault().Netunit;


        //    objBIt.Grossunit = Grossvalue;
        //    objBIt.GrossAdj = Grossvalue;
        //    objBIt.Grossfinal = Grossvalue * objBIt.Qty * objBIt.NtsDays;
        //    objBIt.Grosstotal = Grossvalue * objBIt.Qty * objBIt.NtsDays;

        //    objBIt.Netfinal = netvalue * objBIt.Qty * objBIt.NtsDays;
        //    objBIt.Nettotal = netvalue * objBIt.Qty * objBIt.NtsDays;
        //    objBIt.Netunit = netvalue;*/

        //    objbkt.Grossunit = (strgrossvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join(",", strgrossvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strgrossvaluearr.Distinct().OfType<string>().ToArray());
        //    objbkt.GrossAdj = Grossvalue;
        //    objbkt.Grossfinal = grosspricetotal;//Grossvalue * objBI.Qty* objBI.NtsDays;
        //    objbkt.Grosstotal = grosspricetotal;//Grossvalue * objBI.Qty * objBI.NtsDays;

        //    objbkt.Netfinal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 
        //    objbkt.Nettotal = netpricetotal;//netvalue * objBI.Qty * objBI.NtsDays; 

        //    objbkt.Netunit = (strnetvaluearr.Distinct().OfType<string>().Count() > 1) ? String.Join(",", strnetvaluearr.Distinct().OfType<string>().ToArray()) : string.Join("", strnetvaluearr.Distinct().OfType<string>().ToArray());


        //    Tuple<decimal, decimal> curr = CommonValues.CalculateItinearycurrency(objbkt.ItinCurrencyID, objbkt.BkgCurrencyID);
        //    if (curr != null)
        //    {
        //        if (objbkt.ItinCurrency != objbkt.BkgCurrencyName && objbkt.ItinCurrencyID != objbkt.BkgCurrencyID)
        //        {
        //            if (objbkt.Grosstotal > 0 && objbkt.Nettotal > 0 && curr.Item1 > 0 && curr.Item2 > 0)
        //            {
        //                objbkt.BkgNettotal = objbkt.Nettotal;
        //                objbkt.BkgNetfinal = objbkt.Netfinal;
        //                objbkt.BkgGrosstotal = objbkt.Grosstotal;
        //                objbkt.BkgGrossfinal = objbkt.Grossfinal;

        //                objbkt.Nettotal = curr.Item2 * objbkt.Nettotal;
        //                objbkt.Netfinal = curr.Item2 * objbkt.Netfinal;
        //                objbkt.Grosstotal = curr.Item2 * objbkt.Grosstotal;
        //                objbkt.Grossfinal = curr.Item2 * objbkt.Grossfinal;


        //            }
        //        }
        //        else
        //        {
        //            objbkt.BkgNettotal = curr.Item1 * objbkt.Nettotal;
        //            objbkt.BkgNetfinal = curr.Item1 * objbkt.Netfinal;
        //            objbkt.BkgGrosstotal = curr.Item1 * objbkt.Grosstotal;
        //            objbkt.BkgGrossfinal = curr.Item1 * objbkt.Grossfinal;
        //        }
        //    }
        //    else
        //    {
        //        objbkt.BkgNettotal = objbkt.Nettotal;
        //        objbkt.BkgNetfinal = objbkt.Netfinal;
        //        objbkt.BkgGrosstotal = objbkt.Grosstotal;
        //        objbkt.BkgGrossfinal = objbkt.Grossfinal;
        //    }
        //    LoadBookingEditGrid(objbkt.BookingID);

        //}

        private void TxtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;

            if (ValidationClass.IsNumeric(TxtQuantity.Text) == false)
            {
                System.Windows.MessageBox.Show("Quantity allow only numeric");
                return;
            }
            if (TxtQuantity.Text.Length > 0 && (!string.IsNullOrEmpty(TxtQuantity.Text)) && txtnights.Text.Length > 0 && (!string.IsNullOrEmpty(txtnights.Text))
               && (objBIt.Qty != Convert.ToDecimal(TxtQuantity.Text) || objBIt.NtsDays != Convert.ToInt32(txtnights.Text)))
            {
                objBIt.Qty = Convert.ToInt32(TxtQuantity.Text);
                objBIt.NtsDays = Convert.ToInt32(txtnights.Text);
                CommonValues.Grossnetcalculation(objBIt);
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void TxtSupplierref_TextChanged(object sender, TextChangedEventArgs e)
        {


        }

        private void TxtCheckindate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            DateTime Startdates = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
            if (e.RemovedItems.Count > 0)
            {
                DateTime OldStartDateVal = (DateTime)e.RemovedItems[0];
                if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
                {
                    if (objBIt.StartDate == OldStartDateVal)
                    {
                        //objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
                        //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                        //LoadBookingEditGrid(objBIt.BookingID);

                        BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;
                        string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
                            {
                                objBIt.Enddate = ((DateTime)objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                            }
                            else
                            {
                                objBIt.Enddate = ((DateTime)objBIt.StartDate); ;
                            }
                        }
                        BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().Enddate = objBIt.Enddate;//(objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                        //TxtCheckindate.SelectedDate = Startdates;
                        dgBookingedited.ItemsSource = BookingItemsitinEdit;

                    }

                    if (objBIt.StartDate != OldStartDateVal && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
                    {

                        if (objBIt.IsRefreshed == false || objBIt.IsRefreshed == null)
                        {
                            MessageBoxResult mesbox = System.Windows.MessageBox.Show("Open rates management screen to refresh rates and check warnings?", "Message", System.Windows.MessageBoxButton.OKCancel);

                            if (mesbox == MessageBoxResult.OK)
                            {
                                objBIt.OldStartDate = OldStartDateVal;
                                objBIt.OldNetunit = objBIt.Netunit;
                                objBIt.IsRefreshed = true;

                                objBIt.NewStartDate = (DateTime)TxtCheckindate.SelectedDate;
                                objBIt.OldNewDatediff = ((DateTime)objBIt.NewStartDate - objBIt.OldStartDate).Days;
                                // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));

                                // dgBookingedited.ItemsSource = BookingItemsitinEditfull;
                                //string username, ItineraryWindow ParentWindow, BookingItems objbkitems

                                // RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit",this);
                                RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsitinEditfull, objBIt, "BookingEdit", this);
                                objref.ShowDialog();

                            }
                        }
                        else
                        {
                            //objBIt.StartDate = OldStartDateVal;
                            //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                            //BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = OldStartDateVal;
                            //TxtCheckindate.SelectedDate = OldStartDateVal;
                            //dgBookingedited.ItemsSource = BookingItemsitinEdit;
                        }

                        // LoadBookingEditGrid(objBIt.BookingID);
                    }
                }
            }
            else
            {
                if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
                {
                    //objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
                    //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                    //LoadBookingEditGrid(objBIt.BookingID);

                    BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;
                    string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                    if (!string.IsNullOrEmpty(servicetypename))
                    {
                        if (servicetypename.ToLower() == "accommodation")
                        {
                            objBIt.Enddate = ((DateTime)objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                        }
                        else
                        {
                            objBIt.Enddate = ((DateTime)objBIt.StartDate); ;
                        }
                    }
                    BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().Enddate = objBIt.Enddate;// (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                    //TxtCheckindate.SelectedDate = Startdates;
                    dgBookingedited.ItemsSource = BookingItemsitinEdit;

                }
            }


            /*  BookingItems objBIt = (BookingItems)objbkitms;
              //long BookingID = 0;

              if (e.RemovedItems.Count > 0)
              {
                  DateTime OldStartDateVal = (DateTime)e.RemovedItems[0];

                  DateTime Startdates = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
                  if (Startdates.Year.ToString() == "0001" || Startdates.Year.ToString() == "1900")
                  {

                  }
                  else
                  {
                      if (objBIt != null)
                      {

                          if (objBIt.StartDate == OldStartDateVal)
                          {
                              objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
                              objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                              LoadBookingEditGrid(objBIt.BookingID);
                          }
                          if (objBIt.StartDate != OldStartDateVal)
                          {
                              if (objBIt.IsRefreshed == false || objBIt.IsRefreshed == null)
                              {
                                  MessageBoxResult mesbox = System.Windows.MessageBox.Show("Open rates management screen to refresh rates and check warnings?", "Message", System.Windows.MessageBoxButton.OKCancel);
                                  if (mesbox == MessageBoxResult.OK)
                                  {
                                      objBIt.OldStartDate = OldStartDateVal;
                                      objBIt.OldNetunit = objBIt.Netunit;


                                      objBIt.NewStartDate = Startdates;
                                      objBIt.OldNewDatediff = ((DateTime)objBIt.NewStartDate - objBIt.OldStartDate).Days;
                                     // objBIt.IsRefreshed = true;
                                      // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                      // TxtCheckindate.SelectedDate = Startdates;
                                      // dgBookingedited.ItemsSource = BookingItemsitinEditfull;
                                      //string username, ItineraryWindow ParentWindow, BookingItems objbkitems

                                      // RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit",this);
                                      RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsitinEditfull, objBIt, "BookingEdit", this);
                                      objref.ShowDialog();
                                      //TxtCheckindate.SelectedDate = objBIt.StartDate;
                                  }

                              }
                              else
                              {
                                  //objBIt.StartDate = OldStartDateVal;
                                  //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                  //BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = OldStartDateVal;
                                  //TxtCheckindate.SelectedDate = OldStartDateVal;
                                  //dgBookingedited.ItemsSource = BookingItemsitinEdit;
                              }

                              // LoadBookingEditGrid(objBIt.BookingID);
                          }

                      }



                  }
              }
              else
              {                
                  if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
                  {
                      objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
                      objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                      LoadBookingEditGrid(objBIt.BookingID);
                  }                                
              }
              */

        }

        private void TxtCheckinTime_ValueChanged(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtCheckinTime.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckinTime.Text)) && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
            {
                objBIt.StartTime = TxtCheckinTime.Text.ToString();
                objBIt.EndTime = TxtCheckinTime.Text.ToString();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }


        private void TxtCheckoutDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtCheckoutDate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckoutDate.Text)) && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
            {
                objBIt.Enddate = (DateTime)TxtCheckoutDate.SelectedDate;
                objBIt.NtsDays = Convert.ToDecimal(((DateTime)TxtCheckoutDate.SelectedDate).Subtract((DateTime)TxtCheckindate.SelectedDate).Days);
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void txtitemname_TextChanged(object sender, TextChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (txtitemname.Text.Length > 0 && (!string.IsNullOrEmpty(txtitemname.Text)) && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
            {
                objBIt.ItemDescription = txtitemname.Text.ToString();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void CmbRequestStatuslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (CmbRequestStatuslist.SelectedItem != null)
            {
                objBIt.SelectedItemRequstStatus = CmbRequestStatuslist.SelectedItem;
                objBIt.Status = (CmbRequestStatuslist.SelectedItem != null) ? ((SQLDataAccessLayer.Models.BkRequestStatus)(CmbRequestStatuslist.SelectedItem)).RequestStatusID.ToString() : Guid.Empty.ToString();


                //IwParWindow.CmbRequestStatus.SelectedValuePath = (CmbRequestStatuslist.SelectedItem != null) ? ((SQLDataAccessLayer.Models.RequestStatus)(CmbRequestStatuslist.SelectedItem)).RequestStatusID.ToString() : Guid.Empty.ToString();//CmbRequestStatuslist.SelectedItem;
                LoadBookingEditGrid(objBIt.BookingID);
            }


        }

        private void Txtstartdt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            //long BookingID = 0;

            if (e.RemovedItems.Count > 0)
            {
                DateTime OldStartDateVal = (DateTime)e.RemovedItems[0];

                DateTime Startdates = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
                if (Startdates.Year.ToString() == "0001" || Startdates.Year.ToString() == "1900")
                {

                }
                else
                {
                    if (objBIt != null)
                    {

                        if (objBIt.StartDate == OldStartDateVal)
                        {
                            BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;
                            TxtCheckindate.SelectedDate = Startdates;
                            dgBookingedited.ItemsSource = BookingItemsitinEdit;
                        }
                        if (objBIt.StartDate != OldStartDateVal)
                        {
                            if (objBIt.IsRefreshed == false || objBIt.IsRefreshed == null)
                            {
                                MessageBoxResult mesbox = System.Windows.MessageBox.Show("Open rates management screen to refresh rates and check warnings?", "Message", System.Windows.MessageBoxButton.OKCancel);
                                if (mesbox == MessageBoxResult.OK)
                                {
                                    objBIt.OldStartDate = OldStartDateVal;
                                    objBIt.OldNetunit = objBIt.Netunit;


                                    objBIt.NewStartDate = Startdates;
                                    objBIt.OldNewDatediff = ((DateTime)objBIt.NewStartDate - objBIt.OldStartDate).Days;
                                    objBIt.IsRefreshed = true;
                                    // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                    // TxtCheckindate.SelectedDate = Startdates;
                                    // dgBookingedited.ItemsSource = BookingItemsitinEditfull;
                                    //string username, ItineraryWindow ParentWindow, BookingItems objbkitems

                                    // RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit",this);
                                    RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsitinEditfull, objBIt, "BookingEdit", this);
                                    objref.ShowDialog();
                                    //TxtCheckindate.SelectedDate = objBIt.StartDate;
                                }

                            }
                            else
                            {
                                //objBIt.StartDate = OldStartDateVal;
                                //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                //BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = OldStartDateVal;
                                //TxtCheckindate.SelectedDate = OldStartDateVal;
                                //dgBookingedited.ItemsSource = BookingItemsitinEdit;
                            }

                            // LoadBookingEditGrid(objBIt.BookingID);
                        }

                    }



                }
            }
            else
            {
                DateTime Startdates = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
                if (Startdates.Year.ToString() == "0001" || Startdates.Year.ToString() == "1900")
                {

                }
                else
                {
                    if (objBIt != null)
                    {
                        BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;
                        TxtCheckindate.SelectedDate = Startdates;
                        dgBookingedited.ItemsSource = BookingItemsitinEdit;
                    }
                }
            }
        }

        private void RequestSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;// ((System.Windows.Controls.DataGridComboBoxColumn)((System.Windows.Controls.DataGridCell)sender).Column);

            BookingItems objBIt = (BookingItems)objbkitms;
            if (comboBox != null)
            {
                if (comboBox.SelectedItem != null)
                {
                    //var selectedItem = this.dgBookingedited.CurrentItem;

                    if (objBIt != null)
                    {
                        objBIt.SelectedItemRequstStatus = ((SQLDataAccessLayer.Models.BkRequestStatus)comboBox.SelectedItem).RequestStatusID;
                        objBIt.Status = ((SQLDataAccessLayer.Models.BkRequestStatus)comboBox.SelectedItem).RequestStatusID.ToString();
                        CmbRequestStatuslist.SelectedValue = ((SQLDataAccessLayer.Models.BkRequestStatus)comboBox.SelectedItem).RequestStatusID;
                        BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().Status = objBIt.Status;
                        dgBookingedited.ItemsSource = BookingItemsitinEdit;
                        // LoadBookingEditGrid(objBIt.BookingID);
                    }
                }
            }
        }
        public void totalcalculation()
        {
            string itincurformat = BookingItemsitinEdit.Where(x => x.BookingID == Convert.ToInt64(txtbookingID.Text)).FirstOrDefault().ItinCurDisplayFormat;
            lblNetTotal.Text = itincurformat + " " + (BookingItemsitinEdit.Sum(x => x.Nettotal)).ToString("0.00");
            lblGrossTotal.Text = itincurformat + " " + (BookingItemsitinEdit.Sum(x => x.Grossfinal)).ToString("0.00");

        }
        private void dgBookingedited_LayoutUpdated(object sender, EventArgs e)
        {
            Thickness t = lblTotal.Margin;
            t.Left = (dgBookingedited.Columns[0].ActualWidth + 7);
            lblTotal.Margin = t;
            lblTotal.Width = dgBookingedited.Columns[13].ActualWidth;

            lblNetTotal.Width = dgBookingedited.Columns[14].ActualWidth;
            lblGrossTotal.Width = dgBookingedited.Columns[15].ActualWidth;
        }


        private void TxtSupplierref_LostFocus(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtSupplierref.Text.Length > 0 && (!string.IsNullOrEmpty(TxtSupplierref.Text)))
            {
                objBIt.Ref = TxtSupplierref.Text.Trim();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        //public void ReterivePricingEditRate(string SupplierServiceId, string PricingOptionId)
        //{
        //    try
        //    {
        //        // SupplierPriceRateEdit sspredobj = dgPricingRateEdit.SelectedItem as SupplierPriceRateEdit;
        //        if (PriceEditDt != null)
        //        {
        //            ListofPricedit = null;
        //            PriceEditDt = null;
        //            ListofPricedit = objsupdal.PriceEditRateRetrive(Guid.Parse(SupplierServiceId),
        //                 Guid.Parse(PricingOptionId));
        //            if (ListofPricedit != null && ListofPricedit.Count > 0)
        //            {
        //                foreach (SupplierPriceRateEdit sup in ListofPricedit)
        //                {
        //                    if (PriceEditDt.Where(x => x.PricingOptionId == sup.PricingOptionId && x.PriceEditRateId == sup.PriceEditRateId).Count() == 0)
        //                    {
        //                        PriceEditDt.Add(sup);
        //                    }
        //                }
        //            }
        //            if (ListofPricedit == null || ListofPricedit.Count == 0)
        //            {
        //                // dgPricingRateEdit.ItemsSource = null;
        //            }
        //            // dgPricingRateEdit.ItemsSource = PriceEdit.Where(x => x.PricingOptionId == objPO.PricingOptionId).ToList();
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        errobj.WriteErrorLoginfo("SuppPricingOptionTemplate", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
        //    }
        //}

        private void TxtBooking_LostFocus(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtBooking.Text.Length > 0 && (!string.IsNullOrEmpty(TxtBooking.Text)))
            {
                objBIt.BookingNote = TxtBooking.Text.Trim();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void TxtVoucher_LostFocus(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtVoucher.Text.Length > 0 && (!string.IsNullOrEmpty(TxtVoucher.Text)))
            {
                objBIt.VoucherNote = TxtVoucher.Text.Trim();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void TxtPrivateMsg_LostFocus(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtPrivateMsg.Text.Length > 0 && (!string.IsNullOrEmpty(TxtPrivateMsg.Text)))
            {
                objBIt.Privatemsg = TxtPrivateMsg.Text.Trim();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void GetRecordNumberwise()
        {
            if (txtpagerecord.Text.Length > 0 && (!string.IsNullOrEmpty(txtpagerecord.Text)))
            {
                int totalrecord = Convert.ToInt32(lbltotalrecord.Content);
                if (Convert.ToInt32(txtpagerecord.Text) > totalrecord)
                {
                    MessageBox.Show("Given Record No's Exceed the total record...");
                    return;
                }
                else
                {
                    if (Convert.ToInt32(txtpagerecord.Text) == 0)
                    {
                        MessageBox.Show("Given Record No's more than zero...");
                        return;
                    }
                    else
                    {
                        int recint = 0;
                        recint = Convert.ToInt32(txtpagerecord.Text);
                        objbkitms = (BookingItems)BookingItemsitinEditfull.ToList()[recint - 1];
                        BookingItems objBIt = (BookingItems)objbkitms;
                        CmbBookingItemsList.SelectedIndex = recint - 1;
                        LoadBookingEditGrid(objBIt.BookingID);
                    }
                }

            }
        }
        private void txtpagerecord_LostFocus(object sender, RoutedEventArgs e)
        {
            GetRecordNumberwise();
        }

        private void txtpagerecord_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            e.Handled = true;
            GetRecordNumberwise();
        }

        private void dgBookingedited_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgBookingedited.BeginEdit();
        }

        private void TxtCheckoutTime_ValueChanged(object sender, RoutedEventArgs e)
        {
            BookingItems objBIt = (BookingItems)objbkitms;
            if (TxtCheckoutTime.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckoutTime.Text)) && ((System.Windows.UIElement)sender).IsKeyboardFocusWithin == true)
            {
                objBIt.EndTime = TxtCheckoutTime.Text.ToString();
                LoadBookingEditGrid(objBIt.BookingID);
            }
        }

        private void btnAddfollowup_Click(object sender, RoutedEventArgs e)
        {
            //this.FollowupViewModel = new FollowupViewModel();
            //this.FollowupViewModel.Bookingid = txtbookingID.Text;
            this.FollowupViewModel.Bookingid = this.objbkitms.BookingID;
            this.FollowupViewModel.Itineraryid = this.objbkitms.ItineraryID.ToString();
            this.FollowupViewModel.Taskid = Guid.NewGuid().ToString();
            this.FollowupViewModel.TaskName = txtbookingname.Text;// "New Service" + " (" + (SupplierSM.Count + 1) + ")";
            this.FollowupViewModel.Notes = "";
            this.FollowupViewModel.DateDue = null;
            this.FollowupViewModel.AssignedtoSelectedItem = ListUserdet.Where(x => x.Userid.ToString() == IwParWindow.CmbAgentAssignedTo.SelectedValue.ToString()).FirstOrDefault();//.Userid.ToString();


            this.FollowupViewModel.Datecompleted = null;
            this.FollowupViewModel.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
            this.FollowupViewModel.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;
            this.FollowupViewModel.DeletedBy = Guid.Empty.ToString();
            this.Followupcommd.AddCommand.Execute();
            //Folloupcntrl.DataContext = this.FollowupViewModel.Folluptask.Where(x=>x.Bookingid==objbkitms.BookingID).ToList();
        }
        private void btnSavefollowup_Click(object sender, EventArgs e)
        {
            this.Followupcommd.SaveCommand.Execute();
        }

        private void OverridePaymentTerm_TextChanged(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(OverridePaymentTerm.Text, out int daysToSubtract))
            {
                if (daysToSubtract < 0 || daysToSubtract > 31)
                {
                    MessageBox.Show("please enter date between 1 to 31");
                    OverridePaymentTerm.Text = String.Empty;
                }
                else if (daysToSubtract > Convert.ToInt32(this.BookingItemsitinEdit.First().StartDate.Date.Day))
                {
                    MessageBox.Show("Enter Date is greater than orginal date so it will change the month of due date");
                    OverridePaymentTerm.Text = String.Empty;
                }

                BookingItems objBIt = (BookingItems)objbkitms;
                objBIt.PaymentDueDate = this.BookingItemsitinEdit.First().StartDate.AddDays(-daysToSubtract);
                objBIt.SupplierPaymentTermsOverrideindays = daysToSubtract;
                decimal days = objsupdal.SupplierRetrive("FIR", Guid.Parse(this.BookingItemsitinEdit.First().SupplierID)).First().SupplierPaymentTermsindays;
                objBIt.SupplierPaymentTermsindays = Convert.ToInt32(days);
                LoadBookingEditGrid(objBIt.BookingID);
                //this.BookingItemsitinEdit.First().PaymentDueDate = this.BookingItemsitinEdit.First().StartDate.AddDays(-daysToSubtract);
                //this.BookingItemsitinEdit.First().SupplierPaymentTermsindays = daysToSubtract;
                //decimal days = objsupdal.SupplierRetrive("FIR", Guid.Parse(this.BookingItemsitinEdit.First().SupplierID)).First().SupplierPaymentTermsindays;
                //this.BookingItemsitinEdit.First().SupplierPaymentTermsOverrideindays = Convert.ToInt32(days);
            }
            else
            {
                MessageBox.Show("please enter valid date");
                OverridePaymentTerm.Text = String.Empty;
            }
        }

        private void DepositeAmountOverrideValue_TextChanged(object sender, RoutedEventArgs e)
        {
            decimal depositAmount;
            if (decimal.TryParse(DepositeAmountOverrideValue.Text, out depositAmount))
            {
                //this.BookingItemsitinEdit.First().SupplierPaymentOverrideDepositAmount = depositAmount;
                //this.BookingItemsitinEdit.First().SupplierPaymentDepositAmount = depositAmount;
                //decimal amount = objsupdal.SupplierRetrive("FIR", Guid.Parse(this.BookingItemsitinEdit.First().SupplierID)).First().SupplierPaymentDepositAmount;
                //this.BookingItemsitinEdit.First().SupplierPaymentOverrideDepositAmount = Convert.ToInt32(amount);

                BookingItems objBIt = (BookingItems)objbkitms;
                objBIt.SupplierPaymentOverrideDepositAmount = depositAmount;                
                decimal amount = objsupdal.SupplierRetrive("FIR", Guid.Parse(this.BookingItemsitinEdit.First().SupplierID)).First().SupplierPaymentDepositAmount;
                objBIt.SupplierPaymentDepositAmount = Convert.ToDecimal(amount);
                LoadBookingEditGrid(objBIt.BookingID);

            }
            else
            {
                MessageBox.Show("please enter valid amount");
                DepositeAmountOverrideValue.Text = String.Empty;
            }
        }

        private void OverridePaymentTerm_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "0")
            {
                textBox.Text = "";
            }

        }

        private void DepositeAmountOverrideValue_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "0")
            {
                textBox.Text = "";
            }

        }

        //private void btnDeletefollowup_Click(object sender, RoutedEventArgs e)
        //{
        //    this.FollowupViewModel.DeleteFollowuptaskCommand.Execute();
        //}
        /*
private void TxtCheckindate_LostFocus(object sender, RoutedEventArgs e)
{
   BookingItems objBIt = (BookingItems)objbkitms;
   //if (e.RemovedItems.Count > 0)
   //{
       DateTime oldDateval = (DateTime)((System.Windows.Controls.DatePicker)e.Source).SelectedDate;
       if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
       {
           if (objBIt.StartDate == (DateTime)TxtCheckindate.SelectedDate)
           {
               objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
               objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
               LoadBookingEditGrid(objBIt.BookingID);
           }
           //if (objBIt.StartDate != (DateTime)TxtCheckindate.SelectedDate)
           //{
           //    objBIt.OldStartDate = objBIt.StartDate;
           //    objBIt.OldNetunit = objBIt.Netunit;
           //    objBIt.NewNetunit = objBIt.Netunit;

           //    objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
           //    objBIt.OldNewDatediff = ((DateTime)objBIt.StartDate - objBIt.OldStartDate).Days;
           //    objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
           //    RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates,objBIt, "BookingEdit");
           //    objref.ShowDialog();
           //    LoadBookingEditGrid(objBIt.BookingID);
           //}
           if (objBIt.StartDate != (DateTime)TxtCheckindate.SelectedDate)
           {

               MessageBoxResult mesbox = System.Windows.MessageBox.Show("Open rates management screen to refresh rates and check warnings?", "Message", System.Windows.MessageBoxButton.OKCancel);
               if (mesbox == MessageBoxResult.OK)
               {
                   objBIt.OldStartDate = objBIt.StartDate;
                   objBIt.OldNetunit = objBIt.Netunit;

                   objBIt.NewStartDate = (DateTime)TxtCheckindate.SelectedDate;
                   objBIt.OldNewDatediff = ((DateTime)objBIt.NewStartDate - objBIt.OldStartDate).Days;
                   // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));

                   // dgBookingedited.ItemsSource = BookingItemsitinEditfull;
                   //string username, ItineraryWindow ParentWindow, BookingItems objbkitems
                   RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit");
                   objref.ShowDialog();
               }
               else
               {
                   //objBIt.StartDate = OldStartDateVal;
                   //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                   //BookingItemsitinEdit.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = OldStartDateVal;
                   //TxtCheckindate.SelectedDate = OldStartDateVal;
                   //dgBookingedited.ItemsSource = BookingItemsitinEdit;
               }

               // LoadBookingEditGrid(objBIt.BookingID);
           }
       }
   //}
   //else
   //{
   //    if (TxtCheckindate.Text.Length > 0 && (!string.IsNullOrEmpty(TxtCheckindate.Text)))
   //    {
   //        objBIt.StartDate = (DateTime)TxtCheckindate.SelectedDate;
   //        objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
   //        LoadBookingEditGrid(objBIt.BookingID);
   //    }
   //}

}

*/
    }
}
