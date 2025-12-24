using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DocumentLibraryModels
{
    public class DocumentLibraryImageExportResponse
    {
        public long Fkid { get; set; }
        public string TableName { get; set; } = null!;
        public required byte[] Data { get; set; }
        public Guid Guid { get; set; }
        public int FileSize { get; set; }
    }
    public class DocumentLibraryDetailsResponse
    {
        public long Sysid { get; set; }
        public required string FileName { get; set; }
        public required string FileType { get; set; }
        public required int FileSize { get; set; }
        public required Guid Guid { get; set; }
        public required string EnteredBy { get; set; }
        public required DateTime EntryDate { get; set; }
        public required string ModifiedBy { get; set; }
        public required DateTime ModifiedDate { get; set; }

    }
}
