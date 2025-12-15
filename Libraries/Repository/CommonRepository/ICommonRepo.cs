using Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.CommonRepository
{
    public interface ICommonRepo
    {
        Task<DateTime> GetDatetime();
        Task<byte[]?> GetLogoWithTextAsync(APIRequestDetails apiRequestDetails);
        Task<byte[]?> GetLogoonlyAsync(APIRequestDetails apiRequestDetails);
        Task<byte[]?> GetFavIconAsync(APIRequestDetails apiRequestDetails);
        Task<List<RecordHistoryResponse>> GetRecordHistory(GetRecordHistoryRequest request, APIRequestDetails apiRequestDetails);
        Task<List<PostOfficeResponse>> GetPostOffice(PostOfficeRequest request);
    }
}
