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
    public class GetFeesGentrationRequest
    {
        public int acadamicYear { get; set; }
        public int classfkid  { get; set; }
        public int sectionfkid { get; set; }
        public int feestypefkid { get; set; }
        public int amount { get; set; }
    }
    public class GentrationFeesRequest
    {
        public int[] studentdetailsfkid { get; set; } = Array.Empty<int>();
        public int sectionfkid { get; set; }       
        public int academicYearFkid { get; set; }
        public int feestypefkid { get; set; }
        public int amount { get; set; }
    }
}
