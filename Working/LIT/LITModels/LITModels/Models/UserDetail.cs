using System;
using System.Collections.Generic;

namespace LITModels.LITModels.Models;

public partial class UserDetail
{
    public Guid? UserId { get; set; }

    public string? UserName { get; set; }

    public string? FullName { get; set; }

    public string? EmailAddress { get; set; }

    public string? Password { get; set; }

    public string? ConfirmPassword { get; set; }

    public string? EmailSignature { get; set; }

    public Guid? UserRoles { get; set; }

    public bool? IsActivated { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }

    public Guid? DeletedBy { get; set; }
}
