using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StaffEducationDetail
{
    public int SysId { get; set; }

    public int StaffDetailsFkid { get; set; }

    public string? DegreeType { get; set; }

    public string? Degree { get; set; }

    public int? YearOfpassing { get; set; }

    public string? UniversityName { get; set; }

    public string? InstituionName { get; set; }

    public string? Mode { get; set; }

    public string? PassPercentage { get; set; }

    public string? Specialization { get; set; }

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StaffDetail StaffDetailsFk { get; set; } = null!;
}
