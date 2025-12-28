using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StudentClassDetail
{
    public int SysId { get; set; }

    public int StudentDetailsFkid { get; set; }

    public int AcademicYearFkid { get; set; }

    public int ClassSectionFkid { get; set; }

    public string? StudentType { get; set; }

    public string? RollNo { get; set; }

    public string? ExamRegisterNumber { get; set; }

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StudentDetail StudentDetailsFk { get; set; } = null!;
}
