using System;

namespace SQLDataAccessLayer.Models
{
    public class EmailLogsSettingCollection
    {
        public long CustomerEmailSettingId { get; set; } // Primary Key
        public Guid? ItineraryID { get; set; } // Nullable

        public DateTime? SentDate { get; set; }
        public Guid? TypeID { get; set; }
        public string Type { get; set; }
        public string FromAddress { get; set; }
        public string RecipientEmail { get; set; }
        public string Recipient { get; set; }
        public string EmailSubject { get; set; }
        public string AttachmentPDF { get; set; }
        public string AttachmentWord { get; set; }
        public string EmailBodyContentTemplate { get; set; }
        public string EmailBodyContentPreview { get; set; }
        public bool? SendStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public Guid? PassengerId { get; set; }
    }

}
