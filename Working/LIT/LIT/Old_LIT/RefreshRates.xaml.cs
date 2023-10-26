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
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using LIT.Old_LIT;
using LITModels;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for RefreshRates.xaml
    /// </summary>
    public partial class RefreshRates : Window
    {
        string loginusernameval = string.Empty;
        string loginuserid = string.Empty;
        Errorlog errobj = new Errorlog();
        BookingItems objbkitms;
        CommonAndCalcuation CommonValues = new CommonAndCalcuation();
        DBConnectionEF DBconnEF = new DBConnectionEF();

        //Refresh Rate _BookingItemsRefreshrates
        private ObservableCollection<BookingItems> _BkgItemsRefreshrates;
        public ObservableCollection<BookingItems> BkingItemsRefreshrates
        {
            get { return _BkgItemsRefreshrates ?? (_BkgItemsRefreshrates = new ObservableCollection<BookingItems>()); }
            set
            {
                _BkgItemsRefreshrates = value;
            }
        }
        private ObservableCollection<BookingItems> _BkgItemsRefreshratesCancel;
        public ObservableCollection<BookingItems> BkgItemsRefreshratesCancel
        {
            get { return _BkgItemsRefreshratesCancel ?? (_BkgItemsRefreshratesCancel = new ObservableCollection<BookingItems>()); }
            set
            {
                _BkgItemsRefreshratesCancel = value;
            }
        }

        private ItineraryWindow IwParWindowre;
        Bookingedit bkeditobj;
        string sourcefrompage= string.Empty;
        public RefreshRates()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public RefreshRates(string loginusername, ItineraryWindow IwParWindow, ObservableCollection<BookingItems> bkgitms,BookingItems bookingitems, string sourcefrom, Bookingedit bkedit =null)
        {
            InitializeComponent();
            this.DataContext = this;
            dgBookingRefreshedited.CellEditEnding += dgBookingRefreshedited_TargetUpdatedevt;
            //dgBookingRefreshedited.BeginningEdit += (s, ss) => ss.EditingEventArgs() = true;
            if (bkgitms!=null && bkgitms.Count>0)
            {
                BkingItemsRefreshrates = null;
                BkingItemsRefreshrates = bkgitms;
                BkgItemsRefreshratesCancel = bkgitms;
                objbkitms = bookingitems;
                loginusernameval = loginusername;
                sourcefrompage = sourcefrom;
                if (IwParWindow != null)
                {
                    IwParWindowre = IwParWindow;
                }
                if(bkedit!=null)
                {
                    bkeditobj = bkedit;
                }                
                TxtStartDate.SelectedDate = IwParWindowre.TxtItinerarystartdt.SelectedDate;
                TxtStartDate2.SelectedDate = IwParWindowre.TxtItinerarystartdt.SelectedDate;
                TxtEndDate.SelectedDate = IwParWindowre.TxtItineraryEndDt.SelectedDate;
                TxtEndDate2.SelectedDate = IwParWindowre.TxtItineraryEndDt.SelectedDate;
                chbSelectedsameno.IsChecked= true;

                DgBookDatabind();
            }
        }

        private void DgBookDatabind()
        {
            if (BkingItemsRefreshrates.Count > 0)
            {
                int days = 0;
                foreach (BookingItems bkit in BkingItemsRefreshrates)
                {
                    if (BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault() != null)
                    {
                        if (bkit.StartDate == objbkitms.StartDate && objbkitms.BookingID == bkit.BookingID)
                        { 
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldStartDate = objbkitms.OldStartDate;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                        }
                        else if (bkit.StartDate == objbkitms.StartDate && objbkitms.BookingID != bkit.BookingID)
                        {
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldStartDate = bkit.StartDate;
                            days = (objbkitms.OldNewDatediff != null) ? Convert.ToInt32(objbkitms.OldNewDatediff) : 0;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                        }
                        else if (bkit.StartDate < objbkitms.StartDate)
                        {
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldStartDate = bkit.StartDate;
                            if (bkit.StartDate < objbkitms.OldStartDate)
                            {
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate;

                            }
                            if (bkit.StartDate > objbkitms.OldStartDate)
                            {
                                days = (objbkitms.OldNewDatediff != null) ? Convert.ToInt32(objbkitms.OldNewDatediff) : 0;
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                            }

                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewNetunit = bkit.Netunit;
                            if (bkit.StartDate == objbkitms.OldStartDate)
                            {
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate;

                            }


                        }
                        else if (bkit.StartDate > objbkitms.StartDate)
                        {
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldStartDate = bkit.StartDate;
                            days = (objbkitms.OldNewDatediff != null) ? Convert.ToInt32(objbkitms.OldNewDatediff) : 0;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);

                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewNetunit = bkit.Netunit;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                            
                        }
                        //else if (bkit.StartDate <= objbkitms.StartDate)
                        //{
                        //    int days = (objbkitms.OldNewDatediff != null) ? Convert.ToInt32(objbkitms.OldNewDatediff) : 0;
                        //    BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);
                        //}


                       // BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().OldNetunit = bkit.Netunit;
                       // BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewNetunit = bkit.NewNetunit;
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Warningmsgurl = null;
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().ServiceWarning = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SupplierWarning = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Resultmsg = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Resultmsgurl = null;
                    }
                }
               // var lst = BkingItemsRefreshrates.Where(x => x.StartDate >= objbkitms.StartDate).ToList();

                
                dgBookingRefreshedited.ItemsSource = BkingItemsRefreshrates.OrderBy(x => x.OldStartDate).ToList();
            }

        }


        private void DgBookDatabindNewDate(int diffdaysval,DateTime StartDate, DateTime OldStartDate, long BookingID)// BookingItems objbkitms)
        {
            if (BkingItemsRefreshrates.Count > 0)
            {
                int days = diffdaysval;
                foreach (BookingItems bkit in BkingItemsRefreshrates)
                {
                    if (BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault() != null)
                    {
                        if (BookingID == bkit.BookingID)
                        {
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = StartDate;
                            BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;                            
                        }
                        else if (BookingID != bkit.BookingID)
                        {
                            if (bkit.StartDate == StartDate)
                            {
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = StartDate;
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                            }
                            if (bkit.StartDate > StartDate)
                            {
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);
                                BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                            }
                            //if (bkit.StartDate < StartDate)
                            //{
                            //    BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().NewStartDate = bkit.StartDate.AddDays(days);
                            //    BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SelectedIdforRefresh = true;
                            //}
                        }
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Warningmsgurl = null;
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().ServiceWarning = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().SupplierWarning = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Resultmsg = "";
                        BkingItemsRefreshrates.Where(x => x.BookingID == bkit.BookingID).FirstOrDefault().Resultmsgurl = null;
                    }
                }

                dgBookingRefreshedited.ItemsSource = BkingItemsRefreshrates.OrderBy(x => x.OldStartDate).ToList();
            }

        }
        private void btnRefreshRates_Click(object sender, RoutedEventArgs e)
        {
            foreach (BookingItems objbkt in BkingItemsRefreshrates)
            {
                if (objbkt.SelectedIdforRefresh == true)
                {
                    CommonValues.NewRatesforSelectedDates(objbkt);

                    Tuple<string, string> warreslt = CommonValues.ReteriveWarnings(objbkt.ServiceID, objbkt.NewStartDate, objbkt.NewStartDate.AddDays(Convert.ToInt32(objbkt.NtsDays)));
                    if (warreslt != null)
                    {
                        objbkt.SupplierWarning = warreslt.Item2;
                        objbkt.ServiceWarning = warreslt.Item1;
                        if ((objbkt.ServiceWarning != null || objbkt.SupplierWarning != null) && (objbkt.SupplierWarning.Length > 0 || objbkt.ServiceWarning.Length > 0))
                        {
                            Image FailureImage = new Image();
                            BitmapImage FailureImagebitmapImage = new BitmapImage(new Uri(String.Format("/LIT.Core;component/Media/Images/{0}.png", "warning"), UriKind.Relative));
                            FailureImage.Source = FailureImagebitmapImage;
                            objbkt.Warningmsgurl = FailureImagebitmapImage;

                            // Button btn=(Button)dgBookingRefreshedited.Columns[10].GetCellContent( btnWarningmsg.visi
                        }
                        else
                        {
                            objbkt.Warningmsgurl = null;
                        }
                    }
                }
            }
            dgBookingRefreshedited.ItemsSource = BkingItemsRefreshrates.OrderBy(x => x.OldStartDate).ToList();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
           
            if (sourcefrompage != string.Empty)
            {
                int days = 0;
                if (sourcefrompage.ToLower() == "bookingedit")
                {
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.StartDate = x.NewStartDate;
                        days = (x.NtsDays != null) ? Convert.ToInt32(x.NtsDays) : 0;
                        string servicetypename = DBconnEF.GetServicetypename(x.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
                            {
                                x.Enddate = ((DateTime)x.StartDate).AddDays(Convert.ToInt32(x.NtsDays));
                            }
                            else
                            {
                                x.Enddate = ((DateTime)x.StartDate);
                            }
                        }
                       // x.Enddate = x.StartDate.AddDays(days);
                        x.IsRefreshed = true;
                        x.Day = x.StartDate.DayOfWeek.ToString();
                        x.Netunit = x.NewNetunit.ToString();
                        x.NewNetUnitNotinSupptbl = (x.NewNetUnitNotinSupptbl == true) ? true : false;
                        if (x.RefreshRateEditedflag == true)
                        {
                            CommonValues.Grossnetcalculation(x);
                        }
                    });
                     IwParWindowre.BookingItemsitin = BkingItemsRefreshrates;

                    //BookingEdit bkedit = new BookingEdit(loginusernameval, IwParWindowre, objbkitms);
                    bkeditobj.BookingItemsitinEditfull = BkingItemsRefreshrates;
                    bkeditobj.LoadBookingEditGrid(objbkitms.BookingID);
                    // IwParWindowre.ReteriveBookingItems();
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = false;
                        x.SelectedIdforRefresh = false;
                    });
                    this.Close();
                    // bkedit.ShowDialog();

                }
               /* if (sourcefrompage.ToLower() == "bookingitinerary")
                {
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.StartDate = x.NewStartDate;
                        days = (x.NtsDays != null) ? Convert.ToInt32(x.NtsDays) : 0;
                        x.Enddate = x.StartDate.AddDays(days);
                        x.IsRefreshed = true;
                        x.Day = x.StartDate.DayOfWeek.ToString();
                        x.Netunit = x.NewNetunit.ToString();
                        x.NewNetUnitNotinSupptbl = (x.NewNetUnitNotinSupptbl == true) ? true : false;
                        if (x.RefreshRateEditedflag == true)
                        {
                            CommonValues.Grossnetcalculation(x);
                        }
                    });

                    IwParWindowre.BookingItemsitin = BkingItemsRefreshrates;
                   // IwParWindowre.ReteriveBookingItems();
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = false;
                        x.SelectedIdforRefresh = false;
                    });
                    this.Close();
                }*/
            }

        }

        private void btnCancelRefreshRates_Click(object sender, RoutedEventArgs e)
        {
            if (sourcefrompage != string.Empty)
            {
                if (sourcefrompage.ToLower() == "bookingedit")
                {
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = true;
                        x.StartDate = x.OldStartDate;
                        x.Netunit = x.OldNetunit;
                        x.SelectedIdforRefresh = false;
                    });

                   // objbkitms.IsRefreshed=true;
                    bkeditobj.BookingItemsitinEditfull = BkingItemsRefreshrates;
                    bkeditobj.LoadBookingEditGrid(objbkitms.BookingID);
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = false;
                        x.SelectedIdforRefresh=false;
                    });
                    this.Close();



                }
                if (sourcefrompage.ToLower() == "bookingitinerary")
                {

                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = true;
                        x.StartDate = x.OldStartDate;
                        x.SelectedIdforRefresh = false;
                    });

                    IwParWindowre.BookingItemsitin = BkingItemsRefreshrates;
                   // IwParWindowre.ReteriveBookingItems();
                    BkingItemsRefreshrates.ToList().ForEach(x =>
                    {
                        x.IsRefreshed = false;
                        x.SelectedIdforRefresh = false;
                    });
                    
                    this.Close();

                }
            }
           // this.Close();

        }


       
        private void Imgicon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext)!=null)
            {
                if(!string.IsNullOrEmpty(((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).ServiceWarning))
                {
                    string sermsg = ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).ServiceWarning.ToString();
                    MessageBox.Show(sermsg, "Service Level Warning");
                }
                if (!string.IsNullOrEmpty(((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).SupplierWarning))
                {
                    string supmsg = ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).SupplierWarning.ToString();
                    MessageBox.Show(supmsg,"Supplier Level Warning");
                }

            }
            
        }

        private void TextBlock_ToolTipOpening(object sender, ToolTipEventArgs e)
        {
            if (((System.Windows.FrameworkElement)sender) != null)
            {
                ((System.Windows.FrameworkElement)sender).ToolTip = "No rates found for this date";
            }
           // MessageBox.Show("Test");
        }

       /* private void TxtNewstartdt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                if (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext) != null)
                {
                    if (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).IsNewDateChanged == false 
                        || ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).IsNewDateChanged == null)
                    {
                        MessageBoxResult ret = MessageBox.Show("Would you like to move the start date of the subsequent bookings accordingly?", "Message", MessageBoxButton.YesNo);
                        if (ret == MessageBoxResult.Yes)
                        {
                            if (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext) != null)
                            {
                                long bookingid = ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkElement)sender).DataContext).BookingID;
                                int daysdiff = 0;
                                if (BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault() != null)
                                {
                                    //BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault().IsNewDateChanged = true;
                                    //BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault().StartDate = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
                                    objbkitms = BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault();
                                    daysdiff = (((DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate) - (objbkitms.StartDate)).Days;
                                    objbkitms.StartDate = (DateTime)((System.Windows.Controls.DatePicker)((object)sender)).SelectedDate;
                                    DgBookDatabindNewDate(daysdiff);
                                }
                            }
                        }
                    }
                }
            }
        }
        */

        private void dgBookingRefreshedited_BeginningEdit(object sender, System.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
          
        }
        private void dgBookingRefreshedited_TargetUpdatedevt(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            if ((e.Row.DataContext) != null)
            {
                ContentPresenter contentPresenter = e.EditingElement as ContentPresenter;
                DataTemplate editingTemplate = contentPresenter.ContentTemplate;
                System.Windows.Controls.DatePicker dp = editingTemplate.FindName("TxtNewstartdt", contentPresenter)
                                            as System.Windows.Controls.DatePicker;
                if (dp != null && ((SQLDataAccessLayer.Models.BookingItems)dp.DataContext).NewStartDate != (DateTime)(dp.SelectedDate)) 
                { 

                    MessageBoxResult ret = MessageBox.Show("Would you like to move the start date of the subsequent bookings accordingly?", "Message", MessageBoxButton.YesNo);
                    if (ret == MessageBoxResult.Yes)
                    {
                        if (((SQLDataAccessLayer.Models.BookingItems)dp.DataContext) != null)
                        {
                            long bookingid = ((SQLDataAccessLayer.Models.BookingItems)dp.DataContext).BookingID;
                            int daysdiff = 0;
                            if (BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault() != null)
                            {
                                
                                if (dp != null)
                                {
                                    objbkitms = BkingItemsRefreshrates.Where(x => x.BookingID == bookingid).FirstOrDefault();
                                    daysdiff = ((DateTime)(dp.SelectedDate) - (objbkitms.StartDate)).Days;
                                    // objbkitms.StartDate = (DateTime)dp.SelectedDate;
                                    DgBookDatabindNewDate(daysdiff, (DateTime)dp.SelectedDate, objbkitms.OldStartDate, objbkitms.BookingID);// ((SQLDataAccessLayer.Models.BookingItems)dp.DataContext));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            string serviceid = (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext) != null) ? ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext).ServiceID : string.Empty;
            string SupplierID = (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext) != null) ? ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext).SupplierID : string.Empty;
            DateTime NewDateTime = (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext) != null) ? ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext).NewStartDate : DateTime.MinValue;
            long bkgid = (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext) != null) ? ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext).BookingID : 0;
            decimal nightdays = (((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext) != null) ? ((SQLDataAccessLayer.Models.BookingItems)((System.Windows.FrameworkContentElement)sender).DataContext).NtsDays : 0;
            
            //  SupplierServiceUC se = new SupplierServiceUC(loginusernameval, serviceid, bkgid, NewDateTime, this);
            SupplierServiceUC se = new SupplierServiceUC(loginusernameval, SupplierID, serviceid, bkgid, NewDateTime, nightdays, this);
            se.ShowDialog();
        }

        private void dgBookingRefreshedited_SelectedCellsChanged(object sender, System.Windows.Controls.SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            string header = (string)currentCell.Column.Header;
            dgBookingRefreshedited.BeginEdit();
        }
    }
}
