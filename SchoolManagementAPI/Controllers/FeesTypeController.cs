using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ClassModels;
using Models.FeesTypeModels;
using Services.CommonServices;
using Services.FeesTypeServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FeesTypeController : Controller
    {
        private readonly ICommonService _ICommonService;
        private readonly IFeesTypeService _FeesTypeService;
        public FeesTypeController(ICommonService ICommonService, IFeesTypeService feesTypeService   )
        {
            _ICommonService = ICommonService;
            _FeesTypeService = feesTypeService;
        }
        [HttpPost("GetFeesType")]
        public async Task<IActionResult> GetFeesType()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _FeesTypeService.GetFeesTypeListAsync(apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("SaveFeesType")]
        public async Task<IActionResult> SaveFeesType(SaveFeesTypeRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _FeesTypeService.SaveFeesTypeAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("DeleteFeesType")]
        public async Task<IActionResult> DeleteFeesType(FeesTypePKRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _FeesTypeService.DeleteFeesTypeAsync(request, apiRequestDetails);

            return Ok(result);
        }

        [HttpPost("GetFeesListView")]
        public async Task<IActionResult> GetFeesListView(GetFeesGentrationRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.GetFeesTypeListAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("InsertStudentFees")]
        public async Task<IActionResult> InsertStudentFees(GentrationFeesRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.InsertStudentFeesAsync(request, apiRequestDetails);
            return Ok(result);
        }

        [HttpPost("GetApproveFees")]
        public async Task<IActionResult> GetApproveFees()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.GetApproveFeesAsync(apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("GetApproveFeesView")]
        public async Task<IActionResult> GetApproveFeesView([FromBody] GetApproveFeesViewRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.GetApproveFeesViewAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateFeesApprove")]
        public async Task<IActionResult> UpdateFeesApprove([FromBody] UpdateFeesApproveRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.UpdateFeesApproveAsync(request, apiRequestDetails);

            return Ok(result);
        }
        #region Gentrate Concession
        [HttpPost("GetConcessionList")]
        public async Task<IActionResult> GetConcessionList(GeConcessionGentrationRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.GetConcessionListViewAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("InsertStudentConcession")]
        public async Task<IActionResult> InsertStudentConcession(GenerationConcessionRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _FeesTypeService.InsertStudentConcessionAsync(request, apiRequestDetails);
            return Ok(result);
        }
        #endregion
    }
}
