using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DashboardModels;
using Services.CommonServices;
using Services.DashboardServices;

namespace CMS.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _IDashboardService;
        private readonly ICommonService _ICommonService;

        public DashboardController(IDashboardService dashboardService, ICommonService commonService)
        {
            _IDashboardService = dashboardService;
            _ICommonService = commonService;
        }

        [HttpPost("GetStudentCountClassWise")]
        public async Task<IActionResult> GetStudentCountClassWise()
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IDashboardService.GetStudentCountClassWiseAsync(apirequestdetails);
            return Ok(result);
        }

        [HttpPost("GetFeesSummaryClassWise")]
        public async Task<IActionResult> GetFeesSummaryClassWise(GetFeesSummaryClassWiseRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IDashboardService.GetFeesSummaryClassWiseAsync(request, apirequestdetails);
            return Ok(result);
        }
    }
}