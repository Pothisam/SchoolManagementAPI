using Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UserRepository
{
    public interface IUserRepo
    {
        Task<LoginResponse> AdminLoginAsync(LoginRequestwithIP login);
    }
}
