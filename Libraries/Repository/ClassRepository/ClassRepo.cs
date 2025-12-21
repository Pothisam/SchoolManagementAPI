using Microsoft.EntityFrameworkCore;
using Models.ClassModels;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ClassRepository
{
    public class ClassRepo : IClassRepo
    {
        private readonly SchoolManagementContext _context;
        public ClassRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<bool> IsClassExistsAsync(AddClassRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.Classes.AnyAsync(x =>
            x.ClassName == request.ClassName &&
            x.InstitutionCode == apiRequestDetails.InstitutionCode &&
            (x.Status == "Active" || x.Status == "InActive"));
        }
        public async Task<int> AddClassAsync(AddClassRequest request, APIRequestDetails apiRequestDetails)
        {
            var entity = new Class
            {
                ClassName = request.ClassName.ToUpper(),
                Status = "Active",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName,
            };
            _context.Classes.Add(entity);
            await _context.SaveChangesAsync();
            return entity.SysId;
        }

        public async Task<List<ClassResponse>> GetClassListAsync(APIRequestDetails apiRequestDetails)
        {
            return await _context.Classes
            .Where(x =>
                x.InstitutionCode == apiRequestDetails.InstitutionCode &&
                (x.Status == "Active" || x.Status == "InActive")
            ).OrderBy(x => x.ClassName)
            .Select(x => new ClassResponse
            {
                SysId = x.SysId,
                ClassName = x.ClassName,
                Status = x.Status,
                EnteredBy = x.EnteredBy,
                EntryDate = x.EntryDate,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate
            }).ToListAsync();
        }

        public async Task<Class?> GetClassByIdAsync(UpdateClassStatusRequest request, APIRequestDetails apiRequestDetails)
        {
            return await _context.Classes.FirstOrDefaultAsync(x => x.SysId == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
        }

        public async Task<bool> UpdateClassAsync(Class entity)
        {
            try
            {
                _context.Classes.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<GetClassResponse>> GetClassListActiveAsync(APIRequestDetails apiRequestDetails)
        {
            return await _context.Classes.Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Status == "Active").OrderBy(x => x.ClassName)
                         .Select(x => new GetClassResponse
                         {
                             SysId = x.SysId,
                             ClassName = x.ClassName
                         }).ToListAsync();
        }
    }
}
