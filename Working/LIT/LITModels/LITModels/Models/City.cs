using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class City
{
    public Guid CityId { get; set; }

    public string? CityName { get; set; }

    public Guid? RegionId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
