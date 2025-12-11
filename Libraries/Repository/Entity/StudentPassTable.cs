using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StudentPassTable
{
    public int SysId { get; set; }

    public int? StudentDetailsFkid { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Otp { get; set; }

    public string? Status { get; set; }

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StudentDetail? StudentDetailsFk { get; set; }
}
