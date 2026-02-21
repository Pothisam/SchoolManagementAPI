using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.FeesTypeModels
{
    public class FeesTypeListResponse
    {
        public int Sysid { get; set; }
        public string FeesDescription { get; set; }
        public string status { get; set; }
        public string Entryby { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
    public class StudentFeeGenerateStatusResponse
    {
        public int Sysid { get; set; }
        public string StudentName { get; set; } = "";
        public string Stdid { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string SectionName { get; set; } = "";
        public string Hostel { get; set; }          // change type if yours is string/int
        public string Year { get; set; } = "";    // change type if yours is int
        public decimal Debit { get; set; }
        public string Status { get; set; } = "";
    }
}
