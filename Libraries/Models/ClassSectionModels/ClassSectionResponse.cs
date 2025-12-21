using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ClassSectionModels
{
    public class ClassSectionResponse
    {
        public int SysId { get; set; }
        public int ClassFkid { get; set; }
        public string ClassName { get; set; } = null!;
        public string SectionName { get; set; } = null!;
        public string EnteredBy { get; set; } = null!;

        public DateTime EntryDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
