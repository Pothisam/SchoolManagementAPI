using Models.InstitutionDetailsModels;
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
    }
}
