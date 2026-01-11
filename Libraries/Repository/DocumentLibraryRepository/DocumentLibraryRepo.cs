using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
using Repository.Entity;

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
        public async Task<List<DocumentLibraryDetailsResponse>> GetDocumentLibrary(DocumentLibraryListRequest request, APIRequestDetails apiRequestDetails)
        {
            List<DocumentLibraryDetailsResponse>? response = await (from x in _context.DocumentLibraries
                                                                    where x.Fkid == request.FKID && x.TableName == request.TableName
                                                                    && x.Action == request.Action && x.Status == "Active" && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                                    select new DocumentLibraryDetailsResponse
                                                                    {
                                                                        Sysid = x.Sysid,
                                                                        FileName = x.FileName,
                                                                        FileType = x.FileType,
                                                                        FileSize = x.FileSize,
                                                                        Guid = x.Guid,
                                                                        EnteredBy = x.EnteredBy,
                                                                        EntryDate = x.EntryDate,
                                                                        ModifiedBy = x.ModifiedBy ?? "",
                                                                        ModifiedDate = x.ModifiedDate
                                                                    }).ToListAsync();
            return response;
        }

        public async Task DeleteDocumentLibrary(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails)
        {
            DocumentLibrary Col = _context.DocumentLibraries.Single(x => x.Sysid == request.SysId);
            Col.FileSize = 0;
            Col.Data = new byte[0];
            Col.ModifiedBy = apiRequestDetails.UserName;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateDocumentLibrary(DocumentLibraryUpdate request)
        {
            DocumentLibrary Col = _context.DocumentLibraries.Single(x => x.Sysid == request.Sysid);
            Col.FileSize = (int)Math.Ceiling(request.Data.Length / 1024.0);
            Col.ContentType = request.ContentType;
            Col.FileName = request.FileName;
            Col.Data = request.Data;
            Col.ModifiedBy = request.ModifiedBy;
            Col.Guid = Guid.NewGuid();
            Col.Status = "Active";
            await _context.SaveChangesAsync();
        }

        public async Task<DocumentLibraryDetailsDownloadResponse> DownloadFileAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails)
        {
            DocumentLibraryDetailsDownloadResponse response = await (from x in _context.DocumentLibraries
                                                                     where x.Sysid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                                     select new DocumentLibraryDetailsDownloadResponse
                                                                     {
                                                                         FileName = x.FileName,
                                                                         FileType = x.FileType,
                                                                         Data = x.Data,
                                                                     }).FirstAsync();
            return response;
        }

        public async Task<long> GetDocumentLibrarySysid(DocumentLibraryGetRequest request)
        {
            return await _context.DocumentLibraries.Where(x => x.Fkid == request.FKID && x.TableName == request.TableName && x.FileType == request.FileType && x.Action == request.Action).Select(x => x.Sysid).FirstAsync();
        }
    }
}
