using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class ItineraryBookingTotal
{
    public Guid ItineraryBookingTotalId { get; set; }

    public Guid? ItineraryId { get; set; }

    public decimal? NetTotal { get; set; }

    public decimal? GrossTotal { get; set; }

    public decimal? NetFinal { get; set; }

    public decimal? GrossFinal { get; set; }

    public decimal? GrossAdjustment { get; set; }

    public decimal? MarginAdjustmentOverrideall { get; set; }

    public string? MarginAdjustmentAdjustoption { get; set; }

    public string? MarginAdjustmentAdjusttobe { get; set; }

    public string? MarginAdjustmentApplyto { get; set; }

    public decimal? MarginAdjustmentGross { get; set; }

    public decimal? GrossAdjustmentMarkup { get; set; }

    public decimal? GrossAdjustmentGross { get; set; }

    public decimal? GrossAdjustmentFinalOverride { get; set; }

    public decimal? FinalMargin { get; set; }

    public decimal? FinalSell { get; set; }

    public decimal? FinalAgentCommission { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
