﻿using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class PassengerGroup
{
    public Guid Passengergroupid { get; set; }

    public string? Passengergroupname { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
