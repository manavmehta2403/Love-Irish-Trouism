using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class DropDownListValueDetail
{
    public Guid? DropDownListValueDetailsId { get; set; }

    public Guid? DropdownlistHeaderId { get; set; }

    public Guid? ValueField { get; set; }

    public string? TextField { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
