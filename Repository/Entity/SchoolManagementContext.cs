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

    public virtual DbSet<InstitutionDetail> InstitutionDetails { get; set; }

    public virtual DbSet<SmspassTable> SmspassTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Connection string is configured via dependency injection
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
            entity.Property(e => e.EntryDate).HasColumnType("datetime");
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
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
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

            entity.Property(e => e.Entrydate).HasColumnType("datetime");
            entity.Property(e => e.FkInstitutionDetails).HasColumnName("FK_InstitutionDetails");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Validitydate).HasColumnType("date");

            entity.HasOne(d => d.FkInstitutionDetailsNavigation).WithMany(p => p.SmspassTables)
                .HasForeignKey(d => d.FkInstitutionDetails)
                .HasConstraintName("FK_SMSPassTable_InstitutionDetails");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
