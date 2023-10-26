using SQLDataAccessLayer.DAL;
using Prism.Ioc;
using SQLDataAccessLayer.Models;
 
using System;

using System.Collections.Generic;

using System.Collections.ObjectModel;

using System.Data;

using System.Data.SqlTypes;

using System.Globalization;

using System.Linq;

using System.Reflection;

using System.Runtime.Intrinsics.X86;

using System.Text;

using System.Text.RegularExpressions;

using System.Threading.Tasks;

using System.Windows;

using System.Windows.Automation.Peers;

using System.Windows.Automation.Provider;

using System.Windows.Controls;

using System.Windows.Controls.Primitives;

using System.Windows.Data;

using System.Windows.Documents;

using System.Windows.Input;

using System.Windows.Media;

using System.Windows.Media.Imaging;

using System.Windows.Navigation;

using System.Windows.Shapes;

//using LIT.LITModels.Models;

using LITModels;

using static LIT.Old_LIT.TreeViewCreation;

using static LIT.Old_LIT.MainWindow;

using System.Collections;

using System.Windows.Threading;

using LIT.Modules.TabControl.ViewModels;

using LITModels.LITModels.Models;

using System.IO;

using System.Reflection.PortableExecutable;

using LIT.Core.pdfTemplates;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using LIT.ViewModels;

using Path = System.IO.Path;

using System.Net.Http;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

using System.Reflection.Metadata;

using iTextSharp.text.pdf;

using iTextSharp.text.html.simpleparser;

using iTextSharp.tool.xml;

using System.Diagnostics;

using iTextSharp.text.pdf.codec;

using Microsoft.VisualBasic;
using static System.Net.Mime.MediaTypeNames;
using LIT.Core.Mvvm;
using iTextSharp.tool.xml.html.head;
using System.ComponentModel;
using iTextSharp.tool.xml.html;
using Color = System.Windows.Media.Color;
using LIT.Core.Controls;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Security.RightsManagement;

//using System.Windows.Threading;

//using System.Windows.Threading;

//using System.Windows.Threading;

//using System.Windows.Threading;

//using System.Windows.Threading;

//using System.Windows.Threading;



namespace LIT.Old_LIT

{

    /// <summary>

    /// Interaction logic for ItineraryWindow.xaml

    /// </summary>


    public partial class ItineraryWindow : UserControl

    {

        SQLDataAccessLayer.DAL.ItineraryDAL objitdal = new SQLDataAccessLayer.DAL.ItineraryDAL();

        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();

        CommonAndCalcuation CommonValues = new CommonAndCalcuation();

        DBConnectionEF DBconnEF = new DBConnectionEF();

        public string Imagepdfhtmlfolderpath
        {
            get; set;
        }


        //  SQLDataAccessLayer.DAL.Errorlog erobj = new SQLDataAccessLayer.DAL.Errorlog();

        // DataSet dsAgent= new DataSet();

        // DataSet dsSource=new DataSet(); 

        // DataSet dsStatus= new DataSet();    

        // DataSet dsCity= new DataSet();  

        // DataSet dsAgentAssignedto= new DataSet();

        List<Userdetails> ListUserdet = new List<Userdetails>();

        DataSet dsFolder = new DataSet();

        string loginusername = string.Empty;

        string loginuserid = string.Empty;

        string loginuserrole = string.Empty;

        string recordmode = string.Empty;

        string itineraryid = string.Empty;

        DataSet dsItineraryRetr = new DataSet();

        string Foldersettingurl = string.Empty;

        Errorlog errobj = new Errorlog();

        public bool DatasaveresultItinerary = false;



        //public event RoutedEventHandler ParentSelectedValue;



        private MainWindow _parentWindow;



        List<CommonValueList> ListofAgent = new List<CommonValueList>();

        List<CommonValueList> ListofSource = new List<CommonValueList>();

        List<CommonValueList> ListofStatus = new List<CommonValueList>();

        List<CommonValueCountrycity> ListofCity = new List<CommonValueCountrycity>();

        List<BookingItems> ListofBookingItems = new List<BookingItems>();
        List<BookingItems> ListofBookingItemsTotal = new List<BookingItems>();

        List<Pickupdroplocation> ListofPDLoction = new List<Pickupdroplocation>();
        List<BkRequestStatus> ListofRequestStatus = new List<BkRequestStatus>();
        List<TourList> ListofTour = new List<TourList>();
        //List<Currencydetails> ListofCurrencyServiceidwise = new List<Currencydetails>();



        string rootfilepathItinval = string.Empty;



        public ItineraryClientTabViewModel clientTabViewModel { get; set; }        
        public  Itinerary_ClientTabPassengerViewModel CTPassengerVMitn { get; set; }

        public ItineraryCommentsTabViewModel tabViewModel { get; set; }

        public string itincurformat = string.Empty;

        string Changecurformat = string.Empty;

        public ItineraryWindow DataContext { get; }
        public ItineraryWindow()

        {

            InitializeComponent();
            this.DataContext = this;
            // loadcmbvalues();

        }

        private static bool codeExecuted = false;
        public ItineraryWindow(string username, MainWindow ParentWindow, string itinearyvalueid = "", string rootfilepathItin = "")     //object sender)

        {

            //if(sender!=null)

            //{



            //if(sender.GetType().Name.ToString().ToUpper()=="BUTTON")

            //{



            //    InitializeComponent();

            //    ButtonAutomationPeer peer = new ButtonAutomationPeer(BtnSave);

            //    IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;

            //    invokeProv.Invoke();



            //}

            //else

            //{



            InitializeComponent();
            this.DataContext = this;

            hidesterlingcolumn();
            Imagepdfhtmlfolderpath = DBconnEF.GetImagePDFTHtmlFolderPath();

            loginusername = username.Trim();

            _parentWindow = ParentWindow;

            rootfilepathItinval = (!string.IsNullOrEmpty(rootfilepathItin)) ? rootfilepathItin : "";

            //TxtItineraryName.Focus();

            loadcmbvalues();

            TxtEnteredBy.Text = loginusername + ", " + DateTime.Today.ToShortDateString().ToString();

            TxtEnteredBy.IsReadOnly = true;

            TxtDateCreated.SelectedDate = DateTime.Now;

            TxtItinerarystartdt.SelectedDate = DateTime.Now;

            TxtCustomerID.IsEnabled = false;



            this.clientTabViewModel = new ItineraryClientTabViewModel();

            this.tabViewModel = new ItineraryCommentsTabViewModel();
            this.CTPassengerVMitn=new Itinerary_ClientTabPassengerViewModel();

            loginuserid = loadDropDownListValues.Currentuseridinfo(loginusername);

            if (!string.IsNullOrEmpty(itinearyvalueid))

            {

                recordmode = "Edit";

                hdnmode.Text = "Edit";

                hdnitineraryid.Text = itinearyvalueid.ToString();

                TxtItineraryID.Text = itinearyvalueid.ToString();



                this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                clientcontrol.DataContext = this.clientTabViewModel;

                ReteriveItinearyValues(itinearyvalueid);

                if (this.clientTabViewModel.Groupoption == false && this.clientTabViewModel.Individualoption == false)
                {
                    this.clientTabViewModel.Groupoption = true;
                    this.clientTabViewModel.RefundPaymentTotalAmountcheck = true; 
                }


            }

            else

            {

                recordmode = "Save";

                hdnmode.Text = "Save";

                TxtItineraryID.Text = (Guid.NewGuid()).ToString();

                hdnitineraryid.Text = TxtItineraryID.Text;

                string autoid = loadDropDownListValues.LoadItineraryAutoNumber();//objitdal.LoadSingleValues();

                if (!string.IsNullOrEmpty(autoid))

                {

                    hdnitineraryAutono.Text = autoid;

                    TxtItineraryID.Text = autoid;

                }
                this.clientTabViewModel.Groupoption = true;
                this.clientTabViewModel.RefundPaymentTotalAmountcheck = true;
                clientcontrol.DataContext = this.clientTabViewModel;

            }

            //}



            //}



            TxtblItinNamewithpath.Text = rootfilepathItinval.Trim() + TxtItineraryName.Text.Trim();



            RBConfirmedPaid.Checked += (sender, e) => PrintPDf();

            RBQuotation.Checked += (sender, e) => PrintPDf();

            RBConfirmedDeposit.Checked += (sender, e) => PrintPDf();


           // RBCoachbkg.Checked += (sender, e) => PrintCoachBooking();


            //if (!codeExecuted)
            //{
            //    NotificationService notificationService = (NotificationService)ContainerLocator.Container.Resolve(typeof(NotificationService));
            //    notificationService.ClearAllNotifications();
            //    codeExecuted = true;
            //}




        }



        public void loadcmbvalues()

        {



            try

            {

                ListUserdet = loadDropDownListValues.LoadUserDropDownlist("User");

                if (ListUserdet != null && ListUserdet.Count > 0)

                {

                    CmbAgentAssignedTo.ItemsSource = ListUserdet;

                    CmbAgentAssignedTo.SelectedValuePath = "Userid";

                    CmbAgentAssignedTo.DisplayMemberPath = "Fullname";



                    Guid Userid = Guid.Empty;

                    Userid = ListUserdet.Where(x => x.UserName.ToString().ToUpper().Trim() == loginusername.ToString().ToUpper().Trim()).FirstOrDefault().Userid;

                    CmbAgentAssignedTo.SelectedValue = Userid.ToString();

                    loginuserid = Userid.ToString();

                    //foreach (DataRow dr in dsAgentAssignedto.Tables[0].Rows)

                    //{

                    //    if (dr["UserName"].ToString().ToUpper().Trim() == loginusername.ToString().ToUpper().Trim())

                    //    {

                    // loginuserid= dr["UserId"].ToString();

                    // CmbAgentAssignedTo.SelectedValue = dr["UserId"].ToString();

                    //    }

                    //}





                }





                ListofAgent = loadDropDownListValues.LoadCommonValuesWithDefault("Agent");

                if (ListofAgent != null && ListofAgent.Count > 0)

                {

                    CmbAgent.ItemsSource = ListofAgent;

                    CmbAgent.SelectedValuePath = "ValueField";

                    CmbAgent.DisplayMemberPath = "TextField";

                    if (ListofAgent.Where(x => x.IsDefault == true).FirstOrDefault() != null)

                    {

                        CmbAgent.SelectedValue = ListofAgent.Where(x => x.IsDefault == true).FirstOrDefault().ValueField;

                    }

                    //foreach (var item in ListofAgent)

                    //{

                    //    if (item.IsDefault.ToString().ToUpper()=="TRUE")

                    //    {

                    // CmbAgent.SelectedValue = item.ValueField.ToString();

                    //    }

                    //}        

                }



                ListofStatus = loadDropDownListValues.LoadCommonValues("Itinerary Status");

                if (ListofStatus != null && ListofStatus.Count > 0)

                {

                    CmbStatus.ItemsSource = ListofStatus;

                    CmbStatus.SelectedValuePath = "ValueField";

                    CmbStatus.DisplayMemberPath = "TextField";

                }



                ListofSource = loadDropDownListValues.LoadCommonValues("Itinerary Source");

                if (ListofSource != null && ListofSource.Count > 0)

                {

                    CmbSource.ItemsSource = ListofSource;

                    CmbSource.SelectedValuePath = "ValueField";

                    CmbSource.DisplayMemberPath = "TextField";

                }

                ListofTour = loadDropDownListValues.LoadTourList();

                if (ListofTour != null && ListofTour.Count > 0)

                {

                    CmbTourlist.ItemsSource = ListofTour;

                    CmbTourlist.SelectedValuePath = "Tourlistid";

                    CmbTourlist.DisplayMemberPath = "Tourlistname";
                    if (ListofTour.Where(x => x.Tourlistname.ToLower() == "ireland").FirstOrDefault() != null)
                    {
                        CmbTourlist.SelectedValue = ListofTour.Where(x => x.Tourlistname.ToLower() == "ireland").FirstOrDefault().Tourlistid;
                    }

                }
                



                CommonValueCountrycity commonValueCountrycity = new CommonValueCountrycity();

                ListofCity = loadDropDownListValues.LoadCommonValuesCity("City", commonValueCountrycity);

                if (ListofCity != null && ListofCity.Count > 0)

                {

                    CmbArrivalCity.ItemsSource = ListofCity;

                    CmbArrivalCity.SelectedValuePath = "CityId";

                    CmbArrivalCity.DisplayMemberPath = "CityName";



                    CmbDepartureCity.ItemsSource = ListofCity;

                    CmbDepartureCity.SelectedValuePath = "CityId";

                    CmbDepartureCity.DisplayMemberPath = "CityName";

                }



                Foldersettingurl = loadDropDownListValues.LoadFolderName("ItineraryFolder");





                /* dsFolder = objitdal.LoadCommonValues("ItineraryFolder");

                if (dsFolder != null && dsFolder.Tables.Count > 0)

                {

                    if (dsFolder.Tables[0].Rows.Count > 0)

                    Foldersettingurl = dsFolder.Tables[0].Rows[0]["FieldValue"].ToString();

                }*/





                ListofRequestStatus = loadDropDownListValues.LoadRequestStatus();

                if (ListofRequestStatus != null && ListofRequestStatus.Count > 0)

                {

                    CmbRequestStatusitin.ItemsSource = ListofRequestStatus;

                    CmbRequestStatusitin.SelectedValuePath = "RequestStatusId";

                    CmbRequestStatusitin.DisplayMemberPath = "RequestStatusName";

                }


                ListofPDLoction = loadDropDownListValues.Loadpickupdroplocation();

                if (ListofPDLoction != null && ListofPDLoction.Count > 0)

                {

                    CmbDroplocation.ItemsSource = ListofPDLoction;

                    CmbDroplocation.SelectedValuePath = "PickupDropLocationId";

                    CmbDroplocation.DisplayMemberPath = "LocationName";


                    CmbPickuplocation.ItemsSource = ListofPDLoction;

                    CmbPickuplocation.SelectedValuePath = "PickupDropLocationId";

                    CmbPickuplocation.DisplayMemberPath = "LocationName";

                }

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }



        public void saveupdateItinerary()

        {

            try

            {

                ItineraryModels objitm = new ItineraryModels();

                objitm.ItineraryID = hdnitineraryid.Text.Trim();

                objitm.ItineraryAutoId = Convert.ToInt64(hdnitineraryAutono.Text.Trim());

                objitm.ItineraryName = TxtItineraryName.Text.Trim();

                objitm.DisplayName = TxtDisplayName.Text.Trim();

                objitm.Email = TxtEmail.Text.Trim();

                objitm.Phone = TxtPhone.Text.Trim();

                objitm.ItineraryStartDate = (TxtItinerarystartdt.SelectedDate == null) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(TxtItinerarystartdt.SelectedDate.ToString());

                objitm.ItineraryEndDate = (TxtItineraryEndDt.SelectedDate == null) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(TxtItineraryEndDt.SelectedDate.ToString());

                objitm.ArrivalCity = (CmbArrivalCity.SelectedValue == null) ? Guid.Empty.ToString() : CmbArrivalCity.SelectedValue.ToString();

                objitm.ArrivalFlight = TxtArrivalFlight.Text.Trim();

                objitm.DepartureCity = (CmbDepartureCity.SelectedValue == null) ? Guid.Empty.ToString() : CmbDepartureCity.SelectedValue.ToString();

                objitm.DepartureFlight = TxtDepartureFlight.Text.Trim();

                objitm.Agent = (CmbAgent.SelectedValue == null) ? Guid.Empty.ToString() : CmbAgent.SelectedValue.ToString();

                objitm.AgentAssignedTo = (CmbAgentAssignedTo.SelectedValue == null) ? Guid.Empty.ToString() : CmbAgentAssignedTo.SelectedValue.ToString();

                objitm.Enteredby = TxtEnteredBy.Text.Trim();

                objitm.Status = (CmbStatus.SelectedValue == null) ? Guid.Empty.ToString() : CmbStatus.SelectedValue.ToString();

                objitm.Source = (CmbSource.SelectedValue == null) ? Guid.Empty.ToString() : CmbSource.SelectedValue.ToString();

                objitm.Customerid = Guid.Empty.ToString();

                objitm.Bookingid = Guid.Empty.ToString();

                objitm.Supplierid = Guid.Empty.ToString();

                objitm.Clientsid = Guid.Empty.ToString();
                
                objitm.TourlistID = (CmbTourlist.SelectedValue==null)?Guid.Empty.ToString(): CmbTourlist.SelectedValue.ToString();

               objitm.ClientFirstname=TxtClientFirstName.Text.Trim();
                objitm.ClientLastname = TxtClientLastname.Text.Trim();
                objitm.ClientDisplayname = TxtClientdisplayname.Text.Trim();

                objitm.DateCreated = (TxtDateCreated.SelectedDate == null) ? Convert.ToDateTime("1900-01-01") : Convert.ToDateTime(TxtDateCreated.SelectedDate.ToString());

                string rootpath = string.Empty;

                if (!string.IsNullOrEmpty(rootfilepathItinval.Trim()))

                {

                    if (rootfilepathItinval.Trim().ToLower() == "itineraries\\")

                    { rootpath = rootfilepathItinval; }

                    else if (rootfilepathItinval.Trim().ToLower() == "itineraries\\new itinerary")

                    { rootpath = rootfilepathItinval.Replace("New Itinerary", ""); }

                    else

                    {

                        rootpath = rootfilepathItinval.Remove(rootfilepathItinval.LastIndexOf("\\"));

                    }

                }

                objitm.ItineraryFolderPath = rootpath;

                objitm.InclusionNotes = TxtInclusion.Text;

                string purpose = string.Empty;

                if (recordmode.ToString().ToLower() == "edit")

                {

                    purpose = "I";

                    objitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    objitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    objitm.DeletedBy = Guid.Empty.ToString();

                }

                else if (recordmode.ToString().ToLower() == "save")

                {

                    purpose = "I";

                    objitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    objitm.ModifiedBy = Guid.Empty.ToString();

                    objitm.DeletedBy = Guid.Empty.ToString();

                }

                else if (recordmode.ToString().ToLower() == "delete")

                {

                    purpose = "D";

                    objitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    objitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    objitm.IsDeleted = true;

                    objitm.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                }

                string objret = objitdal.SaveUpdateItinerary(purpose, objitm);

                if (!string.IsNullOrEmpty(objret))
                {

                    if (objret.ToString().ToLower() == "1") ;

                }

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }



        }



        public void BtnSave_Click(object sender, RoutedEventArgs e)

        {

            //try

            //{

            //MessageBox.Show("test save");

            //ItineraryWindow iw = this;

            //RoutedEventHandler evt = ParentSelectedValue;

            //if(evt != null)

            //{

            //    evt(sender, e);

            //}



            string valmsg = validation();
            string valmsgPax = this.clientTabViewModel.CTPaxinfoCommand.ExecuteValidation();
            string valmsgPass = this.clientTabViewModel.CTPassengerCommand.ValidationforPassenger("save");

            if (valmsg == string.Empty && valmsgPax == string.Empty && valmsgPass == string.Empty)
            {

                saveupdateItinerary();
                saveupdateBookingItems();
                saveupdateTotalDetails();
               
                //Client tab save code

                this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                this.clientTabViewModel.Loginuserid = loginuserid;
                this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text))?Convert.ToDecimal(TxtFinalsell.Text):0;
                this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

                this.clientTabViewModel.CTPaxinfoCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTPassengerCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTPaymentCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTRoomTypeCommand.SaveCommand.Execute();

                this.tabViewModel.CommentsTabCommands.SaveCommand.Execute();

                string rootpathval = string.Empty;

                if (rootfilepathItinval.Trim().ToLower() == "itineraries\\")
                { rootpathval = rootfilepathItinval; }

                else if (rootfilepathItinval.Trim().ToLower() == "itineraries\\new itinerary")
                { rootpathval = rootfilepathItinval.Replace("New Itinerary", ""); }
                else
                {
                    rootpathval = rootfilepathItinval;//.Remove(rootfilepathItinval.LastIndexOf("\\"));
                }

                TxtblItinNamewithpath.Text = rootpathval + TxtItineraryName.Text.Trim();

                TreeviewAccordion tracc = new TreeviewAccordion();
                NodeViewModel snvm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();

                if (snvm != null)
                {
                    snvm.IsSelectedItinerary = true;
                    snvm.IsExpandedItinerary = true;
                    if (snvm.Children.Where(x => x.Id == hdnitineraryid.Text).ToList().Count > 0)
                    {
                        snvm.Children.Where(x => x.Id == hdnitineraryid.Text).FirstOrDefault().IsSelectedItinerary = true;
                        snvm.Children.Where(x => x.Id == hdnitineraryid.Text).FirstOrDefault().IsExpandedItinerary = true;
                    }
                }
                _parentWindow.trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
                _parentWindow.trviewcontact.ItemsSource = tracc.ContactTreeViewModel.SupplierItems;

                BookingItemsitin = null; 
                ReteriveBookingItems();

                // MainWindow mw=new MainWindow(loginusername);



                //int cnt=tr.Tree.Items.Count();

                //ViewModelBase vmb = new ViewModelBase();

                //    vmb.OnPropertyChangedNotify("Name");

                //this.NavigationService.Refresh();

                // mw.Show();

                MessageBox.Show("Itinerary saved successfully");

            }

            else

            {

                if (!string.IsNullOrEmpty(valmsg))
                {
                    MessageBox.Show(valmsg);
                }
                else if (!string.IsNullOrEmpty(valmsgPax))
                {
                    MessageBox.Show(valmsgPax);
                }
                else if (!string.IsNullOrEmpty(valmsgPass))
                {
                    MessageBox.Show(valmsgPass);
                }

            }

            // }

            //catch (Exception ex)

            //{

            //    erobj.writeLog(ex.Message.ToString());

            //}



        }





        public void BtnSaveandclose_Click(object sender, RoutedEventArgs e)

        {

            //try

            //{

            string valmsg = validation();
            string valmsgPax=this.clientTabViewModel.CTPaxinfoCommand.ExecuteValidation();
            string valmsgPass = this.clientTabViewModel.CTPassengerCommand.ValidationforPassenger("save");

            if (valmsg == string.Empty && valmsgPax == string.Empty && valmsgPass==string.Empty)
            {

                saveupdateItinerary();
                saveupdateBookingItems();
                saveupdateTotalDetails();
                
                //Client tab save code

                this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                this.clientTabViewModel.Loginuserid = loginuserid;

                this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;               

                this.clientTabViewModel.CTPaxinfoCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTPassengerCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTPaymentCommand.SaveCommand.Execute();

                this.clientTabViewModel.CTRoomTypeCommand.SaveCommand.Execute();

                this.tabViewModel.CommentsTabCommands.SaveCommand.Execute();


                DatasaveresultItinerary = true;

                TreeviewAccordion tracc = new TreeviewAccordion();

                NodeViewModel snvm = tracc.ItineraryTreeViewModelTr.Items.FirstOrDefault();

                if (snvm != null)
                {
                    snvm.IsSelectedItinerary = true;
                    snvm.IsExpandedItinerary = true;
                    if (snvm.Children.Where(x => x.Id == hdnitineraryid.Text).ToList().Count > 0)
                    {
                        snvm.Children.Where(x => x.Id == hdnitineraryid.Text).FirstOrDefault().IsSelectedItinerary = true;
                        snvm.Children.Where(x => x.Id == hdnitineraryid.Text).FirstOrDefault().IsExpandedItinerary = true;
                    }
                }

                _parentWindow.trview.ItemsSource = tracc.ItineraryTreeViewModelTr.Items;
                _parentWindow.trviewcontact.ItemsSource = tracc.ContactTreeViewModel.SupplierItems;
                //  BookingItemsitin = null;
                // ReteriveBookingItems();

            }

            else
            {
                if (!string.IsNullOrEmpty(valmsg))
                {
                    MessageBox.Show(valmsg);
                }
                else if (!string.IsNullOrEmpty(valmsgPax))
                {
                    MessageBox.Show(valmsgPax);
                }
                else if (!string.IsNullOrEmpty(valmsgPass))
                {
                    MessageBox.Show(valmsgPass);
                }
                

            }

            //}

            //catch (Exception ex)

            //{

            //    erobj.writeLog(ex.Message.ToString());

            //}

        }

        public string validation()

