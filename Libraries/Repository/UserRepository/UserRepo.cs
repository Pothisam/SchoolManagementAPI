using Microsoft.EntityFrameworkCore;
using Models.UserModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UserRepository
{
    public class UserRepo : IUserRepo
    {
        private readonly SchoolManagementContext _context;
        public UserRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<LoginResponse> AdminLoginAsync(LoginRequestwithIP login)
        {
            LoginResponse? response = await (from x in _context.SmspassTables
                                             join y in _context.InstitutionDetails on x.InstitutionCode equals y.Sysid
                                             where x.UserName == login.UserName && x.Password == login.Password
                                             select new LoginResponse
                                             {
                                                 SysId = x.Sysid,
                                                 UserName = "Admin",
                                                 UserAuthkey = x.Password,
                                                 InstitutionCode = x.Sysid,
                                                 LoginType = "Admin",
                                                 InstitutionType = y.InstitutionType,
                                             }).FirstOrDefaultAsync();
            return response ?? new LoginResponse();
        }
    }
}
