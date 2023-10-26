using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class PaymentDetail
{
    public Guid PaymentId { get; set; }

    public decimal? Amount { get; set; }

    public Guid? CurrencyCode { get; set; }

    public DateTime? DateofPayment { get; set; }

    public string? Details { get; set; }

    public decimal? ExchangeRate { get; set; }

    public decimal? Fee { get; set; }

    public decimal? FeePercent { get; set; }

    public Guid? FeeType { get; set; }

    public bool? Inclusive { get; set; }

    public decimal? PaymentAmount { get; set; }

    public Guid? PaymentTypeId { get; set; }

    public string? Personname { get; set; }

    public decimal? Sale { get; set; }

    public Guid? ItineraryId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public decimal? RefundPaymentTotalAmount { get; set; }

    public Guid? Passengerid { get; set; }
}
