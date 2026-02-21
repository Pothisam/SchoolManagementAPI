using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class FeesType
{
    public int Sysid { get; set; }

    public string FeesDescription { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string Entryby { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? Modifiedby { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual ICollection<StudentFeesTransaction> StudentFeesTransactions { get; set; } = new List<StudentFeesTransaction>();
}
