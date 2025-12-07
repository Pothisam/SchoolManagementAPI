using Models.CommonModels;
using Models.InstitutionDetailsModels;
using Repository.InstitutionDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.InstitutionDetailsServices
{
    public class InstitutionDetailsService : IInstitutionDetailsService
    {
        private readonly IInstitutionDetailsRepo _institutionDetailsRepo;
        public InstitutionDetailsService(IInstitutionDetailsRepo IInstitutionDetailsRepo)
        {
            _institutionDetailsRepo = IInstitutionDetailsRepo;
        }

        public async Task<CommonResponse<string>> AddInstitutionAsync(AddInstitutionRequest request, APIRequestDetails apiRequestDetails)
        {
            var existing = await _institutionDetailsRepo.GetInstitutionByNameAsync(
            request.InstitutionName!,
            apiRequestDetails.InstitutionCode);
            if (existing != null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Institution already exists"
                };
                
            }
            var result = await _institutionDetailsRepo.AddInstitutionAsync(request, apiRequestDetails);

            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Institution added successfully"
            };
        }

        public async Task<CommonResponse<InstitutionDetailsResponse>> GetInstitutionDetail(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<InstitutionDetailsResponse>();
            var result = await _institutionDetailsRepo.GetInstitutionDetail(apiRequestDetails.InstitutionCode);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }

        public async Task<CommonResponse<string>> UpdateInstitutionAsync(UpdateInstitutionRequest request, APIRequestDetails apiRequestDetails)
        {
            var existing = await _institutionDetailsRepo.GetInstitutionByIdAsync(request.Sysid);

            if (existing == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Institution not found"
                };
            }
            var result = await _institutionDetailsRepo.UpdateInstitutionAsync(request, apiRequestDetails);

            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Institution details updated successfully"
            };
        }

       
        public async Task<CommonResponse<string>> UpdateInstitutionLogoAsync(UpdateInstitutionLogoRequest request, APIRequestDetails apiRequestDetails)
        {
            var existing = await _institutionDetailsRepo.GetInstitutionByIdAsync(request.Sysid);

            if (existing == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Institution not found"
                };
            }

            var result = await _institutionDetailsRepo.UpdateInstitutionLogoAsync(
                request,
                apiRequestDetails
            );

            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Institution logo updated successfully"
            };
        }
        public async Task<CommonResponse<string>> UpdateInstitutionFaviconAsync(UpdateInstitutionFaviconRequest request, APIRequestDetails apiRequestDetails)
        {
            var existing = await _institutionDetailsRepo.GetInstitutionByIdAsync(request.Sysid);

            if (existing == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Institution not found"
                };
            }

            bool updated = await _institutionDetailsRepo.UpdateInstitutionFaviconAsync(request, apiRequestDetails);
            if (!updated)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Unable to update favicon"
                };
            }

            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Favicon updated successfully"
            };
        }

        public async Task<CommonResponse<string>> UpdateInstitutionLogoWithTextAsync(UpdateInstitutionLogoWithTextRequest request, APIRequestDetails apiRequestDetails)
        {
            var existing = await _institutionDetailsRepo.GetInstitutionByIdAsync(request.Sysid);

            if (existing == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Institution not found"
                };
            }

            bool updated = await _institutionDetailsRepo.UpdateInstitutionLogoWithTextAsync(request, apiRequestDetails);

            if (!updated)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Unable to update logo with text"
                };
            }

            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Logo with text updated successfully"
            };
        }
    }
}
