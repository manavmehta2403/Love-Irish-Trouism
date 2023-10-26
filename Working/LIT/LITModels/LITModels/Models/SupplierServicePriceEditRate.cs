using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierServicePriceEditRate
{
    public Guid PriceEditRateId { get; set; }

    public Guid? PricingOptionId { get; set; }

    public int? ChooseEditOption { get; set; }

    public decimal? NetPrice { get; set; }

    public decimal? MarkupPercentage { get; set; }

    public decimal? GrossPrice { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public Guid? SupplierServiceId { get; set; }

    public bool? Monday { get; set; }

    public bool? Tuesday { get; set; }

    public bool? Wednesday { get; set; }

    public bool? Thursday { get; set; }

    public bool? Friday { get; set; }

    public bool? Saturday { get; set; }

    public bool? Sunday { get; set; }

    public int? RatevalidFromDay { get; set; }

    public int? RatevalidToDay { get; set; }

    public int? Rounding { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
