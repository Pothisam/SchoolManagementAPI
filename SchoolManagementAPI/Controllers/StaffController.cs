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
        [HttpPost("GetStaffAutoComplete")]
        public async Task<IActionResult> GetStaffAutoComplete(StaffAutocompleteRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffAutoComplete(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff(AddStaffRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.AddStaffAsync(request.staffdetails, request.LanguageRequests, request.EducationRequests, request.ExperienceRequests, request.DocumentRequests, apirequestdetails);
            return Ok(result);
        }
        #region Staff View List
        [HttpPost("GetStaffCount")]
        public async Task<IActionResult> GetStaffCount()
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffCountAsync(apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffDesignationList")]
        public async Task<IActionResult> GetStaffDesignationList()
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffDesignationListAsync(apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffDetailSearch")]
        public async Task<IActionResult> GetStaffDetailByDepartmentCode(StaffSearchRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffDetailSearchAsync(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
    }
}
