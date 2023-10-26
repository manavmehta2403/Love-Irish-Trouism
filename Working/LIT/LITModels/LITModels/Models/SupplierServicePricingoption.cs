using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierServicePricingoption
{
    public Guid PricingOptionId { get; set; }

    public string? PricingOptionName { get; set; }

    public decimal? NetPrice { get; set; }

    public decimal? MarkupPercentage { get; set; }

    public decimal? GrossPrice { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public Guid? SupplierServiceDetailsRateId { get; set; }

    public Guid? SupplierServiceId { get; set; }

    public Guid? Type { get; set; }

    public bool? IsDefault { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
