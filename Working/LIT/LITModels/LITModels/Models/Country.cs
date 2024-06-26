﻿using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class Country
{
    public Guid CountryId { get; set; }

    public string? CountryName { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
