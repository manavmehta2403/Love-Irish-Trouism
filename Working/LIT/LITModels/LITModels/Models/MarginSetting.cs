using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class MarginSetting
{
    public Guid SettingsId { get; set; }

    public decimal? Overrideall { get; set; }

    public string? Adjustoption { get; set; }

    public string? Adjusttobe { get; set; }

    public string? Applyto { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
