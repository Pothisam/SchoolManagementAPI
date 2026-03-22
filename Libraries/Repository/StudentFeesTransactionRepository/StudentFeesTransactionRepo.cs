using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.StudentFeesTransactionModels;
using Repository.Entity;

namespace Repository.StudentFeesTransactionRepository
{
    public class StudentFeesTransactionRepo : IStudentFeesTransactionRepo
    {
        private readonly SchoolManagementContext _context;
        public StudentFeesTransactionRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<List<StudentFeesTransactionResponse>> GetFeesList(StudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            var documentQuery = _context.DocumentLibraries
                .Where(d => d.Action == "Image-Upload"
                            && d.TableName == "StudentDetails"
                            && d.FileSize != 0);
            var result = await (from sft in _context.StudentFeesTransactions

                                join scd in _context.StudentClassDetails
                                    on sft.StudentClassDetailsFkid equals scd.SysId into scdJoin
                                from scd in scdJoin.DefaultIfEmpty()

                                join sd in _context.StudentDetails
                                    on scd.StudentDetailsFkid equals sd.SysId into sdJoin
                                from sd in sdJoin.DefaultIfEmpty()

                                join cs in _context.ClassSections
                                    on scd.ClassSectionFkid equals cs.SysId

                                join c in _context.Classes
                                    on cs.ClassFkid equals c.SysId

                                join ay in _context.AcademicYears
                                    on scd.AcademicYearFkid equals ay.SysId
                                join y in documentQuery on sd.SysId equals y.Fkid into documentGroup
                                where ay.SysId == request.Batch
                                      && c.SysId == request.Class
                                      && cs.SysId == request.Section
                                      && sft.InstitutionCode == apiRequestDetails.InstitutionCode
                                      && sft.Status == "Approved"

                                group new { sft, sd, c, cs, ay, scd } by new
                                {
                                    sd.SysId,
                                    sd.Stdid,
                                    scd.RollNo,
                                    sd.Name,
                                    sd.Initial,
                                    c.ClassName,
                                    cs.SectionName,
                                    ay.Year,
                                    AcademicYearSysId = ay.SysId,
                                    CSSysid =scd.SysId

                                }
                                into g
                                select new StudentFeesTransactionResponse
                                {
                                    SysId = g.Key.SysId,
                                    Stdid = g.Key.Stdid,
                                    rollno = g.Key.RollNo,
                                    Name = g.Key.Name + " " + g.Key.Initial,

                                    Initial = g.Key.Initial,
                                    ClassName = g.Key.ClassName + " (" + g.Key.SectionName + ")",
                                    SectionName = g.Key.SectionName,
                                    Year = g.Key.Year,
                                    ClassSectionSysId = g.Key.CSSysid,
                                    Debit = g.Sum(x => x.sft.Debit),
                                    Credit = g.Sum(x => x.sft.Credit),
                                    Balance = g.Sum(x => x.sft.Debit) - g.Sum(x => x.sft.Credit),
                                    AcadamicYear = g.Key.AcademicYearSysId,
                                    Guid = documentQuery
                                                   .OrderBy(d => d.ModifiedBy)
                                                   .Select(d => (Guid?)d.Guid)
                                                   .FirstOrDefault()
                                }).ToListAsync();

            return result;
        }
        public async Task<List<StudentFeesTransactionResponse>> GetFeesDetailsBtNameList(StudentFeesTransactionByNameRequest request, APIRequestDetails apiRequestDetails)
        {
            var documentQuery = _context.DocumentLibraries
                .Where(d => d.Action == "Image-Upload"
                            && d.TableName == "StudentDetails"
                            && d.FileSize != 0);

            var result = await (from sft in _context.StudentFeesTransactions

                                join scd in _context.StudentClassDetails
                                    on sft.StudentClassDetailsFkid equals scd.SysId into scdJoin
                                from scd in scdJoin.DefaultIfEmpty()

                                join sd in _context.StudentDetails
                                    on scd.StudentDetailsFkid equals sd.SysId into sdJoin
                                from sd in sdJoin.DefaultIfEmpty()

                                join cs in _context.ClassSections
                                    on scd.ClassSectionFkid equals cs.SysId

                                join c in _context.Classes
                                    on cs.ClassFkid equals c.SysId

                                join ay in _context.AcademicYears
                                    on scd.AcademicYearFkid equals ay.SysId
                                join y in documentQuery on sd.SysId equals y.Fkid into documentGroup
                                where
                                 sft.Status == "Approved" &&
             sft.InstitutionCode == apiRequestDetails.InstitutionCode &&
             (
                 scd.RollNo.StartsWith(request.StudentName) ||
                 (sd.Name + " " + sd.Initial).StartsWith(request.StudentName) ||
                 sd.Stdid.StartsWith(request.StudentName)
             )

                                group new { sft, sd, c, cs, ay, scd } by new
                                {
                                    sd.SysId,
                                    sd.Stdid,
                                    scd.RollNo,
                                    sd.Name,
                                    sd.Initial,
                                    c.ClassName,
                                    cs.SectionName,
                                    ay.Year,
                                    AcademicYearSysId = ay.SysId,
                                    CSSysid = scd.SysId
                                }
                                into g
                                select new StudentFeesTransactionResponse
                                {
                                    SysId = g.Key.SysId,
                                    Stdid = g.Key.Stdid,
                                    rollno = g.Key.RollNo,
                                    Name = g.Key.Name + " " + g.Key.Initial,

                                    Initial = g.Key.Initial,
                                    ClassName = g.Key.ClassName + " (" + g.Key.SectionName + ")",
                                    Year = g.Key.Year,
                                    Debit = g.Sum(x => x.sft.Debit),
                                    Credit = g.Sum(x => x.sft.Credit),
                                    Balance = g.Sum(x => x.sft.Debit) - g.Sum(x => x.sft.Credit),
                                    AcadamicYear = g.Key.AcademicYearSysId,
                                    ClassSectionSysId = g.Key.CSSysid,
                                    Guid = documentQuery
                                                   .OrderBy(d => d.ModifiedBy)
                                                   .Select(d => (Guid?)d.Guid)
                                                   .FirstOrDefault()
                                }).ToListAsync();
            return result;
        }

