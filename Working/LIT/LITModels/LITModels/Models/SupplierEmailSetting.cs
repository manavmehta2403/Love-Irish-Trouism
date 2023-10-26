using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class SupplierEmailSetting
{
    public long SupplierEmailSettingsid { get; set; }

    public string? FromAddress { get; set; }

    public string? Bcc { get; set; }

    public string? EmailSubject { get; set; }

    public string? EmailBodyContentTemplate { get; set; }

    public string? EmailTemplate { get; set; }

    public string? ToAddress { get; set; }
    public string? SupplierName { get; set; }

    public string? EmailBodyContentPreview { get; set; }

    public string? Attachment { get; set; }

    public bool? SendStatus { get; set; }

    public string? Error { get; set; }

    public Guid? SetbookingStatusAfterSuccessfulSend { get; set; }

    public bool? Skipthispage { get; set; }

    public bool? Saveacopyofthesentemail { get; set; }

    public bool? Bcctosendersemailaddress { get; set; }

    public bool? Showpricedetailsforbooking { get; set; }

    public bool? Includeareadreceipt { get; set; }

    public bool? GroupbookingsbySupplieremail { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
    public Guid? SupplierId { get; set; }
}
