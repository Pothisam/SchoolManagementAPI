using Models.AcademicYearModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AcademicYearRepository
{
    public interface IAcademicYearRepo
    {
        Task<bool> IsAcademicYearExistsAsync(AddAcademicYearRequest request, APIRequestDetails apiRequestDetails);
        Task<bool> AddAcademicYearAsync(AddAcademicYearRequest request,APIRequestDetails apiRequestDetails);
        Task<List<AcademicYearResponse>> GetAcademicYearListAsync(APIRequestDetails apiRequestDetails);
        Task<List<AcademicYearResponse>>  GetActiveAcademicYearListAsync(APIRequestDetails apiRequestDetails);
        Task<bool> UpdateAcademicYearStatusAsync(UpdateAcademicYear request, APIRequestDetails apiRequestDetails);
    }
}
