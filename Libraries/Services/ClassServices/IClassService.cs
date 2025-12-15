using Models.ClassModels;
using Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ClassServices
{
    public interface IClassService
    {
        Task<CommonResponse<string>> AddClassAsync(AddClassRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<ClassResponse>>> GetClassListAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateClassStatusAsync(UpdateClassStatusRequest request,APIRequestDetails apiRequestDetails);
    }
}
