using System;
using System.Collections.Generic;

namespace Repository.Entity;

public partial class StaffMasterView
{
    public int Sysid { get; set; }

    public string StaffId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Initial { get; set; } = null!;

    public string Staffname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Sex { get; set; } = null!;

    public DateTime Dob { get; set; }

    public int? Age { get; set; }

    public DateTime Doj { get; set; }

    public DateTime? Dor { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? Religion { get; set; }

    public string? Community { get; set; }

    public string? Cast { get; set; }

    public string? PhysicalDisablity { get; set; }

    public string MobileNo { get; set; } = null!;

    public string? Emailid { get; set; }

    public string MaritalStatus { get; set; } = null!;

    public string? AddharCardNo { get; set; }

    public string BloodGroup { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public int DesignationCode { get; set; }

    public string StaffType { get; set; } = null!;

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

    public string? MotherTongue { get; set; }

    public string? Ifsccode { get; set; }

    public string? BankName { get; set; }

    public string? BankAddress { get; set; }

    public string? AccountNumber { get; set; }

    public string? Micrcode { get; set; }

    public string? PancardNo { get; set; }

    public int InstitutionCode { get; set; }

    public string Status { get; set; } = null!;

    public string EnteredBy { get; set; } = null!;

    public DateTime Entrydate { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime ModifiedDate { get; set; }

    public Guid? Guid { get; set; }
}
