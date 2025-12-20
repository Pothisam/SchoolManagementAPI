using Models.CommonModels;
using Models.StaffModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StaffServices
{
    public interface IStaffService
    {
        Task<CommonResponse<string>> AddStaffAsync(StaffDetailsAddRequest request, List<StaffLanguageDetailAddRequest> languageRequests, List<StaffEducationDetailAddRequest> educationRequests, List<StaffExperienceDetailAddRequest> experienceRequests, List<DocumentLibraryBulkInsert> documnetRequests, APIRequestDetails apiRequestDetails);
        #region Add Language
        
        #endregion
        
    }
}
