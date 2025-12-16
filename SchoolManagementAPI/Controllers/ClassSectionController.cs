using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.ClassSectionModels;
using Services.ClassSectionServices;
using Services.CommonServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ClassSectionController : Controller
    {
        private readonly IClassSectionService _classSectionService;
        private readonly ICommonService _commonService;

        public ClassSectionController(IClassSectionService classSectionService, ICommonService commonService)
        {
            _classSectionService = classSectionService;
            _commonService = commonService;
        }
        [HttpPost("AddClassSection")]
        public async Task<IActionResult> AddClassSection(ClassSectionRequest request)
        {
            var apiRequestDetails = _commonService.GetAPIRequestDetails(User);
            var result = await _classSectionService.AddClassSectionAsync(request, apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("RemoveClassSection")]
        public async Task<IActionResult> RemoveClassSection(ClassSectionRequest request)
        {
            var apiRequestDetails = _commonService.GetAPIRequestDetails(User);
            var result = await _classSectionService.RemoveLastSectionAsync(request, apiRequestDetails);
            return Ok(result);
        }
    }
}
