using Models.AcademicYearModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AcademicYearServices
{
    public interface IAcademicYearService
    {
        Task<CommonResponse<string>> AddAcademicYearAsync(AddAcademicYearRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<AcademicYearResponse>>> GetAcademicYearListAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateAcademicYearStatusAsync(UpdateAcademicYear request, APIRequestDetails apiRequestDetails);
    }
}
