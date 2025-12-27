
using Models.CommonModels;
using Models.DocumentLibraryModels;
using Models.StudentModels;
using Repository.DocumentLibraryRepository;
using Repository.Entity;
using Repository.StudentRepository;
using Services.CommonServices;
using Services.DocumentLibraryServices;
using static Azure.Core.HttpHeader;


namespace Services.StudentServices
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _IStudentRepo;
        private readonly ICommonService _ICommonService;
        private readonly IDocumentLibraryRepo _IDocumentLibraryRepo;
        public StudentService(IStudentRepo IStudentRepo, ICommonService CommonService, IDocumentLibraryRepo DocumentLibraryRepo)
        {
            _IStudentRepo = IStudentRepo;
            _ICommonService = CommonService;
            _IDocumentLibraryRepo = DocumentLibraryRepo;
        }

        public async Task<CommonResponse<string>> AddStudent(AddStudentRequest request, APIRequestDetails apiRequestDetails)
        {
            if (!await _IStudentRepo.IsDuplicateAadharAsync(request.studentdetails.AadharCardNo, apiRequestDetails))
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "The student's details already exist. Duplicate entry of the Aadhaar number is not permitted."
                };
            }
            string studentId = await _IStudentRepo.GenerateStudentIdAsync(apiRequestDetails);
            var studentdetails = new StudentDetail
            {
                Stdid = studentId,
                ApplicationNumber = request.studentdetails.ApplicationNumber,
                AdmissionNumber = request.studentdetails.AdmissionNumber,
                AdmissionSerialNumber = request.studentdetails.AdmissionSerialNumber,
                Name = request.studentdetails.Name.ToUpper(),
                Initial = request.studentdetails.Initial.ToUpper(),
                Dob = Convert.ToDateTime(request.studentdetails.Dob),
                PlaceOfBirth = request.studentdetails.PlaceOfBirth.ToUpper(),
                MotherTongue = request.studentdetails.MotherTongue.ToUpper(),
                FatherName = request.studentdetails.FatherName.ToUpper(),
                FatherOccupation = request.studentdetails.FatherOccupation.ToUpper(),
                FatherIncome = request.studentdetails.FatherIncome,
                BloodGroup = request.studentdetails.BloodGroup,
                MotherName = request.studentdetails.MotherName.ToUpper(),
                MotherOccupation = request.studentdetails.MotherOccupation.ToUpper(),
                MotherIncome = request.studentdetails.MotherIncome,
                AadharCardNo = request.studentdetails.AadharCardNo,
                MobileNo = request.studentdetails.MobileNo,
                MobileNo2 = request.studentdetails.MobileNo2,
                Emailid = request.studentdetails.Emailid,
                FirstLanguage = request.studentdetails.FirstLanguage,
                Religion = request.studentdetails.Religion,
                Community = request.studentdetails.Community,
                Caste = request.studentdetails.Caste.ToUpper(),
                GuardianName = request.studentdetails.GuardianName.ToUpper(),
                ExtraCurricularActivities = request.studentdetails.ExtraCurricularActivities.ToUpper(),
                PhysicalDisability = request.studentdetails.PhysicalDisability,
                Volunteers = request.studentdetails.Volunteers,
                Gender = request.studentdetails.Gender,
                ParmanentAddress1 = request.studentdetails.ParmanentAddress1,
                ParmanentAddress2 = request.studentdetails.ParmanentAddress2,
                ParmanentAddressPincode = request.studentdetails.ParmanentAddressPincode,
                ParmanentAddressPostOffice = request.studentdetails.ParmanentAddressPostOffice,
                ParmanentAddressDistrict = request.studentdetails.ParmanentAddressDistrict,
                ParmanentAddressState = request.studentdetails.ParmanentAddressState,
                CommunicationAddress1 = request.studentdetails.CommunicationAddress1,
                CommunicationAddress2 = request.studentdetails.CommunicationAddress2,
                CommunicationAddressPincode = request.studentdetails.CommunicationAddressPincode,
                CommunicationAddressPostOffice = request.studentdetails.CommunicationAddressPostOffice,
                CommunicationAddressDistrict = request.studentdetails.CommunicationAddressDistrict,
                CommunicationAddressState = request.studentdetails.CommunicationAddressState,
                
                
                InstitutionCode = apiRequestDetails.InstitutionCode,              
                Referredby = request.studentdetails.Referredby,
                DocumentEnclosed = request.studentdetails.DocumentEnclosed,
                DocumentNotEnclosed = request.studentdetails.DocumentNotEnclosed,
                EnteredBy = apiRequestDetails.UserName
            };
            var Studentpasstable = new StudentPassTable
            {
                UserName = request.studentdetails.Name.ToUpper() + " " + request.studentdetails.Initial.ToUpper(),
                Password = await _ICommonService.Encrypt(request.studentdetails.Dob.ToString("dd/MM/yyyy").Replace('-', '/') ?? string.Empty),
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName

            };
            var StudentClassDetail = new StudentClassDetail
            {
                AcademicYearFkid = request.studentdetails.AcadamicYear,
                ClassSectionFkid = request.studentdetails.ClassSection,
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EnteredBy = apiRequestDetails.UserName

            };
            var StudentID = await _IStudentRepo.AddStudent(studentdetails, Studentpasstable, StudentClassDetail);
            if (!string.IsNullOrEmpty(request.studentdetails.ImageData))
            {
                var checkExistsRequest = new DocumentLibraryGetRequest
                {
                    FKID = StudentID,
                    TableName = "StudentDetails",
                    FileType = "Profile-Image",
                    Action = "Image-Upload"
                };
                bool documentExists = await _IDocumentLibraryRepo.DocumentLibraryExists(checkExistsRequest);
                if (!documentExists)
                {
                    await _IDocumentLibraryRepo.InsertDocumentLibrary(new DocumentLibrary
                    {
                        Fkid = StudentID,
                        TableName = "StudentDetails",
                        FileType = "Profile-Image",
                        Action = "Image-Upload",
                        FileName = request.studentdetails.ImageFileName ?? null,
                        ContentType = request.studentdetails.ImageContentType ?? null,
                        Data = Convert.FromBase64String(request.studentdetails.ImageData.Split(',')[1].Trim()),
                        InstitutionCode = apiRequestDetails.InstitutionCode,
                        EnteredBy = apiRequestDetails.UserName
                    });
                }
                foreach (var documnetRequest in request.documentrequests)
                {
                    var document = new DocumentLibrary
                    {
                        Fkid = StudentID,
                        InstitutionCode = apiRequestDetails.InstitutionCode,
                        TableName = "StudentDetails",
                        Action = "Document-Upload",
                        FileType = documnetRequest.FileType,
                        FileName = documnetRequest.FileName,
                        ContentType = documnetRequest.ContentType,
                        Data = Convert.FromBase64String(documnetRequest.Data.Split(',')[1].Trim()),
                        EnteredBy = apiRequestDetails.UserName,
                    };
                    // documents
                    await _IDocumentLibraryRepo.InsertDocumentLibrary(document);
                }
            }
            return new CommonResponse<string>
            {
                Status = StudentID > 0 ? Status.Success : Status.Failed,
                Message = StudentID > 0
                   ? "Student details saved successfully."
                   : "Unable to add Student"
            };
        }
    }
}
