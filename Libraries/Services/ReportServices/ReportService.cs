
using Azure;
using Models.CommonModels;
using Models.ReportModels;
using Repository.ReportRepository;


namespace Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IReportRepo _reportRepo;

        public ReportService(IReportRepo reportRepo)
        {
            _reportRepo = reportRepo;
        }
        public async Task<CommonResponse<List<StudentTrasferResponse>>> GetStudentPromotionListAsync(StudentTrasferRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentTrasferResponse>>();
            var result = await _reportRepo.GetStudentPromotionListAsync(request, apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }
    }
}
