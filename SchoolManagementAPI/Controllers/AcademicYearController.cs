using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.AcademicYearModels;
using Services.AcademicYearServices;
using Services.ClassServices;
using Services.CommonServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AcademicYearController : Controller
    {
        private readonly IAcademicYearService _academicYearService;
        private readonly ICommonService _ICommonService;
        public AcademicYearController(IAcademicYearService academicYearService, ICommonService ICommonService)
        {
            _academicYearService = academicYearService;
            _ICommonService = ICommonService;
        }
        [HttpPost("AddAcademicYear")]
        public async Task<IActionResult> AddAcademicYear(AddAcademicYearRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _academicYearService.AddAcademicYearAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetAcademicYearList")]
        public async Task<IActionResult> GetAcademicYearList()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _academicYearService.GetAcademicYearListAsync(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetActiveAcademicYearList")]
        public async Task<IActionResult> GetActiveAcademicYearList()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _academicYearService.GetActiveAcademicYearListAsync(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("UpdateAcademicYearStatus")]
        public async Task<IActionResult> UpdateAcademicYearStatus(UpdateAcademicYear request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            return Ok(await _academicYearService.UpdateAcademicYearStatusAsync(request, apiRequestDetails));
        }
    }
}
