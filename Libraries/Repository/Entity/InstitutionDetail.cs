using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class InstitutionDetail
{
    public int Sysid { get; set; }

    public string? InstitutionName { get; set; }

    public string? Emailid { get; set; }

    public string? OfficialMail { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? MobileNumer { get; set; }

    public string? AlternateMobileNumer { get; set; }

    public string? Website { get; set; }

    public string? Landline { get; set; }

    public string? Pincode { get; set; }

    public string? PostofficeName { get; set; }

    public string? Districtname { get; set; }

    public string? StateName { get; set; }

    public string? LogoFileName { get; set; }

    public string? LogoContentType { get; set; }

    public byte[]? LogoData { get; set; }

    public string? LogoWithTextFileName { get; set; }

    public string? LogoWithTextContentType { get; set; }

    public byte[]? LogoWithTextData { get; set; }

    public string? FaviconFileName { get; set; }

    public string? FaviconContentType { get; set; }

    public byte[]? FaviconData { get; set; }

    public string? InstitutionType { get; set; }

    public string? StaffIdprefix { get; set; }

    public string? EnteredBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<SmspassTable> SmspassTables { get; set; } = new List<SmspassTable>();
}
