using Models.AcademicYearModels;
using Models.CommonModels;
using Repository.AcademicYearRepository;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AcademicYearServices
{
    public class AcademicYearService : IAcademicYearService
    {
        private readonly IAcademicYearRepo _academicYearRepo;

        public AcademicYearService(IAcademicYearRepo academicYearRepo)
        {
            _academicYearRepo = academicYearRepo;
        }
        public async Task<CommonResponse<string>> AddAcademicYearAsync(AddAcademicYearRequest request, APIRequestDetails apiRequestDetails)
        {
            var exists = await _academicYearRepo
            .IsAcademicYearExistsAsync(request, apiRequestDetails);

            if (exists)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Academic year already exists"
                };
            }

            var result = await _academicYearRepo.AddAcademicYearAsync(
                request,
                apiRequestDetails
            );

            return new CommonResponse<string>
            {
                Status = result ? Status.Success : Status.Failed,
                Message = result
                    ? "Academic year added successfully"
                    : "Unable to add academic year"
            };
        }

        public async Task<CommonResponse<List<AcademicYearResponse>>> GetAcademicYearListAsync(APIRequestDetails apiRequestDetails)
        {
            var result = await _academicYearRepo.GetAcademicYearListAsync(apiRequestDetails);

            return new CommonResponse<List<AcademicYearResponse>>
            {
                Status = Status.Success,
                Data = result
            };
        }
    }
}
