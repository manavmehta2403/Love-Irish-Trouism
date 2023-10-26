using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Linq;

namespace SQLDataAccessLayer.Models
{
    public class BookingModel
    {

       
    }
    public class BookingSupplierServiceModels
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }

        public string ServiceTypeID { get; set; }

        public string ServiceID { get; set; }
        public string ServiceName { get; set; }

        public string CityID { get; set; }
        public string CityName { get; set; }

        public string RegionID { get; set; }
        public string RegionName { get; set; }

    }


    public class SelectedSupplierBooking: INotifyPropertyChanged
    {
        public string SupplierID { get; set; }

        public string ServiceTypeID { get; set; }

        public string ServiceID { get; set; }
        public int BookingID { get; set; }

        public string BookingName { get; set; }

        public string ItemsDescription { get; set; }

        public DateTime? BookingStartDate { get; set; }

        private TimeSpan _BookingStartTime;
        public TimeSpan BookingStartTime { get { return _BookingStartTime; } set {
                _BookingStartTime = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("BookingStartTime"));
            } }

       // public string BookingStartTimestr => string.Format("", BookingStartTime);
       


        public string NightDays { get; set; }

        public string Quantity { get; set; }

        public string PricingOptionId { get; set; }

        private object _selectedforbkgitem;
        public object selectedforbkgitem
        {
            get { return _selectedforbkgitem; }
            set
            {
                _selectedforbkgitem = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("selectedforbkgitem"));
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


    public class BookingItems : INotifyPropertyChanged
    {
        public string ItineraryID { get; set; }
        public long BookingAutoID { get; set; }
        public string SupplierID { get; set; }
        public string ServiceTypeID { get; set; }
        public string ServiceID { get; set; }
        public long BookingID { get; set; }
        public string AgentCommission { get; set; }
        public string AgentCommissionPercentage { get; set; }
        public string BkgCurrencyID { get; set; }
        public string BkgCurrencyName { get; set; }
        public string BookingName { get; set; }
        public string City { get; set; }
        public string CityID { get; set; }
        public string Comments { get; set; }
        public string Day { get; set; }
        public DateTime Enddate { get; set; }
        public string EndTime { get; set; }
        public string ExchRate { get; set; }


        private decimal _GrossAdj;
        public decimal GrossAdj
        {
            get { return _GrossAdj; }
            set
            {
                _GrossAdj = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("GrossAdj"));
            }
        }

        private decimal _Grossfinal;
        public decimal Grossfinal
        {
            get { return _Grossfinal; }
            set
            {
                _Grossfinal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Grossfinal"));
            }
        }




        private decimal _BkgGrosstotal;
        public decimal BkgGrosstotal
        {
            get { return _BkgGrosstotal; }
            set
            {
                _BkgGrosstotal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("BkgGrosstotal"));
            }
        }

        private decimal _BkgGrossfinal;
        public decimal BkgGrossfinal
        {
            get { return _BkgGrossfinal; }
            set
            {
                _BkgGrossfinal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("BkgGrossfinal"));
            }
        }




        private decimal _Grosstotal;
        public decimal Grosstotal
        {
            get { return _Grosstotal; }
            set
            {
                _Grosstotal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Grosstotal"));
            }
        }



        private string _Grossunit;
        public string Grossunit
        {
            get { return _Grossunit; }
            set
            {
                _Grossunit = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Grossunit"));
            }
        }



        private bool _Invoiced;
        public bool Invoiced
        {
            get { return _Invoiced; }
            set
            {
                _Invoiced = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Invoiced"));
            }
        }
        public string ItemDescription { get; set; }
        public string ItinCurrency { get; set; }

        private decimal _Netfinal;
        public decimal Netfinal
        {
            get { return _Netfinal; }
            set
            {
                _Netfinal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Netfinal"));
            }
        }

        private decimal _Nettotal;
        public decimal Nettotal
        {
            get { return _Nettotal; }
            set
            {
                _Nettotal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Nettotal"));
            }
        }

        private decimal _BkgNetfinal;
        public decimal BkgNetfinal
        {
            get { return _BkgNetfinal; }
            set
            {
                _BkgNetfinal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("BkgNetfinal"));
            }
        }

        private decimal _BkgNettotal;
        public decimal BkgNettotal
        {
            get { return _BkgNettotal; }
            set
            {
                _BkgNettotal = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("BkgNettotal"));
            }
        }

        private string _Netunit;
        public string Netunit
        {
            get { return _Netunit; }
            set
            {
                _Netunit = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Netunit"));
            }
        }


        private decimal _NtsDays;
        public decimal NtsDays
        {
            get { return _NtsDays; }
            set
            {
                _NtsDays = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("NtsDays"));
            }
        }
        public DateTime PaymentDueDate { get; set; }


        private decimal _Qty;
        public decimal Qty
        {
            get { return _Qty; }
            set
            {
                _Qty = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Qty"));
            }
        }
        public string Ref { get; set; }
        public string Region { get; set; }
        public string RegionID { get; set; }
        public string ServiceName { get; set; }
        //private DateTime _StartDate;

        //public DateTime StartDate
        //{
        //    get { return _StartDate; }
        //    set
        //    {
        //        _StartDate = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
        //        this.OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
        //    }
        //}
        private DateTime _StartDate;
        public DateTime StartDate
        {
            get { return _StartDate; }
            set
            {
                _StartDate = value;
                //this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                //this.OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }
        public string StartTime { get; set; }
        public DateTime StartTimedt { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }

        public string PricingOptionId { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public string PricingRateID { get; set; }
        public string DaysValid { get; set; }


        public string CustomCode { get; set; }
        public string MarkupPercentage { get; set; }
        public string CommissionPercentage { get; set; }
        public string PaymentTerms { get; set; }

        private object _SelectedItemRequstStatus;
        public object SelectedItemRequstStatus
        {
            get { return _SelectedItemRequstStatus; }
            set
            {
                _SelectedItemRequstStatus = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemRequstStatus"));
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

        public string ItinCurDisplayFormat { get; set; }
        public string BkgCurDisplayFormat { get; set; }

        public string ItinCurrencyID { get; set; }
        public string ChangeCurrencyFormat { get; set; }

        public string ChangeCurrencyID { get; set; }

        public long BookingNotesid { get; set; }
        public string? BookingNote { get; set; }
        public string? VoucherNote { get; set; }
        public string? Privatemsg { get; set; }


        public DateTime OldStartDate { get; set; }
        public DateTime NewStartDate { get; set; }
        private string _NewNetunit;
        public string NewNetunit
        {
            get { return _NewNetunit; }
            set
            {
                _NewNetunit = value;
            }
        }
        private string _NewGrossunit;
        public string NewGrossunit
        {
            get { return _NewGrossunit; }
            set
            {
                _NewGrossunit = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("NewGrossunit"));
            }
        }
        private string _NewMarkupPercentage;
        public string NewMarkupPercentage
        {
            get { return _NewMarkupPercentage; }
            set
            {
                _NewMarkupPercentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("NewMarkupPercentage"));
            }
        }
        private string _OldNetunit;
        public string OldNetunit
        {
            get { return _OldNetunit; }
            set
            { _OldNetunit = value; }
        }

        private decimal _OldNewDatediff;
        public decimal OldNewDatediff
        {
            get { return _OldNewDatediff; }
            set
            { _OldNewDatediff = value; }
        }


        private string _SupplierWarning;
        public string SupplierWarning
        {
            get { return _SupplierWarning; }
            set
            {
                _SupplierWarning = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SupplierWarning"));
            }
        }

        private string _ServiceWarning;
        public string ServiceWarning
        {
            get { return _ServiceWarning; }
            set
            {
                _ServiceWarning = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("ServiceWarning"));
            }
        }
        public ImageSource Warningmsgurl { get; set; }

        public ImageSource Resultmsgurl { get; set; }



        private string _Resultmsg;
        public string Resultmsg
        {
            get { return _Resultmsg; }
            set
            {
                _Resultmsg = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("Resultmsg"));
            }
        }

        private bool _SelectedIdforRefresh;
        public bool SelectedIdforRefresh
        {
            get { return _SelectedIdforRefresh; }
            set
            {
                _SelectedIdforRefresh = value; this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedIdforRefresh"));
            }
        }

        private bool _isRefreshed;
        public bool IsRefreshed { get; set; }

        private bool _NewNetUnitNotinSupptbl;
        public bool NewNetUnitNotinSupptbl
        {
            get { return _NewNetUnitNotinSupptbl; }
            set
            {
                _NewNetUnitNotinSupptbl = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("NewNetUnitNotinSupptbl"));
            }
        }

        private bool _RefreshRateEditedflag;
        public bool RefreshRateEditedflag
        {
            get { return _RefreshRateEditedflag; }
            set
            {
                _RefreshRateEditedflag = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("RefreshRateEditedflag"));
            }
        }

        private bool _sterlingcolumnvisible;
        public bool sterlingcolumnvisible
        {
            get { return _sterlingcolumnvisible; }
            set
            {
                _sterlingcolumnvisible = value;
            }
        }

        public string BookingidIdentifier { get; set; }

        private object _SelectedItemPickuplocation;
        public object SelectedItemPickuplocation
        {
            get { return _SelectedItemPickuplocation; }
            set
            {
                _SelectedItemPickuplocation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemPickuplocation"));
            }
        }

        private object _SelectedItemDroplocation;
        public object SelectedItemDroplocation
        {
            get { return _SelectedItemDroplocation; }
            set
            {
                _SelectedItemDroplocation = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItemDroplocation"));
            }
        }

        public string Pickuplocation { get; set; }
        public string Droplocation { get; set; }

        //,@SupplierPaymentTermsindays,@SupplierPaymentDepositAmount,@SupplierPaymentTermsOverrideindays,@SupplierPaymentOverrideDepositAmount
        private decimal _SupplierPaymentDepositAmount;
        private decimal _SupplierPaymentOverrideDepositAmount;

        private int _SupplierPaymentTermsindays;
        private int _SupplierPaymentTermsOverrideindays;
        public decimal SupplierPaymentDepositAmount
        {
            get { return _SupplierPaymentDepositAmount; }
            set
            {
                _SupplierPaymentDepositAmount = value;
            }
        }

        public decimal SupplierPaymentOverrideDepositAmount
        {
            get { return _SupplierPaymentOverrideDepositAmount; }
            set
            {
                _SupplierPaymentOverrideDepositAmount = value;
            }
        }

        public int SupplierPaymentTermsindays
        {
            get { return _SupplierPaymentTermsindays; }
            set
            {
                _SupplierPaymentTermsindays = value;
            }
        }

        public int SupplierPaymentTermsOverrideindays
        {
            get { return _SupplierPaymentTermsOverrideindays; }
            set
            {
                _SupplierPaymentTermsOverrideindays = value;
            }
        }



        private decimal _MarginAdjustmentOverrideall;
        private decimal _MarginAdjustmentGross;
        private decimal _GrossAdjustmentMarkup;
        private decimal _GrossAdjustmentGross;
        private decimal _GrossAdjustmentFinalOverride;
        private decimal _FinalMargin;
        private decimal _FinalSell;
        private decimal _FinalAgentCommission;
        private decimal _SumofNetTotal;
        private decimal _SumofGrossTotal;
        private decimal _SumofNetFinal;
        private decimal _SumofGrossFinal;
        private decimal _SumofGrossAdjustment;


        public decimal MarginAdjustmentOverrideall
        {
            get { return _MarginAdjustmentOverrideall; }
            set
            {
                _MarginAdjustmentOverrideall = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("MarginAdjustmentOverrideall"));
            }
        }
        public decimal MarginAdjustmentGross
        {
            get { return _MarginAdjustmentGross; }
            set
            {
                _MarginAdjustmentGross = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("MarginAdjustmentGross"));
            }
        }
        public decimal GrossAdjustmentMarkup
        {
            get { return _GrossAdjustmentMarkup; }
            set
            {
                _GrossAdjustmentMarkup = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("GrossAdjustmentMarkup"));
            }
        }
        public decimal GrossAdjustmentGross
        {
            get { return _GrossAdjustmentGross; }
            set
            {
                _GrossAdjustmentGross = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("GrossAdjustmentGross"));
            }
        }

        public decimal GrossAdjustmentFinalOverride
        {
            get { return _GrossAdjustmentFinalOverride; }
            set
            {
                _GrossAdjustmentFinalOverride = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("GrossAdjustmentFinalOverride"));
            }
        }


        public decimal FinalMargin
        {
            get { return _FinalMargin; }
            set
            {
                _FinalMargin = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("_FinalMargin"));
            }
        }

        public decimal FinalSell
        {
            get { return _FinalSell; }
            set
            {
                _FinalSell = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("FinalSell"));
            }
        }

        public decimal FinalAgentCommission
        {
            get { return _FinalAgentCommission; }
            set
            {
                _FinalAgentCommission = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("FinalAgentCommission"));
            }
        }


       

        public decimal SumofNetTotal
        {
            get { return _SumofNetTotal; }
            set
            {
                _SumofNetTotal = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SumofNetTotal"));
            }
        }
        public decimal SumofGrossTotal
        {
            get { return _SumofGrossTotal; }
            set
            {
                _SumofGrossTotal = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SumofGrossTotal"));
            }
        }
        public decimal SumofNetFinal
        {
            get { return _SumofNetFinal; }
            set
            {
                _SumofNetFinal = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SumofNetFinal"));
            }
        }
        public decimal SumofGrossFinal
        {
            get { return _SumofGrossFinal; }
            set
            {
                _SumofGrossFinal = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SumofGrossFinal"));
            }
        }
        public decimal SumofGrossAdjustment
        {
            get { return _SumofGrossAdjustment; }
            set
            {
                _SumofGrossAdjustment = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("SumofGrossAdjustment"));
            }
        }

        public string ItineraryBookingTotalId { get; set; }

        private decimal _FinalMarginpercentage;
        public decimal FinalMarginpercentage
        {
            get { return _FinalMarginpercentage; }
            set
            {
                _FinalMarginpercentage = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs(""));
                this.OnPropertyChanged(new PropertyChangedEventArgs("FinalMarginpercentage"));
            }
        }

        //  public bool IsNewDateChanged { get; set; }
    }


    /* public class BookingItemRefreshrates
     {
        public ObservableCollection<BookingItems> bkitems { get; set; }


         public DateTime OldStartDate { get; set; }
         public DateTime NewStartDate { get; set; }
         private string _NewNetunit;
         public string NewNetunit
         {
             get { return _NewNetunit; }
             set
             {
                 _NewNetunit = value; 
             }
         }

         private string _OldNetunit;
         public string OldNetunit
         {
             get { return _OldNetunit; }
             set
             { _OldNetunit = value; }
         }



         public string Warning { get; set; }

         public string Resultmsg { get; set; }


     }

     */


    public class DateEqualsConverter : IMultiValueConverter
    {
        public object Convert(object[] values,
            Type targetType,
            object parameter,
            CultureInfo culture)
        {
            return System.Convert.ToDateTime(values[0])
                .Equals(System.Convert.ToDateTime(values[1]));
        }

        public object[] ConvertBack(object value,
            Type[] targetTypes,
            object parameter,
            CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
