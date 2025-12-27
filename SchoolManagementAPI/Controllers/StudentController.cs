

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        #endregion
    }
}
