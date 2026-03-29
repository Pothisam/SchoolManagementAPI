using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.StudentFeesTransactionModels;
using Repository.Entity;
using Repository.StudentFeesTransactionRepository;

namespace Services.StudentFeesTransactionServices
{
    public class StudentFeesTransactionService : IStudentFeesTransactionService
    {
        private readonly IStudentFeesTransactionRepo _studentFeesTransactionRepo;
        public StudentFeesTransactionService(IStudentFeesTransactionRepo studentFeesTransactionRepo)
        {
            _studentFeesTransactionRepo = studentFeesTransactionRepo;
        }
        public async Task<CommonResponse<List<StudentFeesTransactionResponse>>> GetFeesList(StudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentFeesTransactionResponse>>();
            List<StudentFeesTransactionResponse> result = await _studentFeesTransactionRepo.GetFeesList(request, apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }
        public async Task<CommonResponse<List<StudentFeesTransactionResponse>>> GetFeesDetailsBtNameList(StudentFeesTransactionByNameRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<StudentFeesTransactionResponse>>();
            List<StudentFeesTransactionResponse> result = await _studentFeesTransactionRepo.GetFeesDetailsBtNameList(request, apiRequestDetails);
            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "No Data Found";
            }
            else
            {
                response.Status = Status.Success;
                response.Message = "";
                response.Data = result;
            }
            return response;
        }

        public async Task<CommonResponse<GetDebitResponse>> GetDebitAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<GetDebitResponse>();
            var result = await _studentFeesTransactionRepo.GetDebitAsync(request, apiRequestDetails);

            response.Status = Status.Success;
            response.Data = result;

            return response;
        }

        public async Task<CommonResponse<GetCreditResponse>> GetCreditAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<GetCreditResponse>();
            var result = await _studentFeesTransactionRepo.GetCreditAsync(request, apiRequestDetails);

            response.Status = Status.Success;
            response.Data = result;

            return response;
        }

        public async Task<CommonResponse<string>> AddStudentFeesTransactionAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<string>();

            var balance = await _studentFeesTransactionRepo.GetApprovedDebitCreditAsync(request, apiRequestDetails);
            if (balance == null || balance.Debit <= 0)
            {
                response.Status = Status.Failed;
                response.Message = "Fees Type is Not Created";
                return response;
            }
            if (balance.Debit < (balance.Credit + request.Amount))
            {
                response.Status = Status.Failed;
                response.Message = $"You Can collect Fees only {balance.Debit - balance.Credit} In This Fees Type";
                return response;
            }
            var feesType = await _studentFeesTransactionRepo.GetFeesTypeByIdAsync(request, apiRequestDetails);
            if (feesType == null)
            {
                response.Status = Status.Failed;
                response.Message = "Invalid Fees Type";
                return response;
            }

            var studentInfo = await _studentFeesTransactionRepo.GetStudentDetailInfoAsync(request, apiRequestDetails);
            if (studentInfo == null)
            {
                response.Status = Status.Failed;
                response.Message = "Student not found";
                return response;
            }
            if (request.PaymentMode == "Cheque" || request.PaymentMode == "Demand Draft" || request.PaymentMode == "Online Transfer")
            {
                if (string.IsNullOrWhiteSpace(request.ChequeNo))
                {
                    response.Status = Status.Failed;
                    response.Message = "Cheque Number is required";
                    return response;
                }

                bool chequeExists = await _studentFeesTransactionRepo.ChequeNumberExistsAsync(request, apiRequestDetails);
                if (chequeExists)
                {
                    response.Status = Status.Failed;
                    response.Message = $"{request.PaymentMode} Number Already Exists Duplicates Entry Not Allowed";
                    return response;
                }
            }

            string finYear = GetFinancialYear(request.GenerateDate);
            int nextRefNo = await _studentFeesTransactionRepo.GetNextRefNoAsync(finYear, apiRequestDetails);

            DateTime chequeDateValue = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(request.ChequeDate))
            {
                DateTime.TryParse(request.ChequeDate, out chequeDateValue);
            }
            var entity = new StudentFeesTransaction
            {
                StudentFkid = request.StudentFkid,
                FeesTypeFkid = request.FeesTypeFkid,
                StudentClassDetailsFkid = studentInfo.ClassSectionSysId,
                RefNo = nextRefNo,
                PaymentMode = request.PaymentMode,
                BankName = string.IsNullOrWhiteSpace(request.BankName) ? null : request.BankName.Trim(),
                ChequeNo = string.IsNullOrWhiteSpace(request.ChequeNo) ? null : request.ChequeNo.Trim().ToUpper(),
                ChequeDate = chequeDateValue == DateTime.MinValue ? null : chequeDateValue,
                TransationType = "CR",
                GenerateDate = request.GenerateDate,
                Description = request.PaymentMode +": "+ request.Description,
                Debit = 0,
                Credit = request.Amount,
                Remark = request.Remark,
                Status = "Approved",
                InstitutionCode = apiRequestDetails.InstitutionCode,
                EntryBy = apiRequestDetails.UserName,
                EntryDate = DateTime.Now,
                ModifiedBy = apiRequestDetails.UserName,
                ModifiedDate = DateTime.Now
            };

            int sysId = await _studentFeesTransactionRepo.AddStudentFeesTransactionAsync(entity);

            response.Status = Status.Success;
            response.Data = sysId.ToString();
            response.Message = "Success";
            return response;
        }
        public static string GetFinancialYear(DateTime generateDate)
        {
            return generateDate.Month >= 4
                ? $"{generateDate.Year}-{generateDate.Year + 1}"
                : $"{generateDate.Year - 1}-{generateDate.Year}";
        }
        #region Fees Report
        public async Task<CommonResponse<List<GetFeesReportDateWiseResponse>>> GetFeesReportDateWiseAsync(GetFeesReportDateWiseRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<List<GetFeesReportDateWiseResponse>>();

            if (request.FromDate.Date > request.ToDate.Date)
            {
                response.Status = Status.Failed;
                response.Message = "FromDate should not be greater than ToDate";
                return response;
            }

            var result = await _studentFeesTransactionRepo.GetFeesReportDateWiseAsync(request, apiRequestDetails);

            if (result.Any())
            {
                response.Status = Status.Success;
                response.Data = result;
            }
            else
            {
                response.Status = Status.Failed;
                response.Message = "No records found";
                response.Data = new List<GetFeesReportDateWiseResponse>();
            }

            return response;
        }

        public async Task<CommonResponse<GetFeesReportDateWisePrintResponse>> GetFeesReportByIdAsync(GetPrintCashReceiptValueRequest request, APIRequestDetails apiRequestDetails)
        {
            var response = new CommonResponse<GetFeesReportDateWisePrintResponse>();

            var result = await _studentFeesTransactionRepo.GetFeesReportByIdAsync(request, apiRequestDetails);

            if (result == null)
            {
                response.Status = Status.Failed;
                response.Message = "Record not found";
                return response;
            }

            response.Status = Status.Success;
            response.Data = result;
            return response;
        }
        #endregion
    }
}
