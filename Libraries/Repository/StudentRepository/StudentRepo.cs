using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.CommonModels;
using Models.StudentModels;
using Repository.Entity;


namespace Repository.StudentRepository
{
    public class StudentRepo : IStudentRepo
    {
        private readonly SchoolManagementContext _context;
        public StudentRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        #region Add Student
        public async Task<bool> IsDuplicateAadharAsync(string AadharCardNo, APIRequestDetails apiRequestDetails)
        {
            var isDuplicate = await _context.StudentDetails
            .AnyAsync(x => x.AadharCardNo == AadharCardNo && x.InstitutionCode == apiRequestDetails.InstitutionCode);
            return !isDuplicate;
        }
        public async Task<string> GenerateStudentIdAsync(APIRequestDetails apiRequestDetails)
        {
            DateTime currentDate = DateTime.Now;
            int studentCount = await _context.StudentDetails
            .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.EntryDate.Year == currentDate.Year)
            .CountAsync();

            // Increment the count for the new student
            int newStudentNumber = studentCount + 1;

            // Format the count as a zero-padded string
            string paddedCount = newStudentNumber.ToString("D5"); // Pads to 5 digits (e.g., 00001)

            // Combine year, institution code, and padded count to create the Student ID
            string studentId = $"{currentDate.Year}{apiRequestDetails.InstitutionCode}{paddedCount}";

            return studentId;
        }
        public async Task<int> AddStudent(StudentDetail request, StudentPassTable passTable, StudentClassDetail StudentClassDetails)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.StudentDetails.Add(request);
                await _context.SaveChangesAsync();
                if(request.SysId == 0)
                {
                    return 0;
                }
                passTable.StudentDetailsFkid = request.SysId;
                _context.StudentPassTables.Add(passTable);
                await _context.SaveChangesAsync();

                StudentClassDetails.StudentDetailsFkid = request.SysId;
                _context.StudentClassDetails.Add(StudentClassDetails);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return request.SysId;
            }
            catch (DbUpdateException ex)
            {
                var error = ex.InnerException?.Message;
                await transaction.RollbackAsync();
                return 0;
            }
        }
        #endregion

    }
}
