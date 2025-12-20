using Microsoft.EntityFrameworkCore;
using Models.AcademicYearModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.AcademicYearRepository
{
    public class AcademicYearRepo : IAcademicYearRepo
    {
        private readonly SchoolManagementContext _context;
        public AcademicYearRepo(SchoolManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAcademicYearAsync(AddAcademicYearRequest request, APIRequestDetails apiRequestDetails)
        {
            var entity = new AcademicYear
            {
                YearDate = request.YearDate,
                Year = request.Year,
                Status = "Active",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName,
                EntryDate = DateTime.Now
            };

            _context.AcademicYears.Add(entity);
            await _context.SaveChangesAsync();

            return entity.SysId > 0;
        }

        public async Task<bool> IsAcademicYearExistsAsync(AddAcademicYearRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.AcademicYears.AnyAsync(x => x.Year == request.Year && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }
        public async Task<List<AcademicYearResponse>> GetAcademicYearListAsync(APIRequestDetails apiRequestDetails)
        {
            return await _context.AcademicYears.Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode)
                          .Select(x => new AcademicYearResponse
                          {
                              SysId = x.SysId,
                              YearDate = x.YearDate,
                              Year = x.Year,
                              Status = x.Status,
                              EnteredBy = x.EnteredBy,
                              EntryDate = x.EntryDate,
                              ModifiedBy = x.ModifiedBy,
                              ModifiedDate = x.ModifiedDate
                          }).OrderByDescending(x => x.YearDate).ToListAsync();
        }
    }
}
