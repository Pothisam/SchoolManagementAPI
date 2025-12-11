using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class ClassSection
{
    public int SysId { get; set; }

    public int ClassFkid { get; set; }

    public string SectionName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual Class ClassFk { get; set; } = null!;

    public virtual ICollection<StudentClassDetail> StudentClassDetails { get; set; } = new List<StudentClassDetail>();
}
