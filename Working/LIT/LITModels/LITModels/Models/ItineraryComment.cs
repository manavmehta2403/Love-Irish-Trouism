using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ItineraryComment
{
    public Guid Commentsid { get; set; }

    public long? BookingId { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierRefNo { get; set; }

    public string? Comments { get; set; }

    public Guid? Itineraryid { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? CommentedOn { get; set; }

    public Guid? CommentedBy { get; set; }
}
