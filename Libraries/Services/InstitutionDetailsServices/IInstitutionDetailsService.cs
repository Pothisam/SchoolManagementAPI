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
        Task<CommonResponse<InstitutionDetailsResponse>> GetInstitutionDetail(int InstitutionCode);
    }
}
