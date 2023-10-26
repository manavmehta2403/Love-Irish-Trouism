using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class Supplierfolderinfo
{
    public long Supplierfolderinfoid { get; set; }

    public int? Isparent { get; set; }

    public int? ParentId { get; set; }

    public string? Supplierfolderpath { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Createdon { get; set; }

    public Guid? Createdby { get; set; }
}
