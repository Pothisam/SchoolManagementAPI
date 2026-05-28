# Libraries/Services — Claude Instructions

## Purpose

This library contains all business logic. Services validate input, coordinate repository calls, and always return a `CommonResponse<T>`. Controllers call services; services call repositories — never the other way around.

## Folder Structure

```
Libraries/Services/
├── CommonServices/
│   ├── ICommonService.cs
│   └── CommonService.cs          ← encryption, JWT, claims extraction, shared utilities
├── StudentServices/
│   ├── IStudentService.cs
│   └── StudentService.cs
├── ClassServices/
│   ├── IClassService.cs
│   └── ClassService.cs
├── ClassSectionServices/
├── AcademicYearServices/
├── StaffServices/
├── UserServices/
├── DocumentLibraryServices/
├── FeesTypeServices/
├── StudentFeesTransactionServices/
├── ReportServices/
├── InstitutionDetailsServices/
└── DependencyInjection.cs
```

## Service Interface Pattern

```csharp
public interface IXxxService
{
    Task<CommonResponse<string>> AddXxxAsync(AddXxxRequest request, APIRequestDetails apiRequestDetails);
    Task<CommonResponse<List<XxxResponse>>> GetXxxListAsync(APIRequestDetails apiRequestDetails);
    Task<CommonResponse<XxxResponse>> GetXxxByIdAsync(GetXxxByIdRequest request, APIRequestDetails apiRequestDetails);
    Task<CommonResponse<string>> UpdateXxxAsync(UpdateXxxRequest request, APIRequestDetails apiRequestDetails);
}
```

Use `CommonResponse<string>` for Add/Update operations (return success message or new ID as string).
Use `CommonResponse<List<T>>` for list queries.
Use `CommonResponse<T>` for single-record fetch.

## Service Implementation Pattern

```csharp
public class XxxService : IXxxService
{
    private readonly IXxxRepo _IXxxRepo;
    private readonly ICommonService _ICommonService;

    public XxxService(IXxxRepo xxxRepo, ICommonService commonService)
    {
        _IXxxRepo = xxxRepo;
        _ICommonService = commonService;
    }

    public async Task<CommonResponse<string>> AddXxxAsync(
        AddXxxRequest request, APIRequestDetails apiRequestDetails)
    {
        try
        {
            // 1. Validate — check for duplicates, required fields, business rules
            bool exists = await _IXxxRepo.IsXxxExistsAsync(request, apiRequestDetails);
            if (exists)
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Record already exists."
                };

            // 2. Execute — call repository
            int newId = await _IXxxRepo.AddXxxAsync(request, apiRequestDetails);

            // 3. Return success
            return new CommonResponse<string>
            {
                Status = Status.Success,
                Message = "Added successfully.",
                Data = newId.ToString()
            };
        }
        catch (Exception ex)
        {
            return new CommonResponse<string>
            {
                Status = Status.Failed,
                Message = ex.Message
            };
        }
    }
}
```

## Key Rules

- **Always wrap in try/catch** and return `Status.Failed` with `ex.Message` on exception — never let exceptions escape a service method
- **All methods are `async Task<CommonResponse<T>>`** — no sync methods
- **Validate before writing** — check duplicates or business rules via repo before calling Add/Update
- **String normalization**: call `.ToUpper()` on name fields (StudentName, StaffName) when mapping to entity
- **Never expose raw entities** — always map to Response DTOs before returning
- **Pass `APIRequestDetails` into every repo call** — it carries the tenant code and caller identity

## CommonService — Shared Utilities

`ICommonService` / `CommonService` is injected into every other service and all controllers. Key methods:

### GetAPIRequestDetails

Extracts the caller's identity from JWT claims. Called in every controller action before any service call.

```csharp
APIRequestDetails apirequestdetails = _ICommonService.GetAPIRequestDetails(User);
```

### Encrypt

AES-256 password encryption. Use for storing and comparing passwords.

