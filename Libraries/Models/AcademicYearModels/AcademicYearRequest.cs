using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AcademicYearModels
{
    public class AddAcademicYearRequest
    {
        public int YearDate { get; set; }
        public string Year { get; set; } = null!;
    }
    public class UpdateAcademicYear
    {
        public required int Sysid { get; set; } 
    }
}
