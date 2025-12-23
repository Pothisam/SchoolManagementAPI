using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DocumentLibraryRepository
{
    public class DocumentLibraryRepo: IDocumentLibraryRepo
    {
        private readonly SchoolManagementContext _context;
        public DocumentLibraryRepo(SchoolManagementContext context)
        {
            _context = context;
        }

        public async Task<bool> DocumentLibraryExists(DocumentLibraryGetRequest request)
        {
            return await _context.DocumentLibraries
           .AnyAsync(x => x.Fkid == request.FKID && x.TableName == request.TableName && x.FileType == request.FileType && x.Action == request.Action);
        }

        public async Task<DocumentLibraryImageExportResponse> GetProfileImagebyGuidAsync(DocumentLibraryGuid request, APIRequestDetails apiRequestDetails)
        {
            DocumentLibraryImageExportResponse? response = await (from x in _context.DocumentLibraries
                                                                  where x.Guid == request.Guid && x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Status == "Active" && x.FileSize != 0
                                                                  select new DocumentLibraryImageExportResponse
                                                                  {
                                                                      Fkid = x.Fkid,
                                                                      Guid = x.Guid,
                                                                      Data = x.Data,
                                                                      TableName = x.TableName,
                                                                      FileSize = x.FileSize
                                                                  }).FirstAsync();
            return response;
        }

        public async Task InsertDocumentLibrary(DocumentLibrary request)
        {
            await _context.DocumentLibraries.AddAsync(request);
            await _context.SaveChangesAsync();
        }
    }
}
