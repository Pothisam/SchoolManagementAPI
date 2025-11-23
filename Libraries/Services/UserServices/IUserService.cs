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
    }
}
