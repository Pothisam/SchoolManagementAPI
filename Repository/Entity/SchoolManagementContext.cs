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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=WIN-J0V379CPS02;Database=SchoolManagement;User ID=CMS;Password=CMS@123;Trusted_Connection=False;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InstitutionDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable(tb => tb.HasTrigger("trg_UpdateModifiedDate"));

            entity.Property(e => e.Address1)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AlternateMobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.DistrictName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.EmailId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("EmailID");
            entity.Property(e => e.EntryBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EntryDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FavIconContentType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FavIconFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InstitutionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Landline)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LogoContentType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LogoFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LogoWithTextContentType)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LogoWithTextFileName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MobileNo)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.PinCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.StaffIdprefix)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("StaffIDPrefix");
            entity.Property(e => e.StateName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.SysId).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
