using Models.CommonModels;
using Models.DashboardModels;
using Repository.DashboardRepository;

namespace Services.DashboardServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepo _IDashboardRepo;

        public DashboardService(IDashboardRepo dashboardRepo)
        {
            _IDashboardRepo = dashboardRepo;
        }

        public async Task<CommonResponse<List<StudentCountClassWiseResponse>>> GetStudentCountClassWiseAsync(APIRequestDetails apiRequestDetails)
        {
            var result = await _IDashboardRepo.GetStudentCountClassWiseAsync(apiRequestDetails);
            return new CommonResponse<List<StudentCountClassWiseResponse>>
            {
                Status = Status.Success,
                Data = result
            };
        }

        public async Task<CommonResponse<List<FeesSummaryClassWiseResponse>>> GetFeesSummaryClassWiseAsync(GetFeesSummaryClassWiseRequest request, APIRequestDetails apiRequestDetails)
        {
            var result = await _IDashboardRepo.GetFeesSummaryClassWiseAsync(request, apiRequestDetails);
            return new CommonResponse<List<FeesSummaryClassWiseResponse>>
            {
                Status = Status.Success,
                Data = result
            };
        }
    }
}