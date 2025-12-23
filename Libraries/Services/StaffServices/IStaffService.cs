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
        Task<CommonResponse<List<AutoCompleteResponse>>> GetStaffAutoComplete(StaffAutocompleteRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> AddStaffAsync(StaffDetailsAddRequest request, List<StaffLanguageDetailAddRequest> languageRequests, List<StaffEducationDetailAddRequest> educationRequests, List<StaffExperienceDetailAddRequest> experienceRequests, List<DocumentLibraryBulkInsert> documnetRequests, APIRequestDetails apiRequestDetails);


        #region Staff View List
        Task<CommonResponse<StaffCountResponse>> GetStaffCountAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<DesignationListResponse>>> GetStaffDesignationListAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StaffDetailSearchResponse>>> GetStaffDetailSearchAsync(StaffSearchRequest request, APIRequestDetails apiRequestDetails);
        #endregion

    }
}
