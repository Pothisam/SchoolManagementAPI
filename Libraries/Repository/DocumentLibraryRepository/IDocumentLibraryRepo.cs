using Models.DocumentLibraryModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DocumentLibraryRepository
{

    public interface IDocumentLibraryRepo
    {
        Task InsertDocumentLibrary(DocumentLibrary request);
        Task<bool> DocumentLibraryExists(DocumentLibraryGetRequest request);
    }
}
