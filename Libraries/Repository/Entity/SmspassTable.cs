using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class SmspassTable
{
    public int Sysid { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public DateTime Entrydate { get; set; }

    public DateTime ModifiedDate { get; set; }

    public DateTime? Validitydate { get; set; }

    public virtual InstitutionDetail InstitutionCodeNavigation { get; set; } = null!;
}
