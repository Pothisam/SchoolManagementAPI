using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AcademicYearModels
{
    public class AcademicYearResponse
    {
        public int SysId { get; set; }
        public DateTime YearDate { get; set; }
        public string Year { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string EnteredBy { get; set; } = null!;
        public DateTime EntryDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
