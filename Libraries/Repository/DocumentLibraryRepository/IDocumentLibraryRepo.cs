using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Repository.Entity;

namespace Repository.DocumentLibraryRepository
{

    public interface IDocumentLibraryRepo
    {
        Task InsertDocumentLibrary(DocumentLibrary request);
        Task<bool> DocumentLibraryExists(DocumentLibraryGetRequest request);
        Task<DocumentLibraryImageExportResponse> GetProfileImagebyGuidAsync(DocumentLibraryGuid request, APIRequestDetails apiRequestDetails);
    }
}
