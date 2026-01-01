using Models.CommonModels;
using Models.StudentModels;

namespace Services.StudentServices
{
    public interface IStudentService
    {
        #region Add Student
        Task<CommonResponse<string>> AddStudent(AddStudentRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<AutoCompleteResponse>>> GetStudentAutoComplete(AutoCompleteRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region View Student List
        Task<CommonResponse<StudentCountResponse>> GetStudentCountAsync(APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StudentDetailsShortResponse>>> GetStudentDetailsShortAsync(StudentShortRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StudentDetailsShortResponse>>> GetStudentDetailsShortAsync(StudentSearchRequest request, APIRequestDetails apiRequestDetails);
        #endregion
    }
}
