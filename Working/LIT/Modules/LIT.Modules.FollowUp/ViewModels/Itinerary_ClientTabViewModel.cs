using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LIT.Common;
using LIT.Core.Mvvm;
using LIT.Modules.TabControl.Commands;
using Prism.Mvvm;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;

namespace LIT.Modules.TabControl.ViewModels
{
    public class Itinerary_ClientTabViewModel : BindableBase
    {

        private readonly Itinerary_ClientTabPassengerCommand _CTPassengerCommand;
        public Itinerary_ClientTabPassengerCommand CTPassengerCommand { get; set; }

        private Itinerary_ClienttabPaymentCommand _CTPaymentCommand;
        public Itinerary_ClienttabPaymentCommand CTPaymentCommand { get; set; }

        private readonly Itinerary_ClienttabRoomTypeCommand _CTRoomTypeCommand;
        public Itinerary_ClienttabRoomTypeCommand CTRoomTypeCommand { get; set; }

        public Itinerary_ClientTabPaxInformationCommand CTPaxinfoCommand { get; set; }
        public IOperations IntrCTPassengerViewModel { get; private set; }
        public IOperations IntrCTPaymentViewModel { get; private set; }
        public IOperations IntrCTRoomTypeViewModel { get; private set; }

        public IOperations IntrCTPaxinfoViewModel { get; private set; }
        #region private

        
        

        private static ItineraryDAL _objitindal;
        private static Clienttabdal objclntdal;

        //private static Itinerary_ClientTabPassengerViewModel objClPassvm;
        //  private static Itinerary_ClientTabPaymentViewmodel objClPaymentvm;

        private string _Paxid;
        private int _PaxNumbers;



        private long _Totalpassenger;
        private decimal _RefundPaymentTotalAmount;

        private long _Bookingid;
        private string _Itineraryid;
        private string _CreatedBy;
        private string _ModifiedBy;
        private bool _IsDeleted;
        private string _DeletedBy;
        private string _Loginuserid;
        private string _ItinCurDisplayFormat;


        #endregion
        #region Public

        #region "Pax"
        public string Paxid
        {
            get { return _Paxid; }
            set
            {
                SetProperty(ref _Paxid, value);

            }
        }
        public int PaxNumbers { get { return _PaxNumbers; } set { SetProperty(ref _PaxNumbers, value); } }

        #endregion "Pax"


        

        
        public string ItinCurDisplayFormat
        {
            get { return _ItinCurDisplayFormat; }
            set { SetProperty(ref _ItinCurDisplayFormat, value); }
        }
        private PassengerDetails _selectedItemvmct;
        public PassengerDetails SelectedItemvmct
        {
            get { return _selectedItemvmct; }
            set { SetProperty(ref _selectedItemvmct, value); }
        }


        private RoomTypesClienttab _selectedItemRoomtypevmct;
        public RoomTypesClienttab SelectedItemRoomtypevmct
        {
            get { return _selectedItemRoomtypevmct; }
            set { SetProperty(ref _selectedItemRoomtypevmct, value); }
        }

        private PaymentDetails _SelectedItempaymentvmct;
        public PaymentDetails SelectedItempaymentvmct
        {
            get { return _SelectedItempaymentvmct; }
            set { SetProperty(ref _SelectedItempaymentvmct, value); }
        }
        public long Bookingid
        {
            get { return _Bookingid; }
            set
            {
                SetProperty(ref _Bookingid, value);
            }
        }

        public string Itineraryid
        {
            get { return _Itineraryid; }
            set
            {
                SetProperty(ref _Itineraryid, value);
            }
        }

        public string CreatedBy
        {
            get { return _CreatedBy; }
            set
            {
                SetProperty(ref _CreatedBy, value);
            }
        }

        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set
            {
                SetProperty(ref _ModifiedBy, value);
            }
        }

        public string DeletedBy
        {
            get { return _DeletedBy; }
            set
            {
                SetProperty(ref _DeletedBy, value);
            }
        }

        public string Loginuserid
        {
            get { return _Loginuserid; }
            set
            {
                SetProperty(ref _Loginuserid, value);
            }
        }
        public long Totalpassenger
        {
            get { return _Totalpassenger; }
            set
            {
                SetProperty(ref _Totalpassenger, value);
            }
        }

