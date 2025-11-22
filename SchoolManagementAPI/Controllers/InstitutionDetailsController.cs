using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.InstitutionDetailsServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstitutionDetailsController : Controller
    {
        private readonly IInstitutionDetailsService _institutionDetailsService;
        public InstitutionDetailsController(IInstitutionDetailsService institutionDetailsService)
        {
            _institutionDetailsService = institutionDetailsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInstitutionDetails()
        {
            var result = await _institutionDetailsService.GetInstitutionDetail(1);
            return Ok(result);
        }
    }
}
