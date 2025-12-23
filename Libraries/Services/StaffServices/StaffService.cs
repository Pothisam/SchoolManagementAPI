using Microsoft.EntityFrameworkCore;
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StaffModels;
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
                Religion = request.Religion,
                Community = request.Community,
                Cast = request.Cast,
                PhysicalDisablity = request.PhysicalDisablity,

                MobileNo = request.MobileNo,
                Emailid = request.Emailid,
                MotherTongue = request.MotherTongue,
                MaritalStatus = request.MaritalStatus,
                AadharCardNo = request.AadharCardNo,
                BloodGroup = request.BloodGroup,

                Designation = request.Designation,
                DesignationCode = request.DesignationCode,
                StaffType = request.StaffType,

                ParmanentAddress1 = request.ParmanentAddress1,
                ParmanentAddress2 = request.ParmanentAddress2,
                ParmanentAddressPincode = request.ParmanentAddressPincode,
                ParmanentAddressPostOffice = request.ParmanentAddressPostOffice,
                ParmanentAddressDistrict = request.ParmanentAddressDistrict,
                ParmanentAddressState = request.ParmanentAddressState,

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
                    LanguageKnow = languageRequest.LanguageKnow,
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
                    InstituionName = educationRequest.InstituionName,
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
                    InstituionName = expirenceRequest.InstituionName,
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
    }
}
