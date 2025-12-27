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
        #endregion
    }
}
