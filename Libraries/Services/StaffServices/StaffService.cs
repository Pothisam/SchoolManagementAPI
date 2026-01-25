using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
using Models.StudentModels;
using Repository.DocumentLibraryRepository;
using Repository.Entity;
using Repository.StaffRepository;
using Services.CommonServices;
using Services.DocumentLibraryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.StaffServices
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepo _staffRepo;
        private readonly IDocumentLibraryRepo _IDocumentLibraryRepo;
        private readonly ICommonService _ICommonService;

        public StaffService(IStaffRepo staffRepo, ICommonService commonService, IDocumentLibraryRepo IDocumentLibraryRepo)
        {
            _staffRepo = staffRepo;
            _ICommonService = commonService;
            _IDocumentLibraryRepo = IDocumentLibraryRepo;
        }
        public async Task<CommonResponse<string>> AddStaffAsync(StaffDetailsAddRequest request, List<StaffLanguageDetailAddRequest> languageRequests, 
            List<StaffEducationDetailAddRequest> educationRequests, List<StaffExperienceDetailAddRequest> experienceRequests, 
            List<DocumentLibraryBulkInsert> documnetRequests, APIRequestDetails apiRequestDetails)
        {
            if(!await _staffRepo.CheckDuplicate(request, apiRequestDetails))
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Staff Mobile Number Already Exists Please Check"
                };
            }
            var staffId = await _staffRepo.GenerateStaffID(apiRequestDetails);
            var staff = new StaffDetail
            {
                StaffId = staffId,
                Title = request.Title,
                Name = request.Name,
                Initial = request.Initial,
                Sex = request.Sex,
                Dob = request.Dob,
                Doj = request.Doj,
                Dor = request.Dor,

                PlaceOfBirth = request.PlaceOfBirth,
                Religion = request.religion,
                Community = request.community,
                Cast = request.caste,
                PhysicalDisablity = request.physicalDisability,

                MobileNo = request.MobileNo,
                Emailid = request.Emailid,
                MotherTongue = request.MotherTongue,
                MaritalStatus = request.MaritalStatus,
                AadharCardNo = request.AadharCardNo,
                BloodGroup = request.BloodGroup,

                Designation = request.Designation,
                DesignationCode = request.DesignationCode,
                StaffType = request.StaffType,

                ParmanentAddress1 = request.PermanentAddress1,
                ParmanentAddress2 = request.PermanentAddress2,
                ParmanentAddressPincode = request.PermanentAddressPincode,
                ParmanentAddressPostOffice = request.PermanentAddressPostOffice,
                ParmanentAddressDistrict = request.PermanentAddressDistrict,
                ParmanentAddressState = request.PermanentAddressState,

                CommunicationAddress1 = request.CommunicationAddress1,
                CommunicationAddress2 = request.CommunicationAddress2,
                CommunicationAddressPincode = request.CommunicationAddressPincode,
                CommunicationAddressPostOffice = request.CommunicationAddressPostOffice,
                CommunicationAddressDistrict = request.CommunicationAddressDistrict,
                CommunicationAddressState = request.CommunicationAddressState,

                Ifsccode = request.Ifsccode,
                BankName = request.BankName,
                BankAddress = request.BankAddress,
                AccountNumber = request.AccountNumber,
                Micrcode = request.Micrcode,
                PancardNo = request.PancardNo,

                Status = "Active",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName
            };

            var password = await _ICommonService.Encrypt(request.Dob.ToString("dd/MM/yyyy").Replace('-', '/') ?? string.Empty);
            var passTable = new StaffPassTable
            {
                Password = password,
                Status = "Active",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName
            };

            int FKID = await _staffRepo.AddStaffAsync(staff, passTable);
            foreach (var languageRequest in languageRequests)
            {
                var staffLanguageDetail = new StaffLanguageDetail
                {
                    StaffFkid = FKID,
                    LanguageKnow = languageRequest.language,
                    ReadLanguage = languageRequest.ReadLanguage,
                    WriteLanguage = languageRequest.WriteLanguage,
                    SpeakLanguage = languageRequest.SpeakLanguage,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    EnteredBy = apiRequestDetails.UserName
                };

                await _staffRepo.AddStaffLanguageDetail(staffLanguageDetail);
            }
            foreach (var educationRequest in educationRequests)
            {
                var educationDetail = new StaffEducationDetail
                {
                    StaffDetailsFkid = FKID,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    DegreeType = educationRequest.DegreeType,
                    Degree = educationRequest.Degree,
                    YearOfpassing = educationRequest.YearOfpassing,
                    UniversityName = educationRequest.UniversityName,
                    InstituionName = educationRequest.InstitutionName,
                    Mode = educationRequest.Mode,
                    PassPercentage = educationRequest.PassPercentage,
                    Specialization = educationRequest.Specialization,
                    EnteredBy = apiRequestDetails.UserName
                };
                // Save educationDetail to the database or process as needed
                await _staffRepo.AddStaffEducationDetail(educationDetail);
            }
            foreach (var expirenceRequest in experienceRequests)
            {
                var expirenceDetail = new StaffExperience
                {
                    StaffDetailsFkid = FKID,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    InstituionName = expirenceRequest.InstitutionName,
                    Position = expirenceRequest.Position,
                    FromDate = expirenceRequest.FromDate,
                    Todate = expirenceRequest.Todate,
                    Salary = expirenceRequest.Salary,
                    EnteredBy = apiRequestDetails.UserName
                };
                // Save educationDetail to the database or process as needed
                await _staffRepo.AddStaffExperienceDetail(expirenceDetail);
            }
            if (!string.IsNullOrEmpty(request.ImageData))
            {
                var checkExistsRequest = new DocumentLibraryGetRequest
                {
                    FKID = FKID,
                    TableName = "StaffDetails",
                    FileType = "Profile-Image",
                    Action = "Image-Upload"
                };
                bool documentExists = await _IDocumentLibraryRepo.DocumentLibraryExists(checkExistsRequest);
                if (!documentExists)
                {
                    await _IDocumentLibraryRepo.InsertDocumentLibrary(new DocumentLibrary
                    {
                        Fkid = FKID,
                        TableName = "StaffDetails",
                        FileType = "Profile-Image",
                        Action = "Image-Upload",
                        FileName = request.ImageFileName ?? null,
                        FileSize = (int)Math.Ceiling(request.ImageData.Length / 1024.0),
                        ContentType = request.ImageContentType ?? null,
                        Data = Convert.FromBase64String(request.ImageData.Split(',')[1].Trim()),
                        InstitutionCode = apiRequestDetails.InstitutionCode,
                        EnteredBy = apiRequestDetails.UserName
                    });
                }
            }
            foreach (var documnetRequest in documnetRequests)
            {
                var document = new DocumentLibrary
                {
                    Fkid = FKID,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    TableName = "StaffDetails",
                    Action = "Document-Upload",
                    FileType = documnetRequest.FileType,
                    FileName = documnetRequest.FileName,
                    FileSize = (int)Math.Ceiling(request.ImageData.Length / 1024.0),
                    ContentType = documnetRequest.ContentType,
                    Data = Convert.FromBase64String(documnetRequest.Data.Split(',')[1].Trim()),
                    EnteredBy = apiRequestDetails.UserName,
                };
                // documents
                await _IDocumentLibraryRepo.InsertDocumentLibrary(document);
            }
            return new CommonResponse<string>
            {
                Status = FKID > 0 ? Status.Success : Status.Failed,
                Message = FKID > 0
                    ? "Staff added successfully"
                    : "Unable to add staff"
            };
        }

        public async Task<CommonResponse<List<AutoCompleteResponse>>> GetStaffAutoComplete(StaffAutocompleteRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<AutoCompleteResponse>>();
            List<AutoCompleteResponse> result = await _staffRepo.GetStaffAutoComplete(request, apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }
        #region Staff View List
        public async Task<CommonResponse<StaffCountResponse>> GetStaffCountAsync(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<StaffCountResponse>();
            var result = await _staffRepo.GetStaffCountAsync(apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }

        public async Task<CommonResponse<List<DesignationListResponse>>> GetStaffDesignationListAsync(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<DesignationListResponse>>();
            var designations = await _staffRepo.GetStaffDesignationListAsync(apiRequestDetails);

            if (designations.Any())
            {
                response.Status = Status.Success;
                response.Data = designations;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "No designations found for the given institution code.";
            }

            return response;
        }

       

        public async Task<CommonResponse<List<StaffDetailSearchResponse>>> GetStaffDetailSearchAsync(StaffSearchRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StaffDetailSearchResponse>>();
            var staffDetails = await _staffRepo.GetStaffDetailSearchAsync(request, apiRequestDetails);

            if (staffDetails.Any())
            {
                response.Status = Status.Success;
                response.Data = staffDetails;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "No staff details found";
            }

            return response;
        }
        #endregion
        public async Task<CommonResponse<StaffDetailsResponse>> GetStaffDetails(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<StaffDetailsResponse>
            {
                Data = new StaffDetailsResponse
                {
                    StaffDetail = new StaffDetailResponse(),
                    StaffLanguage = new List<StaffLanguageResponse>(),
                    StaffEducation = new List<StaffEducationResponse>(),
                    StaffExprience = new List<StaffExperienceResponse>(),
                    StaffDocument = new List<DocumentLibraryDetailsResponse>()
                }
            };
            var DocumentRequest = new DocumentLibraryListRequest
            {
                FKID = request.SysId,
                TableName = "StaffDetails",
                Action = "Document-Upload"
            };
            var staffDetail = await _staffRepo.GetStaffDetailByIDAsync(request, apiRequestDetails);
            var StafLanguage = await _staffRepo.GetStaffLanguageKnowByIDAsync(request, apiRequestDetails);
            var StaffEducation = await _staffRepo.GetStaffEducationDetailsByIDAsync(request, apiRequestDetails);
            var StaffExperience = await _staffRepo.GetStaffExperienceDetailsByIDAsync(request, apiRequestDetails);
            var StaffDocument = await _IDocumentLibraryRepo.GetDocumentLibrary(DocumentRequest, apiRequestDetails);
            if (staffDetail != null)
            {
                response.Status = Status.Success;
                response.Data.StaffDetail = staffDetail;
                response.Data.StaffLanguage = StafLanguage;
                response.Data.StaffEducation = StaffEducation;
                response.Data.StaffExprience = StaffExperience;
                response.Data.StaffDocument = StaffDocument;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }
        #region Add Language
        public async Task<CommonResponse<string>> AddStaffLanguageAsync(UpdateAddStaffLanguageDetail request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            var staffLanguageDetail = new StaffLanguageDetail
            {
                StaffFkid = request.SysId,
                LanguageKnow = request.language,
                ReadLanguage = request.ReadLanguage,
                WriteLanguage = request.WriteLanguage,
                SpeakLanguage = request.SpeakLanguage,
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName
            };
            await _staffRepo.AddStaffLanguageDetail(staffLanguageDetail);
            response.Status = Status.Success;
            response.Message = $"Language Detail '{request.language}' Added Successfully";
            return response;
        }
        public async Task<CommonResponse<List<StaffLanguageResponse>>> GetStaffLanguageDetails(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StaffLanguageResponse>>();
            var StafLanguage = await _staffRepo.GetStaffLanguageKnowByIDAsync(request, apiRequestDetails);
            if (StafLanguage.Any())
            {
                response.Status = Status.Success;
                response.Data = StafLanguage;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }
        public async Task<CommonResponse<string>> DeleteStaffLanguageAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _staffRepo.DeleteStaffLanguageAsync(request, apiRequestDetails);
            response.Status = Status.Failed;
            response.Message = "Language Detail Deleted Successfully";
            return response;
        }
        #endregion
        #region Education
        public async Task<CommonResponse<string>> InsertStaffEducationDetailsAsync(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            StaffEducationDetailAdd AddRequest = _ICommonService.TransformClass<UpdateAddStaffEducationDetailAdd, StaffEducationDetailAdd>(request);
            AddRequest.Id = request.SysId;
            AddRequest.InstitutionCode = apiRequestDetails.InstitutionCode;
            if (await _staffRepo.CheckDuplicateStaffEducation(AddRequest))
            {
                var educationDetail = new StaffEducationDetail
                {
                    StaffDetailsFkid = request.SysId,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    DegreeType = request.DegreeType,
                    Degree = request.Degree,
                    YearOfpassing = request.YearOfpassing,
                    UniversityName = request.UniversityName,
                    InstituionName = request.InstitutionName,
                    Mode = request.Mode,
                    PassPercentage = request.PassPercentage,
                    Specialization = request.Specialization,
                    EnteredBy = apiRequestDetails.UserName
                };
                await _staffRepo.AddStaffEducationDetail(educationDetail);
                response.Status = Status.Success;
                response.Message = "Staff Education Details Inserted";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "Staff Education Details already exist. Duplicate entry not allowed.";
            }
            return response;
        }
        public async Task<CommonResponse<List<StaffEducationResponse>>> GetStaffEducationDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StaffEducationResponse>>();
            var StafEducationdetails = await _staffRepo.GetStaffEducationDetailsByIDAsync(request, apiRequestDetails);
            if (StafEducationdetails.Any())
            {
                response.Status = Status.Success;
                response.Data = StafEducationdetails;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }
        public async Task<CommonResponse<string>> DeleteStaffEducationAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _staffRepo.DeleteStaffEducationAsync(request, apiRequestDetails);
            response.Status = Status.Failed;
            response.Message = "Saff Education Detail Deleted Successfully";
            return response;
        }
        public async Task<CommonResponse<string>> UpdateStaffEducationDetailsAsync(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            if (await _staffRepo.CheckDuplicateStaffEducationUpdate(request, apiRequestDetails))
            {
                await _staffRepo.UpdateStaffEducationDetail(request, apiRequestDetails);
                response.Status = Status.Success;
                response.Message = "Staff Education Details Updated Successfully";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "Staff Education Details already exist. Duplicate entry not allowed.";
            }
            return response;
        }
        #endregion
        #region Expirence
        public async Task<CommonResponse<string>> AddStaffExperienceDetailAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            StaffExperienceDetailAdd AddRequest = _ICommonService.TransformClass<UpdateStaffExperienceDetailAddRequest, StaffExperienceDetailAdd>(request);
            AddRequest.Id = request.SysId;
            AddRequest.InstitutionCode = apiRequestDetails.InstitutionCode;
            var expirenceDetail = new StaffExperience
            {
                StaffDetailsFkid = request.SysId,
                InstitutionCode = apiRequestDetails.InstitutionCode,
                InstituionName = request.InstitutionName,
                Position = request.Position,
                FromDate = request.FromDate,
                Todate = request.Todate,
                Salary = request.Salary,
                EnteredBy = apiRequestDetails.UserName
            };
            await _staffRepo.AddStaffExperienceDetail(expirenceDetail);
            response.Status = Status.Success;
            response.Message = "Staff Education Details Inserted";
            return response;
        }
        public async Task<CommonResponse<List<StaffExperienceResponse>>> GetStaffExperienceDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StaffExperienceResponse>>();
            var StafExpirence = await _staffRepo.GetStaffExperienceDetailsByIDAsync(request, apiRequestDetails);
            if (StafExpirence.Any())
            {
                response.Status = Status.Success;
                response.Data = StafExpirence;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }
        public async Task<CommonResponse<string>> DeleteStaffExperienceAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _staffRepo.DeleteStaffExperienceAsync(request, apiRequestDetails);
            response.Status = Status.Failed;
            response.Message = "Staff Experience Detail Deleted Successfully";
            return response;
        }
        public async Task<CommonResponse<string>> UpdateStaffExperienceDetailsAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _staffRepo.UpdateStaffExperienceDetailsAsync(request, apiRequestDetails);
            response.Status = Status.Success;
            response.Message = "Staff Experience Details Updated Successfully";
            return response;
        }
        #endregion
        #region Document    
        public async Task<CommonResponse<string>> AddStaffDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails)
        {
            var checkExistsRequest = new DocumentLibraryGetRequest
            {
                FKID = request.FKID,
                TableName = "StaffDetails",
                FileType = request.FileType,
                Action = "Document-Upload"
            };
            bool documentExists = await _IDocumentLibraryRepo.DocumentLibraryExists(checkExistsRequest);
            var response = new CommonResponse<string>();
            if (documentExists)
            {
                response.Status = Status.Failed;
                response.Message = "Document Type Already Exists. Can't insert duplicate record.";
            }
            else
            {
                await _IDocumentLibraryRepo.InsertDocumentLibrary(new DocumentLibrary
                {
                    Fkid = request.FKID,
                    TableName = "StaffDetails",
                    FileType = request.FileType,
                    Action = "Document-Upload",
                    FileName = request.FileName,
                    FileSize = (int)Math.Ceiling(request.Data.Length / 1024.0),
                    ContentType = request.ContentType,
                    Data = Convert.FromBase64String(request.Data.Split(',')[1].Trim()),
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    EnteredBy = apiRequestDetails.UserName
                });
                response.Status = Status.Success;
                response.Message = $"Document Type {request.FileType} inserted successfully.";
            }
            return response;
        }

        public async Task<CommonResponse<List<DocumentLibraryDetailsResponse>>> GetStaffDocumentAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var DocumentRequest = new DocumentLibraryListRequest
            {
                FKID = request.SysId,
                TableName = "StaffDetails",
                Action = "Document-Upload"
            };
            var response = new CommonResponse<List<DocumentLibraryDetailsResponse>>();
            var StafDocument = await _IDocumentLibraryRepo.GetDocumentLibrary(DocumentRequest, apiRequestDetails);
            if (StafDocument.Any())
            {
                response.Status = Status.Success;
                response.Data = StafDocument;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "";
            }
            return response;
        }

        public async Task<CommonResponse<string>> DeleteStafffDocumentAsync(DocumentLibrarySysid request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            await _IDocumentLibraryRepo.DeleteDocumentLibrary(request, apiRequestDetails);
            response.Status = Status.Failed;
            response.Message = "Document Deleted Successfully";
            return response;
        }

        public async Task<CommonResponse<string>> UpdateStaffDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails)
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
        #endregion

        public async Task<CommonResponse<string>> UpdateStaffDetailsAsync(UpdateStaffDetailsRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            var result = await _staffRepo.UpdateStaffDetailsAsync(request, apiRequestDetails);
            if (result)
            {
                if (request.Status == "InActive")
                {
                    await _staffRepo.UpdateStaffPasstableStatusAsync(request.Sysid, request.Status);
                }
                response.Status = Status.Success;
                response.Message = "Staff Details Updated Successfully";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "Error: Unable to update staff details";
            }

            return response;
        }

        public async Task<CommonResponse<string>> ResetStaffPasswordAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            var StaffDetails = await _staffRepo.GetStaffDetailByIDAsync(request, apiRequestDetails);
            if (StaffDetails != null)
            {
                var password = await _ICommonService.Encrypt(StaffDetails.DOB.ToString("dd/MM/yyyy").Replace('-', '/') ?? string.Empty);

                var Updaterequest = new StaffDetailsPasswordReset { Password = password, SysId = request.SysId };
                await _staffRepo.ResetStaffPasswordAsync(Updaterequest, apiRequestDetails);
                response.Status = Status.Success;
                response.Message = "The staff password has been reset to their date of birth (DOB)";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "The requested staff details were not found.";
            }
            return response;
        }
        public async Task<CommonResponse<List<StaffNameAndSysidResponse>>> GetStaffNameList(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StaffNameAndSysidResponse>>();
            var result = await _staffRepo.GetStaffNameListAsync(apiRequestDetails);

            if (result.Any())
            {
                response.Status = Status.Success;
                response.Data = result;
            }
            else
            {
                response.Status = Status.Failed;
            }
            return response;
        }

    }
}
