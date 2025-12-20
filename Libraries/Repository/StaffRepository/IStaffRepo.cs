using Models.CommonModels;
using Models.StaffModels;
using Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.StaffRepository
{
    public interface IStaffRepo
    {
        Task<Boolean> CheckDuplicate(StaffDetailsAddRequest request, APIRequestDetails apiRequestDetails);
        Task<int> AddStaffAsync(StaffDetail staff, StaffPassTable passTable);
        #region Add Language
        Task AddStaffLanguageDetail(StaffLanguageDetail request);
        #endregion
        #region Add Education
        Task AddStaffEducationDetail(StaffEducationDetail request);
        #endregion
        #region Expirence
        Task AddStaffExperienceDetail(StaffExperience request);
        #endregion
    }
}
