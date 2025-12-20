using Models.CommonModels;
using Models.DocumentLibraryModels;
using Repository.DocumentLibraryRepository;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DocumentLibraryServices
{
    public class DocumentLibraryService : IDocumentLibraryServices
    {
        private readonly IDocumentLibraryRepo _IDocumentLibraryRepo;
        public DocumentLibraryService(IDocumentLibraryRepo IDocumentLibraryRepo)
        {
            _IDocumentLibraryRepo = IDocumentLibraryRepo;
        }
        public async Task InsertDocumentLibrary(DocumentLibraryInsertRequest request, APIRequestDetails apiRequestDetails)
        {
            var newdocument = new DocumentLibrary
            {
                Fkid = request.FKID,
                TableName = request.TableName,
                FileName = request.FileName,
                ContentType = request.ContentType,
                Data = request.Data,
                FileType = request.FileType,
                Action = request.Action,
                FileSize = (int)Math.Ceiling(request.Data.Length / 1024.0),
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName
            };
            await _IDocumentLibraryRepo.InsertDocumentLibrary(newdocument);
        }
    }
}
