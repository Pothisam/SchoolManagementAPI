using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StaffLanguageDetail
{
    public int SysId { get; set; }

    public int StaffFkid { get; set; }

    public string LanguageKnow { get; set; } = null!;

    public string ReadLanguage { get; set; } = null!;

    public string WriteLanguage { get; set; } = null!;

    public string SpeakLanguage { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public virtual StaffDetail StaffFk { get; set; } = null!;
}
