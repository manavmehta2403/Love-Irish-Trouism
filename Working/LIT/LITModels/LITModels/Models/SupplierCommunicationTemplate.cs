using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models
{
    public partial class SupplierCommunicationTemplate
    {
        public Guid ContentId { get; set; }
        public string? ContentName { get; set; }
        public string? For { get; set; }
        public string? Type { get; set; }
        public Guid? SupplierId { get; set; }
        public string? Heading { get; set; }
        public string? ReportImage { get; set; }
        public string? OnlineImage { get; set; }
        public string? BodyHtml { get; set; }
        public bool? IsActivated { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
