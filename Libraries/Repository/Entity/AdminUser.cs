using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class AdminUser
{
    public int SysId { get; set; }

    public int StaffFkid { get; set; }

    public string Name { get; set; } = null!;

    public string OtherSettings { get; set; } = null!;

    public string AllowLogin { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StaffDetail StaffFk { get; set; } = null!;
}
