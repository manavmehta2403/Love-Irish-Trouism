using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ItineraryDetail
{
    public Guid ItineraryId { get; set; }

    public string? ItineraryName { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? ItineraryStartDate { get; set; }

    public DateTime? ItineraryEndDate { get; set; }

    public Guid? ArrivalCity { get; set; }

    public string? ArrivalFlight { get; set; }

    public Guid? DepartureCity { get; set; }

    public string? DepartureFlight { get; set; }

    public Guid? Agent { get; set; }

    public Guid? AgentAssignedTo { get; set; }

    public string? Enteredby { get; set; }

    public Guid? Status { get; set; }

    public Guid? Source { get; set; }

    public Guid? Customerid { get; set; }

    public Guid? Bookingid { get; set; }

    public Guid? Supplierid { get; set; }

    public Guid? Clientsid { get; set; }

    public string? ItineraryFolderPath { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public long? ItineraryAutoId { get; set; }

    public string? InclusionNotes { get; set; }
}
