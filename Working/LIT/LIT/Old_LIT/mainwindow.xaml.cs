using Accessibility;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
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
using System.Xaml;
using System.Xml.Linq;
using Key = System.Windows.Input.Key;
using static LIT.Old_LIT.TreeViewCreation;
using LIT.Modules.TabControl.Views;
using LIT.Core.Mvvm;
using ViewModelBase = LIT.Old_LIT.TreeViewCreation.ViewModelBase;
using Org.BouncyCastle.Asn1.Ocsp;
using Prism.Ioc;
using LIT.Views;
using System.Runtime.CompilerServices;
using LIT.ViewModels;
using LIT.Core.Controls;
//using static LIT.NodeViewModel;

namespace LIT.Old_LIT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        string loginusername = string.Empty;
        string loginuserid = string.Empty;
        string loginuserrole = string.Empty;
        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();
        SQLDataAccessLayer.DAL.SupplierDAL objsupdal = new SQLDataAccessLayer.DAL.SupplierDAL();
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        // SQLDataAccessLayer.DAL.Errorlog erobj = new SQLDataAccessLayer.DAL.Errorlog();
        DataSet dsItineraryRetr = new DataSet();

        List<string> listitineraryFolder = new List<string>();
        List<string> listitineraryname = new List<string>();
        List<TreeViewModel> listitinerarytree = new List<TreeViewModel>();
        public TreeViewModel ItineraryTreeViewModelTr { get; } = new TreeViewModel();
        public NodeViewModel itineraryNodeViewModelTr { get; } = new NodeViewModel();

        Errorlog errobj = new Errorlog();
        ItineraryWindow Itinerwin = null;
        Supplier supwindow = null;

        private ObservableCollection<Tabitemdetails> _Tabitemdet;
        public ObservableCollection<Tabitemdetails> Tabitemdet
        {
            get { return _Tabitemdet; }
            set
            {
                _Tabitemdet = value;
            }
        }
        Tabitemdetails objtabit;


        TreeviewAccordion tracc = new TreeviewAccordion();
        // public IEnumerable<Tabitemdetails> IEnTabitem = objtabit;

        //  public ObservableCollection<TreeNodeView> Items { get; set; }        
        // TreeNodeView objnode= new TreeNodeView();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            //this.Navigationframe.NavigationService.Navigate(new System.Uri("ItineraryWindow.xaml", UriKind.Relative), StudentName, false);
            //this.Navigationframe.NavigationService.LoadCompleted +=
            //  new System.Windows.Navigation.LoadCompletedEventHandler(NavigationService_LoadCompleted);
        }
        //public event RoutedEventHandler ParentValue;
        public MainWindow(string UserName)
        {
            InitializeComponent();
            this.DataContext = this;
            //Initializing Child class in Parent Constructor         
            Itinerwin = new ItineraryWindow();
            supwindow=new Supplier();
            //Registering child class event           
            // Itinerwin.NotifyParentEvent += new NotifyParentDelegate(_child_NotifyParentEvent);




            Tabitemdet = new ObservableCollection<Tabitemdetails>();
            loginusername = UserName;
            listitinerarytree.Add(ItineraryTreeViewModelTr);
            // TreeviewAccordion objtr=new TreeviewAccordion();
            // stpan.DataContext = objtr;
            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);
            loginuserrole = loadDropDownListValues.CurrentUserRole(loginusername);
            if (!string.IsNullOrEmpty(loginuserrole))
            {
                if (loginuserrole.ToLower() == "travel advisor")
                {
                    //accsupplier.Visibility = Visibility.Hidden;
                    trviewsupplier.Visibility = Visibility.Collapsed;
                    MnuNewSupplier.Visibility = Visibility.Collapsed;
                    SupplierExpander.Visibility=Visibility.Collapsed;
                }
            }
            trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
            trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;
            trviewcontact.ItemsSource = tracc.ContactTreeViewModel.SupplierItems;
            // TreeViewModel treeView = new TreeViewModel();
            //trview.ItemsSource = treeView.Items;
            // DataContext = objtr.ItineraryTreeViewModelTr.Items; 

            trview.SelectedItemChanged += trview_SelectedItemChanged;
            // Bind your ListView to the Notifications collectionCommonServiceLocator
            //NotificationListView.ItemsSource = _notificationService.Notifications;
            // Bind your ListView to the Notifications collection
            //notifications = new ObservableCollection<Notification>();
            //NotificationListView.ItemsSource = notifications;

            _notificationService = (NotificationService)ContainerLocator.Container.Resolve(typeof(NotificationService));
            InitializeEventHandlers();
            //Testing
            //_notificationService.Notifications.Add(new NotificationItem { Message = "Testing", Timestamp = DateTime.Now.ToString() });
            //_notificationService.Notifications.Add(new NotificationItem { Message = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. ", Timestamp = DateTime.Now.ToString() });
            //_notificationService.Notifications.Add(new NotificationItem { Message = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters.", Timestamp = DateTime.Now.ToString() });
            //Testing End
            NotificationList.ItemsSource = _notificationService.Notifications;

            Closing += OnMainWindowClosing;
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            _notificationService.ClosePopupsOnApplicationExit();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChangedNotify method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private NotificationService _notificationService;

        public NotificationService NotificationService
        {
            get => _notificationService;
            set
            {
                _notificationService = value;
                // Call OnPropertyChangedNotify whenever the property is updated
                OnPropertyChanged();
            }
        }
        private void InitializeEventHandlers()
        {
            BellButton.Click += ShowNotifications_Click;
            //NotificationListView.AddHandler(Button.ClickEvent, new RoutedEventHandler(ClearNotification_Click));
        }

        private bool isPopupOpen = false; // Initialize as closed

        private void ShowNotifications_Click(object sender, RoutedEventArgs e)
        {
            if (isPopupOpen)
            {
                NotificationsPopup.IsOpen = false; // Close the popup if it's open
            }
            else
            {
                NotificationsPopup.IsOpen = true; // Open the popup if it's closed
            }

            isPopupOpen = !isPopupOpen; // Toggle the flag
        }

        private void ClearAllNotifications_Click(object sender, RoutedEventArgs e)
        {
            NotificationsPopup.IsOpen = false;
            _notificationService.ClearAllNotifications();
        }

        private void DeleteNotification_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.CommandParameter is NotificationItem notificationItem)
            {
                _notificationService.Notifications.Remove(notificationItem);
            }
        }

        //private void ClearNotification_Click(object sender, RoutedEventArgs e)
        //{
        //    // Logic to clear individual notifications
        //    if (sender is Button button && button.DataContext is string notification)
        //    {
        //        _notificationService.Notifications.Remove(notification);
        //    }
        //}

        //private void ClearAllNotifications_Click(object sender, RoutedEventArgs e)
        //{
        //    // Logic to clear all notifications
        //    _notificationService.ClearAllNotifications();
        //}

        //void NavigationService_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    ((ItineraryWindow)e.Content).saveupdateItinerary += new EventHandler(itinerary_save);
        //}

        private List<Expander> expanders = new List<Expander>();

        private void Expander_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expander)
            {
                expanders.Add(expander);
                expander.Expanded += Expander_Expanded;
            }
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender is Expander expandedExpander)
            {
                foreach (Expander expander in expanders)
                {
                    if (expander != expandedExpander)
                    {
                        expander.IsExpanded = false;
                    }
                }
            }
        }

        public void MnuNewItinerary_Click(object sender, RoutedEventArgs e)
        {
            AddItineraryTabDetails("newitinerary");
            //try
            //{
            //ItineraryWindow window = new ItineraryWindow(loginusername, this, "");
            //// window.Show();
            //window.BtnSave.Visibility = Visibility.Visible;
            //window.BtnSaveandclose.Visibility = Visibility.Visible;

            //Navigationframe.Navigate(new System.Uri("ItineraryWindow.xaml",
            // UriKind.RelativeOrAbsolute));

            //Navigationframe.Navigate(window);
            //}
            //catch (Exception ex)
            //{
            //    erobj.writeLog(ex.Message.ToString());
            //}
        }

        //public void BtnSave_Click(object sender, EventArgs e)
        //{
        //    ItineraryWindow wi = new ItineraryWindow(loginusername);
        //    wi.xyz += new EventHandler(callitinerary);
        //    //if (wi.xyz != null)
        //    //{
        //    //    wi.xyz(this);
        //    //    // MessageBox.Show(wi.TxtItineraryName.Text);
        //    //}
        //}
        //public void callitinerary(object sender, EventArgs e)
        //{
        //    MessageBox.Show("test");
        //}

        //private void customControl_Click(object sender, RoutedEventArgs e)
        //{
        //    ItineraryWindow wi;

        //        wi= new ItineraryWindow(loginusername);
        //    wi.saveupdateItinerary();
        //    MessageBox.Show("You have just click your custom control");
        //}


        //public class Tabdetails
        //{
        //    public string Title { get; set; }   
        //    public FrameworkElement Content { get; set; }    
        //}

        //public ObservableCollection<Tabdetails>  TabItemslist { get; set; }
        public bool ItinearayReterive(string ItinearyIdval)
        {
            if (ItinearyIdval != "")
            {
                try
                {
                    dsItineraryRetr = null;
                    dsItineraryRetr = objitdal.ItineraryRetrive("FR", Guid.Parse(ItinearyIdval));
                    if (dsItineraryRetr != null)
                    {
                        if (dsItineraryRetr.Tables.Count > 0)
                        {
                            return true;
                            // ItineraryWindow window = new ItineraryWindow(loginusername, this, ItinearyIdval);
                            //window.BtnSave.Visibility = Visibility.Visible;
                            // window.BtnSaveandclose.Visibility = Visibility.Visible;
                            // Navigationframe.Navigate(window);
                        }
                    }
                }
                catch (Exception ex)
                {

                    errobj.WriteErrorLoginfo("MainWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                    return false;
                }

            }
            return false;
        }

        public bool SupplierReterive(string SupplierIdval)
        {
            if (SupplierIdval != "")
            {
                try
                {
                    List<SupplierModels> listsupplier = new List<SupplierModels>();

                    listsupplier = objsupdal.SupplierRetriveID("FR", Guid.Parse(SupplierIdval));
                    if (listsupplier != null && listsupplier.Count > 0)
                    {
                        if (listsupplier.Count > 0)
                        {
                            return true;
                            // Supplier window = new Supplier(loginusername, this, SupplierIdval);
                            // window.BtnSupplierSave.Visibility = Visibility.Visible;
                            //  window.BtnSupplierSaveandclose.Visibility = Visibility.Visible;
                            // Navigationframe.Navigate(window);
                        }

                    }

                }
                catch (Exception ex)
                {
                    errobj.WriteErrorLoginfo("MainWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                    return false;
                }

            }
            return false;
        }


        /* public void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
         {
             if (trview.SelectedValue != null)
             {
                 string SelectedTreeNodetID = ((NodeViewModel)trview.SelectedValue).Id;
                 if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                 {
                     AddItineraryTabDetails(SelectedTreeNodetID);
                 }
             }

         }
        */
        public void AddItineraryTabDetails(string SelectedTreeNodetID, string recfrom=null)
        {
            objtabit = new Tabitemdetails();
            if (SelectedTreeNodetID.ToLower() == "newitinerary")
            {
                objtabit.title = "New Itinerary";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "New Itinerary";
                objtabit.tabfrom = "Itinerary_";
                objtabit.FilePath = "Itineraries\\New Itinerary";

            }
            else
            {
                if (recfrom == null)
                {
                    objtabit.title = ((LIT.Old_LIT.NodeViewModel)trview.SelectedValue).Name;

                    objtabit.tabid = (((LIT.Old_LIT.NodeViewModel)trview.SelectedValue).Id != null) ? ((LIT.Old_LIT.NodeViewModel)trview.SelectedValue).Id : string.Empty;
                    objtabit.tabName = ((LIT.Old_LIT.NodeViewModel)trview.SelectedValue).Name;
                    objtabit.tabfrom = "Itinerary_";
                    string filepath = ((LIT.Old_LIT.NodeViewModel)trview.SelectedValue).FolderName + "\\";
                    if (filepath.Contains("\\\\"))
                    {
                        filepath = filepath.Replace("\\\\", "\\");
                    }
                    objtabit.FilePath = filepath;
                }
                else
                {
                    NodeViewModel Invm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
                    if (Invm.Children.Where(x => x.Id == SelectedTreeNodetID).ToList().Count > 0)
                    {
                        Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().IsSelectedItinerary = true;
                        Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().IsExpandedItinerary = true;

                        objtabit.title = Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().Name;

                        objtabit.tabid = (Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().Id != null) ?
                            Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().Id : string.Empty;
                        objtabit.tabName = Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().Name;
                        objtabit.tabfrom = "Itinerary_";
                        string filepath = Invm.Children.Where(x => x.Id == SelectedTreeNodetID).FirstOrDefault().FolderName + "\\";
                        if (filepath.Contains("\\\\"))
                        {
                            filepath = filepath.Replace("\\\\", "\\");
                        }
                        objtabit.FilePath = filepath;
                    }
                    else
                    {
                        NodeViewModel Invmchld = null;
                        GetChildfromFolder(Invm, SelectedTreeNodetID, ref Invmchld);
                        if (Invmchld != null)
                        {

                            objtabit.title = Invmchld.Name;

                            objtabit.tabid = (Invmchld.Id != null) ? Invmchld.Id : string.Empty;
                            objtabit.tabName = Invmchld.Name;
                            objtabit.tabfrom = "Itinerary_";
                            string filepath = Invmchld.FolderName + "\\";
                            if (filepath.Contains("\\\\"))
                            {
                                filepath = filepath.Replace("\\\\", "\\");
                            }
                            objtabit.FilePath = filepath;
                        }
                    }

                }
            }
            if (objtabit.tabfrom != null)
                Addtabitem(SelectedTreeNodetID, objtabit);
        }

        public void AddSupplierTabDetails(string SelectedTreeNodetID,string recfrom=null)
        {
            objtabit = new Tabitemdetails();
            if (SelectedTreeNodetID.ToLower() == "newsupplier")
            {
                objtabit.title = "New Supplier";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "New Supplier";
                objtabit.tabfrom = "Supplier_";
                objtabit.FilePath = "Supplier\\New Supplier";
            }
            else
            {
                if (recfrom == null)
                {


                    objtabit.title = ((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                    objtabit.tabid = (((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId != null) ? ((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId : string.Empty;
                    objtabit.tabName = ((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                    objtabit.tabfrom = "Supplier_";
                    string filepath = ((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierFolderName + "\\";
                    if (filepath.Contains("\\\\"))
                    {
                        filepath = filepath.Replace("\\\\", "\\");
                    }
                    objtabit.FilePath = filepath;
                }
                else
                {
                    SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                    if (snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).ToList().Count > 0)
                    {
                        snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().IsSelectedsupplier = true;
                        snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().IsExpandedsupplier = true;

                        objtabit.title = snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().SupplierName;

                        objtabit.tabid = (snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().SupplierId != null) ?
                            snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().SupplierId : string.Empty;
                        objtabit.tabName = snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().SupplierName;
                        objtabit.tabfrom = "Supplier_";
                        string filepath = snvm.SupplierChildren.Where(x => x.SupplierId == SelectedTreeNodetID).FirstOrDefault().SupplierFolderName + "\\";
                        if (filepath.Contains("\\\\"))
                        {
                            filepath = filepath.Replace("\\\\", "\\");
                        }
                        objtabit.FilePath = filepath;
                    }
                    else
                    {
                        SupplierNodeViewModel snvmchld = null;
                        GetChildfromFolderSupplier(snvm, SelectedTreeNodetID, ref snvmchld);
                        if(snvmchld != null)
                        {
                            
                            objtabit.title = snvmchld.SupplierName;

                            objtabit.tabid = (snvmchld.SupplierId != null) ? snvmchld.SupplierId : string.Empty;
                            objtabit.tabName = snvmchld.SupplierName;
                            objtabit.tabfrom = "Supplier_";
                            string filepath = snvmchld.SupplierFolderName + "\\";
                            if (filepath.Contains("\\\\"))
                            {
                                filepath = filepath.Replace("\\\\", "\\");
                            }
                            objtabit.FilePath = filepath;
                        }
                    }
                }
            }
            if(objtabit.tabfrom!=null)
            Addtabitem(SelectedTreeNodetID, objtabit);
        }

        public void AddGlobalSearchTabDetails()
        {
            if (Tabitemdet.Where(x => x.tabfrom.ToLower() == "globalsearch_").Count() > 0)
            {
                objtabit = Tabitemdet.Where(x => x.tabfrom.ToLower() == "globalsearch_").FirstOrDefault();
                Addtabitem("GlobalSearch", objtabit);
            }
            else
            {
                objtabit = new Tabitemdetails();
                objtabit.title = "Global Search";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "Global Search";
                objtabit.tabfrom = "GlobalSearch_";
                objtabit.FilePath = "";
                Addtabitem("GlobalSearch", objtabit);
            }


        }

        public void AddEmailLogTabDetails()
        {
            if (Tabitemdet.Where(x => x.tabfrom.ToLower() == "emaillog_").Count() > 0)
            {
                objtabit = Tabitemdet.Where(x => x.tabfrom.ToLower() == "emaillog_").FirstOrDefault();
                Addtabitem("EmaiLogTab", objtabit);
            }
            else
            {
                objtabit = new Tabitemdetails();
                objtabit.title = "Email Log";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "Email Log";
                objtabit.tabfrom = "EmailLog_";
                objtabit.FilePath = "";
                Addtabitem("EmailLogTab", objtabit);
            }

        }

        public void AddSupplierPaymentSearchTabDetails()
        {
            if (Tabitemdet.Where(x => x.tabfrom.ToLower() == "supplierpaymentsearch_").Count() > 0)
            {
                objtabit = Tabitemdet.Where(x => x.tabfrom.ToLower() == "supplierpaymentsearch_").FirstOrDefault();
                Addtabitem("SupplierPaymentSearchTab", objtabit);
            }
            else
            {
                objtabit = new Tabitemdetails();
                objtabit.title = "Supplier Payment";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "Supplier Payment";
                objtabit.tabfrom = "SupplierPaymentSearch_";
                objtabit.FilePath = "";
                Addtabitem("SupplierPaymentSearchTab", objtabit);
            }
        }

        public void AddItinerarySearchTabDetails()
        {
            if (Tabitemdet.Where(x => x.tabfrom.ToLower() == "itinerarysearch_").Count() > 0)
            {
                objtabit = Tabitemdet.Where(x => x.tabfrom.ToLower() == "itinerarysearch_").FirstOrDefault();
                Addtabitem("ItinerarySearchTab", objtabit);
            }
            else
            {
                objtabit = new Tabitemdetails();
                objtabit.title = "Itinerary Search";
                objtabit.tabid = Guid.NewGuid().ToString();
                objtabit.tabName = "Itinerary Search";
                objtabit.tabfrom = "ItinerarySearch_";
                objtabit.FilePath = "";
                Addtabitem("ItinerarySearchTab", objtabit);
            }
        }
        private void Addtabitem(string SelectedTreeNodetID, Tabitemdetails objtabit)
        {
            try
            {
                TabItem tbitm = new TabItem();
                tbitm.MinWidth = 100;
                tbitm.Header = objtabit.tabName; tbitm.Name = objtabit.tabfrom.ToString();//+ objtabit.tabName.ToString();
                tbitm.Uid = objtabit.tabid;
                tbitm.HeaderTemplate = mnuTablist.FindResource("TabHeader") as DataTemplate;
                tbitm.Template = mnuTablist.FindResource("CustomTabItemTemplate") as ControlTemplate;
                // tbitm.Content = objtabit.FilePath;
                //ItineraryWindow Itinerwin = null;
                //Supplier supwindow = null;
                if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                {

                    if (objtabit.tabfrom.ToLower() == "itinerary_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "newitinerary")
                        {
                            Itinerwin = new ItineraryWindow(loginusername, this, "", objtabit.FilePath);
                        }
                        else
                        {
                            bool res = ItinearayReterive(SelectedTreeNodetID);
                            if (res)
                            {
                                Itinerwin = new ItineraryWindow(loginusername, this, SelectedTreeNodetID, objtabit.FilePath);
                            }
                        }
                        Itinerwin.BtnSave.Visibility = Visibility.Visible;
                        Itinerwin.BtnSaveandclose.Visibility = Visibility.Visible;
                        Itinerwin.TxtItineraryName.Focusable=true;
                        Itinerwin.TxtItineraryName.Focus();
                        Itinerwin.Loaded += (sender, e) => Keyboard.Focus(Itinerwin.TxtItineraryName);
                        Itinerwin.ItinBooking.Drop += SuppliertreeView_Drop;
                        tbitm.Content = Itinerwin;
                        tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                        tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                        tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                        tbitm.AllowDrop = true;
                        //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#BFF480"));
                        // tbitm.drag += SuppliertreeView_DragOver;
                        // tbitm.Drop += SuppliertreeView_Drop;

                    }

                    if (objtabit.tabfrom.ToLower() == "supplier_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "newsupplier")
                        {
                            supwindow = new Supplier(loginusername, this, "", objtabit.FilePath);
                        }
                        else
                        {
                            bool res = SupplierReterive(SelectedTreeNodetID);
                            if (res)
                            {
                                supwindow = new Supplier(loginusername, this, SelectedTreeNodetID, objtabit.FilePath);
                            }
                        }
                        supwindow.BtnSupplierSave.Visibility = Visibility.Visible;
                        supwindow.BtnSupplierSaveandclose.Visibility = Visibility.Visible;
                        
                        supwindow.TxtSupplierName.Focusable = true;
                        supwindow.TxtSupplierName.Focus();
                        supwindow.Loaded += (sender, e) => Keyboard.Focus(supwindow.TxtSupplierName);
                        tbitm.Content = supwindow;
                        //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                        tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                        tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                        tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                    }
                    if (objtabit.tabfrom.ToLower() == "globalsearch_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "globalsearch")
                        {
                            GlobalSearch gswin = new GlobalSearch(this);
                            tbitm.Content = gswin;
                            gswin.Loaded += (sender, e) => Keyboard.Focus(gswin.TxtSearchval);
                            //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                            tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                            tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                            tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                            tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        }
                    }
                    if (objtabit.tabfrom.ToLower() == "emaillog_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "emaillogtab")
                        {
                            EmailLogTab gswin = new EmailLogTab(this);
                            tbitm.Content = gswin;
                            //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                            tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                            tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                            tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                            tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        }
                    }
                    if (objtabit.tabfrom.ToLower() == "supplierpaymentsearch_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "supplierpaymentsearchtab")
                        {
                            SupplierPaymentSearchTab gswin = new SupplierPaymentSearchTab(this);
                            tbitm.Content = gswin;
                            //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                            tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                            tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                            tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                            tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        }
                    }
                    if (objtabit.tabfrom.ToLower() == "itinerarysearch_")
                    {
                        if (SelectedTreeNodetID.ToLower() == "itinerarysearchtab")
                        {
                            ItinerarySearchTab gswin = new ItinerarySearchTab(this);
                            tbitm.Content = gswin;
                            //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                            tbitm.BorderThickness = new Thickness(1, 1, 1, 1);
                            tbitm.BorderBrush = (Brush)(new BrushConverter().ConvertFrom("#FFDEDEDE"));
                            tbitm.Foreground = (Brush)(new BrushConverter().ConvertFrom("#252E39"));
                            tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#FFFAFAFA"));
                        }
                    }
                }
                else
                {

                    if (objtabit.tabfrom.ToLower() == "itinerary_")
                    {
                        tbitm.Content = Itinerwin;
                    }
                    if (objtabit.tabfrom.ToLower() == "supplier_")
                    {
                        tbitm.Content = supwindow;
                    }

                }
                objtabit.tabItemvalues = tbitm;

                if (Tabitemdet != null)
                {
                    if (Tabitemdet.Where(x => x.tabid == objtabit.tabid).Count() == 0)
                    {
                        tbitm.IsSelected = true;
                        tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#daf9b6"));
                        mnuTablist.Items.Add(tbitm);                        
                        Tabitemdet.Add(objtabit);
                    }
                    else
                    {
                        tbitm.IsSelected = true;
                        //tbitm.Background = (Brush)(new BrushConverter().ConvertFrom("#BFF480"));
                        // mnuTablist.SelectedItem= tbitm;

                        int index = Tabitemdet.IndexOf(Tabitemdet.Where(x => x.tabid == objtabit.tabid).FirstOrDefault());
                        int Preselectedindex = (!string.IsNullOrEmpty(hdnselectedmenu.Text)) ? Convert.ToInt32(hdnselectedmenu.Text) : 0;
                        if (index != -1 && index > -1)
                        {
                            if (index != Preselectedindex)
                            {
                                mnuTablist.SelectedIndex = -1;
                                Dispatcher.BeginInvoke((Action)(() => mnuTablist.SelectedIndex = index));
                              //  Dispatcher.BeginInvoke((Action)(() => mnuTablist.Background = (Brush)(new BrushConverter().ConvertFrom("#BFF480"))));
                                hdnselectedmenu.Text = index.ToString();
                            }
                        }

                    }


                }





            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("MainWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
        }
        private void GetChildfromFolder(NodeViewModel rootNode, string recid, ref NodeViewModel outputnode)
        {
            if (rootNode.Id == recid)
            {
                outputnode = rootNode;
            }
            foreach (var itinerary in rootNode.Children)
            {
                GetChildfromFolder(itinerary, recid, ref outputnode);                
            }

        }
        

        private void GetChildfromFolderSupplier(SupplierNodeViewModel rootNode, string recid, ref SupplierNodeViewModel outputnode)
        {
            if (rootNode.SupplierId == recid)
            {
                outputnode = rootNode;
            }
            foreach (var itinerary in rootNode.SupplierChildren)
            {
                GetChildfromFolderSupplier(itinerary, recid, ref outputnode);
            }

        }
        private void TreeViewItem_OnItemSelected(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = e.OriginalSource as TreeViewItem;
            if (item != null)
            {
                try
                {
                    ItemsControl parent = GetSelectedTreeViewItemParent(item);

                    TreeViewItem treeitem = parent as TreeViewItem;
                    string MyValue = treeitem.Header.ToString();//Gets you the immediate parent
                }
                catch (Exception ex)
                {
                    errobj.WriteErrorLoginfo("MainWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
                }
            }
        }
        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        private void MnuNewSupplier_Click(object sender, RoutedEventArgs e)
        {
            AddSupplierTabDetails("newsupplier");
            //Supplier SuppWn = new Supplier(loginusername, this, "");
            //SuppWn.BtnSupplierSave.Visibility = Visibility.Visible;
            //SuppWn.BtnSupplierSaveandclose.Visibility = Visibility.Visible;
            //Navigationframe.Navigate(SuppWn);

        }

        /*  private void trviewsupplier_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
          {
              string SelectedTreeNodetID = ((SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId;
              if (!string.IsNullOrEmpty(SelectedTreeNodetID))
              {
                  AddSupplierTabDetails(SelectedTreeNodetID);
                  //objtabit = new Tabitemdetails();
                  //objtabit.title = ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                  //objtabit.tabid = (((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId != null) ? ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId : string.Empty;
                  //objtabit.tabName = ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                  //objtabit.tabfrom = "Supplier_";
                  //Addtabitem(SelectedTreeNodetID, objtabit);
                  // MessageBox.Show(SelectedTreeNodetID);
                  // SupplierReterive(SelectedTreeNodetID);
              }
          }*/

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string tabuid = (sender as Button).CommandParameter.ToString();

            DeleteTabitem(tabuid);

        }

        public void DeleteTabitem(string tabuid)
        {
            /*  var item = mnuTablist.Items.Cast<TabItem>().Where
                         (i => i.Uid.Equals(tabuid)).SingleOrDefault();

              if (item != null)
              {
                  TabItem tab = item as TabItem;

                  TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                  mnuTablist.DataContext = null;

                  Tabitemdet.Remove(Tabitemdet.Where(m => m.tabid == tab.Uid).FirstOrDefault());
                  mnuTablist.Items.Remove(tab);
                  mnuTablist.DataContext = Tabitemdet;
                  if (selectedTab == null || selectedTab.Equals(tab))
                  {
                      if(Tabitemdet.Count>0)
                      selectedTab = Tabitemdet[0].tabItemvalues;
                  }
                  mnuTablist.SelectedItem = selectedTab;
              }

               */
            try
            {
                if (mnuTablist.SelectedItem != null)
                {
                    var item = mnuTablist.Items.Cast<TabItem>().Where
                        (i => i.Uid.Equals(tabuid)).SingleOrDefault();
                    TabItem selectedTab = null;
                    if (item != null)
                    {
                        selectedTab = item as TabItem;
                    }
                    else
                    {
                        selectedTab = mnuTablist.SelectedItem as TabItem;
                    }

                    mnuTablist.DataContext = null;
                    TabItem PrevselectedTab = null;

                    tabuid = selectedTab.Uid;

                    int indx = 0;

                    indx = Tabitemdet.ToList().FindIndex(x => x.tabid == tabuid);
                    if (indx > 0)
                    {
                        if (Tabitemdet.Count() == indx - 1)
                        {
                            PrevselectedTab = Tabitemdet[indx].tabItemvalues;
                        }
                        else
                        {
                            PrevselectedTab = Tabitemdet[indx - 1].tabItemvalues;
                        }
                    }
                    if (indx == 0)
                    {
                        PrevselectedTab = Tabitemdet[0].tabItemvalues;
                    }



                    Tabitemdet.Remove(Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault());
                    mnuTablist.Items.Remove(selectedTab);
                    mnuTablist.DataContext = Tabitemdet;
                    if (selectedTab == null || selectedTab.Equals(PrevselectedTab))
                    {
                        if (Tabitemdet.Count() > 0)
                            selectedTab = Tabitemdet[0].tabItemvalues;
                    }
                    // mnuTablist.SelectedItem = selectedTab;
                    mnuTablist.SelectedItem = PrevselectedTab;

                }
            }
            catch (Exception ex) { errobj.WriteErrorLoginfo("MainWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), ""); }

        }

        private void mnuTablist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string uidval = string.Empty, selectedval = string.Empty;


            if ((((System.Windows.Controls.Primitives.Selector)sender).SelectedItem != null))
            {
                uidval = ((System.Windows.UIElement)((System.Windows.Controls.Primitives.Selector)sender).SelectedItem).Uid;
                selectiontabandtreenode(uidval);
                tabchangefocus();

            }
        }

        private void WriteTextToFile(string text)
        {
            try
            {
                string filePath = "ItineraryID.txt"; // Specify the file path here

                // Check if the file already exists
                if (File.Exists(filePath))
                {
                    // If the file exists, overwrite its content
                    File.WriteAllText(filePath, text);
                }
                else
                {
                    // If the file doesn't exist, create a new file and write the text
                    File.WriteAllText(filePath, text);
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur while working with the file
                Console.WriteLine("Error writing to the file: " + ex.Message);
            }
        }

        private void selectiontabandtreenode(string uidval)
        {
            string selectedval = string.Empty;

            TreeviewAccordion tracc = new TreeviewAccordion();
            NodeViewModel Invm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
            if (Tabitemdet.Where(m => m.tabid == uidval).Count() > 0)
            {
                selectedval = Tabitemdet.Where(m => m.tabid == uidval).FirstOrDefault().tabfrom;
            }
            if (selectedval.ToLower() == "itinerary_")
            {
                this.WriteTextToFile(uidval);
                if (Invm != null)
                {
                    Invm.IsSelectedItinerary = true;
                    Invm.IsExpandedItinerary = true;
                    if (Invm.Children.Where(x => x.Id == uidval).ToList().Count > 0)
                    {
                        Invm.Children.ToList().ForEach(x => { x.IsExpandedItinerary = false; x.IsSelectedItinerary = false; });
                        Invm.Children.Where(x => x.Id == uidval).FirstOrDefault().IsSelectedItinerary = true;
                        Invm.Children.Where(x => x.Id == uidval).FirstOrDefault().IsExpandedItinerary = true;
                    }
                    else
                    {
                        /* string Foldersettingurl = string.Empty, drFoldersettingurl = string.Empty;
                         Foldersettingurl = loadDropDownListValues.LoadFolderName("ItineraryFolder");
                         DataSet distinctItineraryFolder = new DataSet();
                         SQLDataAccessLayer.DAL.ItineraryDAL objitdaltrmdl = new SQLDataAccessLayer.DAL.ItineraryDAL();
                         distinctItineraryFolder = objitdaltrmdl.GetDbvalue("", "GetItinearyDistinctFolderPath");

                         foreach (DataRow row in distinctItineraryFolder.Tables[0].Rows)
                         {
                             if (row["ItineraryFolderPath"] != null)
                             {
                                 var Itinearyfolderlist = Invm.FolderName.Split("\\").ToList();

                                 if (Itinearyfolderlist.Count > 0)
                                 {
                                     GetnodebyIDItinerary(Invm, Itinearyfolderlist[0], ref Invm);
                                     if (Invm == null)
                                     {
                                         Invm.IsExpandedItinerary = true;
                                         Invm.IsSelectedItinerary = true;
                                     }

                                     Itinearyfolderlist.RemoveAt(0);
                                    // GetnodebyIDItinerary(Invm, Itinearyfolderlist[0], ref Invm);
                                 }

                             }
                         }
                        */
                    }
                     
                    trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;

                }
            }
            if (selectedval.ToLower() == "supplier_")
            {
                //supwindow = new Supplier(loginusername, this, uidval);
                SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                if (snvm != null)
                {

                    snvm.IsSelectedsupplier = true;
                    snvm.IsExpandedsupplier = true;
                    if (snvm.SupplierChildren.Where(x => x.SupplierId == uidval).ToList().Count > 0)
                    {
                        snvm.SupplierChildren.Where(x => x.SupplierId == uidval).FirstOrDefault().IsSelectedsupplier = true;
                        snvm.SupplierChildren.Where(x => x.SupplierId == uidval).FirstOrDefault().IsExpandedsupplier = true;
                    }

                }
                
                trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;
            }



        }

        private void GetnodebyIDItinerary(NodeViewModel rootNode, string SupID, ref NodeViewModel outputnode)
        {

            if (rootNode.Id == SupID)
            {
                outputnode = rootNode;
            }
            foreach (var Supplier in rootNode.Children)
            {
                //Supplier.IsSelectedItinerary = true;
                //Supplier.IsExpandedItinerary = true;
                GetnodebyIDItinerary(Supplier, SupID, ref outputnode);

            }

        }
        private void GetnodebyIDSupplier(SupplierNodeViewModel rootNode, string SupID, ref SupplierNodeViewModel outputnode)
        {

            if (rootNode.SupplierId == SupID)
            {
                outputnode = rootNode;
            }
            foreach (var Supplier in rootNode.SupplierChildren)
            {
                GetnodebyIDSupplier(Supplier, SupID, ref outputnode);

            }

        }
        private void SelectPreviousTab()
        {
            if (mnuTablist.SelectedItem != null)
            {
                TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                string tabuid = selectedTab.Uid;

                int indx = 0;

                indx = Tabitemdet.ToList().FindIndex(x => x.tabid == tabuid);
                if (indx > 0)
                {
                    if (Tabitemdet.Count() == indx - 1)
                    {
                        selectedTab = Tabitemdet[indx].tabItemvalues;
                    }
                    else
                    {
                        selectedTab = Tabitemdet[indx - 1].tabItemvalues;
                    }
                }
                if (indx == 0)
                {
                    selectedTab = Tabitemdet[0].tabItemvalues;
                }
                mnuTablist.SelectedItem = selectedTab;

            }
        }

        private void SelectNextTab()
        {
            if (mnuTablist.SelectedItem != null)
                {
                    TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                    string tabuid = selectedTab.Uid;

                    int indx = 0;

                    indx = Tabitemdet.ToList().FindIndex(x => x.tabid == tabuid);
                    if (indx > 0)
                    {
                        if (Tabitemdet.Count() == indx + 1)
                        {
                            selectedTab = Tabitemdet[indx].tabItemvalues;
                        }
                        else
                        {
                            selectedTab = Tabitemdet[indx + 1].tabItemvalues;
                        }
                    }
                    if (indx == 0)
                    {
                        selectedTab = Tabitemdet[indx + 1].tabItemvalues;
                    }
                    mnuTablist.SelectedItem = selectedTab;

                }            
        }
    
        private void btnPreviousTab_Click(object sender, RoutedEventArgs e)
        {
            if (mnuTablist.Items.Count > 1)
            {
                SelectPreviousTab();
            }
        }

        private void btnNextTab_Click(object sender, RoutedEventArgs e)
        {
            if (mnuTablist.Items.Count > 1)
            {
                SelectNextTab();
            }
        }

        private void btnDeletetab_Click(object sender, RoutedEventArgs e)
        {
            if (mnuTablist.SelectedItem != null)
            {
                TabItem selectedTab = null;
                selectedTab = mnuTablist.SelectedItem as TabItem;
                if (selectedTab != null)
                {
                    DeleteTabitem(selectedTab.Uid);
                }
            }

        }

        private void BtnSaveandclose_Click(object sender, RoutedEventArgs e)
        {
            if (mnuTablist.SelectedItem != null)
            {
                TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();
                //if (!string.IsNullOrEmpty(tabfrom))
                //{
                //    if (tabfrom.ToLower() == "itinerary_")
                //    {
                //        Itinerwin.BtnSaveandclose_Click(sender, e);
                //        DeleteTabitem(selectedTab.Uid);
                //    }
                //    if (tabfrom.ToLower() == "supplier_")
                //    {

                //        supwindow.BtnSupplierSaveandclose_Click(sender, e);
                //        DeleteTabitem(selectedTab.Uid);
                //    }
                //}

                int indx = 0;

                indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);
                bool Datasaveresultrec = false;
                
                if (!string.IsNullOrEmpty(tabfrom))
                {
                    if (tabfrom.ToLower() == "itinerary_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSaveandclose_Click(sender, e);
                                Datasaveresultrec = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).DatasaveresultItinerary;
                               // tabnameaftersave = ((LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;

                            }
                            else
                            {
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSaveandclose_Click(sender, e);
                                Datasaveresultrec = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).DatasaveresultItinerary;
                                // tabnameaftersave = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;
                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSaveandclose_Click(sender, e);
                            Datasaveresultrec = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).DatasaveresultItinerary;
                           // tabnameaftersave = ((LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;
                        }
                    }
                    if (tabfrom.ToLower() == "supplier_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSaveandclose_Click(sender, e);
                                Datasaveresultrec = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Datasaveresultsupplier;
                                //tabnameaftersave = ((LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;
                            }
                            else
                            {
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSaveandclose_Click(sender, e);
                                Datasaveresultrec = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Datasaveresultsupplier;
                                //tabnameaftersave = ((LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;
                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSaveandclose_Click(sender, e);
                            Datasaveresultrec = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Datasaveresultsupplier;
                            //tabnameaftersave = ((LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;

                        }
                        // supwindow = new Supplier(loginusername, this, selectedTab.Uid);
                        // supwindow.BtnSupplierSave_Click(sender, e);
                    }
                    if (Datasaveresultrec == true)
                    {
                        DeleteTabitem(selectedTab.Uid);
                    }
                    //if(!string.IsNullOrEmpty(tabnameaftersave))
                    //{
                    //    selectedTab.Name= tabnameaftersave;
                    //    Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabName= tabnameaftersave;
                    //    mnuTablist.DataContext = Tabitemdet;
                    //}
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (mnuTablist.SelectedItem != null)
            {
                TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();

                int indx = 0;

                indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);
                string tabnameaftersave = string.Empty;

                if (!string.IsNullOrEmpty(tabfrom))
                {
                    if (tabfrom.ToLower() == "itinerary_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSave_Click(sender, e);
                                tabnameaftersave = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;
                            }
                            else
                            {
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSave_Click(sender, e);
                                tabnameaftersave = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;
                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSave_Click(sender, e);
                            tabnameaftersave = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Text;
                        }
                    }
                    if (tabfrom.ToLower() == "supplier_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSave_Click(sender, e);
                                tabnameaftersave = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;
                            }
                            else
                            {
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSave_Click(sender, e);
                                tabnameaftersave = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;
                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).BtnSupplierSave_Click(sender, e);
                            tabnameaftersave = ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Text;

                        }
                        // supwindow = new Supplier(loginusername, this, selectedTab.Uid);
                        // supwindow.BtnSupplierSave_Click(sender, e);
                    }
                    if (!string.IsNullOrEmpty(tabnameaftersave))
                    {
                        selectedTab.Header = tabnameaftersave;
                        Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabName = tabnameaftersave;
                        //mnuTablist.DataContext = Tabitemdet;
                    }
                }

                //  selectiontabandtreenode(selectedTab.Uid);
            }
        }



        private void tabchangefocus()
        {
            if (mnuTablist.SelectedItem != null && Tabitemdet.Count>0)
            {
                TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                string tabfrom = (Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault()!=null)?
                    Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString():"";

                int indx = 0;

                indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);
                string tabnameaftersave = string.Empty;

                if (!string.IsNullOrEmpty(tabfrom))
                {
                    if (tabfrom.ToLower() == "itinerary_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focusable = true;
                                ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focus();
                                //((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                 //   (sender, e) => Keyboard.Focus(((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName);

                            }
                            else
                            {
                                if (indx == 1 && mnuTablist.Items.Count == 1)
                                {
                                    ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focusable = true;
                                    ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focus();
                                   // ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                      //  (sender, e) => Keyboard.Focus(((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[0]).Content).TxtItineraryName);

                                }
                                else
                                {
                                    ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focusable = true;
                                    ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focus();
                                  //  ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                     //   (sender, e) => Keyboard.Focus(((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName);
                                }
                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focusable = true;
                            ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName.Focus();
                           // ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                              //  (sender, e) => Keyboard.Focus(((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtItineraryName);

                        }
                        // supwindow = new Supplier(loginusername, this, selectedTab.Uid);
                        // supwindow.BtnSupplierSave_Click(sender, e);
                    }
                    if (tabfrom.ToLower() == "supplier_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focusable = true;
                                ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focus();
                               // ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                 //   (sender, e) => Keyboard.Focus(((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName);

                            }
                            else
                            {
                                if (indx == 1 && mnuTablist.Items.Count==1)
                                {
                                    ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focusable = true;
                                    ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focus();
                                   // ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                      //  (sender, e) => Keyboard.Focus(((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[0]).Content).TxtSupplierName);
                                }
                                else
                                {
                                    ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focusable = true;
                                    ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focus();
                                   // ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                                     //   (sender, e) => Keyboard.Focus(((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName);
                                }

                            }
                        }
                        if (indx == 0)
                        {
                            ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focusable = true;
                            ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName.Focus();
                           // ((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).Loaded +=
                              //  (sender, e) => Keyboard.Focus(((LIT.Old_LIT.Supplier)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content).TxtSupplierName);
                        }
                        // supwindow = new Supplier(loginusername, this, selectedTab.Uid);
                        // supwindow.BtnSupplierSave_Click(sender, e);
                    }

                }

                //  selectiontabandtreenode(selectedTab.Uid);
            }
        }
        private void mouseEnterSave(object sender, MouseEventArgs e)
        {

            BtnSave.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void museLeaveOption(object sender, MouseEventArgs e)
        {
            BtnSave.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF579F00"));
        }

        private void mouseEnterSaveClose(object sender, MouseEventArgs e)
        {
            BtnSaveandclose.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void museLeaveOptionSaveClose(object sender, MouseEventArgs e)
        {
            BtnSaveandclose.Foreground = (Brush)(new BrushConverter().ConvertFrom("#FF579F00"));
        }

      /*  private void Accordion_SelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            string selectedval = string.Empty;
            TabItem selectedTab = null;
            string uidval = string.Empty;
            selectedTab = mnuTablist.SelectedItem as TabItem;
            TreeviewAccordion tracc = new TreeviewAccordion();
            NodeViewModel Invm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
            if (Tabitemdet.Where(m => m.tabid == uidval).Count() > 0)
            {
                selectedval = Tabitemdet.Where(m => m.tabid == uidval).FirstOrDefault().tabfrom;
            }
            if (selectedval.ToLower() == "itinerary_")
            {
                if (Invm != null)
                {
                    Invm.IsSelectedItinerary = true;
                    Invm.IsExpandedItinerary = true;
                    if (Invm.Children.Where(x => x.Id == uidval).ToList().Count > 0)
                    {
                        Invm.Children.ToList().ForEach(x => { x.IsExpandedItinerary = false; x.IsSelectedItinerary = false; });
                        Invm.Children.Where(x => x.Id == uidval).FirstOrDefault().IsSelectedItinerary = true;
                        Invm.Children.Where(x => x.Id == uidval).FirstOrDefault().IsExpandedItinerary = true;
                    }
                    else
                    {
                        /* string Foldersettingurl = string.Empty, drFoldersettingurl = string.Empty;
                         Foldersettingurl = loadDropDownListValues.LoadFolderName("ItineraryFolder");
                         DataSet distinctItineraryFolder = new DataSet();
                         SQLDataAccessLayer.DAL.ItineraryDAL objitdaltrmdl = new SQLDataAccessLayer.DAL.ItineraryDAL();
                         distinctItineraryFolder = objitdaltrmdl.GetDbvalue("", "GetItinearyDistinctFolderPath");

                         foreach (DataRow row in distinctItineraryFolder.Tables[0].Rows)
                         {
                             if (row["ItineraryFolderPath"] != null)
                             {
                                 var Itinearyfolderlist = Invm.FolderName.Split("\\").ToList();

                                 if (Itinearyfolderlist.Count > 0)
                                 {
                                     GetnodebyIDItinerary(Invm, Itinearyfolderlist[0], ref Invm);
                                     if (Invm == null)
                                     {
                                         Invm.IsExpandedItinerary = true;
                                         Invm.IsSelectedItinerary = true;
                                     }

                                     Itinearyfolderlist.RemoveAt(0);
                                    // GetnodebyIDItinerary(Invm, Itinearyfolderlist[0], ref Invm);
                                 }

                             }
                         }
                        */
                   /* }

                    trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;

                }
            }
            if (selectedval.ToLower() == "supplier_")
            {
                //supwindow = new Supplier(loginusername, this, uidval);
                SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
                if (snvm != null)
                {

                    snvm.IsSelectedsupplier = true;
                    snvm.IsExpandedsupplier = true;
                    if (snvm.SupplierChildren.Where(x => x.SupplierId == uidval).ToList().Count > 0)
                    {
                        snvm.SupplierChildren.Where(x => x.SupplierId == uidval).FirstOrDefault().IsSelectedsupplier = true;
                        snvm.SupplierChildren.Where(x => x.SupplierId == uidval).FirstOrDefault().IsExpandedsupplier = true;
                    }

                }
                trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;
            }

        }
        */
        //private void Accordioncntrl_SelectedItemsChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
            
        //    TreeviewAccordion tracc = new TreeviewAccordion();
        //    if (((System.Windows.FrameworkElement)Accordioncntrl.SelectedItem).Name.ToLower()=="accsupplier")
        //    {
        //        NodeViewModel Invm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();
        //        Invm.IsSelectedItinerary = true;
        //        Invm.IsExpandedItinerary = true;
        //        if (Invm.Children.ToList().Count > 0)
        //        {
        //            Invm.Children.ToList().ForEach(x => { x.IsExpandedItinerary = false; x.IsSelectedItinerary = false; });
        //        }
        //        trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
        //    }
        //    if (((System.Windows.FrameworkElement)Accordioncntrl.SelectedItem).Name.ToLower() == "accitinerary")
        //    {
        //        SupplierNodeViewModel snvm = tracc.SupplierTreeViewModel.SupplierItems.FirstOrDefault();
        //        if (snvm != null)
        //        {

        //            snvm.IsSelectedsupplier = true;
        //            snvm.IsExpandedsupplier = true;
        //            if (snvm.SupplierChildren.ToList().Count > 0)
        //            {
        //                snvm.SupplierChildren.ToList().ForEach(x => { x.IsExpandedsupplier = false; x.IsSelectedsupplier = false; });
        //            }

        //        }
        //        trviewsupplier.ItemsSource = tracc.SupplierTreeViewModel.SupplierItems;
        //    }

        //}

        /* Tree view Drag and Drop */
        private Point _lastMouseDown;
        private SupplierNodeViewModel draggedItem;
        ItineraryWindow _target;
         
        private void SuppliertreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                ItineraryWindow _target = GetNearestContainer(e.OriginalSource as UIElement);
                 _lastMouseDown = e.GetPosition(_target);
            }
        }
        private void SuppliertreeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (mnuTablist.Items.Count > 0)
                {
                    TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                    string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();
                    ItineraryWindow _target = null;
                    int indx = 0;

                    indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);

                    if (!string.IsNullOrEmpty(tabfrom))
                    {
                        if (tabfrom.ToLower() == "itinerary_")
                        {
                            if (indx > 0)
                            {
                                if (Tabitemdet.Count() == indx - 1)
                                {
                                    _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                                }
                                else
                                {
                                    _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                                }
                            }
                            if (indx == 0)
                            {
                                _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                            }
                        }

                    }


                    //TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                    if (selectedTab != null)
                    {
                        // string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();

                        // if(tabfrom)
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            // ItineraryWindow _target = GetNearestContainer(e.OriginalSource as UIElement);
                            Point currentPosition = e.GetPosition(_target);

                            //  if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                            //     (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                            //  {
                            draggedItem = (SupplierNodeViewModel)trviewsupplier.SelectedItem;
                            if (draggedItem != null)
                            {

                                //object data = e.Data.GetData(typeof(DateTime));
                                //if (data == null) { return; }
                                //nodeToDropIn.Nodes.Add(data.ToString());
                                if (_target != null)
                                {
                                    DragDropEffects finalDropEffect =
                                     // DragDrop.DoDragDrop(this, _target, DragDropEffects.Move);

                                     DragDrop.DoDragDrop(trviewsupplier, trviewsupplier.SelectedValue, DragDropEffects.Move);
                                    //Checking target is not null and item is
                                    //dragging(moving)

                                    if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                                    // if (_target != null)
                                    {
                                        _target.ItinBooking.IsSelected=true;
                                        //string selectedsupplierid=(((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue)!=null)?((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId:string.Empty;
                                        //BookingSupplierSearch wnbkadd = new BookingSupplierSearch(loginusername, _target, selectedsupplierid);
                                        //wnbkadd.ShowDialog();
                                    }
                                }
                            }
                            //}
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }



        //private void SuppliertreeView_DragOver(object sender, DragEventArgs e)
        //{
        //    try
        //    {
        //         e.Effects = DragDropEffects.Move;
        //         e.Handled = true;

        //    }
        //    catch (Exception)
        //    {
        //    }

        //}



        private void SuppliertreeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                //e.Effects = DragDropEffects.Move;
                //e.Handled = true;

                if (mnuTablist.Items.Count > 0)
                {
                    TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                    string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();
                    ItineraryWindow _target = null;
                    int indx = 0;

                    indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);

                    if (!string.IsNullOrEmpty(tabfrom))
                    {
                        if (tabfrom.ToLower() == "itinerary_")
                        {
                            if (indx > 0)
                            {
                                if (Tabitemdet.Count() == indx - 1)
                                {
                                    _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                                }
                                else
                                {
                                    _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                                }
                            }
                            if (indx == 0)
                            {
                                _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                            }
                        }

                    }


                    //TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                    if (selectedTab != null)
                    {
                        _target.ItinBooking.IsSelected = true;
                        string selectedsupplierid = (((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue) != null) ? ((LIT.Old_LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId : string.Empty;
                        BookingSupplierSearch wnbkadd = new BookingSupplierSearch(loginusername, _target, selectedsupplierid);
                        wnbkadd.ShowDialog();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private ItineraryWindow GetNearestContainer(UIElement? uIElement)
        {
            if (uIElement != null)
            {
                TabItem selectedTab = mnuTablist.SelectedItem as TabItem;
                string tabfrom = Tabitemdet.Where(m => m.tabid == selectedTab.Uid).FirstOrDefault().tabfrom.ToString();
                ItineraryWindow _target = null;
                int indx = 0;

                indx = Tabitemdet.ToList().FindIndex(x => x.tabid == selectedTab.Uid);

                if (!string.IsNullOrEmpty(tabfrom))
                {
                    if (tabfrom.ToLower() == "itinerary_")
                    {
                        if (indx > 0)
                        {
                            if (Tabitemdet.Count() == indx - 1)
                            {
                                _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                            }
                            else
                            {
                                _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                            }
                        }
                        if (indx == 0)
                        {
                            _target = ((LIT.Old_LIT.ItineraryWindow)((System.Windows.Controls.ContentControl)this.mnuTablist.Items[indx]).Content);
                        }
                    }

                }

                return _target;                
                 
            }
            return null;
           // throw new NotImplementedException();
        }

        private void trviewsupplier_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((SupplierNodeViewModel)trviewsupplier.SelectedValue) != null)
            {
                string SelectedTreeNodetID = ((SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId;
                if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                {
                    AddSupplierTabDetails(SelectedTreeNodetID);
                    //objtabit = new Tabitemdetails();
                    //objtabit.title = ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                    //objtabit.tabid = (((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId != null) ? ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId : string.Empty;
                    //objtabit.tabName = ((LIT.SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierName;
                    //objtabit.tabfrom = "Supplier_";
                    //Addtabitem(SelectedTreeNodetID, objtabit);
                    // MessageBox.Show(SelectedTreeNodetID);
                    // SupplierReterive(SelectedTreeNodetID);
                }
            }
        }
        
        private void trviewContact_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((SupplierNodeViewModel)trviewcontact.SelectedValue) != null)
            {
                string SelectedTreeNodetID = ((SupplierNodeViewModel)trviewcontact.SelectedValue).SupplierId;
                if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                {
                    Contacts contacts = new Contacts();
                    ContactsViewModel viewModel = new ContactsViewModel(this, SelectedTreeNodetID);                    
                    contacts.DataContext = viewModel;
                    contacts.ShowDialog();

                }
            }
        }

        private void trviewsupplier_KeyUp(object sender, KeyEventArgs e)
        {
            //TreeView topic = sender as TreeView;
           // string keyValue = e.Key.ToString();
            if (((char)e.Key) == (char)(Key.Enter))
            {
                string SelectedTreeNodetID = ((SupplierNodeViewModel)trviewsupplier.SelectedValue).SupplierId;
                if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                {
                    AddSupplierTabDetails(SelectedTreeNodetID);
                }
            }
            //if (Keyboard.IsKeyUp(e.Key.Enter))
            //{
            //    //do something here
            //}
           
        }

        private void trview_KeyUp(object sender, KeyEventArgs e)
        {
            if (((char)e.Key) == (char)(Key.Enter))
            {
                if (trview.SelectedValue != null)
                {
                    string SelectedTreeNodetID = ((NodeViewModel)trview.SelectedValue).Id;
                    if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                    {
                        AddItineraryTabDetails(SelectedTreeNodetID);
                    }
                }

            }
        }

        private void trview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (trview.SelectedValue != null)
            {
                string SelectedTreeNodetID = ((NodeViewModel)trview.SelectedValue).Id;
                if (!string.IsNullOrEmpty(SelectedTreeNodetID))
                {
                    AddItineraryTabDetails(SelectedTreeNodetID);
                }
            }

        }

        private void OpenPaymentSupplier(object sender, RoutedEventArgs e)
        {
            Itinerwin.CallPaymentSupplier();
        }

        private void SupplierPaymentSearch_Click(object sender, RoutedEventArgs e)
        {
            AddSupplierPaymentSearchTabDetails();
        }

        private void ItinerarySearch_Click(object sender, RoutedEventArgs e)
        {
            AddItinerarySearchTabDetails();
        }
        private void EmailLog_Click(object sender, RoutedEventArgs e)
        {
            AddEmailLogTabDetails();
        }
        private void GlobalSearch_Click(object sender, RoutedEventArgs e)
        {
            // GlobalSearch obj = new GlobalSearch();
            // obj.Show();
            AddGlobalSearchTabDetails();
        }

        private TreeViewItem selectedTreeViewItem; // Field to store the selected item
        private List<TreeViewItem> copiedItems = new List<TreeViewItem>(); // List to store copied items

        private void trview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            selectedTreeViewItem = trview.SelectedItem as TreeViewItem; // Update the selected item
        }

        private TreeViewItem CloneTreeViewItem(TreeViewItem sourceItem)
        {
            TreeViewItem newItem = new TreeViewItem();
            newItem.Header = sourceItem.Header; // Copy the header content

            // Clone the child items recursively
            foreach (TreeViewItem childItem in sourceItem.Items)
            {
                newItem.Items.Add(CloneTreeViewItem(childItem));
            }

            return newItem;
        }

        //private void NewMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    TreeViewItem newItem = new TreeViewItem();
        //    newItem.Header = "New Item"; // Set the default name for the new item

        //    // Add the new item as a top-level item in the TreeView
        //    trview.Items.Add(newItem);
        //}
        //private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    // Clear the previous copied items
        //    copiedItems.Clear();

        //    // Get the selected item from the TreeView
        //    if (selectedTreeViewItem != null)
        //    {
        //        copiedItems.Add(CloneTreeViewItem(selectedTreeViewItem));
        //    }
        //}

        //private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    // Check if there are copied items
        //    if (copiedItems.Count > 0)
        //    {
        //        // Retrieve the currently selected TreeViewItem (if any)
        //        if (selectedTreeViewItem != null)
        //        {
        //            // Add the copied items as children to the selected item
        //            foreach (TreeViewItem copiedItem in copiedItems)
        //            {
        //                selectedTreeViewItem.Items.Add(CloneTreeViewItem(copiedItem));
        //            }
        //        }
        //        else
        //        {
        //            // Add the copied items as top-level items if nothing is selected
        //            foreach (TreeViewItem copiedItem in copiedItems)
        //            {
        //                trview.Items.Add(CloneTreeViewItem(copiedItem));
        //            }
        //        }
        //    }
        //}

        //private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    // Clear the previous copied items
        //    copiedItems.Clear();

        //    // Remove the selected item from the TreeView
        //    if (selectedTreeViewItem != null)
        //    {
        //        copiedItems.Add(CloneTreeViewItem(selectedTreeViewItem));
        //        TreeViewItem parentItem = selectedTreeViewItem.Parent as TreeViewItem;
        //        if (parentItem != null)
        //        {
        //            parentItem.Items.Remove(selectedTreeViewItem);
        //        }
        //        else
        //        {
        //            trview.Items.Remove(selectedTreeViewItem);
        //        }
        //    }
        //}

        //private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    // Remove the selected item from the TreeView
        //    if (selectedTreeViewItem != null)
        //    {
        //        TreeViewItem parentItem = selectedTreeViewItem.Parent as TreeViewItem;
        //        if (parentItem != null)
        //        {
        //            parentItem.Items.Remove(selectedTreeViewItem);
        //        }
        //        else
        //        {
        //            trview.Items.Remove(selectedTreeViewItem);
        //        }
        //    }
        //}

        //private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    // Allow the user to edit the name of the selected item
        //    if (selectedTreeViewItem != null)
        //    {
        //        TextBox textBox = new TextBox();
        //        textBox.Text = selectedTreeViewItem.Header.ToString();
        //        textBox.KeyUp += (s, keyArgs) =>
        //        {
        //            if (keyArgs.Key == Key.Enter)
        //            {
        //                selectedTreeViewItem.Header = textBox.Text;
        //            }
        //        };

        //        selectedTreeViewItem.Header = textBox;
        //        textBox.Focus();
        //    }
        //}

        private void MnuNewContact_Click(object sender, RoutedEventArgs e)
        {
            Contacts objcon = new Contacts(true);
            ContactsViewModel viewModel = new ContactsViewModel(this);
            viewModel.UserId = Guid.Parse(this.loginuserid);
            objcon.DataContext = viewModel;
            objcon.ShowDialog();
        }

        private void DataTrigger_Error(object sender, ValidationErrorEventArgs e)
        {

        }
    }

    public class TreeviewAccordion
    {

        public TreeViewModel ItineraryTreeViewModelTr { get; }

        public SupplierTreeViewModel SupplierTreeViewModel { get; }
        public ContactTreeViewModel ContactTreeViewModel { get; }

        public TreeviewAccordion()
        {
            ItineraryTreeViewModelTr = new TreeViewModel();
            SupplierTreeViewModel = new SupplierTreeViewModel();
            ContactTreeViewModel = new ContactTreeViewModel();
        }
    }

    public class NodeViewModel : ViewModelBase
    {
        public NodeViewModel()
        {
            Children = new ObservableCollection<NodeViewModel>();
        }

        private bool _IsExpandedItinerary;

        public bool IsExpandedItinerary
        {
            get { return _IsExpandedItinerary; }
            set
            {
                _IsExpandedItinerary = value;
                //OnPropertyChangedNotify("IsExpandedItinerary");
            }
        }

        private bool _IsSelectedItinerary;

        public bool IsSelectedItinerary
        {
            get { return _IsSelectedItinerary; }
            set
            {
                _IsSelectedItinerary = value;
                //OnPropertyChangedNotify("IsSelectedItinerary");
            }
        }


        private string _Id;
        public string Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _Parentflag;
        public string Parentflag
        {
            get { return _Parentflag; }
            set
            {
                _Parentflag = value;
                OnPropertyChanged("Parentflag");
            }
        }

        private string _Name = string.Empty;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;

                    OnPropertyChanged("Name");
                }
            }
        }

        private string _FolderName = string.Empty;
        public string FolderName
        {
            get { return _FolderName; }
            set
            {
                if (_FolderName != value)
                {
                    _FolderName = value;
                    OnPropertyChanged("FolderName");
                }
            }
        }

        private string _Folderpath = string.Empty;
        public string Folderpath
        {
            get { return _Folderpath; }
            set
            {
                if (_Folderpath != value)
                {
                    _Folderpath = value;
                    OnPropertyChanged("Folderpath");
                }
            }
        }



        //private ImageSource _ChildImageurl = null;
        //public ImageSource ChildImageurl
        //{
        //    get { return _ChildImageurl; }
        //    set
        //    {
        //        if (_ChildImageurl != value)
        //        {
        //            _ChildImageurl = value;
        //            OnPropertyChangedNotify("ChildImageurl");
        //        }
        //    }
        //}


        private ImageSource _Imageurl = null;
        public ImageSource Imageurl
        {
            get { return _Imageurl; }
            set
            {
                if (_Imageurl != value)
                {
                    _Imageurl = value;
                    OnPropertyChanged("Imageurl");
                }
            }
        }

        //private ObservableCollection<NodeViewModel> _FolderStructure;
        //public ObservableCollection<NodeViewModel> FolderStructure
        //{
        //    get { return _FolderStructure; }
        //    set
        //    {
        //        _FolderStructure = value;
        //    }
        //}



        private ObservableCollection<NodeViewModel> _Children;
        public ObservableCollection<NodeViewModel> Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                OnPropertyChanged("Children");
            }
        }

        public CompositeCollection Itemscoll
        {
            get
            {
                return new CompositeCollection()
          {
             new CollectionContainer() { Collection = Children },
             // Add other type of collection in composite collection
             // new CollectionContainer() { Collection = OtherTypeSources }
          };
            }
        }

    }

    public class SupplierNodeViewModel : ViewModelBase
    {
        public SupplierNodeViewModel()
        {
            SupplierChildren = new ObservableCollection<SupplierNodeViewModel>();
        }

        private bool _IsExpandedsupplier;

        public bool IsExpandedsupplier
        {
            get { return _IsExpandedsupplier; }
            set
            {
                _IsExpandedsupplier = value;
                OnPropertyChanged("IsExpandedsupplier");
            }
        }

        private bool _IsSelectedsupplier;

        public bool IsSelectedsupplier
        {
            get { return _IsSelectedsupplier; }
            set
            {
                _IsSelectedsupplier = value;
                OnPropertyChanged("_IsSelectedsupplier");
            }
        }

        private string _SupplierId;
        public string SupplierId
        {
            get { return _SupplierId; }
            set
            {
                _SupplierId = value;
                OnPropertyChanged("SupplierId");
            }
        }

        private string _SupplierParentflag;
        public string SupplierParentflag
        {
            get { return _SupplierParentflag; }
            set
            {
                _SupplierParentflag = value;
                OnPropertyChanged("SupplierParentflag");
            }
        }

        private string _SupplierName = string.Empty;
        public string SupplierName
        {
            get { return _SupplierName; }
            set
            {
                if (_SupplierName != value)
                {
                    _SupplierName = value;

                    OnPropertyChanged("SupplierName");
                }
            }
        }

        private string _SupplierFolderName = string.Empty;
        public string SupplierFolderName
        {
            get { return _SupplierFolderName; }
            set
            {
                if (_SupplierFolderName != value)
                {
                    _SupplierFolderName = value;
                    OnPropertyChanged("SupplierFolderName");
                }
            }
        }

        private string _SupplierFolderpath = string.Empty;
        public string SupplierFolderpath
        {
            get { return _SupplierFolderpath; }
            set
            {
                if (_SupplierFolderpath != value)
                {
                    _SupplierFolderpath = value;
                    OnPropertyChanged("SupplierFolderpath");
                }
            }
        }




        private ImageSource _SupplierImageurl = null;
        public ImageSource SupplierImageurl
        {
            get { return _SupplierImageurl; }
            set
            {
                if (_SupplierImageurl != value)
                {
                    _SupplierImageurl = value;
                    OnPropertyChanged("SupplierImageurl");
                }
            }
        }




        private ObservableCollection<SupplierNodeViewModel> _SupplierChildren;
        public ObservableCollection<SupplierNodeViewModel> SupplierChildren
        {
            get { return _SupplierChildren; }
            set
            {
                _SupplierChildren = value;
                OnPropertyChanged("SupplierChildren");
            }
        }



    }

    public class UniformTabPanel : UniformGrid
    {
        public UniformTabPanel()
        {
            this.IsItemsHost = true;
            this.Rows = 1;

            //Default, so not really needed..
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            //var totalMaxWidth = this.Children.OfType<TabItem>().Sum(tab => tab.MaxWidth);
            //if (!double.IsInfinity(totalMaxWidth))
            //{
            //    this.HorizontalAlignment = (constraint.Width > totalMaxWidth)
            //                                        ? HorizontalAlignment.Left
            //                                        : HorizontalAlignment.Stretch;
            //}

            //return base.MeasureOverride(constraint);
            var children = this.Children.OfType<TabItem>();
            var totalMaxWidth = children.Sum(tab => tab.MinWidth);
            if (!double.IsInfinity(totalMaxWidth))
            {
                this.HorizontalAlignment = (constraint.Width > totalMaxWidth)
                                                    ? HorizontalAlignment.Left
                                                    : HorizontalAlignment.Stretch;
                foreach (var child in children)
                {
                    child.Width = this.HorizontalAlignment == System.Windows.HorizontalAlignment.Left
                            ? child.MinWidth
                            : Double.NaN;
                }
            }
            return base.MeasureOverride(constraint);
        }
    }

    /* public class supplierTreeviewAcc
      {

          public SupplierTreeViewModel SupplierTreeViewModel { get; } = new SupplierTreeViewModel();
          public SupplierNodeViewModel SupplierNodeViewModel { get; } = new SupplierNodeViewModel();
      }
    */
    //public event PropertyChangedEventHandler PropertyChanged;
    //protected void OnPropertyChangedNotify(string p_strPropertyName)
    //{
    //    if (PropertyChanged != null)
    //        PropertyChanged(this, new PropertyChangedEventArgs(p_strPropertyName));
    //}

    //    public readonly ObservableCollection<NodeViewModel> _Children
    //= new ObservableCollection<NodeViewModel>();
    //    public ObservableCollection<NodeViewModel> Children { get { return _Children; } }

    

}
