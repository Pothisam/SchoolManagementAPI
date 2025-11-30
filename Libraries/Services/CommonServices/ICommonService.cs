using Models.CommonModels;
using Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.CommonServices
{
    public interface ICommonService
    {
        Task<string> Encrypt(string CipherText);
        Task CreateJWTToken(CommonResponse<LoginResponse> response, LoginResponse result, LoginRequestwithIP request);
        APIRequestDetails GetAPIRequestDetails(ClaimsPrincipal user);
    }
}
