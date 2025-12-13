using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CommonRepository
{
    public class CommonRepo : ICommonRepo
    {
        private readonly SchoolManagementContext _context;
        public CommonRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<DateTime> GetDatetime()
        {
            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "SELECT GETDATE()";
                _context.Database.OpenConnection();

                using (var result = await command.ExecuteReaderAsync())
                {
                    if (await result.ReadAsync())
                    {
                        return result.GetDateTime(0);
                    }
                }
            }
            throw new InvalidOperationException("Failed to retrieve date and time.");
        }

        public async Task<byte[]?> GetFavIconAsync(APIRequestDetails apiRequestDetails)
        {
            var logoData = await _context.InstitutionDetails
                                     .Where(x => x.Sysid == apiRequestDetails.InstitutionCode)
                                     .Select(img => img.LogoWithTextData)
                                     .FirstOrDefaultAsync();
            return logoData;
        }

        public async Task<byte[]?> GetLogoonlyAsync(APIRequestDetails apiRequestDetails)
        {
            var logoData = await _context.InstitutionDetails
                                     .Where(x => x.Sysid == apiRequestDetails.InstitutionCode)
                                     .Select(img => img.LogoData)
                                     .FirstOrDefaultAsync();
            return logoData;
        }

        public async Task<byte[]?> GetLogoWithTextAsync(APIRequestDetails apiRequestDetails)
        {
            var logoData = await _context.InstitutionDetails
                                     .Where(x => x.Sysid == apiRequestDetails.InstitutionCode)
                                     .Select(img => img.FaviconData)
                                     .FirstOrDefaultAsync();
            return logoData;
        }
    }
}
