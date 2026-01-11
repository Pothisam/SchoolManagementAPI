using System.Data;
using Models.CommonModels;
using Models.StudentModels;
using Repository.Entity;

namespace Repository.StudentRepository
{
    public interface IStudentRepo
    {
        #region Add Student
        Task<bool> IsDuplicateAadharAsync(string AadharCardNo, APIRequestDetails apiRequestDetails);
        Task<string> GenerateStudentIdAsync(APIRequestDetails apiRequestDetails);
        Task<int> AddStudent(StudentDetail request, StudentPassTable passTable, StudentClassDetail StudentClassDetails);
        Task<List<AutoCompleteResponse>> GetStudentAutoComplete(AutoCompleteRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region View Student List
        Task<StudentCountResponse> GetStudentCountAsync(APIRequestDetails apiRequestDetails);
        Task<List<StudentDetailsShortResponse>> GetStudentDetailsShortAsync(StudentShortRequest request, APIRequestDetails apiRequestDetails);
        Task<List<StudentDetailsShortResponse>> GetStudentDetailsShortAsync(StudentSearchRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region View Student Details
        Task<StudentMasterViewResponse> GetStudentDetailBySysid(StudentDetailsViewRequest request, APIRequestDetails apiRequestDetails);
        Task<StudentDetail?> GetStudentByIdAsync(UpdateStudentDetailRequest request, APIRequestDetails apiRequestDetails);
        Task<StudentClassDetail?> GetActiveStudentClassAsync(UpdateStudentDetailRequest request, APIRequestDetails apiRequestDetails);
        Task<bool> UpdateAsync(StudentDetail student, StudentClassDetail? classDetail);
        Task<bool> ResetStudentPasswordAsync(StudentPassword request, APIRequestDetails apiRequestDetails);
        #endregion
    }
}
