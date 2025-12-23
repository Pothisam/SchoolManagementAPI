using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.StaffModels
{
    public class StaffCountResponse
    {
        public int TotalStaff { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Teaching { get; set; }
        public int NonTeaching { get; set; }
    }
    public class DesignationListResponse
    {
        public string Designation { get; set; }
        public int DesignationCode { get; set; }
    }
    public class StaffDetailSearchResponse
    {
        public int SysId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string StaffType { get; set; }
        public string Gender { get; set; }
        public string EnteredBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? Guid { get; set; }
    }
}
