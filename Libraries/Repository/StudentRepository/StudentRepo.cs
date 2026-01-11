using System.Collections;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
                if (request.SysId == 0)
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
        public async Task<List<AutoCompleteResponse>> GetStudentAutoComplete(AutoCompleteRequest request, APIRequestDetails apiRequestDetails)
        {
            var autoCompleteResponse = new List<AutoCompleteResponse>();
            if (request.TableName == "StudentDetails")
            {
                var query = request.ColumnName switch
                {
                    "PlaceOfBirth" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.PlaceOfBirth.StartsWith(request.SearchParam))
            .OrderBy(x => x.RollNo)
                        .Select(x => x.PlaceOfBirth)
            .Distinct()
                        .Take(5),

                    "MotherTongue" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.MotherTongue.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.MotherTongue)
                        .Distinct()
                        .Take(5),

                    "ExtraCurricularActivities" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.ExtraCurricularActivities.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.ExtraCurricularActivities)
                        .Distinct()
                        .Take(5),

                    "Caste" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Caste.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.Caste)
                        .Distinct()
                        .Take(5),


                    "BoardingPoint" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.BoardingPoint.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.BoardingPoint)
                        .Distinct()
                        .Take(5),

                    "Name" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Name.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.Name)
                        .Distinct()
                        .Take(5),

                    "RollNo" => _context.StudentMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.RollNo.StartsWith(request.SearchParam))
                        .OrderBy(x => x.RollNo)
                        .Select(x => x.RollNo)
                        .Distinct()
                        .Take(5),
                    "AadharCardNo" => _context.StudentMasterViews
                    .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.AadharCardNo.StartsWith(request.SearchParam))
                    .OrderBy(x => x.RollNo)
                    .Select(x => x.AadharCardNo)
                    .Distinct()
                    .Take(5),
                    _ => throw new NotImplementedException($"Column '{request.ColumnName}' is not supported.")
                };

                var result = await query.ToListAsync();
                autoCompleteResponse.AddRange(result.Select(value => new AutoCompleteResponse { Column = value }));
            }
            return autoCompleteResponse;
        }
        #endregion
        #region View Student List
        public async Task<StudentCountResponse> GetStudentCountAsync(APIRequestDetails apiRequestDetails)
        {
            return await (from x in _context.StudentMasterViews
                          where x.InstitutionCode == apiRequestDetails.InstitutionCode
                          group x by x.InstitutionCode into g
                          select new StudentCountResponse
                          {
                              TotalStudent = g.Count(),
                              Male = g.Count(x => x.Gender == "Male"),
                              Female = g.Count(x => x.Gender == "Female")
                          }).FirstOrDefaultAsync();
        }
        public async Task<List<StudentDetailsShortResponse>> GetStudentDetailsShortAsync(StudentShortRequest request, APIRequestDetails apiRequestDetails)
        {
            bool isDefaultRequest = request.CourseSysid <= 0;

            IQueryable<StudentMasterView> studentQuery = _context.StudentMasterViews
                .AsNoTracking()
                .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode);

            if (!isDefaultRequest)
            {
                studentQuery = studentQuery
                    .Where(x => x.ClassSysId == request.CourseSysid);
            }

            studentQuery = isDefaultRequest
                ? studentQuery.OrderByDescending(x => x.ModifiedDate).Take(10)
                : studentQuery.OrderBy(x => x.RollNo);

            IQueryable<DocumentLibrary> documentQuery = _context.DocumentLibraries
                .AsNoTracking()
                .Where(d =>
                    d.Action == "Image-Upload" &&
                    d.TableName == "StudentDetails" &&
                    d.FileSize > 0);

            return await (
                from s in studentQuery
                join d in documentQuery on s.Sysid equals d.Fkid into docGroup
                select new StudentDetailsShortResponse
                {
                    SysId = s.Sysid,
                    Name = s.Name,
                    CourseSection = s.ClassSection,
                    AcadamicYear = s.Year,
                    RollNo = s.RollNo,
                    DOB = s.Dob,
                    EnteredBy = s.EnteredBy,
                    EntryDate = s.Entrydate,
                    ModifiedBy = s.ModifiedBy,
                    ModifiedDate = s.ModifiedDate,
                    gender = s.Gender,
                    Guid = docGroup
                        .OrderByDescending(x => x.ModifiedDate)
                        .Select(x => (Guid?)x.Guid)
                        .FirstOrDefault()
                }
            ).ToListAsync();
        }
        public async Task<List<StudentDetailsShortResponse>> GetStudentDetailsShortAsync(StudentSearchRequest request, APIRequestDetails apiRequestDetails)
        {
            List<StudentDetailsShortResponse> StudentMasterViewList = new();
            IQueryable<StudentMasterView> query = _context.StudentMasterViews
                .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode);
            if (request.ColumnName == "AutoComplete")
            {
                query = query.Where(x => x.Name.StartsWith(request.SearchParam) || x.RollNo.StartsWith(request.SearchParam) || x.AadharCardNo.StartsWith(request.SearchParam));
            }
            else if (request.ColumnName == "Gender")
            {
                query = query.Where(x => x.Gender == request.SearchParam);
            }

            var documentQuery = _context.DocumentLibraries
                .Where(d => d.Action == "Image-Upload"
                            && d.TableName == "StudentDetails"
                            && d.FileSize != 0);
            StudentMasterViewList = await (from x in query
                                           join y in documentQuery on x.Sysid equals y.Fkid into documentGroup
                                           select new StudentDetailsShortResponse
                                           {
                                               SysId = x.Sysid,
                                               Name = x.Name,
                                               CourseSection = x.ClassSection,
                                               AcadamicYear = x.Year,
                                               RollNo = x.RollNo,
                                               DOB = x.Dob,
                                               EnteredBy = x.EnteredBy,
                                               EntryDate = x.Entrydate,
                                               ModifiedBy = x.ModifiedBy,
                                               ModifiedDate = x.ModifiedDate,
                                               gender = x.Gender,
                                               Guid = documentGroup
                                                   .OrderBy(d => d.ModifiedBy)
                                                   .Select(d => (Guid?)d.Guid)
                                                   .FirstOrDefault()
                                           }).ToListAsync();

            return StudentMasterViewList;
        }


        #endregion
        #region View Student Details
        public async Task<StudentMasterViewResponse> GetStudentDetailBySysid(StudentDetailsViewRequest request, APIRequestDetails apiRequestDetails)
        {
            StudentMasterViewResponse? StudentMasterView = await (from x in _context.StudentMasterViews 
                                                                  where x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Sysid == request.Sysid
                                                                  select new StudentMasterViewResponse
                                                                  {
                                                                      Sysid = x.Sysid,
                                                                      Stdid = x.Stdid,
                                                                      StudentType = x.StudentType,
                                                                      ApplicationNumber = x.ApplicationNumber,
                                                                      AdmissionNumber = x.AdmissionNumber,
                                                                      AdmissionSerialNumber = x.AdmissionSerialNumber,
                                                                      Initial = x.Initial,
                                                                      StudentName = x.StudentName,
                                                                      Name = x.Name,
                                                                      Dob = x.Dob,
                                                                      PlaceOfBirth = x.PlaceOfBirth,
                                                                      MotherTongue = x.MotherTongue,
                                                                      FatherName = x.FatherName,
                                                                      FatherOccupation = x.FatherOccupation,
                                                                      FatherIncome = x.FatherIncome,
                                                                      BloodGroup = x.BloodGroup,
                                                                      MotherName = x.MotherName,
                                                                      MotherOccupation = x.MotherOccupation,
                                                                      MotherIncome = x.MotherIncome,
                                                                      AadharCardNo = x.AadharCardNo,
                                                                      MobileNo = x.MobileNo,
                                                                      MobileNo2 = x.MobileNo2,
                                                                      Emailid = x.Emailid,
                                                                      FirstLanguage = x.FirstLanguage,
                                                                      Parents = x.Parents,
                                                                      Religion = x.Religion,
                                                                      Community = x.Community,
                                                                      Caste = x.Caste,
                                                                      GuardianName = x.GuardianName,
                                                                      ExtraCurricularActivities = x.ExtraCurricularActivities,
                                                                      PhysicalDisability = x.PhysicalDisability,
                                                                      Volunteers = x.Volunteers,
                                                                      Gender = x.Gender,
                                                                      ParmanentAddress1 = x.ParmanentAddress1,
                                                                      ParmanentAddress2 = x.ParmanentAddress2,
                                                                      ParmanentAddressPincode = x.ParmanentAddressPincode,
                                                                      ParmanentAddressPostOffice = x.ParmanentAddressPostOffice,
                                                                      ParmanentAddressDistrict = x.ParmanentAddressDistrict,
                                                                      ParmanentAddressState = x.ParmanentAddressState,
                                                                      CommunicationAddress1 = x.CommunicationAddress1,
                                                                      CommunicationAddress2 = x.CommunicationAddress2,
                                                                      CommunicationAddressPincode = x.CommunicationAddressPincode,
                                                                      CommunicationAddressPostOffice = x.CommunicationAddressPostOffice,
                                                                      CommunicationAddressDistrict = x.CommunicationAddressDistrict,
                                                                      CommunicationAddressState = x.CommunicationAddressState,
                                                                      DateOfAdmission = x.DateOfAdmission,
                                                                      BroSysStudyingStudied = x.BroSysStudyingStudied,
                                                                      NameBroSys = x.NameBroSys,
                                                                      ModeOftransport = x.ModeOftransport,
                                                                      BoardingPoint = x.BoardingPoint,
                                                                      Hostel = x.Hostel,
                                                                      Remark = x.Remark,
                                                                      InstitutionCode = x.InstitutionCode,
                                                                      Status = x.Status,
                                                                      EnteredBy = x.EnteredBy,
                                                                      Entrydate = x.Entrydate,
                                                                      ModifiedBy = x.ModifiedBy,
                                                                      ModifiedDate = x.ModifiedDate,
                                                                      Referredby = x.Referredby,
                                                                      DocumentEnclosed = x.DocumentEnclosed,
                                                                      DocumentNotEnclosed = x.DocumentNotEnclosed,
                                                                      RollNo = x.RollNo,
                                                                      ExamRegisterNumber = x.ExamRegisterNumber,
                                                                      AcademicYearSysId = x.AcademicYearSysId,
                                                                      Year = x.Year,
                                                                      ClassSysId = x.ClassSysId,
                                                                      Class = x.Class,
                                                                      ClassSectionSysId = x.ClassSectionSysId,
                                                                      Section = x.Section,
                                                                      ClassSection = x.ClassSection,
                                                                      Guid = x.Guid
                                                                  }).FirstOrDefaultAsync();
            return StudentMasterView ?? new StudentMasterViewResponse();
        }

        public async Task<StudentDetail?> GetStudentByIdAsync(UpdateStudentDetailRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.StudentDetails.FirstOrDefaultAsync(x =>x.SysId == request.Sysid && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }

        public async Task<bool> UpdateAsync(StudentDetail student, StudentClassDetail? classDetail)
        {
            try
            {
                _context.StudentDetails.Update(student);

                if (classDetail != null)
                {
                    _context.StudentClassDetails.Update(classDetail);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<StudentClassDetail?> GetActiveStudentClassAsync(UpdateStudentDetailRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.StudentClassDetails.FirstOrDefaultAsync(x => x.StudentDetailsFkid == request.Sysid && x.AcademicYearFkid == request.AcademicYearSysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }

        public async Task<bool> ResetStudentPasswordAsync(StudentPassword request, APIRequestDetails apiRequestDetails)
        {
            var studentLogin = await _context.StudentPassTables.SingleOrDefaultAsync(x => x.StudentDetailsFkid == request.Sysid
                                && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            if (studentLogin != null)
            {
                studentLogin.Password = request.Password;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
        #endregion
    }
}
