using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class FollowupTask
{
    public Guid Taskid { get; set; }

    public string? TaskName { get; set; }

    public string? Notes { get; set; }

    public DateTime? DateDue { get; set; }

    public Guid? Assignedto { get; set; }

    public DateTime? Datecompleted { get; set; }

    public long? Bookingid { get; set; }

    public Guid? Itineraryid { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }

    public DateTime? DateCreated { get; set; }
}
