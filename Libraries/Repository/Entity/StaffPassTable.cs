using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StaffPassTable
{
    public int SysId { get; set; }

    public int StaffDetailsFkid { get; set; }

    public string Password { get; set; } = null!;

    public string? Otp { get; set; }

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StaffDetail StaffDetailsFk { get; set; } = null!;
}
