﻿using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class BookingNote
{
    public long BookingNotesid { get; set; }

    public string? Booking { get; set; }

    public string? Voucher { get; set; }

    public string? Privatemsg { get; set; }

    public long? BookingId { get; set; }

    public Guid? ItineraryId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
