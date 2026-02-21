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

        public async Task<List<StudentFeeGenerateStatusResponse>> GetFeesListViewAsync(GetFeesGentrationRequest request,APIRequestDetails apiRequestDetails)
        {
            var result = await (
                from scd in _context.StudentClassDetails

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

                    // LEFT JOIN StudentFeesTransaction
                join sft0 in _context.StudentFeesTransactions
                    on new
                    {
                        StudentFKID = scd.StudentDetailsFkid,
                        StudentClassDetailsFKID = cs.SysId,
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

                where ay.SysId == request.acadamicYear
                   && c.SysId == request.classfkid
                   && cs.SysId == request.sectionfkid
                   && scd.InstitutionCode == apiRequestDetails.InstitutionCode

                select new StudentFeeGenerateStatusResponse
                {
                    Sysid = smw.Sysid,
                    StudentName = smw.StudentName,
                    Stdid = smw.Stdid,
                    ClassName = c.ClassName,
                    SectionName = cs.SectionName,
                    Hostel = smw.Hostel,
                    Year = ay.Year,
                    Debit = (decimal?)sft.Debit ?? (decimal)request.amount,
                    Status = sft != null ? "Generated" : "Not Generated"
                }
            ).ToListAsync();

            return result;
        }
    }
}
