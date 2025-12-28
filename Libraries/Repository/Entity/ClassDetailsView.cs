using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class ClassDetailsView
{
    public int SysId { get; set; }

    public string Year { get; set; } = null!;

    public string ClassName { get; set; } = null!;

    public string SectionName { get; set; } = null!;

    public string ClassSection { get; set; } = null!;

    public int InstitutionCode { get; set; }
}
