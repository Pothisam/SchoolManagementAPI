using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StudentFeesTransaction
{
    public int SysId { get; set; }

    public int StudentFkid { get; set; }

    public int FeesTypeFkid { get; set; }

    public int StudentClassDetailsFkid { get; set; }

    public int RefNo { get; set; }

    public string PaymentMode { get; set; } = null!;

    public string BankName { get; set; } = null!;

    public string ChequeNo { get; set; } = null!;

    public DateTime ChequeDate { get; set; }

    public string TransationType { get; set; } = null!;

    public DateTime GenerateDate { get; set; }

    public string Description { get; set; } = null!;

    public decimal Debit { get; set; }

    public decimal Credit { get; set; }

    public string Remark { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EntryBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual FeesType FeesTypeFk { get; set; } = null!;

    public virtual StudentClassDetail StudentClassDetailsFk { get; set; } = null!;

    public virtual StudentDetail StudentFk { get; set; } = null!;
}
