using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.FeesTypeModels;
using Repository.Entity;

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
    }
}
