using Models.CommonModels;
using Models.UserModels;
using Repository.UserRepository;
using Services.CommonServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly ICommonService _commonService;
        private readonly IUserRepo _userRepo;
        public UserService(IUserRepo IUserRepo, ICommonService ICommonService)
        {
            _commonService = ICommonService;
            _userRepo = IUserRepo;  
        }
        public async Task<CommonResponse<LoginResponse>> SMSLoginAsync(LoginRequestwithIP request)
        {
            var response = new CommonResponse<LoginResponse>();
            request.Password = await _commonService.Encrypt(request.Password);
            var result = await _userRepo.AdminLoginAsync(request);
            if (result.InstitutionCode == 0)
            {
                response.Status = Status.Failed;
                response.Message = "Invalid User Name or Password";
            }
            else
            {
                await _commonService.CreateJWTToken(response, result, request);
                
            }
            return response;
        }
    }
}
