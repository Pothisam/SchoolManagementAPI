using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.InstitutionDetailsModels;
using Services.CommonServices;
using Services.InstitutionDetailsServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstitutionDetailsController : Controller
    {
        private readonly IInstitutionDetailsService _institutionDetailsService;
        private readonly ICommonService _ICommonService;
        public InstitutionDetailsController(IInstitutionDetailsService institutionDetailsService, ICommonService ICommonService)
        {
            _institutionDetailsService = institutionDetailsService;
            _ICommonService = ICommonService;
        }

        [HttpPost("GetInstitutionDetails")]
        public async Task<IActionResult> GetInstitutionDetails()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _institutionDetailsService.GetInstitutionDetail(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("AddInstitution")]
        public async Task<IActionResult> AddInstitution(AddInstitutionRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _institutionDetailsService.AddInstitutionAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateInstitution")]
        public async Task<IActionResult> UpdateInstitution(UpdateInstitutionRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _institutionDetailsService.UpdateInstitutionAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateInstitutionLogo")]
        public async Task<IActionResult> UpdateInstitutionLogo(UpdateInstitutionLogoRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _institutionDetailsService.UpdateInstitutionLogoAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateInstitutionFavicon")]
        public async Task<IActionResult> UpdateInstitutionFavicon(UpdateInstitutionFaviconRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _institutionDetailsService.UpdateInstitutionFaviconAsync(request, apiRequestDetails);

            return Ok(result);
        }
        [HttpPost("UpdateInstitutionLogoWithText")]
        public async Task<IActionResult> UpdateInstitutionLogoWithText(UpdateInstitutionLogoWithTextRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);

            var result = await _institutionDetailsService.UpdateInstitutionLogoWithTextAsync(request, apiRequestDetails);

            return Ok(result);
        }
    }
}