        public decimal RefundPaymentTotalAmount
        {
            get { return _RefundPaymentTotalAmount; }
            set
            {
                SetProperty(ref _RefundPaymentTotalAmount, value);
            }
        }
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set
            {
                SetProperty(ref _IsDeleted, value);
            }
        }


        #region Commands
        public RelayCommand SavePaxInfoCommand
        {
            get;
            set;
        }

        public RelayCommand ReterivePaxInfoCommand
        {
            get;
            set;
        }
        public RelayCommand AddRoomTypesCommand
        {
            get;
            set;
        }
        public RelayCommand ReteriveRoomTypesCommand
        {
            get;
            set;
        }





        public Itinerary_ClientTabPassengerViewModel objClPassvm { get; set; }
        public Itinerary_ClientTabPaymentViewmodel objClPayment { get; set; }

        public Itinerary_ClienttabRoomtypeViewmodel objClRoomtype { get; set; }

        #endregion "Commands" 
        #region "observable Collection"


        #region "Passenger Observable collection"
        private ObservableCollection<string> _Payeeval;
        public ObservableCollection<string> Payeeval
        {
            get { return _Payeeval ?? (_Payeeval = new ObservableCollection<string>()); }
            set
            {
                _Payeeval = value;
                
            }
        }

        private ObservableCollection<string> _PassengerStatusval;
        public ObservableCollection<string> PassengerStatusval
        {
            get { return _PassengerStatusval ?? (_PassengerStatusval = new ObservableCollection<string>()); }
            set
            {
                _PassengerStatusval = value;
                //_CTPassengerCommand.rai
            }
        }

        private ObservableCollection<CommonValueList> _AgenttovalPass;
        public ObservableCollection<CommonValueList> AgenttovalPass
        {
            get { return _AgenttovalPass ?? (_AgenttovalPass = new ObservableCollection<CommonValueList>()); }
            set
            {
                SetProperty(ref _AgenttovalPass, value);
                //_CTPassengerCommand.
               
            }
        }

        private ObservableCollection<Currencydetails> _Currencydetailsval;
        public ObservableCollection<Currencydetails> Currencydetailsval
        {
            get { return _Currencydetailsval ?? (_Currencydetailsval = new ObservableCollection<Currencydetails>()); }
            set
            {
                SetProperty(ref _Currencydetailsval, value);
                //CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<OptionforRoomtypes> _Roomtypeval;
        public ObservableCollection<OptionforRoomtypes> Roomtypeval
        {
            get { return _Roomtypeval ?? (_Roomtypeval = new ObservableCollection<OptionforRoomtypes>()); }
            set
            {
                SetProperty(ref _Roomtypeval, value);
                //_CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }        

        private ObservableCollection<AgeGroupValues> _AgeGroupval;
        public ObservableCollection<AgeGroupValues> AgeGroupval
        {
            get { return _AgeGroupval ?? (_AgeGroupval = new ObservableCollection<AgeGroupValues>()); }
            set
            {
                SetProperty(ref _AgeGroupval, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        //private ObservableCollection<Userdetails> _PassengerStatusval;
        //public ObservableCollection<Userdetails> PassengerStatusval
        //{
        //    get { return _PassengerStatusval ?? (_PassengerStatusval = new ObservableCollection<Userdetails>()); }
        //    set
        //    {
        //        _PassengerStatusval = value;
        //    }
        //}

        private ObservableCollection<CommonValueCountrycity> _PassengerCountrylist;
        public ObservableCollection<CommonValueCountrycity> PassengerCountrylist
        {
            get { return _PassengerCountrylist ?? (_PassengerCountrylist = new ObservableCollection<CommonValueCountrycity>()); }
            set
            {
                SetProperty(ref _PassengerCountrylist, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }


        private ObservableCollection<PassengergroupValues> _Passengergroupval;
        public ObservableCollection<PassengergroupValues> Passengergroupval
        {
            get { return _Passengergroupval ?? (_Passengergroupval = new ObservableCollection<PassengergroupValues>()); }
            set
            {
                SetProperty(ref _Passengergroupval, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PassengerTypeValues> _Passengertypeval;
        public ObservableCollection<PassengerTypeValues> Passengertypeval
        {
            get { return _Passengertypeval ?? (_Passengertypeval = new ObservableCollection<PassengerTypeValues>()); }
            set
            {
                SetProperty(ref _Passengertypeval, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PaymenttypeValues> _Payementtype;
        public ObservableCollection<PaymenttypeValues> Payementtype
        {
            get { return _Payementtype ?? (_Payementtype = new ObservableCollection<PaymenttypeValues>()); }
            set
            {
                SetProperty(ref _Payementtype, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PassengerNamelist> _Passengernamelist;
        public ObservableCollection<PassengerNamelist> Passengernamelist
        {
            get { return _Passengernamelist ?? (_Passengernamelist = new ObservableCollection<PassengerNamelist>()); }
            set
            {
                SetProperty(ref _Passengernamelist, value);
               // _CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<PassengerDetails> _PassengerDetailsobser;
        public ObservableCollection<PassengerDetails> PassengerDetailsobser
        {
            get { return _PassengerDetailsobser ?? (_PassengerDetailsobser = new ObservableCollection<PassengerDetails>()); }
            set
            {

                SetProperty(ref _PassengerDetailsobser, value);
                //_CTPassengerCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion "Passenger Observable collection"
        #region "Room type client tab observable collection"
        private ObservableCollection<OptionforRoomtypes> _Rmtypeval;
        public ObservableCollection<OptionforRoomtypes> Rmtypeval
        {
            get { return _Rmtypeval ?? (_Rmtypeval = new ObservableCollection<OptionforRoomtypes>()); }
            set
            {
                SetProperty(ref _Rmtypeval, value);
               // _CTRoomTypeCommand.RaiseCanExecuteChanged();
            }
        }

        private ObservableCollection<RoomTypesClienttab> _RoomTypesClienttabObser;
        public ObservableCollection<RoomTypesClienttab> RoomTypesClienttabObser
        {
            get { return _RoomTypesClienttabObser ?? (_RoomTypesClienttabObser = new ObservableCollection<RoomTypesClienttab>()); }
            set
            {
                SetProperty(ref _RoomTypesClienttabObser, value);
                //_CTRoomTypeCommand.RetrieveCommand.Execute();
            }
        }
        #endregion "Room type client tab observable collection"

        #region "PaymentRefund observable collection"
        private ObservableCollection<PaymentDetails> _PaymentRefundObser;
        public ObservableCollection<PaymentDetails> PaymentRefundObser
        {
            get { return _PaymentRefundObser ?? (_PaymentRefundObser = new ObservableCollection<PaymentDetails>()); }
            set
            {
                _PaymentRefundObser = value;
            }
        }
        #endregion "PaymentRefund observable collection"

        #endregion "observable Collection"
        #endregion

        public PassengerDetails objpassdet { get; set; }
        public PaymentDetails objpayment { get; set; }
        public RoomTypesClienttab objroomTypeclienttab { get; set; }
        public Paxinformation objpaxinformation { get; set; }

        #region "Constructor"
        public Itinerary_ClientTabViewModel()//IRegionManager regionManager) :
           // base(regionManager)
        {
            //objClPassvm = new Itinerary_ClientTabPassengerViewModel(regionManager);

            //objClPayment = new Itinerary_ClientTabPaymentViewmodel(regionManager);

            //objClRoomtype = new Itinerary_ClienttabRoomtypeViewmodel(regionManager);

            objClPassvm = new Itinerary_ClientTabPassengerViewModel();

            objClPayment = new Itinerary_ClientTabPaymentViewmodel();

            objClRoomtype = new Itinerary_ClienttabRoomtypeViewmodel();

            CTPassengerCommand = new Itinerary_ClientTabPassengerCommand(this);
            IntrCTPassengerViewModel = new Itinerary_ClientTabPassengerCommand(this);
            IntrCTPaymentViewModel = new Itinerary_ClienttabPaymentCommand(this);
            CTPaymentCommand = new Itinerary_ClienttabPaymentCommand(this);

            IntrCTRoomTypeViewModel = new Itinerary_ClienttabPaymentCommand(this);
            CTRoomTypeCommand = new Itinerary_ClienttabRoomTypeCommand(this);

            IntrCTPaxinfoViewModel = new Itinerary_ClientTabPaxInformationCommand(this);
            CTPaxinfoCommand = new Itinerary_ClientTabPaxInformationCommand(this);

            // this.SavePaxInfoCommand = new DelegateCommand(SavePaxInfoCommandExecute);
            // this.ReterivePaxInfoCommand = new DelegateCommand(ReterivePaxInfoCommandExecute);
            _CTRoomTypeCommand = new Itinerary_ClienttabRoomTypeCommand(this);
            objpassdet = new PassengerDetails();
            objpayment = new PaymentDetails();
            objroomTypeclienttab = new RoomTypesClienttab();
            objpaxinformation = new Paxinformation();

        }
        #endregion "Contructor"

        #region "Private Methods"
        //private void SavePaxInfoCommandExecute()
        //{
        //    CTPaxinfoCommand.SaveCommand.Execute();
        //}
        //private void ReterivePaxInfoCommandExecute()
        //{ }

        #endregion "Private Methods"

    }

    public class Itinerary_ClientTabPassengerViewModel : BindableBase
    {

        private static PassengerDetails objpassdet;
        private PassengerDetails _selectedItem;
        public PassengerDetails SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        #region "Passenger details private"
        private string _Passengerid;
        private int _Age;
        private string _AgeGroup;
        private object _AgegroupselectedItem;
        private string _Agent;
        private object _AgentselectedItem;
        private decimal _AgentNet;
        private decimal _CommissionOverride;
        private decimal _CommissionPercentage;
        private string _Comments;
        private string _CompanyName;
        private string _Country;
        private decimal _DefaultPrice;
        private string _DisplayName;
        private string _Email;
        private string _FirstName;
        private string _LastName;
        private string _PassengerStatus;
        private object _PassengerStatusselectedItem;
        private string _PassengerType;
        private object _PassengerTypeselectedItem;
        private string _Payee;
        private bool _PayingPax;
        private decimal _PriceOverride;
        private string _Room;
        private string _Rommtype;
        private object _RommtypeselectedItem;
        private DateTime? _Saledate;
        private string _Title;
        private string _GroupName;
        private object _GroupNameselectedItem;
        private object _PayeeSelectedItem;
        private object _CountrySelectedItem;
        //private long _Bookingid;
        //private string _Itineraryid;
        //private string _CreatedBy;
        //private string _ModifiedBy;
        //private bool _IsDeleted;
        //private string _DeletedBy;
        //private string _Loginuserid;




        #endregion "Passenger details private"

        #region "Passenger details public"
        public string Passengerid { get { return _Passengerid; } set { SetProperty(ref _Passengerid, value); } }
        public int Age { get { return _Age; } set { SetProperty(ref _Age, value); } }
        public string AgeGroup { get { return _AgeGroup; } set { SetProperty(ref _AgeGroup, value); } }
        public object AgegroupselectedItem
        {
            get { return _AgegroupselectedItem; }
            set
            {
                SetProperty(ref _AgegroupselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public string Agent { get { return _Agent; } set { SetProperty(ref _Agent, value); } }

        public object AgentSelectedItem
        {
            get { return _AgentselectedItem; }
            set
            {
                SetProperty(ref _AgentselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public decimal AgentNet { get { return _AgentNet; } set { SetProperty(ref _AgentNet, value); } }
        public decimal CommissionOverride { get { return _CommissionOverride; } set { SetProperty(ref _CommissionOverride, value); } }
        public decimal CommissionPercentage { get { return _CommissionPercentage; } set { SetProperty(ref _CommissionPercentage, value); } }
        public string Comments { get { return _Comments; } set { SetProperty(ref _Comments, value); } }
        public string CompanyName { get { return _CompanyName; } set { SetProperty(ref _CompanyName, value); } }
        public string Country { get { return _Country; } set { SetProperty(ref _Country, value); } }


        public decimal DefaultPrice { get { return _DefaultPrice; } set { SetProperty(ref _DefaultPrice, value); } }
        public string DisplayName { get { return _DisplayName; } set { SetProperty(ref _DisplayName, value); } }
        public string Email { get { return _Email; } set { SetProperty(ref _Email, value); } }
        public string FirstName { get { return _FirstName; } set { SetProperty(ref _FirstName, value); } }
        public string LastName { get { return _LastName; } set { SetProperty(ref _LastName, value); } }
        public string PassengerStatus { get { return _PassengerStatus; } set { SetProperty(ref _PassengerStatus, value); } }


        public object PassengerStatusselectedItem
        {
            get { return _PassengerStatusselectedItem; }
            set
            {
                SetProperty(ref _PassengerStatusselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public string PassengerType { get { return _PassengerType; } set { SetProperty(ref _PassengerType, value); } }
        public object PassengertypeSelectedItem
        {
            get { return _PassengerTypeselectedItem; }
            set
            {
                SetProperty(ref _PassengerTypeselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public string Payee { get { return _Payee; } set { SetProperty(ref _Payee, value); } }

        public object PayeeSelectedItem
        {
            get { return _PayeeSelectedItem; }
            set
            {
                SetProperty(ref _PayeeSelectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public bool PayingPax { get { return _PayingPax; } set { SetProperty(ref _PayingPax, value); } }
        public decimal PriceOverride { get { return _PriceOverride; } set { SetProperty(ref _PriceOverride, value); } }
        public string Room { get { return _Room; } set { SetProperty(ref _Room, value); } }
        public string Rommtype { get { return _Rommtype; } set { SetProperty(ref _Rommtype, value); } }
        public object RommtypeselectedItem
        {
            get { return _RommtypeselectedItem; }
            set
            {
                SetProperty(ref _RommtypeselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }
        public DateTime? Saledate { get { return _Saledate; } set { SetProperty(ref _Saledate, value); } }
        public string Title { get { return _Title; } set { SetProperty(ref _Title, value); } }

        public string GroupName { get { return _GroupName; } set { SetProperty(ref _GroupName, value); } }

        public object GroupNameSelectedItem
        {
            get { return _GroupNameselectedItem; }
            set
            {
                SetProperty(ref _GroupNameselectedItem, value);
                //  passen.RaiseCanExecuteChanged();
            }
        }

        public object CountrySelectedItem { get { return _CountrySelectedItem; } set { SetProperty(ref _CountrySelectedItem, value); } }

        //public long Bookingid { get { return _Bookingid; } set { SetProperty(ref _Bookingid, value); } }

        //public string Itineraryid { get { return _Itineraryid; } set { SetProperty(ref _Itineraryid, value); } }

        //public string CreatedBy { get { return _CreatedBy; } set { SetProperty(ref _CreatedBy, value); }}

        //public string ModifiedBy { get { return _ModifiedBy; } set { SetProperty(ref _ModifiedBy, value); }}

        //public string DeletedBy { get { return _DeletedBy; } set { SetProperty(ref _DeletedBy, value); } }

        //public string Loginuserid { get { return _Loginuserid; } set { SetProperty(ref _Loginuserid, value);} }

        //public bool IsDeleted { get { return _IsDeleted; } set { SetProperty(ref _IsDeleted, value); }}
        #endregion "Passenger details"

        //#region Commands       
        //public DelegateCommand AddPassengerDetailsCommand
        //{
        //    get;
        //    set;
        //}
        //public DelegateCommand ReterivePassengerDetailsCommand
        //{
        //    get;
        //    set;
        //}
        //public DelegateCommand SavePassengerDetailsCommand
        //{
        //    get;
        //    set;
        //}
        //public DelegateCommand DeletePassengerDetailsCommand
        //{
        //    get;
        //    set;
        //}


        //#endregion "Commands"



        #region "constructor"

        LoadDropDownListValues loadDropDownListValues = new LoadDropDownListValues();


        public Itinerary_ClientTabPassengerViewModel()
            //(IRegionManager regionManager) :  base(regionManager)
        {

        }
        #endregion "Contructor"





    }

    public class Itinerary_ClienttabRoomtypeViewmodel : BindableBase //RegionViewModelBase
    {
        private RoomTypesClienttab _selectedItemRoomtype;
        public RoomTypesClienttab SelectedItemRoomtype
        {
            get { return _selectedItemRoomtype; }
            set { SetProperty(ref _selectedItemRoomtype, value); }
        }
        #region "Room details private"
        private string _RoomtypeIDvm;
        private long _Bookingidvm;
        private int _RmsBkdvm;
        private int _PaxBkdvm;
        private int _RmsSoldvm;
        private int _PaxSoldvm;
        private decimal _SellPricevm;
        private string _OptionTypeRoomid;
        private object _OptionTypeRoomidselectedItem;

        //private string _Itineraryidvm;
        //private string _CreatedByvm;
        //private string _ModifiedByvm;
        //private bool _IsDeletedvm;
        //private string _DeletedByvm;
        //private string _Loginuseridvm;
        #endregion "Room details"

        #region "Room details public"

        public object OptionTypeRoomidselectedItem
        {
            get { return _OptionTypeRoomidselectedItem; }
            set
            { SetProperty(ref _OptionTypeRoomidselectedItem, value); }
        }
        public string OptionTypeRoomid { get { return _OptionTypeRoomid; } set { SetProperty(ref _OptionTypeRoomid, value); } }
        public string RoomtypeIDvm { get { return _RoomtypeIDvm; } set { SetProperty(ref _RoomtypeIDvm, value); } }
        public int RmsBkdvm { get { return _RmsBkdvm; } set { SetProperty(ref _RmsBkdvm, value); } }

        public int PaxBkdvm { get { return _PaxBkdvm; } set { SetProperty(ref _PaxBkdvm, value); } }
        public int RmsSoldvm { get { return _RmsSoldvm; } set { SetProperty(ref _RmsSoldvm, value); } }
        public int PaxSoldvm { get { return _PaxSoldvm; } set { SetProperty(ref _PaxSoldvm, value); } }
        public decimal SellPricevm { get { return _SellPricevm; } set { SetProperty(ref _SellPricevm, value); } }



        public Itinerary_ClienttabRoomtypeViewmodel() //(IRegionManager regionManager) : base(regionManager)
        {

        }

        //public long Bookingidvm { get { return _Bookingidvm; } set { SetProperty(ref _Bookingidvm, value); } }

        //public string Itineraryidvm { get { return _Itineraryidvm; } set { SetProperty(ref _Itineraryidvm, value); } }

        //public string CreatedByvm { get { return _CreatedByvm; } set { SetProperty(ref _CreatedByvm, value); }}

        //public string ModifiedByvm { get { return _ModifiedByvm; } set { SetProperty(ref _ModifiedByvm, value); }}

        //public string DeletedByvm { get { return _DeletedByvm; } set { SetProperty(ref _DeletedByvm, value); } }

        //public string Loginuseridvm { get { return _Loginuseridvm; } set { SetProperty(ref _Loginuseridvm, value);} }

        //public bool IsDeletedvm { get { return _IsDeletedvm; } set { SetProperty(ref _IsDeletedvm, value); }}

        #endregion "Room types details"
    }
    public class Itinerary_ClientTabPaymentViewmodel : BindableBase //RegionViewModelBase
    {

        private PaymentDetails _selectedItempayment;
        public PaymentDetails SelectedItempayment
        {
            get { return _selectedItempayment; }
            set { SetProperty(ref _selectedItempayment, value); }
        }
        #region "Payment details private"
        private string _PaymentIDvm;
        private decimal _Amountvm;
        private string _CurrencyCodevm;
        private DateTime? _DateofPaymentvm;
        private string _Detailsvm;
        private decimal _ExchangeRatevm;
        private decimal _Feevm;
        private decimal _FeePercentvm;
        private string _FeeTypevm;
        private bool _Inclusivevm;
        private decimal _PaymentAmountvm;
        private string _PaymentTypeIDvm;
        private string _Personnamevm;
        private decimal _Salevm;
        private object _FeeTypeSelectedItem;
        private object _PaymentTypeSelectedItem;
        private object _CurrencyCodeSelectedItem;
        private object _PersonnameSelectedItem;


        
        //private long _Bookingidvm;
        //private string _Itineraryidvm;
        //private string _CreatedByvm;
        //private string _ModifiedByvm;
        //private bool _IsDeletedvm;
        //private string _DeletedByvm;
        //private string _Loginuseridvm;
        #endregion "Payment details"

        #region "Payment details public"
        public string PaymentIDvm { get { return _PaymentIDvm; } set { SetProperty(ref _PaymentIDvm, value); } }
        public decimal Amountvm { get { return _Amountvm; } set { SetProperty(ref _Amountvm, value); } }
        public string CurrencyCodevm { get { return _CurrencyCodevm; } set { SetProperty(ref _CurrencyCodevm, value); } }
        public DateTime? DateofPaymentvm { get { return _DateofPaymentvm; } set { SetProperty(ref _DateofPaymentvm, value); } }
        public string Detailsvm { get { return _Detailsvm; } set { SetProperty(ref _Detailsvm, value); } }
        public decimal ExchangeRatevm { get { return _ExchangeRatevm; } set { SetProperty(ref _ExchangeRatevm, value); } }
        public decimal Feevm { get { return _Feevm; } set { SetProperty(ref _Feevm, value); } }
        public decimal FeePercentvm { get { return _FeePercentvm; } set { SetProperty(ref _FeePercentvm, value); } }
        public string FeeTypevm { get { return _FeeTypevm; } set { SetProperty(ref _FeeTypevm, value); } }
        public bool Inclusivevm { get { return _Inclusivevm; } set { SetProperty(ref _Inclusivevm, value); } }
        public decimal PaymentAmountvm { get { return _PaymentAmountvm; } set { SetProperty(ref _PaymentAmountvm, value); } }
        public string PaymentTypeIDvm { get { return _PaymentTypeIDvm; } set { SetProperty(ref _PaymentTypeIDvm, value); } }
        public string Personnamevm { get { return _Personnamevm; } set { SetProperty(ref _Personnamevm, value); } }
        public decimal Salevm { get { return _Salevm; } set { SetProperty(ref _Salevm, value); } }

        public object PaymentTypeSelectedItem
        {
            get { return _PaymentTypeSelectedItem; }
            set
            { SetProperty(ref _PaymentTypeSelectedItem, value); }
        }

        public object CurrencyCodeSelectedItem
        {
            get { return _CurrencyCodeSelectedItem; }
            set
            { SetProperty(ref _CurrencyCodeSelectedItem, value); }
        }

        public object FeeTypeSelectedItem
        {
            get { return _FeeTypeSelectedItem; }
            set
            { SetProperty(ref _FeeTypeSelectedItem, value); }
        }
        public object PersonnameSelectedItem
        {
            get { return _PersonnameSelectedItem; }
            set
            { SetProperty(ref _PersonnameSelectedItem, value); }
        }
        

        //public long Bookingidvm { get { return _Bookingidvm; } set { SetProperty(ref _Bookingidvm, value); } }

        //public string Itineraryidvm { get { return _Itineraryidvm; } set { SetProperty(ref _Itineraryidvm, value); } }

        //public string CreatedByvm { get { return _CreatedByvm; } set { SetProperty(ref _CreatedByvm, value); } }

        //public string ModifiedByvm { get { return _ModifiedByvm; } set { SetProperty(ref _ModifiedByvm, value); } }

        //public string DeletedByvm { get { return _DeletedByvm; } set { SetProperty(ref _DeletedByvm, value); } }

        //public string Loginuseridvm { get { return _Loginuseridvm; } set { SetProperty(ref _Loginuseridvm, value); } }

        //public bool IsDeletedvm { get { return _IsDeletedvm; } set { SetProperty(ref _IsDeletedvm, value); } }

        #endregion "Payment details"


        #region "Contructor Payment"
        public Itinerary_ClientTabPaymentViewmodel()//(IRegionManager regionManager) : base(regionManager)
        {

        }
        #endregion "Contructor Payment"


    }
}
