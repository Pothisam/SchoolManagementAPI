using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.UserModels;

namespace Repository.UserRepository
{
    public interface IUserRepo
    {
        Task<LoginResponse> AdminLoginAsync(LoginRequestwithIP login);
        Task<bool> UpdateAdminPasswordAsync(ChangePasswordRequest request, APIRequestDetails apiRequestDetails);
    }
}
