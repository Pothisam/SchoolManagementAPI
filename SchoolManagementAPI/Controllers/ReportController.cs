using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ReportModels;
using Services.CommonServices;
using Services.ReportServices;
using Services.StudentServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _IReportService;
        private readonly ICommonService _ICommonService;
        public ReportController(IReportService IReportService, ICommonService ICommonService)
        {
            _IReportService = IReportService;
            _ICommonService = ICommonService;
        }
        [HttpPost("GetStudentTransferDetails")]
        public async Task<IActionResult> GetStudentTransferDetails(StudentTrasferRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IReportService.GetStudentPromotionListAsync(request, apiRequestDetails);
            return Ok(result);
        }
    }
}
