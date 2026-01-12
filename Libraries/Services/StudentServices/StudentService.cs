
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
        #region Add Student
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
                Remark = request.studentdetails.Remark,
                DateOfAdmission = request.studentdetails.DateOfAdmission,
                BroSysStudyingStudied = request.studentdetails.BroSysStudyingStudied,
                NameBroSys = request.studentdetails.NameBroSys,
                ModeOftransport = request.studentdetails.ModeOftransport,
                BoardingPoint = request.studentdetails.BoardingPoint,
                Hostel = request.studentdetails.Hostel,
                
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
                StudentType = request.studentdetails.StudentType,
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
                        EnteredBy = apiRequestDetails.UserName,
                        FileSize = (int)Math.Ceiling(request.studentdetails.ImageData.Length / 1024.0),
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
                        FileSize = (int)Math.Ceiling(documnetRequest.Data.Length / 1024.0),
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
        public async Task<CommonResponse<List<AutoCompleteResponse>>> GetStudentAutoComplete(AutoCompleteRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<AutoCompleteResponse>>();
            List<AutoCompleteResponse> result = new List<AutoCompleteResponse>();
            if (request.ColumnName == "NameRollNoAadhar")
            {
                AutoCompleteRequest Name = new AutoCompleteRequest { TableName = request.TableName, ColumnName = "Name", SearchParam = request.SearchParam };
                var L1 = await _IStudentRepo.GetStudentAutoComplete(Name, apiRequestDetails);
                AutoCompleteRequest RollNo = new AutoCompleteRequest { TableName = request.TableName, ColumnName = "RollNo", SearchParam = request.SearchParam };
                var L2 = await _IStudentRepo.GetStudentAutoComplete(RollNo, apiRequestDetails);
                AutoCompleteRequest AadharCardNo = new AutoCompleteRequest { TableName = request.TableName, ColumnName = "AadharCardNo", SearchParam = request.SearchParam };
                var L3 = await _IStudentRepo.GetStudentAutoComplete(AadharCardNo, apiRequestDetails);
                result = L1.Union(L2).Union(L3).Take(5).ToList();
            }
            else
            {
                result = await _IStudentRepo.GetStudentAutoComplete(request, apiRequestDetails);
            }
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
        public async Task<CommonResponse<string>> ResetStudentPasswordAsync(StudentDetailsViewRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            var result = await _IStudentRepo.GetStudentDetailBySysid(request, apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
                return response;
            }
            var password = await _ICommonService.Encrypt(result.Dob.ToString("dd/MM/yyyy").Replace('-', '/') ?? string.Empty);
            var passwordRequest = new StudentPassword
            {
                Sysid = request.Sysid,
                Password = password
            };
            var isReset = await _IStudentRepo.ResetStudentPasswordAsync(passwordRequest, apiRequestDetails);
            if (isReset)
            {
                response.Status = Status.Success;
                response.Message = "Password reset successfully.";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "Could not reset password.";
            }
            return response;
        }
        #endregion
        #region View Student List
        public async Task<CommonResponse<StudentCountResponse>> GetStudentCountAsync(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<StudentCountResponse>();
            var result = await _IStudentRepo.GetStudentCountAsync(apiRequestDetails);

            if (result != null)
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

        public async Task<CommonResponse<List<StudentDetailsShortResponse>>> GetStudentDetailsShortAsync(StudentShortRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentDetailsShortResponse>>();
            var result = await _IStudentRepo.GetStudentDetailsShortAsync(request, apiRequestDetails);

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
        public async Task<CommonResponse<List<StudentDetailsShortResponse>>> GetStudentDetailsShortAsync(StudentSearchRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentDetailsShortResponse>>();
            var result = await _IStudentRepo.GetStudentDetailsShortAsync(request, apiRequestDetails);

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
        #endregion
        #region View Student Details
        public async Task<CommonResponse<StudentDetailsResponse>> GetStudentDetailBySysid(StudentDetailsViewRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<StudentDetailsResponse>
            {
                Data = new StudentDetailsResponse
                {
                    StudentDetail = new StudentMasterViewResponse(),
                    StudentDocument = new List<DocumentLibraryDetailsResponse>()
                }
            };

            var result = await _IStudentRepo.GetStudentDetailBySysid(request, apiRequestDetails);

            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
                return response;
            }

            var documentRequest = new DocumentLibraryListRequest
            {
                FKID = request.Sysid,
                TableName = "StudentDetails",
                Action = "Document-Upload"
            };

            var studentDocument = await _IDocumentLibraryRepo.GetDocumentLibrary(documentRequest, apiRequestDetails);

            response.Status = Status.Success;
            response.Message = string.Empty;
            response.Data.StudentDetail = result;
            response.Data.StudentDocument = studentDocument;

            return response;
        }

        public async Task<CommonResponse<string>> UpdateStudent(UpdateStudentDetailRequest request, APIRequestDetails apiRequestDetails)
        {
            var student = await _IStudentRepo.GetStudentByIdAsync(request,apiRequestDetails);
            if (student == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Student not found"
                };
            }
            student.ApplicationNumber = request.ApplicationNumber;
            student.AdmissionNumber = request.AdmissionNumber;
            student.AdmissionSerialNumber = request.AdmissionSerialNumber;
            student.Initial = request.Initial;
            student.Name = request.StudentName;
            student.Dob = request.Dob;
            student.PlaceOfBirth = request.PlaceOfBirth;
            student.MotherTongue = request.MotherTongue;
            student.FatherName = request.FatherName;
            student.FatherOccupation = request.FatherOccupation;
            student.FatherIncome = request.FatherIncome;
            student.BloodGroup = request.BloodGroup;
            student.MotherName = request.MotherName;
            student.MotherOccupation = request.MotherOccupation;
            student.MotherIncome = request.MotherIncome;
            student.AadharCardNo = request.AadharCardNo;
            student.MobileNo = request.MobileNo;
            student.MobileNo2 = request.MobileNo2;
            student.Emailid = request.Emailid;
            student.FirstLanguage = request.FirstLanguage;
            student.Parents = request.Parents;
            student.Religion = request.Religion;
            student.Community = request.Community;
            student.Caste = request.Caste;
            student.GuardianName = request.GuardianName;
            student.ExtraCurricularActivities = request.ExtraCurricularActivities;
            student.PhysicalDisability = request.PhysicalDisability;
            student.Volunteers = request.Volunteers;
            student.Gender = request.Gender;

            student.ParmanentAddress1 = request.ParmanentAddress1;
            student.ParmanentAddress2 = request.ParmanentAddress2;
            student.ParmanentAddressPincode = request.ParmanentAddressPincode;
            student.ParmanentAddressPostOffice = request.ParmanentAddressPostOffice;
            student.ParmanentAddressDistrict = request.ParmanentAddressDistrict;
            student.ParmanentAddressState = request.ParmanentAddressState;

            student.CommunicationAddress1 = request.CommunicationAddress1;
            student.CommunicationAddress2 = request.CommunicationAddress2;
            student.CommunicationAddressPincode = request.CommunicationAddressPincode;
            student.CommunicationAddressPostOffice = request.CommunicationAddressPostOffice;
            student.CommunicationAddressDistrict = request.CommunicationAddressDistrict;
            student.CommunicationAddressState = request.CommunicationAddressState;

            student.DateOfAdmission = request.DateOfAdmission;
            student.Remark = request.Remark;
            student.Status = request.Status;
            student.Referredby = request.Referredby;
            student.DocumentEnclosed = request.DocumentEnclosed;
            student.DocumentNotEnclosed = request.DocumentNotEnclosed;

            // 🔐 Audit
            student.ModifiedBy = apiRequestDetails.UserName;

            var classDetail = await _IStudentRepo.GetActiveStudentClassAsync(request, apiRequestDetails);

            if (classDetail != null)
            {
                classDetail.ClassSectionFkid = request.ClassSectionSysId;
                classDetail.StudentType = request.StudentType;
                classDetail.RollNo = request.RollNo;
                classDetail.ExamRegisterNumber = request.ExamRegisterNumber;
                classDetail.ModifiedBy = apiRequestDetails.UserName;
            }

            var result = await _IStudentRepo.UpdateAsync(student, classDetail);
            return new CommonResponse<string>
            {
                Status = result ? Status.Success : Status.Failed,
                Message = result
                ? "Student details updated successfully"
                : "Unable to update student details"
            };
        }
        #endregion
        #region Document Library
        public async Task<CommonResponse<string>> AddStudentDocumentAsync(DocumentLibraryBulkInsertByFKID request, APIRequestDetails apiRequestDetails)
        {
            var checkExistsRequest = new DocumentLibraryGetRequest
            {
                FKID = request.FKID,
                TableName = "StudentDetails",
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
                var document = new DocumentLibrary
                {
                    Fkid = request.FKID,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    TableName = "StudentDetails",
                    Action = "Document-Upload",
                    FileType = request.FileType,
                    FileName = request.FileName,
                    FileSize = (int)Math.Ceiling(request.Data.Length / 1024.0),
                    ContentType = request.ContentType,
                    Data = Convert.FromBase64String(request.Data.Split(',')[1].Trim()),
                    EnteredBy = apiRequestDetails.UserName,
                };
                // documents
                await _IDocumentLibraryRepo.InsertDocumentLibrary(document);
                response.Status = Status.Success;
                response.Message = $"Document Type {request.FileType} inserted successfully.";
            }
            return response;
        }

        public async Task<CommonResponse<List<DocumentLibraryDetailsResponse>>> GetStudentDocumentAsync(StudentDetailsViewRequest request, APIRequestDetails apiRequestDetails)
        {
            var DocumentRequest = new DocumentLibraryListRequest
            {
                FKID = request.Sysid,
                TableName = "StudentDetails",
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

        


        #endregion
    }
}
