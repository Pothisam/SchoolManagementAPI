using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.StudentFeesTransactionModels;

namespace Repository.StudentFeesTransactionRepository
{
    public interface IStudentFeesTransactionRepo
    {
        Task<List<StudentFeesTransactionResponse>> GetFeesList(StudentFeesTransactionRequest request, APIRequestDetails apiRequestDetails);
        Task<List<StudentFeesTransactionResponse>> GetFeesDetailsBtNameList(StudentFeesTransactionByNameRequest request, APIRequestDetails apiRequestDetails);
        Task<GetDebitResponse> GetDebitAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
        Task<GetCreditResponse> GetCreditAsync(GetDebitRequest request, APIRequestDetails apiRequestDetails);
    }
}
