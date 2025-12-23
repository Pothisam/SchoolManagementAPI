using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DocumentLibraryModels
{
    public class DocumentLibraryGetRequest
    {
        public required int FKID { get; set; }
        public required string TableName { get; set; }
        public required string FileType { get; set; }
        public required string Action { get; set; }
    }
    public class DocumentLibraryInsertRequest : DocumentLibraryGetRequest
    {
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required byte[] Data { get; set; }

    }
    public partial class DocumentLibraryGuid
    {
        public required Guid Guid { get; set; }
    }
}
