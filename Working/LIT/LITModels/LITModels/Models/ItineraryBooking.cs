using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ItineraryBooking
{
    public Guid? ItineraryId { get; set; }

    public long Bkid { get; set; }

    public string? BookingName { get; set; }

    public long? BookingAutoId { get; set; }

    public Guid? CityId { get; set; }

    public string? City { get; set; }

    public string? Comments { get; set; }

    public string? Day { get; set; }

    public DateTime? EndDate { get; set; }

    public string? EndTime { get; set; }

    public string? ExchRate { get; set; }

    public decimal? GrossAdj { get; set; }

    public decimal? Grossfinal { get; set; }

    public decimal? Grosstotal { get; set; }

    public string? Grossunit { get; set; }

    public bool? Invoiced { get; set; }

    public string? ItemDescription { get; set; }

    public string? ItinCurrency { get; set; }

    public decimal? Netfinal { get; set; }

    public decimal? Nettotal { get; set; }

    public string? Netunit { get; set; }

    public decimal? NightsDays { get; set; }

    public DateTime? PaymentDueDate { get; set; }

    public string? Ref { get; set; }

    public Guid? RegionId { get; set; }

    public string? Region { get; set; }

    public string? ServiceName { get; set; }

    public DateTime? StartDate { get; set; }

    public string? StartTime { get; set; }

    public Guid? Status { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public decimal? Quantity { get; set; }

    public string? DaysValid { get; set; }

    public string? AgentCommission { get; set; }

    public string? AgentCommissionPercentage { get; set; }

    public string? BkgCurrencyName { get; set; }

    public Guid? SupplierId { get; set; }

    public Guid? ServiceTypeId { get; set; }

    public Guid? ServiceId { get; set; }

    public Guid? BkgCurrencyId { get; set; }

    public Guid? PricingOptionId { get; set; }

    public Guid? PricingRateId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public string? MarkupPercentage { get; set; }

    public string? CommissionPercentage { get; set; }

    public string? PaymentTerms { get; set; }

    public Guid? ItinCurrencyId { get; set; }

    public Guid? ChangeCurrencyId { get; set; }

    public bool NewNetUnitNotinSupptbl { get; set; }

    public Guid? BookingidIdentifier { get; set; }

    public Guid? PickupLocation { get; set; }

    public Guid? DropLocation { get; set; }
}
