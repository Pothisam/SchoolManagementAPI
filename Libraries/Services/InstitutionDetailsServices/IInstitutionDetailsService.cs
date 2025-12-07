using Models.CommonModels;
using Models.InstitutionDetailsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.InstitutionDetailsServices
{
    public interface IInstitutionDetailsService
    {
        Task<CommonResponse<InstitutionDetailsResponse>> GetInstitutionDetail(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> AddInstitutionAsync(AddInstitutionRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateInstitutionAsync(UpdateInstitutionRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateInstitutionLogoAsync(UpdateInstitutionLogoRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateInstitutionFaviconAsync(UpdateInstitutionFaviconRequest request,APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateInstitutionLogoWithTextAsync(UpdateInstitutionLogoWithTextRequest request,APIRequestDetails apiRequestDetails);
    }
}
