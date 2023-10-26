using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierAutoNumber
{
    public long Pkid { get; set; }

    public long? AutoNoStart { get; set; }

    public long? LastAutoNo { get; set; }
}
