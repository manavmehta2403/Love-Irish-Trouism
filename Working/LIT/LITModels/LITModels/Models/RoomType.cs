using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class RoomType
{
    public Guid Roomtypesid { get; set; }

    public Guid? OptionTypeRoomid { get; set; }

    public int? RmsBkd { get; set; }

    public int? PaxBkd { get; set; }

    public int? RmsSold { get; set; }

    public int? PaxSold { get; set; }

    public decimal? SellPrice { get; set; }

    public Guid? Itineraryid { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
