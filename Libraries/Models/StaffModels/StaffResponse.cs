using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.DocumentLibraryModels;

namespace Models.StaffModels
{
    public class StaffCountResponse
    {
        public int TotalStaff { get; set; }
        public int Male { get; set; }
        public int Female { get; set; }
        public int Teaching { get; set; }
        public int NonTeaching { get; set; }
    }
    public class DesignationListResponse
    {
        public string Designation { get; set; }
        public int DesignationCode { get; set; }
    }
    public class StaffDetailSearchResponse
    {
        public int SysId { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string StaffType { get; set; }
        public string Gender { get; set; }
        public string EnteredBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? Guid { get; set; }
    }
    public class StaffDetailsResponse
    {
        public required StaffDetailResponse StaffDetail { get; set; }
        public List<StaffLanguageResponse> StaffLanguage { get; set; } = new List<StaffLanguageResponse>();
        public List<StaffEducationResponse> StaffEducation { get; set; } = new List<StaffEducationResponse>();
        public List<StaffExperienceResponse> StaffExprience { get; set; } = new List<StaffExperienceResponse>();
        public List<DocumentLibraryDetailsResponse> StaffDocument { get; set; } = new List<DocumentLibraryDetailsResponse>();

    }
    public class StaffDetailResponse
    {
        public string StaffID { get; set; }
        public string StaffType { get; set; }
        public string EnteredBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public string Sex { get; set; }
        public string BloodGroup { get; set; }
        public DateTime DOB { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string PhysicalDisability { get; set; }
        public string Community { get; set; }
        public string Caste { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string AadharCardNo { get; set; }
        public int DesignationCode { get; set; }
        public DateTime? DOJ { get; set; }

        public string PermanentAddress1 { get; set; }
        public string PermanentAddress2 { get; set; }
        public string PermanentAddressPincode { get; set; }
        public string PermanentAddressPostOffice { get; set; }
        public string PermanentAddressDistrict { get; set; }
        public string PermanentAddressState { get; set; }

        public string CommunicationAddress1 { get; set; }
        public string CommunicationAddress2 { get; set; }
        public string CommunicationAddressPincode { get; set; }
        public string CommunicationAddressPostOffice { get; set; }
        public string CommunicationAddressDistrict { get; set; }
        public string CommunicationAddressState { get; set; }

        public string Status { get; set; }
        public string MotherTongue { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string AccountNumber { get; set; }
        public string MICRCode { get; set; }
        public string PANCardNo { get; set; }
        public Guid? Guid { get; set; }
    }

    public class StaffLanguageResponse
    {
        public int SysId { get; set; }
        public string language { get; set; } = null!;

        public string ReadLanguage { get; set; } = null!;

        public string WriteLanguage { get; set; } = null!;

        public string SpeakLanguage { get; set; } = null!;
    }
    public class StaffEducationResponse
    {
        public int SysId { get; set; }
        public string DegreeType { get; set; }
        public string Degree { get; set; }
        public int YearOfPassing { get; set; }
        public string UniversityName { get; set; }
        public string InstitutionName { get; set; }
        public string Mode { get; set; }
        public string PassPercentage { get; set; }
        public string Specialization { get; set; }
    }
    public class StaffExperienceResponse
    {
        public int SysId { get; set; }
        public string InstitutionName { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Salary { get; set; }
    }
}
