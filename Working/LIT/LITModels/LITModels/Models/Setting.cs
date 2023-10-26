using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class Setting
{
    public long SettingsId { get; set; }

    public string? FieldName { get; set; }

    public string? FieldValue { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public bool? IsDeleted { get; set; }
}
