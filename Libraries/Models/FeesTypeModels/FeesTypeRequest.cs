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
    public class GetApproveFeesViewRequest
    {
        public int AcademicYearSysId { get; set; }
        public int ClassSectionId { get; set; }
        public int FeesTypeFkid { get; set; }
        public DateTime GDate { get; set; } // date passed from UI
    }
    public class UpdateFeesApproveRequest
    {
        public int[] studentdetailsfkid { get; set; }  = Array.Empty<int>();
        public bool Approved { get; set; }
    }
    public class GeConcessionGentrationRequest
    {
        public int acadamicYear { get; set; }
        public int classfkid { get; set; }
        public int sectionfkid { get; set; }
        public int feestypefkid { get; set; }
    }
    public class GenerationConcessionRequest
    {
        public int[] studentdetailsfkid { get; set; } = Array.Empty<int>();
        public int sectionfkid { get; set; }
        public int academicYearFkid { get; set; }
        public int feestypefkid { get; set; }
    }
}
