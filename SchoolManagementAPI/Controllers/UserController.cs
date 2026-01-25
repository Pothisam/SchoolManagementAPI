using Microsoft.AspNetCore.Mvc;
using Models.UserModels;
using Services.CommonServices;
using Services.UserServices;

namespace SchoolManagementAPI.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _IUserServices;
        private readonly ICommonService _ICommonService;
        public UserController(IUserService IUserServices, ICommonService ICommonService)
        {
            _IUserServices = IUserServices;
            _ICommonService = ICommonService;
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
        [HttpPost("SMS/ChangePassword")]
        public async Task<IActionResult> ChangeAdminPassword(ChangePasswordRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IUserServices.ChangeAdminPasswordAsync(request, apiRequestDetails);
            return Ok(result);
        }
        #region Admin User
        [HttpPost("SMS/GetAdminUsers")]
        public async Task<IActionResult> GetAdminUsers()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IUserServices.GetAdminUsersAsync(apiRequestDetails);
            return Ok(result);
        }
        [HttpPost("SMS/AddOrUpdateAdminUser")]
        public async Task<IActionResult> AddOrUpdateAdminUser(AddAdminUserRequest request)
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IUserServices.AddOrUpdateAdminUserAsync(request, apiRequestDetails);
            return Ok(result);
        }

        [HttpPost("SMS/GetAccessSettings")]
        public async Task<IActionResult> GetAccessSettings()
        {
            var apiRequestDetails = _ICommonService.GetAPIRequestDetails(User);
            var result = await _IUserServices.GetSettingsByFIDAsync(apiRequestDetails);
            return Ok(result);
        }
        #endregion
    }
}
