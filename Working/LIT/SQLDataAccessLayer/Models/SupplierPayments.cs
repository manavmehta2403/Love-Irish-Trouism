using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class SupplierPayments
    {
        public string  SupplierPaymentId { get; set; }
        public long  BookingId { get; set; }
        public string  ItineraryId { get; set; }
        public string  SupplierId { get; set; }
        public long  InvoiceId { get; set; }
        public string  InvoiceNumber { get; set; }
        public DateTime?  InvoiceDate { get; set; }
        public decimal  InvoiceAmount { get; set; }
        public DateTime? InvoiceDueDate { get; set; }
        public string  PaymentType { get; set; }
        public decimal  ExchangeRate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string  CurrencyCode { get; set; }
        public decimal  CurrencyExchangeRate { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime?  PaymentDate { get; set; }
        public decimal  TotalOutstanding { get; set; }
        public string  Notes { get; set; }
        public string  CreatedBy { get; set; }
        public string  ModifiedBy { get; set; }
        public bool  IsDeleted { get; set; }
        public string  DeletedBy { get; set; }

        public string BookingIDIdentifier { get;set; }
    }
}
