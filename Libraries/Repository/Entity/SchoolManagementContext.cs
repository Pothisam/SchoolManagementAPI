using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repository.Entity;

public partial class SchoolManagementContext : DbContext
{
    public SchoolManagementContext()
    {
    }

    public SchoolManagementContext(DbContextOptions<SchoolManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AcademicYear> AcademicYears { get; set; }

    public virtual DbSet<AuditTable> AuditTables { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassSection> ClassSections { get; set; }

    public virtual DbSet<InstitutionDetail> InstitutionDetails { get; set; }

    public virtual DbSet<SmspassTable> SmspassTables { get; set; }

    public virtual DbSet<StaffDetail> StaffDetails { get; set; }

    public virtual DbSet<StaffEducationDetail> StaffEducationDetails { get; set; }

    public virtual DbSet<StaffLanguageDetail> StaffLanguageDetails { get; set; }

    public virtual DbSet<StaffPassTable> StaffPassTables { get; set; }

    public virtual DbSet<StudentClassDetail> StudentClassDetails { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<StudentPassTable> StudentPassTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=SARASC34693;Database=SchoolManagement;User ID=CMS;Password=Q8w$3Lm9#Vr7Xp2!;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AcademicYear>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_AcademicYear_SysId");

            entity.ToTable("AcademicYear", tb => tb.HasTrigger("AcademicYearAudit"));

            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Year)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AuditTable>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AuditTable");

            entity.Property(e => e.Application)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Data).IsUnicode(false);
            entity.Property(e => e.Fid).HasColumnName("FID");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Modifiedby)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sysid).ValueGeneratedOnAdd();
            entity.Property(e => e.TableAction)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_Class_SysId");

            entity.ToTable("Class", tb => tb.HasTrigger("ClassAudit"));

            entity.Property(e => e.ClassName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ClassSection>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_ClassSection_SysId");

            entity.ToTable("ClassSection", tb => tb.HasTrigger("ClassSectionAudit"));

            entity.Property(e => e.ClassFkid).HasColumnName("ClassFKID");
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SectionName)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.ClassFk).WithMany(p => p.ClassSections)
                .HasForeignKey(d => d.ClassFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClassSection_Class");
        });

        modelBuilder.Entity<InstitutionDetail>(entity =>
        {
            entity.HasKey(e => e.Sysid);

            entity.Property(e => e.Address1)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.AlternateMobileNumer)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Districtname)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Emailid)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FaviconContentType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FaviconFileName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Landline)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.LogoContentType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("logoContentType");
            entity.Property(e => e.LogoData).HasColumnName("logoData");
            entity.Property(e => e.LogoFileName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("logoFileName");
            entity.Property(e => e.LogoWithTextContentType)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("logoWithTextContentType");
            entity.Property(e => e.LogoWithTextData).HasColumnName("logoWithTextData");
            entity.Property(e => e.LogoWithTextFileName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("logoWithTextFileName");
            entity.Property(e => e.MobileNumer)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OfficialMail)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.PostofficeName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffIdprefix)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("StaffIDPrefix");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Website)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("website");
        });

        modelBuilder.Entity<SmspassTable>(entity =>
        {
            entity.HasKey(e => e.Sysid);

            entity.ToTable("SMSPassTable");

            entity.Property(e => e.Entrydate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Validitydate).HasColumnType("date");

            entity.HasOne(d => d.InstitutionCodeNavigation).WithMany(p => p.SmspassTables)
                .HasForeignKey(d => d.InstitutionCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SMSPassTable_InstitutionDetails");
        });

        modelBuilder.Entity<StaffDetail>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StaffDetails_SysId");

            entity.ToTable(tb => tb.HasTrigger("StaffDetailsAudit"));

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AddharCardNo)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Age)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.BankAddress)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.BankName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Cast)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddress1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddress2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressDistrict)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressPincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressPostOffice)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressState)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Community)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Designation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.Doj)
                .HasColumnType("date")
                .HasColumnName("DOJ");
            entity.Property(e => e.Dor)
                .HasColumnType("date")
                .HasColumnName("DOR");
            entity.Property(e => e.Emailid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Initial)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.MaritalStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Micrcode)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("MICRCode");
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MotherTongue)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PancardNo)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PANCardNo");
            entity.Property(e => e.ParmanentAddress1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddress2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressDistrict)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressPincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressPostOffice)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressState)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhysicalDisablity)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Religion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StaffId)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("StaffID");
            entity.Property(e => e.StaffType)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(5)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StaffEducationDetail>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StaffEducationDetails_SysId");

            entity.ToTable(tb => tb.HasTrigger("StaffEducationDetailsAudit"));

            entity.Property(e => e.Degree)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.DegreeType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.InstituionName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Mode)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PassPercentage)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Specialization)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StaffDetailsFkid).HasColumnName("StaffDetailsFKID");
            entity.Property(e => e.UniversityName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.YearOfpassing).HasColumnName("YearOFPassing");

            entity.HasOne(d => d.StaffDetailsFk).WithMany(p => p.StaffEducationDetails)
                .HasForeignKey(d => d.StaffDetailsFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffEducationDetails_StaffDetails");
        });

        modelBuilder.Entity<StaffLanguageDetail>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StaffLanguageDetails_SysId");

            entity.ToTable(tb => tb.HasTrigger("StaffLanguageDetailsAudit"));

            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LanguageKnow)
                .HasMaxLength(35)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ReadLanguage)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.SpeakLanguage)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.StaffFkid).HasColumnName("StaffFKID");
            entity.Property(e => e.WriteLanguage)
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.HasOne(d => d.StaffFk).WithMany(p => p.StaffLanguageDetails)
                .HasForeignKey(d => d.StaffFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffLanguageDetails_StaffDetails");
        });

        modelBuilder.Entity<StaffPassTable>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StaffPassTable_SysId");

            entity.ToTable("StaffPassTable", tb => tb.HasTrigger("StaffPassTableAudit"));

            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Otp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("OTP");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.StaffDetailsFkid).HasColumnName("StaffDetailsFKID");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.StaffDetailsFk).WithMany(p => p.StaffPassTables)
                .HasForeignKey(d => d.StaffDetailsFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StaffPassTable_StaffDetails");
        });

        modelBuilder.Entity<StudentClassDetail>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StudentClassDetails_SysId");

            entity.ToTable(tb => tb.HasTrigger("StudentClassDetailsAudit"));

            entity.Property(e => e.AcademicYearFkid).HasColumnName("AcademicYearFKID");
            entity.Property(e => e.ClassSectionFkid).HasColumnName("ClassSectionFKID");
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExamRegisterNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.RollNo)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.StudentDetailsFkid).HasColumnName("StudentDetailsFKID");

            entity.HasOne(d => d.AcademicYearFk).WithMany(p => p.StudentClassDetails)
                .HasForeignKey(d => d.AcademicYearFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentClassDetails_AcademicYear");

            entity.HasOne(d => d.ClassSectionFk).WithMany(p => p.StudentClassDetails)
                .HasForeignKey(d => d.ClassSectionFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentClassDetails_ClassSection");

            entity.HasOne(d => d.StudentDetailsFk).WithMany(p => p.StudentClassDetails)
                .HasForeignKey(d => d.StudentDetailsFkid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentClassDetails_StudentDetails");
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StudentDetails_SysId");

            entity.ToTable(tb => tb.HasTrigger("StudentDetailsAudit"));

            entity.Property(e => e.AadharCardNo)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.AdmissionNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.AdmissionSerialNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.ApplicationNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.BoardingPoint)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BroSysStudyingStudied)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Caste)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CharityAmount).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.CharityScholarship)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddress1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddress2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressDistrict)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressPincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressPostOffice)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CommunicationAddressState)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Community)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Concession).HasColumnType("numeric(8, 2)");
            entity.Property(e => e.CourseType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DateOfAdmission).HasColumnType("date");
            entity.Property(e => e.Dob)
                .HasColumnType("date")
                .HasColumnName("DOB");
            entity.Property(e => e.DocumentEnclosed)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.DocumentNotEnclosed)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Emailid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExtraCurricularActivities)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FatherOccupation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstLanguage)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.GuardianName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Hostel)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Initial)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.LastDate).HasColumnType("date");
            entity.Property(e => e.ManagementScholarship)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo2)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModeOftransport)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ModeOFTransport");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MotherName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MotherOccupation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MotherTongue)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameBroSys)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Parents)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddress1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddress2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressDistrict)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressPincode)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressPostOffice)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParmanentAddressState)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhysicalDisability)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.PlaceOfBirth)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Quota)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Reasonforleaving)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Referredby)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Religion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Remark)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ScholarShip)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ScholarShipType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Stdid)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StudentMobileNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TcreceivedDate)
                .HasColumnType("date")
                .HasColumnName("TCReceivedDate");
            entity.Property(e => e.Volunteers)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StudentPassTable>(entity =>
        {
            entity.HasKey(e => e.SysId).HasName("PK_StudentPassTable_SysId");

            entity.ToTable("StudentPassTable", tb => tb.HasTrigger("StudentPassTableAudit"));

            entity.Property(e => e.EnteredBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Otp)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("OTP");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.StudentDetailsFkid).HasColumnName("StudentDetailsFKID");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.StudentDetailsFk).WithMany(p => p.StudentPassTables)
                .HasForeignKey(d => d.StudentDetailsFkid)
                .HasConstraintName("FK_StudentPassTable_StudentDetails");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