        {



            string ret = string.Empty;

            #region "Validation for Itinerary tab start"

            if (TxtItineraryName.Text.Length == 0)

            {

                ret = "Please provide the Itinerary Name";

                TxtItineraryName.Focus();

                return ret;

            }

            //else if (TxtDisplayName.Text.Length == 0)

            //{

            //    ret = "Please provide the Display Name";

            //    TxtDisplayName.Focus();

            //}

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

            if (TxtItinerarystartdt.SelectedDate == null)
            {
                ret = "Please provide the Itinerary Start date";
                TxtItinerarystartdt.Focus();
                return ret;
            }
            if (TxtItineraryEndDt.SelectedDate == null)
            {
                ret = "Please provide the Itinerary end date";
                TxtItineraryEndDt.Focus();
                return ret;
            }

            if (string.IsNullOrEmpty(TxtClientFirstName.Text))
            {
                ret = "Please provide the client first name";
                TxtClientFirstName.Focus();
                return ret;
            }
            if (string.IsNullOrEmpty(TxtClientLastname.Text))
            {                
                ret="Please provide the client last name";
                TxtClientLastname.Focus();
                return ret;
            }
            //if (TxtPhone.Text.Length > 0)

            //{

            //    if ((!Regex.IsMatch(TxtPhone.Text, "^[0-9.\\-]+$")))

            //    {

            // ret = "Please provide a valid Phone";

            // TxtPhone.Focus();

            // return ret;

            //    }

            //}





            if (TxtItineraryEndDt.Text.Length > 0 && TxtItinerarystartdt.Text.Length > 0)

            {

                if (Convert.ToDateTime(TxtItinerarystartdt.Text) > Convert.ToDateTime(TxtItineraryEndDt.Text))

                {

                    ret = "The Itinerary End Date needs to be later than the Itinerary Start Date. Please change the dates as needed";



                    TxtItinerarystartdt.Focus();

                    return ret;

                }

            }


            if (TxtGrsAdjmarkup.Text.Length>0 &&  ValidationClass.IsNumericDotwith2decimal(TxtGrsAdjmarkup.Text) == false)
            {
                ret="Gross Adjustment Markup allow only numeric";                
                TxtGrsAdjmarkup.Text = string.Empty;
                TxtGrsAdjmarkup.Focus();
                return ret;
            }
            if (txtmarginpercenOverrideall.Text.Length>0 && ValidationClass.IsNumericDotwith2decimal(txtmarginpercenOverrideall.Text) == false)
            {
                ret = "Margin Override all allow only numeric";                
                txtmarginpercenOverrideall.Text = string.Empty;
                txtmarginpercenOverrideall.Focus();
                return ret;
            }
            if (TxtGrsAdjFinalOverrides.Text.Length > 0 && ValidationClass.IsNumericDotwith2decimal(TxtGrsAdjFinalOverrides.Text) == false)
            {
                ret = "Gross Adjustment Final Override allow only numeric";                
                TxtGrsAdjFinalOverrides.Text = string.Empty;
                TxtGrsAdjFinalOverrides.Focus();
                return ret;
            }


            if (CmbStatus.SelectedValue != null)

            {



                loginuserrole = loadDropDownListValues.CurrentUserRole(loginusername);

                if (!string.IsNullOrEmpty(loginuserrole))

                {

                    if (loginuserrole.ToLower() == "travel advisor")

                    {

                        string existingstatusid = DBconnEF.ExistingItineraryStatus(hdnitineraryid.Text);

                        string currstatusid = string.Empty;

                        currstatusid = (((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem) != null) ? ((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).ValueField.ToString() : string.Empty;



                        if ((!string.IsNullOrEmpty(existingstatusid)) && (!string.IsNullOrEmpty(currstatusid)))

                        {

                            if (existingstatusid == currstatusid)

                            {

                            }

                            else

                            {

                                if (((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).TextField.ToLower() == "confirmed deposit" ||

                                    ((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).TextField.ToLower() == "confirmed - paid in full")

                                {

                                    ret = "Your current permission level does not allow change the Itinerary status";

                                    CmbStatus.Focus();

                                    return ret;

                                }

                            }

                        }

                        else

                        {

                            if (((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).TextField.ToLower() == "confirmed deposit" ||

                             ((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).TextField.ToLower() == "confirmed - paid in full")

                            {

                                ret = "Your current permission level does not allow change the Itinerary status";

                                CmbStatus.Focus();

                                return ret;

                            }

                        }



                    }

                }

            }

            return ret;

            #endregion "Validation for Itinerary tab end"

        }



        public void ReteriveItinearyValues(string ItinearyIdval)

        {

            try

            {

                if (ItinearyIdval != "")

                {

                    dsItineraryRetr = null;

                    dsItineraryRetr = objitdal.ItineraryRetrive("FIR", Guid.Parse(ItinearyIdval));

                    if (dsItineraryRetr != null && dsItineraryRetr.Tables.Count > 0)

                    {

                        if (dsItineraryRetr.Tables[0].Rows.Count > 0)

                        {

                            hdnitineraryid.Text = dsItineraryRetr.Tables[0].Rows[0]["ItineraryID"].ToString();

                            TxtItineraryName.Text = dsItineraryRetr.Tables[0].Rows[0]["ItineraryName"].ToString();

                            TxtDisplayName.Text = dsItineraryRetr.Tables[0].Rows[0]["DisplayName"].ToString();

                            TxtEmail.Text = dsItineraryRetr.Tables[0].Rows[0]["Email"].ToString();

                            TxtPhone.Text = dsItineraryRetr.Tables[0].Rows[0]["Phone"].ToString();

                            hdnitineraryAutono.Text = dsItineraryRetr.Tables[0].Rows[0]["ItineraryAutoId"].ToString();

                            TxtItineraryID.Text = dsItineraryRetr.Tables[0].Rows[0]["ItineraryAutoId"].ToString();

                            DateTime stdt = (dsItineraryRetr.Tables[0].Rows[0]["ItineraryStartDate"].ToString()

                         != "") ? Convert.ToDateTime(dsItineraryRetr.Tables[0].Rows[0]["ItineraryStartDate"].ToString()) : DateTime.Now;

                            DateTime enddt = (dsItineraryRetr.Tables[0].Rows[0]["ItineraryEndDate"].ToString()

                         != "") ? Convert.ToDateTime(dsItineraryRetr.Tables[0].Rows[0]["ItineraryEndDate"].ToString()) : DateTime.Now;

                            string strstdt = stdt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                            string strenddt = enddt.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);



                            TxtItinerarystartdt.SelectedDate = (strstdt == "01/01/1900") ? null : stdt;

                            TxtItineraryEndDt.SelectedDate = (strenddt == "01/01/1900") ? null : enddt;

                            TxtDateCreated.SelectedDate = (dsItineraryRetr.Tables[0].Rows[0]["Datecreated"].ToString() != "") ? Convert.ToDateTime(dsItineraryRetr.Tables[0].Rows[0]["Datecreated"].ToString()) : null;

                            CmbArrivalCity.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["ArrivalCity"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["ArrivalCity"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["ArrivalCity"].ToString();

                            TxtArrivalFlight.Text = dsItineraryRetr.Tables[0].Rows[0]["ArrivalFlight"].ToString();

                            CmbDepartureCity.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["DepartureCity"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["DepartureCity"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["DepartureCity"].ToString();



                            TxtDepartureFlight.Text = dsItineraryRetr.Tables[0].Rows[0]["DepartureFlight"].ToString();

                            CmbAgent.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["Agent"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["Agent"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["Agent"].ToString();



                            CmbAgentAssignedTo.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["AgentAssignedTo"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["AgentAssignedTo"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["AgentAssignedTo"].ToString();

                            TxtEnteredBy.Text = dsItineraryRetr.Tables[0].Rows[0]["Enteredby"].ToString();

                            CmbStatus.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["Status"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["Status"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["Status"].ToString();

                            CmbSource.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["Source"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["Source"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["Source"].ToString();

                            //objitm.Customerid = dsItineraryRetr.Tables[0].Rows[0]["Customerid"].ToString();

                            //objitm.Bookingid = dsItineraryRetr.Tables[0].Rows[0]["Bookingid"].ToString();

                            //objitm.Supplierid = dsItineraryRetr.Tables[0].Rows[0]["Supplierid"].ToString();

                            //objitm.Clientsid = dsItineraryRetr.Tables[0].Rows[0]["Clientsid"].ToString();

                            TxtItineraryFolderPath.Text = dsItineraryRetr.Tables[0].Rows[0]["ItineraryFolderPath"].ToString();

                            TxtInclusion.Text = dsItineraryRetr.Tables[0].Rows[0]["InclusionNotes"].ToString();

                            CmbTourlist.SelectedValue = (dsItineraryRetr.Tables[0].Rows[0]["TourlistID"].ToString() == "" || dsItineraryRetr.Tables[0].Rows[0]["TourlistID"].ToString() == "00000000-0000-0000-0000-000000000000") ? null : dsItineraryRetr.Tables[0].Rows[0]["TourlistID"].ToString();

                            //ClientFirstname,ClientLastname,ClientDisplayname
                            TxtClientFirstName.Text = dsItineraryRetr.Tables[0].Rows[0]["ClientFirstname"].ToString();
                            TxtClientLastname.Text = dsItineraryRetr.Tables[0].Rows[0]["ClientLastname"].ToString();
                            TxtClientdisplayname.Text = dsItineraryRetr.Tables[0].Rows[0]["ClientDisplayname"].ToString();
                            //objitm.CreatedBy = dsItineraryRetr.Tables[0].Rows[0]["CreatedBy"].ToString();

                            //objitm.IsDeleted = dsItineraryRetr.Tables[0].Rows[0]["IsDeleted"].ToString();



                            ReteriveBookingItems();



                            //Client tab save code

                            //this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                            //this.clientTabViewModel.Loginuserid = loginuserid;

                            //this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                            //this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

                            //this.clientTabViewModel.CTPaxinfoCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTPaymentCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTRoomTypeCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTPassengerCommand.RetrieveCommand.Execute();



                        }



                    }

                }

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

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



        #region "Booking Tab"

        /* Booing Tab Start*/



        private ObservableCollection<BookingItems> _BookingItemsitin;

        public ObservableCollection<BookingItems> BookingItemsitin

        {

            get { return _BookingItemsitin ?? (_BookingItemsitin = new ObservableCollection<BookingItems>()); }

            set

            {

                _BookingItemsitin = value;

            }

        }



        private ObservableCollection<BookingNote> _BookingItemsNotes;

        public ObservableCollection<BookingNote> BookingItemsNotes

        {

            get { return _BookingItemsNotes ?? (_BookingItemsNotes = new ObservableCollection<BookingNote>()); }

            set

            {

                _BookingItemsNotes = value;

            }

        }

        private void BtnBookingAdd_Click(object sender, RoutedEventArgs e)
        {
            if(TxtItinerarystartdt.SelectedDate==null)
            {
                MessageBox.Show("Please provide the Itinerary Start date");
                TxtItinerarystartdt.Focus();
                return;
            }
            if(TxtItineraryEndDt.SelectedDate==null)
            {
                MessageBox.Show("Please provide the Itinerary end date");
                TxtItineraryEndDt.Focus();
                return;
            }
            if (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text))
            {
                //string messagestr = "Warning : Itinerary Price Override exists (" + itincurformat + " " + TxtGrsAdjFinalOverrides.Text + ") and should be updated to reflect this change\r\n\r\nDo you want to remove override now?\r\n";
                string messagestr = "Do you really want to edit the booking record, as the Itinerary Price Override is already available?\r\n\r\nWarning : Itinerary Final Override Price is (" + itincurformat + " " + TxtGrsAdjFinalOverrides.Text + ") In case there are any changes to the record, the price will be updated based on the change with the same margin\r\n";

                MessageBoxResult mesbox = System.Windows.MessageBox.Show(messagestr, "Message", System.Windows.MessageBoxButton.OKCancel);

                if (mesbox == MessageBoxResult.OK)
                {
                    TxtGrsAdjFinalOverrides.Text = string.Empty;
                    TxtFinalsell.Text = string.Empty;
                    BookingSupplierSearch wnbkadd = new BookingSupplierSearch(loginusername, this);

                    wnbkadd.ShowDialog();
                }
            }
            else {
                BookingSupplierSearch wnbkadd = new BookingSupplierSearch(loginusername, this);

                wnbkadd.ShowDialog();
            }



        }



        private void BtnBookingEdit_Click(object sender, RoutedEventArgs e)

        {

            BookingItems objBI = dgItinBooking.SelectedItem as BookingItems;

            if (objBI != null)

            {

                Bookingedit wnbked = new Bookingedit(loginusername, this, objBI);

                wnbked.ShowDialog();

            }

            else

            {

                System.Windows.MessageBox.Show("Please select booking item");

                return;

            }



        }





        private void saveupdateBookingItems()

        {

            try

            {

                if (dgItinBooking.Items.Count > 0)

                {

                    int cntrowsuccss = 0;

                    foreach (BookingItems bkit in dgItinBooking.Items)

                    {

                        BookingItems objBitm = new BookingItems();

                        objBitm.ItineraryID = hdnitineraryid.Text.Trim();

                        objBitm.BookingID = bkit.BookingID;

                        objBitm.BookingName = bkit.BookingName;

                        objBitm.BookingAutoID = bkit.BookingAutoID;

                        objBitm.City = bkit.City;

                        objBitm.Comments = bkit.Comments;

                        objBitm.Day = bkit.Day;

                        objBitm.Enddate = bkit.Enddate;

                        objBitm.EndTime = bkit.EndTime;

                        objBitm.ExchRate = bkit.ExchRate;

                        objBitm.GrossAdj = bkit.GrossAdj;

                        objBitm.Grossfinal = bkit.Grossfinal;

                        objBitm.Grosstotal = bkit.Grosstotal;

                        objBitm.Grossunit = bkit.Grossunit;

                        objBitm.Invoiced = bkit.Invoiced;

                        objBitm.ItemDescription = bkit.ItemDescription;

                        objBitm.ItinCurrency = bkit.ItinCurrency;

                        objBitm.Netfinal = bkit.Netfinal;

                        objBitm.Nettotal = bkit.Nettotal;

                        objBitm.Netunit = bkit.Netunit;

                        objBitm.NtsDays = bkit.NtsDays;

                        objBitm.PaymentDueDate = bkit.PaymentDueDate;

                        objBitm.Ref = bkit.Ref;

                        objBitm.Region = bkit.Region;

                        objBitm.ServiceName = bkit.ServiceName;

                        objBitm.StartDate = bkit.StartDate;

                        objBitm.StartTime = bkit.StartTime;

                        objBitm.Status = (((SQLDataAccessLayer.Models.BkRequestStatus)bkit.SelectedItemRequstStatus) != null) ? ((SQLDataAccessLayer.Models.BkRequestStatus)bkit.SelectedItemRequstStatus).RequestStatusID : Guid.Empty.ToString();

                        objBitm.Pickuplocation = (((SQLDataAccessLayer.Models.Pickupdroplocation)bkit.SelectedItemPickuplocation) != null) ? ((SQLDataAccessLayer.Models.Pickupdroplocation)bkit.SelectedItemPickuplocation).PickupDropLocationId : Guid.Empty.ToString();
                        
                        objBitm.Droplocation = (((SQLDataAccessLayer.Models.Pickupdroplocation)bkit.SelectedItemDroplocation) != null) ? ((SQLDataAccessLayer.Models.Pickupdroplocation)bkit.SelectedItemDroplocation).PickupDropLocationId : Guid.Empty.ToString();

                        objBitm.Type = bkit.Type;



                        objBitm.ItemDescription = bkit.ItemDescription;

                        objBitm.Qty = bkit.Qty;

                        objBitm.DaysValid = bkit.DaysValid;

                        objBitm.AgentCommission = bkit.AgentCommission;

                        objBitm.AgentCommissionPercentage = bkit.AgentCommissionPercentage;

                        objBitm.BkgCurrencyName = bkit.BkgCurrencyName;

                        objBitm.SupplierID = (!string.IsNullOrEmpty(bkit.SupplierID) ? Guid.Parse(bkit.SupplierID) : Guid.Empty).ToString();

                        objBitm.ServiceTypeID = (!string.IsNullOrEmpty(bkit.ServiceTypeID) ? Guid.Parse(bkit.ServiceTypeID) : Guid.Empty).ToString();

                        objBitm.ServiceID = (!string.IsNullOrEmpty(bkit.ServiceID) ? Guid.Parse(bkit.ServiceID) : Guid.Empty).ToString();

                        objBitm.BkgCurrencyID = (!string.IsNullOrEmpty(bkit.BkgCurrencyID) ? Guid.Parse(bkit.BkgCurrencyID) : Guid.Empty).ToString();

                        objBitm.PricingOptionId = (!string.IsNullOrEmpty(bkit.PricingOptionId) ? Guid.Parse(bkit.PricingOptionId) : Guid.Empty).ToString();

                        objBitm.PricingRateID = (!string.IsNullOrEmpty(bkit.PricingRateID) ? Guid.Parse(bkit.PricingRateID) : Guid.Empty).ToString();

                        objBitm.RegionID = (!string.IsNullOrEmpty(bkit.RegionID) ? Guid.Parse(bkit.RegionID) : Guid.Empty).ToString();

                        objBitm.CityID = (!string.IsNullOrEmpty(bkit.CityID) ? Guid.Parse(bkit.CityID) : Guid.Empty).ToString();

                        objBitm.CustomCode = bkit.CustomCode;

                        objBitm.CommissionPercentage = bkit.CommissionPercentage;

                        objBitm.MarkupPercentage = bkit.MarkupPercentage;

                        objBitm.PaymentTerms = bkit.PaymentTerms;

                        objBitm.ItinCurrencyID = (!string.IsNullOrEmpty(bkit.ItinCurrencyID) ? Guid.Parse(bkit.ItinCurrencyID) : Guid.Empty).ToString();

                        objBitm.ChangeCurrencyID = (!string.IsNullOrEmpty(bkit.ChangeCurrencyID) ? Guid.Parse(bkit.ChangeCurrencyID) : Guid.Empty).ToString();

                        objBitm.NewNetUnitNotinSupptbl = bkit.NewNetUnitNotinSupptbl;

                        objBitm.BookingidIdentifier = (!string.IsNullOrEmpty(bkit.BookingidIdentifier) ? Guid.Parse(bkit.BookingidIdentifier) : Guid.Empty).ToString();

                        objBitm.SupplierPaymentTermsindays = bkit.SupplierPaymentTermsindays;
                        objBitm.SupplierPaymentDepositAmount = bkit.SupplierPaymentDepositAmount;
                        objBitm.SupplierPaymentTermsOverrideindays = bkit.SupplierPaymentTermsOverrideindays;
                        objBitm.SupplierPaymentOverrideDepositAmount = bkit.SupplierPaymentOverrideDepositAmount;

                        string purpose = string.Empty;

                        if (recordmode.ToString().ToLower() == "edit")

                        {

                            purpose = "I";

                            objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                            objBitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                            objBitm.DeletedBy = Guid.Empty.ToString();

                        }

                        else if (recordmode.ToString().ToLower() == "save")

                        {

                            purpose = "I";

                            objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                            objBitm.ModifiedBy = Guid.Empty.ToString();

                            objBitm.DeletedBy = Guid.Empty.ToString();

                        }

                        else if (recordmode.ToString().ToLower() == "delete")

                        {

                            purpose = "D";

                            objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                            objBitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                            objBitm.IsDeleted = true;

                            objBitm.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        }

                        string objret = objitdal.SaveUpdateBooking(purpose, objBitm);

                        if (!string.IsNullOrEmpty(objret))

                        {

                            if (objret.ToString().ToLower() == "1")

                            {

                                if (!string.IsNullOrEmpty(bkit.BookingNote) || !string.IsNullOrEmpty(bkit.VoucherNote) || !string.IsNullOrEmpty(bkit.Privatemsg))

                                {

                                    objBitm.ItineraryID = hdnitineraryid.Text.Trim();

                                    objBitm.BookingID = bkit.BookingID;

                                    objBitm.BookingNotesid = bkit.BookingNotesid;

                                    objBitm.BookingNote = bkit.BookingNote;

                                    objBitm.VoucherNote = bkit.VoucherNote;

                                    objBitm.Privatemsg = bkit.Privatemsg;

                                    if (recordmode.ToString().ToLower() == "edit")

                                    {

                                        purpose = "I";

                                        objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                                        objBitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                                        objBitm.DeletedBy = Guid.Empty.ToString();

                                    }

                                    else if (recordmode.ToString().ToLower() == "save")

                                    {

                                        purpose = "I";

                                        objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                                        objBitm.ModifiedBy = Guid.Empty.ToString();

                                        objBitm.DeletedBy = Guid.Empty.ToString();

                                    }

                                    string objret1 = objitdal.SaveUpdateBookingNotes(purpose, objBitm);

                                }

                                cntrowsuccss = cntrowsuccss + 1;

                            }

                            // MessageBox.Show("Itinerary saved successfully");





                        }

                    }

                   

                }



            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }



        public void ReteriveBookingItemsTotalDetails(string recfromdborscreen)
        {

            try
            {

                if (recfromdborscreen.ToLower() == "db")
                {
                    ListofBookingItemsTotal = objitdal.BookingFinalTotalsRetrive(Guid.Parse(hdnitineraryid.Text));

                    if (ListofBookingItemsTotal != null && ListofBookingItemsTotal.Count > 0)
                    {
                        foreach (BookingItems sup in ListofBookingItemsTotal)
                        {

                            hdnItineraryBookingTotalId.Text = sup.ItineraryBookingTotalId;
                            hdnitineraryid.Text = sup.ItineraryID;
                            //lblTotalAmountNetTotal.Text = sup.SumofNetTotal.ToString();
                            //lblTotalAmountGrossTotal.Text = sup.SumofGrossTotal.ToString();
                            //hdnItineraryBookingTotalId.Text = sup.ItineraryBookingTotalId;

                            //lblTotalAmountGrossAdj.Text = itincurformat + "  " + sup.SumofGrossAdjustment.ToString("0.00");

                            //lblTotalGrossFinal.Text = itincurformat + "  " + sup.SumofGrossFinal.ToString("0.00");

                            //lblTotalAmountGrossTotal.Text = itincurformat + "  " + sup.SumofGrossTotal.ToString("0.00");

                            //lblTotalAmountNetFinal.Text = itincurformat + "  " + sup.SumofNetFinal.ToString("0.00");

                            //lblTotalAmountNetTotal.Text = itincurformat + "  " + sup.SumofNetTotal.ToString("0.00");



                            //lblChangeGrossFinal.Text = itincurformat + "  " + sup.SumofGrossFinal.ToString("0.00");

                            //lblChangeGrossTotal.Text = itincurformat + "  " + sup.SumofGrossTotal.ToString("0.00");

                            //lblChangeNetFinal.Text = itincurformat + "  " + sup.SumofNetFinal.ToString("0.00");

                            //lblChangeNetTotal.Text = itincurformat + "  " + sup.SumofNetTotal.ToString("0.00");
                            if (string.IsNullOrEmpty(txtmarginpercenOverrideall.Text))
                            {
                                txtmarginpercenOverrideall.Text = sup.MarginAdjustmentOverrideall.ToString("0.00");
                            }
                            if (string.IsNullOrEmpty(TxtGrsAdjgrossprice.Text))
                            {
                                TxtGrsAdjgrossprice.Text = sup.MarginAdjustmentGross.ToString("0.00");
                            }
                            if (string.IsNullOrEmpty(TxtGrsAdjgrossprice.Text))
                            {
                                TxtGrsAdjgrossprice.Text = sup.GrossAdjustmentGross.ToString("0.00");
                            }
                            //if (string.IsNullOrEmpty(TxtFinalsell.Text))
                            //{
                            //    TxtFinalsell.Text = sup.FinalSell.ToString("0.00");
                            //}
                            if (string.IsNullOrEmpty(TxtGrsAdjmarkup.Text))
                            {
                                TxtGrsAdjmarkup.Text = sup.GrossAdjustmentMarkup.ToString("0.00");
                            }
                            if (string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text))
                            {
                                if (sup.GrossAdjustmentFinalOverride > 0)
                                {
                                    TxtGrsAdjFinalOverrides.Text = sup.GrossAdjustmentFinalOverride.ToString("0.00");
                                    TxtFinalsell.Text = sup.GrossAdjustmentFinalOverride.ToString("0.00");
                                }
                                else if (sup.FinalSell > 0)
                                {
                                    //TxtGrsAdjFinalOverrides.Text = sup.FinalSell.ToString("0.00");
                                    TxtFinalsell.Text = sup.FinalSell.ToString("0.00");
                                }
                                else
                                {
                                    TxtGrsAdjFinalOverrides.Text = "";
                                    TxtFinalsell.Text = "";
                                }

                                if ((!string.IsNullOrEmpty(TxtFinalsell.Text)) && (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text)))
                                {
                                    if (TxtFinalsell.Text.Trim() != TxtGrsAdjFinalOverrides.Text.Trim())
                                    {
                                        TxtGrsAdjFinalOverrides.Text = TxtGrsAdjFinalOverrides.Text;
                                        TxtFinalsell.Text = TxtGrsAdjFinalOverrides.Text;
                                    }
                                }
                            }
                            else
                            {
                                if ((!string.IsNullOrEmpty(TxtFinalsell.Text)) && (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text)))
                                {
                                    if (TxtFinalsell.Text.Trim() != TxtGrsAdjFinalOverrides.Text.Trim())
                                    {
                                        TxtGrsAdjFinalOverrides.Text = TxtGrsAdjFinalOverrides.Text;
                                        TxtFinalsell.Text = TxtGrsAdjFinalOverrides.Text;
                                    }
                                }
                            }
                            if (string.IsNullOrEmpty(TxtFinalmarginprice.Text))
                            {
                                decimal diffamt = sup.FinalMargin;
                                TxtFinalmarginprice.Text = diffamt.ToString("0.00") + " (" + sup.FinalMarginpercentage.ToString("0.00") + " %)";
                            }

                        }

                    }
                    else
                    {
                        decimal? commonmargin = DBconnEF.GetbookingMargin();
                        if (commonmargin !=null && commonmargin>0)
                        {
                            txtmarginpercenOverrideall.Text =commonmargin.Value.ToString("0.00");
                        }
                    }
                }

                 
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }

        }

        public void ReteriveBookingItems()

        {

            try

            {

                string recfromdborscreen = string.Empty;

                if (BookingItemsitin.Count == 0)

                {

                    ListofBookingItems = objitdal.BookingItemsRetrive(Guid.Parse(hdnitineraryid.Text));

                    if (ListofBookingItems != null && ListofBookingItems.Count > 0)

                    {

                        foreach (BookingItems sup in ListofBookingItems)

                        {

                            if (BookingItemsitin.Where(x => x.BookingID == sup.BookingID).Count() == 0)

                            {

                                //ListofCurrencyServiceidwise = loadDropDownListValues.CurrencyinfoReterive(sup.ServiceID.ToString());

                                //if (ListofCurrencyServiceidwise.Count > 0)

                                //{

                                //    sup.BkgCurrencyName = ListofCurrencyServiceidwise[0].CurrencyName;

                                //    sup.BkgCurDisplayFormat = ListofCurrencyServiceidwise[0].DisplayFormat;

                                //    sup.BkgCurrencyID = ListofCurrencyServiceidwise[0].CurrencyID;

                                //}

                                Tuple<decimal, decimal> curr = CommonValues.CalculateItinearycurrency(sup.ItinCurrencyID, sup.BkgCurrencyID);

                                if (curr != null)

                                {

                                    if (sup.ItinCurrency != sup.BkgCurrencyName && sup.ItinCurrencyID != sup.BkgCurrencyID)

                                    {

                                        if (sup.Grosstotal > 0 && sup.Nettotal > 0 && curr.Item1 > 0 && curr.Item2 > 0)

                                        {

                                            sup.BkgNettotal = sup.Nettotal / curr.Item2;

                                            sup.BkgNetfinal = sup.Netfinal / curr.Item2;

                                            sup.BkgGrosstotal = sup.Grosstotal / curr.Item2;

                                            sup.BkgGrossfinal = sup.Grossfinal / curr.Item2;



                                            //sup.Nettotal = curr.Item2 * sup.Nettotal;

                                            //sup.Netfinal = curr.Item2 * sup.Netfinal;

                                            //sup.Grosstotal = curr.Item2 * sup.Grosstotal;

                                            //sup.Grossfinal = curr.Item2 * sup.Grossfinal;





                                        }

                                    }

                                    else

                                    {

                                        sup.BkgNettotal = curr.Item1 * sup.Nettotal;

                                        sup.BkgNetfinal = curr.Item1 * sup.Netfinal;

                                        sup.BkgGrosstotal = curr.Item1 * sup.Grosstotal;

                                        sup.BkgGrossfinal = curr.Item1 * sup.Grossfinal;

                                    }

                                }

                                else

                                {

                                    sup.BkgNettotal = sup.Nettotal;

                                    sup.BkgNetfinal = sup.Netfinal;

                                    sup.BkgGrosstotal = sup.Grosstotal;

                                    sup.BkgGrossfinal = sup.Grossfinal;

                                }





                                sup.ChangeCurrencyID = sup.BkgCurrencyID;

                                sup.SelectedItemRequstStatus =

                                //(((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                                //((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID : Guid.Empty.ToString(); 

                                ListofRequestStatus.Where(x => x.RequestStatusID == sup.Status).FirstOrDefault();
                                
                                sup.SelectedItemPickuplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == sup.Pickuplocation).FirstOrDefault();
                                sup.SelectedItemDroplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == sup.Droplocation).FirstOrDefault();


                                sup.SupplierPaymentTermsindays = sup.SupplierPaymentTermsindays;
                                sup.SupplierPaymentDepositAmount = sup.SupplierPaymentDepositAmount;
                                sup.SupplierPaymentTermsOverrideindays = sup.SupplierPaymentTermsOverrideindays;
                                sup.SupplierPaymentOverrideDepositAmount = sup.SupplierPaymentOverrideDepositAmount;

                                BookingItemsitin.Add(sup);

                            }

                        }


                        recfromdborscreen = "DB";
                    }

                }

                else

                {

                    foreach (BookingItems sup in BookingItemsitin)

                    {

                        if (BookingItemsitin.Where(x => x.BookingID == sup.BookingID).Count() > 0)

                        {

                            var Requestid = (((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                             ((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID.ToString() : string.Empty.ToString();

                            if (!String.IsNullOrEmpty(Requestid))

                            {

                                sup.SelectedItemRequstStatus = ListofRequestStatus.Where(x => x.RequestStatusID == Requestid).FirstOrDefault();

                                sup.Status = ListofRequestStatus.Where(x => x.RequestStatusID == Requestid).FirstOrDefault().RequestStatusID;

                            }

                            var pickupid = (((SQLDataAccessLayer.Models.Pickupdroplocation)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemPickuplocation)) != null) ?

                            ((SQLDataAccessLayer.Models.Pickupdroplocation)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemPickuplocation)).PickupDropLocationId.ToString() : string.Empty.ToString();

                            if (!String.IsNullOrEmpty(pickupid))

                            {

                                sup.SelectedItemPickuplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == pickupid).FirstOrDefault();

                                sup.Pickuplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == pickupid).FirstOrDefault().PickupDropLocationId;

                            }
                            var Dropid = (((SQLDataAccessLayer.Models.Pickupdroplocation)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemDroplocation)) != null) ?

                            ((SQLDataAccessLayer.Models.Pickupdroplocation)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemDroplocation)).PickupDropLocationId.ToString() : string.Empty.ToString();

                            if (!String.IsNullOrEmpty(Dropid))

                            {

                                sup.SelectedItemDroplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == Dropid).FirstOrDefault();

                                sup.Droplocation = ListofPDLoction.Where(x => x.PickupDropLocationId == Dropid).FirstOrDefault().PickupDropLocationId;

                            }



                            ////((SQLDataAccessLayer.Models.RequestStatus)sup.SelectedItemRequstStatus!=null)?((SQLDataAccessLayer.Models.RequestStatus)sup.SelectedItemRequstStatus).RequestStatusID:Guid.Empty.ToString();

                            //// ListofRequestStatus.Where(x => x.RequestStatusID == sup.Status).FirstOrDefault();

                            ////sup.Status = (((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                            ////((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID.ToString() : Guid.Empty.ToString();

                            ////CmbRequestStatusitin.SelectedValuePath = (((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                            //// ((SQLDataAccessLayer.Models.RequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID.ToString() : Guid.Empty.ToString();





                            sup.Netunit = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Netunit;

                            sup.Nettotal = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Nettotal;

                            sup.Netfinal = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Netfinal;

                            sup.GrossAdj = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().GrossAdj;

                            sup.Grossfinal = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Grossfinal;

                            sup.Grosstotal = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Grosstotal;

                            sup.Grossunit = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Grossunit;

                            sup.StartDate = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().StartDate;

                            sup.StartTime = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().StartTime;

                            sup.Enddate = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Enddate;

                            sup.EndTime = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().EndTime;

                            sup.Ref = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Ref;

                            sup.ItemDescription = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().ItemDescription;

                            sup.NtsDays = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().NtsDays;

                            sup.Qty = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Qty;

                            sup.Invoiced = BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().Invoiced;



                            //// cmbrequestvalueassign(sup.BookingID);



                            // BookingItemsitin.Remove(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault());

                            //BookingItemsitin.Add(sup);

                        }

                    }
                    recfromdborscreen = "SCR";
                }



                if (ListofBookingItems == null || ListofBookingItems.Count == 0)

                {

                    dgItinBooking.ItemsSource = null;

                }



                //dgItinBooking.Items.Clear();

                // dgItinBooking.ItemsSource = null;

                // dgItinBooking.Items.Refresh();

                // cmbrequestvalueassign();

                if (BookingItemsitin.Select(x => x.BkgCurrencyName).Distinct().Contains("Pound Sterling") == true)

                {



                    visiblesterlingcolumn();

                }

                else

                {

                    // BookingItemsitin.ToList().ForEach(x => { x.sterlingcolumnvisible = false; });

                    hidesterlingcolumn();

                }

                dgItinBooking.ItemsSource = BookingItemsitin.OrderBy(x => x.StartDate).ToList();
                currencyformat();
                totalcalculation();
                ReteriveBookingItemsTotalDetails(recfromdborscreen);

                this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                this.clientTabViewModel.Loginuserid = loginuserid;

                this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

                this.clientTabViewModel.CTPaxinfoCommand.RetrieveCommand.Execute();
                this.clientTabViewModel.CTPassengerCommand.RetrieveCommand.Execute();

                if(this.clientTabViewModel.PassengerDetailsobser.Count>0)
                {
                    if(this.clientTabViewModel.PassengerDetailsobser.Where(x=>x.Email==TxtEmail.Text && x.ItineraryID==hdnitineraryid.Text).Count()>0)
                    {
                        CTPassengerVMitn.Passengerid=this.clientTabViewModel.PassengerDetailsobser.Where(x => x.Email == TxtEmail.Text && x.ItineraryID == hdnitineraryid.Text).FirstOrDefault().Passengerid;
                    }
                }
                this.clientTabViewModel.CTRoomTypeCommand.RetrieveCommand.Execute();
                this.clientTabViewModel.CTPaymentCommand.RetrieveCommand.Execute();
            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }

      
        public void visiblesterlingcolumn()

        {

            int cnt = dgItinBooking.Columns.Count;

            BookingItemsitin.ToList().ForEach(x => { x.sterlingcolumnvisible = true; });

            dgItinBooking.Columns[cnt - 7].Visibility = System.Windows.Visibility.Visible;

            dgItinBooking.Columns[cnt - 6].Visibility = System.Windows.Visibility.Visible;

            dgItinBooking.Columns[cnt - 5].Visibility = System.Windows.Visibility.Visible;

            dgItinBooking.Columns[cnt - 4].Visibility = System.Windows.Visibility.Visible;

            lblChangeNetTotal.Visibility = System.Windows.Visibility.Visible;

            lblChangeGrossTotal.Visibility = System.Windows.Visibility.Visible;

            lblChangeNetFinal.Visibility = System.Windows.Visibility.Visible;

            lblChangeGrossFinal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeNetTotal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeGrossTotal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeGrossFinal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeNetfinal.Visibility = System.Windows.Visibility.Visible;



            //brdstrelingcolumn.Visibility = System.Windows.Visibility.Visible;



            var bc = new BrushConverter();

            //brdChangeNetTotal.Background = (Brush)bc.ConvertFrom("#f0f0f0"); 

            //brdChangeNetTotal.BorderBrush = (Brush)bc.ConvertFrom("#c9c9c9");

            //brdChangeGrossTotal.Background = (Brush)bc.ConvertFrom("#f0f0f0"); 

            //brdChangeGrossTotal.BorderBrush = (Brush)bc.ConvertFrom("#c9c9c9");

            //brdChangeGrossFinal.Background = (Brush)bc.ConvertFrom("#f0f0f0");

            //brdChangeGrossFinal.BorderBrush = (Brush)bc.ConvertFrom("#c9c9c9");

            //brdChangeNetfinal.Background = (Brush)bc.ConvertFrom("#f0f0f0");

            //brdChangeNetfinal.BorderBrush = (Brush)bc.ConvertFrom("#c9c9c9");



            //brdChangeNetTotal.BorderThickness = new Thickness(1, 1, 1, 1);

            //brdChangeGrossTotal.BorderThickness = new Thickness(1, 1, 1, 1);

            //brdChangeGrossFinal.BorderThickness = new Thickness(1, 1, 1, 1);

            //brdChangeNetfinal.BorderThickness = new Thickness(1, 1, 1, 1);



            //colBkgNetTotal.Width = new GridLength(dgItinBooking.Columns[cnt - 7].ActualWidth,GridUnitType.Star);

            //colBkgNetFinal.Width = new GridLength(dgItinBooking.Columns[cnt - 6].ActualWidth, GridUnitType.Star);

            //colBkgGrossFinal.Width = new GridLength(dgItinBooking.Columns[cnt - 5].ActualWidth, GridUnitType.Star);

            //colBkgGrossTotal.Width = new GridLength(dgItinBooking.Columns[cnt - 4].ActualWidth, GridUnitType.Star);





            //colBkgNetTotal.Width = new GridLength(1, GridUnitType.Star);

            //colBkgNetFinal.Width = new GridLength(1, GridUnitType.Star);

            //colBkgGrossFinal.Width = new GridLength(1, GridUnitType.Star);

            //colBkgGrossTotal.Width = new GridLength(1, GridUnitType.Star);



        }

        public void hidesterlingcolumn()

        {

            int cnt = dgItinBooking.Columns.Count;

            BookingItemsitin.ToList().ForEach(x => { x.sterlingcolumnvisible = false; });

            dgItinBooking.Columns[cnt - 7].Visibility = System.Windows.Visibility.Collapsed;

            dgItinBooking.Columns[cnt - 6].Visibility = System.Windows.Visibility.Collapsed;

            dgItinBooking.Columns[cnt - 5].Visibility = System.Windows.Visibility.Collapsed;

            dgItinBooking.Columns[cnt - 4].Visibility = System.Windows.Visibility.Collapsed;

            lblChangeNetTotal.Visibility = System.Windows.Visibility.Collapsed;

            lblChangeGrossTotal.Visibility = System.Windows.Visibility.Collapsed;

            lblChangeNetFinal.Visibility = System.Windows.Visibility.Collapsed;

            lblChangeGrossFinal.Visibility = System.Windows.Visibility.Collapsed;

            //brdChangeNetTotal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeGrossTotal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeGrossFinal.Visibility = System.Windows.Visibility.Visible;

            //brdChangeNetfinal.Visibility = System.Windows.Visibility.Visible;



            //  brdChangeNetTotal.Background = Brushes.White;

            // // brdChangeNetTotal.BorderBrush = Brushes.White;

            //  brdChangeGrossTotal.Background = Brushes.White;

            ////  brdChangeGrossTotal.BorderBrush = Brushes.White;

            //  brdChangeGrossFinal.Background = Brushes.White;

            ////  brdChangeGrossFinal.BorderBrush = Brushes.White;

            //  brdChangeNetfinal.Background = Brushes.White;

            //  //  brdChangeNetfinal.BorderBrush = Brushes.White;

            //  brdChangeNetTotal.BorderThickness = new Thickness(0,1,0,0);

            //  brdChangeGrossTotal.BorderThickness = new Thickness(0, 1, 0, 0);

            //  brdChangeGrossFinal.BorderThickness = new Thickness(0, 1, 0, 0);

            //  brdChangeNetfinal.BorderThickness = new Thickness(0, 1, 0, 0);



            //colBkgNetTotal.Width = new GridLength(0);

            //colBkgNetFinal.Width = new GridLength(0);

            //colBkgGrossFinal.Width = new GridLength(0);

            //colBkgGrossTotal.Width = new GridLength(0);











        }

        private void cmbrequestvalueassign()

        {

            foreach (BookingItems sup in dgItinBooking.Items)

            {

                var combox = dgItinBooking.Columns[dgItinBooking.Columns.Count - 2];

                CmbRequestStatusitin.SelectedValuePath = (((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                        ((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusID.ToString() : Guid.Empty.ToString();

                CmbRequestStatusitin.DisplayMemberPath = (((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)) != null) ?

                        ((SQLDataAccessLayer.Models.BkRequestStatus)(BookingItemsitin.Where(x => x.BookingID == sup.BookingID).FirstOrDefault().SelectedItemRequstStatus)).RequestStatusName.ToString() : string.Empty.ToString();

            }



        }

        public void currencyformat()
        {
            //  itincurformat = Currencydispalyformat()

            List<Currencydetail> Listcur = new List<Currencydetail>();

            Listcur = DBconnEF.Currencydispalyformat();

            if (Listcur != null && Listcur.Count > 0)

            {

                itincurformat = Listcur.Where(x => x.CurrencyCode.ToLower() == "eur" && x.CurrencyName.ToLower() == "euro" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;

                Changecurformat = Listcur.Where(x => x.CurrencyCode.ToLower() == "gbp" && x.CurrencyName.ToLower() == "pound sterling" && x.IsDeleted == false && x.Isenable == true).FirstOrDefault().DisplayFormat;

            }
        }

        public void totalcalculation()

        {
            lblTotalAmountGrossAdj.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");

                lblTotalGrossFinal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Grossfinal)).ToString("0.00");

            lblTotalAmountGrossTotal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Grosstotal)).ToString("0.00");

            // lblTotalAmountGrossUnit.Text = (BookingItemsitin.Sum(x => Convert.ToDecimal(x.Grossunit)).ToString("0.00");

            lblTotalAmountNetFinal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00");

            lblTotalAmountNetTotal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Nettotal)).ToString("0.00");



            lblChangeGrossFinal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Grossfinal)).ToString("0.00");

            lblChangeGrossTotal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Grosstotal)).ToString("0.00");

            // lblTotalAmountGrossUnit.Text = (BookingItemsitin.Sum(x => Convert.ToDecimal(x.Grossunit)).ToString("0.00");

            lblChangeNetFinal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00");

            lblChangeNetTotal.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.Nettotal)).ToString("0.00");


            txtMargingrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            TxtGrsAdjgrossprice.Text =  (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            TxtFinalsell.Text =  (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");


            this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
            this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

            decimal diffamt = (Convert.ToDecimal((BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00")) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
            decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, BookingItemsitin.Sum(x => x.GrossAdj));
            TxtFinalmarginprice.Text = diffamt.ToString() + " ("+Finalmarginper.ToString("0.00") + " %)";
                
            // lblTotalAmountNetUnit.Text = (BookingItemsitin.Sum(x => x.Netunit)).ToString("0.00");

        }

        //private void txtmarginpercen_TextChanged(object sender, TextChangedEventArgs e)
        //{
           
        //}
        private void txtmarginpercenOverrideall_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidationClass.IsNumericDotwith2decimal(txtmarginpercenOverrideall.Text) == false)
            {
                System.Windows.MessageBox.Show("Margin Override all allow only numeric");
                txtmarginpercenOverrideall.Text = string.Empty;
                //txtmarginpercenOverrideall.Focusable = true;
                //txtmarginpercenOverrideall.Focus();                                               
                //FocusManager.SetFocusedElement(_parentWindow, txtmarginpercenOverrideall);
                txtmarginpercenOverrideall.Dispatcher.BeginInvoke((Action)(() => { txtmarginpercenOverrideall.Focus(); }));
                return;
            }
            //txtmarginpercenOverrideall.Text=txtmarginpercenOverrideall.Text.Replace(/ [^\d\.] / g, "").replace(/\./, "x").replace(/\./ g, "").replace(/ x /, ".");
            decimal Grossadjmarginval = Convert.ToDecimal((!string.IsNullOrEmpty(txtmarginpercenOverrideall.Text)) ? txtmarginpercenOverrideall.Text : 0);
            foreach (BookingItems items in BookingItemsitin)
            {
                items.GrossAdj = (decimal)DBconnEF.GrossAdjCalculation(items.Grossfinal, Grossadjmarginval);
            }
            txtMargingrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            dgItinBooking.ItemsSource = BookingItemsitin.OrderBy(x => x.StartDate).ToList();
            lblTotalAmountGrossAdj.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            txtMargingrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            TxtGrsAdjgrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
            TxtFinalsell.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");

            this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
            this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

            decimal diffamt = (Convert.ToDecimal((BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00")) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
            decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, BookingItemsitin.Sum(x => x.GrossAdj));
            TxtFinalmarginprice.Text = diffamt.ToString() + " (" + Finalmarginper.ToString("0.00") + " %)";
        }

        private void TxtGrsAdjFinalOverrides_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidationClass.IsNumericDotwith2decimal(TxtGrsAdjFinalOverrides.Text) == false)
            {
                System.Windows.MessageBox.Show("Gross Adjustment Final Overrides allow only numeric");
                TxtGrsAdjFinalOverrides.Text = string.Empty;
                //TxtGrsAdjFinalOverrides.Focus();                
                TxtGrsAdjFinalOverrides.Dispatcher.BeginInvoke((Action)(() => { TxtGrsAdjFinalOverrides.Focus(); }));
                return;
            }
            loginuserrole = loadDropDownListValues.CurrentUserRole(loginusername);

            if (!string.IsNullOrEmpty(loginuserrole))
            {
                if (loginuserrole.ToLower() == "travel advisor")
                {
                    MessageBox.Show("Your current permission level does not allow change the Itinerary final price...!"); //TxtGrsAdjFinalOverrides.Text = "";
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text) && Convert.ToDecimal(TxtGrsAdjFinalOverrides.Text) > 0)
                    {
                        decimal diffamt = (Convert.ToDecimal(TxtGrsAdjFinalOverrides.Text) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
                        decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, Convert.ToDecimal(TxtGrsAdjFinalOverrides.Text));
                        TxtFinalmarginprice.Text = diffamt.ToString() + " (" + Finalmarginper.ToString("0.00") + " %)";
                        TxtFinalsell.Text = TxtGrsAdjFinalOverrides.Text;

                        this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                        this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;
                    }
                    else
                    {
                        decimal diffamt = (Convert.ToDecimal((BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00")) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
                        decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, BookingItemsitin.Sum(x => x.GrossAdj));
                        TxtFinalmarginprice.Text = diffamt.ToString() + " (" + Finalmarginper.ToString("0.00") + " %)";
                        if ((!string.IsNullOrEmpty(TxtGrsAdjmarkup.Text) && Convert.ToDecimal(TxtGrsAdjmarkup.Text) > 0))
                        {
                            TxtFinalsell.Text = TxtGrsAdjgrossprice.Text;
                        }
                        else
                        {
                            TxtFinalsell.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                        }

                        this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                        this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;
                    }
                }
            }
        }

        //private void btnmargin_Click(object sender, RoutedEventArgs e)
        //{
        //    BookingMargin bkmarg = new BookingMargin(loginusername, this);
        //    bkmarg.ShowDialog();
        //}

        private void TxtGrsAdjmarkup_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ValidationClass.IsNumericDotwith2decimal(TxtGrsAdjmarkup.Text) == false)
            {
                System.Windows.MessageBox.Show("Gross Adjustment Markup allow only numeric");
                TxtGrsAdjmarkup.Text = string.Empty;
                //TxtGrsAdjmarkup.Focus(); 
                TxtGrsAdjmarkup.Dispatcher.BeginInvoke((Action)(() => { TxtGrsAdjmarkup.Focus(); }));
                return;
            }
            if (ValidationClass.IsNumericDotwith2decimal(txtmarginpercenOverrideall.Text) == false)
            {
                System.Windows.MessageBox.Show("Margin Override all allow only numeric");                
                txtmarginpercenOverrideall.Text = string.Empty;
                // txtmarginpercenOverrideall.Focus();
                txtmarginpercenOverrideall.Dispatcher.BeginInvoke((Action)(() => { txtmarginpercenOverrideall.Focus(); }));
                return;
            }
            if ((!string.IsNullOrEmpty(TxtGrsAdjmarkup.Text) && Convert.ToDecimal(TxtGrsAdjmarkup.Text) > 0)
                && (!string.IsNullOrEmpty(txtMargingrossprice.Text) && Convert.ToDecimal(txtMargingrossprice.Text) > 0))
            {

                decimal Grossadjmarginval = Convert.ToDecimal((!string.IsNullOrEmpty(txtmarginpercenOverrideall.Text)) ? txtmarginpercenOverrideall.Text : 0);
                decimal Grossadjmarkup = Convert.ToDecimal((!string.IsNullOrEmpty(TxtGrsAdjmarkup.Text)) ? TxtGrsAdjmarkup.Text : 0);
                foreach (BookingItems items in BookingItemsitin)
                {
                    items.GrossAdj = (decimal)DBconnEF.GrossAdjCalculation(items.Grossfinal, Grossadjmarginval, Grossadjmarkup);
                }
                dgItinBooking.ItemsSource = BookingItemsitin.OrderBy(x => x.StartDate).ToList();
                lblTotalAmountGrossAdj.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                if ((!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text) && Convert.ToDecimal(TxtGrsAdjFinalOverrides.Text) > 0))
                {
                    TxtFinalsell.Text = TxtGrsAdjFinalOverrides.Text;
                }
                else
                {
                    TxtFinalsell.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                }

                this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

                TxtGrsAdjgrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                decimal diffamt = (Convert.ToDecimal(TxtGrsAdjgrossprice.Text) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
                decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, Convert.ToDecimal((BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00")));
                TxtFinalmarginprice.Text = diffamt.ToString() + " (" + Finalmarginper.ToString("0.00") + " %)";
                
            }
            else
            {
                if ((!string.IsNullOrEmpty(txtMargingrossprice.Text) && Convert.ToDecimal(txtMargingrossprice.Text) > 0))
                {
                    decimal Grossadjmarginval = Convert.ToDecimal((!string.IsNullOrEmpty(txtmarginpercenOverrideall.Text)) ? txtmarginpercenOverrideall.Text : 0);
                    decimal Grossadjmarkup = Convert.ToDecimal((!string.IsNullOrEmpty(TxtGrsAdjmarkup.Text)) ? TxtGrsAdjmarkup.Text : 0);
                    foreach (BookingItems items in BookingItemsitin)
                    {
                        items.GrossAdj = (decimal)DBconnEF.GrossAdjCalculation(items.Grossfinal, Grossadjmarginval, Grossadjmarkup);
                    }
                    dgItinBooking.ItemsSource = BookingItemsitin.OrderBy(x => x.StartDate).ToList();
                    lblTotalAmountGrossAdj.Text = itincurformat + "  " + (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                    decimal diffamt = (Convert.ToDecimal(txtMargingrossprice.Text) - Convert.ToDecimal((BookingItemsitin.Sum(x => x.Netfinal)).ToString("0.00")));
                    decimal Finalmarginper = (decimal)DBconnEF.FinalMarginpercentagecalculation(diffamt, BookingItemsitin.Sum(x => x.GrossAdj));
                    TxtFinalmarginprice.Text = diffamt.ToString() + " (" + Finalmarginper.ToString("0.00") + " %)";                   
                    TxtFinalsell.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");
                    TxtGrsAdjgrossprice.Text = (BookingItemsitin.Sum(x => x.GrossAdj)).ToString("0.00");

                    this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                    this.clientTabViewModel.TotalAmountstring = "Total Amount " + itincurformat;

                }
            }
        }
        private void dgItinBooking_LayoutUpdated(object sender, EventArgs e)

        {

            Thickness t = lblTotal.Margin;

            t.Left = (dgItinBooking.Columns[0].ActualWidth + 7);

            lblTotal.Margin = t;

            lblTotal.Width = dgItinBooking.Columns[10].ActualWidth;



            lblTotalAmountGrossAdj.Width = dgItinBooking.Columns[17].ActualWidth;

            lblTotalGrossFinal.Width = dgItinBooking.Columns[18].ActualWidth;

            lblTotalAmountGrossTotal.Width = dgItinBooking.Columns[20].ActualWidth;

            // lblTotalAmountGrossUnit.Width = dgItinBooking.Columns[20].ActualWidth;

            lblTotalAmountNetFinal.Width = dgItinBooking.Columns[24].ActualWidth;

            lblTotalAmountNetTotal.Width = dgItinBooking.Columns[25].ActualWidth;

            // lblTotalAmountNetUnit.Width = dgItinBooking.Columns[26].ActualWidth;

        }

        private void saveupdateTotalDetails()

        {

            try

            {


                if (dgItinBooking.Items.Count > 0)
                {
                    BookingItems objBitm = new BookingItems();

                    objBitm.ItineraryID = hdnitineraryid.Text.Trim();

                    if (!string.IsNullOrEmpty(hdnItineraryBookingTotalId.Text))
                    {
                        objBitm.ItineraryBookingTotalId = hdnItineraryBookingTotalId.Text;
                    }
                    else
                    {
                        objBitm.ItineraryBookingTotalId = Guid.NewGuid().ToString();
                    }
                    objBitm.SumofNetTotal = (!string.IsNullOrEmpty(lblTotalAmountNetTotal.Text)) ? Convert.ToDecimal(lblTotalAmountNetTotal.Text.Replace("€  ", "")) : 0;

                    objBitm.SumofGrossTotal = (!string.IsNullOrEmpty(lblTotalAmountGrossTotal.Text)) ? Convert.ToDecimal(lblTotalAmountGrossTotal.Text.Replace("€  ", "")) : 0;

                    objBitm.SumofNetFinal = (!string.IsNullOrEmpty(lblTotalAmountNetFinal.Text)) ? Convert.ToDecimal(lblTotalAmountNetFinal.Text.Replace("€  ", "")) : 0;

                    objBitm.SumofGrossFinal = (!string.IsNullOrEmpty(lblTotalGrossFinal.Text)) ? Convert.ToDecimal(lblTotalGrossFinal.Text.Replace("€  ", "")) : 0;

                    objBitm.SumofGrossAdjustment = (!string.IsNullOrEmpty(lblTotalAmountGrossAdj.Text)) ? Convert.ToDecimal(lblTotalAmountGrossAdj.Text.Replace("€  ", "")) : 0;

                    objBitm.MarginAdjustmentOverrideall = (!string.IsNullOrEmpty(txtmarginpercenOverrideall.Text)) ? Convert.ToDecimal(txtmarginpercenOverrideall.Text.Replace("€  ", "")) : 0;

                    objBitm.MarginAdjustmentGross = (!string.IsNullOrEmpty(txtMargingrossprice.Text)) ? Convert.ToDecimal(txtMargingrossprice.Text.Replace("€  ", "")) : 0;

                    objBitm.GrossAdjustmentMarkup = (!string.IsNullOrEmpty(TxtGrsAdjmarkup.Text)) ? Convert.ToDecimal(TxtGrsAdjmarkup.Text.Replace("€  ", "")) : 0;

                    objBitm.GrossAdjustmentGross = (!string.IsNullOrEmpty(TxtGrsAdjgrossprice.Text)) ? Convert.ToDecimal(TxtGrsAdjgrossprice.Text.Replace("€  ", "")) : 0;

                    objBitm.GrossAdjustmentFinalOverride = (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text)) ? Convert.ToDecimal(TxtGrsAdjFinalOverrides.Text.Replace("€  ", "")) : 0;

                    string FinalMarginval = string.Empty, FinalMarginpercen = string.Empty;
                    if ((!string.IsNullOrEmpty(TxtFinalmarginprice.Text)))
                    {
                        FinalMarginval = TxtFinalmarginprice.Text.Replace("€  ", "").Split(" ")[0];
                        FinalMarginpercen = TxtFinalmarginprice.Text.Replace("(", "").Replace(")", "").Split(" ")[1];
                    }
                    else
                    {
                        FinalMarginval = "0";
                        FinalMarginpercen = "0";

                    }
                    objBitm.FinalMargin = (!string.IsNullOrEmpty(FinalMarginval)) ? Convert.ToDecimal(FinalMarginval) : 0;
                    objBitm.FinalMarginpercentage = (!string.IsNullOrEmpty(FinalMarginpercen)) ? Convert.ToDecimal(FinalMarginpercen) : 0;


                    objBitm.FinalSell = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text.Replace("€  ", "")) : 0;

                    //  objBitm.FinalAgentCommission = (!string.IsNullOrEmpty(TxtFinalagentCommission.Text)) ? Convert.ToDecimal(TxtFinalagentCommission.Text) : 0;

                    string purpose = string.Empty;

                    if (recordmode.ToString().ToLower() == "edit")

                    {

                        purpose = "I";

                        objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        objBitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        objBitm.DeletedBy = Guid.Empty.ToString();

                    }

                    else if (recordmode.ToString().ToLower() == "save")

                    {

                        purpose = "I";

                        objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        objBitm.ModifiedBy = Guid.Empty.ToString();

                        objBitm.DeletedBy = Guid.Empty.ToString();

                    }

                    else if (recordmode.ToString().ToLower() == "delete")

                    {

                        purpose = "D";

                        objBitm.CreatedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        objBitm.ModifiedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                        objBitm.IsDeleted = true;

                        objBitm.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;

                    }

                    string objret = objitdal.SaveUpdateBookingFinalTotals(purpose, objBitm);

                    //  if (objret == "1") { BookingItemsitin = null; ReteriveBookingItems(); }

                }
            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }


        private void BtnDeleteBooking_Click(object sender, RoutedEventArgs e)

        {

            if (dgItinBooking.SelectedItems.Count > 0)

            {

                MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

                if (messageBoxResult == MessageBoxResult.Yes)

                {
                    List<BookingItems> listdel = new List<BookingItems>();
                    foreach(BookingItems item in dgItinBooking.SelectedItems)
                    {
                        listdel.Add(item);
                    }
                    // DeleteBookingItems(objBI);

                    if (listdel.Count > 0)
                    {
                        DeleteBookingItemsMultiple(listdel);
                    }

                }

            }

            else

            {

                System.Windows.MessageBox.Show("Please select booking item");

                return;

            }



        }



        private void DeleteBookingItems(BookingItems ObjBidel)

        {

            try

            {

                if (dgItinBooking.Items.Count > 0)

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

                        BookingItemsitin.Remove(BookingItemsitin.Where(m => m.BookingID == ObjBidel.BookingID

                        && m.ItineraryID == ObjBidel.ItineraryID && m.SupplierID == ObjBidel.SupplierID &&

                        m.ServiceID == ObjBidel.ServiceID).FirstOrDefault());

                        //dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;

                        //PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);

                        ReteriveBookingItems();

                    }



                }

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }





        private void DeleteBookingItemsMultiple(List<BookingItems> listdelitem)

        {

            try

            {

                foreach (BookingItems items in listdelitem)

                {

                    if (listdelitem.Count > 0)

                    {

                        BookingItems objBItdel = new BookingItems();

                        objBItdel.ItineraryID = items.ItineraryID;

                        objBItdel.BookingID = items.BookingID;

                        objBItdel.SupplierID = items.SupplierID;

                        objBItdel.ServiceID = items.ServiceID;

                        objBItdel.PricingRateID = items.PricingRateID;

                        objBItdel.PricingOptionId = items.PricingOptionId;

                        objBItdel.IsDeleted = true;

                        objBItdel.DeletedBy = (string.IsNullOrEmpty(loginuserid)) ? Guid.Empty.ToString() : loginuserid;



                        string objret = objitdal.DeleteBookingItems(objBItdel);

                        if (!string.IsNullOrEmpty(objret))

                        {

                            if (objret.ToString().ToLower() == "1")

                            {

                                //MessageBox.Show("Booking Item Deleted successfully");

                                BookingItemsitin.Remove(BookingItemsitin.Where(m => m.BookingID == items.BookingID

                                && m.ItineraryID == items.ItineraryID && m.SupplierID == items.SupplierID &&

                                m.ServiceID == items.ServiceID).FirstOrDefault());

                            }

                            else if (objret.ToString().ToLower() == "-1")

                            {

                                BookingItemsitin.Remove(BookingItemsitin.Where(m => m.BookingID == items.BookingID

                                    && m.ItineraryID == items.ItineraryID && m.SupplierID == items.SupplierID &&

                                    m.ServiceID == items.ServiceID).FirstOrDefault());

                            }





                            //dgSupplierServicesRates.ItemsSource = SupplierSRatesDt.Where(x => x.SupplierServiceId == ssm.SupplierServiceId); ;

                            //PricingOptcheckExpireactive(ssRateobj.SupplierServiceId, ssRateobj.SupplierServiceDetailsRateId, ssRateobj.IsExpired);



                        }



                    }

                }

                ReteriveBookingItems();

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

        }



        private void MnuBookAll_Click(object sender, RoutedEventArgs e)

        {

            if (BookingItemsitin.Count > 0)

            {

                string Emailoption = string.Empty;

                Emailoption = "all";

                BookingEmail ObjBE = new BookingEmail();

                ObjBE.Subject = "Booking request for " + TxtItineraryName.Text.Trim();

                Guid Agentuserid = (CmbAgentAssignedTo.SelectedValue != null) ? Guid.Parse(CmbAgentAssignedTo.SelectedValue.ToString()) : Guid.Empty;



                ObjBE.BccEmail = (ListUserdet.Where(x => x.Userid == Agentuserid).FirstOrDefault() != null) ? ListUserdet.Where(x => x.Userid == Agentuserid).FirstOrDefault().EmailAddress : string.Empty;

                SupplierRequestEmail wnbked = new SupplierRequestEmail(loginusername, Emailoption, ObjBE, this);

                wnbked.ShowDialog();

            }

            else

            {

                MessageBox.Show("Booking items should not be empty");

                return;

            }



        }





        private void MnuBookSelected_Click(object sender, RoutedEventArgs e)

        {

            string Emailoption = string.Empty;

            if (BookingItemsitin.Count > 0)

            {

                List<BookingItems> listbt = new List<BookingItems>();

                BookingEmail ObjBE = new BookingEmail();

                ObjBE.Subject = "Booking request for " + TxtItineraryName.Text.Trim();

                Guid Agentuserid = (CmbAgentAssignedTo.SelectedValue != null) ? Guid.Parse(CmbAgentAssignedTo.SelectedValue.ToString()) : Guid.Empty;



                ObjBE.BccEmail = (ListUserdet.Where(x => x.Userid == Agentuserid).FirstOrDefault() != null) ? ListUserdet.Where(x => x.Userid == Agentuserid).FirstOrDefault().EmailAddress : string.Empty;

                if (dgItinBooking.SelectedItems.Count > 0)

                {

                    foreach (BookingItems items in dgItinBooking.SelectedItems)

                    {

                        //BookingItems objBI = dgItinBooking.SelectedItems as BookingItems;

                        if (items != null)

                        {

                            Emailoption = Emailoption + "," + items.BookingID;



                        }

                    }

                    // Emailoption=Emailoption.Remove(0,1);

                    SupplierRequestEmail wnbked = new SupplierRequestEmail(loginusername, Emailoption, ObjBE, this);

                    wnbked.ShowDialog();

                }

                else

                {

                    MessageBox.Show("Please select any booking items");

                    return;

                }

            }

            else

            {

                MessageBox.Show("Booking items should not be empty");

                return;

            }

        }



        private void DataGridCell_MouseDoubleClick(object sender, MouseButtonEventArgs e)

        {

            var dataGridCellTarget = (DataGridCell)sender;

            BookingItems objBI = dgItinBooking.SelectedItem as BookingItems;
            if (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text))
            {
                string messagestr = "Do you really want to edit the booking record, as the Itinerary Price Override is already available?\r\n\r\nWarning : Itinerary Final Override Price is (" + itincurformat + " " + TxtGrsAdjFinalOverrides.Text + ") In case there are any changes to the record, the price will be updated based on the change with the same margin\r\n";

                MessageBoxResult mesbox = System.Windows.MessageBox.Show(messagestr, "Message", System.Windows.MessageBoxButton.OKCancel);

                if (mesbox == MessageBoxResult.OK)
                {
                    TxtGrsAdjFinalOverrides.Text = string.Empty;
                    TxtFinalsell.Text = string.Empty;
                    if (objBI != null)
                    {
                        Bookingedit wnbked = new Bookingedit(loginusername, this, objBI);
                        wnbked.ShowDialog();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Please select booking item");
                        return;
                    }

                }
                else
                {

                }

            }
            else
            {
                if (objBI != null)
                {
                    Bookingedit wnbked = new Bookingedit(loginusername, this, objBI);
                    wnbked.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("Please select booking item");
                    return;
                }
            }
           


        }



        private void dgItinBooking_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)

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
                        string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                        if (!string.IsNullOrEmpty(servicetypename))
                        {
                            if (servicetypename.ToLower() == "accommodation")
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
                                    elnt.Focus();
                                    elnt.Focusable = true;
                                    return;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text))
                        {
                            //string messagestr = "Warning : Itinerary Price Override exists ("+itincurformat+" "+ TxtGrsAdjFinalOverrides.Text + ") and should be updated to reflect this change\r\n\r\nDo you want to remove override now?\r\n";                            
                            string messagestr = "Do you really want to edit the booking record, as the Itinerary Price Override is already available?\r\n\r\nWarning : Itinerary Final Override Price is (" + itincurformat + " " + TxtGrsAdjFinalOverrides.Text + ") In case there are any changes to the record, the price will be updated based on the change with the same margin\r\n";

                            MessageBoxResult mesbox = System.Windows.MessageBox.Show(messagestr, "Message", System.Windows.MessageBoxButton.OKCancel);

                            if (mesbox == MessageBoxResult.OK)
                            {
                                TxtGrsAdjFinalOverrides.Text = string.Empty;
                                TxtFinalsell.Text = string.Empty;
                                if (objBIt != null)
                                {
                                    objBIt.NtsDays = Convert.ToInt32(elnt.Text);
                                    
                                    if (!string.IsNullOrEmpty(servicetypename))
                                    {
                                        if (servicetypename.ToLower() == "accommodation")
                                        {
                                            objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                        }
                                        else
                                        {
                                            objBIt.Enddate = (objBIt.StartDate);
                                        }
                                    }
                                    //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                    CommonValues.Grossnetcalculation(objBIt);
                                    currencyformat();
                                    totalcalculation();
                                }
                            }
                            else 
                            {
                                elnt.Text = objBIt.NtsDays.ToString();
                            }

                        }
                        else
                        {
                            if (objBIt != null)
                            {
                                objBIt.NtsDays = Convert.ToInt32(elnt.Text);
                               // string servicetypename = DBconnEF.GetServicetypename(objBIt.ServiceTypeID);
                                if (!string.IsNullOrEmpty(servicetypename))
                                {
                                    if (servicetypename.ToLower() == "accommodation")
                                    {
                                        objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                    }
                                    else
                                    {
                                        objBIt.Enddate = (objBIt.StartDate);
                                    }
                                }
                                //objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));
                                CommonValues.Grossnetcalculation(objBIt);
                                currencyformat();
                                totalcalculation();
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

                        if (!string.IsNullOrEmpty(TxtGrsAdjFinalOverrides.Text))
                        {
                            //string messagestr = "Warning : Itinerary Price Override exists ("+itincurformat+" "+ TxtGrsAdjFinalOverrides.Text +") and should be updated to reflect this change\r\n\r\nDo you want to remove override now?\r\n";
                            string messagestr = "Do you really want to edit the booking record, as the Itinerary Price Override is already available?\r\n\r\nWarning : Itinerary Final Override Price is (" + itincurformat + " " + TxtGrsAdjFinalOverrides.Text + ") In case there are any changes to the record, the price will be updated based on the change with the same margin\r\n";

                            MessageBoxResult mesbox = System.Windows.MessageBox.Show(messagestr, "Message", System.Windows.MessageBoxButton.OKCancel);

                            if (mesbox == MessageBoxResult.OK)
                            {
                                TxtGrsAdjFinalOverrides.Text = string.Empty;
                                TxtFinalsell.Text = string.Empty;
                                if (objBIt != null)
                                {
                                    objBIt.Qty = Convert.ToInt32(elqty.Text);
                                    CommonValues.Grossnetcalculation(objBIt);
                                    currencyformat();
                                    totalcalculation();
                                }
                            }
                            else
                            {
                                elqty.Text = objBIt.Qty.ToString();
                            }

                        }
                        else
                        {
                            if (objBIt != null)
                            {
                                objBIt.Qty = Convert.ToInt32(elqty.Text);
                                CommonValues.Grossnetcalculation(objBIt);
                                currencyformat();
                                totalcalculation();
                            }
                        }
                        


                    }

                }

            }

        }



        public BookingItems BookingModel { get; set; }



        private void dgItinBooking_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)

        {

            //supplierPayment supplierPayment = new supplierPayment();

            //supplierPaymentViewModel supplierPaymentViewModel = new supplierPaymentViewModel();

            //if (e.AddedCells.Count == 0) return;

            //var currentCell = e.AddedCells[0];

            //string header = (string)currentCell.Column.Header;

            //dgItinBooking.BeginEdit();

            //if (sender is DataGrid dataGrid && dataGrid.SelectedItem != null)

            //{

            //    supplierPaymentViewModel.SelectedInvoice = (BookingItems)dataGrid.SelectedItem;

            //    supplierPayment.DataContext = supplierPaymentViewModel;



            //}

        }



        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)

        {

            if (e.Source is TabControl tabControl)

            {

                if (tabControl.SelectedItem is TabItem selectedTabItem)

                {

                    // Check if the selectedTabItem is your specific TabItem

                    if (selectedTabItem.Name == "Commentab")

                    {

                        if (tabViewModel.ItineraryId != hdnitineraryid.Text)

                        {

                            tabViewModel.ItineraryId = hdnitineraryid.Text;

                            tabViewModel.LoginId = loginuserid;

                            tabViewModel.UserName = loginusername;

                            ItineraryCommentsTab.DataContext = tabViewModel;



                        }



                    }

                    if (selectedTabItem.Header.ToString().ToLower() == "additional")
                    {
                        RBConfirmedDeposit.IsChecked = false; //Added by Angappan.S Allow to select. we create multiple generation files
                        RBConfirmedPaid.IsChecked = false;
                        RBQuotation.IsChecked = false;                        
                        webBrowsercontentQuotation.Navigate((Uri)null);

                        EmailLogSettingViewModel emailLogSetting = new EmailLogSettingViewModel();
                        emailLogSetting.Itineraryid= hdnitineraryid.Text;
                        emailLogSetting.EmailLogsSettingCommand.RetrieveCommand.Execute();
                        this.Communication.DataContext = emailLogSetting;
                    }

                     if(selectedTabItem.Header.ToString().ToLower()=="clients")
                    {
                       
                            this.clientTabViewModel.Itineraryid = hdnitineraryid.Text;

                            this.clientTabViewModel.Loginuserid = loginuserid;

                            this.clientTabViewModel.FinalPaymentAmount = (!string.IsNullOrEmpty(TxtFinalsell.Text)) ? Convert.ToDecimal(TxtFinalsell.Text) : 0;
                            this.clientTabViewModel.TotalAmountstring = "Total Amount "+ itincurformat;

                            //this.clientTabViewModel.CTPaxinfoCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTPaymentCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTRoomTypeCommand.RetrieveCommand.Execute();

                            //this.clientTabViewModel.CTPassengerCommand.RetrieveCommand.Execute();
                        
                    } 

                }

            }

        }



        /*

              private void Txtstartdt_LostFocus(object sender, RoutedEventArgs e)

              {

           //long BookingID = 0;

           // ((System.Windows.Controls.TextBox)e.OriginalSource).Text

           if (((System.Windows.Controls.TextBox)e.OriginalSource).Text != ((System.Windows.Controls.TextBox)e.Source).Text)

           { 

               BookingItems objBIt = dgItinBooking.SelectedItem as BookingItems;

               if ((sender) != null)

               {

            DateTime OldStartDateVal = DateTime.MinValue;// e.RemovedItems[0];



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

                 BookingItemsitin.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;

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

                  // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));

                  // TxtCheckindate.SelectedDate = Startdates;

                  // dgBookingedited.ItemsSource = BookingItemsitinEditfull;

                  //string username, ItineraryWindow ParentWindow, BookingItems objbkitems



                  // RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit",this);

                  RefreshRates objref = new RefreshRates(loginusername, this, BookingItemsitin, objBIt, "bookingItinerary", null);

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

           }

              }



              */



        /* private void Txtstartdt_SelectedDateChanged(object sender, SelectionChangedEventArgs e)

         {

             //long BookingID = 0;

             BookingItems objBIt = dgItinBooking.SelectedItem as BookingItems;

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

               BookingItemsitin.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().StartDate = Startdates;       

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

                // objBIt.Enddate = (objBIt.StartDate).AddDays(Convert.ToInt32(objBIt.NtsDays));

                // TxtCheckindate.SelectedDate = Startdates;

                // dgBookingedited.ItemsSource = BookingItemsitinEditfull;

                //string username, ItineraryWindow ParentWindow, BookingItems objbkitems



                // RefreshRates objref = new RefreshRates(loginusername, IwParWindow, BookingItemsRefreshrates, objBIt, "BookingEdit",this);

                RefreshRates objref = new RefreshRates(loginusername, this, BookingItemsitin, objBIt, "bookingItinerary", null);

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





         }



         */







        //private void CmbRequestSelectionChanged(object sender, SelectionChangedEventArgs e)

        //{

        //    var comboBox = sender as ComboBox;// ((System.Windows.Controls.DataGridComboBoxColumn)((System.Windows.Controls.DataGridCell)sender).Column);





        //    if (comboBox != null)

        //    {

        // if (comboBox.SelectedItem != null)

        // {

        //     BookingItems objBIt = (BookingItems) this.dgItinBooking.CurrentItem;



        //     if (objBIt != null)

        //     {

        //  objBIt.SelectedItemRequstStatus = ((SQLDataAccessLayer.Models.RequestStatus)comboBox.SelectedItem).RequestStatusID;

        //  objBIt.Status = ((SQLDataAccessLayer.Models.RequestStatus)comboBox.SelectedItem).RequestStatusID.ToString();

        //  CmbRequestStatusitin.SelectedValuePath = ((SQLDataAccessLayer.Models.RequestStatus)comboBox.SelectedItem).RequestStatusID;

        //  BookingItemsitin.Where(x => x.BookingID == objBIt.BookingID).FirstOrDefault().Status = objBIt.Status;

        //  //dgItinBooking.ItemsSource = BookingItemsitin;

        //  // LoadBookingEditGrid(objBIt.BookingID);

        //     }

        // }

        //  }

        //}



        //public class ComboboxvalueConverter:IValueConverter

        //{

        //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)

        //    {

        // if(string.IsNullOrEmpty(value.ToString()))

        // {

        //     value= 



        // }

        //    }



        //}

        #endregion "Booking Tab"

        /* Booing Tab end*/







        /* Pdf Creation purpose Html Create start */



        #region "Html Create start"

        PDFGenerationdal pdfdal = new PDFGenerationdal();

        PDFGenerationModel pdfmodel = new PDFGenerationModel();

        List<PDFGenerationModel> Pdfdata = new List<PDFGenerationModel>();

        EmailDal EmailDalobj = new EmailDal();

        public void GetReportContentQuotationConfirmation()

        {

            string type = string.Empty;
            string selectedvalue =string.Empty;
            if (CmbStatus.SelectedItem!= null)
            {
                selectedvalue=((SQLDataAccessLayer.Models.CommonValueList)CmbStatus.SelectedItem).TextField;
            }

            if (RBQuotation.IsChecked == true )

            {

                type = "Quotation";

            }

            if (RBConfirmedDeposit.IsChecked == true)

            {

                type = "Confirmed - Deposit";

            }

            if (RBConfirmedPaid.IsChecked == true)

            {

                type = "Confirmed - Paid in full";

            }

            
            if (!string.IsNullOrEmpty(hdnitineraryid.Text))
            {

                Pdfdata = pdfdal.PDFGenerationView(Guid.Parse(hdnitineraryid.Text), type);



                if (Pdfdata.Count > 0)

                {

                    ChangeTemplateasQuotationconfirmhtml(Pdfdata, type);

                }
                else
                {
                    MessageBox.Show("Please provide the booking details");
                    return;
                }



            }

        }





        private string LoadTemplate()

        {

            string templateFullPath = string.Empty;

            try

            {

                var exePath = AppDomain.CurrentDomain.BaseDirectory;

                var pagesFolder = Directory.GetParent(exePath);

                templateFullPath = pagesFolder.FullName + "\\Email Templates\\EmailQuotation.html";

                return templateFullPath;

            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("Itinerarywindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

                return templateFullPath;

            }



        }

        private void ChangeTemplateasQuotationconfirmhtml(List<PDFGenerationModel> objpdfm, string type)

        {





            string strMarkers = String.Empty;

            string filePath = LoadTemplate();

            if (!string.IsNullOrEmpty(filePath))

            {



                strMarkers = File.ReadAllText(filePath);

                StringBuilder SBMaincontent = new StringBuilder(strMarkers);

                string EmailStrMarkers = string.Empty;

                //List<string> CoreMarkers = EmailDalobj.GetMarkervalues(strMarkers, "##", "##");

                //List<string> CorecostMarkers = EmailDalobj.GetMarkervalues(strMarkers, "#$#", "#$#");

                //List<string> RepeatedMarkers = EmailDalobj.GetMarkervalues(strMarkers, "##BookingsStart##", "##BookingsEnd##");

                int i = 0;



                StringBuilder mainhtmlcontent = new StringBuilder();

                string strheader = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\"/>\r\n    <meta name=\"application-name\"/>\r\n    <meta name=\"author\"/>\r\n    <meta name=\"description\"/>\r\n    <meta name=\"generator\"/>\r\n    <meta name=\"keywords\"/>\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\"/>\r\n    <title>LIV PDF</title>\r\n</head>\r\n<body>";

                string divbodycontentstart = "<div class=\"body_contents\" style=\" padding: 0px 10px; font-family: 'Arial',sans-serif;\">";

                string divbodycontentend = "</div>";

                string divheadercontent = "<table style=\"width: 100%;\">\r\n            <tbody>\r\n                <tr style=\"display: flex; flex-wrap:nowrap; justify-content: flex-end; padding: 15px 0px;\">\r\n                    <td\r\n                        style=\"border-right: 2px solid #44A826; padding-right: 10px; margin-right: 10px; width:75%;text-align: right;\">\r\n                        <p class=\"title_name\"\r\n                            style=\" font-size: 23px; color: #44A826; margin: 0 0 5px 0; font-weight: bold; line-height: 25px;\">\r\n                            Love Irish Tours</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;\">\r\n                            Unit 8 Scurlockstown Business Park,</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;\">\r\n                            Trim,</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;\">\r\n                            Co Meath</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;\">\r\n                            Ph: 1888-508-6639</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;\">\r\n                            Website: www.loveirishtours.com</p>\r\n                        <p class=\"para top_intro\"\r\n                            style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600; margin-bottom: 30px;\">\r\n                            Niall Carroll</p>\r\n                    </td>\r\n                    <td style=\"display: flex;align-items: center;justify-content: center;\">\r\n                        " +
                    "<img src=\"https://loveirishtours.com/wp-content/uploads/2017/11/love-irish-tours-logo-optimized.png\"\r\n                            alt=\"LIV\" />\r\n                    </td>\r\n                </tr>\r\n            </tbody>\r\n        </table>";





                string footer = "</body>\r\n</html>";



                string footerclass = string.Empty;

                string divclientstart = string.Empty;

                string divcostdetails = string.Empty;

                string divwelcomesection = string.Empty;

                string divinclusionnotes = string.Empty;

                string notesitems = string.Empty;



                string staticpart = string.Empty;



                string secondtablestart = string.Empty;

                string secondtableend = string.Empty;

                string enddate = string.Empty;


                string Customerinfostart = string.Empty;
                string customerinfoend = string.Empty;
                string customerdaydetails = string.Empty;
                string customerdaydetailsend = string.Empty;
                string customerdaydetailscontent = string.Empty;
                string customerquotevaluesstart = string.Empty;
                string customerquotevaluescontent = string.Empty;
                string customerquotevalueend = string.Empty;
                string customerquotevaluescontentinclusionpart = string.Empty;
                string docenddate = string.Empty;



                StringBuilder dynamicpartcontent = new StringBuilder();



                PDFGenerationModel _marker = new PDFGenerationModel();

                foreach (PDFGenerationModel Bkit in objpdfm)

                {



                    EmailStrMarkers = strMarkers;

                    _marker.ItineraryID = Bkit.ItineraryID;

                    _marker.ItineraryName = Bkit.ItineraryName;

                    _marker.DisplayName = Bkit.DisplayName;

                    _marker.Email = Bkit.Email;

                    _marker.ItineraryStartDate = Bkit.ItineraryStartDate;

                    _marker.ItineraryEndDate = Bkit.ItineraryEndDate;

                    _marker.InclusionNotes = Bkit.InclusionNotes;

                    _marker.status = Bkit.status;

                    _marker.TextField = Bkit.TextField;

                    _marker.Bkid = Bkit.Bkid;

                    _marker.BookingName = Bkit.BookingName;

                    _marker.EndDate = Bkit.EndDate;

                    _marker.ItemDescription = Bkit.ItemDescription;

                    _marker.ItinCurrency = Bkit.ItinCurrency;

                    _marker.Netunit = Bkit.Netunit;

                    _marker.NightsDays = Bkit.NightsDays;

                    _marker.ServiceName = Bkit.ServiceName;

                    _marker.StartDate = Bkit.StartDate;

                    _marker.Description = Bkit.Description;

                    _marker.SupplierID = Bkit.SupplierID;

                    _marker.ContentID = Bkit.ContentID;

                    _marker.ContentName = Bkit.ContentName;

                    _marker.ContentFor = Bkit.ContentFor;

                    _marker.Heading = Bkit.Heading;

                    _marker.ReportImage = Bkit.ReportImage;

                    _marker.OnlineImage = Bkit.OnlineImage;

                    _marker.BodyHtml = Bkit.BodyHtml;

                    _marker.Paxid = Bkit.Paxid;

                    _marker.PaxNumbers = Bkit.PaxNumbers;



                    _marker.Personcost = Bkit.Personcost;

                    _marker.Deposit = Bkit.Deposit;

                    _marker.Daycount = Bkit.Daycount;

                    _marker.NameofDate = Bkit.NameofDate;

                    _marker.Deposit = Bkit.Deposit;

                    _marker.Totalamount = Bkit.Totalamount;

                    _marker.NameofItineraryStartDate = Bkit.NameofItineraryStartDate;

                    _marker.NameofItineraryEndDate = Bkit.NameofItineraryEndDate;

                    _marker.City = Bkit.City;

                    _marker.Type = Bkit.Type;

                    _marker.ItineraryAutoId = Bkit.ItineraryAutoId;
                    _marker.Phone = Bkit.Phone;
                    _marker.nightdayvalues = Bkit.nightdayvalues;


                    string perpersoncost = string.Empty;

                    if (_marker.PaxNumbers > 0)

                    {

                        perpersoncost = (Convert.ToDecimal(_marker.Totalamount / _marker.PaxNumbers)).ToString("0.00");

                    }

                    else

                    {

                        perpersoncost = (Convert.ToDecimal(_marker.Totalamount)).ToString("0.00");

                    }

                    string perDeposit = string.Empty;

                    string remainingbalance = string.Empty;






                    string selected = string.Empty;

                    if (RBConfirmedDeposit.IsChecked == true)

                    {

                        selected = "Final Itinerary for";

                    }

                    if (RBConfirmedPaid.IsChecked == true)

                    {

                        selected = "Final Itinerary for";

                    }

                    if (RBQuotation.IsChecked == true)

                    {

                        selected = "Quotation for  ";

                    }

                    if (i == 0)

                    {


                        if (type == "Quotation")

                        {

                            perDeposit = (Convert.ToDecimal((_marker.Totalamount * 20) / 100)).ToString("0.00");

                        }



                        if (type == "Confirmed - Deposit")

                        {

                            perDeposit = _marker.Deposit.ToString("0.00");

                            remainingbalance = Convert.ToDecimal(_marker.Totalamount - _marker.Deposit).ToString("0.00");

                        }

                        if (type == "Confirmed - Paid in full")

                        {

                            perDeposit = string.Empty;

                        }

                        if (!string.IsNullOrEmpty(_marker.ItineraryEndDate))

                        {

                            docenddate = _marker.ItineraryStartDate + " - " + _marker.ItineraryEndDate;

                        }

                        else

                        {

                            docenddate = _marker.ItineraryStartDate;

                        }

                        string tourfor = (ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault() != null) ?
                            ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault().Tourlistname.ToString() : string.Empty;

                        Customerinfostart = "<div id=\"Customerinfo\">\r\n           " +
                            " <h1 style=\"font-size: 20px; color: #44A826; margin-bottom: 15px; text-align:center;font-weight: bold;\">\r\n   " +
                            "             " + tourfor + " Itinerary for " + _marker.ItineraryName + "</h1>\r\n       " +
                            "     <h2\r\nstyle=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; \">\r\n\r\n   " +
                            "             Thank you for considering Love Irish Tours to take care of your travel plans for your Magical trip to\r\n               " + tourfor + ".\r\n            </h2>\r\n      " +
                            "      <p\r\n style=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n      " +
                            "    <span style=\"font-weight: 400;\"> Name: </span>" + _marker.ItineraryName + "</p>\r\n          " +
                            "  <p\r\nstyle=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n       " +
                            "        <span style=\"font-weight: 400;\"> Email Address: </span>" + _marker.Email + "</p>\r\n  " +
                            "          <p\r\n style=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                            "            <span style=\"font-weight: 400;\">  Contact Number:</span> " + _marker.Phone + "</p>\r\n " +
                            "           <p\r\n style=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                            "           <span style=\"font-weight: 400;\">   Ireland Trip Dates:</span> " + docenddate + " </p>\r\n " +
                            "           <p\r\n style=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                            "           <span style=\"font-weight: 400;\">   Tour Type: </span> " + _marker.Tourtype + "</p>\r\n       " +
                            "     <p\r\n style=\"font-size: 16px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n " +
                            "         <span style=\"font-weight: 400;\"> Itinerary ID:</span> " + _marker.ItineraryAutoId + " </p>";
                        customerinfoend = " </div>\r\n<br/>\r\n<br/>";


                        customerquotevaluesstart = "<div style=\"width: 7%;float: left;border:0px solid #aaa;\" id=\"Middletable\">" +
                            " <table width=\"80%\" cellpadding=\"5\" style=\"padding-left:5px;\" >\r\n                    <tbody>\r\n                        <tr>\r\n<td style=\"padding:5px;\">\r\n&nbsp;\r\n  </td>\r\n  </tr>\r\n  \r\n</tbody>\r\n </table>\r\n            </div>" +
                            "<div style=\"width: 38%;float: left;\" id=\"customerquote\">\r\n " +
                            "               <table cellpadding=\"5\" style=\"width: 300px;padding-left: 25px;\">\r\n                    <tbody>\r\n                     ";
                        customerquotevaluescontent =
                        "   <tr>\r\n<td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\">\r\n " +
                        "<p style=\"font-size: 16px;margin-bottom: 5px;color: #44A826;\">\r\n " +
                        "  Price Per Person <br/><b>\r\n " + itincurformat + " " + perpersoncost + "\r\n</b> </p>\r\n  </td>\r\n  " + "</tr>\r\n  " +
                        "<tr>\r\n      <td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\">\r\n" +
                        "<p style=\"font-size: 16px;text-align: left; margin-bottom: 5px;color: #44A826;\">\r\n    " +
                        "Deposit Per Person <br/><b>\r\n " + itincurformat + " " + perDeposit + "\r\n</b></p>\r\n      </td>\r\n  </tr>\r\n  " +
                        "<tr>\r\n      <td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;text-align: left;border: 1px solid #333;\">\r\n" +
                        "<p style=\"font-size: 16px;margin-bottom: 5px;color: #44A826;\">\r\n    " +
                        "Duration <br/><b>\r\n    " + _marker.nightdayvalues + " days "+ (Convert.ToInt32(_marker.nightdayvalues)-1).ToString() + " nights\r\n</b></p>\r\n      </td>\r\n  </tr>\r\n  ";


                        customerquotevalueend = "</tbody>\r\n </table>\r\n  </div><div style=\"page-break-before:always\">&nbsp;</div>";

                        string quoinclusionnotesstart = string.Empty, quoinclusionnotesend = string.Empty, quoinclusionnotesliststart = string.Empty, quoinclusion = string.Empty;

                        quoinclusionnotesstart = "<ul class=\"inclusion_list\" style=\"padding-left: 20px; margin-top: 5px;margin-bottom: 15px;\">\r\n  ";


                        if (_marker.InclusionNotes.Contains("\r\n"))
                        {
                            foreach (string note in _marker.InclusionNotes.Split("\r\n"))
                            {
                                if (!string.IsNullOrEmpty(note))
                                {
                                    quoinclusionnotesliststart = quoinclusionnotesliststart + "<li class=\"para body_para\" style=\"text-align: left;  font-size: 16px; color: #44A826; margin: 0 0 5px 0; font-weight: 400; list-style: disc;\">\r\n     " +
                        " " + note + "\r\n  </li>\r\n   ";
                                }

                            }
                        }
                        quoinclusionnotesend = "  </ul>\r\n   ";
                        quoinclusion = quoinclusionnotesstart + quoinclusionnotesliststart + quoinclusionnotesend;

                        customerquotevaluescontentinclusionpart = "<tr>\r\n      <td style=\"padding:0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\">" +
                            "\r\n<p style=\"font-size: 16px;text-align: left;margin-bottom: 5px;color: #44A826;\">\r\n    " +
                            "Inclusion " + quoinclusion + "\r\n</p>\r\n      </td>\r\n  </tr>\r\n";

                        customerdaydetails = " <div style=\"width: 48%;float: left;background: #f5f5f5;\" id=\"Customerdayinfoloop\">" +
                            "              <table width=\"100%\" style=\" border: 1px solid #999;border-top:0px;border-bottom:3px solid #44A826;\" cellpadding=\"6\" cellspacing=\"0\">\r\n  " +
                            "                  <tbody>";
                        customerdaydetailsend = " </tbody>\r\n                </table>\r\n            </div>";




                        secondtablestart = "<div class=\"cilent_update_info\">";

                        secondtableend = "</div><br/>";



                        divclientstart = " <table style=\"width: 100%;margin-top:10px;\">\r\n                <tbody>\r\n                    <tr>\r\n                        <td><h1 class=\"clientname\"\r\n                style=\"font-size: 20px; color: #44A826; margin-bottom: 8px; text-align:left;font-weight: bold;\">" +



                           selected + " " + _marker.ItineraryName + "</h1>" +

                           "<h2 class=\"arrival_time\"\r\n                style=\"font-size: 16px; color: rgb(102, 94, 94); margin: 5px 0px 5px 0px; font-weight: 600; font-family: Tahoma, Arial, sans-serif; text-align: left; line-height: 30px;\">" +

                           "Arriving " + _marker.NameofItineraryStartDate + ", " + _marker.ItineraryStartDate + ", departing ";



                        if (!string.IsNullOrEmpty(_marker.ItineraryEndDate))

                        {

                            enddate = _marker.NameofItineraryEndDate + ", " + _marker.ItineraryEndDate + "</h2></td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>";

                        }

                        else

                        {

                            enddate = _marker.NameofItineraryEndDate + "</h2></td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>";

                        }



                        if (type == "Quotation")

                        {

                            divcostdetails = "<div style=\"clear:both;\"></div><div style=\"width: 50%;float: left;\"><div class=\"cost_status_block\" id=\"Costdetails\" style=\"margin: 0px 0 30px 0;\">\r\n" +
                                "            <p style=\"font-size: 18px; color: #ff0400; margin: 8px 0px; font-weight: 600;\">"

                         + "Total Cost " + itincurformat + " " + _marker.Totalamount + "<br/>    "

                         + "Per Person Cost " + itincurformat + " " + perpersoncost + "<br/>"

                          + "Deposit " + itincurformat + " " + perDeposit + "</p></div></div>";

                        }



                        if (type == "Confirmed - Deposit")

                        {

                            divcostdetails = "<div style=\"clear:both;\"></div><div style=\"width: 65%;float: left;\"><div class=\"cost_status_block\" id=\"Costdetails\" style=\"margin: 0px 0 30px 0;\">\r\n" +
                                "            <p style=\"font-size: 18px; color: #ff0400; margin: 8px 0px; font-weight: 600;\">"

                         + "Total Cost " + itincurformat + " " + _marker.Totalamount + "<br/>    "

                         + "Per Person Cost " + itincurformat + " " + perpersoncost + "<br/>"

                          + "Deposit " + itincurformat + " " + perDeposit + "<br/>"

                          + "Remaining balance due  " + itincurformat + " " + remainingbalance + "</p></div></div>";



                        }

                        if (type == "Confirmed - Paid in full")

                        {

                            divcostdetails = string.Empty;



                        }





                        divwelcomesection = "<div class=\"welcome_section\">\r\n            <p class=\"section_title\" style=\"margin-bottom: 5px; color: #665e5e; font-size: 16px;\">\r\n                <strong>Welcome:</strong></p>\r\n            <p class=\"para body_para\"\r\n                style=\"text-align: left;  font-size: 16px; color: #665e5e; margin: 0 0 15px 0; font-weight: 400; \">\r\n                Ireland has it all for your vacation. From the haunting beauty of the pure, unspoiled landscapes and the\r\n                drama of the coastline, to the urban buzz of the country's dynamic cities mixed with the magic of\r\n                thousands of years' worth of culture and history, Ireland is a country that never fails to surprise.\r\n            </p>\r\n        </div>";

                        if (!string.IsNullOrEmpty(_marker.InclusionNotes))

                        {
                            string divinclusionnotesstart = string.Empty, divinclusionnotesend = string.Empty, divinclusionnotesliststart = string.Empty;

                            divinclusionnotesstart = " <div class=\"welcome_section\" id=\"Inclusionnotes\">\r\n " +

                                " <p class=\"section_title\" style=\"margin-bottom: 5px; color: #665e5e; font-size: 16px;\"><strong>Inclusion:</strong></p>\r\n     " +

                                "<!--  Dynamic List start-->\r\n    " +
                                " <ul class=\"inclusion_list\" style=\"padding-left: 20px; margin-top: 5px;margin-bottom: 15px;\">\r\n  ";


                            if (_marker.InclusionNotes.Contains("\r\n"))
                            {
                                foreach (string note in _marker.InclusionNotes.Split("\r\n"))
                                {
                                    if (!string.IsNullOrEmpty(note))
                                    {
                                        divinclusionnotesliststart = divinclusionnotesliststart + "<li class=\"para body_para\" style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; list-style: disc;\">\r\n     " +
                            " " + note + "\r\n  </li>\r\n   ";
                                    }

                                }
                            }


                            divinclusionnotesend = "  </ul>\r\n   " +
                               "  <!-- Dynamic List end-->\r\n </div>";

                            divinclusionnotes = divinclusionnotesstart + divinclusionnotesliststart + divinclusionnotesend;

                        }

                        else

                        {

                            divinclusionnotes = string.Empty;

                        }

                        if (type == "Quotation")

                        {

                            footerclass = "<footer class=\"terms_conditions\">\r\n            <h1 class=\"main_terms_title\"\r\n                style=\"font-size: 20px; color: #44A826; margin-bottom: 20px; text-align: left;\">\r\n                Terms and Conditions\r\n            </h1>\r\n            <p class=\"para body_para\"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Quote Validity: <br /></strong>\r\n                This quotation is valid for 7 days, and is subject to availability of all quoted services at the time of\r\n                booking.\r\n            </p>\r\n            <p class=\"para body_para\"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Currency: <br /></strong>\r\n                All prices are quoted in Euros. Your deposit and final payment amounts will be due in Euros. Love Irish\r\n                Tours can take\r\n                no responsibility for fluctuations in currency. You can make your balance payment for your tour at an\r\n                earlier time if you\r\n                decide the currency exchange rates are favourable to you.\r\n            </p>\r\n            <p class=\"para body_para\"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Payment: <br /></strong>\r\n                A 20% deposit is required to book your trip.<br />\r\n                The remaining balance is required 60 days prior to the date of your departure.<br />\r\n                Visa, MasterCard or American Express are all acceptable methods of payment.<br />\r\n                Payments can be made here on our website: https://loveirishtours.com/send-payment/\r\n            </p>\r\n            <p class=\"para body_para \"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Travel Insurance: <br /></strong>\r\n                It is a condition of your booking that you take out adequate health and travel insurance for your trip\r\n                to cover all\r\n                eventualities including cancellations related to Covid. Love Irish Tours can take no responsibility if\r\n                you cannot travel due\r\n                to Covid or if you contract Covid while on tour. You should have an adequate travel & health insurance\r\n                plan to cover you\r\n                in this event.\r\n            </p>\r\n\r\n            <p class=\"para body_para\"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Contact: <br /></strong>\r\n                www.loveirishtours.com/ - Ph +353-46-9437555 - Toll-Free Ph 1888-508-6639<br />\r\n                Unit 8 - Scurlockstown Business Park - Trim - Meath - Ireland\r\n            </p>\r\n            <p class=\"para body_para\"\r\n                style=\"font-size: 16px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;\">\r\n                Full terms and conditions found on our website: <a\r\n                    href=\"https://loveirishtours.com/terms-and-conditions/\"\r\n                    style=\"text-decoration: none; color: #665e5e;\" target=\"_blank\">\r\n                    https://loveirishtours.com/terms-and-conditions/</a>\r\n            </p>\r\n        </footer>";



                        }

                        if (type == "Confirmed - Deposit")

                        {

                            footerclass = string.Empty;

                        }

                        if (type == "Confirmed - Paid in full")

                        {

                            footerclass = string.Empty;

                        }

                        if (type == "Quotation")

                        {

                            notesitems = string.Empty;

                        }

                        if (type == "Confirmed - Deposit")

                        {

                            notesitems = string.Empty;

                        }

                        if (type == "Confirmed - Paid in full")

                        {

                            notesitems = string.Empty;

                        }



                    }















                    #region dynamic part with div

                    //                    string divdynamicstart = "<div class=\"dynamic_activity_flow\"><main>";

                    //                    string divdynamicend = " </main>\r\n\r\n        </div>";



                    //                    string divdayinfoheader = "<h1 class=\"activity_day_info\" id=\"dayheader\"\r\n                    style=\"margin: 20px 0px;font-weight: 600; color: #44A826;font-size: 20px;\">"

                    //                 + "Day " + _marker.Daycount + " " + _marker.NameofDate + ", " +

                    //                 _marker.StartDate + " - " + _marker.City + "</h1>";



                    //                    string divactivity_nameheader = "<h2 class=\"activity_name\" id=\"Activityheader\"\r\n                    style=\"font-size: 18px; font-weight: 600; color: black; margin-bottom: 10px; line-height: 30px;\">\r\n                    "

                    //                 + _marker.BookingName + "</h2>";

                    //                    string divactivitydetstart = " <div class=\"activity_details\" id=\"activitydetails\">\r\n                    <div class=\"row\" style=\"display: flex; flex-wrap:nowrap;\">\r\n                        <div class=\"content_boxes_pra\" style=\"max-width: 70%; margin: 0 10px 0 0;\">\r\n                            <p class=\"para body_para\"\r\n                                " +

                    //                        "style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px;\">"

                    //                    + _marker.BodyHtml + "</p>";



                    //                    string divstaydetailnote1 = string.Empty;

                    //                    string divstaydetailnote2 = string.Empty;

                    //                    if (_marker.Type.ToLower() == "accommodation")

                    //                    {

                    //                        string roomname = (_marker.ItemDescription.IndexOf(",") > 0) ? _marker.ItemDescription.Split(",")[1] : _marker.ItemDescription;

                    //                        divstaydetailnote1 = "<div class=\"last_note mt-4\">\r\n                                <p class=\"info_main_note\"\r\n                                    style=\"margin-bottom: 0; font-weight: 600; text-align: left; color: black; font-size: 16px; margin-bottom: 10px;\">" +

                    //                         "Stay at " + _marker.BookingName + "<br/>";

                    //                        divstaydetailnote2 = _marker.NightsDays + " x " + roomname + "</p>\r\n    </div>";

                    //                    }

                    //                    else

                    //                    {

                    //                        divstaydetailnote1 = string.Empty;

                    //                        divstaydetailnote2 = string.Empty;

                    //                    }

                    //                    string divimagepart = string.Empty;

                    //                    if (!string.IsNullOrEmpty(_marker.ReportImage))

                    //                    {

                    //                        divimagepart = "<div class=\"img-boxes\"\r\n style=\"max-width: 30%; margin: 0 10px 0 auto; text-align: right; align-self: stretch;\">"

                    //      + "<img src=\"" + _marker.ReportImage + "\" class=\"side_acivity_img\" style=\"background: #f9f9f9; height: 150px;width: 220px;object-fit: cover;object-position: center; margin: auto;\"/>\r\n       </div>";

                    //                    }

                    //                    else

                    //                    {

                    //                        divimagepart = "";

                    //                    }





                    //                    string divactivitydetend = "</div>\r\n                    </div>\r\n                </div>";





                    #endregion "Dynamic part with div"


                    string imagepath = DBconnEF.GetReportArrowImage();

                    // image.Source = new BitmapImage(uri);  

                    #region dynamic part with table


                    #region "dynamic part for day details start"
                    customerdaydetailscontent = customerdaydetailscontent + "<tr>\r\n" +
    "                            <td width=\"10%\" style=\"border-top:1px solid #999999;text-align: center; vertical-align:middle;line-height: 30px;\">\r\n  " +
    "                              <img src=\"" + imagepath + "\" alt=\"arrow\" style=\"width:20px; margin-top:8px;\"/>\r\n                            </td>\r\n  " +
    "                          <td width=\"20%\" style=\"text-align: center; vertical-align:middle;border-top:1px solid #999999;\"> Day "
    + _marker.Daycount + " </td>\r\n <td width=\"70%\" style=\"text-align: left; vertical-align:middle;border-top:1px solid #999999; line-height: 15px;\">" + _marker.BookingName + "</td>\r\n                        </tr>";


                    #endregion "dynamic part for day details end "

                    string divdynamicstart = "<table class=\"dynamic_activity_flow\" style=\"width: 100%;border-collapse: collapse;\">\r\n    <tr>\r\n\r\n        <td>";

                    string divdynamicend = " </td>\r\n    </tr>\r\n\r\n</table>";



                    string tablestart = "<table style=\"width: 100%;border-collapse: collapse;\">\r\n                <tbody>";

                    string tableend = "</tbody>\r\n            </table>";



                    string divdayinfoheader = "<tr>\r\n <td> <p class=\"activity_day_info\" id=\"dayheader\" style=\"margin: 20px 0px 10px 0px;font-weight: 600; color: #44A826;font-size: 20px;display: inline-block;\">"

                 + "Day " + _marker.Daycount + " " + _marker.NameofDate + ", " + _marker.StartDate;





                    string cityval = string.Empty;

                    if (!string.IsNullOrEmpty(_marker.City))

                    {

                        cityval = " - " + _marker.City + "</p></td>\r\n                    </tr>"; ;

                    }

                    else

                    {

                        cityval = "</p></td>\r\n                    </tr>";

                    }









                    string divactivity_nameheader = "<tr>\r\n                        <td><p class=\"activity_name\" id=\"Activityheader\" style=\"font-size: 18px; font-weight: 600; color: black; margin-bottom: 10px; \">\r\n                           "

                 + _marker.BookingName + "</p></td>\r\n                    </tr>";



                    string divactivitydetstart = "<tr>\r\n                        <td>\r\n                           " +

                        " <table style=\"width: 100%;border-collapse: collapse;\" class=\"activity_details\" id=\"activitydetails\">\r\n " +

                        "                               <tbody>\r\n                                    <tr class=\"content_boxes_pra\">\r\n                                        ";



                    string tdonestart = "<td style=\"width: 70%;vertical-align: top;padding: 15px 15px 15px 0px;\">\r\n\r\n                                            " +

                        "<p class=\"para body_para\"\r\n                                               style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 10px 0; font-weight: 400; line-height: 15px;\">"





                    + _marker.BodyHtml + "</p>";



                    string divstaydetailnote1 = string.Empty;

                    string divstaydetailnote2 = string.Empty;

                    if (_marker.Type.ToLower() == "accommodation")

                    {

                        string roomname = (_marker.ItemDescription.IndexOf(",") > 0) ? _marker.ItemDescription.Split(",")[1] : _marker.ItemDescription;

                        divstaydetailnote1 = "<div class=\"last_note mt-4\">\r\n                                                <p class=\"info_main_note\"\r\n                                                   style=\"margin-bottom: 0; font-weight: 600; text-align: left; color: black; font-size: 16px; margin-bottom: 5px;\">" +

                         "Stay at " + _marker.BookingName + "<br/>";

                        divstaydetailnote2 = _marker.NightsDays + " x " + roomname + "</p>\r\n    </div>";

                    }

                    else

                    {

                        divstaydetailnote1 = string.Empty;

                        divstaydetailnote2 = string.Empty;

                    }

                    string tdoneend = "</td>";

                    string divimagepart = string.Empty;

                    if (!string.IsNullOrEmpty(_marker.ReportImage))

                    {

                        divimagepart = "<td class=\"img-boxes\"\r\n                                            style=\"width: 30%; margin: 0 10px 0 auto; text-align: center; align-self: stretch;vertical-align:top;\">"

      + "<img src=\"" + _marker.ReportImage + "\" class=\"side_acivity_img\" style=\"background: #f9f9f9; height: 150px;width: 250px;object-fit: cover;object-position: center; margin: auto;\"/>\r\n      </td>";

                    }

                    else

                    {

                        divimagepart = "";

                    }





                    string divactivitydetend = " </tr></tbody>\r\n                            </table><br/>\r\n                        </td>\r\n                    </tr>";





                    #endregion "Dynamic part with table"

                    string dynamicpart = divdynamicstart + tablestart + divdayinfoheader + cityval + divactivity_nameheader + divactivitydetstart + tdonestart + divstaydetailnote1 + divstaydetailnote2 +

                        tdoneend + divimagepart + divactivitydetend + tableend + divdynamicend;



                    dynamicpartcontent.AppendLine(dynamicpart);

                    _marker.ItineraryID = Bkit.ItineraryID;
                    i++;
                }

                #region "Payment Link Start"
                if (type == "Quotation" || type == "Confirmed - Deposit")
                {

                    string divPaymentlinkstart = string.Empty;
                    string CustomerPaymentlink = string.Empty;

                     CustomerPaymentlink = DBconnEF.GetCustomerpaymentlink();
                    //CustomerPaymentlink = "http://192.168.10.146/lit_live/itinerary-stripe-payment/?id=##&pid=##";

                    //divPaymentlinkstart = "<table style=\"width: 50%;float: left;text-align:center; margin-top:20px;\"> <tr>" +
                    //    "<td style=\"width:150px;height:35px;text-align: center; vertical-align:middle; line-height: 35px;border:3px solid #44A826; color:#44A826;font-weight:bold; \">" +
                    //  "<a href =\"" + CustomerPaymentlink + "\" style=\"text-decoration:none; color:#44A826; target=\"_blank\">" + "Pay Now</a></td></tr></table>" +
                    //    "<div style=\"clear:both;\"></div>";
                    divPaymentlinkstart = "<table style=\"line-height: 30px;margin-top: 15px;\">\r\n<tbody>\r\n <tr>\r\n  <td  style=\"width: 120px;border:3px solid #44A826;padding: 7px 20px;text-align: center;\">" +
                        "<a href=\"" + CustomerPaymentlink + "\" style=\"text-decoration:none; color:#44A826; font-weight:bold; width: 120px; \" target=\"_blank\">" + "Pay Now</a>" +
                        "</td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table>" +
                       "<div style=\"clear:both;\"></div>";

                    //" <a\r\n href=\"https://loveirishtours.com/terms-and-conditions/\"\r\n style=\"text-decoration: none; color: #665e5e;\" target=\"_blank\">\r\n " +
                    //    "                   https://loveirishtours.com/terms-and-conditions/</a>"
                    if (clientTabViewModel.Individualoption == true || clientTabViewModel.Groupoption == true)
                    {
                        staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divPaymentlinkstart + divwelcomesection + divinclusionnotes + secondtableend;
                    }
                    else if (clientTabViewModel.Individualoption == false && clientTabViewModel.Groupoption == false)
                    {
                        staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divwelcomesection + divinclusionnotes + secondtableend;
                    }
                }
                else
                {
                    staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divwelcomesection + divinclusionnotes + secondtableend;
                }
                #endregion "Payment Link End"


                string findl = Customerinfostart + customerinfoend + customerdaydetails + customerdaydetailscontent + customerdaydetailsend + customerquotevaluesstart +
             customerquotevaluescontent + customerquotevaluescontentinclusionpart + customerquotevalueend;

                mainhtmlcontent.AppendLine(strheader);

                mainhtmlcontent.AppendLine(divbodycontentstart);

                mainhtmlcontent.AppendLine(divheadercontent);

                mainhtmlcontent.AppendLine(findl);

                mainhtmlcontent.AppendLine(staticpart);

                mainhtmlcontent.Append(dynamicpartcontent);

                mainhtmlcontent.AppendLine(footerclass);

                mainhtmlcontent.AppendLine(divbodycontentend);

                mainhtmlcontent.AppendLine(footer);



                webBrowsercontentQuotation.NavigateToString(mainhtmlcontent.ToString());



                // Assuming you have the HTML content in your 'mainhtmlcontent' StringBuilder

                string htmlContent = mainhtmlcontent.ToString();


                
                if (RBConfirmedDeposit.IsChecked == true)
                {
                    OutputFileName = $"{_marker.ItineraryName}_{_marker.ItineraryAutoId}_ConfirmedDeposit_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    Selectionoption = "Deposit Confirmation";
                    PDfresfiles = Paymentlinkupdate(type, htmlContent, OutputFileName);
                }

                if (RBConfirmedPaid.IsChecked == true)
                {
                    OutputFileName = $"{_marker.ItineraryName}_{_marker.ItineraryAutoId}_ConfirmedPaid_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    Selectionoption = "Full Payment Confirmation";
                    PDfresfiles = Paymentlinkupdate(type, htmlContent, OutputFileName);
                }

                if (RBQuotation.IsChecked == true)
                {
                    OutputFileName = $"{_marker.ItineraryName}_{_marker.ItineraryAutoId}_Quotation_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                    Selectionoption = "Quotation";
                    PDfresfiles = Paymentlinkupdate(type, htmlContent, OutputFileName);
                }


                if (PDfresfiles.Count > 0)
                {
                    foreach (var k in PDfresfiles)
                    {
                        GenerateHTMLFile(k.FileContestlist, k.OutputFilesname);

                        // Call the method to convert HTML to PDF

                        PdfSharpConvertAndSave(k.FileContestlist, k.OutputFilesname, Selectionoption, Imagepdfhtmlfolderpath);
                    }
                }
                else
                {
                    GenerateHTMLFile(htmlContent, OutputFileName);
                    PdfSharpConvertAndSave(htmlContent, OutputFileName, Selectionoption,Imagepdfhtmlfolderpath);

                }




                #region docs 
                /*
                string fileName = "GeneratedDocument.docx"; // Set the desired file name here.

                byte[] docBytes = ConvertHtmlToDocx(htmlContent);

                if (docBytes != null)
                {
                    File.WriteAllBytes(fileName, docBytes);
                    MessageBox.Show("Word document generated successfully!");
                }
                */
                #endregion
            }

        }

        public List<PDFFilelist> PDfresfiles = new List<PDFFilelist>();
        public string OutputFileName
        {

            get;

            set;

        }

        public string Selectionoption
        {

            get;

            set;

        }

        
        public static void PdfSharpConvertAndSave(string htmlContent, string outputPath, string Selectionoption,string Imagepdfhtmlfolderpath)
        {

            //string outputDirectory = Path.Combine("C:\\SupplierCommunication\\", "Pdf"); // Combine with 'Pdf' subdirectory

            string outputDirectory = Path.Combine(Imagepdfhtmlfolderpath, "Pdf");

            // Ensure the 'Pdf' subdirectory exists

            if (!Directory.Exists(outputDirectory))

            {

                Directory.CreateDirectory(outputDirectory);

            }



            outputPath = Path.Combine(outputDirectory, outputPath);



            // Your HTML content as a string



            byte[] pdfBytes = GeneratePdfFromHtml(htmlContent);



            // Save the PDF to a file

            // Specify the file path where you want to save the PDF

            System.IO.File.WriteAllBytes(outputPath, pdfBytes);



            // Optionally, you can perform additional logic here

            //// Save the PDF to a file

            //string msg = Selectionoption + " generated successfully";

            //MessageBox.Show(msg);



            //using (var ms = new MemoryStream())

            //{

            //    var pdf = TheArtOfDev.HtmlRenderer.PdfSharp.PdfGenerator.GeneratePdf(htmlContent, PdfSharp.PageSize.A4);

            //    pdf.Save(ms);



            //    // Save the PDF to the specified output file path

            //    File.WriteAllBytes(outputPath, ms.ToArray());

            //}

        }





        public void GenerateHTMLFile(string htmlContent, string fileoutpath, string filefor = null)
        {
            string outputDirectory = string.Empty;
            if (filefor == null)
            {

                outputDirectory = Path.Combine(Imagepdfhtmlfolderpath+"\\", "HTMLFilePdf"); // Combine with 'Pdf' subdirectory
            }
            else if (filefor.ToLower() == "html")
            {
                outputDirectory = Path.Combine(Imagepdfhtmlfolderpath+"\\", "HTMLFileForWordDoc"); // Combine with 'Pdf' subdirectory
            }
            else if (filefor.ToLower() == "htmlcoach")
            {
                outputDirectory = Path.Combine(Imagepdfhtmlfolderpath+"\\", "HTMLFileForCoachbooking"); // Combine with 'Pdf' subdirectory
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            fileoutpath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(fileoutpath));
            System.IO.File.WriteAllText(fileoutpath + ".html", htmlContent);
        }

        public static byte[] GeneratePdfFromHtml(string htmlContent)
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document();

                    // Check if document is not null
                    if (document != null)
                    {
                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                        // Check if writer is not null
                        if (writer != null)
                        {
                            document.Open();

                            // Use XMLWorkerHelper to parse HTML and add it to the PDF document
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader(htmlContent));

                            document.Close();
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                            return memoryStream.ToArray();
                        }
                        else
                        {
                            // Handle the case where writer is null
                        }
                    }
                    else
                    {
                        // Handle the case where document is null
                    }
                }

                // Handle other exceptions as needed

            }
            catch (Exception ex)
            {
                // Handle other exceptions as needed
                Console.WriteLine("An error occurred: " + ex.Message);
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }

            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            return null; // Return null in case of an error
        }
        
        private byte[] ConvertHtmlToDocx(string htmlContent)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Create(memStream, WordprocessingDocumentType.Document))
                {
                    MainDocumentPart mainPart = wordDoc.AddMainDocumentPart();
                    mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document();
                    DocumentFormat.OpenXml.Wordprocessing.Body body = mainPart.Document.AppendChild(new DocumentFormat.OpenXml.Wordprocessing.Body());
                    
                    var altChunk = new AltChunk();
                    altChunk.Id = "AltChunkId1";
                    AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(AlternativeFormatImportPartType.Html, "AltChunkId1");
                    using (Stream stream = chunk.GetStream())
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(htmlContent);
                        stream.Write(byteArray, 0, byteArray.Length);
                    }
                    AltChunk altChunk1 = mainPart.Document.Body.Elements<AltChunk>().FirstOrDefault();
                    if (altChunk1 != null)
                    {
                        altChunk1.Remove();
                    }
                    mainPart.Document.Body.AppendChild(altChunk);
                }
                return memStream.ToArray();
            }
        }


        private void PrintPDf()

        {
            try

            {
                if (clientTabViewModel.PassengerDetailsobser.Count == 0)
                {
                    MessageBox.Show("Please provide the passenger details");
                    return;
                }
                else
                {
                    GetReportContentQuotationConfirmation();

                    SendEmail.IsEnabled = true;

                    SendEmail.Foreground = new SolidColorBrush(Color.FromArgb(255, 87, 159, 0));
                }


            }

            catch (Exception ex)

            {

                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");

            }

            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }

        }

        private void Printpdf_Click(object sender, RoutedEventArgs e)

        {
            if (clientTabViewModel.PassengerDetailsobser.Count == 0)
            {
                MessageBox.Show("Please provide the passenger details");
                return;
            }
            else
            {
                GetReportContentQuotationConfirmation();

                SendEmail.IsEnabled = true;

                SendEmail.Foreground = new SolidColorBrush(Color.FromArgb(255, 87, 159, 0));
            }

        }



        private void SendEmail_Click(object sender, RoutedEventArgs e)
        {
            if (PDfresfiles.Count > 0)
            {
                // Sendpdfemail sendpdfemail = new Sendpdfemail(OutputFileName, TxtItineraryName.Text, TxtEmail.Text, Selectionoption, hdnitineraryid.Text);
                Sendpdfemail sendpdfemail = new Sendpdfemail(PDfresfiles, loginusername);
                sendpdfemail.Show();
            }
            else
            {
                MessageBox.Show("Please provide the passenger details");
                return;
            }

        }

        /*Commented by Angappan.S Date: 30-09-2023 reason : Create multiple pdf generation*/
        //private void CmbStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int selectedIndex = CmbStatus.SelectedIndex;

        //    switch (selectedIndex)
        //    {
        //        case 0:
        //            RBConfirmedPaid.IsChecked = true;
        //            webBrowsercontentQuotation.Visibility = Visibility.Visible;
        //           PrintPDf();
        //            break;
        //        case 1:
        //            RBConfirmedDeposit.IsChecked = true;
        //            webBrowsercontentQuotation.Visibility = Visibility.Visible;
        //            PrintPDf();
        //            break;
        //        case 3:
        //            RBQuotation.IsChecked = true;
        //            webBrowsercontentQuotation.Visibility = Visibility.Visible;
        //            PrintPDf();
        //            break;
        //        default:
        //            // If none of the above cases match, you can clear the selection.
        //            RBConfirmedDeposit.IsChecked = false;
        //            RBConfirmedPaid.IsChecked = false;
        //            RBQuotation.IsChecked = false;
        //            webBrowsercontentQuotation.Visibility = Visibility.Hidden;
        //            break;
        //    }

        //    // Manually raise the Checked event for the initially selected radio button.
        //    // This will trigger the event handler and call the PrintPDf method.
        //    // Call the PrintPDf method directly.
        //}

        public void CallPaymentSupplier()
        {
            if (dgItinBooking.SelectedItem != null)
            {
                supplierPayment paymentWindow = new supplierPayment();
                supplierPaymentViewModel supplierPaymentViewModel = new supplierPaymentViewModel(dgItinBooking.SelectedItem as BookingItems);
                paymentWindow.DataContext = supplierPaymentViewModel;
                paymentWindow.Show();
            }
            else
            {
                MessageBox.Show("First Please Select Any booking");
            }


        }

        //private void SupplierPayment_Click(object sender, RoutedEventArgs e)
        //{
        //  CallPaymentSupplier();
        //}

        //pdf
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the DataContext of the clicked element (the data item)
            var x2 = sender as Button;
            var x3 = x2.DataContext as EmailLogsSettingCollection;
            string pdfFilePath = x3.AttachmentPDF;
            //string pdfFilePath = @"D:\Sumit\TFS\LITPrism\LIT\LIT\bin\Debug\net6.0-windows\Edwin.pdf";

            if (string.IsNullOrEmpty(pdfFilePath) || System.IO.Path.GetExtension(pdfFilePath).ToLower()!=".pdf")
            {
                MessageBox.Show("PDF file not found.");
            }
            else
            {
                var webViewWindow = new PdfViewer(pdfFilePath);
                webViewWindow.ShowDialog();
            }
        }

        // email
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var x2 = sender as Button;
            var x3 = x2.DataContext as EmailLogsSettingCollection;

            var htmlContent = x3.EmailBodyContentPreview;

            var webViewWindow = new WebViewHtlmDisplay(htmlContent);
            webViewWindow.ShowDialog();

        }

        public List<PDFFilelist> Paymentlinkupdate(string type, string htmlContent, string OutputFileName)
        {
            List<PDFFilelist> restpdf = new List<PDFFilelist>();
            string leadpassgereamil = string.Empty;
            string leadpassengerid = string.Empty;
            string itineraryid = string.Empty;
            string divPaymentlinkstart = string.Empty;
            string divPaymentlinkend = string.Empty;
            string CustomerPaymentlink = string.Empty;
                
                // CustomerPaymentlink = DBconnEF.GetCustomerpaymentlink();
                CustomerPaymentlink = "http://192.168.10.146/lit_live/itinerary-stripe-payment/?id=##&pid=##";
                if (clientTabViewModel.Groupoption == true)
                {
                    int s = 0;
                    leadpassgereamil = clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().Email;
                    leadpassengerid = clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().Passengerid;
                    itineraryid = (clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().ItineraryID!=null)?
                    clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().ItineraryID: clientTabViewModel.Itineraryid;
                    PDFFilelist PDFList = new PDFFilelist();
                    if (type == "Quotation" || type == "Confirmed - Deposit")
                    {
                        if (htmlContent.Contains("##"))
                        {
                            htmlContent = htmlContent.Replace("?id=##", "?id=" + itineraryid);
                            htmlContent = htmlContent.Replace("&pid=##", "&pid=" + leadpassengerid);
                        }
                    }
                    if (type == "Confirmed - Paid in full")
                    {
                        htmlContent = htmlContent;
                    }

                    PDFList.FileContestlist = htmlContent;
                    string stOutputFileName = Path.GetFileNameWithoutExtension(OutputFileName);
                    stOutputFileName = stOutputFileName + "_" + Convert.ToInt32(s + 1).ToString() + Path.GetExtension(OutputFileName);
                    PDFList.OutputFilesname = stOutputFileName;                    
                    PDFList.EmailID = leadpassgereamil;
                    PDFList.ItineraryName = TxtItineraryName.Text;
                    PDFList.Itineraryid = itineraryid.ToString();
                    PDFList.Passengerid = leadpassengerid;
                    PDFList.Selectionoption = Selectionoption;
                    restpdf.Add(PDFList);

                }
                else if (clientTabViewModel.Individualoption == true)
                {
                    int K = 0;
                    foreach (PassengerDetails pass in clientTabViewModel.PassengerDetailsobser)
                    {
                        string Individualpassgereamil = string.Empty;
                        string Individualpassengerid = string.Empty;
                        string Individualitineraryid = string.Empty;
                        string htmlContentres = string.Empty;
                        PDFFilelist PDFList = new PDFFilelist();
                        leadpassgereamil = clientTabViewModel.PassengerDetailsobser.Where(x => x.Passengerid == pass.Passengerid).FirstOrDefault().Email;
                        leadpassengerid = clientTabViewModel.PassengerDetailsobser.Where(x => x.Passengerid == pass.Passengerid).FirstOrDefault().Passengerid;
                        itineraryid = clientTabViewModel.Itineraryid;//.PassengerDetailsobser.Where(x => x.Passengerid == pass.Passengerid).FirstOrDefault().ItineraryID;
                        if (type == "Quotation" || type == "Confirmed - Deposit")
                        {
                            if (htmlContent.Contains("##"))
                            {
                                htmlContentres = htmlContent.Replace("?id=##", "?id=" + itineraryid);
                                htmlContentres = htmlContentres.Replace("&pid=##", "&pid=" + leadpassengerid);
                            }
                        }
                        if (type == "Confirmed - Paid in full")
                        {
                            htmlContentres = htmlContent;
                        }
                        PDFList.FileContestlist = htmlContentres;
                        string stOutputFileName = Path.GetFileNameWithoutExtension(OutputFileName);
                        stOutputFileName = stOutputFileName + "_" + Convert.ToInt32(K + 1).ToString() + Path.GetExtension(OutputFileName);
                        PDFList.OutputFilesname = stOutputFileName;
                        PDFList.EmailID = leadpassgereamil;
                        PDFList.ItineraryName = TxtItineraryName.Text;
                        PDFList.Itineraryid = itineraryid.ToString();
                        PDFList.Passengerid = leadpassengerid;
                        PDFList.Selectionoption = Selectionoption;
                        restpdf.Add(PDFList);
                        K = K + 1;
                    }
                }
                else if (clientTabViewModel.Individualoption == false && clientTabViewModel.Groupoption == false)
                {
                    
                }
            
            return restpdf;
        }

        public class PDFFilelist
        {
            public string FileContestlist { get; set; }
            public string OutputFilesname { get; set; }
            public string ItineraryName { get; set; }
            public string EmailID { get; set; }
            public string Selectionoption { get; set; }
            public string Itineraryid { get; set; }
            public string Passengerid { get; set; }
            public string Bodyhtml { get; set; }
            public string Attachmentfile { get; set; }
            public string passengerName { get; set; }

        }

        private void SupplierPaymentOption_Click(object sender, RoutedEventArgs e)
        {
            CallPaymentSupplier();
        }






        #endregion "Html Create end"



        #region word document creation start

        List<PDFGenerationModel> WordDoc = new List<PDFGenerationModel>();
        private void btnsaveworddoc_Click(object sender, RoutedEventArgs e)
        {
            var x2 = sender as Button;
            var x3 = x2.DataContext as EmailLogsSettingCollection;
            GetReportContentQuotationConfirmation_WordDoc(x3.ItineraryID.ToString(), x3.Type, x3.RecipientEmail, x3.PassengerId.ToString());
            
            string htmlfile = string.Empty;
            if (PDfresfiles.Count>0)
            {
                htmlfile = Imagepdfhtmlfolderpath+"\\HTMLFileForWordDoc\\" + PDfresfiles[0].OutputFilesname;
            }

            
            string outputDirectory = Path.Combine(Imagepdfhtmlfolderpath+"\\", "HTMLFileForWordDoc"); // Combine with 'Pdf' subdirectory


            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }
            string outputDirectorywrd = Path.Combine(Imagepdfhtmlfolderpath+"\\", "WordDoc"); // Combine with 'Pdf' subdirectory


            if (!Directory.Exists(outputDirectorywrd))
            {
                Directory.CreateDirectory(outputDirectorywrd);
            }
            string htmlcont = System.IO.File.ReadAllText(htmlfile);
            string filename = System.IO.Path.GetFileNameWithoutExtension(htmlfile) + ".docx";

            //string pdfFilePath = @"D:\Sumit\TFS\LITPrism\LIT\LIT\bin\Debug\net6.0-windows\Edwin.pdf";
            if (!string.IsNullOrEmpty(htmlcont))
            {
                byte[] pdfBytes = ConvertHtmlToDocx(htmlcont);
                System.IO.File.WriteAllBytes(Imagepdfhtmlfolderpath+"\\WordDoc\\" + filename, pdfBytes);
            }
            
            /*
             string htmlstart = "<html lang=\"en\" xmlns:o='urn:schemas-micorsoft-com:office:office'\r\nxmlns:w='urn:schemas-micorsoft-com:office:word'\r\nxmlns='http://www.w3.org/TR/REC-html40'>\"";
              string openxml = "<xml><w:WordDocument><w:View>Print</w:View><w:Zoom>100</Zoom>\r\n<w:DoNotOptimizedForBrowser/>\r\n</w:WordDocument></xml></head>";

              string finalhtml = string.Empty;
              string htmlcontxml = htmlcont;

              htmlcontxml.Replace("<html lang=\"en\">", htmlstart);
              htmlcontxml.Replace("</head>", openxml);
              finalhtml = htmlcontxml;

              byte[] byteArray = Encoding.UTF8.GetBytes(finalhtml);
              //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
              MemoryStream stream = new MemoryStream(byteArray);
              System.IO.File.WriteAllBytes("C:\\SupplierCommunication\\WordDoc\\" + filename, byteArray);
            
            */

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = filename; // Default file name
            dlg.DefaultExt = "docx"; // Default file extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {

                dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                WebClient webClient = new WebClient();
                //  webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(Imagepdfhtmlfolderpath+"\\WordDoc\\" + filename),
                    dlg.FileName);
                // Save document
                string filename1 = dlg.FileName;
            }

        }

        public void GetReportContentQuotationConfirmation_WordDoc(string itineraryidval, string type, string email, string passengerid)
        {
            if (!string.IsNullOrEmpty(itineraryidval))
            {
                WordDoc = pdfdal.PDFGenerationView(Guid.Parse(itineraryidval), type);
                if (WordDoc.Count > 0)
                {
                     ChangeTemplateasQuotationconfirmhtmlFor_WordDocument(WordDoc, type, email, passengerid);
                }
                else
                {
                    MessageBox.Show("Please provide the booking details");
                    return;
                }
            }
        }
        private void ChangeTemplateasQuotationconfirmhtmlFor_WordDocument(List<PDFGenerationModel> objpdfm, string type, string email, string passengerid)
       {
            if (type.ToLower().Trim() == "deposit confirmation")
            {
                type = "Confirmed - Deposit";
            }
                if (type.ToLower().Trim() == "full payment confirmation")
                {
                type = "Confirmed - Paid in full";
                    }

                    string strMarkers = String.Empty;

           string filePath = LoadTemplate();

           if (!string.IsNullOrEmpty(filePath))
           {
               strMarkers = File.ReadAllText(filePath);

               StringBuilder SBMaincontent = new StringBuilder(strMarkers);

               string EmailStrMarkers = string.Empty;

               int i = 0;

               StringBuilder mainhtmlcontent = new StringBuilder();

               string strheader = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n\r\n<head>\r\n    <meta charset=\"UTF-8\" />\r\n    <meta name=\"application-name\" />\r\n    <meta name=\"author\" />\r\n    <meta name=\"description\" />\r\n    <meta name=\"generator\" />\r\n    <meta name=\"keywords\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\" />\r\n    <title>LIV PDF</title>\r\n</head>\r\n\r\n<body>";
               string divbodycontentstart = "<div class=\"body_contents\" style=\" padding: 0px 10px; font-family: 'Arial',sans-serif;\">";
               string divbodycontentend = "</div>";

                //string divheadercontent = "<table style=\"width: 100%;\">\r\n<tbody>\r\n    <tr style=\"display: flex; flex-wrap:nowrap; justify-content: flex-end; padding: 15px 0px;\">\r\n " +
                //    "       <td\r\nstyle=\" font-size: 20px; color: #44A826; margin: 0 0 5px 0; font-weight: bold; line-height: 25px;font-family: Arial, sans-serif;\">\r\n" +
                //    "<p class=\"title_name\"\r\n    style=\" font-size: 20px; color: #44A826; margin: 0 0 5px 0; font-weight: bold; line-height: 25px;\">\r\n    Love Irish Tours</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\">\r\n    Unit 8 Scurlockstown Business Park,</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\">\r\n    Trim,</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\">\r\n    Co Meath</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\">\r\n    Ph: 1888-508-6639</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\">\r\n    Website: www.loveirishtours.com</p>\r\n" +
                //    "<p class=\"para top_intro\"\r\n    style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif; margin-bottom: 30px;\">\r\n    Niall Carroll</p>\r\n        " +
                //    "</td>\r\n        <td style=\"display: flex;align-items: center;justify-content: center;\">\r\n<img src=\"https://loveirishtours.com/wp-content/uploads/2017/11/love-irish-tours-logo-optimized.png\"\r\n    alt=\"LIV\" />\r\n        " +
                //    "</td>\r\n    </tr>\r\n</tbody>\r\n        </table>";

                string divheadercontent = "<table style=\"width: 100%;\">\r\n    <tbody>\r\n      <tr style=\"display: flex; flex-wrap:nowrap; justify-content: flex-end; padding: 15px 0px;\">\r\n " +
                "<td\r\n style=\"border-right: 2px solid #44A826; padding-right: 10px; margin-right: 10px; width:75%;text-align: right;\"><p class=\"title_name\"\r\n                           " +
                "style=\" font-size: 20px; color: #44A826; margin: 0 0 5px 0; font-weight: bold; line-height: 25px;font-family: Arial, sans-serif;\"> Love Irish Tours</p>\r\n " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Unit 8 Scurlockstown Business Park,</p>\r\n " +
                "         <p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Trim,</p>\r\n          " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Co Meath</p>\r\n" +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Ph: 1888-508-6639</p>\r\n          " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Website: www.loveirishtours.com</p>\r\n" +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif; margin-bottom: 30px;font-family: Arial, sans-serif;\"\"> Niall Carroll</p></td>\r\n" +
                "<td style=\"display: flex;align-items: center;justify-content: center;\"><img src=\"https://loveirishtours.com/wp-content/uploads/2017/11/love-irish-tours-logo-optimized.png\"\r\n                            alt=\"LIV\" /></td>\r\n      </tr>\r\n    </tbody>\r\n  </table>";


                string footer = "</body>\r\n</html>";

               string footerclass = string.Empty;

               string divclientstart = string.Empty;
               string divcostdetails = string.Empty;
               string divcostdetailsend = string.Empty;

               string divwelcomesection = string.Empty;
               string divinclusionnotes = string.Empty;

               string notesitems = string.Empty;
               string staticpart = string.Empty;
               string secondtablestart = string.Empty;
               string secondtableend = string.Empty;
               string enddate = string.Empty;


               string Customerinfostart = string.Empty;
               string customerinfoend = string.Empty;
               string customerdaydetails = string.Empty;
               string customerdaydetailsend = string.Empty;
               string customerdaydetailscontent = string.Empty;
               string customerquotevaluesstart = string.Empty;
               string customerquotevaluescontent = string.Empty;
               string customerquotevalueend = string.Empty;
               string customerquotevaluescontentinclusionpart = string.Empty;
               string docenddate = string.Empty;

               StringBuilder dynamicpartcontent = new StringBuilder();

               //   PDFGenerationModel _marker = new PDFGenerationModel();

               foreach (PDFGenerationModel Bkit in objpdfm)
               {
                   string perpersoncost = string.Empty;

                   if (Bkit.PaxNumbers > 0)
                   {
                       perpersoncost = (Convert.ToDecimal(Bkit.Totalamount / Bkit.PaxNumbers)).ToString("0.00");
                   }
                   else
                   {
                       perpersoncost = (Convert.ToDecimal(Bkit.Totalamount)).ToString("0.00");
                   }

                   string perDeposit = string.Empty;

                   string remainingbalance = string.Empty;

                   string selected = string.Empty;

                   if (type.ToLower() == "confirmed - deposit" || type.ToLower() == "confirmed - paid in full")
                   {
                       selected = "Final Itinerary for";
                   }
                   if (type.ToLower() == "quotation")
                   {
                       selected = "Quotation for  ";
                   }
                   if (i == 0)
                   {
                       if (type == "Quotation")
                       {
                           perDeposit = (Convert.ToDecimal((Bkit.Totalamount * 20) / 100)).ToString("0.00");
                       }
                       if (type == "Confirmed - Deposit")
                       {
                           perDeposit = Bkit.Deposit.ToString();
                           remainingbalance = Convert.ToDecimal(Bkit.Totalamount - Bkit.Deposit).ToString("0.00");
                       }
                       if (type == "Confirmed - Paid in full")
                       {
                           perDeposit = string.Empty;
                       }

                       if (!string.IsNullOrEmpty(Bkit.ItineraryEndDate))
                       {
                           docenddate = Bkit.ItineraryStartDate + " - " + Bkit.ItineraryEndDate;
                       }
                       else
                       {
                           docenddate = Bkit.ItineraryStartDate;
                       }

                       string tourfor = (ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault() != null) ?
                           ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault().Tourlistname.ToString() : string.Empty;

                       Customerinfostart = "<div id=\"Customerinfo\">\r\n           " +
                           " <h1 style=\"font-size: 16px; color: #44A826; margin-bottom: 15px; text-align:center;font-weight: bold;\">\r\n   " +
                           "             " + tourfor + " Itinerary for " + Bkit.ItineraryName + "</h1>\r\n       " +
                           "     <h2 style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left;\">\r\n\r\n   " +
                           "             Thank you for considering Love Irish Tours to take care of your travel plans for your Magical trip to\r\n               " + tourfor + ".\r\n            </h2>\r\n      " +
                           "      <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"\">\r\n      " +
                           "    <span style=\"font-weight: 400;color:#44A826;\"> Name: </span>" + Bkit.ItineraryName + "</p>\r\n          " +
                           "  <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n       " +
                           "        <span style=\"font-weight: 400;color:#44A826;\"> Email Address: </span>" + Bkit.Email + "</p>\r\n  " +
                           "          <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                           "            <span style=\"font-weight: 400;color:#44A826;\">  Contact Number:</span> " + Bkit.Phone + "</p>\r\n " +
                           "           <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                           "           <span style=\"font-weight: 400;color:#44A826;\">   Ireland Trip Dates:</span> " + docenddate + " </p>\r\n " +
                           "           <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n  " +
                           "           <span style=\"font-weight: 400;color:#44A826;\">   Tour Type: </span> " + Bkit.Tourtype + "</p>\r\n       " +
                           "     <p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">\r\n " +
                           "         <span style=\"font-weight: 400;color:#44A826;\"> Itinerary ID:</span> " + Bkit.ItineraryAutoId + " </p>";
                       customerinfoend = " </div>\r\n<br/>\r\n<br/>";


                       customerquotevaluesstart = "<td style=\"width: 7%;float: left;border:0px;\" id=\"Middletable\">" +
                           " <table width=\"80%\" cellpadding=\"5\" style=\"padding-left:5px;border:0px solid #ffffff;border: 0px;\" >\r\n                    <tbody>\r\n                      " +
                           "  <tr>\r\n<td style=\"padding:5px;border: 0px;\">\r\n&nbsp;\r\n  </td>\r\n  </tr>\r\n  \r\n</tbody>\r\n </table>\r\n            </td>" +
                           "<td style=\"width: 38%;float: left;border: 0px;\" id=\"customerquote\">\r\n " +
                           "<table cellpadding=\"5\" style=\"width: 300px;padding-left: 25px;font-family: Arial, sans-serif;\"><tbody>";
                       customerquotevaluescontent =
                       "   <tr>\r\n<td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\"\">\r\n " +
                       "<p style=\"font-size: 14px;margin-bottom: 5px;color: #44A826;font-family: Arial, sans-serif;\">\r\n " +
                       "  Price Per Person <br/><b>\r\n " + itincurformat + " " + perpersoncost + "\r\n</b> </p>\r\n  </td>\r\n  " + "</tr>\r\n  " +
                       "<tr>\r\n      <td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\">\r\n" +
                       "<p style=\"font-size: 14px;text-align: left; margin-bottom: 5px;color: #44A826;font-family: Arial, sans-serif;\">\r\n    " +
                       "Deposit Per Person <br/><b>\r\n " + itincurformat + " " + perDeposit + "\r\n</b></p>\r\n      </td>\r\n  </tr>\r\n  " +
                       "<tr>\r\n      <td style=\"padding: 0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;text-align: left;border: 1px solid #333;\">\r\n" +
                       "<p style=\"font-size: 14px;margin-bottom: 5px;color: #44A826;font-family: Arial, sans-serif;\">\r\n    " +
                       "Duration <br/><b>\r\n    " + Bkit.nightdayvalues + " days " + (Convert.ToInt32(Bkit.nightdayvalues) - 1).ToString() + " nights\r\n</b></p>\r\n      </td>\r\n  </tr>\r\n  ";


                       customerquotevalueend = "</tbody>\r\n </table>\r\n  </td></tr>\r\n     </tbody>\r\n        </table><div style=\"page-break-before:always\">&nbsp;</div>";

                       string quoinclusionnotesstart = string.Empty, quoinclusionnotesend = string.Empty, quoinclusionnotesliststart = string.Empty, quoinclusion = string.Empty;

                       quoinclusionnotesstart = "<ul class=\"inclusion_list\" style=\"padding-left: 20px; margin-top: 5px;margin-bottom: 15px;\">\r\n  ";


                       if (Bkit.InclusionNotes.Contains("\r\n"))
                       {
                           foreach (string note in Bkit.InclusionNotes.Split("\r\n"))
                           {
                               if (!string.IsNullOrEmpty(note))
                               {
                                   quoinclusionnotesliststart = quoinclusionnotesliststart + "<li class=\"para body_para\" style=\"text-align: left;  font-size: 14px; color: #44A826; margin: 0 0 5px 0; font-weight: 400; list-style: disc;font-family: Arial, sans-serif;\">\r\n     " +
                       " " + note + "\r\n  </li>\r\n   ";
                               }

                           }
                       }
                       quoinclusionnotesend = "  </ul>\r\n   ";
                       quoinclusion = quoinclusionnotesstart + quoinclusionnotesliststart + quoinclusionnotesend;

                       customerquotevaluescontentinclusionpart = "<tr>\r\n      <td style=\"padding:0px 8px 8px 8px;background: #cfe2f3;border-radius:15px;border: 1px solid #333;\">" +
                           "\r\n<p style=\"font-size: 14px;text-align: left;margin-bottom: 5px;color: #44A826;font-family: Arial, sans-serif;\">\r\n    " +
                           "Inclusion " + quoinclusion + "\r\n</p>\r\n      </td>\r\n  </tr>\r\n";

                       customerdaydetails = " <table style=\"width: 100%;\">\r\n <tbody>\r\n<tr>\r\n <td style=\"width: 48%;float: left;background: #f5f5f5;\" id=\"Customerdayinfoloop\">" +
                           "<table width=\"100%\"\r\n style=\" border: 1px solid #999;border-top:0px;border-bottom:3px solid #44A826;\"\r\n cellpadding=\"6\" cellspacing=\"0\">\r\n <tbody>";
                       customerdaydetailsend = "</tbody>\r\n </table></td>";




                       secondtablestart = "<div class=\"cilent_update_info\">";

                       secondtableend = "</div><br/>";



                       divclientstart = "<table style=\"width: 100%;margin-top:10px;\">\r\n <tbody>\r\n     <tr>\r\n <td>\r\n " +
                            "<h1 class=\"clientname\"\r\n  style=\"font-size: 16px; color: #44A826; margin-bottom: 8px; text-align:left;font-weight: bold;font-family: Arial, sans-serif;\">" +
                        selected + " " + Bkit.ItineraryName + "</h1>" +
                          "<h2 class=\"arrival_time\" style=\"font-size: 14px; color: rgb(102, 94, 94); margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;>" +
                          "Arriving " + Bkit.NameofItineraryStartDate + ", " + Bkit.ItineraryStartDate + ", departing ";

                       if (!string.IsNullOrEmpty(Bkit.ItineraryEndDate))
                       {
                           enddate = Bkit.NameofItineraryEndDate + ", " + Bkit.ItineraryEndDate + "</h2></td>\r\n </tr>\r\n</tbody>\r\n  </table>";
                       }
                       else
                       {
                           enddate = Bkit.NameofItineraryEndDate + "</h2></td>\r\n </tr>\r\n </tbody>\r\n </table>";
                       }
                       if (type == "Quotation")
                       {
                           divcostdetails = "<div style=\"clear:both;\"></div><table style=\"width: 100%;\">\r\n <tbody>\r\n  <tr>\r\n <td style=\"width: 50%;float: left;\"><div class=\"cost_status_block\" id=\"Costdetails\" style=\"margin: 0px 0 30px 0;\">\r\n" +
                               "            <p style=\"font-size: 18px; color: #ff0400; margin: 8px 0px; font-weight: 600;font-family: Arial, sans-serif;\">"
                        + "Total Cost " + itincurformat + " " + Bkit.Totalamount + "<br/>    "
                        + "Per Person Cost " + itincurformat + " " + perpersoncost + "<br/>"
                         + "Deposit " + itincurformat + " " + perDeposit + "</p></div></td>";
                       }
                       if (type == "Confirmed - Deposit")
                       {
                           divcostdetails = "<div style=\"clear:both;\"></div><table style=\"width: 100%;\">\r\n <tbody>\r\n  <tr>\r\n " +
                               "<td style=\"width: 50%;float: left;\"><div class=\"cost_status_block\" id=\"Costdetails\" style=\"margin: 0px 0 30px 0;\">\r\n" +
                               "            <p style=\"font-size: 18px; color: #ff0400; margin: 8px 0px; font-weight: 600;font-family: Arial, sans-serif;\">"

                        + "Total Cost " + itincurformat + " " + Bkit.Totalamount + "<br/>    "
                        + "Per Person Cost " + itincurformat + " " + perpersoncost + "<br/>"
                         + "Deposit " + itincurformat + " " + perDeposit + "<br/>"
                         + "Remaining balance due  " + itincurformat + " " + remainingbalance + "</p></div></td>";
                       }
                       if (type == "Confirmed - Paid in full")
                       {
                           divcostdetails = string.Empty;
                       }

                       divwelcomesection = "<div style=\"clear:both;\"></div> <div class=\"welcome_section\">\r\n <p class=\"section_title\" style=\"margin-bottom: 5px; color: #665e5e; font-size: 14px;\">\r\n                    <strong>Welcome:</strong>\r\n                </p>\r\n                <p class=\"para body_para\"\r\n                    style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 15px 0; font-weight: 400;font-family: Arial, sans-serif;line-height: 18px; \">\r\n                    Ireland has it all for your vacation. From the haunting beauty of the pure, unspoiled landscapes and\r\n                    the\r\n                    drama of the coastline, to the urban buzz of the country's dynamic cities mixed with the magic of\r\n                    thousands of years' worth of culture and history, Ireland is a country that never fails to surprise.\r\n  </p>\r\n  </div>";

                       if (!string.IsNullOrEmpty(Bkit.InclusionNotes))
                       {
                           string divinclusionnotesstart = string.Empty, divinclusionnotesend = string.Empty, divinclusionnotesliststart = string.Empty;

                           divinclusionnotesstart = " <div class=\"welcome_section\" id=\"Inclusionnotes\">\r\n " +

                               " <p class=\"section_title\" style=\"margin-bottom: 5px; color: #665e5e; font-size: 14px;\"><strong>Inclusion:</strong></p>\r\n     " +

                               "<!--  Dynamic List start-->\r\n    " +
                               " <ul class=\"inclusion_list\" style=\"padding-left: 20px; margin-top: 5px;margin-bottom: 15px;\">\r\n  ";


                           if (Bkit.InclusionNotes.Contains("\r\n"))
                           {
                               foreach (string note in Bkit.InclusionNotes.Split("\r\n"))
                               {
                                   if (!string.IsNullOrEmpty(note))
                                   {
                                       divinclusionnotesliststart = divinclusionnotesliststart + "<li class=\"para body_para\" style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; list-style: disc;\">\r\n     " +
                           " " + note + "\r\n  </li>\r\n   ";
                                   }

                               }
                           }
                           divinclusionnotesend = "  </ul>\r\n   " +
                              "  <!-- Dynamic List end-->\r\n </div>";

                           divinclusionnotes = divinclusionnotesstart + divinclusionnotesliststart + divinclusionnotesend;
                       }
                       else
                       {
                           divinclusionnotes = string.Empty;
                       }
                       if (type == "Quotation")
                       {
                           footerclass = "<footer class=\"terms_conditions\">\r\n            <h1 class=\"main_terms_title\"\r\n                style=\"font-size: 16px; color: #44A826; margin-bottom: 20px; text-align: left;\">\r\n                Terms and Conditions\r\n            </h1>\r\n   " +
                                "         <p class=\"para body_para\"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">Quote Validity: <br /></strong>\r\n                This quotation is valid for 7 days, and is subject to availability of all quoted services at the time of\r\n                booking.\r\n            </p>\r\n  " +
                                "          <p class=\"para body_para\"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\"><br />" +
                                "Currency: <br /></strong>\r\n                All prices are quoted in Euros. Your deposit and final payment amounts will be due in Euros. Love Irish\r\n                Tours can take\r\n                no responsibility for fluctuations in currency. You can make your balance payment for your tour at an\r\n                earlier time if you\r\n                decide the currency exchange rates are favourable to you.\r\n            </p>\r\n  " +
                                "          <p class=\"para body_para\"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\"><br />" +
                                "Payment: <br /></strong>\r\n                A 20% deposit is required to book your trip.<br />\r\n                The remaining balance is required 60 days prior to the date of your departure.<br />\r\n                Visa, MasterCard or American Express are all acceptable methods of payment.<br />\r\n                Payments can be made here on our website: https://loveirishtours.com/send-payment/\r\n            </p>\r\n           " +
                                " <p class=\"para body_para \"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\">" +
                                "<br />Travel Insurance: <br /></strong>\r\n                It is a condition of your booking that you take out adequate health and travel insurance for your trip\r\n                to cover all\r\n                eventualities including cancellations related to Covid. Love Irish Tours can take no responsibility if\r\n                you cannot travel due\r\n                to Covid or if you contract Covid while on tour. You should have an adequate travel & health insurance\r\n                plan to cover you\r\n                in this event.\r\n            </p>\r\n\r\n " +
                                "           <p class=\"para body_para\"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                <strong style=\"display: block; margin-bottom: 5px;\"><br />" +
                                "Contact: <br /></strong>\r\n                www.loveirishtours.com/ - Ph +353-46-9437555 - Toll-Free Ph 1888-508-6639<br />\r\n                Unit 8 - Scurlockstown Business Park - Trim - Meath - Ireland\r\n            </p>\r\n  " +
                                "          <p class=\"para body_para\"\r\n                style=\"font-size: 14px; text-align: left; color: #665e5e; margin: 0 0 15px 0px; font-weight: 400;line-height: 18px;\">\r\n                Full terms and conditions found on our website: " +
                                "<a\r\n                    href=\"https://loveirishtours.com/terms-and-conditions/\"\r\n                    style=\"text-decoration: none; color: #665e5e;\" target=\"_blank\">\r\n                    https://loveirishtours.com/terms-and-conditions/</a>\r\n            </p>\r\n        </footer>";
                       }
                       if (type == "Confirmed - Deposit")
                       {
                           footerclass = string.Empty;
                       }
                       if (type == "Confirmed - Paid in full")
                       {
                           footerclass = string.Empty;
                       }
                       if (type == "Quotation")
                       {
                           notesitems = string.Empty;
                       }
                       if (type == "Confirmed - Deposit")
                       {
                           notesitems = string.Empty;
                       }
                       if (type == "Confirmed - Paid in full")
                       {
                           notesitems = string.Empty;
                       }
                   }

                   string imagepath = DBconnEF.GetReportArrowImage();

                   #region dynamic part with table

                   #region "dynamic part for day details start"
                   customerdaydetailscontent = customerdaydetailscontent + "<tr>\r\n" +
   "                            <td width=\"10%\" style=\"border-top:1px solid #999999;text-align: center; vertical-align:middle;line-height: 30px;\">\r\n  " +
   "                              <img src=\"" + imagepath + "\" alt=\"arrow\" style=\"width:20px; margin-top:8px;\"/>\r\n                            </td>\r\n  " +
   "                          <td width=\"20%\" style=\"text-align: center; vertical-align:middle;border-top:1px solid #999999;font-family: Arial, sans-serif;\"> Day "
   + Bkit.Daycount + " </td>\r\n <td style=\"text-align: left; vertical-align:middle;border-top:1px solid #999999; line-height: 15px;font-family: Arial, sans-serif;\">" + Bkit.BookingName + "</td>\r\n                        </tr>";


                   #endregion "dynamic part for day details end "

                   string divdynamicstart = "<table class=\"dynamic_activity_flow\" style=\"width: 100%;border-collapse: collapse;\">\r\n    <tr>\r\n\r\n        <td>";
                   string divdynamicend = " </td>\r\n    </tr>\r\n\r\n</table>";

                   string tablestart = "<table style=\"width: 100%;border-collapse: collapse;\">\r\n                <tbody>";
                   string tableend = "</tbody>\r\n            </table>";

                   string divdayinfoheader = "<tr>\r\n <td> <p class=\"activity_day_info\" id=\"dayheader\" style=\"margin: 20px 0px 10px 0px;font-weight: 600; color: #44A826;font-size: 16px;display: inline-block;font-family: Arial, sans-serif;\">"
                + "Day " + Bkit.Daycount + " " + Bkit.NameofDate + ", " + Bkit.StartDate;

                   string cityval = string.Empty;

                   if (!string.IsNullOrEmpty(Bkit.City))
                   {
                       cityval = " - " + Bkit.City + "</p></td>\r\n                    </tr>";
                   }
                   else
                   {
                       cityval = "</p></td>\r\n                    </tr>";
                   }
                   string divactivity_nameheader = "<tr>\r\n <td><p class=\"activity_name\" id=\"Activityheader\" style=\"font-size: 16px; font-weight: 600; color: black; margin-bottom: 10px;font-family: Arial, sans-serif;\">"
                + Bkit.BookingName + "</p></td>\r\n  </tr>";

                   string divactivitydetstart = "<tr>\r\n <td>\r\n   " +
                       " <table style=\"width: 100%;border-collapse: collapse;\" class=\"activity_details\"\r\n id=\"activitydetails\">\r\n " +
                       " <tbody>\r\n <tr class=\"content_boxes_pra\">\r\n ";

                   string tdonestart = "<td style=\"width: 70%;vertical-align: top;padding: 15px 15px 15px 0px;\">\r\n\r\n                                            " +

                       "<p class=\"para body_para\"\r\n  style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 10px 0; font-weight: 400; line-height: 18px;\">"
                   + Bkit.BodyHtml + "</p>";

                   string divstaydetailnote1 = string.Empty;
                   string divstaydetailnote2 = string.Empty;
                   if (Bkit.Type.ToLower() == "accommodation")
                   {
                       string roomname = (Bkit.ItemDescription.IndexOf(",") > 0) ? Bkit.ItemDescription.Split(",")[1] : Bkit.ItemDescription;
                       divstaydetailnote1 = "<div class=\"last_note mt-4\">\r\n  <p class=\"info_main_note\"\r\n  style=\"margin-bottom: 0; font-weight: 600; text-align: left; color: black; font-size: 14px; margin-bottom: 5px;\">" +
                        "Stay at " + Bkit.BookingName + "<br/>";

                       divstaydetailnote2 = Bkit.NightsDays + " x " + roomname + "</p>\r\n    </div>";
                   }
                   else
                   {
                       divstaydetailnote1 = string.Empty;
                       divstaydetailnote2 = string.Empty;
                   }

                   string tdoneend = "</td>";

                   string divimagepart = string.Empty;

                   if (!string.IsNullOrEmpty(Bkit.ReportImage))
                   {
                       divimagepart = "<td class=\"img-boxes\"\r\n style=\"width: 30%; margin: 0 10px 0 auto; text-align: center; align-self: stretch;vertical-align:top;\">"
     + "<img src=\"" + Bkit.ReportImage + "\" class=\"side_acivity_img\" style=\"background: #f9f9f9; height: 150px;width: 50%;object-fit: cover;object-position: center; margin: auto;\"/>\r\n      </td>";

                   }
                   else
                   {
                       divimagepart = "";
                   }

                   string divactivitydetend = " </tr></tbody>\r\n                            </table><br/>\r\n                        </td>\r\n                    </tr>";

                   #endregion "Dynamic part with table"

                   string dynamicpart = divdynamicstart + tablestart + divdayinfoheader + cityval + divactivity_nameheader + divactivitydetstart + tdonestart + divstaydetailnote1 + divstaydetailnote2 +

                       tdoneend + divimagepart + divactivitydetend + tableend + divdynamicend;



                   dynamicpartcontent.AppendLine(dynamicpart);

                   Bkit.ItineraryID = Bkit.ItineraryID;
                    i++;

               }

               #region "Payment Link Start"
               if (type.ToLower() == "quotation" || type.ToLower() == "confirmed - deposit")
               {

                   string divPaymentlinkstart = string.Empty;
                   string CustomerPaymentlink = string.Empty;

                   CustomerPaymentlink = DBconnEF.GetCustomerpaymentlink();
                    //CustomerPaymentlink = "http://192.168.10.146/lit_live/itinerary-stripe-payment/?id=##&pid=##";

                    //divPaymentlinkstart = "<table style=\"width: 50%;float: left;text-align:center; margin-top:20px;\"> <tr>" +
                    //    "<td style=\"width:150px;height:35px;text-align: center; vertical-align:middle; line-height: 35px;border:3px solid #44A826; color:#44A826;font-weight:bold; \">" +
                    //  "<a href =\"" + CustomerPaymentlink + "\" style=\"text-decoration:none; color:#44A826; target=\"_blank\">" + "Pay Now</a></td></tr></table>" +
                    //    "<div style=\"clear:both;\"></div>";
                    divPaymentlinkstart = "<td style=\"width: 40%;float: left;\"> <table style=\"line-height: 30px;margin-top: 15px;\">\r\n<tbody>\r\n  <tr>\r\n <td>" +
                        "<a href=\"" + CustomerPaymentlink + "\" style=\"text-decoration:none; color:#44A826; font-weight:bold; width: 120px; \" target=\"_blank\">" + "Pay Now</a>" +
                        "</td>\r\n                        </tr>\r\n                    </tbody>\r\n                </table></td>";
                      

                   //" <a\r\n href=\"https://loveirishtours.com/terms-and-conditions/\"\r\n style=\"text-decoration: none; color: #665e5e;\" target=\"_blank\">\r\n " +
                   //    "                   https://loveirishtours.com/terms-and-conditions/</a>"

                   divcostdetailsend = "</tr>\r\n </tbody>\r\n</table> <div style=\"clear:both;\"></div>";
                   if (clientTabViewModel.Individualoption == true || clientTabViewModel.Groupoption == true)
                   {
                       staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divPaymentlinkstart + divcostdetailsend + divwelcomesection + divinclusionnotes + secondtableend;
                   }
                   else if (clientTabViewModel.Individualoption == false && clientTabViewModel.Groupoption == false)
                   {
                       staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divcostdetailsend + divwelcomesection + divinclusionnotes + secondtableend;
                   }
               }
               else
               {
                   staticpart = secondtablestart + divclientstart + enddate + divcostdetails + divcostdetailsend + divwelcomesection + divinclusionnotes + secondtableend;
               }
               #endregion "Payment Link End"


               string findl = Customerinfostart + customerinfoend + customerdaydetails + customerdaydetailscontent + customerdaydetailsend + customerquotevaluesstart +
            customerquotevaluescontent + customerquotevaluescontentinclusionpart + customerquotevalueend;

               mainhtmlcontent.AppendLine(strheader);

               mainhtmlcontent.AppendLine(divbodycontentstart);

               mainhtmlcontent.AppendLine(divheadercontent);

               mainhtmlcontent.AppendLine(findl);

               mainhtmlcontent.AppendLine(staticpart);

               mainhtmlcontent.Append(dynamicpartcontent);

               mainhtmlcontent.AppendLine(footerclass);

               mainhtmlcontent.AppendLine(divbodycontentend);

               mainhtmlcontent.AppendLine(footer);



               // webBrowsercontentQuotation.NavigateToString(mainhtmlcontent.ToString());



               // Assuming you have the HTML content in your 'mainhtmlcontent' StringBuilder

               string htmlContent = mainhtmlcontent.ToString();

               string ItineraryName = TxtItineraryName.Text;
               string ItineraryAutoId = hdnitineraryAutono.Text;

                    if (type == "Confirmed - Deposit")
                    {

                   OutputFileName = $"{ItineraryName}_{ItineraryAutoId}_ConfirmedDeposit_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_word" + ".html";
                   Selectionoption = "Deposit Confirmation";
                   PDfresfiles = Paymentlinkupdate_worddoc(type, htmlContent, OutputFileName,email,passengerid);
               }

                if (type == "Confirmed - Paid in full")
                {
                   OutputFileName = $"{ItineraryName}_{ItineraryAutoId}_ConfirmedPaid_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_word" + ".html";
                   Selectionoption = "Full Payment Confirmation";
                    PDfresfiles = Paymentlinkupdate_worddoc(type, htmlContent, OutputFileName, email, passengerid);
                }
               if (type.ToLower() == "quotation")
               {
                   OutputFileName = $"{ItineraryName}_{ItineraryAutoId}_Quotation_" + DateTime.Now.ToString("yyyyMMddhhmmss") + "_word" + ".html";
                   Selectionoption = "Quotation";
                   PDfresfiles = Paymentlinkupdate_worddoc(type, htmlContent, OutputFileName, email, passengerid);
               }


               if (PDfresfiles.Count > 0)
               {
                   foreach (var k in PDfresfiles)
                   {
                       GenerateHTMLFile(k.FileContestlist, k.OutputFilesname, "html");
                   }
               }
               else
               {
                   GenerateHTMLFile(htmlContent, OutputFileName, "html");
               }
           }

       }

        public List<PDFFilelist> Paymentlinkupdate_worddoc(string type, string htmlContent, string OutputFileName, string email, string passengerid)
        {
            List<PDFFilelist> restpdf = new List<PDFFilelist>();

           
                string leadpassgereamil = string.Empty;
                string leadpassengerid = string.Empty;
                string itineraryid = string.Empty;
                string divPaymentlinkstart = string.Empty;
                string divPaymentlinkend = string.Empty;
                string CustomerPaymentlink = string.Empty;

                int s = 0;
                leadpassgereamil = email; //clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().Email;
                leadpassengerid = passengerid; //clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).FirstOrDefault().Passengerid;
                itineraryid = hdnitineraryid.Text;
                PDFFilelist PDFList = new PDFFilelist();
            if (type == "Quotation" || type == "Confirmed - Deposit")
            {
                if (htmlContent.Contains("##"))
                {
                    htmlContent = htmlContent.Replace("?id=##", "?id=" + itineraryid);
                    htmlContent = htmlContent.Replace("&pid=##", "&pid=" + leadpassengerid);
                }

            }
                PDFList.FileContestlist = htmlContent;
                string stOutputFileName = Path.GetFileNameWithoutExtension(OutputFileName);
                stOutputFileName = stOutputFileName + "_" + Convert.ToInt32(s + 1).ToString() + Path.GetExtension(OutputFileName);
                PDFList.OutputFilesname = stOutputFileName;
                PDFList.EmailID = leadpassgereamil;
                PDFList.ItineraryName = TxtItineraryName.Text;
                PDFList.Itineraryid = itineraryid.ToString();
                PDFList.Passengerid = leadpassengerid;
                PDFList.Selectionoption = Selectionoption;
                restpdf.Add(PDFList);


             
            return restpdf;
        }

        #endregion word document creation end




        /* Pdf Creation purpose Html Create end */



        #region "Coach booking form start

        List<CoachBookingModel> coachbkgdata = new List<CoachBookingModel>();
        List<Attractionlist> coachbkgAttractiondata  = new List<Attractionlist>();
        List<PassengerNameRoomlist> coachbkgPassengerdata = new List<PassengerNameRoomlist>();
        private void PrintCoachBooking()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;                
                if (!string.IsNullOrEmpty(hdnitineraryid.Text))
                {
                    string finaloutfilename = string.Empty;
                    coachbkgdata = pdfdal.CoachBookingGenerationView(Guid.Parse(hdnitineraryid.Text));
                    if (coachbkgdata.Count > 0)
                    {
                        coachbkgPassengerdata = pdfdal.CoachBookingPassengerList(Guid.Parse(hdnitineraryid.Text));
                        coachbkgAttractiondata = pdfdal.CoachBookingAttractionList(Guid.Parse(hdnitineraryid.Text));
                        if (coachbkgdata.Count > 0)
                        {
                            GetCoachbookingformhtml(coachbkgdata, coachbkgPassengerdata, coachbkgAttractiondata,ref finaloutfilename);

                            if (!string.IsNullOrEmpty(finaloutfilename))
                            {
                                string outputDirectoryhtm = Path.Combine(Imagepdfhtmlfolderpath+"\\", "HTMLFileForCoachbooking"); 


                                if (!Directory.Exists(outputDirectoryhtm))
                                {
                                    Directory.CreateDirectory(outputDirectoryhtm);
                                }

                                string outputDirectorywrd = Path.Combine(Imagepdfhtmlfolderpath+"\\", "WordDocCoachbooking"); 


                                if (!Directory.Exists(outputDirectorywrd))
                                {
                                    Directory.CreateDirectory(outputDirectorywrd);
                                }

                                // string filename = "Coachbooking_" + hdnitineraryAutono.Text + "_" + DateTime.Now.ToString("ddmmyyyyhhssmm") + ".docx";
                                string filename = System.IO.Path.GetFileNameWithoutExtension(finaloutfilename) + ".docx";
                                string htmfile = Imagepdfhtmlfolderpath+"\\HTMLFileForCoachbooking\\" + finaloutfilename;
                                string htmlcont = System.IO.File.ReadAllText(htmfile);
                                if (!string.IsNullOrEmpty(htmlcont))
                                {
                                    byte[] pdfBytes = ConvertHtmlToDocx(htmlcont);
                                    System.IO.File.WriteAllBytes(Imagepdfhtmlfolderpath+"\\WordDocCoachbooking\\" + filename, pdfBytes);

                                    Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                                    dlg.FileName = filename; // Default file name
                                    dlg.DefaultExt = "docx"; // Default file extension

                                    // Show save file dialog box
                                    Nullable<bool> result = dlg.ShowDialog();

                                    // Process save file dialog box results
                                    if (result == true)
                                    {

                                        dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                                        WebClient webClient = new WebClient();
                                        //  webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                                        //webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                                        webClient.DownloadFileAsync(new Uri(Imagepdfhtmlfolderpath+"\\WordDocCoachbooking\\" + filename),
                                            dlg.FileName);
                                        // Save document
                                        string filename1 = dlg.FileName;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Please provide the booking details");
                        return;
                    }
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }
            catch (Exception ex)
            {
                errobj.WriteErrorLoginfo("ItineraryWindow", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), "");
            }
            finally
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            }

        }

        private void GetCoachbookingformhtml(List<CoachBookingModel> coachbkgdatadb, List<PassengerNameRoomlist> coachbkgPassengerdatadb, List<Attractionlist> coachbkgAttractiondatadb, ref string finaloutputfile)
        {

            int i = 0;
            StringBuilder mainhtmlcontent = new StringBuilder();

            string strheader = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n<meta charset=\"UTF-8\"/>\r\n<meta name=\"application-name\"/>\r\n<meta name=\"author\"/>\r\n<meta name=\"description\"/>\r\n<meta name=\"generator\"/>\r\n<meta name=\"keywords\"/>\r\n<meta name=\"viewport\" content=\"width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no\"/>\r\n<title>CB Form for Coach Booking</title>\r\n</head>\r\n<body>";

            string divbodycontentstart = "<div class=\"body_contents\" style=\" padding: 0px 10px; font-family: 'Arial',sans-serif;\">";

            string divbodycontentend = "</div>";

            string divheadercontent = "<table style=\"width: 100%;\">\r\n    <tbody>\r\n      <tr style=\"display: flex; flex-wrap:nowrap; justify-content: flex-end; padding: 15px 0px;\">\r\n " +
                "<td\r\n style=\"border-right: 2px solid #44A826; padding-right: 10px; margin-right: 10px; width:75%;text-align: right;\"><p class=\"title_name\"\r\n                           " +
                "style=\" font-size: 20px; color: #44A826; margin: 0 0 5px 0; font-weight: bold; line-height: 25px;font-family: Arial, sans-serif;\"> Love Irish Tours</p>\r\n " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Unit 8 Scurlockstown Business Park,</p>\r\n " +
                "         <p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Trim,</p>\r\n          " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Co Meath</p>\r\n" +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Ph: 1888-508-6639</p>\r\n          " +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif;\"> Website: www.loveirishtours.com</p>\r\n" +
                "<p class=\"para top_intro\" style=\"font-size: 14px; color: #665e5e; margin: 0 0 5px 0; font-weight: 400; line-height: 15px; font-weight: 600;font-family: Arial, sans-serif; margin-bottom: 30px;font-family: Arial, sans-serif;\"\"> Niall Carroll</p></td>\r\n" +
                "<td style=\"display: flex;align-items: center;justify-content: center;\"><img src=\"https://loveirishtours.com/wp-content/uploads/2017/11/love-irish-tours-logo-optimized.png\"\r\n                            alt=\"LIV\" /></td>\r\n      </tr>\r\n    </tbody>\r\n  </table>";

            string footer = "</body>\r\n</html>";

            string divclientstart = string.Empty;
            string secondtablestart = string.Empty;
            string secondtableend = string.Empty;
            string enddate = string.Empty;
            string Customerinfostart = string.Empty;
            string customerinfoend = string.Empty;
            string docenddate = string.Empty;
            string passengernamelistcontent = string.Empty;

            string attractionliststart = string.Empty;
            string attractionlistcontent = string.Empty;
            string attractionlistend = string.Empty, passengernameliststart = string.Empty, passengernamelistend = string.Empty, importantnoteabttour = string.Empty;

            StringBuilder dynamicpartcontent = new StringBuilder();         

            CoachBookingModel _marker = new CoachBookingModel();

            foreach (CoachBookingModel Bkit in coachbkgdatadb)
            {                 
                string selected = string.Empty;
                selected = "Final Itinerary for";

                if (i == 0)
                {
                    if (!string.IsNullOrEmpty(Bkit.ItineraryEndDate))
                    {
                        docenddate = Bkit.ItineraryStartDate + " - " + Bkit.ItineraryEndDate;
                    }
                    else
                    {
                        docenddate = Bkit.ItineraryStartDate;
                    }

                    string tourfor = (ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault() != null) ?
                        ListofTour.Where(x => x.Tourlistid == CmbTourlist.SelectedValue.ToString()).FirstOrDefault().Tourlistname.ToString() : string.Empty;

                    Customerinfostart = "<div id=\"Customerinfo\">" +
                        " <h1 style=\"font-size: 16px; color: #44A826; margin-bottom: 15px; text-align:center;font-weight: bold;\">" +
                        "CB FORM FOR " + Bkit.ItineraryName + "</h1>\r\n       " +
                        "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Tour Dates: </span> " + Bkit.ItineraryStartDate + " - " + Bkit.ItineraryEndDate + "</p>\r\n      " +
                        "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Number of People: </span> " + Bkit.PaxNumbers + "</p>\r\n      " +
                        "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Vehicle Type: </span> </p>\r\n      " +
                        "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Driver Start Date: </span></p>\r\n      " +
                        "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; fot-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Transfer/Last Day Date: </span></p><br/>\r\n      " +
                    "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Flight Details: </span></p>\r\n      " +
                    "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Departure flight details: </span></p><br/>\r\n      " +
                    "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Pick Up Location on Date: </span></p>\r\n      " +
                    "<p style=\"font-size: 14px; color:#000; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;color: #44A826;\"> Departure Details: </span></p><br/>\r\n      ";

                    
                    passengernameliststart = "<p style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\">" +
                            "<span style=\"font-weight: 400;\"> Names of People on the Group: </span>";
                    foreach (PassengerNameRoomlist objpas in coachbkgPassengerdatadb)
                    {                        
                        passengernamelistcontent = passengernamelistcontent+ "<p style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 24px;\">"
                            + objpas.PassengerName_Room.ToString() + "</p>";
                    }
                    passengernamelistend = "</p>";
                    attractionliststart = "<p style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;\"> List of Attractions Booked on Itinerary: </span>";
                    foreach (Attractionlist objat in coachbkgAttractiondatadb)
                    {
                        attractionlistcontent= attractionlistcontent+"<p style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 24px;\">&#x2022;"
                            + objat.Attractions + "</p>";
                    }
                    attractionlistend = "</p>";
                    
                         importantnoteabttour = string.Empty;
                    importantnoteabttour= "<p style=\"font-size: 14px; color:#44A826; margin: 5px 0px 5px 0px; font-weight: 600; font-family: Arial, sans-serif; text-align: left; line-height: 30px;\"> " +
                        "<span style=\"font-weight: 400;\"> Important Notes About the Tour: </span></p>";

                    customerinfoend = "</div>\r\n  <br/>\r\n  <div style=\"page-break-before:always\">&nbsp;</div>";

                    secondtablestart = "<div class=\"cilent_update_info\">";

                    secondtableend = "</div><br/>";

                    divclientstart = "<h1 class=\"clientname\"\r\n style=\"font-size: 16px; color: #44A826; margin-bottom: 8px; text-align:left;font-weight: bold;\">" +

                       selected + " " + Bkit.ItineraryName + "</h1>" +

                        "<h2 class=\"arrival_time\"\r\n style=\"font-size: 14px; font-weight: 600; color: black; margin-bottom: 10px;\">" +
                        "Arriving " + Bkit.NameofItineraryStartDate + ", " + Bkit.ItineraryStartDate + ", departing ";

                    if (!string.IsNullOrEmpty(Bkit.ItineraryEndDate))
                    {
                        enddate = Bkit.NameofItineraryEndDate + ", " + Bkit.ItineraryEndDate + "</h2>";
                    }
                    else
                    {
                        enddate = Bkit.NameofItineraryEndDate + "</h2>";
                    }                    

                }                
                #region dynamic part with table                

                string divdynamicstart = "<table class=\"dynamic_activity_flow\" style=\"width: 100%;border-collapse: collapse;\">\r\n    <tr>\r\n\r\n        <td>";

                string divdynamicend = " </td>\r\n    </tr>\r\n\r\n</table>";

                string tablestart = "<table style=\"width: 100%;border-collapse: collapse;\">\r\n                <tbody>";

                string tableend = "</tbody>\r\n            </table>";

                string divdayinfoheader = "<tr>\r\n <td> <p class=\"activity_day_info\" id=\"dayheader\" style=\"margin: 20px 0px 10px 0px;font-weight: 600; color: #44A826;font-size: 16px;display: inline-block;font-family: Arial, sans-serif;\">"
                + "Day " + Bkit.Daycount + " " + Bkit.NameofDate + ", " + Bkit.StartDate;

                string cityval = string.Empty;

                if (!string.IsNullOrEmpty(Bkit.City))
                {
                    cityval = " - " + Bkit.City + "</p></td>\r\n  </tr>";
                }
                else
                {
                    cityval = "</p></td>\r\n  </tr>";
                }

                string divactivity_nameheader = "<tr>\r\n<td><p class=\"activity_name\" id=\"Activityheader\" style=\"font-size: 16px; font-weight: 600; color: black; margin-bottom: 10px;font-family: Arial, sans-serif;line-height: 18px; \">\r\n                           "
                + Bkit.BookingName + "</p></td>\r\n                    </tr>";

                string divactivitydetstart = "<tr>\r\n  <td>\r\n " +
                    " <table style=\"width: 100%;border-collapse: collapse;\" class=\"activity_details\" id=\"activitydetails\">\r\n " +
                    " <tbody>\r\n <tr class=\"content_boxes_pra\">\r\n  ";

                string tdonestart = "<td style=\"width: 70%;vertical-align: top;padding: 15px 15px 15px 0px;\">\r\n\r\n                                            " +
                    "<p class=\"para body_para\"\r\n  style=\"text-align: left;  font-size: 14px; color: #665e5e; margin: 0 0 10px 0; font-weight: 400; line-height: 15px;font-family: Arial, sans-serif;\">"
                 + Bkit.BodyHtml + "</p>";

                string divstaydetailnote1 = string.Empty;
                string divstaydetailnote2 = string.Empty;

                if (Bkit.Type.ToLower() == "accommodation")
                {
                    string roomname = (Bkit.ItemDescription.IndexOf(",") > 0) ? Bkit.ItemDescription.Split(",")[1] : Bkit.ItemDescription;
                    divstaydetailnote1 = "<div class=\"last_note mt-4\">\r\n <p class=\"info_main_note\"\r\n style=\"margin-bottom: 0; font-weight: 600; text-align: left; color: black; font-size: 14px; margin-bottom: 5px;font-family: Arial, sans-serif;\">" +
                    "Stay at " + Bkit.BookingName + "<br/>";
                    divstaydetailnote2 = Bkit.NightsDays + " x " + roomname + "</p>\r\n    </div>";
                }
                else
                {
                    divstaydetailnote1 = string.Empty;
                    divstaydetailnote2 = string.Empty;
                }

                string tdoneend = "</td>";

                string divimagepart = string.Empty;

                if (!string.IsNullOrEmpty(Bkit.ReportImage))
                {
                    divimagepart = "<td class=\"img-boxes\"\r\n  style=\"width: 30%; margin: 0 10px 0 auto; text-align: center; vertical-align:top;\">"
                    + "<img src=\"" + Bkit.ReportImage + "\" class=\"side_acivity_img\" style=\"background: #f9f9f9; height: 50px;width: 75px;object-fit: cover;object-position: center; margin: auto;\"/>\r\n      </td>";
                }
                else
                {
                    divimagepart = "";
                }

                string divactivitydetend = "</tr></tbody>\r\n </table><br/>\r\n  </td>\r\n  </tr>";
                string divactivity_attractionvalue = string.Empty;
                string divactivity_attractionend = string.Empty;
                string divactivity_attractionstart = string.Empty;

                if (coachbkgAttractiondatadb.Count > 0)
                {
                    divactivity_attractionstart = "<tr>\r\n<td><p class=\"activity_name\" id=\"Activityheader\" style=\"font-size: 18px; color: #ff0400; margin: 8px 0px; font-weight: 600;;font-family: Arial, sans-serif;\">ATTRACTIONS: <br/>";

                    foreach (Attractionlist objat in coachbkgAttractiondatadb)
                    {
                        if (objat.Bkid == Bkit.Bkid.ToString())
                        {
                            divactivity_attractionvalue = divactivity_attractionvalue + objat.Attractions + "<br/>";
                        }
                    }
                    divactivity_attractionend = "</p></td>\r\n </tr>";
                }
                
                #endregion "Dynamic part with table"

                string dynamicpart = divdynamicstart + tablestart + divdayinfoheader + cityval + divactivity_nameheader + divactivitydetstart + tdonestart + divstaydetailnote1 + divstaydetailnote2 +
                                        tdoneend + divimagepart + divactivitydetend + divactivity_attractionstart + divactivity_attractionvalue + divactivity_attractionend+ tableend + divdynamicend;

                dynamicpartcontent.AppendLine(dynamicpart);
                Bkit.ItineraryID = Bkit.ItineraryID;
                i++;
            }

            string findl = Customerinfostart + customerinfoend+ passengernameliststart + passengernamelistcontent + passengernamelistend + attractionliststart + attractionlistcontent + attractionlistend +
                        importantnoteabttour + secondtablestart + divclientstart + enddate + secondtableend;

            mainhtmlcontent.AppendLine(strheader);

            mainhtmlcontent.AppendLine(divbodycontentstart);

            mainhtmlcontent.AppendLine(divheadercontent);

            mainhtmlcontent.AppendLine(findl);

            mainhtmlcontent.Append(dynamicpartcontent);

            mainhtmlcontent.AppendLine(divbodycontentend);

            mainhtmlcontent.AppendLine(footer);

            string htmlContent = mainhtmlcontent.ToString();

            string ItineraryName = TxtItineraryName.Text;
            string ItineraryAutoId = hdnitineraryAutono.Text;
            OutputFileName = $"{ItineraryName}_{ItineraryAutoId}_Coachbkg_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".html";
            Selectionoption = "Full Payment Confirmation";

            GenerateHTMLFile(htmlContent, OutputFileName, "htmlcoach");
            finaloutputfile = OutputFileName;
        }

        #endregion "Coach booking form end

        private void btnsavecoachbooking_Click(object sender, RoutedEventArgs e)
        {
            PrintCoachBooking();
        }

        private void TxtClientFirstName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtClientFirstName.Text))
            {
                if (!string.IsNullOrEmpty(TxtClientLastname.Text))
                {
                    TxtClientdisplayname.Text = TxtClientFirstName.Text.Trim() + " " + TxtClientLastname.Text.Trim();
                }
                else
                {
                    TxtClientdisplayname.Text = TxtClientFirstName.Text.Trim();
                }

                CTPassengerVMitn.FirstName = TxtClientFirstName.Text.Trim();
                CTPassengerVMitn.LastName = TxtClientLastname.Text.Trim();
                CTPassengerVMitn.DisplayName = TxtClientdisplayname.Text.Trim();
                CTPassengerVMitn.Email = TxtEmail.Text.Trim();
                if (clientTabViewModel.PassengerDetailsobser.Where(x => x.LeadPassenger == true).ToList().Count == 0)
                {
                    CTPassengerVMitn.LeadPassenger = true;
                }
                clientTabViewModel.Itineraryid = hdnitineraryid.Text.Trim();
                if (CTPassengerVMitn.Passengerid != null)
                {
                    CTPassengerVMitn.Passengerid = CTPassengerVMitn.Passengerid;
                }
                else
                {
                    CTPassengerVMitn.Passengerid = Guid.NewGuid().ToString();
                }

                //this.clientTabViewModel.objClPassvm = CTPassengerVMitn;
                this.clientTabViewModel.CTPassengerCommand.CTPassengerVM = CTPassengerVMitn;
                this.clientTabViewModel.CTPassengerCommand.AddCommandfromcore.Execute();
            }
            if (string.IsNullOrEmpty(TxtClientFirstName.Text))
            {
                MessageBox.Show("Please provide the client first name");                
                return;                
            }
            //if (string.IsNullOrEmpty(TxtClientLastname.Text))
            //{
            //    TxtClientdisplayname.Text = string.Empty;
            //    MessageBox.Show("Please provide the client last name");                
            //    return;
            //}
        }

        private void TxtClientLastname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtClientFirstName.Text))
            {
                if (!string.IsNullOrEmpty(TxtClientLastname.Text))
                {
                    TxtClientdisplayname.Text = TxtClientFirstName.Text.Trim() + " " + TxtClientLastname.Text.Trim();
                }
                else
                {
                    TxtClientdisplayname.Text = TxtClientFirstName.Text.Trim();
                }


                CTPassengerVMitn.FirstName = TxtClientFirstName.Text.Trim();
                CTPassengerVMitn.LastName = TxtClientLastname.Text.Trim();
                CTPassengerVMitn.DisplayName = TxtClientdisplayname.Text.Trim();
                CTPassengerVMitn.Email = TxtEmail.Text.Trim();                
                if(clientTabViewModel.PassengerDetailsobser.Where(x=>x.LeadPassenger==true).ToList().Count==0)
                {
                    CTPassengerVMitn.LeadPassenger = true;
                }
                clientTabViewModel.Itineraryid = hdnitineraryid.Text.Trim();
                if (CTPassengerVMitn.Passengerid != null)
                {
                    CTPassengerVMitn.Passengerid = CTPassengerVMitn.Passengerid;
                }
                else
                {
                    CTPassengerVMitn.Passengerid = Guid.NewGuid().ToString();
                }

                //this.clientTabViewModel.objClPassvm = CTPassengerVMitn;
                this.clientTabViewModel.CTPassengerCommand.CTPassengerVM = CTPassengerVMitn;
                this.clientTabViewModel.CTPassengerCommand.AddCommandfromcore.Execute();
            }
            //if (string.IsNullOrEmpty(TxtClientFirstName.Text))
            //{
            //    MessageBox.Show("Please provide the client first name");                
            //    return;
            //}
            if (string.IsNullOrEmpty(TxtClientLastname.Text))
            {
                TxtClientdisplayname.Text = string.Empty;
                MessageBox.Show("Please provide the client last name");                
                return;
            }

        }
    }

    //public static class FocusFields1

    //{

    //    public static void BeginInvoke1<T>(this T element, Action<T> action, DispatcherPriority priority = DispatcherPriority.ApplicationIdle) where T : UIElement

    //    {

    // element.Dispatcher.BeginInvoke(priority, action);

    //    }

    //}

}



