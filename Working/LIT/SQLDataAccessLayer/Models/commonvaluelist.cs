
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class CommonValueList
    {
        public Guid ValueField { get; set; }
        public string TextField { get; set; }

        public bool IsDefault { get; set; }
    }

    public class CommonValueCountrycity
    {
        public Guid CountryId { get; set; }
        public string CountryName { get; set; }

        public Guid CityId { get; set; }
        public string CityName { get; set; }

        public Guid StatesId { get; set; }
        public string StatesName { get; set; }

        public Guid RegionId { get; set; }
        public string RegionName { get; set; }
    }

    public class Errorlogobj
    {
        public string PageName { get; set; }
        public string FunctionName { get; set; }
        public string MessageDescription { get; set; }
        public string CreatedBy { get; set; }
        public string ErrorFrom { get; set; }
    }

    public class Currencydetails
    {
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public bool Isenable { get; set; }
        public string DisplayFormat { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string CurrencyCulture { get; set; }

        
    }
    public class BkRequestStatus: BindableBase, INotifyPropertyChanged
    {
        

        private string _RequestStatusID;
        public string RequestStatusID
        {
            get { return _RequestStatusID; }
            set
            {
                _RequestStatusID = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("RequestStatusID"));
            }
        }

        private string _RequestStatusName;
        public string RequestStatusName
        {
            get { return _RequestStatusName; }
            set
            {
                _RequestStatusName = value;
                // OnPropertyChangedWithNotification(ref _RequestStatusName, value);
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("RequestStatusName"));
            }
        }


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

    public class CurrencySettingvalues
    {
        public long CurrencySettings { get; set; }
        public Guid? FromCurrencyId { get; set; }
        public decimal? FromCurrencyValue { get; set; }
        public Guid? ToCurrencyId { get; set; }
        public decimal? ToCurrencyValue { get; set; }
        public DateTime? CurrencyFromDate { get; set; }
        public DateTime? CurrencyToDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }

    public class CommunicationTypeStatus : INotifyPropertyChanged
    {


        private string _TypeID;
        public string TypeID
        {
            get { return _TypeID; }
            set
            {
                _TypeID = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("TypeID"));
            }
        }

        private string _TypeName;
        public string TypeName
        {
            get { return _TypeName; }
            set
            {
                _TypeName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("TypeName"));
            }
        }


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
    public class FTPServerdetails
    {
        public string FTPUrl { get; set; }
        public string FTPUsername { get; set; }
        public string FTPPassword { get; set; }
    }

    public class  Pickupdroplocation
    {
        public string PickupDropLocationId { get; set; }
        public string LocationName { get; set; }
    }

    public class bookingitemlist
    {
        public long BookingID { get; set; }
        public string BookingName { get; set; }

    }

    public class Salutation
    {
        public string SalutationId { get; set; }
        public string SalutationName { get; set; }
    }
    public class TourList
    {
        public string Tourlistid { get; set; }
        public string Tourlistname { get; set; }
    }

}