using Models.CommonModels;
using Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserServices
{
    public interface IUserService
    {
        Task<CommonResponse<LoginResponse>> SMSLoginAsync(LoginRequestwithIP request);
        Task<CommonResponse<string>> ChangeAdminPasswordAsync(ChangePasswordRequest request, APIRequestDetails apiRequestDetails);
        #region Admin User
        Task<CommonResponse<List<AdminUserResponse>>> GetAdminUsersAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> AddOrUpdateAdminUserAsync(AddAdminUserRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<Dictionary<string, bool>>> GetSettingsByFIDAsync(APIRequestDetails apiRequestDetails);
        #endregion
    }
}
