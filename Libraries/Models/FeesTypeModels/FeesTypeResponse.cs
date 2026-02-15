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
        public string Entryby { get; set; }
        public DateTime? EntryDate { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
