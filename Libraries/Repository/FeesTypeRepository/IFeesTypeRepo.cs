using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.FeesTypeModels;
using Repository.Entity;

namespace Repository.FeesTypeRepository
{
    public interface IFeesTypeRepo
    {
        Task<List<FeesTypeListResponse>> GetFeesTypeListAsync(APIRequestDetails apiRequestDetails);
        Task<int> GetFeesTypeSysIdByNameAsync(SaveFeesTypeRequest request, APIRequestDetails apiRequestDetails);
        Task AddFeesTypeAsync(FeesType entity);
        Task<FeesType> GetFeesTypeBySysIdAsync(int sysId, APIRequestDetails apiRequestDetails);

        Task SaveChangesAsync();
        #region Gentrate Fees
        Task<List<StudentFeeGenerateStatusResponse>> GetFeesListViewAsync(GetFeesGentrationRequest request, APIRequestDetails apiRequestDetails);
        Task<int?> GetStudentClassDetailsSysIdAsync(int studentFkid, int academicYearFkid, int classSectionFkid, APIRequestDetails apiRequestDetails);
        Task<bool> IsFeesTransactionExistsAsync(int studentFkid,int feesTypeFkid,int studentClassDetailsFkid,string transationType,decimal debit,APIRequestDetails apiRequestDetails);
        Task<int> GetNextRefNoByGenerateDateAsync(DateTime generateDate,string transationType,APIRequestDetails apiRequestDetails);

        Task<string?> GetFeesTypeDescriptionAsync(int feesTypeFkid,APIRequestDetails apiRequestDetails);

        Task<bool> AddStudentFeesTransactionAsync(StudentFeesTransaction entity);

        #endregion
        #region Apporve Fees
        Task<List<StudentApproveFeesResponse>> GetApproveFeesAsync(APIRequestDetails apiRequestDetails);
        Task<List<ApproveFeesViewResponse>> GetApproveFeesViewAsync(GetApproveFeesViewRequest request, APIRequestDetails apiRequestDetails);
        Task<int> UpdateFeesApproveAsync(UpdateFeesApproveRequest request, APIRequestDetails apiRequestDetails);
        #endregion
    }
}
