using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierInformation
{
    public Guid SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public long? SupplierAutoId { get; set; }

    public string? Hosts { get; set; }

    public bool? IsSupplierActive { get; set; }

    public string? CustomCode { get; set; }

    public string? Street { get; set; }

    public Guid? City { get; set; }

    public Guid? Region { get; set; }

    public Guid? State { get; set; }

    public Guid? Country { get; set; }

    public string? Postcode { get; set; }

    public string? Phone { get; set; }

    public string? Mobile { get; set; }

    public string? FreePh { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public string? Website { get; set; }

    public string? PostalAddress { get; set; }

    public string? SupplierComments { get; set; }

    public string? SupplierDescription { get; set; }

    public string? SupplierFolderPath { get; set; }

    public long? SupplierfolderinfoId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
