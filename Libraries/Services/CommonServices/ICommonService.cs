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
        Task<CommonResponse<string>> GetDatetime();
        Task<CommonResponse<InstitutionLogoResponse>> GetLogo(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<RecordHistoryResponse>>> GetRecordHistory(GetRecordHistoryRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<PostOfficeResponse>>> GetPostOffice(PostOfficeRequest request);
    }
}
