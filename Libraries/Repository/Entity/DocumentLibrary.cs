using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class DocumentLibrary
{
    public int Sysid { get; set; }

    public long Fkid { get; set; }

    public string TableName { get; set; } = null!;

    public string FileName { get; set; } = null!;

    public string ContentType { get; set; } = null!;

    public byte[] Data { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public int FileSize { get; set; }

    public string Action { get; set; } = null!;

    public Guid Guid { get; set; }

    public string Status { get; set; } = null!;

    public int InstitutionCode { get; set; }

    public string EnteredBy { get; set; } = null!;

    public DateTime EntryDate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }
}
