using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class AuditTable
{
    public int Sysid { get; set; }

    public int? Fid { get; set; }

    public string TableName { get; set; } = null!;

    public string? TableAction { get; set; }

    public string? Application { get; set; }

    public string Modifiedby { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }

    public string Data { get; set; } = null!;

    public int InstitutionCode { get; set; }
}
