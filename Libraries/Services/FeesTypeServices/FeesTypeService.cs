using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.FeesTypeModels;
using Repository.Entity;
using Repository.FeesTypeRepository;

namespace Services.FeesTypeServices
{
    public class FeesTypeService : IFeesTypeService
    {
        private readonly IFeesTypeRepo _feesTyperepo;
        public FeesTypeService(IFeesTypeRepo feesTypeRepo)
        {
            _feesTyperepo = feesTypeRepo;
        }
        public async Task<CommonResponse<List<FeesTypeListResponse>>> GetFeesTypeListAsync(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<FeesTypeListResponse>>();

            var result = await _feesTyperepo.GetFeesTypeListAsync(apiRequestDetails);

            if (result.Any())
            {
                response.Status = Status.Success;
                response.Data = result;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "No Active Fees Type Found";
            }

            return response;
        }

        public async Task<CommonResponse<string>> SaveFeesTypeAsync(SaveFeesTypeRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();

            string feesType = (request.FeesTypeDescription ?? string.Empty).Trim().ToUpper();

            if (string.IsNullOrEmpty(feesType))
            {
                response.Status = Status.Failed;
                response.Message = "Fees Type is required";
                return response;
            }
            int sysId = await _feesTyperepo.GetFeesTypeSysIdByNameAsync(request,apiRequestDetails);

            if (sysId == 0)
            {
                var entity = new FeesType
                {
                    FeesDescription = feesType,
                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    Entryby = apiRequestDetails.UserName,
                    Status = "Active"
                };

                await _feesTyperepo.AddFeesTypeAsync(entity);
                await _feesTyperepo.SaveChangesAsync();

                response.Status = Status.Success;
                response.Message = "Fees Type Added Successfully";
                return response;
            }
            var existing = await _feesTyperepo.GetFeesTypeBySysIdAsync(sysId, apiRequestDetails);

            if (existing == null)
            {
                response.Status = Status.Failed;
                response.Message = "Fees Type not found";
                return response;
            }

            existing.Status = "Active";
            existing.Modifiedby = apiRequestDetails.UserName;


            await _feesTyperepo.SaveChangesAsync();

            response.Status = Status.Success;
            response.Message = "Fees Type Updated";
            return response;
        }
        public async Task<CommonResponse<string>> DeleteFeesTypeAsync(FeesTypePKRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();

            var entity = await _feesTyperepo.GetFeesTypeBySysIdAsync(request.Sysid, apiRequestDetails);

            if (entity == null)
            {
                response.Status = Status.Failed;
                response.Message = "Fees Type not found";
                return response;
            }

            entity.Status = entity.Status == "Active" ? "Inactive" : "Active";
            entity.Modifiedby = apiRequestDetails.UserName;
            response.Status = entity.Status == "Active" ? Status.Success : Status.Failed;
            response.Message = entity.Status == "Active" ? "Record Activated" : "Record Inactived";
            await _feesTyperepo.SaveChangesAsync();

            //response.Status = Status.Success;
            //response.Message = "Record Deleted Successfully";
            return response;
        }

        public async Task<CommonResponse<List<StudentFeeGenerateStatusResponse>>> GetFeesTypeListAsync(GetFeesGentrationRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentFeeGenerateStatusResponse>>();

            List<StudentFeeGenerateStatusResponse> result = await _feesTyperepo.GetFeesListViewAsync(request, apiRequestDetails);

            if (result.Any())
            {
                response.Status = Status.Success;
                response.Data = result;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }

            return response;
        }

        public async Task<CommonResponse<string>> InsertStudentFeesAsync(GentrationFeesRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            const string transType = "DR";
            var now = DateTime.Now;

            decimal debit = request.amount;
            decimal credit = 0;
            var feesDesc = await _feesTyperepo.GetFeesTypeDescriptionAsync(request.feestypefkid, apiRequestDetails);
            var description = string.IsNullOrWhiteSpace(feesDesc) ? "Fees Generated" : feesDesc;
            int inserted = 0;
            int duplicate = 0;
            int classNotFound = 0;
            int failed = 0;
            foreach (var studentId in request.studentdetailsfkid.Distinct())
            {
                // 1) Find StudentClassDetailsFkid
                var scdSysId = await _feesTyperepo.GetStudentClassDetailsSysIdAsync(
                    studentId,
                    request.academicYearFkid,
                    request.sectionfkid,
                    apiRequestDetails);

                if (scdSysId == null || scdSysId.Value <= 0)
                {
                    classNotFound++;
                    continue;
                }

                // 2) Duplicate check (your rule)
                var exists = await _feesTyperepo.IsFeesTransactionExistsAsync(
                    studentId,
                    request.feestypefkid,
                    scdSysId.Value,
                    transType,
                    debit,
                    apiRequestDetails);

                if (exists)
                {
                    duplicate++;
                    continue;
                }

                // 3) RefNo based on FY from GenerateDate
                var refNo = await _feesTyperepo.GetNextRefNoByGenerateDateAsync(now, transType, apiRequestDetails);

                // 4) Create entity (only columns available in StudentFeesTransaction)
                var entity = new StudentFeesTransaction
                {
                    StudentFkid = studentId,
                    FeesTypeFkid = request.feestypefkid,
                    StudentClassDetailsFkid = scdSysId.Value,

                    RefNo = refNo,

                    //PaymentMode = null,
                    //BankName = null,
                    //ChequeNo = null,
                    //ChequeDate = now,

                    TransationType = transType,
                    GenerateDate = now,
                    Description = description,

                    Debit = debit,
                    Credit = credit,

                    Remark = "",

                    Status = "Created",

                    InstitutionCode = apiRequestDetails.InstitutionCode,
                    EntryBy = apiRequestDetails.UserName,
                    EntryDate = now,
                    ModifiedBy = apiRequestDetails.UserName,
                    ModifiedDate = now
                };
                var saved = await _feesTyperepo.AddStudentFeesTransactionAsync(entity);

                if (saved) inserted++;
                else failed++;
            }
            if (inserted > 0)
            {
                response.Status = Status.Success;
                response.Message =
                    $"Fees inserted successfully. Inserted: {inserted}, Duplicates skipped: {duplicate}, Class not found: {classNotFound}, Failed: {failed}";
            }
            else
            {
                response.Status = Status.Failed;
                response.Message =
                    $"No fees inserted. Duplicates skipped: {duplicate}, Class not found: {classNotFound}, Failed: {failed}";
            }
            return response;

        }

        #region Apporve Fees
        public async Task<CommonResponse<List<StudentApproveFeesResponse>>> GetApproveFeesAsync(APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentApproveFeesResponse>>();

            var data = await _feesTyperepo.GetApproveFeesAsync(apiRequestDetails);

            response.Status = Status.Success;
            response.Data = data;

            return response;
        }

        public async Task<CommonResponse<List<ApproveFeesViewResponse>>> GetApproveFeesViewAsync(GetApproveFeesViewRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<ApproveFeesViewResponse>>();

            var data = await _feesTyperepo.GetApproveFeesViewAsync(request, apiRequestDetails);

            response.Status = Status.Success;
            response.Data = data;

            return response;
        }

        public async Task<CommonResponse<string>> UpdateFeesApproveAsync(UpdateFeesApproveRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();
            int updated = await _feesTyperepo.UpdateFeesApproveAsync(request, apiRequestDetails);

            response.Status = Status.Success;
            response.Message = "Done";
            return response;
        }

        #endregion
    }
}
