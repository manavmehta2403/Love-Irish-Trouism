using LIT.Core.Mvvm;
using LIT.Modules.TabControl.ViewModels;
using LIT.Old_LIT;
using LIT.ViewModels;
using Prism.Commands;
using SQLDataAccessLayer.DAL;
using SQLDataAccessLayer.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace LIT.Commands
{
    public class supplierPaymentCommand : CrudOperations<supplierPaymentCommand>
    {
        private readonly supplierPaymentViewModel _viewModel;
        private readonly SupplierPaymentdal _supplierPaymentdal;

        public DelegateCommand AddCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand RetrieveCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public supplierPaymentCommand(supplierPaymentViewModel viewModel)
            : base("Add", "Delete", "Retrieve", "Save")
        {
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _supplierPaymentdal = new SupplierPaymentdal();

           // _viewModel.PropertyChanged += ViewModel_PropertyChanged;

            AddCommand = new DelegateCommand(ExecuteAdd);
            DeleteCommand = new DelegateCommand(ExecuteDelete);
            RetrieveCommand = new DelegateCommand(ExecuteRetrieve);
            SaveCommand = new DelegateCommand(ExecuteSave);
        }

        public new event EventHandler CanExecuteChanged;


        protected override void ExecuteDelete()
        {
            SupplierPayments deleteRecord = _viewModel.SelectedPayment;
            _viewModel.PaymentRecord.Remove(deleteRecord);
            _supplierPaymentdal.DeleteSupplierPaymentsDetails(deleteRecord.SupplierPaymentId, _viewModel.SelectedInvoice.ItineraryID);
        }

        protected override void ExecuteSave()
        {
            if (_viewModel.PaymentRecord.Count > 0)
            {
                foreach (var item in _viewModel.PaymentRecord)
                {
                  _supplierPaymentdal.SaveUpdateSupplierPaymentDetails("I", item);
                    _viewModel.PaidAmount += item.InvoiceAmount;
                }
            }
        }
        
        protected override void ExecuteAdd()
        {
            decimal paymentAmount = _viewModel.PaymentRecord.Count == 0 ? _viewModel.SelectedInvoice.Grossfinal : _viewModel.PaymentRecord.Last().PaymentAmount - _viewModel.PaymentRecord.Last().InvoiceAmount;
            SupplierPayments supplierPayment = new SupplierPayments();
            supplierPayment.SupplierPaymentId = Guid.NewGuid().ToString();
            supplierPayment.BookingId = _viewModel.SelectedInvoice.BookingID;
            supplierPayment.ItineraryId = _viewModel.SelectedInvoice.ItineraryID;
            supplierPayment.SupplierId = _viewModel.SelectedInvoice.SupplierID;
            supplierPayment.BookingIDIdentifier = _viewModel.SelectedInvoice.BookingidIdentifier;
            supplierPayment.InvoiceId = 100;
            supplierPayment.InvoiceNumber = _viewModel.PaymentRecord.Count.ToString();
            supplierPayment.InvoiceDate = _viewModel.SelectedInvoice.StartDate;

            supplierPayment.InvoiceAmount = .00m;
            supplierPayment.PaymentAmount = paymentAmount;

            supplierPayment.InvoiceDueDate = _viewModel.SelectedInvoice.PaymentDueDate;
            supplierPayment.PaymentType = "Credit Card";
            supplierPayment.ExchangeRate = decimal.Parse(_viewModel.SelectedInvoice.ExchRate);
            supplierPayment.CurrencyCode = _viewModel.SelectedInvoice.ItinCurrency; 
            supplierPayment.CurrencyExchangeRate = decimal.Parse(_viewModel.SelectedInvoice.ExchRate);
            supplierPayment.ConvertedAmount = decimal.Parse(_viewModel.SelectedInvoice.ExchRate) * paymentAmount;
            supplierPayment.PaymentDate = DateTime.Now;
            supplierPayment.TotalOutstanding = 0.0m;
            supplierPayment.Notes = string.Empty;
            supplierPayment.CreatedBy = _viewModel.SelectedInvoice.ItineraryID;
            supplierPayment.ModifiedBy = _viewModel.SelectedInvoice.ItineraryID;
            supplierPayment.IsDeleted = false;
            supplierPayment.DeletedBy = _viewModel.SelectedInvoice.ItineraryID;
            _viewModel.PaymentRecord.Add(supplierPayment);
        }

        protected override void ExecuteRetrieve()
        {
          _viewModel.PaymentRecord.AddRange(_supplierPaymentdal.RetriveSupplierPaymentsDetails(Guid.Parse(_viewModel.SelectedInvoice.ItineraryID), _viewModel.SelectedInvoice.BookingID,Guid.Parse(_viewModel.SelectedInvoice.BookingidIdentifier)));
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
}

