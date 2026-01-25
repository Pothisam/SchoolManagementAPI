using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DocumentLibraryModels;
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
        #region view Staff
        [HttpPost("GetStaffDetailByID")]
        public async Task<IActionResult> GetStaffDetailByID(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffDetails(request, apirequestdetails);

            return Ok(result);
        }
        #endregion
        #region Language
        [HttpPost("AddStaffLanguage")]
        public async Task<IActionResult> AddStaffLanguageAsync(UpdateAddStaffLanguageDetail request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.AddStaffLanguageAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffLanguageKnowByID")]
        public async Task<IActionResult> GetStaffLanguageKnowByID(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffLanguageDetails(request, apirequestdetails);

            return Ok(result);
        }
        [HttpPost("DeleteStaffLanguage")]
        public async Task<IActionResult> DeleteStaffLanguage(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.DeleteStaffLanguageAsync(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
        #region Education
        [HttpPost("AddStaffEducation")]
        public async Task<IActionResult> InsertStaffEducationDetailsAsync(UpdateAddStaffEducationDetailAdd request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.InsertStaffEducationDetailsAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffEducationDetailsByID")]
        public async Task<IActionResult> GetStaffEducationDetailsByID(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffEducationDetailsByIDAsync(request, apirequestdetails);

            return Ok(result);
        }
        [HttpPost("DeleteStaffEducation")]
        public async Task<IActionResult> DeleteStaffEducation(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.DeleteStaffEducationAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("UpdateStaffEducationDetails")]
        public async Task<IActionResult> DeleteStaffEducation(UpdateAddStaffEducationDetailAdd request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.UpdateStaffEducationDetailsAsync(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
        #region Expirence 
        [HttpPost("AddStaffExperienceDetail")]
        public async Task<IActionResult> AddStaffExperienceDetail(UpdateStaffExperienceDetailAddRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.AddStaffExperienceDetailAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffExperienceDetailsByID")]
        public async Task<IActionResult> GetStaffExperienceDetailsByID(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffExperienceDetailsByIDAsync(request, apirequestdetails);

            return Ok(result);
        }
        [HttpPost("DeleteStaffExperience")]
        public async Task<IActionResult> DeleteStaffExperience(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.DeleteStaffExperienceAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("UpdateStaffExperienceDetails")]
        public async Task<IActionResult> UpdateStaffExperienceDetails(UpdateStaffExperienceDetailAddRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.UpdateStaffExperienceDetailsAsync(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
        #region Document 
        [HttpPost("AddStaffDocument")]
        public async Task<IActionResult> AddStaffDocument(DocumentLibraryBulkInsertByFKID request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.AddStaffDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffDocument")]
        public async Task<IActionResult> GetStaffDocument(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("DeleteStafffDocument")]
        public async Task<IActionResult> DeleteStafffDocumentAsync(DocumentLibrarySysid request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.DeleteStafffDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("UpdateStafffDocument")]
        public async Task<IActionResult> DeleteStafffDocumentAsync(DocumentLibraryBulkInsertByFKID request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.UpdateStaffDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("UpdateStaffDetails")]
        public async Task<IActionResult> UpdateStaffDetails(UpdateStaffDetailsRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.UpdateStaffDetailsAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("ResetStaffPassword")]
        public async Task<IActionResult> ResetStaffPassword(StaffDetailsPK request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.ResetStaffPasswordAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStaffNameList")]
        public async Task<IActionResult> GetStaffNameListByDepartmentCode()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _staffService.GetStaffNameList(apiRequestDetails);
            return Ok(result);
        }
        #endregion
    }
}