        public async Task<GetDebitResponse> GetDebitAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails)
        {
            List<string> statusList = new List<string> { "Approved", "Deleted", "Reject" };

            List<GetDebitItemResponse> r1 = await (
                from sft in _context.StudentFeesTransactions

                join scd in _context.StudentClassDetails
                    on sft.StudentClassDetailsFkid equals scd.SysId into scdJoin
                from scd in scdJoin.DefaultIfEmpty()

                join ay in _context.AcademicYears
                    on scd.AcademicYearFkid equals ay.SysId into ayJoin
                from ay in ayJoin.DefaultIfEmpty()

                where sft.StudentFkid == request.SysId
                      && ay.SysId == request.Batch
                      && sft.TransationType == "DR"
                      && sft.InstitutionCode == apiRequestDetails.InstitutionCode
                      && statusList.Contains(sft.Status)

                orderby sft.EntryDate

                select new GetDebitItemResponse
                {
                    SysId = sft.SysId,
                    EntryDate = sft.EntryDate,
                    Description = sft.Description,
                    Debit = sft.Debit,
                    Status = sft.Status,
                    EntryBy = sft.EntryBy,
                    ModifiedBy = sft.ModifiedBy,
                    ModifiedDate = sft.ModifiedDate,
                    GenerateDate = sft.GenerateDate,
                    FeesId = sft.FeesTypeFkid
                }
            ).ToListAsync();

            decimal r2 = r1.Where(x => x.Status == "Approved").Sum(x => x.Debit);

            return new GetDebitResponse
            {
                R1 = r1,
                R2 = r2
            };
        }

