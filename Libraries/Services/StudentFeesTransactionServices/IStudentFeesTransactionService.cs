using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.StaffModels;
using Models.StudentFeesTransactionModels;

namespace Services.StudentFeesTransactionServices
{
    public interface IStudentFeesTransactionService
    {
        Task<CommonResponse<List<StudentFeesTransactionResponse>>> GetFeesList(StudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<StudentFeesTransactionResponse>>> GetFeesDetailsBtNameList(StudentFeesTransactionByNameRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<GetDebitResponse>> GetDebitAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<GetCreditResponse>> GetCreditAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<string>> AddStudentFeesTransactionAsync(AddStudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<List<GetFeesReportDateWiseResponse>>> GetFeesReportDateWiseAsync(GetFeesReportDateWiseRequest request, APIRequestDetails apiRequestDetails);
        Task<CommonResponse<GetFeesReportDateWisePrintResponse>> GetFeesReportByIdAsync(GetPrintCashReceiptValueRequest request,APIRequestDetails apiRequestDetails);
    }
}
