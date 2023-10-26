using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SQLDataAccessLayer.Models
{
    public class SupplierModels
    {
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public long SupplierAutoId { get; set; }
        public string Hosts { get; set; }
        public bool IsSupplierActive { get; set; }
        public string CustomCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string FreePh { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string PostalAddress { get; set; }
        public string SupplierComments { get; set; }
        public string SupplierDescription { get; set; }
        public string SupplierFolderPath { get; set; }
        public string SupplierfolderinfoId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        public int SupplierPaymentTermsindays { get; set; }
        public decimal SupplierPaymentDepositAmount { get; set; }

    }


    
        public class SupplierServiceModels :INotifyPropertyChanged
    {
        
        public string SupplierId { get; set; }

        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
       // public string Type { get; set; }        
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        public string Currency { get; set; }
        public string Groupinfo { get; set; }
        public bool IsActive { get; set; }

        public string Type
        {
            get; set;
            
        }

        /*  private string _Type;
          public SupplierServiceType SuppServicetype { get; set; }
      */

        private int _selectedItemRow;
        public int SelectedItemRow
        {
            get { return _selectedItemRow; }
            set
            {
                _selectedItemRow = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemRow"));
            }
        }
        private SupplierServiceModels _selectedItemRowvalue;
        public SupplierServiceModels SelectedItemRowvalue
        {
            get { return _selectedItemRowvalue; }
            set
            {
                _selectedItemRowvalue = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemRowvalue"));
            }
        }
        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }


        private object _SelectedItemCurrency;
        public object SelectedItemCurrency
        {
            get { return _SelectedItemCurrency; }
            set
            {
                _SelectedItemCurrency = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemCurrency"));
            }
        }


        private object _SelectedItemGroupInfo;
        public object SelectedItemGroupInfo
        {
            get { return _SelectedItemGroupInfo; }
            set
            {
                _SelectedItemGroupInfo = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemGroupInfo"));
            }
        }
        /*
        private ObservableCollection<SupplierServiceModels> _selectedServiceType;

        public ObservableCollection<SupplierServiceModels> SelectedServiceType
        {
            get { return _selectedServiceType; }
            set
            {
                _selectedServiceType = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_selectedServiceType)));
            }
        }*/
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
              //  handler(this, new PropertyChangedEventArgs(e));
              this.PropertyChanged(this,e);
            }
        }
        
    }


    public class SupplierServiceRatesDt 
    {
        public string SupplierServiceId { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        //public string StrValidFrom { get; set; }
        //public string StrValidTo { get; set; }
        public string SupplierServiceDetailsRateId { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }

        // public string Type { get; set; }        
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

    }

    public class SupplierServiceWarning
    {
        public string SupplierServiceId { get; set; }
        public DateTime ValidFromwarning { get; set; }
        public DateTime ValidTowarning { get; set; }
        public string SupplierServiceDetailsWarningID { get; set; }
        public bool IsActive { get; set; }
        public bool IsExpired { get; set; }
        public string WarDescription { get; set; }
        public string Messagefor { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

    }


    public class SupplierPricingOption:INotifyPropertyChanged
    {
        public string SupplierServiceId { get; set; }
        public string SupplierServiceDetailsRateId { get; set; }

        public string PricingOptionId { get; set; }

        private string _PricingOptionName;
        public string PricingOptionName
        {
            get { return _PricingOptionName; }
            set
            {
                _PricingOptionName = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("PricingOptionName"));
            }
        }
public decimal NetPrice { get; set; }
        public decimal MarkupPercentage { get; set; }
        public decimal GrossPrice { get; set; }
        public decimal CommissionPercentage { get; set; }

        public string PriceType { get; set; }
        public bool PriceIsDefault { get; set; }

        public bool PriceIsActive { get; set; }        
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        private object _PriceTypeSelectedItem;
        public object PriceTypeSelectedItem
        {
            get { return _PriceTypeSelectedItem; }
            set
            {
                _PriceTypeSelectedItem = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("PriceTypeSelectedItem"));
            }
        }

        private object _selectedforbkg;
        public object selectedforbkg
        {
            get { return _selectedforbkg; }
            set
            {
                _selectedforbkg = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("selectedforbkg"));
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

        public string CurrencyDisplayFormat { get; set; }
        public string CurrencyName { get; set; }
    }





    public class SupplierPriceRateEdit : INotifyPropertyChanged
    {
        public string PriceEditRateId { get; set; }
        public string PricingOptionId { get; set; }


        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged1 != null)
        //        PropertyChanged1(this, new PropertyChangedEventArgs(propertyName));
        //}
        private decimal _NetPrice;
        public decimal NetPrice { get { return _NetPrice; } set { _NetPrice = value; this.OnPropertyChanged(new PropertyChangedEventArgs("")); this.OnPropertyChanged(new PropertyChangedEventArgs("GrossPrice")); } }
        
        private decimal _MarkupPercentage;
        public decimal MarkupPercentage { get { return _MarkupPercentagecal; } set { _MarkupPercentage = value; this.OnPropertyChanged(new PropertyChangedEventArgs("")); this.OnPropertyChanged(new PropertyChangedEventArgs("MarkupPercentage")); } }

        private decimal _MarkupPercentagecal { get { return CalculateMarkuppercentage(GrossPrice); } }
        public decimal GrossPricecal { get { return calculategross(); } }

        private decimal _GrossPrice;
        public decimal GrossPrice { get { return GrossPricecal; } set { _GrossPrice = value; this.OnPropertyChanged(new PropertyChangedEventArgs("")); this.OnPropertyChanged(new PropertyChangedEventArgs("GrossPrice")); } }

        
        public decimal CommissionPercentage { get; set; }

        public string SupplierServiceId { get; set; }

        private bool _Monday;
        private bool _Tuesday;
        private bool _Wednesday;
        private bool _Thursday;
        private bool _Friday;
        private bool _Saturday;
        private bool _Sunday;


        public bool Monday { get { return _Monday; } set { _Monday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Monday")); } }
        public bool Tuesday { get { return _Tuesday; } set { _Tuesday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Tuesday")); } }
        public bool Wednesday { get { return _Wednesday; } set { _Wednesday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Wednesday")); } }
        public bool Thursday { get { return _Thursday; } set { _Thursday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Thursday")); } }
        public bool Friday { get { return _Friday; } set { _Friday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Friday")); } }
        public bool Saturday { get { return _Saturday; } set { _Saturday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Saturday")); } }
        public bool Sunday { get { return _Sunday; } set { _Sunday = value; this.OnPropertyChanged(new PropertyChangedEventArgs("Sunday")); } }


        public bool PriceEditIsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        private int _ChooseEditOption;
        public int ChooseEditOption { get { return _ChooseEditOption; } set { _ChooseEditOption = value; this.OnPropertyChanged(new PropertyChangedEventArgs("")); this.OnPropertyChanged(new PropertyChangedEventArgs("ChooseEditOption")); } }
        private int _Rounding;
        public int Rounding { get { return _Rounding; } set { _Rounding = value; this.OnPropertyChanged(new PropertyChangedEventArgs("")); this.OnPropertyChanged(new PropertyChangedEventArgs("Rounding")); } }

        public string CurrencyDisplayFormat { get; set; }
        public string CurrencyName { get; set; }
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
        private decimal calculategross()
        {
            decimal grossprice = 0;
            decimal quo = 0, addval = 0;
            grossprice = (NetPrice + ((NetPrice * _MarkupPercentage) / 100));

            if (Rounding == 10)
            {
                quo = (grossprice % 10);
                if (quo > 0)
                    addval = 10 - quo;
            }
            if (Rounding == 5)
            {
                quo = (grossprice % 5);
                if (quo > 0)
                    addval = 5 - quo;
            }
            if (Rounding == 1)
            {
                quo = (grossprice % 1);
                if (quo > 0)
                    addval = 1 - quo;
            }
            if (Rounding == null || Rounding == 0)
            {
                addval = 0;
            }

            grossprice = grossprice + addval;
          //  grossprice = decimal.Round(grossprice, 2, MidpointRounding.AwayFromZero);
            CalculateMarkuppercentage(grossprice);
            
            return grossprice;
        }
        private decimal CalculateMarkuppercentage(decimal grossprice)
        {
           // decimal grossprice = (NetPrice + ((NetPrice * _MarkupPercentage) / 100));
            decimal newmarkuppercentage = 0;
            if (grossprice > 0)
            {
                newmarkuppercentage = ((grossprice - NetPrice) * 100) / NetPrice;
            }
            if(grossprice==0 && NetPrice==0 && _MarkupPercentage>0)
            {
                newmarkuppercentage = _MarkupPercentage;
            }

           // newmarkuppercentage = decimal.Round(newmarkuppercentage, 2, MidpointRounding.AwayFromZero);
            return newmarkuppercentage;
        }
    }

    public class SupplierServiceType
    {
        public string ServiceTypeID { get; set; }

        public string ServiceTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDefault { get; set; }

        public SupplierServiceType Defaultselectedtype { get; set; }
    }


    public class SupplierCommunicationNotes 
    {
        public string Notesinfo { get; set; }
        public string SupplierId { get; set; }
        public string NotesinfoID { get; set; }
        public bool Autoselected { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

    }

    public class SupplierCommunicationContentdata : INotifyPropertyChanged
    {
        public string ContentID { get; set; }


        private string _contentname;
        public string ContentName
        {

            get { return _contentname; }
            set
            {
                _contentname = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("contentname"));
            }
        }
        public string ContentFor { get; set; }
        public string ContentType { get; set; }
        public string SupplierID { get; set; }

        public string ServiceID { get; set; }

        public string Heading { get; set; }
        public string ReportImage { get; set; }
        public string OnlineImage { get; set; }
        public string BodyHtml { get; set; }
        public bool IsActivated { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

        public bool IsselectedContent { get; set; }

        public bool IsLastadded { get; set; }

        private object _selectedItemcontentype;
        public object SelectedItemcontentype
        {
            get { return _selectedItemcontentype; }
            set
            {
                _selectedItemcontentype = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemcontentype"));
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

    public class supplierservicemenulist
    {
        public String ID { get; set; }
        public string SupplierServiceID { get; set; }
        public string ServiceName { get; set; }
    }


}
