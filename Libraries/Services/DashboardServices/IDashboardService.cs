using Models.CommonModels;
using Models.DashboardModels;

namespace Services.DashboardServices
{
    public interface IDashboardService
    {
        Task<CommonResponse<List<StudentCountClassWiseResponse>>> GetStudentCountClassWiseAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<FeesSummaryClassWiseResponse>>> GetFeesSummaryClassWiseAsync(GetFeesSummaryClassWiseRequest request, APIRequestDetails apiRequestDetails);
    }
}