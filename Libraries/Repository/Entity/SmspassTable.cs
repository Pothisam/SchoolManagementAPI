using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class SmspassTable
{
    public int Sysid { get; set; }

    public int? FkInstitutionDetails { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public DateTime? Entrydate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? Validitydate { get; set; }

    public virtual InstitutionDetail? FkInstitutionDetailsNavigation { get; set; }
}
