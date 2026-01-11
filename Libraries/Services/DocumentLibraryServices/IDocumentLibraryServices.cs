using Models.CommonModels;
using Models.DocumentLibraryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DocumentLibraryServices
{
    public interface IDocumentLibraryServices
    {
        Task InsertDocumentLibrary(DocumentLibraryInsertRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<DocumentLibraryImageExportResponse>> GetProfileImagebyID(DocumentLibraryGuid request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> UpdateDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<DocumentLibraryDetailsDownloadResponse>> DownloadFileAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> DeleteDocumentAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> InsertandUpdateProfileImage(InsertorUpdateProfileRequest request, APIRequestDetails apiRequestDetails);
    }
}
