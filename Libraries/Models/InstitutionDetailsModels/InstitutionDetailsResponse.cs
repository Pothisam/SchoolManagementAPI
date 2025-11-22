using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.InstitutionDetailsModels
{
    public class InstitutionDetailsResponse
    {
        public int Sysid { get; set; }

        public string? InstitutionName { get; set; }

        public string? Emailid { get; set; }

        public string? OfficialMail { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? MobileNumer { get; set; }

        public string? AlternateMobileNumer { get; set; }

        public string? Website { get; set; }

        public string? Landline { get; set; }

        public string? Pincode { get; set; }

        public string? PostofficeName { get; set; }

        public string? Districtname { get; set; }

        public string? StateName { get; set; }
        public string? StaffIdprefix { get; set; }
        public string? InstitutionType { get; set; }
        public string? EnteredBy { get; set; }

        public DateTime? EntryDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
