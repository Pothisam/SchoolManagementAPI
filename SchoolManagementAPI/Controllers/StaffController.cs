using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.StaffModels;
using Services.CommonServices;
using Services.InstitutionDetailsServices;
using Services.StaffServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StaffController : Controller
    {
        private readonly IStaffService _staffService;
        private readonly ICommonService _ICommonService;
        public StaffController(IStaffService staffService, ICommonService ICommonService)
        {
            _staffService = staffService;
            _ICommonService = ICommonService;
        }
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff(AddStaffRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.AddStaffAsync(request.staffdetails, request.LanguageRequests, request.EducationRequests, request.ExperienceRequests, request.DocumentRequests, apirequestdetails);
            return Ok(result);
        }
    }
}
