using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.FeesTypeModels;
using Repository.Entity;
using static System.Collections.Specialized.BitVector32;

namespace Repository.FeesTypeRepository
{
    public class FeesTypeRepo : IFeesTypeRepo
    {
        private readonly SchoolManagementContext _context;
        public FeesTypeRepo(SchoolManagementContext context)
        {
            _context = context;
        }

        public async Task<List<FeesTypeListResponse>> GetFeesTypeListAsync(APIRequestDetails apiRequestDetails)
        {
            var result = await (from x in _context.FeesTypes
                                where x.InstitutionCode == apiRequestDetails.InstitutionCode
                                      && x.Status == "Active"
                                select new FeesTypeListResponse
                                {
                                    Sysid = x.Sysid,
                                    FeesDescription = x.FeesDescription,
                                    status = x.Status,
                                    Entryby = x.Entryby,
                                    EntryDate = x.EntryDate,
                                    Modifiedby = x.Modifiedby,
                                    ModifiedDate = x.ModifiedDate
                                }).ToListAsync();

            return result;
        }

        public async Task<int> GetFeesTypeSysIdByNameAsync(SaveFeesTypeRequest request, APIRequestDetails apiRequestDetails)
        {
            return await (from x in _context.FeesTypes
                          where x.InstitutionCode == apiRequestDetails.InstitutionCode
                                && x.FeesDescription == request.FeesTypeDescription
                          select x.Sysid).FirstOrDefaultAsync();
        }
        public async Task AddFeesTypeAsync(FeesType entity)
        {
            await _context.FeesTypes.AddAsync(entity);
        }

