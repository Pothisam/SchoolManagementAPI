using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
using Repository.DocumentLibraryRepository;
using Repository.Entity;

namespace Services.DocumentLibraryServices
{
    public class DocumentLibraryService : IDocumentLibraryServices
    {
        private readonly IDocumentLibraryRepo _IDocumentLibraryRepo;
        public DocumentLibraryService(IDocumentLibraryRepo IDocumentLibraryRepo)
        {
            _IDocumentLibraryRepo = IDocumentLibraryRepo;
        }



        public async Task<CommonResponse<DocumentLibraryImageExportResponse>> GetProfileImagebyID(DocumentLibraryGuid request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<DocumentLibraryImageExportResponse>();
            var File = await _IDocumentLibraryRepo.GetProfileImagebyGuidAsync(request, apiRequestDetails);
            if (File != null)
            {
                response.Status = Status.Success;
                response.Data = File;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
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

        public async Task<CommonResponse<string>> UpdateDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            var document = new DocumentLibraryUpdate
            {
                Sysid = request.FKID,
                FileName = request.FileName,
                ContentType = request.ContentType,
                Data = Convert.FromBase64String(request.Data.Split(',')[1].Trim()),
                ModifiedBy = apiRequestDetails.UserName,
            };
            await _IDocumentLibraryRepo.UpdateDocumentLibrary(document);
            response.Status = Status.Success;
            response.Message = "Document updated Successfully";
            return response;
        }
        public async Task<CommonResponse<DocumentLibraryDetailsDownloadResponse>> DownloadFileAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<DocumentLibraryDetailsDownloadResponse>();
            var File = await _IDocumentLibraryRepo.DownloadFileAsync(request, apiRequestDetails);
            if (File != null)
            {
                response.Status = Status.Success;
                response.Data = File;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }

        public async Task<CommonResponse<string>> DeleteDocumentAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _IDocumentLibraryRepo.DeleteDocumentLibrary(request, apiRequestDetails);
            response.Status = Status.Success;
            response.Message = "Document Deleted Successfully";
            return response;
        }

        public async Task<CommonResponse<string>> InsertandUpdateProfileImage(InsertorUpdateProfileRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            string tableName = request.Table.ToLower() == "staff" ? "StaffDetails" : request.Table.ToLower() == "student" ? "StudentDetails" : null;

            if (tableName == null)
            {
                response.Status = Status.Failed;
                response.Message = "Invalid table name.";
                return response;
            }

            var checkExistsRequest = new DocumentLibraryGetRequest
            {
                FKID = request.Sysid,
                TableName = tableName,
                FileType = "Profile-Image",
                Action = "Image-Upload"
            };

            bool documentExists = await _IDocumentLibraryRepo.DocumentLibraryExists(checkExistsRequest);

            if (!documentExists)
            {
                await _IDocumentLibraryRepo.InsertDocumentLibrary(new DocumentLibrary
                {
                    Fkid = request.Sysid,
                    TableName = tableName,
                    FileType = "Profile-Image",
                    Action = "Image-Upload",
                    FileName = request.FileName ?? null,
                    FileSize = (int)Math.Ceiling(request.Data.Length / 1024.0),
                    ContentType = request.ContentType ?? null,
                    Data = Convert.FromBase64String(request.Data.Split(',')[1].Trim()),
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    EnteredBy = apiRequestDetails.UserName
                });
                response.Status = Status.Success;
                response.Message = $"{tableName} Profile Image is Inserted";
            }
            else
            {
                long Sysid = await _IDocumentLibraryRepo.GetDocumentLibrarySysid(checkExistsRequest);
                var document = new DocumentLibraryUpdate
                {
                    ContentType = request.ContentType,
                    FileName = request.FileName,
                    Data = Convert.FromBase64String(request.Data.Split(',')[1].Trim()),
                    ModifiedBy = apiRequestDetails.UserName,
                    Sysid = Sysid
                };
                await _IDocumentLibraryRepo.UpdateDocumentLibrary(document);
                response.Status = Status.Success;
                response.Message = $"{tableName} Profile Image is Updated";
            }

            return response;
        }
    }
}
