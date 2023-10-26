using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ErrorLog
{
    public long LogId { get; set; }

    public string? PageName { get; set; }

    public string? FunctionName { get; set; }

    public string? MessageDescription { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? ErrorFrom { get; set; }
}
