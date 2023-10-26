using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class EmailSetting
{
    public string? Smtpserver { get; set; }

    public int? Smtpport { get; set; }

    public bool? IsSslrequired { get; set; }

    public string? FromEmailName { get; set; }

    public string? FromEmail { get; set; }

    public string? FromEmailPassword { get; set; }

    public string? Bccemail { get; set; }
}
