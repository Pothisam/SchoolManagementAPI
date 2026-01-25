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
        public async Task<List<StaffLanguageResponse>> GetStaffLanguageKnowByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            List<StaffLanguageResponse>? response = await (from x in _context.StaffLanguageDetails
                                                           where x.StaffFkid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                           select new StaffLanguageResponse
                                                           {
                                                               SysId = x.SysId,
                                                               language = x.LanguageKnow,
                                                               ReadLanguage = x.ReadLanguage,
                                                               SpeakLanguage = x.SpeakLanguage,
                                                               WriteLanguage = x.WriteLanguage
                                                           }).ToListAsync();
            return response;
        }
        public async Task DeleteStaffLanguageAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var staffLanguage = await _context.StaffLanguageDetails
                                              .SingleOrDefaultAsync(x => x.SysId == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
            _context.StaffLanguageDetails.Remove(staffLanguage);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region Add Education
        public async Task AddStaffEducationDetail(StaffEducationDetail request)
        {
            await _context.StaffEducationDetails.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        public async Task<List<StaffEducationResponse>> GetStaffEducationDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            List<StaffEducationResponse>? response = await (from x in _context.StaffEducationDetails
                                                            where x.StaffDetailsFkid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                            select new StaffEducationResponse
                                                            {
                                                                SysId = x.SysId,
                                                                DegreeType = x.DegreeType,
                                                                Degree = x.Degree,
                                                                YearOfPassing = (int)x.YearOfpassing,
                                                                UniversityName = x.UniversityName,
                                                                InstitutionName = x.InstituionName,
                                                                Mode = x.Mode,
                                                                PassPercentage = x.PassPercentage,
                                                                Specialization = x.Specialization
                                                            }).OrderBy(x => x.YearOfPassing).ToListAsync();

            return response;
        }
        public async Task<bool> CheckDuplicateStaffEducation(StaffEducationDetailAdd request)
        {
            var isDuplicate = await _context.StaffEducationDetails
        .AnyAsync(x => x.StaffDetailsFkid == request.Id && x.DegreeType == request.DegreeType && x.Degree == request.Degree && x.InstitutionCode == request.InstitutionCode);

            return !isDuplicate;
        }
        public async Task DeleteStaffEducationAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var staffeducation = await _context.StaffEducationDetails
                                              .SingleOrDefaultAsync(x => x.SysId == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
            _context.StaffEducationDetails.Remove(staffeducation);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckDuplicateStaffEducationUpdate(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails)
        {
            var GetFKID = await _context.StaffEducationDetails.Where(x => x.SysId == request.SysId).Select(x => x.StaffDetailsFkid).SingleOrDefaultAsync();
            var isDuplicate = await _context.StaffEducationDetails
        .AnyAsync(x => x.SysId != request.SysId && x.StaffDetailsFkid == GetFKID && x.DegreeType == request.DegreeType && x.Degree == request.Degree && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            return !isDuplicate;
        }
        public async Task UpdateStaffEducationDetail(UpdateAddStaffEducationDetailAdd request, APIRequestDetails apiRequestDetails)
        {
            var staffEducationDetail = await _context.StaffEducationDetails
            .SingleOrDefaultAsync(x => x.SysId == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            if (staffEducationDetail != null)
            {
                staffEducationDetail.DegreeType = request.DegreeType;
                staffEducationDetail.Degree = request.Degree;
                staffEducationDetail.YearOfpassing = request.YearOfpassing;
                staffEducationDetail.UniversityName = request.UniversityName;
                staffEducationDetail.InstituionName = request.InstitutionName;
                staffEducationDetail.Mode = request.Mode;
                staffEducationDetail.PassPercentage = request.PassPercentage;
                staffEducationDetail.Specialization = request.Specialization;

                await _context.SaveChangesAsync();
            }
        }
        #endregion
        #region Expirence
        public async Task AddStaffExperienceDetail(StaffExperience request)
        {
            await _context.StaffExperiences.AddAsync(request);
            await _context.SaveChangesAsync();
        }
        public async Task<List<StaffExperienceResponse>> GetStaffExperienceDetailsByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            List<StaffExperienceResponse>? response = await (from x in _context.StaffExperiences
                                                             where x.StaffDetailsFkid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                             select new StaffExperienceResponse
                                                             {
                                                                 SysId = x.Sysid,
                                                                 InstitutionName = x.InstituionName,
                                                                 Position = x.Position,
                                                                 FromDate = x.FromDate,
                                                                 ToDate = x.Todate,
                                                                 Salary = (int)x.Salary
                                                             }).ToListAsync();
            return response;
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
        public async Task DeleteStaffExperienceAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            var staffexpirence = await _context.StaffExperiences
                                              .SingleOrDefaultAsync(x => x.Sysid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
            _context.StaffExperiences.Remove(staffexpirence);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateStaffExperienceDetailsAsync(UpdateStaffExperienceDetailAddRequest request, APIRequestDetails apiRequestDetails)
        {
            var staffExperienceDetail = await _context.StaffExperiences
            .SingleOrDefaultAsync(x => x.Sysid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);

            if (staffExperienceDetail != null)
            {
                staffExperienceDetail.InstituionName = request.InstitutionName;
                staffExperienceDetail.Position = request.Position;
                staffExperienceDetail.FromDate = request.FromDate;
                staffExperienceDetail.Todate = request.Todate;
                staffExperienceDetail.Salary = request.Salary;

                await _context.SaveChangesAsync();
            }
        }
        #endregion
        #region Staff View List
        public async Task<StaffCountResponse> GetStaffCountAsync(APIRequestDetails apiRequestDetails)
        {
            StaffCountResponse? response = await (from x in _context.StaffMasterViews
                                                  where x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                  group x by new { x.InstitutionCode } into g
                                                  select new StaffCountResponse
                                                  {
                                                      TotalStaff = g.Count(c => c.StaffType == "Teaching") + g.Count(c => c.StaffType == "Non-Teaching"),
                                                      Male = g.Count(x => x.Sex == "Male"),
                                                      Female = g.Count(x => x.Sex == "Female"),
                                                      Teaching = g.Count(c => c.StaffType == "Teaching"),
                                                      NonTeaching = g.Count(c => c.StaffType == "Non-Teaching")
                                                  }).FirstOrDefaultAsync();
            return response;
        }

        public async Task<List<DesignationListResponse>> GetStaffDesignationListAsync(APIRequestDetails apiRequestDetails)
        {
            var result = await (from x in _context.StaffMasterViews
                                where x.InstitutionCode == apiRequestDetails.InstitutionCode
                                orderby x.DesignationCode ascending
                                select new DesignationListResponse
                                {
                                    Designation = x.Designation,
                                    DesignationCode = (int)x.DesignationCode
                                }).Distinct().ToListAsync();

            return result;
        }

        public async Task<List<StaffDetailSearchResponse>> GetStaffDetailSearchAsync(StaffSearchRequest request, APIRequestDetails apiRequestDetails)
        {
            List<StaffDetailSearchResponse> StaffMasterViewList = new();

            IQueryable<StaffMasterView> query = _context.StaffMasterViews
                .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode);

            if (request.ColumnName == "Designation")
            {
                query = query.Where(x => x.DesignationCode == int.Parse(request.SearchParam));
            }
            else if (request.ColumnName == "Name")
            {
                query = query.Where(x => x.Name.Contains(request.SearchParam));
            }
            else if (request.ColumnName == "Gender")
            {
                query = query.Where(x => x.Sex == request.SearchParam);
            }
            else if (request.ColumnName == "StaffType")
            {
                query = query.Where(x => x.StaffType == request.SearchParam);
            }

            var documentQuery = _context.DocumentLibraries
                .Where(d => d.Action == "Image-Upload"
                            && d.TableName == "StaffDetails"
                            && d.FileSize != 0);

            StaffMasterViewList = await (from x in query
                                         join y in documentQuery on x.Sysid equals y.Fkid into documentGroup
                                         orderby x.Staffname ascending
                                         select new StaffDetailSearchResponse
                                         {
                                             SysId = x.Sysid,
                                             Name = x.Name,
                                             Designation = x.Designation,
                                             MobileNo = x.MobileNo,
                                             StaffType = x.StaffType,
                                             Gender = x.Sex,
                                             EnteredBy = x.EnteredBy,
                                             EntryDate = x.Entrydate,
                                             ModifiedBy = x.ModifiedBy,
                                             ModifiedDate = x.ModifiedDate,
                                             Guid = documentGroup
                                                 .OrderBy(d => d.ModifiedBy)
                                                 .Select(d => (Guid?)d.Guid)
                                                 .FirstOrDefault()
                                         }).ToListAsync();

            if (string.IsNullOrEmpty(request.ColumnName))
            {
                StaffMasterViewList = await query
                    .OrderByDescending(x => x.ModifiedDate)
                    .Select(x => new StaffDetailSearchResponse
                    {
                        SysId = x.Sysid,
                        Name = x.Name,
                        Designation = x.Designation,
                        MobileNo = x.MobileNo,
                        StaffType = x.StaffType,
                        Gender = x.Sex,
                        EnteredBy = x.EnteredBy,
                        EntryDate = x.Entrydate,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedDate = x.ModifiedDate,
                        Guid = (from d in documentQuery
                                where d.Fkid == x.Sysid
                                orderby d.ModifiedBy
                                select (Guid?)d.Guid).FirstOrDefault()
                    }).ToListAsync();
            }

            return StaffMasterViewList;
        }

        public async Task<StaffDetailResponse> GetStaffDetailByIDAsync(StaffDetailsPK request, APIRequestDetails apiRequestDetails)
        {
            StaffDetailResponse? result = await (from x in _context.StaffDetails
                                                 let document = (from y in _context.DocumentLibraries
                                                                 where y.Fkid == x.SysId
                                                                 && y.Action == "Image-Upload"
                                                                 && y.TableName == "StaffDetails"
                                                                 && y.FileSize != 0
                                                                 select y).FirstOrDefault()
                                                 where x.SysId == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode
                                                 select new StaffDetailResponse
                                                 {
                                                     StaffID = x.StaffId,
                                                     StaffType = x.StaffType,
                                                     EnteredBy = x.EnteredBy,
                                                     EntryDate = x.EntryDate,
                                                     ModifiedBy = x.ModifiedBy,
                                                     ModifiedDate = x.ModifiedDate,
                                                     Title = x.Title,
                                                     Name = x.Name,
                                                     Initial = x.Initial,
                                                     Sex = x.Sex,
                                                     BloodGroup = x.BloodGroup,
                                                     DOB = x.Dob,
                                                     PlaceOfBirth = x.PlaceOfBirth,
                                                     MaritalStatus = x.MaritalStatus,
                                                     Religion = x.Religion,
                                                     PhysicalDisability = x.PhysicalDisablity,
                                                     Community = x.Community,
                                                     Caste = x.Cast,
                                                     MobileNo = x.MobileNo,
                                                     EmailId = x.Emailid,
                                                     AadharCardNo = x.AadharCardNo,
                                                     DesignationCode = x.DesignationCode,
                                                     DOJ = x.Doj,

                                                     PermanentAddress1 = x.ParmanentAddress1,
                                                     PermanentAddress2 = x.ParmanentAddress2,
                                                     PermanentAddressPincode = x.ParmanentAddressPincode,
                                                     PermanentAddressPostOffice = x.ParmanentAddressPostOffice,
                                                     PermanentAddressDistrict = x.ParmanentAddressDistrict,
                                                     PermanentAddressState = x.ParmanentAddressState,

                                                     CommunicationAddress1 = x.CommunicationAddress1,
                                                     CommunicationAddress2 = x.CommunicationAddress2,
                                                     CommunicationAddressPincode = x.CommunicationAddressPincode,
                                                     CommunicationAddressPostOffice = x.CommunicationAddressPostOffice,
                                                     CommunicationAddressDistrict = x.CommunicationAddressDistrict,
                                                     CommunicationAddressState = x.CommunicationAddressState,

                                                     Status = x.Status,
                                                     MotherTongue = x.MotherTongue,
                                                     IFSCCode = x.Ifsccode.ToUpper(),
                                                     BankName = x.BankName.ToUpper(),
                                                     BankAddress = x.BankAddress.ToUpper(),
                                                     AccountNumber = x.AccountNumber.ToUpper(),
                                                     MICRCode = x.Micrcode.ToUpper(),
                                                     PANCardNo = x.PancardNo.ToUpper(),
                                                     Guid = document != null ? document.Guid : null
                                                 }).FirstOrDefaultAsync();
            return result;
        }




        #endregion
        public async Task<bool> UpdateStaffDetailsAsync(UpdateStaffDetailsRequest request, APIRequestDetails apiRequestDetails)
        {
            try
            {
                var staffDetail = await _context.StaffDetails
                    .SingleOrDefaultAsync(x => x.SysId == request.Sysid && x.InstitutionCode == apiRequestDetails.InstitutionCode);

                if (staffDetail == null) return false;

                staffDetail.StaffType = request.StaffType.Trim();
                staffDetail.Title = request.Title.Trim();
                staffDetail.Name = request.Name.ToUpper().Trim();
                staffDetail.Initial = request.Initial.ToUpper().Trim();
                staffDetail.Sex = request.sex.Trim();
                staffDetail.Dob = request.DOB;
                staffDetail.PlaceOfBirth = request.PlaceOfBirth.Trim();
                staffDetail.MaritalStatus = request.MaritalStatus.Trim();
                staffDetail.Religion = request.Religion.Trim();
                staffDetail.PhysicalDisablity = request.PhysicalDisability.Trim();
                staffDetail.BloodGroup = request.BloodGroup;
                staffDetail.Community = request.Community;
                staffDetail.Cast = request.Caste.ToUpper();
                staffDetail.MobileNo = request.MobileNo;
                staffDetail.Emailid = request.EmailID;
                staffDetail.AadharCardNo = request.AadharCardNo;
                staffDetail.Designation = request.Designation;
                staffDetail.DesignationCode = request.DesignationCode;
                staffDetail.Doj = request.DOJ;
                staffDetail.ParmanentAddress1 = request.PermanentAddress1;
                staffDetail.ParmanentAddress2 = request.PermanentAddress2;
                staffDetail.ParmanentAddressPincode = request.PermanentAddressPincode;
                staffDetail.ParmanentAddressPostOffice = request.PermanentAddressPostOffice;
                staffDetail.ParmanentAddressDistrict = request.PermanentAddressDistrict;
                staffDetail.ParmanentAddressState = request.PermanentAddressState;
                staffDetail.CommunicationAddress1 = request.CommunicationAddress1;
                staffDetail.CommunicationAddress2 = request.CommunicationAddress2;
                staffDetail.CommunicationAddressPincode = request.CommunicationAddressPincode;
                staffDetail.CommunicationAddressPostOffice = request.CommunicationAddressPostOffice;
                staffDetail.CommunicationAddressDistrict = request.CommunicationAddressDistrict;
                staffDetail.CommunicationAddressState = request.CommunicationAddressState;
                staffDetail.Ifsccode = request.IFSCCode.ToUpper();
                staffDetail.BankName = request.BankName;
                staffDetail.BankAddress = request.BankAddress;
                staffDetail.AccountNumber = request.AccountNumber;
                staffDetail.Micrcode = request.MICRCode;
                staffDetail.PancardNo = request.PANCardNo.ToUpper();
                staffDetail.MotherTongue = request.MotherTongue.ToUpper();
                staffDetail.Status = request.Status;
                staffDetail.ModifiedBy = apiRequestDetails.UserName;
                staffDetail.Dor = request.DOR;
                _context.StaffDetails.Update(staffDetail);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetStaffPasswordAsync(StaffDetailsPasswordReset request, APIRequestDetails apiRequestDetails)
        {
            var staffPassTable = await _context.StaffPassTables.SingleOrDefaultAsync(x => x.StaffDetailsFkid == request.SysId && x.InstitutionCode == apiRequestDetails.InstitutionCode);
            if (staffPassTable != null)
            {
                staffPassTable.Password = request.Password;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateStaffPasstableStatusAsync(int sysId, string status)
        {
            var staffPasstable = await _context.StaffPassTables.SingleOrDefaultAsync(x => x.StaffDetailsFkid == sysId);
            if (staffPasstable != null)
            {
                staffPasstable.Status = status;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<StaffNameAndSysidResponse>> GetStaffNameListAsync(APIRequestDetails apiRequestDetails)
        {
            return await _context.StaffMasterViews
                .Where(x => x.InstitutionCode == apiRequestDetails.InstitutionCode)
                .OrderBy(x => x.Staffname)
                .Select(x => new StaffNameAndSysidResponse
                {
                    Sysid = x.Sysid,
                    Name = x.Name,
                    Staffname = x.Staffname
                }).ToListAsync();
        }
    }
}
