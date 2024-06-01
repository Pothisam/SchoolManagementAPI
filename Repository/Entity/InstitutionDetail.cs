using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class InstitutionDetail
{
    public long SysId { get; set; }

    public string InstitutionName { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public string? MobileNo { get; set; }

    public string? AlternateMobileNo { get; set; }

    public string? Landline { get; set; }

    public string? PinCode { get; set; }

    public string? DistrictName { get; set; }

    public string? StateName { get; set; }

    public string? LogoFileName { get; set; }

    public string? LogoContentType { get; set; }

    public byte[]? LogoData { get; set; }

    public string? LogoWithTextFileName { get; set; }

    public string? LogoWithTextContentType { get; set; }

    public byte[]? LogoWithTextData { get; set; }

    public string? FavIconFileName { get; set; }

    public string? FavIconContentType { get; set; }

    public byte[]? FavIconData { get; set; }

    public string InstitutionType { get; set; } = null!;

    public string StaffIdprefix { get; set; } = null!;

    public string? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
