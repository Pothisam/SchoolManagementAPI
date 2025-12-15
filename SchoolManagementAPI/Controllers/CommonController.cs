using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CommonModels;
using SchoolManagementAPI.Attributes;
using Services.CommonServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Authorize]

    [Route("[controller]")]
    public class CommonController : Controller
    {
        private readonly ICommonService _ICommonService;
        public CommonController(ICommonService ICommonService)
        {
            _ICommonService = ICommonService;
        }
        [HttpPost("GetDate")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDate()
        {
            var result = await _ICommonService.GetDatetime();
            return Ok(result);
        }
        [HttpPost("GetLogo")]
        public async Task<IActionResult> GetLogo()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _ICommonService.GetLogo(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetRecordHistory")]
        public async Task<IActionResult> GetRecordHistory(GetRecordHistoryRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _ICommonService.GetRecordHistory(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetPostOffice")]
        public async Task<IActionResult> GetPostOffice(PostOfficeRequest request)
        {
            var result = await _ICommonService.GetPostOffice(request);
            return Ok(result);
        }
    }
}