        public async Task<FeesType> GetFeesTypeBySysIdAsync(int sysId, APIRequestDetails apiRequestDetails)
        {
            return await _context.FeesTypes.SingleOrDefaultAsync(x => x.Sysid == sysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #region Gentrate Fees
        public async Task<List<StudentFeeGenerateStatusResponse>> GetFeesListViewAsync(GetFeesGentrationRequest request,APIRequestDetails apiRequestDetails)
        {
            // Step 1: Filter + dedupe key: take Max(SysId) per StudentDetailsFkid
            var scdKeys =
                from scd in _context.StudentClassDetails
                where scd.InstitutionCode == apiRequestDetails.InstitutionCode
                      && scd.AcademicYearFkid == request.acadamicYear
                      && scd.ClassSectionFkid == request.sectionfkid
                      && scd.Status == "Active"
                group scd by new { scd.StudentDetailsFkid, scd.InstitutionCode } into g
                select new
                {
                    g.Key.StudentDetailsFkid,
                    g.Key.InstitutionCode,
                    StudentClassDetailsSysId = g.Max(x => x.SysId)
                };

            // Step 2: Join back to StudentClassDetails using Max(SysId) to get the actual row
            var query =
                from k in scdKeys
                join scd in _context.StudentClassDetails
                    on new { SysId = k.StudentClassDetailsSysId, k.InstitutionCode }
                    equals new { SysId = scd.SysId, scd.InstitutionCode }

                join smw in _context.StudentMasterViews
                    on new { Sysid = scd.StudentDetailsFkid, scd.InstitutionCode }
                    equals new { Sysid = smw.Sysid, smw.InstitutionCode }

                join ay in _context.AcademicYears
                    on new { SysId = scd.AcademicYearFkid, scd.InstitutionCode }
                    equals new { ay.SysId, ay.InstitutionCode }

                join cs in _context.ClassSections
                    on new { SysId = scd.ClassSectionFkid, scd.InstitutionCode }
                    equals new { cs.SysId, cs.InstitutionCode }

                join c in _context.Classes
                    on new { SysId = cs.ClassFkid, cs.InstitutionCode }
                    equals new { c.SysId, c.InstitutionCode }

                    // LEFT JOIN StudentFeesTransactions (IMPORTANT: StudentClassDetailsFKID = scd.SysId)
                join sft0 in _context.StudentFeesTransactions
                    on new
                    {
                        StudentFKID = scd.StudentDetailsFkid,
                        StudentClassDetailsFKID = scd.SysId,
                        FeesTypeFKID = request.feestypefkid,
                        scd.InstitutionCode
                    }
                    equals new
                    {
                        StudentFKID = sft0.StudentFkid,
                        StudentClassDetailsFKID = sft0.StudentClassDetailsFkid,
                        FeesTypeFKID = sft0.FeesTypeFkid,
                        sft0.InstitutionCode
                    }
                    into sftGroup
                from sft in sftGroup.DefaultIfEmpty()

                    // keep if you want to ensure section belongs to requested class
                where c.SysId == request.classfkid

                select new StudentFeeGenerateStatusResponse
                {
                    Sysid = smw.Sysid,
                    StudentName = smw.StudentName,
                    Stdid = smw.Stdid,
                    ClassName = c.ClassName,
                    SectionName = cs.SectionName,
                    Hostel = smw.Hostel,
                    Year = ay.Year,
                    Debit = sft != null ? ((decimal?)sft.Debit ?? request.amount) : request.amount,
                    Status = sft != null ? "Generated" : "Not Generated"
                };

            return await query.ToListAsync();
        }

        public async Task<int?> GetStudentClassDetailsSysIdAsync(int studentFkid, int academicYearFkid, int classSectionFkid, APIRequestDetails apiRequestDetails)
        {
            return await _context.StudentClassDetails
                .AsNoTracking()
                .Where(x =>
                    x.StudentDetailsFkid == studentFkid &&
                    x.AcademicYearFkid == academicYearFkid &&
                    x.ClassSectionFkid == classSectionFkid &&
                    x.InstitutionCode == apiRequestDetails.InstitutionCode &&
                    x.Status == "Active")
                .Select(x => (int?)x.SysId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsFeesTransactionExistsAsync(int studentFkid, int feesTypeFkid, int studentClassDetailsFkid, string transationType, decimal debit, APIRequestDetails apiRequestDetails)
        {
            return await _context.StudentFeesTransactions
                .AsNoTracking()
                .AnyAsync(x =>
                    x.InstitutionCode == apiRequestDetails.InstitutionCode &&
                    x.StudentFkid == studentFkid &&
                    x.FeesTypeFkid == feesTypeFkid &&
                    x.StudentClassDetailsFkid == studentClassDetailsFkid &&
                    x.TransationType == transationType &&
                    x.Debit == debit &&
                    x.Status != "Deleted");
        }

        public async Task<int> GetNextRefNoByGenerateDateAsync(DateTime generateDate, string transationType, APIRequestDetails apiRequestDetails)
        {
            var fyStart = GetFinancialYearStartDate(generateDate);
            var fyEndExclusive = fyStart.AddYears(1);

            var maxRef = await _context.StudentFeesTransactions
                .AsNoTracking()
                .Where(x =>
                    x.InstitutionCode == apiRequestDetails.InstitutionCode &&
                    x.TransationType == transationType &&
                    x.GenerateDate >= fyStart &&
                    x.GenerateDate < fyEndExclusive &&
                    x.Status != "Deleted")
                .MaxAsync(x => (int?)x.RefNo);

            return (maxRef ?? 0) + 1;
        }
        private static DateTime GetFinancialYearStartDate(DateTime date)
        {
            // India FY: Apr 1 - Mar 31
            int startYear = date.Month >= 4 ? date.Year : date.Year - 1;
            return new DateTime(startYear, 4, 1);
        }
        public async Task<string?> GetFeesTypeDescriptionAsync(int feesTypeFkid, APIRequestDetails apiRequestDetails)
        {
            return await _context.FeesTypes
                 .AsNoTracking()
                 .Where(x =>
                     x.Sysid == feesTypeFkid &&
                     x.InstitutionCode == apiRequestDetails.InstitutionCode)
                 .Select(x => x.FeesDescription)
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> AddStudentFeesTransactionAsync(StudentFeesTransaction entity)
        {
            try
            {
                _context.StudentFeesTransactions.Add(entity);
                await _context.SaveChangesAsync();
                return entity.SysId > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }
}
