using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.DashboardModels;
using Repository.Entity;

namespace Repository.DashboardRepository
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly SchoolManagementContext _context;

        public DashboardRepo(SchoolManagementContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCountClassWiseResponse>> GetStudentCountClassWiseAsync(APIRequestDetails apiRequestDetails)
        {
            return await (
                from scd in _context.StudentClassDetails.AsNoTracking()
                join cs in _context.ClassSections.AsNoTracking() on scd.ClassSectionFkid equals cs.SysId
                join c in _context.Classes.AsNoTracking() on cs.ClassFkid equals c.SysId
                join ay in _context.AcademicYears.AsNoTracking() on scd.AcademicYearFkid equals ay.SysId
                where scd.InstitutionCode == apiRequestDetails.InstitutionCode
                   && ay.Status == "Active"
                   && scd.Status == "Active"
                group scd by new { c.SysId, c.ClassName } into g
                orderby g.Key.ClassName
                select new StudentCountClassWiseResponse
                {
                    ClassSysId = g.Key.SysId,
                    ClassName = g.Key.ClassName,
                    StudentCount = g.Count()
                }
            ).ToListAsync();
        }

        public async Task<List<FeesSummaryClassWiseResponse>> GetFeesSummaryClassWiseAsync(GetFeesSummaryClassWiseRequest request, APIRequestDetails apiRequestDetails)
        {
            return await (
                from sft in _context.StudentFeesTransactions.AsNoTracking()
                join scd in _context.StudentClassDetails.AsNoTracking() on sft.StudentClassDetailsFkid equals scd.SysId
                join cs in _context.ClassSections.AsNoTracking() on scd.ClassSectionFkid equals cs.SysId
                join c in _context.Classes.AsNoTracking() on cs.ClassFkid equals c.SysId
                join ay in _context.AcademicYears.AsNoTracking() on scd.AcademicYearFkid equals ay.SysId
                where sft.InstitutionCode == apiRequestDetails.InstitutionCode
                   && ay.SysId == request.AcademicYearSysId
                   && sft.Status == "Approved"
                group sft by new { c.SysId, c.ClassName } into g
                orderby g.Key.ClassName
                select new FeesSummaryClassWiseResponse
                {
                    ClassSysId = g.Key.SysId,
                    ClassName = g.Key.ClassName,
                    TotalFeesAmount = g.Sum(x => x.Debit),
                    FeesAmountReceived = g.Sum(x => x.Credit),
                    FeesBalance = g.Sum(x => x.Debit) - g.Sum(x => x.Credit)
                }
            ).ToListAsync();
        }
    }
}