using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.FeesTypeModels;

namespace Services.FeesTypeServices
{
    public interface IFeesTypeService
    {
        Task<CommonResponse<List<FeesTypeListResponse>>> GetFeesTypeListAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> SaveFeesTypeAsync(SaveFeesTypeRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteFeesTypeAsync(FeesTypePKRequest request, APIRequestDetails apiRequestDetails);
        #region Gentrate Fees
        Task<CommonResponse<List<StudentFeeGenerateStatusResponse>>> GetFeesTypeListAsync(GetFeesGentrationRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> InsertStudentFeesAsync(GentrationFeesRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Apporve Fees
        Task<CommonResponse<List<StudentApproveFeesResponse>>> GetApproveFeesAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<ApproveFeesViewResponse>>> GetApproveFeesViewAsync(GetApproveFeesViewRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateFeesApproveAsync(UpdateFeesApproveRequest request, APIRequestDetails apiRequestDetails);
        #endregion
    }
}
