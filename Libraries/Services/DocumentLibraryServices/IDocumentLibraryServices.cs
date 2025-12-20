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
    }
}
