using Microsoft.AspNetCore.Mvc;
using Models.UserModels;
using Services.UserServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _IUserServices;
        public UserController(IUserService IUserServices)
        {
            _IUserServices = IUserServices;
        }
        [HttpPost("SMS/login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var requestWithIP = new LoginRequestwithIP
            {
                UserName = request.UserName,
                Password = request.Password,
                IPAddress = ipAddress
            };
            var response = await _IUserServices.SMSLoginAsync(requestWithIP);
            return Ok(response);
        }
    }
}
