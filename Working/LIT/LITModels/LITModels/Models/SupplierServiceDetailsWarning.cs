﻿using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierServiceDetailsWarning
{
    public DateTime? ValidFrom { get; set; }

    public DateTime? ValidTo { get; set; }

    public Guid SupplierServiceDetailsWarningId { get; set; }

    public string? Description { get; set; }

    public string? Messagefor { get; set; }

    public Guid? SupplierServiceId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsExpired { get; set; }
}