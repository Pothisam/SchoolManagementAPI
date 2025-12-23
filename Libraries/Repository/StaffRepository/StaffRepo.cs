using Microsoft.EntityFrameworkCore;
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
    public class StaffRepo : IStaffRepo
    {
        private readonly SchoolManagementContext _context;
        public StaffRepo(SchoolManagementContext context)
        {
            _context = context;
        }
        public async Task<bool> CheckDuplicate(StaffDetailsAddRequest request, APIRequestDetails apiRequestDetails)
        {
            var isDuplicate = await _context.StaffMasterViews.AnyAsync(x => x.MobileNo == request.MobileNo && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            return !isDuplicate;
        }
        public async Task<int> AddStaffAsync(StaffDetail staff, StaffPassTable passTable)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.StaffDetails.Add(staff);
                await _context.SaveChangesAsync();

                passTable.StaffDetailsFkid = staff.SysId;
                _context.StaffPassTables.Add(passTable);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return staff.SysId;
            }
            catch
            {
                await transaction.RollbackAsync();
                return 0;
            }
        }
        public async Task<string> GenerateStaffID(APIRequestDetails apiRequestDetails)
        {
            var prefix = await _context.InstitutionDetails
                .Where(x => x.Sysid == apiRequestDetails.InstitutionCode)
                .Select(x => x.StaffIdprefix)
                .FirstOrDefaultAsync();

            var staffCount = await _context.StaffDetails
                .CountAsync(x => x.InstitutionCode == apiRequestDetails.InstitutionCode) + 1;

            string staffID = staffCount.ToString().PadLeft(5, '0');
            if (prefix != null)
            {
                staffID = prefix + staffID;
            }
            return staffID;
        }
        #region Add Language
        public async Task AddStaffLanguageDetail(StaffLanguageDetail request)
        {
            await _context.StaffLanguageDetails.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Add Education
        public async Task AddStaffEducationDetail(StaffEducationDetail request)
        {
            await _context.StaffEducationDetails.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Expirence
        public async Task AddStaffExperienceDetail(StaffExperience request)
        {
            await _context.StaffExperiences.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AutoCompleteResponse>> GetStaffAutoComplete(StaffAutocompleteRequest request, APIRequestDetails apiRequestDetails)
        {
            var autoCompleteResponse = new List<AutoCompleteResponse>();

            if (request.TableName == "StaffDetails")
            {
                var query = request.ColumnName switch
                {
                    "Name" => _context.StaffMasterViews
                        .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode && x.Staffname.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.Name.ToUpper())
                        .Distinct()
                        .Take(5),

                    "Cast" => _context.StaffMasterViews
                        .Where(x => x.Cast.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.Cast.ToUpper())
                        .Distinct()
                        .Take(5),

                    "MotherTongue" => _context.StaffMasterViews
                        .Where(x => x.MotherTongue.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.MotherTongue.ToUpper())
                        .Distinct()
                        .Take(5),

                    _ => throw new NotImplementedException($"Column '{request.ColumnName}' is not supported.")
                };

                var result = await query.ToListAsync();
                autoCompleteResponse.AddRange(result.Select(value => new AutoCompleteResponse { Column = value }));
            }
            else if (request.TableName == "StaffLanguageDetails")
            {
                var query = request.ColumnName switch
                {
                    "LanguageKnow" => _context.StaffLanguageDetails
                        .Where(x => x.LanguageKnow.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.LanguageKnow.ToUpper())
                        .Distinct()
                        .Take(5),

                    _ => throw new NotImplementedException($"Column '{request.ColumnName}' is not supported.")
                };

                var result = await query.ToListAsync();
                autoCompleteResponse.AddRange(result.Select(value => new AutoCompleteResponse { Column = value }));
            }
            else if (request.TableName == "StaffEducationDetails")
            {
                var query = request.ColumnName switch
                {
                    "UniversityName" => _context.StaffEducationDetails
                        .Where(x => x.UniversityName.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.UniversityName)
                        .Distinct()
                        .Take(5),

                    "InstituionName" => _context.StaffEducationDetails
                        .Where(x => x.InstituionName.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.InstituionName)
                        .Distinct()
                        .Take(5),

                    "Specialization" => _context.StaffEducationDetails
                        .Where(x => x.Specialization.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.Specialization)
                        .Distinct()
                        .Take(5),

                    _ => throw new NotImplementedException($"Column '{request.ColumnName}' is not supported.")
                };

                var result = await query.ToListAsync();
                autoCompleteResponse.AddRange(result.Select(value => new AutoCompleteResponse { Column = value }));
            }
            else if (request.TableName == "StaffExpirenceDetails")
            {
                var query = request.ColumnName switch
                {
                    "InstituionName" => _context.StaffExperiences
                        .Where(x => x.InstituionName.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.InstituionName)
                        .Distinct()
                        .Take(5),

                    "Position" => _context.StaffExperiences
                        .Where(x => x.Position.ToUpper().StartsWith(request.SearchParam.ToUpper()))
                        .Select(x => x.Position)
                        .Distinct()
                        .Take(5),

                    _ => throw new NotImplementedException($"Column '{request.ColumnName}' is not supported.")
                };

                var result = await query.ToListAsync();
                autoCompleteResponse.AddRange(result.Select(value => new AutoCompleteResponse { Column = value }));
            }

            return autoCompleteResponse;
        }

        


        #endregion
    }
}
