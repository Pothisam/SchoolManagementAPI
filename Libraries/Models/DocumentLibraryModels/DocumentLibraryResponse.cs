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
}
