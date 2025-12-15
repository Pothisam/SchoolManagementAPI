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
                                     .Select(img => img.FaviconData)
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
                                     .Select(img => img.LogoWithTextData)
                                     .FirstOrDefaultAsync();
            return logoData;
        }

        

        public async Task<List<RecordHistoryResponse>> GetRecordHistory(GetRecordHistoryRequest request, APIRequestDetails apiRequestDetails)
        {
            List<RecordHistoryResponse> response = new();
            int takeCount = request.LoadAllRecord ? 10 : 15;

            var query = from x in _context.AuditTables
                        where x.InstitutionCode == apiRequestDetails.InstitutionCode && x.TableName == request.TableName
                        orderby x.ModifiedDate descending
                        select new AuditTable
                        {
                            Fid = x.Fid,
                            Application = x.Application,
                            Modifiedby = x.Modifiedby,
                            ModifiedDate = x.ModifiedDate,
                            Data = x.Data,
                        };
            // Filter by FID if provided
            if (request.FID != 0)
            {
                query = query.Where(x => x.Fid == request.FID);
            }

            // Filter by Application if not null or empty
            if (!string.IsNullOrEmpty(request.Application))
            {
                query = query.Where(x => x.Application == request.Application);
            }
            response = await (from x in query
                              select new RecordHistoryResponse
                              {
                                  ModifiedBy = x.Modifiedby,
                                  ModifiedDate = x.ModifiedDate,
                                  Data = x.Data,
                              })
                          .Take(takeCount)
                          .ToListAsync();
            return response;
        }

        public async Task<List<PostOfficeResponse>> GetPostOffice(PostOfficeRequest request)
        {
            List<PostOfficeResponse>? PostOfficeResponse = await (from x in _context.AllIndiaPincodeData
                                                                  where x.PinCode == request.pincode
                                                                  select new PostOfficeResponse
                                                                  {
                                                                      OfficeName = x.OfficeName,
                                                                      Districtname = x.Districtname,
                                                                      StateName = x.StateName,
                                                                  }).ToListAsync();
            return PostOfficeResponse;
        }
    }
}
