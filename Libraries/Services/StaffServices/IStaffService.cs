using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
using Models.StudentModels;
using Repository.Entity;

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
        Task<CommonResponse<StaffDetailsResponse>> GetStaffDetails(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        #region Add Language
        Task<CommonResponse<string>> AddStaffLanguageAsync(UpdateAddStaffLanguageDetail request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StaffLanguageResponse>>> GetStaffLanguageDetails(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteStaffLanguageAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Education
        Task<CommonResponse<string>> InsertStaffEducationDetailsAsync(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StaffEducationResponse>>> GetStaffEducationDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteStaffEducationAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateStaffEducationDetailsAsync(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails);

        #endregion
        #region Expirence 
        Task<CommonResponse<string>> AddStaffExperienceDetailAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StaffExperienceResponse>>> GetStaffExperienceDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteStaffExperienceAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateStaffExperienceDetailsAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Document
        Task<CommonResponse<string>> AddStaffDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<DocumentLibraryDetailsResponse>>> GetStaffDocumentAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteStafffDocumentAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateStaffDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails);
        #endregion
        Task<CommonResponse<string>> UpdateStaffDetailsAsync(UpdateStaffDetailsRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> ResetStaffPasswordAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        
    }
}
