using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserModels
{
    public class LoginResponse
    {
        public int SysId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserAuthkey { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string LoginType { get; set; } = null!;
        public string InstitutionType { get; set; } = null!;
        public int InstitutionCode { get; set; }
        public Guid? Guid { get; set; }
        public bool IsPrincipal { get; set; } = false;
    }
    public class AdminUserResponse
    {
        public int Sysid { get; set; }
        public int Fidstaff { get; set; }
        public string Name { get; set; } = null!;
        public string OtherSettings { get; set; } = null!;
        public string AllowLogin { get; set; } = null!;
        public int InstitutionCode { get; set; }
        public string EnteredBy { get; set; } = null!;
        public DateTime Entrydate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string DepartmentCode { get; set; } = null!;
        public Guid? Guid { get; set; }
    }
}
