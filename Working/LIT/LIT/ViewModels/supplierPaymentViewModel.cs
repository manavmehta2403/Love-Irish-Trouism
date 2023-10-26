using LIT.Commands;
using LIT.Modules.TabControl.Commands;
using LIT.Old_LIT;
using Prism.Mvvm;
using SQLDataAccessLayer.Models;
using System.Collections.ObjectModel;

namespace LIT.ViewModels
{
    public class supplierPaymentViewModel : BindableBase
    {
        #region field

        private readonly supplierPaymentCommand _supplierPaymentCommands;

        private ObservableCollection<SupplierPayments> _PaymentRecord;

        private ObservableCollection<BookingItems> _invoiceCollection;

        private BookingItems _selectedInvoice;

        private SupplierPayments _selectedBookingModel;

        private decimal _paidAmount;

        private int _tabIndex;

        private string _loginId;

        #endregion

        #region ctor

        public supplierPaymentViewModel(BookingItems selectedBookingItem)
        {
            _supplierPaymentCommands = new supplierPaymentCommand(this);
            _PaymentRecord = new ObservableCollection<SupplierPayments>();
            _invoiceCollection = new ObservableCollection<BookingItems>();
            if (selectedBookingItem != null)
            {
                this._invoiceCollection.Add(selectedBookingItem);
                this._selectedInvoice = selectedBookingItem;
            }
            _paidAmount = 0.00m;
            _loginId = string.Empty;
        }
        #endregion

        #region public prop

        public supplierPaymentCommand CommentsTabCommands => _supplierPaymentCommands;

        public ObservableCollection<SupplierPayments> PaymentRecord
        {
            get { return _PaymentRecord; }
            set
            {
                SetProperty(ref _PaymentRecord, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }
        } 

        public ObservableCollection<BookingItems> InvoiceCollection
        {
            get { return _invoiceCollection; }
            set
            {
                SetProperty(ref _invoiceCollection, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }
        }

        public string LoginId
        {
            get { return _loginId; }
            set
            {
                SetProperty(ref _loginId, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }

        }


        public SupplierPayments SelectedPayment
        {
            get { return _selectedBookingModel; }
            set
            {
                SetProperty(ref _selectedBookingModel, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }
        }
        
        public BookingItems SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                SetProperty(ref _selectedInvoice, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }
        }

        public decimal PaidAmount
        {
            get { return _paidAmount;  }
            set
            {
                SetProperty(ref _paidAmount, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
            }
        }
        public int TabIndex
        {
            get { return _tabIndex;  }
            set
            {
                SetProperty(ref _tabIndex, value);
                _supplierPaymentCommands.RaiseCanExecuteChanged();
                if(_tabIndex == 1)
                {
                    this.PaymentRecord.Clear();
                    _supplierPaymentCommands.RetrieveCommand.Execute();
                }
            }
        }

        #endregion

        #region priavte method
        #endregion

    }
}

