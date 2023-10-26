using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class Currencydetail
{
    public Guid CurrencyId { get; set; }

    public string? CurrencyName { get; set; }

    public string? CurrencyCode { get; set; }

    public bool? Isenable { get; set; }

    public string? DisplayFormat { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public string? CurrencyCulture { get; set; }
}
