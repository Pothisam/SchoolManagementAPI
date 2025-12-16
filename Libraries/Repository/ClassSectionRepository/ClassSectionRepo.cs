using Microsoft.EntityFrameworkCore;
using Models.ClassSectionModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ClassSectionRepository
{
    public class ClassSectionRepo : IClassSectionRepo
    {
        private readonly SchoolManagementContext _context;
        public ClassSectionRepo(SchoolManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> ActivateSectionAsync(int sysId, APIRequestDetails apiRequestDetails)
        {
            try
            {
                var entity = await _context.ClassSections.FindAsync(sysId);
                if (entity == null) return false;

                entity.Status = "Active";
                entity.ModifiedBy = apiRequestDetails.UserName;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<ClassSection>> GetSectionsAsync(ClassSectionRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.ClassSections.Where(x => x.ClassFkid == request.ClassFkid && x.InstitutionCode == apiRequestDetails.InstitutionCode).ToListAsync();
        }

        public async Task<bool> InactivateSectionAsync(int sysId, APIRequestDetails apiRequestDetails)
        {
            try
            {
                var entity = await _context.ClassSections.FindAsync(sysId);
                if (entity == null) return false;

                entity.Status = "InActive";
                entity.ModifiedBy = apiRequestDetails.UserName;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> InsertAsync(ClassSection entity)
        {
            try
            {
                _context.ClassSections.Add(entity);
                await _context.SaveChangesAsync();
                return entity.SysId > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
