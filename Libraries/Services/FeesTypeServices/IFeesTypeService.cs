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

        Task<CommonResponse<List<StudentFeeGenerateStatusResponse>>> GetFeesTypeListAsync(GetFeesGentrationRequest request,APIRequestDetails apiRequestDetails);
    }
}