```csharp
string encrypted = await _ICommonService.Encrypt(plainTextPassword);
```

### CreateJWTToken

Called only in `UserService` after successful login credential check.

```csharp
string token = _ICommonService.CreateJWTToken(claims);
```

### GetLogo / GetLogoWithText / GetFavIcon

Returns binary image data for the institution. Use in any feature that needs to embed institution branding.

### GetRecordHistory

Returns the audit trail for any record. Pass `TableName` + `ForeignId`:

```csharp
var history = await _ICommonService.GetRecordHistory(tableName, foreignId, apiRequestDetails);
```

### GetPostOffice / GetBankDetailsAsync

Reference data lookups. Called from `CommonController` — not from domain services.

## Student Service — Reference Implementation

The most complex service. Key flow for `AddStudent`:

1. Check duplicate Aadhaar (`IStudentRepo.IsDuplicateAadharAsync`)
2. Generate sequential StudentId (`IStudentRepo.GenerateStudentIdAsync`)
3. Map `AddStudentRequest` → `StudentDetail` entity (ToUpper all name fields)
4. Build `StudentPassTable` (encrypt password via `ICommonService.Encrypt`)
5. Build `StudentClassDetail`
6. Call `IStudentRepo.AddStudent(studentDetail, passTable, classDetail)` — single transaction
7. If photo data present in request, call `IDocumentLibraryRepo` to store binary
8. Return `CommonResponse<string>` with new StudentId

## Document Library Service

Used by `StudentService` and `StaffService` to attach files. Never call `IDocumentLibraryRepo` directly from a controller.

```csharp
await _IDocumentLibraryServices.AddDocumentAsync(
    fkid: newStudentId,
    tableName: "StudentDetail",
    documents: request.Documents,      // List<DocumentLibraryBulkInsert>
    apiRequestDetails: apiRequestDetails
);
```

## User Service — Login Flow

1. Receive `LoginRequest` (UserName + Password)
2. Encrypt password via `ICommonService.Encrypt`
3. Query `StaffPassTable` or `StudentPassTable` matching UserName + encrypted password
4. On match, build JWT claims array
5. Call `ICommonService.CreateJWTToken(claims)`
6. Return `CommonResponse<LoginResponse>` with token

## Dependency Injection Registration

In `Libraries/Services/DependencyInjection.cs`:

```csharp
public static IServiceCollection ServicesDependencyInjection(
    this IServiceCollection service, IConfiguration config)
{
    service.RepositoryDependencyInjection(config);  // always call repo DI first

    service.AddScoped<ICommonService, CommonService>();  // register CommonService first
    service.AddScoped<IXxxService, XxxService>();        // add new services here
    return service;
}
```

## Service Inventory

| Interface | Implementation | Responsibility |
|---|---|---|
| `ICommonService` | `CommonService` | Encryption, JWT generation, claims extraction, logo, audit history, reference lookups |
| `IStudentService` | `StudentService` | Student CRUD, class assignment, documents, autocomplete, password reset |
| `IClassService` | `ClassService` | Class CRUD, status toggle |
| `IClassSectionService` | `ClassSectionService` | Section CRUD linked to a Class |
| `IAcademicYearService` | `AcademicYearService` | Academic year CRUD, status management |
| `IStaffService` | `StaffService` | Staff CRUD with education/experience/language, documents, autocomplete, password reset |
| `IUserService` | `UserService` | Login, password change, admin user management, access settings |
| `IDocumentLibraryServices` | `DocumentLibraryService` | Binary file add/get/delete for any entity |
| `IFeesTypeService` | `FeesTypeService` | Fees type reference data management |
| `IStudentFeesTransactionService` | `StudentFeesTransactionService` | Collect fees, view transaction history |
| `IReportService` | `ReportService` | Custom analytical report queries |
| `IInstitutionDetailsService` | `InstitutionDetailsService` | Institution settings read/update |