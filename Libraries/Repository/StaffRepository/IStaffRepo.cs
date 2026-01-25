using Models.CommonModels;
using Models.StaffModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StaffRepository
{
    public interface IStaffRepo
    {
        Task<Boolean> CheckDuplicate(StaffDetailsAddRequest request, APIRequestDetails apiRequestDetails);
        Task<int> AddStaffAsync(StaffDetail staff, StaffPassTable passTable);
        Task<List<AutoCompleteResponse>> GetStaffAutoComplete(StaffAutocompleteRequest request, APIRequestDetails apiRequestDetails);
        Task<string> GenerateStaffID(APIRequestDetails apiRequestDetails);
        Task<StaffDetailResponse> GetStaffDetailByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        #region Add Language
        Task AddStaffLanguageDetail(StaffLanguageDetail request);
        Task<List<StaffLanguageResponse>> GetStaffLanguageKnowByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task DeleteStaffLanguageAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Add Education
        Task AddStaffEducationDetail(StaffEducationDetail request);
        Task<List<StaffEducationResponse>> GetStaffEducationDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<Boolean> CheckDuplicateStaffEducation(StaffEducationDetailAdd request);
        Task DeleteStaffEducationAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task<Boolean> CheckDuplicateStaffEducationUpdate(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails);
        Task UpdateStaffEducationDetail(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Expirence
        Task AddStaffExperienceDetail(StaffExperience request);
        Task<List<StaffExperienceResponse>> GetStaffExperienceDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task DeleteStaffExperienceAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails);
        Task UpdateStaffExperienceDetailsAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Staff View List
        Task<StaffCountResponse> GetStaffCountAsync(APIRequestDetails apiRequestDetails);
        Task<List<DesignationListResponse>> GetStaffDesignationListAsync(APIRequestDetails apiRequestDetails);
        Task<List<StaffDetailSearchResponse>> GetStaffDetailSearchAsync(StaffSearchRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        Task<bool> UpdateStaffDetailsAsync(UpdateStaffDetailsRequest request, APIRequestDetails apiRequestDetails);

        Task<bool> ResetStaffPasswordAsync(StaffDetailsPasswordReset request, APIRequestDetails apiRequestDetails);
        Task<bool> UpdateStaffPasstableStatusAsync(int sysId, string status);
        Task<List<StaffNameAndSysidResponse>> GetStaffNameListAsync(APIRequestDetails apiRequestDetails);
    }
}
