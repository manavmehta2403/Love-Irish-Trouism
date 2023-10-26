using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLDataAccessLayer.Models
{
    public class Notification
    {
        public Guid BubbleNotificationId { get; set; } // Primary Key (PK), uniqueidentifier
        public int NotificationType { get; set; } // int, not null
        public string? NotificationTitle { get; set; } // varchar(500), null
        public string? Comments { get; set; } // varchar(max)
        public Guid TargetUserId { get; set; } // uniqueidentifier
        public bool IsRead { get; set; } // bit, null
        public DateTime? ReadOn { get; set; } // datetime, null
        public Guid? ItineraryId { get; set; } // uniqueidentifier, null
        public string? BookingId { get; set; } // varchar(max)
        public Guid? SupplierId { get; set; } // uniqueidentifier, null
        public string? SupplierRefNo { get; set; }
        public bool? IsPriceChanged { get; set; } // bit, null
        public decimal? UpdatedPrice { get; set; } // decimal(12), null
        public DateTime CreatedOn { get; set; } // datetime2(7), not null
        public Guid? CreatedBy { get; set; } // uniqueidentifier, null
        public DateTime? ModifiedOn { get; set; } // datetime2(7), null
        public Guid? ModifiedBy { get; set; } // uniqueidentifier, null
        public bool IsDeleted { get; set; } // bit, not null
        public DateTime? DeletedOn { get; set; } // datetime2(7), null
        public Guid? DeletedBy { get; set; } // uniqueidentifier, null
        public string? BookingName { get; set; }
        public string? SupplierName { get; set; }
    }
}
