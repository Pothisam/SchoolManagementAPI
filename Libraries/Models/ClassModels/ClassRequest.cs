using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ClassModels
{
    public class AddClassRequest
    {
        public string ClassName { get; set; } = null!;
    }
    public class UpdateClassStatusRequest
    {
        public required int SysId { get; set; } 

        public required string Status { get; set; }
    }
}
