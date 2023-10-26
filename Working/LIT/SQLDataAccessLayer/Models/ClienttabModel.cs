
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace SQLDataAccessLayer.Models
{
    public class Paxinformationdata : BindableBase
    {
        public string Paxid { get; set; }
        public long PaxNumbers { get; set; }
        public long Bookingid { get; set; }
        public string ItineraryID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        public bool Groupoption { get; set; }
        public bool Individualoption { get; set; }
    }
    public class PassengerDetails: BindableBase
    {
        public string ItinCurDisplayFormat { get; set; }
        public object AgegroupselectedItem { get; set; }
        public int AgegroupselectedItemIndex { get; set; }

        public object AgentselectedItem { get; set; }
        public int AgentselectedItemIndex{ get; set; }
        public object PassengerStatusselectedItem { get; set; }
        public int PassengerStatusselectedItemIndex { get; set; }
        public object PassengerTypeselectedItem { get; set; }
        public int PassengerTypeselectedItemIndex { get; set; }

        public object RommtypeselectedItem { get; set; }
        public object RommtypeselectedItemIndex { get; set; }

        public object PassengerGroupNameselectedItem { get; set; }
        public int PassengerGroupNameselectedItemIndex { get; set; }
        public object PayeeSelectedItem { get; set; }
        public int PayeeSelectedItemIndex { get; set; }

        public object CountrySelectedItem { get; set; }
        public int CountrySelectedItemIndex { get; set; }
        public int SalutationSelectedItemIndex { get; set; }

        private object _SalutationSelectedItem;

        public object SalutationSelectedItem
        {
            get
            {
                return _SalutationSelectedItem;
            }
            set
            {
                _SalutationSelectedItem = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SalutationSelectedItem"));
                //SetProperty(ref _LeadPassenger, value);                
            }
        }



        public object RegionSelectedItem { get; set; }
        public int RegionSelectedItemIndex { get; set; }
        public object StateSelectedItem { get; set; }
        public int StateSelectedItemIndex { get; set; }
        public object CitySelectedItem { get; set; }
        public int CitySelectedItemIndex { get; set; }

        public string Passengerid { get; set; }
        public int? Age { get; set; }
        public string AgeGroup { get; set; }
        public string Agent { get; set; }
        public decimal? AgentNet { get; set; }
        public decimal? CommissionOverride { get; set; }
        public decimal? CommissionPercentage { get; set; }
        public string Comments { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public decimal? DefaultPrice { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassengerStatus { get; set; }
        public string PassengerType { get; set; }
        public string Payee { get; set; }
        public bool PayingPax { get; set; }
        public decimal? PriceOverride { get; set; }
        public string Room { get; set; }
        public string Roomtype { get; set; }
        public DateTime? Saledate { get; set; }
        public string Title { get; set; }
        public string ItineraryID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        public string GroupName { get; set; }

        public long Totalpassenger { get; set; }
        public string SalutationID { get; set; }
        private bool _LeadPassenger;
        public bool LeadPassenger
        {
            get
            {
                return _LeadPassenger;
            }
            set
            {
                _LeadPassenger = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("LeadPassenger"));
                //SetProperty(ref _LeadPassenger, value);                
            }
        }

        public string Address { get;set; }
        public string State { get;set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Postcode { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                //  handler(this, new PropertyChangedEventArgs(e));
                this.PropertyChanged(this, e);
            }
        }

    }

    public class PaymentDetails : BindableBase
    {
        public string ItinCurDisplayFormat { get; set; }
        public string PaymentID { get; set; }
        public decimal Amount { get; set; }

        public string Passengerid { get; set; }
        public decimal RefundPaymentTotalAmount { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime? DateofPayment { get; set; }
        public string Details { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal Fee { get; set; }
        public decimal FeePercent { get; set; }
        public string FeeType { get; set; }
        public bool Inclusive { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentTypeID { get; set; }
        public string Personname { get; set; }
        public decimal Sale { get; set; }
        public string ItineraryID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public object PaymentTypeSelectedItem { get; set; }
        public object CurrencyCodeSelectedItem { get; set; }
        public object FeeTypeSelectedItem { get; set; }
        public object PersonnameSelectedItem { get; set; }

        public int PaymentTypeSelectedItemIndex { get; set; }
        public int CurrencyCodeSelectedItemIndex { get; set; }
        public int FeeTypeSelectedItemIndex { get; set; }
        public int PersonnameSelectedItemIndex { get; set; }


    }

    public class RoomTypesClienttab
    {
        public string ItinCurDisplayFormat { get; set; }
        public string RoomtypeID { get; set; }

        public string OptionTypeRoomid { get; set; }
        public int RmsBkd
        {
            get; set;
        }

        public int PaxBkd
        {
            get; set;
        }
        public int RmsSold
        {
            get; set;
        }
        public int PaxSold
        {
            get; set;
        }
        public decimal SellPrice
        {
            get; set;
        }

        public object OptionTypeRoomidselectedItem { get; set; }
        public int OptionTypeRoomidselectedItemIndex { get; set; }

        public string ItineraryID { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OptionforRoomtypes
    {
        public string OptionTypeRoomid { get; set; }
        public string OptionTypesName { get; set; }
        public string Divisor { get; set; }
    }

    public class PassengerNamelist
    {
        public string Passengerid { get; set; }
        public string PassengerDisplayName { get; set; }
    }

        public class PassengerTypeValues
    {
        public string PassengerTypeid { get; set; }
        public string PassengerTypename { get; set; }

    }

    public class PassengergroupValues
    {
        public string Passengergroupid { get; set; }
        public string Passengergroupname { get; set; }

    }

    public class PaymenttypeValues
    {
        public string Paymenttypesid { get; set; }
        public string Paymenttypesname { get; set; }
        public string Isdefault { get; set; }
    }

    public class AgeGroupValues
    {
        public string AgeGroupsid { get; set; }
        public string AgeGroupsname { get; set; }
    }
}