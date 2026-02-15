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
            entity.ModifiedDate = DateTime.Now;
            response.Status = entity.Status == "Active" ? Status.Success : Status.Failed;
            response.Message = entity.Status == "Active" ? "Record Activated" : "Record Inactived";
            await _feesTyperepo.SaveChangesAsync();

            //response.Status = Status.Success;
            //response.Message = "Record Deleted Successfully";
            return response;
        }
    }
}
