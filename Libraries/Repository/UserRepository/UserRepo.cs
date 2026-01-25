using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
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

        public async Task<bool> UpdateAdminPasswordAsync(ChangePasswordRequest request, APIRequestDetails apiRequestDetails)
        {
            var adminAccount = await _context.SmspassTables.FirstOrDefaultAsync(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Password == request.OldPassword);

            if (adminAccount == null)
                return false;

            adminAccount.Password = request.NewPassword;
            await _context.SaveChangesAsync();
            return true;
        }
        #region Admin User
        public async Task<List<AdminUserResponse>> GetAdminUsersAsync(APIRequestDetails apiRequestDetails)
        {
            return await (from x in _context.AdminUsers
                          join y in _context.StaffMasterViews on x.StaffFkid equals y.Sysid
                          where x.InstitutionCode == apiRequestDetails.InstitutionCode
                          select new AdminUserResponse
                          {
                              Sysid = x.SysId,
                              Fidstaff = x.StaffFkid,
                              Name = y.Name,
                              AllowLogin = x.AllowLogin,
                              OtherSettings = x.OtherSettings,
                              EnteredBy = x.EnteredBy,
                              Entrydate = x.EntryDate,
                              ModifiedBy = x.ModifiedBy,
                              ModifiedDate = x.ModifiedDate,
                              InstitutionCode = x.InstitutionCode,
                              Guid = y.Guid
                          }).ToListAsync();
        }
        public async Task<bool> AddAdminUserAsync(AdminUser adminUser)
        {
            await _context.AdminUsers.AddAsync(adminUser);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<AdminUser> GetAdminUserByFIDAsync(int FID)
        {
            return await _context.AdminUsers.FirstOrDefaultAsync(x => x.StaffFkid == FID);

        }
        public async Task<bool> UpdateAdminUserAsync(AdminUser adminUser)
        {
            _context.AdminUsers.Update(adminUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dictionary<string, bool>> GetSettingsByFIDAsync(APIRequestDetails apirequestdetails)
        {
            var possibleSettings = new List<string> { "Application Settings", "Management", "Staff", "Student" };
            // If user is "CMS-Admin", return all settings as true
            if (apirequestdetails.LoginType == "Admin")
            {
                return possibleSettings.ToDictionary(setting => setting, setting => true);
            }
            // Fetch user details from AdminUsers
            var userRecord = await _context.AdminUsers
                .Where(x => x.StaffFkid == apirequestdetails.SysId)
                .Select(x => new { x.OtherSettings, x.AllowLogin })
                .FirstOrDefaultAsync();

            // If user is not found or AllowLogin is not "Yes", return all settings as false
            if (userRecord == null || userRecord.AllowLogin != "Yes")
            {
                return possibleSettings.ToDictionary(setting => setting, setting => false);
            }
            var settingsDict = possibleSettings.ToDictionary(setting => setting, setting => userRecord.OtherSettings?.Contains(setting) ?? false);

            return settingsDict;
        }
        #endregion
    }
}
