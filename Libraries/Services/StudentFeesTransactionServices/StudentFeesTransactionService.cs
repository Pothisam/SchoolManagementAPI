using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.StudentFeesTransactionModels;
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
    }
}
