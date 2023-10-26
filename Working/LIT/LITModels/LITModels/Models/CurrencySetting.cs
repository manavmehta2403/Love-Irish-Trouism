using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class CurrencySetting
{
    public long CurrencySettings { get; set; }

    public Guid? FromCurrencyId { get; set; }

    public decimal? FromCurrencyValue { get; set; }

    public Guid? ToCurrencyId { get; set; }

    public decimal? ToCurrencyValue { get; set; }

    public DateTime? CurrencyFromDate { get; set; }

    public DateTime? CurrencyToDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
