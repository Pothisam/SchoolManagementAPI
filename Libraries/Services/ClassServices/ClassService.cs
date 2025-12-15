using Models.ClassModels;
using Models.CommonModels;
using Repository.ClassRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ClassServices
{
    public class ClassService : IClassService
    {
        private readonly IClassRepo _classRepo;

        public ClassService(IClassRepo classRepo)
        {
            _classRepo = classRepo;
        }
        public async Task<CommonResponse<string>> AddClassAsync(AddClassRequest request, APIRequestDetails apiRequestDetails)
        {
            var isExists = await _classRepo.IsClassExistsAsync(request, apiRequestDetails);
            if (isExists)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Class already exists"
                };
            }
            bool isAdded = await _classRepo.AddClassAsync(request, apiRequestDetails);
            return new CommonResponse<string>
            {
                Status = isAdded ? Status.Success : Status.Failed,
                Message = isAdded
                ? "Class added successfully"
                : "Unable to add class"
            };
        }

        public async Task<CommonResponse<List<ClassResponse>>> GetClassListAsync(APIRequestDetails apiRequestDetails)
        {
            var result = await _classRepo.GetClassListAsync(apiRequestDetails);

            return new CommonResponse<List<ClassResponse>>
            {
                Status = result.Any() ? Status.Success : Status.Failed,
                Data = result,
                Message = ""
            };
        }

        public async Task<CommonResponse<string>> UpdateClassStatusAsync(UpdateClassStatusRequest request, APIRequestDetails apiRequestDetails)
        {
            // 1. Validate status
            if (request.Status != "Active" && request.Status != "InActive")
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Invalid status value"
                };
            }

            // 2. Fetch entity
            var entity = await _classRepo.GetClassByIdAsync(
                request,
                apiRequestDetails
            );

            if (entity == null)
            {
                return new CommonResponse<string>
                {
                    Status = Status.Failed,
                    Message = "Class not found"
                };
            }

            // 3. Assign status from request
            entity.Status = request.Status;
            entity.ModifiedBy = apiRequestDetails.UserName;

            // 4. Persist
            var result = await _classRepo.UpdateClassAsync(entity);

            // 5. Response
            return new CommonResponse<string>
            {
                Status = result ? Status.Success : Status.Failed,
                Message = result
                    ? $"Class status updated to {request.Status}"
                    : "Unable to update class status"
            };
        }
    }
}
