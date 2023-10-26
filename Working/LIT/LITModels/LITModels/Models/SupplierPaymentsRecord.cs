using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierPaymentsRecord
{
    public Guid SupplierPaymentId { get; set; }

    public long? BookingId { get; set; }

    public Guid? ItineraryId { get; set; }

    public Guid? SupplierId { get; set; }

    public long? InvoiceId { get; set; }

    public string? InvoiceNumber { get; set; }

    public DateTime? InvoiceDate { get; set; }

    public decimal? InvoiceAmount { get; set; }

    public string? PaymentType { get; set; }

    public decimal? ExchangeRate { get; set; }

    public decimal? PaymentAmount { get; set; }

    public string? CurrencyCode { get; set; }

    public decimal? CurrencyExchangeRate { get; set; }

    public decimal? ConvertedAmount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public decimal? TotalOutstanding { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? InvoiceDueDate { get; set; }

    public Guid? BookingIdidentifier { get; set; }
}
