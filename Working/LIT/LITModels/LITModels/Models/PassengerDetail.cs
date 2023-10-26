using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class PassengerDetail
{
    public Guid Passengerid { get; set; }

    public int? Age { get; set; }

    public Guid? AgeGroup { get; set; }

    public Guid? Agent { get; set; }

    public decimal? AgentNet { get; set; }

    public decimal? CmmOvrd { get; set; }

    public decimal? CommissionPercentage { get; set; }

    public string? Comments { get; set; }

    public string? CompanyName { get; set; }

    public Guid? Country { get; set; }

    public decimal? DefaultPrice { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public Guid? PassengerStatus { get; set; }

    public Guid? PassengerType { get; set; }

    public Guid? Payee { get; set; }

    public bool? PayingPax { get; set; }

    public decimal? PriceOverride { get; set; }

    public string? Room { get; set; }

    public Guid? Rommtype { get; set; }

    public DateTime? Saledate { get; set; }

    public string? Title { get; set; }

    public Guid? ItineraryId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public long? Totalpassenger { get; set; }
}
