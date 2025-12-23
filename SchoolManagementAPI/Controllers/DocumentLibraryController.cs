using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DocumentLibraryModels;
using SchoolManagementAPI.Attributes;
using Services.CommonServices;
using Services.DocumentLibraryServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Authorize]
    [RequireIpValidation]
    [Route("[controller]")]
    public class DocumentLibraryController : Controller
    {
        private readonly IDocumentLibraryServices _IDocumentLibraryServices;
        private readonly ICommonService _ICommonService;
        public DocumentLibraryController(IDocumentLibraryServices DocumentLibraryServices, ICommonService iCommonService)
        {
            _IDocumentLibraryServices = DocumentLibraryServices;
            _ICommonService = iCommonService;
        }
        [HttpPost("GetProfileImagebyGuid")]
        public async Task<IActionResult> GetProfileImagebyGuid(DocumentLibraryGuid request)
        {
            var apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IDocumentLibraryServices.GetProfileImagebyID(request, apirequestdetails);
            return Ok(result);
        }
    }
}
