using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.StaffModels
{
    public class StaffAutocompleteRequest
    {
        public required string TableName { get; set; }
        public required string ColumnName { get; set; } = null!;
        public string SearchParam { get; set; }
    }
    public class AddStaffRequest
    {
        public StaffDetailsAddRequest staffdetails { get; set; }
        public List<StaffLanguageDetailAddRequest> LanguageRequests { get; set; }
        public List<StaffEducationDetailAddRequest> EducationRequests { get; set; }
        public List<StaffExperienceDetailAddRequest> ExperienceRequests { get; set; }
        public List<DocumentLibraryBulkInsert> DocumentRequests { get; set; }
    }
    public class StaffDetailsAddRequest
    {
        public string Title { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Initial { get; set; } = null!;
        public string Sex { get; set; } = null!;
        public DateTime Dob { get; set; }
        public DateTime Doj { get; set; }
        public DateTime? Dor { get; set; }

        public string? PlaceOfBirth { get; set; }
        public string? religion { get; set; }
        public string? community { get; set; }
        public string? caste { get; set; }
        public string? physicalDisability { get; set; }

        public string MobileNo { get; set; } = null!;
        public string? Emailid { get; set; }
        public string? MotherTongue { get; set; }
        public string MaritalStatus { get; set; } = null!;
        public string? AadharCardNo { get; set; }
        public string BloodGroup { get; set; } = null!;

        public string Designation { get; set; } = null!;
        public int DesignationCode { get; set; }
        public string StaffType { get; set; } = null!;

        public string? PermanentAddress1 { get; set; }
        public string? PermanentAddress2 { get; set; }
        public string? PermanentAddressPincode { get; set; }
        public string? PermanentAddressPostOffice { get; set; }
        public string? PermanentAddressDistrict { get; set; }
        public string? PermanentAddressState { get; set; }

        public string? CommunicationAddress1 { get; set; }
        public string? CommunicationAddress2 { get; set; }
        public string? CommunicationAddressPincode { get; set; }
        public string? CommunicationAddressPostOffice { get; set; }
        public string? CommunicationAddressDistrict { get; set; }
        public string? CommunicationAddressState { get; set; }

        public string? Ifsccode { get; set; }
        public string? BankName { get; set; }
        public string? BankAddress { get; set; }
        public string? AccountNumber { get; set; }
        public string? Micrcode { get; set; }
        public string? PancardNo { get; set; }
        public string? ImageFileName { get; set; }

        public string? ImageContentType { get; set; }

        public string? ImageData { get; set; }
    }

    public partial class StaffLanguageDetailAdd : StaffLanguageDetailAddRequest
    {
        public int Id { get; set; }
        public int InstitutionCode { get; set; }
    }
    public partial class StaffLanguageDetailAddRequest
    {
        public string language { get; set; } = null!;

        public string ReadLanguage { get; set; } = null!;

        public string WriteLanguage { get; set; } = null!;

        public string SpeakLanguage { get; set; } = null!;

    }
    public partial class StaffEducationDetailAddRequest
    {
        public string DegreeType { get; set; }

        public string Degree { get; set; }

        public int YearOfpassing { get; set; }

        public string UniversityName { get; set; }

        public string InstitutionName { get; set; }

        public string Mode { get; set; }

        public string PassPercentage { get; set; }

        public string Specialization { get; set; }

    }
    public partial class StaffEducationDetailAdd : StaffEducationDetailAddRequest
    {
        public int Id { get; set; }
        public int InstitutionCode { get; set; }
    }
    public partial class StaffExperienceDetailAddRequest
    {
        public string InstitutionName { get; set; }

        public string Position { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime Todate { get; set; }
        public int Salary { get; set; }

    }
    public partial class StaffExperienceDetailAdd : StaffExperienceDetailAddRequest
    {
        public int Id { get; set; }
        public int InstitutionCode { get; set; }
    }
    public class DocumentLibraryBulkInsert
    {
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required string Data { get; set; }
        public required string FileType { get; set; } = null!;

    }
    public partial class DocumentLibraryBulkInsertByFKID : DocumentLibraryBulkInsert
    {
        public required int FKID { get; set; }
    }
    public class StaffSearchRequest
    {
        public required string ColumnName { get; set; } = null!;
        public  string SearchParam { get; set; }
    }
    public partial class StaffDetailsPK
    {
        public required int SysId { get; set; }
    }
    public partial class UpdateAddStaffLanguageDetail : StaffLanguageDetailAddRequest
    {
        public required int SysId { get; set; }
    }
    public partial class UpdateAddStaffEducationDetailAdd : StaffEducationDetailAddRequest
    {
        public required int SysId { get; set; }
    }
    public partial class UpdateStaffExperienceDetailAddRequest : StaffExperienceDetailAddRequest
    {
        public required int SysId { get; set; }
    }
    public class DocumentLibraryUpdate
    {
        public long Sysid { get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
        public required byte[] Data { get; set; }
        public required string ModifiedBy { get; set; }

    }
    public partial class UpdateStaffDetailsRequest
    {
        public int Sysid { get; set; }
        public string StaffType { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Initial { get; set; }
        public string sex { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOR { get; set; }
        public string PlaceOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string PhysicalDisability { get; set; }
        public string BloodGroup { get; set; }
        public string Community { get; set; }
        public string Caste { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string AadharCardNo { get; set; }
        public string Designation { get; set; }
        public int DesignationCode { get; set; }
        public DateTime DOJ { get; set; }
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
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string AccountNumber { get; set; }
        public string MICRCode { get; set; }
        public string PANCardNo { get; set; }
        public string MotherTongue { get; set; }
        public string Status { get; set; }
    }
    public partial class StaffDetailsPasswordReset : StaffDetailsPK
    {
        public required string Password { get; set; }
    }
}
