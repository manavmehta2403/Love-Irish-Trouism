using LIT.Common;
using LIT.Modules.TabControl.ViewModels;
using Microsoft.Identity.Client;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace LIT.Modules.TabControl.Commands
{

    public class Itinerary_ClientTabPaxInformationCommand : IOperations
    {
        public readonly Itinerary_ClientTabViewModel CTviewModel;
        private static Clienttabdal objclntdal;
        public Paxinformation paxinfo;
        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        public List<Currencydetails> ListofCurrency = new List<Currencydetails>();

        public string ItinCurDisplayFormat { get; set; }
        public Itinerary_ClientTabPaxInformationCommand(Itinerary_ClientTabViewModel ctpassvm)
        {
            CTviewModel = ctpassvm;
            paxinfo = CTviewModel.objpaxinformation;
            objclntdal = new Clienttabdal();
            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);

            ListofCurrency = new List<Currencydetails>();
            ListofCurrency = loadDropDownListValues.LoadCurrencyDetails();
            var observablecollectionCurrencydetails = new ObservableCollection<Currencydetails>(ListofCurrency);
            CTviewModel.Currencydetailsval = observablecollectionCurrencydetails;

            
            ItinCurDisplayFormat = this.ListofCurrency.Where(x => x.CurrencyName.ToString().ToLower() == "euro" && x.CurrencyCode.ToString().ToLower() == "eur").FirstOrDefault().DisplayFormat;

        }
        private bool CanExecuteAdd()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Add");
        }

        private void ExecuteAdd()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());
            //CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;

        }

        private bool CanExecuteDelete()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Delete");
        }

        private void ExecuteDelete()
        {
            // Implement your Delete logic here
            // Delete the selected item from the list
            
              //  CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;
        }

        private bool CanExecuteSave()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Save");
        }

        private void ExecuteSave()
        {
            // Implement your Add logic here
            // Add a new item to the list
            if(CTviewModel.PaxNumbers>0)
            SavePaxDetailsCommandExecute();
            // FuviewModel.Items.Add(new FollowupModel());
        }


        private bool CanExecuteRetrieve()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Retrieve");
        }

        private void ExecuteRetrieve()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());
            ReterivePaxDetailsCommandExecute();
           // CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;
        }
        public DelegateCommand SaveCommand
        {
            get; set;
        }

        public DelegateCommand UpdateCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteCommand
        {
            get;
            set;
        }

        public DelegateCommand AddCommand
        {
            get;
            set;
        }
        public DelegateCommand RetrieveCommand
        {
            get;
            set;
        }

        public bool CanExecuteCommand(string commandName)
        {
            //throw new System.NotImplementedException();
            return true;
        }

        private void SavePaxDetailsCommandExecute()
        {

            
            CTviewModel.objpaxinformation.Paxid = (CTviewModel.Paxid!=null)? CTviewModel.Paxid:Guid.NewGuid().ToString();
            CTviewModel.objpaxinformation.PaxNumbers = CTviewModel.PaxNumbers;
            CTviewModel.objpaxinformation.ItineraryID = CTviewModel.Itineraryid;
                if (string.IsNullOrEmpty(CTviewModel.CreatedBy) || (CTviewModel.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                {               
                   CTviewModel.objpaxinformation.CreatedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpaxinformation.CreatedBy = CTviewModel.CreatedBy;
                }

                if (string.IsNullOrEmpty(CTviewModel.ModifiedBy) || (CTviewModel.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    CTviewModel.objpaxinformation.ModifiedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpaxinformation.ModifiedBy = CTviewModel.ModifiedBy; 
                }
                if (string.IsNullOrEmpty(CTviewModel.DeletedBy) || (CTviewModel.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                {                    
                        CTviewModel.objpaxinformation.DeletedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpaxinformation.DeletedBy = CTviewModel.DeletedBy;
                }

                string res = objclntdal.SaveUpdatePaxinformation("I", CTviewModel.objpaxinformation);

        


    }

        private void ReterivePaxDetailsCommandExecute()
        {
            // Itinerary_ClientTabPaxInformation passvmobj = ((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPaxInformation)((System.Delegate)ReterivePassengerDetailsCommandExecute).Target).CTPassengerVM;
            if (CTviewModel.objpaxinformation != null)
            {
                List<Paxinformation> listPaxdet = new List<Paxinformation>();
                listPaxdet = objclntdal.RetrivePaxinformation(Guid.Parse(CTviewModel.Itineraryid));
                if (listPaxdet.Count > 0)
                {
                    foreach (Paxinformation obj in listPaxdet)
                    {
                        Paxinformation objpax=new Paxinformation();
                        objpax.Paxid=obj.Paxid;
                        objpax.PaxNumbers=obj.PaxNumbers;
                        objpax.CreatedBy=obj.CreatedBy;    
                        objpax.DeletedBy=obj.DeletedBy;
                        objpax.ModifiedBy=obj.ModifiedBy;  
                        objpax.ItineraryID=obj.ItineraryID;
                        objpax.Bookingid=obj.Bookingid;
                        CTviewModel.objpaxinformation = objpax;
                        CTviewModel.Paxid = objpax.Paxid;
                        CTviewModel.PaxNumbers = (obj.PaxNumbers!=null)?Convert.ToInt32(obj.PaxNumbers):0;
                        CTviewModel.CreatedBy = obj.CreatedBy;
                        CTviewModel.DeletedBy = obj.DeletedBy;
                        CTviewModel.ModifiedBy = obj.ModifiedBy;
                        CTviewModel.Itineraryid = obj.ItineraryID;
                        CTviewModel.Bookingid = obj.Bookingid;

                        //  listPaxdet.Add(objpax);
                        // ReterivePassdetails(paxinfo.ItineraryID, paxinfo.Loginuserid);
                    }
                }
            }
        }     

    }

    public  class Itinerary_ClientTabPassengerCommand : IOperations
    {
        List<CommonValueList> ListofAgentpass = new List<CommonValueList>();
        List<OptionforRoomtypes> listRoomlist = new List<OptionforRoomtypes>();
        List<PassengerTypeValues> listPassengerType = new List<PassengerTypeValues>();
        List<PaymenttypeValues> listPaymentType = new List<PaymenttypeValues>();
        List<AgeGroupValues> listAgeGroups = new List<AgeGroupValues>();
        List<PassengergroupValues> listPassengergroup = new List<PassengergroupValues>();
        List<CommonValueCountrycity> ListofCountry = new List<CommonValueCountrycity>();
        public readonly Itinerary_ClientTabViewModel CTviewModel;
        public readonly Itinerary_ClientTabPassengerViewModel CTPassengerVM;
        private static Clienttabdal objclntdal;


        public Itinerary_ClienttabPaymentCommand objclpaymentcomd;

        public Itinerary_ClientTabPaxInformationCommand objclpaxcomd;

        public new event EventHandler CanExecuteChangedPassenger;
        public Itinerary_ClientTabPassengerCommand(Itinerary_ClientTabViewModel ctpassvm)
        {
            //FuviewModel = viewModel;
            
            CTPassengerVM = ctpassvm.objClPassvm;
            
            CTviewModel = ctpassvm;
            CTviewModel.objpassdet = new PassengerDetails();
            objclntdal = new Clienttabdal();

            if (CTviewModel.SelectedItemvmct != null)
            {
                CTPassengerVM.SelectedItem = CTviewModel.SelectedItemvmct;
            }

            ListofAgentpass = loadDropDownListValues.LoadCommonValuesWithDefault("Agent");
            var observablecollectionAgent = new ObservableCollection<CommonValueList>(ListofAgentpass);
            CTviewModel.AgenttovalPass = observablecollectionAgent;

            listRoomlist = objclntdal.RetriveRoomtypes();
            var observablecollectionRoom = new ObservableCollection<OptionforRoomtypes>(listRoomlist);
            CTviewModel.Roomtypeval = observablecollectionRoom;


            listPassengerType = objclntdal.RetrivePassengerTypeValues();
            var observablecollectionpasstype = new ObservableCollection<PassengerTypeValues>(listPassengerType);
            CTviewModel.Passengertypeval = observablecollectionpasstype;


            listPaymentType = objclntdal.RetrivePaymentTypeValues();
            var observablecollectionpaymenttype = new ObservableCollection<PaymenttypeValues>(listPaymentType);
            CTviewModel.Payementtype = observablecollectionpaymenttype;

            listAgeGroups = objclntdal.RetriveAgegroupValues();
            var observablecollectionAgegroup = new ObservableCollection<AgeGroupValues>(listAgeGroups);
            CTviewModel.AgeGroupval = observablecollectionAgegroup;

            listPassengergroup = objclntdal.RetrivePassengergroupValues();
            var observablecollectionpassegrp = new ObservableCollection<PassengergroupValues>(listPassengergroup);
            CTviewModel.Passengergroupval = observablecollectionpassegrp;

            CommonValueCountrycity objCVCC
                   = new CommonValueCountrycity();
            ListofCountry = loadDropDownListValues.LoadCommonValuesCountry("Country", objCVCC);
            if (ListofCountry != null && ListofCountry.Count > 0)
            {
                var observablecollectionpasscountry = new ObservableCollection<CommonValueCountrycity>(ListofCountry);
                CTviewModel.PassengerCountrylist = observablecollectionpasscountry;
            }

            //this.AddPassengerDetailsCommand = new DelegateCommand(AddPassengerDetailsCommandExecute);
            //this.ReterivePassengerDetailsCommand = new DelegateCommand(ReterivePassengerDetailsCommandExecute);
            //this.SavePassengerDetailsCommand = new DelegateCommand(SavePassengerDetailsCommandExecute);
            //this.DeletePassengerDetailsCommand = new DelegateCommand(DeletePassengerDetailsCommandExecute);
            CTviewModel.PropertyChanged += ViewModel_PropertyChangedPassenger;
            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);
            LostFocusCommand = new DelegateCommand(ExecuteFocus, CanExecuteFocus);
            objclpaymentcomd = new Itinerary_ClienttabPaymentCommand(ctpassvm);
            RoomlistSelectionChangedCommand = new DelegateCommand(ExecuteRoomlistchange, CanExecuteRoomlistchange);

            objclpaxcomd = new Itinerary_ClientTabPaxInformationCommand(ctpassvm);
            CTviewModel.objpassdet.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;
            CTviewModel.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;

        }

        private void ViewModel_PropertyChangedPassenger(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChangedPassenger?.Invoke(this, new EventArgs());
        }

        private bool CanExecuteRoomlistchange()
        {
            return true;
        }

        private void ExecuteRoomlistchange()
        {
            if (((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ExecuteRoomlistchange).Target).CTviewModel.SelectedItemvmct != null)
            {
                if (((SQLDataAccessLayer.Models.OptionforRoomtypes)((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ExecuteRoomlistchange).Target).CTviewModel.SelectedItemvmct.RommtypeselectedItem) != null)
                {
                    string roomdivisor = ((SQLDataAccessLayer.Models.OptionforRoomtypes)((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ExecuteRoomlistchange).Target).CTviewModel.SelectedItemvmct.RommtypeselectedItem).Divisor;
                    this.CTviewModel.SelectedItemvmct.Room = roomdivisor;
                    this.CTviewModel.objpassdet.Room = roomdivisor;
                }
            }
        }
        private bool CanExecuteAdd()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Add");
        }

        private void ExecuteAdd()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            AddPassengerDetailsCommandExecute();
            CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;

        }

        private bool CanExecuteDelete()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Delete");
        }

        private void ExecuteDelete()
        {
            // Implement your Delete logic here
            // Delete the selected item from the list

            if (CTviewModel.SelectedItemvmct != null)
            {
                CTPassengerVM.SelectedItem = CTviewModel.SelectedItemvmct;
            }
            if (CTPassengerVM.SelectedItem != null)
            {
                DeletePassengerDetailsCommandExecute();
                
                CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;
                // FuviewModel.Items.Remove(FuviewModel.SelectedItem);
            }
        }

        private bool CanExecuteSave()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Save");
        }

        private void ExecuteSave()
        {
            // Implement your Add logic here
            // Add a new item to the list
            SavePassengerDetailsCommandExecute();
            // FuviewModel.Items.Add(new FollowupModel());
        }


        private bool CanExecuteRetrieve()
        {
            return CTviewModel.IntrCTPassengerViewModel.CanExecuteCommand("Retrieve");
        }

        private void ExecuteRetrieve()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            ReterivePassengerDetailsCommandExecute();
            CTviewModel.Totalpassenger = CTviewModel.PassengerDetailsobser.Count;
        }

        private bool CanExecuteFocus()
        {
            // return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Add");
            return true;
        }

        private void ExecuteFocus()
        {

            string passid = ((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ExecuteFocus).Target).CTviewModel.SelectedItemvmct.Passengerid;
            string passname = ((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ExecuteFocus).Target).CTviewModel.SelectedItemvmct.DisplayName;
            // CTviewModel.objClPassvm. = CTviewModel.PaymentRefundObser.Sum(x => x.Amount);

            //PassengerNamelist objpasnl = new PassengerNamelist();
            //objpasnl.PassengerDisplayName = objpassvm.DisplayName;
            //objpasnl.Passengerid = objpassvm.Passengerid;
            this.CTviewModel.Passengernamelist.Where(x => x.Passengerid == passid).FirstOrDefault().PassengerDisplayName = passname;

            ObservableCollection<PassengerNamelist> objPassengerlist = new ObservableCollection<PassengerNamelist>(CTviewModel.Passengernamelist.OrderBy(x => x.PassengerDisplayName).ToList());

            this.CTviewModel.Passengernamelist = objPassengerlist;
        }

      

        #region "Private method"
        private void AddPassengerDetailsCommandExecute()
        {
            AddPassengerDetails();
            
        }
        private void ReterivePassengerDetailsCommandExecute()
        {
            //Itinerary_ClientTabPassengerViewModel passvmobj = ((LIT.Modules.TabControl.Commands.Itinerary_ClientTabPassengerCommand)((System.Delegate)ReterivePassengerDetailsCommandExecute).Target).CTPassengerVM;
            //if (passvmobj != null)
            //{
            //    //ReterivePassdetails(passvmobj.Itineraryid, passvmobj.Loginuserid);
              
            //}
            if (CTviewModel != null)
                ReterivePassdetails(CTviewModel.Itineraryid, CTviewModel.Loginuserid);
        }
        private void SavePassengerDetailsCommandExecute()
        {

            foreach (PassengerDetails objCTPassvm in CTviewModel.PassengerDetailsobser)
            {
                CTviewModel.objpassdet.Passengerid = objCTPassvm.Passengerid;
                CTviewModel.objpassdet.Age = objCTPassvm.Age;
                CTviewModel.objpassdet.AgeGroup = (objCTPassvm.AgegroupselectedItem!=null)? ((AgeGroupValues)objCTPassvm.AgegroupselectedItem).AgeGroupsid: Guid.Empty.ToString();
                CTviewModel.objpassdet.Agent = (objCTPassvm.AgentselectedItem != null) ? ((CommonValueList)objCTPassvm.AgentselectedItem).ValueField.ToString() : Guid.Empty.ToString();// objCTPassvm.Agent;
                CTviewModel.objpassdet.AgentNet = objCTPassvm.AgentNet;
                CTviewModel.objpassdet.CommissionOverride = objCTPassvm.CommissionOverride;
                CTviewModel.objpassdet.CommissionPercentage = objCTPassvm.CommissionPercentage;
                CTviewModel.objpassdet.Comments = objCTPassvm.Comments;
                CTviewModel.objpassdet.CompanyName = objCTPassvm.CompanyName;
                CTviewModel.objpassdet.Country = (objCTPassvm.CountrySelectedItem != null) ? ((CommonValueCountrycity)objCTPassvm.CountrySelectedItem).CountryId.ToString() : Guid.Empty.ToString();
                CTviewModel.objpassdet.DefaultPrice = objCTPassvm.DefaultPrice;
                CTviewModel.objpassdet.DisplayName = objCTPassvm.DisplayName;
                CTviewModel.objpassdet.Email = objCTPassvm.Email;
                CTviewModel.objpassdet.FirstName = objCTPassvm.FirstName;
                CTviewModel.objpassdet.LastName = objCTPassvm.LastName;
                CTviewModel.objpassdet.PassengerStatus = (objCTPassvm.PassengerStatus != null) ? (objCTPassvm.PassengerStatus): Guid.Empty.ToString();
                CTviewModel.objpassdet.PassengerType = (objCTPassvm.PassengerTypeselectedItem != null) ? ((PassengerTypeValues)objCTPassvm.PassengerTypeselectedItem).PassengerTypeid : Guid.Empty.ToString();


                CTviewModel.objpassdet.Payee = (objCTPassvm.Payee != null) ? objCTPassvm.Payee : Guid.Empty.ToString();
                //(objCTPassvm.PayeeSelectedItem!=null)?((SQLDataAccessLayer.Models.PassengerNamelist)objCTPassvm.PayeeSelectedItem).Passengerid : Guid.Empty.ToString();
                CTviewModel.objpassdet.PayingPax = objCTPassvm.PayingPax;
                CTviewModel.objpassdet.PriceOverride = objCTPassvm.PriceOverride;

                CTviewModel.objpassdet.Room = objCTPassvm.Room;
                CTviewModel.objpassdet.Roomtype = (objCTPassvm.RommtypeselectedItem != null) ? ((OptionforRoomtypes)objCTPassvm.RommtypeselectedItem).OptionTypeRoomid : Guid.Empty.ToString(); 
                CTviewModel.objpassdet.Saledate = objCTPassvm.Saledate;
                CTviewModel.objpassdet.Title = objCTPassvm.Title;

                CTviewModel.objpassdet.ItineraryID = CTviewModel.Itineraryid;
                if (string.IsNullOrEmpty(CTviewModel.CreatedBy) || (CTviewModel.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                {                    
                        CTviewModel.objpassdet.CreatedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpassdet.CreatedBy = CTviewModel.CreatedBy;
                }

                if (string.IsNullOrEmpty(CTviewModel.ModifiedBy) || (CTviewModel.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                {
                        CTviewModel.objpassdet.ModifiedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpassdet.ModifiedBy = CTviewModel.ModifiedBy;
                }
                if (string.IsNullOrEmpty(CTviewModel.DeletedBy) || (CTviewModel.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    CTviewModel.objpassdet.DeletedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpassdet.DeletedBy = CTviewModel.DeletedBy;
                }
                CTviewModel.objpassdet.Totalpassenger = CTviewModel.Totalpassenger;
                string res = objclntdal.SaveUpdatePassengerDetails("I", CTviewModel.objpassdet);
            }
        }
        private void DeletePassengerDetailsCommandExecute() 
        {
            PassengerDetails fnvmobj = CTPassengerVM.SelectedItem;
            if (CTviewModel.PaymentRefundObser.Where(x => x.Passengerid == fnvmobj.Passengerid).Count() > 0)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this Passenger? \n\n Please Note: the corresponding payment record for the passenger will also be deleted.", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    DeletePassengersDetails(fnvmobj, CTviewModel.Loginuserid);

                    if (CTviewModel.PaymentRefundObser.Where(x => x.Passengerid == fnvmobj.Passengerid).Count() > 0)
                    {
                        PaymentDetails payvmobj = CTviewModel.PaymentRefundObser.Where(x => x.Passengerid == fnvmobj.Passengerid).FirstOrDefault();
                        objclpaymentcomd.DeletePaymentrefundDetails(payvmobj, CTviewModel.Loginuserid);
                    }
                }
            }
            else
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this Passenger?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    DeletePassengersDetails(fnvmobj, CTviewModel.Loginuserid);
                }

            }
        }
        private void DeletePassengersDetails(PassengerDetails objPassdet, string Loginuserid)
        {
            CTPassengerVM.Passengerid = (objPassdet.Passengerid != null) ? objPassdet.Passengerid : Guid.Empty.ToString();
            CTviewModel.DeletedBy = (objPassdet.DeletedBy!=null) ? objPassdet.DeletedBy : Guid.Empty.ToString();


            string res = objclntdal.DeletePassengerDetails(CTPassengerVM.Passengerid, CTviewModel.DeletedBy);



            if (!string.IsNullOrEmpty(res))
            {
                if (res.ToString().ToLower() == "1")
                {
                    MessageBox.Show("Passenger details Deleted successfully");
                    CTviewModel.PassengerDetailsobser.Remove(CTviewModel.PassengerDetailsobser.Where(m => m.Passengerid == objPassdet.Passengerid).FirstOrDefault());
                    this.CTviewModel.Passengernamelist.Remove(CTviewModel.Passengernamelist.Where(x => x.Passengerid == objPassdet.Passengerid).FirstOrDefault());

                }
                else if (res.ToString().ToLower() == "-1")
                {
                    if (CTviewModel.PassengerDetailsobser.Where(m => m.Passengerid == objPassdet.Passengerid).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Passenger details Deleted successfully"); 
                        CTviewModel.PassengerDetailsobser.Remove(CTviewModel.PassengerDetailsobser.Where(m => m.Passengerid == objPassdet.Passengerid).FirstOrDefault());
                        this.CTviewModel.Passengernamelist.Remove(CTviewModel.Passengernamelist.Where(x => x.Passengerid == objPassdet.Passengerid).FirstOrDefault());

                    }
                    if (objPassdet.ItineraryID != null)
                        ReterivePassdetails(objPassdet.ItineraryID, Loginuserid);
                }
            }
        }


        private void ReterivePassdetails(string ItineraryID, string Loginuserid)
        {
            List<PassengerDetails> listPassdet = new List<PassengerDetails>();
            // Folluptask = new ObservableCollection<FollowupViewModel>();
            listPassdet = objclntdal.RetrivePassengerDetails(Guid.Parse(ItineraryID));
            //  var observablecollectionfoltask = new CustomObservable.CustomObservableCollection<FollowupModel>();
            if (listPassdet.Count > 0)
            {
                foreach (PassengerDetails obj in listPassdet)
                {
                    PassengerDetails objpassvm = new PassengerDetails();
                    objpassvm.Passengerid = obj.Passengerid;
                    objpassvm.Age = obj.Age;
                    objpassvm.AgeGroup = obj.AgeGroup;
                    objpassvm.Agent= obj.Agent;
                    objpassvm.AgentNet = obj.AgentNet;
                    objpassvm.CommissionOverride = obj.CommissionOverride;

                    objpassvm.CommissionPercentage = obj.CommissionPercentage;
                    objpassvm.Comments = obj.Comments;
                    objpassvm.CompanyName = obj.CompanyName;
                    objpassvm.Country = obj.Country;
                    objpassvm.DefaultPrice = obj.DefaultPrice;

                    objpassvm.DisplayName = obj.DisplayName;
                    objpassvm.Email = obj.Email;
                    objpassvm.FirstName = obj.FirstName;
                    objpassvm.LastName = obj.LastName;
                    objpassvm.PassengerStatus = obj.PassengerStatus;

                    objpassvm.PassengerType = obj.PassengerType;
                    objpassvm.Payee = obj.Payee;
                    objpassvm.PayingPax = obj.PayingPax;
                    objpassvm.PriceOverride = obj.PriceOverride;
                    

                    objpassvm.Room = obj.Room;
                    objpassvm.Roomtype = obj.Roomtype;
                    objpassvm.Saledate = obj.Saledate;
                    objpassvm.Title = obj.Title;
                    objpassvm.GroupName = obj.GroupName;
                    objpassvm.ItineraryID = obj.ItineraryID;

                    if (!string.IsNullOrEmpty(obj.AgeGroup))
                    {
                        objpassvm.AgegroupselectedItem = listAgeGroups.Where(x => x.AgeGroupsid == obj.AgeGroup).FirstOrDefault();
                    }
                    else { objpassvm.AgegroupselectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.Agent))
                    {
                        objpassvm.AgentselectedItem = ListofAgentpass.Where(x => x.ValueField == Guid.Parse(obj.Agent)).FirstOrDefault();
                    }
                    else { objpassvm.AgentselectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.PassengerType))
                    {
                        objpassvm.PassengerTypeselectedItem = listPassengerType.Where(x => x.PassengerTypeid == obj.PassengerType).FirstOrDefault();
                    }
                    else { objpassvm.PassengerTypeselectedItem = null; }
                    if (!string.IsNullOrEmpty(obj.Roomtype))
                    {
                        objpassvm.RommtypeselectedItem = listRoomlist.Where(x => x.OptionTypeRoomid == obj.Roomtype).FirstOrDefault();
                    }
                    else { objpassvm.RommtypeselectedItem = null; }                                     

                    if (!string.IsNullOrEmpty(obj.GroupName))
                    {
                        objpassvm.PassengerGroupNameselectedItem = listPassengergroup.Where(x => x.Passengergroupid == obj.GroupName).FirstOrDefault();
                    }
                    else { objpassvm.PassengerGroupNameselectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.PassengerStatus))
                    {
                        objpassvm.PassengerStatusselectedItem = null;
                        // objpassvm.PassengerStatusselectedItem = listPassengergroup.Where(x => x.Passengergroupid == obj.PassengerStatus).FirstOrDefault();
                    }
                    else { objpassvm.PassengerStatusselectedItem = null; }

                    
                    

                    if (string.IsNullOrEmpty(obj.CreatedBy) || (obj.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpassvm.CreatedBy = Loginuserid;
                    }
                    else
                    {
                        objpassvm.CreatedBy = obj.CreatedBy;
                    }
                    if (string.IsNullOrEmpty(obj.ModifiedBy) || (obj.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpassvm.ModifiedBy = Loginuserid;
                    }
                    else
                    {
                        objpassvm.ModifiedBy = obj.ModifiedBy;
                    }
                    if (string.IsNullOrEmpty(obj.DeletedBy) || (obj.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpassvm.DeletedBy = Loginuserid;
                    }
                    else
                    {
                        objpassvm.DeletedBy = obj.DeletedBy;
                    }


                    
                    objpassvm.IsDeleted = obj.IsDeleted;

                    CTviewModel.Totalpassenger = obj.Totalpassenger;

                    if (CTviewModel.PassengerDetailsobser.Where(x => x.Passengerid == obj.Passengerid).Count() == 0)
                    {
                        

                        PassengerNamelist objpasnl=new PassengerNamelist();
                        objpasnl.PassengerDisplayName = objpassvm.DisplayName;
                        objpasnl.Passengerid = objpassvm.Passengerid;
                        this.CTviewModel.Passengernamelist.Add(objpasnl);

                        if (!string.IsNullOrEmpty(obj.Payee))
                        {
                            objpassvm.PayeeSelectedItem = null;
                            // objpassvm.PayeeSelectedItem = CTviewModel.Passengernamelist.Where(x => x.Passengerid == obj.Payee).FirstOrDefault();
                        }
                        else { objpassvm.PayeeSelectedItem = null; }

                        CTviewModel.PassengerDetailsobser.Add(objpassvm);
                        var observablecollectionft = new ObservableCollection<PassengerDetails>(CTviewModel.PassengerDetailsobser.ToList());
                        CTviewModel.PassengerDetailsobser = observablecollectionft;
                    }
                    else
                    {
                        var observablecollectionft = new ObservableCollection<PassengerDetails>(CTviewModel.PassengerDetailsobser.ToList());
                        CTviewModel.PassengerDetailsobser = observablecollectionft;
                    }
                }



            }
        }
        private void AddPassengerDetails()
        {
            PassengerDetails objitpass = new PassengerDetails();

            if(CTPassengerVM.Passengerid!=null)
            {
                objitpass.Passengerid = CTPassengerVM.Passengerid;
            }
            else
            {
                objitpass.Passengerid = Guid.NewGuid().ToString();
            }
            
            objitpass.Age = CTPassengerVM.Age;
            objitpass.AgeGroup = CTPassengerVM.AgeGroup;
            objitpass.Agent = CTPassengerVM.Agent;
            objitpass.AgentNet = CTPassengerVM.AgentNet;
            objitpass.CommissionOverride = CTPassengerVM.CommissionOverride;
            objitpass.CommissionPercentage = CTPassengerVM.CommissionPercentage;
            objitpass.Comments = CTPassengerVM.Comments;
            objitpass.CompanyName = CTPassengerVM.CompanyName;
            objitpass.Country = CTPassengerVM.Country;
            objitpass.DefaultPrice = CTPassengerVM.DefaultPrice;
            objitpass.DisplayName = CTPassengerVM.DisplayName;
            objitpass.Email = CTPassengerVM.Email;
            objitpass.FirstName = CTPassengerVM.FirstName;
            objitpass.LastName = CTPassengerVM.LastName;
            objitpass.PassengerStatus = CTPassengerVM.PassengerStatus;
            objitpass.PassengerType = CTPassengerVM.PassengerType;
            objitpass.Payee = CTPassengerVM.Payee;
            objitpass.PayingPax = CTPassengerVM.PayingPax;
            objitpass.PriceOverride = CTPassengerVM.PriceOverride;
            objitpass.Room = CTPassengerVM.Room;
            objitpass.Roomtype = CTPassengerVM.Rommtype;
            objitpass.Saledate = CTPassengerVM.Saledate;
            objitpass.Title = CTPassengerVM.Title;

            this.CTviewModel.PassengerDetailsobser.Add(objitpass);

            PassengerNamelist objpasnl=new PassengerNamelist();
            objpasnl.PassengerDisplayName = objitpass.DisplayName;
            objpasnl.Passengerid = objitpass.Passengerid;
            if(CTviewModel.Passengernamelist.Where(x=>x.Passengerid== objitpass.Passengerid).Count()==0 )
            {
                this.CTviewModel.Passengernamelist.Add(objpasnl);
            }    
            
           

        }
        #endregion "Private method"

        

        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        public DelegateCommand SaveCommand
        {
            get; set;
        }

        public DelegateCommand UpdateCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteCommand
        {
            get;
            set;
        }

        public DelegateCommand AddCommand
        {
            get;
            set;
        }
        public DelegateCommand RetrieveCommand
        {
            get;
            set;
        }

        public bool CanExecuteCommand(string commandName)
        {
            //throw new System.NotImplementedException();
            return true;
        }

        private DelegateCommand _lostFocusCommand;

        public DelegateCommand LostFocusCommand
        {
            get { return _lostFocusCommand; }
            set { _lostFocusCommand = value; }
        }
        private DelegateCommand _RoomlistSelectionChangedCommand;

        public DelegateCommand RoomlistSelectionChangedCommand
        {
            get { return _RoomlistSelectionChangedCommand; }
            set { _RoomlistSelectionChangedCommand = value; }
        }
        
    }

    public class  Itinerary_ClienttabPaymentCommand:IOperations
    {
        List<PaymenttypeValues> listPaymenttype = new List<PaymenttypeValues>();
        
        public readonly Itinerary_ClientTabViewModel CTviewModel;
        public readonly Itinerary_ClientTabPaymentViewmodel CTPayVM;
        private static Clienttabdal objclntdal;

        public Itinerary_ClientTabPaxInformationCommand objclpaxcomd;

        public Itinerary_ClienttabPaymentCommand(Itinerary_ClientTabViewModel ctpassvm)
        {
            //FuviewModel = viewModel;
            CTPayVM = ctpassvm.objClPayment;
            CTviewModel = ctpassvm;
            CTviewModel.objpayment = new PaymentDetails();
            objclntdal = new Clienttabdal();

            listPaymenttype = objclntdal.RetrivePaymentTypeValues();
            var observablecollectionpay = new ObservableCollection<PaymenttypeValues>(listPaymenttype);
            CTviewModel.Payementtype = observablecollectionpay;


            if (CTviewModel.SelectedItempaymentvmct != null)
            {
                CTPayVM.SelectedItempayment = CTviewModel.SelectedItempaymentvmct;
            }


            ObservableCollection<PassengerNamelist> objPassengerlist = new ObservableCollection<PassengerNamelist>(CTviewModel.Passengernamelist.OrderBy(x => x.PassengerDisplayName).ToList());

            this.CTviewModel.Passengernamelist = objPassengerlist;

            CTviewModel.PropertyChanged += ViewModel_PropertyChangedPayment;

            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);
            LostFocusCommand = new DelegateCommand(Executecal, CanExecutecal);
            PassengerListSelectionChangedCommand = new DelegateCommand(ExecutePasschange, CanExecutePasschange);


            objclpaxcomd = new Itinerary_ClientTabPaxInformationCommand(ctpassvm);
            CTviewModel.objpassdet.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;
            CTviewModel.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;
        }

        public new event EventHandler CanExecuteChangedPayment;        
        private void ViewModel_PropertyChangedPayment(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChangedPayment?.Invoke(this, new EventArgs());
        }
        private bool CanExecutecal()
        {
            // return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Add");
            return true;
        }

        private void Executecal()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            CTviewModel.RefundPaymentTotalAmount = CTviewModel.PaymentRefundObser.Sum(x => x.Amount);
        }
        private bool CanExecuteAdd()
        {
            return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Add");
        }

        private void ExecuteAdd()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            AddPaymentCommandExecute();
            CTviewModel.RefundPaymentTotalAmount = CTviewModel.PaymentRefundObser.Sum(x => x.Amount);
        }

        private bool CanExecuteDelete()
        {
            return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Delete");
        }

        private void ExecuteDelete()
        {
            // Implement your Delete logic here
            // Delete the selected item from the list
            if (CTviewModel.SelectedItempaymentvmct != null)
            {
                CTPayVM.SelectedItempayment = CTviewModel.SelectedItempaymentvmct;
            }
            if (CTPayVM.SelectedItempayment != null)
            {
                DeletePaymentCommandExecute();
                CTviewModel.RefundPaymentTotalAmount = CTviewModel.PaymentRefundObser.Sum(x => x.Amount);
                // FuviewModel.Items.Remove(FuviewModel.SelectedItem);
            }
        }

        private bool CanExecuteSave()
        {
            return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Save");
        }

        private void ExecuteSave()
        {
            // Implement your Add logic here
            // Add a new item to the list
            SavePaymentCommandExecute();
            // FuviewModel.Items.Add(new FollowupModel());
        }


        private bool CanExecuteRetrieve()
        {
            return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Retrieve");
        }

        private void ExecuteRetrieve()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            ReterivePaymentCommandExecute();
            CTviewModel.RefundPaymentTotalAmount = CTviewModel.PaymentRefundObser.Sum(x=>x.Amount);
        }        
        private bool CanExecutePasschange()
        {
            // return CTviewModel.IntrCTPaymentViewModel.CanExecuteCommand("Add");
            return true;
        }

        private void ExecutePasschange()
        {

            if (((SQLDataAccessLayer.Models.PassengerNamelist)((LIT.Modules.TabControl.Commands.Itinerary_ClienttabPaymentCommand)((System.Delegate)ExecutePasschange).Target).CTviewModel.SelectedItempaymentvmct.PersonnameSelectedItem) != null)
            {
                string passid = ((SQLDataAccessLayer.Models.PassengerNamelist)((LIT.Modules.TabControl.Commands.Itinerary_ClienttabPaymentCommand)((System.Delegate)ExecutePasschange).Target).CTviewModel.SelectedItempaymentvmct.PersonnameSelectedItem).Passengerid;
                this.CTviewModel.SelectedItempaymentvmct.Passengerid = passid;
            }
            // CTviewModel.objClPassvm. = CTviewModel.PaymentRefundObser.Sum(x => x.Amount);

            //PassengerNamelist objpasnl = new PassengerNamelist();
            //objpasnl.PassengerDisplayName = objpassvm.DisplayName;
            //objpasnl.Passengerid = objpassvm.Passengerid;
            //this.CTviewModel.Passengernamelist.Where(x => x.Passengerid == passid).FirstOrDefault().PassengerDisplayName = passname;
        }

        #region "Private method"
        private void AddPaymentCommandExecute()
        {
            AddPayment();
        }
        private void ReterivePaymentCommandExecute() {
            //Itinerary_ClientTabPaymentViewmodel paymentvmobj = ((LIT.Modules.TabControl.Commands.Itinerary_ClienttabPaymentCommand)((System.Delegate)ReterivePaymentCommandExecute).Target).CTPayVM;
            //if (paymentvmobj != null)
            //{
            //    Reterivepaymentdetails(paymentvmobj.Itineraryidvm, paymentvmobj.Loginuseridvm);
            //}
            if (CTviewModel != null)
                Reterivepaymentdetails(CTviewModel.Itineraryid, CTviewModel.Loginuserid);
        }

        private void Reterivepaymentdetails(string ItineraryID, string LoginUserid)
        {
            List<PaymentDetails> listPaymentdet = new List<PaymentDetails>();
            // Folluptask = new ObservableCollection<FollowupViewModel>();
            listPaymentdet = objclntdal.RetrivePaymentDetails(Guid.Parse(ItineraryID));
            //  var observablecollectionfoltask = new CustomObservable.CustomObservableCollection<FollowupModel>();
            if (listPaymentdet.Count > 0)
            {
                foreach (PaymentDetails obj in listPaymentdet)
                {
                    PaymentDetails objpaydet = new PaymentDetails();
                    objpaydet.PaymentID = obj.PaymentID;
                    objpaydet.Amount = obj.Amount;
                    objpaydet.CurrencyCode = obj.CurrencyCode;
                    objpaydet.DateofPayment = obj.DateofPayment;
                    objpaydet.Details = obj.Details;
                    objpaydet.ExchangeRate = obj.ExchangeRate;
                    objpaydet.Fee = obj.Fee;
                    objpaydet.FeePercent = obj.FeePercent;
                    objpaydet.FeeType = obj.FeeType;
                    objpaydet.Inclusive = obj.Inclusive;
                    objpaydet.PaymentAmount = obj.PaymentAmount;
                    objpaydet.PaymentTypeID = obj.PaymentTypeID;
                    objpaydet.Personname = obj.Personname;
                    objpaydet.Sale = obj.Sale;
                    objpaydet.ItineraryID = obj.ItineraryID;                    

                    if (!string.IsNullOrEmpty(obj.PaymentTypeID))
                    {
                        objpaydet.PaymentTypeSelectedItem = listPaymenttype.Where(x => x.Paymenttypesid == obj.PaymentTypeID).FirstOrDefault();
                    }
                    else { objpaydet.PaymentTypeSelectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.CurrencyCode))
                    {
                        objpaydet.CurrencyCodeSelectedItem = objclpaxcomd.ListofCurrency.Where(x => x.CurrencyID == obj.CurrencyCode).FirstOrDefault();
                    }
                    else { objpaydet.CurrencyCodeSelectedItem = null; }


                    //if (!obj.Sale)
                    //{
                    //    objpaydet.SaleselectedItem = null;
                    //    // objpassvm.PassengerStatusselectedItem = listPassengergroup.Where(x => x.Passengergroupid == obj.PassengerStatus).FirstOrDefault();
                    //}
                    //else { objpaydet.SaleselectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.FeeType))
                    {
                        objpaydet.FeeTypeSelectedItem = null;
                        // objpassvm.PassengerStatusselectedItem = listPassengergroup.Where(x => x.Passengergroupid == obj.PassengerStatus).FirstOrDefault();
                    }
                    else { objpaydet.FeeTypeSelectedItem = null; }

                    if (!string.IsNullOrEmpty(obj.Personname))
                    {
                        objpaydet.PersonnameSelectedItem = CTviewModel.Passengernamelist.Where(x => x.Passengerid == objpaydet.Passengerid).FirstOrDefault();
                    }
                    else { objpaydet.PersonnameSelectedItem = null; }


                    if (string.IsNullOrEmpty(obj.CreatedBy) || (obj.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpaydet.CreatedBy = LoginUserid;
                    }
                    else
                    {
                        objpaydet.CreatedBy = obj.CreatedBy;
                    }
                    if (string.IsNullOrEmpty(obj.ModifiedBy) || (obj.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpaydet.ModifiedBy = LoginUserid;
                    }
                    else
                    {
                        objpaydet.ModifiedBy = obj.ModifiedBy;
                    }
                    if (string.IsNullOrEmpty(obj.DeletedBy) || (obj.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objpaydet.DeletedBy = LoginUserid;
                    }
                    else
                    {
                        objpaydet.DeletedBy = obj.DeletedBy;
                    }



                    objpaydet.IsDeleted = obj.IsDeleted;

                    CTviewModel.RefundPaymentTotalAmount = obj.RefundPaymentTotalAmount;

                    if (CTviewModel.PaymentRefundObser.Where(x => x.PaymentID == obj.PaymentID).Count() == 0)
                    {
                        CTviewModel.PaymentRefundObser.Add(objpaydet);
                    }
                    else
                    {
                    }
                }
                var observablecollectionft = new ObservableCollection<PaymentDetails>(CTviewModel.PaymentRefundObser.ToList());
                CTviewModel.PaymentRefundObser = null;
                CTviewModel.PaymentRefundObser = observablecollectionft;
            }

        }
        private void SavePaymentCommandExecute()
        {

            foreach (PaymentDetails objCTPayvm in CTviewModel.PaymentRefundObser)
            {
                CTviewModel.objpayment.Amount = objCTPayvm.Amount;
                CTviewModel.objpayment.CurrencyCode = (objCTPayvm.CurrencyCodeSelectedItem!=null) ? ((Currencydetails)(objCTPayvm.CurrencyCodeSelectedItem)).CurrencyID.ToString() : Guid.Empty.ToString();    
                CTviewModel.objpayment.PaymentID = (objCTPayvm.PaymentID!=null)? objCTPayvm.PaymentID.ToString():Guid.NewGuid().ToString();
                CTviewModel.objpayment.FeePercent = objCTPayvm.FeePercent;
                CTviewModel.objpayment.Fee = objCTPayvm.Fee;
                CTviewModel.objpayment.DateofPayment = objCTPayvm.DateofPayment;
                CTviewModel.objpayment.Details = objCTPayvm.Details;
                CTviewModel.objpayment.ExchangeRate = objCTPayvm.ExchangeRate;
                CTviewModel.objpayment.FeeType = (objCTPayvm.FeeTypeSelectedItem != null) ? Guid.Empty.ToString() : Guid.Empty.ToString();    //objCTPayvm.FeeType;
                CTviewModel.objpayment.Inclusive = objCTPayvm.Inclusive;
                CTviewModel.objpayment.ItineraryID = objCTPayvm.ItineraryID;
                CTviewModel.objpayment.PaymentAmount = objCTPayvm.PaymentAmount;
                CTviewModel.objpayment.PaymentTypeID = (objCTPayvm.PaymentTypeSelectedItem != null) ? ((PaymenttypeValues)(objCTPayvm.PaymentTypeSelectedItem)).Paymenttypesid.ToString() : Guid.Empty.ToString();    //objCTPayvm.PaymentTypeID;
                CTviewModel.objpayment.Personname = (objCTPayvm.Personname != null) ? objCTPayvm.Personname.ToString() : string.Empty; 
                CTviewModel.objpayment.Sale = objCTPayvm.Sale;
                CTviewModel.objpayment.ItineraryID = objCTPayvm.ItineraryID;
                if (string.IsNullOrEmpty(CTviewModel.CreatedBy) || (CTviewModel.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                {
                  CTviewModel.objpayment.CreatedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpayment.CreatedBy = CTviewModel.CreatedBy;
                }

                if (string.IsNullOrEmpty(CTviewModel.ModifiedBy) || (CTviewModel.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    
                        CTviewModel.objpayment.ModifiedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpayment.ModifiedBy = CTviewModel.ModifiedBy;
                }
                if (string.IsNullOrEmpty(CTviewModel.DeletedBy) || (CTviewModel.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                {
                    
                        CTviewModel.objpayment.DeletedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objpayment.DeletedBy = CTviewModel.DeletedBy;
                }

                CTviewModel.objpayment.RefundPaymentTotalAmount = CTviewModel.RefundPaymentTotalAmount;
                CTviewModel.objpayment.Passengerid= (objCTPayvm.PersonnameSelectedItem != null) ? (((SQLDataAccessLayer.Models.PassengerNamelist)objCTPayvm.PersonnameSelectedItem).Passengerid).ToString() : Guid.Empty.ToString();    //objCTPayvm.PaymentTypeID;
                string res = objclntdal.SaveUpdatePaymentDetails("I", CTviewModel.objpayment);
            }
        }
        private void DeletePaymentCommandExecute() {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                PaymentDetails payvmobj = CTPayVM.SelectedItempayment;
                DeletePaymentrefundDetails(payvmobj, CTviewModel.Loginuserid);

            }
        }
        public void DeletePaymentrefundDetails(PaymentDetails payvmobj, string loginuserid)
        {
            CTPayVM.PaymentIDvm = (payvmobj.PaymentID != null) ? payvmobj.PaymentID : Guid.Empty.ToString();
            CTviewModel.DeletedBy = (payvmobj.DeletedBy != null) ? payvmobj.DeletedBy : Guid.Empty.ToString();

            string res = objclntdal.DeletePaymentDetails(CTPayVM.PaymentIDvm, CTviewModel.DeletedBy);
            if (!string.IsNullOrEmpty(res))
            {
                if (res.ToString().ToLower() == "1")
                {
                    MessageBox.Show("Payment details Deleted successfully");
                    CTviewModel.PaymentRefundObser.Remove(CTviewModel.PaymentRefundObser.Where(m => m.PaymentID == payvmobj.PaymentID).FirstOrDefault());


                }
                else if (res.ToString().ToLower() == "-1")
                {
                    if (CTviewModel.PaymentRefundObser.Where(m => m.PaymentID == payvmobj.PaymentID).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Payment details Deleted successfully");
                        CTviewModel.PaymentRefundObser.Remove(CTviewModel.PaymentRefundObser.Where(m => m.PaymentID == payvmobj.PaymentID).FirstOrDefault());

                    }
                   // if (CTviewModel.Itineraryid != null)
                      //  Reterivepaymentdetails(CTviewModel.Itineraryid, loginuserid);
                }
            }
        }

        private void AddPayment()
        {
            PaymentDetails objitpay = new PaymentDetails();

            
            objitpay.PaymentID=Guid.NewGuid().ToString();
            objitpay.Amount = 0;
            objitpay.CurrencyCode = string.Empty;            
            objitpay.FeePercent = 0;
            objitpay.Fee = 0;
            objitpay.DateofPayment = null;
            objitpay.Details = string.Empty;
            objitpay.ExchangeRate = 0;
            objitpay.FeeType = string.Empty;
            objitpay.Inclusive = false;
            objitpay.ItineraryID = CTviewModel.Itineraryid;
            objitpay.PaymentAmount = 0;
            objitpay.PaymentTypeID = string.Empty;
            objitpay.Personname = string.Empty;
            objitpay.Sale = 0;

            CTviewModel.PaymentRefundObser.Add(objitpay);

            //var observablecollectionft = new ObservableCollection<PaymentDetails>(CTviewModel.PaymentRefundObser.ToList());
            //CTviewModel.PaymentRefundObser = observablecollectionft;

        }
        #endregion "Private method"



        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        public DelegateCommand SaveCommand
        {
            get; set;
        }

        public DelegateCommand UpdateCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteCommand
        {
            get;
            set;
        }

        public DelegateCommand AddCommand
        {
            get;
            set;
        }
        public DelegateCommand RetrieveCommand
        {
            get;
            set;
        }

        public bool CanExecuteCommand(string commandName)
        {
            //throw new System.NotImplementedException();
            return true;
        }

        private DelegateCommand _lostFocusCommand;

        public DelegateCommand LostFocusCommand
        {
            get { return _lostFocusCommand; }
            set { _lostFocusCommand = value; }
        }

        private DelegateCommand _PassengerListSelectionChangedCommand;
        public DelegateCommand PassengerListSelectionChangedCommand
        {
            get { return _PassengerListSelectionChangedCommand; }
            set { _PassengerListSelectionChangedCommand = value; }
        }
        
    }

    public class Itinerary_ClienttabRoomTypeCommand
    {
        List<OptionforRoomtypes> listRoomlist = new List<OptionforRoomtypes>();
        public readonly Itinerary_ClientTabViewModel CTviewModel;
        public readonly Itinerary_ClienttabRoomtypeViewmodel CTRoomtypevm;
        private static Clienttabdal objclntdal;

        public Itinerary_ClientTabPaxInformationCommand objclpaxcomd;
        public Itinerary_ClienttabRoomTypeCommand(Itinerary_ClientTabViewModel ctpassvm)
        {
            //FuviewModel = viewModel;
            CTRoomtypevm = ctpassvm.objClRoomtype;
            CTviewModel = ctpassvm;
            CTviewModel.objpayment = new PaymentDetails();
            objclntdal = new Clienttabdal();

            if (CTviewModel.SelectedItemRoomtypevmct != null)
            {
                CTRoomtypevm.SelectedItemRoomtype = CTviewModel.SelectedItemRoomtypevmct;
            }                       

            listRoomlist = objclntdal.RetriveRoomtypes();
            var observablecollectionRoom = new ObservableCollection<OptionforRoomtypes>(listRoomlist);
            CTviewModel.Rmtypeval = observablecollectionRoom;

            AddCommand = new DelegateCommand(ExecuteAdd, CanExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve, CanExecuteRetrieve);

            CTviewModel.PropertyChanged += ViewModel_PropertyChangedPayment;

            objclpaxcomd = new Itinerary_ClientTabPaxInformationCommand(ctpassvm);
            CTviewModel.objpassdet.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;
            CTviewModel.ItinCurDisplayFormat = objclpaxcomd.ItinCurDisplayFormat;
        }

        public new event EventHandler CanExecuteChangedRommtype;
        private void ViewModel_PropertyChangedPayment(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChangedRommtype?.Invoke(this, new EventArgs());
        }
        private bool CanExecuteAdd()
        {
            return CTviewModel.IntrCTRoomTypeViewModel.CanExecuteCommand("Add");
        }

        private void ExecuteAdd()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            AddRoomtypeCommandExecute();
        }

        private bool CanExecuteDelete()
        {
            return CTviewModel.IntrCTRoomTypeViewModel.CanExecuteCommand("Delete");
        }

        private void ExecuteDelete()
        {
            // Implement your Delete logic here
            // Delete the selected item from the list

            if (CTviewModel.SelectedItemRoomtypevmct != null)
            {
                CTRoomtypevm.SelectedItemRoomtype = CTviewModel.SelectedItemRoomtypevmct;
            }
            if (CTRoomtypevm.SelectedItemRoomtype != null)
            {
                DeleteRoomtypeCommandExecute();
                // FuviewModel.Items.Remove(FuviewModel.SelectedItem);
            }
        }

        private bool CanExecuteSave()
        {
            return CTviewModel.IntrCTRoomTypeViewModel.CanExecuteCommand("Save");
        }

        private void ExecuteSave()
        {
            // Implement your Add logic here
            // Add a new item to the list
            SaveRoomtypeCommandExecute();
            // FuviewModel.Items.Add(new FollowupModel());
        }


        private bool CanExecuteRetrieve()
        {
            return CTviewModel.IntrCTRoomTypeViewModel.CanExecuteCommand("Retrieve");
        }

        private void ExecuteRetrieve()
        {
            // Implement your Add logic here
            // Add a new item to the list
            // FuviewModel.Items.Add(new FollowupModel());

            ReteriveRoomtypeCommandExecute();
        }

        #region "Private method"
        private void AddRoomtypeCommandExecute()
        {
            AddRoomtype();
        }
        private void ReteriveRoomtypeCommandExecute() {
            //Itinerary_ClienttabRoomtypeViewmodel roomvmobj = ((LIT.Modules.TabControl.Commands.Itinerary_ClienttabRoomTypeCommand)((System.Delegate)ReteriveRoomtypeCommandExecute).Target).CTRoomtypevm;
            //if (roomvmobj != null)
            //{
            //    ReteriveRoomtypedetails(roomvmobj.Itineraryidvm, roomvmobj.Loginuseridvm);
            //}
                
            if(CTviewModel!=null)
            ReteriveRoomtypedetails(CTviewModel.Itineraryid, CTviewModel.Loginuserid);
            

        }

        private void ReteriveRoomtypedetails(string ItineraryId, string Loginuserid)
        {
            List<RoomTypesClienttab> listRoomtypedet = new List<RoomTypesClienttab>();
            // Folluptask = new ObservableCollection<FollowupViewModel>();
            listRoomtypedet = objclntdal.RetriveRoomtypeclienttab(Guid.Parse(ItineraryId));
            //  var observablecollectionfoltask = new CustomObservable.CustomObservableCollection<FollowupModel>();
            if (listRoomtypedet.Count > 0)
            {
                foreach (RoomTypesClienttab obj in listRoomtypedet)
                {
                    RoomTypesClienttab objRoomtypedet = new RoomTypesClienttab();
                    objRoomtypedet.RoomtypeID = obj.RoomtypeID;
                    objRoomtypedet.OptionTypeRoomid = obj.OptionTypeRoomid;
                    objRoomtypedet.RmsBkd = obj.RmsBkd;
                    objRoomtypedet.PaxBkd = obj.PaxBkd;
                    objRoomtypedet.RmsSold = obj.RmsSold;
                    objRoomtypedet.PaxSold = obj.PaxSold;
                    objRoomtypedet.SellPrice = obj.SellPrice;


                    objRoomtypedet.ItineraryID = obj.ItineraryID;

                    if (!string.IsNullOrEmpty(obj.OptionTypeRoomid))
                    {
                        objRoomtypedet.OptionTypeRoomidselectedItem = listRoomlist.Where(x => x.OptionTypeRoomid == obj.OptionTypeRoomid).FirstOrDefault();
                    }
                    else { objRoomtypedet.OptionTypeRoomidselectedItem = null; }

                    
                    if (string.IsNullOrEmpty(obj.CreatedBy) || (obj.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objRoomtypedet.CreatedBy = Loginuserid;
                    }
                    else
                    {
                        objRoomtypedet.CreatedBy = obj.CreatedBy;
                    }
                    if (string.IsNullOrEmpty(obj.ModifiedBy) || (obj.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objRoomtypedet.ModifiedBy = Loginuserid;
                    }
                    else
                    {
                        objRoomtypedet.ModifiedBy = obj.ModifiedBy;
                    }
                    if (string.IsNullOrEmpty(obj.DeletedBy) || (obj.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                    {
                        objRoomtypedet.DeletedBy = Loginuserid;
                    }
                    else
                    {
                        objRoomtypedet.DeletedBy = obj.DeletedBy;
                    }



                    objRoomtypedet.IsDeleted = obj.IsDeleted;

                    if (CTviewModel.RoomTypesClienttabObser.Where(x => x.RoomtypeID == obj.RoomtypeID).Count() == 0)
                    {
                        CTviewModel.RoomTypesClienttabObser.Add(objRoomtypedet);
                    }
                    else
                    {
                       
                    }
                }
                var observablecollectionft = new ObservableCollection<RoomTypesClienttab>(CTviewModel.RoomTypesClienttabObser.ToList());
                CTviewModel.RoomTypesClienttabObser = null;
                CTviewModel.RoomTypesClienttabObser = observablecollectionft;



            }
        }
        private void SaveRoomtypeCommandExecute()
        {

            foreach (RoomTypesClienttab objCTroomtype in CTviewModel.RoomTypesClienttabObser)
            {
                CTviewModel.objroomTypeclienttab.RoomtypeID = (objCTroomtype.RoomtypeID!=null)? objCTroomtype.RoomtypeID:Guid.NewGuid().ToString();
                CTviewModel.objroomTypeclienttab.OptionTypeRoomid = (objCTroomtype.OptionTypeRoomidselectedItem != null) ? ((OptionforRoomtypes)(objCTroomtype.OptionTypeRoomidselectedItem)).OptionTypeRoomid.ToString() : Guid.Empty.ToString(); 
                
                CTviewModel.objroomTypeclienttab.RmsBkd = objCTroomtype.RmsBkd;
                CTviewModel.objroomTypeclienttab.PaxBkd = objCTroomtype.PaxBkd;
                CTviewModel.objroomTypeclienttab.RmsSold = objCTroomtype.RmsSold;
                CTviewModel.objroomTypeclienttab.PaxSold = objCTroomtype.PaxSold;
                CTviewModel.objroomTypeclienttab.SellPrice = objCTroomtype.SellPrice;
               
                CTviewModel.objroomTypeclienttab.ItineraryID = CTviewModel.Itineraryid;
                if (string.IsNullOrEmpty(CTviewModel.CreatedBy) || (CTviewModel.CreatedBy == "00000000-0000-0000-0000-000000000000"))
                {
                        CTviewModel.objroomTypeclienttab.CreatedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objroomTypeclienttab.CreatedBy = CTviewModel.CreatedBy;
                }

                if (string.IsNullOrEmpty(CTviewModel.ModifiedBy) || (CTviewModel.ModifiedBy == "00000000-0000-0000-0000-000000000000"))
                {
                        CTviewModel.objroomTypeclienttab.ModifiedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objroomTypeclienttab.ModifiedBy = CTviewModel.ModifiedBy;
                }
                if (string.IsNullOrEmpty(CTviewModel.DeletedBy) || (CTviewModel.DeletedBy == "00000000-0000-0000-0000-000000000000"))
                {
                        CTviewModel.objroomTypeclienttab.DeletedBy = CTviewModel.Loginuserid;
                }
                else
                {
                    CTviewModel.objroomTypeclienttab.DeletedBy = CTviewModel.DeletedBy;
                }

                string res = objclntdal.SaveUpdateRoomTypeClientTab("I", CTviewModel.objroomTypeclienttab);
            }
        }
        private void DeleteRoomtypeCommandExecute() {
            MessageBoxResult messageBoxResult = MessageBox.Show("Do you really want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                RoomTypesClienttab roomvmobj = CTRoomtypevm.SelectedItemRoomtype;
                DeleteRoomTypesClienttabDetails(roomvmobj, CTviewModel.Loginuserid);

            }
        }

        private void DeleteRoomTypesClienttabDetails(RoomTypesClienttab roomvmobj, string loginuserid)
        {
            CTRoomtypevm.RoomtypeIDvm = (roomvmobj.RoomtypeID != null) ? roomvmobj.RoomtypeID : Guid.Empty.ToString();
            CTviewModel.DeletedBy = (roomvmobj.DeletedBy != null) ? roomvmobj.DeletedBy : Guid.Empty.ToString();

            string res = objclntdal.DeleteRoomtypeclienttab(CTRoomtypevm.RoomtypeIDvm, CTviewModel.DeletedBy);
            if (!string.IsNullOrEmpty(res))
            {
                if (res.ToString().ToLower() == "1")
                {
                    MessageBox.Show("Room type up Deleted successfully");
                    CTviewModel.RoomTypesClienttabObser.Remove(CTviewModel.RoomTypesClienttabObser.Where(m => m.RoomtypeID == roomvmobj.RoomtypeID).FirstOrDefault());


                }
                else if (res.ToString().ToLower() == "-1")
                {
                    if (CTviewModel.RoomTypesClienttabObser.Where(m => m.RoomtypeID == roomvmobj.RoomtypeID).FirstOrDefault() != null)
                    {
                        MessageBox.Show("Room type Deleted successfully");
                        CTviewModel.RoomTypesClienttabObser.Remove(CTviewModel.RoomTypesClienttabObser.Where(m => m.RoomtypeID == roomvmobj.RoomtypeID).FirstOrDefault());

                    }
                    
                     if (roomvmobj.ItineraryID != null)
                       ReteriveRoomtypedetails(roomvmobj.ItineraryID, loginuserid);
                }
            }
        }


        private void AddRoomtype()
        {
            RoomTypesClienttab objitroomtype = new RoomTypesClienttab();
            if (CTRoomtypevm.RoomtypeIDvm != null)
            {
                objitroomtype.RoomtypeID = CTRoomtypevm.RoomtypeIDvm;
            }
            else
            {
                objitroomtype.RoomtypeID = Guid.NewGuid().ToString();
            }
            objitroomtype.RoomtypeID = CTRoomtypevm.RoomtypeIDvm;            
            objitroomtype.RmsBkd = CTRoomtypevm.RmsBkdvm;            
            objitroomtype.PaxBkd = CTRoomtypevm.PaxBkdvm;
            objitroomtype.RmsSold = CTRoomtypevm.RmsSoldvm;
            objitroomtype.PaxSold = CTRoomtypevm.PaxSoldvm;
            objitroomtype.ItineraryID = CTviewModel.Itineraryid;            
            objitroomtype.SellPrice = CTRoomtypevm.SellPricevm;

            this.CTviewModel.RoomTypesClienttabObser.Add(objitroomtype);

        }
        #endregion "Private method"



        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();
        public DelegateCommand SaveCommand
        {
            get; set;
        }

        public DelegateCommand UpdateCommand
        {
            get;
            set;
        }

        public DelegateCommand DeleteCommand
        {
            get;
            set;
        }

        public DelegateCommand AddCommand
        {
            get;
            set;
        }
        public DelegateCommand RetrieveCommand
        {
            get;
            set;
        }

        public bool CanExecuteCommand(string commandName)
        {
            //throw new System.NotImplementedException();
            return true;
        }

    }
}
