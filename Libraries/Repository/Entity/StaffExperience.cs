using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StaffExperience
{
    public int Sysid { get; set; }

    public int StaffDetailsFkid { get; set; }

    public string InstituionName { get; set; } = null!;

    public string Position { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime Todate { get; set; }

    public int Salary { get; set; }

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StaffDetail StaffDetailsFk { get; set; } = null!;
}
