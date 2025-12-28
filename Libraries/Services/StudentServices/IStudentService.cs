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
    }
}
