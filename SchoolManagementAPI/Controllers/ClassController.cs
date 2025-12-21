using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ClassModels;
using Services.ClassServices;
using Services.CommonServices;
using Services.InstitutionDetailsServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ClassController : Controller
    {
        private readonly IClassService _IClassService;
        private readonly ICommonService _ICommonService;
        public ClassController(IClassService classService, ICommonService ICommonService)
        {
            _IClassService = classService;
            _ICommonService = ICommonService;
        }
        [HttpPost("AddClass")]
        public async Task<IActionResult> AddClass(AddClassRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _IClassService.AddClassAsync(request,apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("GetClassList")]
        public async Task<IActionResult> GetClassList()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _IClassService.GetClassListAsync(apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateClassStatus")]
        public async Task<IActionResult> UpdateClassStatus(UpdateClassStatusRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IClassService.UpdateClassStatusAsync(request,apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetClassListActive")]
        public async Task<IActionResult> GetClassListActive()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _IClassService.GetClassListActiveAsync(apiRequestDetails);

            return Ok(result);
        }
    }
}
