using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierService
{
    public Guid ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public Guid? Type { get; set; }

    public Guid? SupplierId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public bool? IsActive { get; set; }

    public Guid? Currency { get; set; }

    public Guid? Groupinfo { get; set; }
}
