using Models.CommonModels;
using Models.InstitutionDetailsModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InstitutionDetails
{
    public interface IInstitutionDetailsRepo
    {
        Task<InstitutionDetailsResponse> GetInstitutionDetail(int InstitutionCode);
        Task<InstitutionDetail?> GetInstitutionByNameAsync(string institutionName, int institutionCode);

        Task<bool> AddInstitutionAsync(AddInstitutionRequest request,APIRequestDetails apiRequestDetails);
        Task<InstitutionDetail?> GetInstitutionByIdAsync(int sysid);

        Task<bool> UpdateInstitutionAsync(UpdateInstitutionRequest request,APIRequestDetails apiRequestDetails);
        Task<bool> UpdateInstitutionLogoAsync(UpdateInstitutionLogoRequest request,APIRequestDetails apiRequestDetails);
        Task<bool> UpdateInstitutionFaviconAsync(UpdateInstitutionFaviconRequest request,APIRequestDetails apiRequestDetails);
        Task<bool> UpdateInstitutionLogoWithTextAsync(UpdateInstitutionLogoWithTextRequest request,APIRequestDetails apiRequestDetails);
    }
}
