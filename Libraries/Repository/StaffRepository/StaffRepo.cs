using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.StaffModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StaffRepository
{
    public class StaffRepo : IStaffRepo
    {
        private readonly SchoolManagementContext _context;
        public StaffRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckDuplicate(StaffDetailsAddRequest request, APIRequestDetails apiRequestDetails)
        {
            var isDuplicate = await _context.StaffMasterViews.AnyAsync(x => x.MobileNo == request.MobileNo && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            return !isDuplicate;
        }
        public async Task<int> AddStaffAsync(StaffDetail staff, StaffPassTable passTable)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.StaffDetails.Add(staff);
                await _context.SaveChangesAsync();

                passTable.StaffDetailsFkid = staff.SysId;
                _context.StaffPassTables.Add(passTable);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return staff.SysId;
            }
            catch
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
        #region Add Language
        public async Task AddStaffLanguageDetail(StaffLanguageDetail request)
        {
            await _context.StaffLanguageDetails.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Add Education
        public async Task AddStaffEducationDetail(StaffEducationDetail request)
        {
            await _context.StaffEducationDetails.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Expirence
        public async Task AddStaffExperienceDetail(StaffExperience request)
        {
            await _context.StaffExperiences.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        
        #endregion
    }
}
