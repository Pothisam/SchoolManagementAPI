using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.AcademicYearModels
{
    public class AddAcademicYearRequest
    {
        public DateTime YearDate { get; set; }
        public string Year { get; set; } = null!;
    }
}
