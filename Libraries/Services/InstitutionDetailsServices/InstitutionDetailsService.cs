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
        public async Task<CommonResponse<InstitutionDetailsResponse>> GetInstitutionDetail(int InstitutionCode)
        {
            var response = new CommonResponse<InstitutionDetailsResponse>();
            var result = await _institutionDetailsRepo.GetInstitutionDetail(InstitutionCode);
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
    }
}
