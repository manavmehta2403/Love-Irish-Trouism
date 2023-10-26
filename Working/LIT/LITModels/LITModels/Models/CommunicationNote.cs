﻿using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class CommunicationNote
{
    public Guid NoteId { get; set; }

    public Guid? SupplierId { get; set; }

    public string? Notes { get; set; }

    public bool? IsAutoSelected { get; set; }

    public bool? IsActivated { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
