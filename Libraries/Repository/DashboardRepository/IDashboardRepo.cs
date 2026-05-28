using Models.CommonModels;
using Models.DashboardModels;

namespace Repository.DashboardRepository
{
    public interface IDashboardRepo
    {
        Task<List<StudentCountClassWiseResponse>> GetStudentCountClassWiseAsync(APIRequestDetails apiRequestDetails);
        Task<List<FeesSummaryClassWiseResponse>> GetFeesSummaryClassWiseAsync(GetFeesSummaryClassWiseRequest request, APIRequestDetails apiRequestDetails);
    }
}