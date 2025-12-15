using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CommonModels
{
    public class GetRecordHistoryRequest
    {
        public int FID { get; set; }
        public required string TableName { get; set; }
        public string Application { get; set; }
        public bool LoadAllRecord { get; set; }
    }
    public class PostOfficeRequest
    {
        [Required]
        public string pincode { get; set; }
    }
}
