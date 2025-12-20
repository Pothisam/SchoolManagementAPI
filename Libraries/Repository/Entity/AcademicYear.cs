using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class AcademicYear
{
    public int SysId { get; set; }

    public DateTime YearDate { get; set; }

    public string Year { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<StudentClassDetail> StudentClassDetails { get; set; } = new List<StudentClassDetail>();
}
