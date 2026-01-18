using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ReportModels
{
    public class StudentTrasferResponse
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime DOB { get; set; }

        public string FromAcademicYear { get; set; } = string.Empty;
        public string FromClassName { get; set; } = string.Empty;
        public string FromSectionName { get; set; } = string.Empty;

        public string ToAcademicYear { get; set; } = string.Empty;

        public string? ExistingClassName { get; set; }
        public string? ExistingSectionName { get; set; }

        public int AlreadyExisting { get; set; }
    }
}