        public async Task<GetCreditResponse> GetCreditAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails)
        {
            List<string> statusList = new List<string> { "Approved", "Deleted", "Reject" };

            List<GetCreditItemResponse> r1 = await (
                from sft in _context.StudentFeesTransactions

                join scd in _context.StudentClassDetails
                    on sft.StudentClassDetailsFkid equals scd.SysId into scdJoin
                from scd in scdJoin.DefaultIfEmpty()

                join ay in _context.AcademicYears
                    on scd.AcademicYearFkid equals ay.SysId into ayJoin
                from ay in ayJoin.DefaultIfEmpty()

                where sft.StudentFkid == request.SysId
                      && ay.SysId == request.Batch
                      && sft.TransationType == "CR"
                      && sft.InstitutionCode == apiRequestDetails.InstitutionCode
                      && statusList.Contains(sft.Status)

                orderby sft.EntryDate

                select new GetCreditItemResponse
                {
                    SysId = sft.SysId,
                    EntryDate = sft.EntryDate,
                    Description = sft.Description,
                    Credit = sft.Credit,
                    Status = sft.Status,
                    EntryBy = sft.EntryBy,
                    ModifiedBy = sft.ModifiedBy,
                    ModifiedDate = sft.ModifiedDate,
                    GenerateDate = sft.GenerateDate,
                    FeesId = sft.FeesTypeFkid
                }
            ).ToListAsync();

            decimal r2 = r1.Where(x => x.Status == "Approved").Sum(x => x.Credit);

            return new GetCreditResponse
            {
                R1 = r1,
                R2 = r2
            };
        }
        #region Fees Add

        public async Task<StudentFeeBalanceDto?> GetApprovedDebitCreditAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            return await (
                from x in _context.StudentFeesTransactions
                join y in _context.StudentClassDetails
                    on x.StudentClassDetailsFkid equals y.SysId
                where x.StudentFkid == request.StudentFkid
                      && x.FeesTypeFkid == request.FeesTypeFkid
                      && x.StudentClassDetailsFkid == request.StudentClassDetailsFkid
                      && x.Status == "Approved"
                      && x.InstitutionCode == apiRequestDetails.InstitutionCode
                group x by new { x.StudentFkid, x.FeesTypeFkid } into g
                select new StudentFeeBalanceDto
                {
                    Debit = g.Sum(s => s.Debit),
                    Credit = g.Sum(s => s.Credit)
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<int> GetNextRefNoAsync(string finYear, APIRequestDetails apiRequestDetails)
        {
            int count = await _context.StudentFeesTransactions.Where(x => x.TransationType == "CR" && x.InstitutionCode == apiRequestDetails.InstitutionCode &&
              (
                 (x.GenerateDate.Month >= 4
                ? x.GenerateDate.Year.ToString() + "-" + (x.GenerateDate.Year + 1).ToString()
                : (x.GenerateDate.Year - 1).ToString() + "-" + x.GenerateDate.Year.ToString()
            ) == finYear)).CountAsync();

            return count + 1;
        }

        public async Task<StudentDetailInfoDto?> GetStudentDetailInfoAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            return await (
                from x in _context.StudentDetails
                join y in _context.StudentClassDetails
                    on x.SysId equals y.StudentDetailsFkid
                where x.SysId == request.StudentFkid && y.SysId == request.StudentClassDetailsFkid && x.InstitutionCode == apiRequestDetails.InstitutionCode
                select new StudentDetailInfoDto
                {
                    StudentFkid = x.SysId,
                    StudentId = x.Stdid,
                    StudentName = x.Name + " " + x.Initial,
                    ClassSectionSysId = y.SysId
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<FeesType?> GetFeesTypeByIdAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.FeesTypes
                .FirstOrDefaultAsync(x => x.Sysid == request.FeesTypeFkid && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }

        public async Task<bool> ChequeNumberExistsAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.StudentFeesTransactions
                .AnyAsync(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.ChequeNo == request.ChequeNo.Trim().ToUpper());
        }

        public async Task<int> AddStudentFeesTransactionAsync(StudentFeesTransaction entity)
        {
            _context.StudentFeesTransactions.Add(entity);
            await _context.SaveChangesAsync();
            return entity.SysId;
        }
        #endregion
    }
}
