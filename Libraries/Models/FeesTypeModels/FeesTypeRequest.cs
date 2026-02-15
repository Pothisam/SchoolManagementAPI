using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FeesTypeModels
{
    public class SaveFeesTypeRequest
    {
        public string FeesTypeDescription { get; set; } = string.Empty;
    }
    public class FeesTypePKRequest
    {
        public int Sysid { get; set; }
    }
}
