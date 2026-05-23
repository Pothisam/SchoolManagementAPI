using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.StudentFeesTransactionModels;
using Models.StudentModels;
using Services.CommonServices;
using Services.StudentFeesTransactionServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class StudentFeesTransactionController : Controller
    {
        private readonly IStudentFeesTransactionService _studentFeesTransactionService;
        private readonly ICommonService _ICommonService;
        public StudentFeesTransactionController(IStudentFeesTransactionService studentFeesTransactionService, ICommonService ICommonService)
        {
            _studentFeesTransactionService = studentFeesTransactionService;
            _ICommonService = ICommonService;
        }
        [HttpPost("GetFeesDetailsByclass")]
        public async Task<IActionResult> GetFeesDetailsByclass(StudentFeesTransactionRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetFeesList(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetFeesDetailsByName")]
        public async Task<IActionResult> GetFeesDetailsByName(StudentFeesTransactionByNameRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetFeesDetailsBtNameList(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetDebit")]
        public async Task<IActionResult> GetDebit(GetDebitRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetDebitAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("GetCredit")]
        public async Task<IActionResult> GetCredit(GetDebitRequest request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetCreditAsync(request, apirequestdetails);
            return Ok(result);
        }
        [HttpPost("AddStudentFeesTransaction")]
        public async Task<IActionResult> AddStudentFeesTransaction(AddStudentFeesTransactionRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.AddStudentFeesTransactionAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetFeesReportDateWise")]
        public async Task<IActionResult> GetFeesReportDateWise(GetFeesReportDateWiseRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetFeesReportDateWiseAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("GetFeesReportById")]
        public async Task<IActionResult> GetFeesReportById(GetPrintCashReceiptValueRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.GetFeesReportByIdAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("DeleteTransaction")]
        public async Task<IActionResult> DeleteTransaction(DeleteFeesTransactionRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _studentFeesTransactionService.DeleteCredit(request, apiRequestDetails);
            return Ok(result);
        }
    }
}
