using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.CommonModels;
using Models.ReportModels;

namespace Repository.ReportRepository
{
    public interface IReportRepo
    {
        Task<List<StudentTrasferResponse>> GetStudentPromotionListAsync(StudentTrasferRequest request,APIRequestDetails apiRequestDetails);
    }
}
