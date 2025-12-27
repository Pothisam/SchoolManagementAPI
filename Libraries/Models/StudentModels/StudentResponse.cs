using Models.DocumentLibraryModels;

namespace Models.StudentModels
{
    public class StudentDetailsResponse
    {
        public required StudentMasterViewResponse StudentDetail { get; set; }
        public List<DocumentLibraryDetailsResponse> StudentDocument { get; set; } = new List<DocumentLibraryDetailsResponse>();

    }
    public partial class StudentMasterViewResponse
    {
        public int SysId { get; set; }

        public string Stdid { get; set; } = null!;

        public string ApplicationNumber { get; set; } = null!;

        public string AdmissionNumber { get; set; } = null!;

        public string? AdmissionSerialNumber { get; set; }

        public string Name { get; set; } = null!;

        public string Initial { get; set; } = null!;

        public DateTime Dob { get; set; }

        public string? PlaceOfBirth { get; set; }

        public string? MotherTongue { get; set; }

        public string? FatherName { get; set; }

        public string? FatherOccupation { get; set; }

        public int FatherIncome { get; set; }

        public string BloodGroup { get; set; } = null!;

        public string? MotherName { get; set; }

        public string? MotherOccupation { get; set; }

        public int MotherIncome { get; set; }

        public string AadharCardNo { get; set; } = null!;

        public string MobileNo { get; set; } = null!;

        public string? MobileNo2 { get; set; }

        public string? StudentMobileNumber { get; set; }

        public string? Emailid { get; set; }

        public string? FirstLanguage { get; set; }

        public string? Parents { get; set; }

        public string? Religion { get; set; }

        public string? Community { get; set; }

        public string? Caste { get; set; }

        public string? GuardianName { get; set; }

        public string? ExtraCurricularActivities { get; set; }

        public string? PhysicalDisability { get; set; }

        public string? Volunteers { get; set; }

        public string CourseType { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string? ParmanentAddress1 { get; set; }

        public string? ParmanentAddress2 { get; set; }

        public string? ParmanentAddressPincode { get; set; }

        public string? ParmanentAddressPostOffice { get; set; }

        public string? ParmanentAddressDistrict { get; set; }

        public string? ParmanentAddressState { get; set; }

        public string? CommunicationAddress1 { get; set; }

        public string? CommunicationAddress2 { get; set; }

        public string? CommunicationAddressPincode { get; set; }

        public string? CommunicationAddressPostOffice { get; set; }

        public string? CommunicationAddressDistrict { get; set; }

        public string? CommunicationAddressState { get; set; }

        public DateTime? DateOfAdmission { get; set; }

        public string? BroSysStudyingStudied { get; set; }

        public string? NameBroSys { get; set; }

        public string? ModeOftransport { get; set; }

        public string? BoardingPoint { get; set; }

        public string? Hostel { get; set; }

        public string? ScholarShip { get; set; }

        public string? ScholarShipType { get; set; }

        public string? CharityScholarship { get; set; }

        public decimal CharityAmount { get; set; }

        public string? ManagementScholarship { get; set; }

        public string? Quota { get; set; }

        public decimal? Concession { get; set; }

        public string? Remark { get; set; }

        public string? Referredby { get; set; }

        public DateTime? TcreceivedDate { get; set; }

        public string? DocumentEnclosed { get; set; }

        public string? DocumentNotEnclosed { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? LastDate { get; set; }

        public string? Reasonforleaving { get; set; }

        
        public string RollNo { get; set; } = null!;

        public string ExamRegisterNumber { get; set; } = null!;

        public string Year { get; set; } = null!;

        public string Class { get; set; } = null!;

        public string Section { get; set; } = null!;

        public string ClassSection { get; set; } = null!;
        public int InstitutionCode { get; set; }

        public string EnteredBy { get; set; } = null!;

        public DateTime EntryDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
