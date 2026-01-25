using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.UserModels;
using Repository.Entity;

namespace Repository.UserRepository
{
    public interface IUserRepo
    {
        Task<LoginResponse> AdminLoginAsync(LoginRequestwithIP login);
        Task<bool> UpdateAdminPasswordAsync(ChangePasswordRequest request, APIRequestDetails apiRequestDetails);
        #region Admin User
        Task<List<AdminUserResponse>> GetAdminUsersAsync(APIRequestDetails apiRequestDetails);
        Task<bool> AddAdminUserAsync(AdminUser adminUser);
        Task<AdminUser> GetAdminUserByFIDAsync(int FID);
        Task<bool> UpdateAdminUserAsync(AdminUser adminUser);
        Task<Dictionary<string, bool>> GetSettingsByFIDAsync(APIRequestDetails apirequestdetails);
        #endregion
    }
}
