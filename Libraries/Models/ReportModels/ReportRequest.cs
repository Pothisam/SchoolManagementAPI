using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ReportModels
{
    public class StudentTrasferRequest
    {
        public int AcademicYearFrom { get; set; } 
        public int AcademicYearTo { get; set; }
        public int ClassSectionFrom { get; set; } = 0;

        public int ClassSectionTo { get; set;} = 0;
    }
}
