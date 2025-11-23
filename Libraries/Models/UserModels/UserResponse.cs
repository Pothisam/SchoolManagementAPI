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
        public bool IsHOD { get; set; } = false;
        public bool IsPrincipal { get; set; } = false;
    }
}
