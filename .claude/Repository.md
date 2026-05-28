# Libraries/Repository — Claude Instructions

## Purpose

This library owns all data access. It contains the EF Core `DbContext`, entity definitions, and repository implementations. Nothing above this layer touches the database directly.

## Folder Structure

```
Libraries/Repository/
├── Entity/
│   ├── SchoolManagementContext.cs     ← Single DbContext for entire app
│   └── (entity classes listed below)
├── StudentRepository/
│   ├── IStudentRepo.cs
│   └── StudentRepo.cs
├── ClassRepository/
│   ├── IClassRepo.cs
│   └── ClassRepo.cs
├── ClassSectionRepository/
├── AcademicYearRepository/
├── StaffRepository/
├── UserRepository/
├── DocumentLibraryRepository/
├── FeesTypeRepository/
├── StudentFeesTransactionRepository/
├── CommonRepository/
├── ReportRepository/
├── InstitutionDetailsRepository/
└── DependencyInjection.cs
```

## DbContext — SchoolManagementContext

Single context for the whole application. Connection string from `appsettings.json → DefaultConnection`.

**All DbSet properties (plural names):**

| DbSet | Entity |
|---|---|
| `AcademicYears` | AcademicYear |
| `AdminUsers` | AdminUser |
| `AllIndiaPincodeData` | AllIndiaPincodeDatum |
| `AuditTables` | AuditTable |
| `Classes` | Class |
| `ClassDetailsViews` | ClassDetailsView *(keyless view)* |
| `ClassSections` | ClassSection |
| `DocumentLibraries` | DocumentLibrary |
| `FeesTypes` | FeesType |
| `InstitutionDetails` | InstitutionDetail |
| `ListOfBankInIndia` | ListOfBankInIndium |
| `SmspassTables` | SmspassTable |
| `StaffDetails` | StaffDetail |
| `StaffEducationDetails` | StaffEducationDetail |
| `StaffExperiences` | StaffExperience |
| `StaffLanguageDetails` | StaffLanguageDetail |
| `StaffMasterViews` | StaffMasterView *(keyless view)* |
| `StaffPassTables` | StaffPassTable |
| `StudentClassDetails` | StudentClassDetail |
| `StudentDetails` | StudentDetail |
| `StudentFeesTransactions` | StudentFeesTransaction |
| `StudentMasterViews` | StudentMasterView *(keyless view)* |
| `StudentPassTables` | StudentPassTable |

## Entity Conventions

**Standard columns every transactional entity has:**

```csharp
public int SysId { get; set; }                       // PK
public int InstitutionCode { get; set; }             // tenant identifier
public string? EnteredBy { get; set; }               // set from APIRequestDetails.UserName
public DateTime? EntryDate { get; set; }             // DB default: GETDATE()
public string? ModifiedBy { get; set; }              // set from APIRequestDetails.UserName
public DateTime? ModifiedDate { get; set; }          // DB default: GETDATE()
public string? Status { get; set; }                  // DB default: 'Active'
```

**OnModelCreating patterns to use consistently:**

```csharp
// PK with named constraint
entity.HasKey(e => e.SysId).HasName("PK_TableName_SysId");

// Timestamp defaults
entity.Property(e => e.EntryDate).HasDefaultValueSql("(getdate())");
entity.Property(e => e.ModifiedDate).HasDefaultValueSql("(getdate())");

// Status default
entity.Property(e => e.Status).HasDefaultValueSql("('Active')");

// Audit trigger
entity.ToTable(tb => tb.HasTrigger("TableNameAudit"));

// FK relationship — never cascade delete
entity.HasOne(d => d.NavProp)
      .WithMany(p => p.Collection)
      .HasForeignKey(d => d.FkidColumn)
      .OnDelete(DeleteBehavior.ClientSetNull)
      .HasConstraintName("FK_Table_RefTable");

// Keyless view
entity.HasNoKey().ToView("ViewName");
```

## Repository Interface Pattern

Every repository follows this exact interface shape:

