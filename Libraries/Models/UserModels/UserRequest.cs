using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.UserModels
{
    public class UserRequest
    {
    }
    public class LoginRequest
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class LoginRequestwithIP
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string IPAddress { get; set; }
    }
}
