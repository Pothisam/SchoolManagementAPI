using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.StudentFeesTransactionModels;
using Repository.Entity;

namespace Repository.StudentFeesTransactionRepository
{
    public interface IStudentFeesTransactionRepo
    {
        Task<List<StudentFeesTransactionResponse>> GetFeesList(StudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<List<StudentFeesTransactionResponse>> GetFeesDetailsBtNameList(StudentFeesTransactionByNameRequest request, APIRequestDetails apiRequestDetails);
        Task<GetDebitResponse> GetDebitAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
        Task<GetCreditResponse> GetCreditAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
        #region Fees Add
        Task<StudentFeeBalanceDto?> GetApprovedDebitCreditAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<int> GetNextRefNoAsync(string finYear, APIRequestDetails apiRequestDetails);
        Task<StudentDetailInfoDto?> GetStudentDetailInfoAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<FeesType?> GetFeesTypeByIdAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<bool> ChequeNumberExistsAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<int> AddStudentFeesTransactionAsync(StudentFeesTransaction entity);
        Task<List<GetFeesReportDateWiseResponse>> GetFeesReportDateWiseAsync(GetFeesReportDateWiseRequest request, APIRequestDetails apiRequestDetails);
        Task<GetFeesReportDateWisePrintResponse?> GetFeesReportByIdAsync(GetPrintCashReceiptValueRequest request, APIRequestDetails apiRequestDetails);
        #endregion
        #region Fees Report
        Task<bool> DeleteCredit(DeleteFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        #endregion
    }
}
