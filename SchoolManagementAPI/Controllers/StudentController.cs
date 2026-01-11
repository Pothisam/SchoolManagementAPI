

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StudentModels;
using Services.CommonServices;
using Services.StudentServices;

namespace CMS.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _IStudentService;
        private readonly ICommonService _ICommonService;
        public StudentController(IStudentService StudentService, ICommonService ICommonService)
        {
            _IStudentService = StudentService;
            _ICommonService = ICommonService;
        }
        #region Add Student
        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent(AddStudentRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.AddStudent(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStudentAutoComplete")]
        public async Task<IActionResult> GetStudentAutoComplete(AutoCompleteRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentAutoComplete(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
        #region View Student List
        [HttpPost("GetStudentCount")]
        public async Task<IActionResult> GetStudentCount()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentCountAsync(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetStudentDetailsShort")]
        public async Task<IActionResult> GetStudentDetailsShort(StudentShortRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentDetailsShortAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetStudentDetailsShortAC")]
        public async Task<IActionResult> GetStudentDetailsShortAC(StudentSearchRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentDetailsShortAsync(request, apiRequestDetails);
            return Ok(result);
        }
        #endregion
        #region View Student Details
        [HttpPost("GetStudentDetailBySysid")]
        public async Task<IActionResult> GetStudentDetailBySysid(StudentDetailsViewRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentDetailBySysid(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(UpdateStudentDetailRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.UpdateStudent(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("ResetStudentPassword")]
        public async Task<IActionResult> ResetStudentPassword(StudentDetailsViewRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.ResetStudentPasswordAsync(request, apiRequestDetails);
            return Ok(result);
        }
        #endregion
        #region Document Library
        [HttpPost("AddStudentDocument")]
        public async Task<IActionResult> AddStudentDocument(DocumentLibraryBulkInsertByFKID request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.AddStudentDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetStudentDocument")]
        public async Task<IActionResult> GetStudentDocument(StudentDetailsViewRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IStudentService.GetStudentDocumentAsync(request, apirequestdetails);
            return Ok(result);
        }
        #endregion
    }
}
