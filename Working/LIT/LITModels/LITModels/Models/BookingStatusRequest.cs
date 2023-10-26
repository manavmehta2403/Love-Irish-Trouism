using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class BookingStatusRequest
{
    public Guid Requestid { get; set; }

    public string? ReferenceNumber { get; set; }

    public string? Comments { get; set; }

    public int? BookingId { get; set; }

    public Guid? ItineraryId { get; set; }

    public string? ItineraryAutoId { get; set; }

    public Guid? BookingStatus { get; set; }

    public string? ResponseStatus { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? IsEmailSent { get; set; }

    public string? AgencyEmail { get; set; }

    public string? AgencyName { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public string? BookingidIdentifier { get; set; }

    public int? BookingAutoId { get; set; }
}
