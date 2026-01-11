using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
using Repository.Entity;

namespace Repository.DocumentLibraryRepository
{

    public interface IDocumentLibraryRepo
    {
        Task InsertDocumentLibrary(DocumentLibrary request);
        Task<bool> DocumentLibraryExists(DocumentLibraryGetRequest request);
        Task<DocumentLibraryImageExportResponse> GetProfileImagebyGuidAsync(DocumentLibraryGuid request, APIRequestDetails apiRequestDetails);
        Task<List<DocumentLibraryDetailsResponse>> GetDocumentLibrary(DocumentLibraryListRequest request, APIRequestDetails apiRequestDetails);
        Task DeleteDocumentLibrary(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails);
        Task UpdateDocumentLibrary(DocumentLibraryUpdate request);
        Task<DocumentLibraryDetailsDownloadResponse> DownloadFileAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails);
        Task<long> GetDocumentLibrarySysid(DocumentLibraryGetRequest request);
    }
}