```csharp
public interface IXxxRepo
{
    Task<int> AddXxxAsync(AddXxxRequest request, APIRequestDetails apiRequestDetails);
    Task<List<XxxResponse>> GetXxxListAsync(APIRequestDetails apiRequestDetails);
    Task<XxxResponse?> GetXxxByIdAsync(GetXxxByIdRequest request, APIRequestDetails apiRequestDetails);
    Task<bool> UpdateXxxAsync(XxxResponse entity, APIRequestDetails apiRequestDetails);
    Task<bool> IsXxxExistsAsync(AddXxxRequest request, APIRequestDetails apiRequestDetails);
}
```

## Repository Implementation Pattern

```csharp
public class XxxRepo : IXxxRepo
{
    private readonly SchoolManagementContext _context;

    public XxxRepo(SchoolManagementContext context)
    {
        _context = context;
    }

    public async Task<int> AddXxxAsync(AddXxxRequest request, APIRequestDetails apiRequestDetails)
    {
        var entity = new XxxEntity
        {
            // map fields
            InstitutionCode = apiRequestDetails.InstitutionCode,
            EnteredBy = apiRequestDetails.UserName,
            ModifiedBy = apiRequestDetails.UserName,
        };
        _context.XxxEntities.Add(entity);
        await _context.SaveChangesAsync();
        return entity.SysId;
    }

    public async Task<List<XxxResponse>> GetXxxListAsync(APIRequestDetails apiRequestDetails)
    {
        return await _context.XxxEntities
            .AsNoTracking()
            .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode)
            .Select(x => new XxxResponse { ... })
            .ToListAsync();
    }
}
```

## Key Rules

- **Always filter by `InstitutionCode`** on every query — no exceptions
- **Use `.AsNoTracking()`** on all read-only queries
- **Use `async/await` throughout** — no `.Result` or `.Wait()`
- **Return the new `SysId`** (int) from Add methods after `SaveChangesAsync()`
- **Return `bool`** from Update methods (`true` = success)
- **Use database transactions** for operations that touch multiple tables:

```csharp
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // multiple SaveChangesAsync calls
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

## Existence Check Pattern

Used before Add to prevent duplicates:

```csharp
public async Task<bool> IsXxxExistsAsync(AddXxxRequest request, APIRequestDetails apiRequestDetails)
{
    return await _context.XxxEntities
        .AnyAsync(x => x.InstitutionCode == apiRequestDetails.InstitutionCode
                     && x.UniqueField == request.UniqueField);
}
```

## ID Generation Pattern (Student / Staff)

Custom sequential ID generation queries the max existing ID filtered by InstitutionCode, then increments. See `StudentRepo.GenerateStudentIdAsync` for the reference implementation.

## Dependency Injection Registration

In `Libraries/Repository/DependencyInjection.cs`:

```csharp
public static IServiceCollection RepositoryDependencyInjection(
    this IServiceCollection service, IConfiguration config)
{
    service.AddDbContext<SchoolManagementContext>(option =>
        option.UseSqlServer(config.GetConnectionString("DefaultConnection")));

    service.AddScoped<IXxxRepo, XxxRepo>(); // add new repos here
    return service;
}
```

## Repository Inventory

| Interface | Implementation | Responsibility |
|---|---|---|
| `IStudentRepo` | `StudentRepo` | Student CRUD, transactional add (Student + Pass + ClassDetail), autocomplete, password reset |
| `IClassRepo` | `ClassRepo` | Class CRUD, status updates, duplicate check |
| `IClassSectionRepo` | `ClassSectionRepo` | Section CRUD linked to Class via ClassFkid |
| `IAcademicYearRepo` | `AcademicYearRepo` | Academic year CRUD, active/inactive filtering |
| `IStaffRepo` | `StaffRepo` | Staff CRUD with related education/experience/language tables |
| `IDocumentLibraryRepo` | `DocumentLibraryRepo` | Generic binary file storage linked to any entity via Fkid + TableName |
| `IFeesTypeRepo` | `FeesTypeRepo` | Fees type reference data CRUD |
| `IStudentFeesTransactionRepo` | `StudentFeesTransactionRepo` | Debit/credit transaction records |
| `ICommonRepo` | `CommonRepo` | Pincode lookup, bank list, logo/favicon, audit history, datetime |
| `IUserRepo` | `UserRepo` | Login credential check, password change, admin user management |
| `IReportRepo` | `ReportRepo` | Custom report queries |
| `IInstitutionDetailsRepo` | `InstitutionDetailsRepo` | Institution settings (name, logo, contact) |