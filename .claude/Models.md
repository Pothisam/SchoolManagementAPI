# Libraries/Models — Claude Instructions

## Purpose

This library contains all DTOs (Data Transfer Objects), request/response wrappers, and configuration models shared across all layers. **No business logic belongs here.**

## Folder Structure

```
Libraries/Models/
├── CommonModels/          ← Shared wrappers used by every feature
├── StudentModels/         ← Student request/response DTOs
├── ClassModels/           ← Class request/response DTOs
├── ClassSectionModels/    ← Class section DTOs
├── AcademicYearModels/    ← Academic year DTOs
├── StaffModels/           ← Staff request/response DTOs
├── FeesModels/            ← Fees type + transaction DTOs
├── UserModels/            ← Login, password, admin user DTOs
├── DocumentLibraryModels/ ← File upload/download DTOs
├── ReportModels/          ← Report-specific response DTOs
└── InstitutionModels/     ← Institution details DTOs
```

## Naming Rules

- **Request DTOs**: suffix `Request` — e.g., `AddStudentRequest`, `UpdateClassStatusRequest`
- **Response DTOs**: suffix `Response` — e.g., `StudentMasterViewResponse`, `ClassResponse`
- **Folder**: create a new `XxxModels/` folder for each new domain feature

## CommonModels (Core Types)

### CommonResponse\<T\>

The universal response envelope. Every service method returns this.

```csharp
public class CommonResponse<T> where T : class
{
    public string? Message { get; set; }
    public Status Status { get; set; }
    public T? Data { get; set; }
}

public enum Status
{
    Success = 200,
    Failed = 300
}
```

### APIRequestDetails

Populated from JWT claims by `CommonService.GetAPIRequestDetails(User)`. Passed into every service and repo method to carry the caller's identity and tenant.

```csharp
public class APIRequestDetails
{
    public required string UserName { get; set; }
    public required int InstitutionCode { get; set; }   // tenant filter
    public required string LoginType { get; set; }
    public required int SysId { get; set; }
    public required bool Ispricipal { get; set; }        // note: existing typo, do not rename
}
```

### AutoCompleteRequest / AutoCompleteResponse

Generic autocomplete used across multiple features.

```csharp
public class AutoCompleteRequest
{
    public string? TableName { get; set; }
    public string? ColumnName { get; set; }
    public string? SearchParam { get; set; }
}

public class AutoCompleteResponse
{
    public string? Column { get; set; }
}
```

## Authentication Models

```csharp
// Login
public class LoginRequest
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

// Contains IP for token claim binding
public class LoginRequestwithIP
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? IPAddress { get; set; }
}

// Returned on successful login
public class LoginResponse
{
    public int SysId { get; set; }
    public string? UserName { get; set; }
    public string? Token { get; set; }
    public string? LoginType { get; set; }
    public string? InstitutionType { get; set; }
    public int InstitutionCode { get; set; }
    public string? Guid { get; set; }
    public bool IsPrincipal { get; set; }
}

public class ChangePasswordRequest
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}
```

## Configuration Models

```csharp
public class JwtConfig
{
    public string? Issuer { get; set; }
    public string? Audince { get; set; }   // note: existing typo, do not rename
}

public class AppKeyConfig
{
    public int? TokenExpiry { get; set; }  // minutes; default 1440 (24 hours)
}

public static class JwtKey
{
    public static readonly string AuthKey = "SVuZbh2ICMYjZydFkDBjjgh9P0C52oJto/xuqMvATvwr/g3lVfl7dWZdOQcv6IIg";
}
```

## Document Library Models

```csharp
// For uploading — Data is base64-encoded binary
public class DocumentLibraryBulkInsert
{
    public string? FileName { get; set; }
    public string? ContentType { get; set; }
    public string? Data { get; set; }
    public string? FileType { get; set; }
}

// For listing attached documents
public class DocumentLibraryDetailsResponse
{
    public int Sysid { get; set; }
    public string? FileName { get; set; }
    public string? FileType { get; set; }
    public long? FileSize { get; set; }
    public string? Guid { get; set; }
    public string? EnteredBy { get; set; }
    public DateTime? EntryDate { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
```

## Common Lookup Response DTOs

```csharp
public class PostOfficeRequest  { public string? pincode { get; set; } }
public class PostOfficeResponse { public string? OfficeName { get; set; } public string? Districtname { get; set; } public string? StateName { get; set; } }
public class BankResponse       { public string? BankName { get; set; } }
```

## Conventions to Follow

- Use `string?` (nullable) for optional string fields; append `= null!` to avoid compiler warnings where appropriate
- Use `int?` for optional numeric IDs
- Do **not** add data annotations (`[Required]`, `[StringLength]`) — validation is handled in the service layer
- Keep DTOs flat — no nesting of complex objects unless absolutely necessary
- Request DTOs contain only input fields; Response DTOs contain only display fields
- Never include navigation properties or EF Core entity references in DTOs