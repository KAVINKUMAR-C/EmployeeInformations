using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels;
using EmployeeInformations.Data.IRepository;
using Microsoft.EntityFrameworkCore;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.OnboardingViewModel;
using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Data.Repository
{
     public class OBEmployeesRepository : IOBEmployeesRepository
    {
        private readonly EmployeesDbContext _dbContext;
        public OBEmployeesRepository(EmployeesDbContext employeesDbContext)
        {
            _dbContext = employeesDbContext;
        }

        public async Task <List<EmployeesEntity>> GetAllEmployees(int companyId)
        {
            return await _dbContext.Employees.Where(x => x.CompanyId == companyId).ToListAsync();
        }
        public async Task<List<DesignationEntity>> GetAllDesignation(int companyId)
        {
            return await _dbContext.Designations.Where(d => d.CompanyId == companyId && d.IsActive && !d.IsDeleted).ToListAsync();
        }
        public async Task<List<DepartmentEntity>> GetAllDepartment(int companyId)
        {
            return await _dbContext.Departments.Where(f => f.CompanyId == companyId && f.IsActive && !f.IsDeleted).ToListAsync();
        }
        public async Task<List<ProfileInfoEntity>> GetAllEmployeeProfile(int companyId)
        {
            return await _dbContext.ProfileInfo.AsNoTracking().Where(x => x.Employees.CompanyId == companyId).ToListAsync();
        }
        public async Task<ProfileInfoEntity> GetProfileByEmployeeId(int empId, int companyId)
        {
            var profileInfo = await _dbContext.ProfileInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return profileInfo;
        }
        public async Task<AddressInfoEntity> GetAddressByEmployeeId(int empId, int companyId)
        {
            var addressInfoEntity = await _dbContext.AddressInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId);
            return addressInfoEntity;
        }
        public async Task<List<StateEntity>> GetAllStates()
        {
            return await _dbContext.state.ToListAsync();
        }
        public async Task<List<CityEntity>> GetAllCities()
        {
            return await _dbContext.cities.Where(x => !x.IsDeleted).ToListAsync();
        }
        public async Task<List<CountryEntity>> GetAllCountry()
        {
            return await _dbContext.CountryEntities.ToListAsync();
        }
        public async Task<List<StateEntity>> GetByCountryId(int countryId)
        {
            return await _dbContext.state.Where(c => c.CountryId == countryId).ToListAsync();
        }
        public async Task<List<CityEntity>> GetByStateId(int StateId)
        {
            return await _dbContext.cities.Where(c => c.StateId == StateId && !c.IsDeleted).ToListAsync();
        }
        public async Task<List<DocumentTypesEntity>> GetAllDocumentTypes(int companyId)
        {
            return await _dbContext.documentTypesEntities.Where(c => c.IsActive && !c.IsDeleted && c.CompanyId == companyId).ToListAsync();
        }
        public async Task<int> GetEmployeeEmail(string officeEmail, int companyId)
        {
            var emailCount = await _dbContext.Employees.Where(x => x.OfficeEmail.ToLower() == officeEmail.ToLower() && x.CompanyId == companyId).CountAsync();
            return emailCount;
        }
        public async Task<int> GetPersonalEmail(string personalEmail)
        {
            var personal = await _dbContext.Employees.Where(y => !string.IsNullOrEmpty(y.PersonalEmail) && y.PersonalEmail.ToLower() == personalEmail.ToLower() && !y.IsDeleted).CountAsync();
            return personal;
        }
        public async Task<ProfileInfoEntity> GetProfileByEmployeeIdview(int empId, int companyId)
        {
            var profileInfo = await _dbContext.ProfileInfo.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId);
            return profileInfo;
        }
        public async Task<string> GetCountryNameByCountryId(int countryId)
        {
            var countryEntities = await _dbContext.CountryEntities.FirstOrDefaultAsync(x => x.CountryId == countryId);
            if (countryEntities != null)
            {
                return countryEntities.Name;
            }
            return string.Empty;
        }
        public async Task<string> GetStateNameByStateId(int stateId)
        {
            var state = await _dbContext.state.FirstOrDefaultAsync(x => x.StateId == stateId);
            if (state != null)
            {
                return state.StateName;
            }
            return string.Empty;
        }
        public async Task<string> GetCityNameByCityId(int cityId)
        {
            var cities = await _dbContext.cities.FirstOrDefaultAsync(x => x.CityId == cityId && !x.IsDeleted);
            if (cities != null)
            {
                return cities.CityName;
            }
            return string.Empty;
        }
        public async Task<string> GetCountryNameBySecondaryCountryId(int? SecondaryCountryId)
        {
            var countryEntities = await _dbContext.CountryEntities.FirstOrDefaultAsync(x => x.CountryId == SecondaryCountryId);
            if (countryEntities != null)
            {
                return countryEntities.Name;
            }
            return string.Empty;
        }
        public async Task<List<ExperienceEntity>> GetAllExperienceView(int empId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId).ToListAsync();
            return experienceEntity;
        }
        public async Task<string> GetStateNameBySecondaryStateId(int? SecondaryStateId)
        {
            var state = await _dbContext.state.FirstOrDefaultAsync(x => x.StateId == SecondaryStateId);
            if (state != null)
            {
                return state.StateName;
            }
            return string.Empty;
        }
        public async Task<string> GetCityNameBySecondaryCityId(int? SecondaryCityId)
        {
            var cities = await _dbContext.cities.FirstOrDefaultAsync(x => x.CityId == SecondaryCityId && !x.IsDeleted);
            if (cities != null)
            {
                return cities.CityName;
            }
            return string.Empty;
        }
        public async Task<List<QualificationEntity>> GetAllQulificationView(int empId, int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(b => b.EmpId == empId && b.Employees.CompanyId == companyId).ToListAsync();
            return qualificationEntity;
        }

        public async Task<bool> GetRejectEmployees(OBEmployees employees, int companyId)
        {
            var result = false;
            var employeeEntities = await _dbContext.Employees.Where(e => e.EmpId == employees.EmpId && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeEntities != null)
            {
                var profileEntity = await _dbContext.ProfileInfo.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                var addressEntity = await _dbContext.AddressInfo.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                var otherDetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                var qualificationEntity = await _dbContext.Qualification.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                var bankDetailsEntity = await _dbContext.BankDetails.Where(e => e.EmpId == employees.EmpId && e.Employees.CompanyId == companyId).FirstOrDefaultAsync();
                if (employeeEntities != null)
                {
                    if (profileEntity != null)
                    {
                        profileEntity.IsDeleted = true;
                        _dbContext.ProfileInfo.Update(profileEntity);
                    }
                    if (addressEntity != null)
                    {
                        addressEntity.IsDeleted = true;
                        _dbContext.AddressInfo.Update(addressEntity);
                    }
                    if (otherDetailsEntity != null)
                    {
                        otherDetailsEntity.IsDeleted = true;
                        _dbContext.OtherDetails.Update(otherDetailsEntity);
                    }
                    if (experienceEntity != null)
                    {
                        experienceEntity.IsDeleted = true;
                        _dbContext.Experience.Update(experienceEntity);
                    }
                    if (qualificationEntity != null)
                    {
                        qualificationEntity.IsDeleted = true;
                        _dbContext.Qualification.Update(qualificationEntity);
                    }
                    if (bankDetailsEntity != null)
                    {
                        bankDetailsEntity.IsDeleted = true;
                        _dbContext.BankDetails.Update(bankDetailsEntity);
                    }

                    employeeEntities.RejectReason = employees.RejectReason;
                    employeeEntities.RelieveId = employees.RelieveId;
                    employeeEntities.IsDeleted = true;
                    _dbContext.Employees.Update(employeeEntities);
                    result = await _dbContext.SaveChangesAsync() > 0;
                }
            }
            return result;
        }
        public async Task<List<OtherDetailsEntity>> GetAllOtherDetailsView(int empId, int companyId)
        {
            var otherdetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId).ToListAsync();
            return otherdetailsEntity;
        }

        public async Task<OtherDetailsEntity> GetOtherDetailsByEmployeeId(int empId, int companyId)
        {
            var otherDetailsEntity = await _dbContext.OtherDetails.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return otherDetailsEntity;
        }
        public async Task<QualificationEntity> GetQualificationByEmployeeId(int empId, int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.AsNoTracking().FirstOrDefaultAsync(w => w.EmpId == empId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return qualificationEntity;
        }
        public async Task<ExperienceEntity> GetExperienceByEmployeeId(int empId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.AsNoTracking().FirstOrDefaultAsync(q => q.EmpId == empId && q.Employees.CompanyId == companyId && !q.IsDeleted);
            return experienceEntity;
        }
        public async Task<BankDetailsEntity> GetBankDetailsByEmployeeId(int empId,int companyId)
        {
            var bankDetailsEntity = await _dbContext.BankDetails.AsNoTracking().FirstOrDefaultAsync(g => g.EmpId == empId && g.Employees.CompanyId == companyId);
            return bankDetailsEntity;
        }
        public async Task<List<BloodGroupEntity>> GetAllBloodGroup()
        {
            return await _dbContext.BloodGroup.ToListAsync();
        }
        public async Task<List<RelationshipTypeEntity>> GetAllRelationshipType()
        {
            return await _dbContext.Relationship.ToListAsync();
        }

        public async Task<Int32> CreateEmployee(EmployeesEntity employeesEntity, int companyId)
        {
            var result = 0;
            if (employeesEntity?.EmpId == 0)
            {
                employeesEntity.CompanyId = companyId;
                await _dbContext.Employees.AddAsync(employeesEntity);
                await _dbContext.SaveChangesAsync();

                result = employeesEntity.EmpId;
            }
            else
            {
                if (employeesEntity != null)
                {
                    _dbContext.Update(employeesEntity);
                    await _dbContext.SaveChangesAsync();
                    result = employeesEntity.EmpId;
                }
            }
            return result;
        }
        public async Task<int> AddProfileInfo(ProfileInfoEntity profileInfoEntity, bool isAdd = false)
        {
            var result = 0;
            Common.Common.WriteServerErrorLog("DateOfJoining : " + profileInfoEntity.DateOfJoining);
            if (profileInfoEntity?.ProfileId == 0)
            {
                await _dbContext.ProfileInfo.AddAsync(profileInfoEntity);
                await _dbContext.SaveChangesAsync();
                result = profileInfoEntity.EmpId;
            }
            else
            {
                if (profileInfoEntity != null)
                {
                    _dbContext.ProfileInfo.Update(profileInfoEntity);
                    await _dbContext.SaveChangesAsync();
                    result = profileInfoEntity.EmpId;
                }
            }

            return result;
        }
        public async Task CreateReportingPersons(List<ReportingPersonsEntity> reportingPersonsEntitys, int EmpId)
        {
            var reportingPersonsEntities = await _dbContext.ReportingPersonsEntities.Where(x => x.EmployeeId == EmpId).ToListAsync();
            if (reportingPersonsEntities.Count() == 0)
            {
                await _dbContext.ReportingPersonsEntities.AddRangeAsync(reportingPersonsEntitys);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                _dbContext.ReportingPersonsEntities.RemoveRange(reportingPersonsEntities);
                await _dbContext.SaveChangesAsync();

                _dbContext.ReportingPersonsEntities.UpdateRange(reportingPersonsEntitys);
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<List<int>> GetReportingPersonEmployeeById(int EmpId, int companyId)
        {
            IList<Employees> reportersList = new List<Employees>();
            var reportingPersonsEntity = await _dbContext.ReportingPersonsEntities.Where(e => e.EmployeeId == EmpId && e.CompanyId == companyId).Select(x => x.ReportingPersonEmpId).ToListAsync();
            return reportingPersonsEntity;
        }
        public async Task<List<EmployeesEntity>> GetAllReportingPersons(int companyId)
        {
            return await _dbContext.Employees.Where(x => x.CompanyId == companyId && !x.IsDeleted && x.RoleId != Convert.ToInt32(Role.Employee)).ToListAsync();
        }
        public async Task<List<RoleEntity>> GetAllRoleTable(int companyId)
        {
            return await _dbContext.RoleEntities.Where(f => f.CompanyId == companyId && f.IsActive && !f.IsDeleted).ToListAsync();
        }

        public async Task<EmployeesEntity> GetEmployeeById(int EmpId, int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmpId && e.CompanyId == companyId && !e.IsDeleted);
            return employeesEntity ?? new EmployeesEntity();
        }
        public async Task<List<SkillSetEntity>> GetAllSkills(int companyId)
        {
            return await _dbContext.SkillSets.Where(c => c.IsActive && !c.IsDeleted && c.CompanyId == companyId).ToListAsync();
        }
        public async Task<int> AddAddressInfo(AddressInfoEntity addressInfoEntity)
        {
            var result = 0;
            if (addressInfoEntity?.AddressId == 0)
            {
                await _dbContext.AddressInfo.AddAsync(addressInfoEntity);
                await _dbContext.SaveChangesAsync();
                result = addressInfoEntity.EmpId;
            }
            else
            {
                if (addressInfoEntity != null)
                {
                    _dbContext.AddressInfo.Update(addressInfoEntity);
                    await _dbContext.SaveChangesAsync();
                    result = addressInfoEntity.EmpId;
                }
            }

            return result;
        }
        public async Task<List<OtherDetailsEntity>> GetAllOtherDetailsViewModel(int empId, int companyId)
        {
            var otherdetailsEntity = await _dbContext.OtherDetails.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return otherdetailsEntity;
        }
        public async Task<int> InsertAndUpdateOtherDetails(OtherDetailsEntity otherDetailsEntity)
        {
            var result = 0;
            if (otherDetailsEntity?.DetailId == 0)
            {
                await _dbContext.OtherDetails.AddAsync(otherDetailsEntity);
                await _dbContext.SaveChangesAsync();
                result = otherDetailsEntity.DetailId;

            }
            else
            {
                if (otherDetailsEntity != null)
                {
                    _dbContext.OtherDetails.Update(otherDetailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = otherDetailsEntity.DetailId;
                }
            }
            return result;
        }
        public async Task<bool> InsertOtherDetailsAttachment(List<OtherDetailsAttachmentsEntity> otherDetailsAttachments, int detailId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.OtherDetailsAttachmentsEntitys.Where(x => x.DetailId == detailId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.OtherDetailsAttachmentsEntitys.AddRangeAsync(otherDetailsAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.OtherDetailsAttachmentsEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.OtherDetailsAttachmentsEntitys.UpdateRange(otherDetailsAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }
        public async Task<OtherDetailsEntity> GetotherDetailsBydetailId(int detailId, int companyId)
        {
            var otherDetailsEntity = await _dbContext.OtherDetails.AsNoTracking().FirstOrDefaultAsync(w => w.DetailId == detailId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return otherDetailsEntity ?? new OtherDetailsEntity();
        }
        public async Task<OtherDetailsAttachmentsEntity> GetOtherDetailsAttachmentsByEmployeeId(int detailId)
        {
            var otherDetailsAttachmentsEntity = await _dbContext.OtherDetailsAttachmentsEntitys.FirstOrDefaultAsync(k => k.DetailId == detailId);
            return otherDetailsAttachmentsEntity;
        }
        public async Task<List<OtherDetailsAttachmentsEntity>> GetAllOtherDetailsAttachment(int detailId)
        {
            return await _dbContext.OtherDetailsAttachmentsEntitys.Where(x => x.DetailId == detailId && !x.IsDeleted).ToListAsync();
        }
        public async Task DeleteOtherDetails(OtherDetailsEntity otherDetailsEntity)
        {
            _dbContext.OtherDetails.Update(otherDetailsEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<OtherDetailsAttachmentsEntity>> GetOtherDetailsDocumentAndFilePath(int detailId)
        {
            var docNmae = await _dbContext.OtherDetailsAttachmentsEntitys.Where(e => e.DetailId == detailId && !e.IsDeleted).ToListAsync();
            return docNmae;
        }
        public async Task DeleteOtherDetailsAttachement(List<OtherDetailsAttachmentsEntity> otherDetailsAttachmentsEntity)
        {
            _dbContext.OtherDetailsAttachmentsEntitys.UpdateRange(otherDetailsAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var result = await _dbContext.OtherDetailsAttachmentsEntitys.Where(e => e.DetailId == detailId && !e.IsDeleted).Select(x => x.DocumentName).ToListAsync();
            return result;
        }
        public async Task<List<QualificationEntity>> GetAllQulificationViewModel(int empId,int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(b => b.EmpId == empId && b.Employees.CompanyId == companyId && !b.IsDeleted).ToListAsync();
            return qualificationEntity;
        }
        public async Task<bool> InsertQualificationAttachment(List<QualificationAttachmentsEntity> qualificationAttachments, int qualificationId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.QualificationAttachmentEntitys.Where(x => x.QualificationId == qualificationId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.QualificationAttachmentEntitys.AddRangeAsync(qualificationAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.QualificationAttachmentEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.QualificationAttachmentEntitys.UpdateRange(qualificationAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }
        public async Task<Int32> InsertAndUpdateQualification(QualificationEntity qualificationEntity)
        {
            var result = 0;
            if (qualificationEntity?.QualificationId == 0)
            {
                await _dbContext.Qualification.AddAsync(qualificationEntity);
                await _dbContext.SaveChangesAsync();
                result = qualificationEntity.QualificationId;
            }
            else
            {
                if (qualificationEntity != null)
                {
                    _dbContext.Qualification.Update(qualificationEntity);
                    await _dbContext.SaveChangesAsync();
                    result = qualificationEntity.QualificationId;
                }
            }
            return result;
        }
        public async Task<QualificationEntity> GetQualificationByQualificationId(int QualificationId,int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.FirstOrDefaultAsync(w => w.QualificationId == QualificationId && w.Employees.CompanyId == companyId && !w.IsDeleted);
            return qualificationEntity ?? new QualificationEntity();
        }
        public async Task<QualificationAttachmentsEntity> GetQualificationAttachmentsByEmployeeId(int qualificationId)
        {
            var qualificationAttachmentsEntity = await _dbContext.QualificationAttachmentEntitys.FirstOrDefaultAsync(g => g.QualificationId == qualificationId);
            return qualificationAttachmentsEntity;
        }
        public async Task<List<QualificationAttachmentsEntity>> GetAllQualificationAttachments(int qualificationId)
        {
            return await _dbContext.QualificationAttachmentEntitys.Where(x => x.QualificationId == qualificationId && !x.IsDeleted).ToListAsync();
        }
        public async Task<List<QualificationEntity>> GetAllQualification(int EmpId, int companyId)
        {
            var qualificationEntity = await _dbContext.Qualification.Where(x => x.EmpId == EmpId && x.Employees.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return qualificationEntity;
        }
        public async Task<List<string>> GetDocumentNameByQualificationId(int qualificationId)
        {
            var quaificationName = new List<string>();
            var result = await _dbContext.QualificationAttachmentEntitys.Where(e => e.QualificationId == qualificationId && !e.IsDeleted).Select(x => x.QualificationName).ToListAsync();
            if (result != null)
            {
                foreach (var item in result)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        quaificationName.Add(item);
                    }
                }
                return quaificationName;
            }
            return new List<string>();
        }
        public async Task<List<QualificationAttachmentsEntity>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var docNmaes = await _dbContext.QualificationAttachmentEntitys.Where(e => e.QualificationId == qualificationId && !e.IsDeleted).ToListAsync();
            return docNmaes;
        }
        public async Task DeleteQualification(QualificationEntity qualificationEntity)
        {
            _dbContext.Qualification.Update(qualificationEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteQualificationAttachement(List<QualificationAttachmentsEntity> qualificationAttachmentsEntity)
        {
            _dbContext.QualificationAttachmentEntitys.UpdateRange(qualificationAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<ExperienceEntity>> GetAllExperienceViewModel(int empId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(e => e.EmpId == empId && e.Employees.CompanyId == companyId && !e.IsDeleted).ToListAsync();
            return experienceEntity;
        }
        public async Task<int> InsertAndUpdateExperience(ExperienceEntity experienceEntity)
        {
            var result = 0;
            if (experienceEntity?.ExperienceId == 0)
            {
                await _dbContext.Experience.AddAsync(experienceEntity);
                await _dbContext.SaveChangesAsync();
                result = experienceEntity.ExperienceId;
            }
            else
            {
                if (experienceEntity != null)
                {
                    _dbContext.Experience.Update(experienceEntity);
                    await _dbContext.SaveChangesAsync();
                    result = experienceEntity.ExperienceId;
                }
            }
            return result;
        }
        public async Task<bool> InsertExperienceAttachment(List<ExperienceAttachmentsEntity> experienceAttachments, int experienceId)
        {
            var result = false;
            var attachmentsEntitys = await _dbContext.ExperienceAttachmentsEntitys.Where(x => x.ExperienceId == experienceId).ToListAsync();
            if (attachmentsEntitys.Count() == 0)
            {
                await _dbContext.ExperienceAttachmentsEntitys.AddRangeAsync(experienceAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            else
            {
                _dbContext.ExperienceAttachmentsEntitys.RemoveRange(attachmentsEntitys);
                await _dbContext.SaveChangesAsync();

                _dbContext.ExperienceAttachmentsEntitys.UpdateRange(experienceAttachments);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }
        public async Task<ExperienceEntity> GetExperienceByExperienceId(int ExperienceId,int companyId)
        {
            var experienceEntity = await _dbContext.Experience.AsNoTracking().FirstOrDefaultAsync(g => g.ExperienceId == ExperienceId && g.Employees.CompanyId == companyId && !g.IsDeleted);
            return experienceEntity ?? new ExperienceEntity();
        }
        public async Task<ExperienceAttachmentsEntity> GetExperienceAttachmentsByEmployeeId(int experienceId)
        {
            var experienceAttachmentsEntity = await _dbContext.ExperienceAttachmentsEntitys.FirstOrDefaultAsync(g => g.ExperienceId == experienceId);
            return experienceAttachmentsEntity;
        }
        public async Task<List<ExperienceAttachmentsEntity>> GetAllExperienceAttachment(int experienceId)
        {
            return await _dbContext.ExperienceAttachmentsEntitys.Where(x => x.ExperienceId == experienceId && !x.IsDeleted).ToListAsync();
        }
        public async Task<List<ExperienceEntity>> GetAllExperience(int EmpId, int companyId)
        {
            var experienceEntity = await _dbContext.Experience.Where(x => x.EmpId == EmpId && x.Employees.CompanyId == companyId && !x.IsDeleted).ToListAsync();
            return experienceEntity;
        }
        public async Task<List<string>> GetDocumentNameByExperienceId(int experienceId)
        {
            var result = await _dbContext.ExperienceAttachmentsEntitys.Where(e => e.ExperienceId == experienceId && !e.IsDeleted).Select(x => x.ExperienceName).ToListAsync();
            return result;
        }
        public async Task<List<ExperienceAttachmentsEntity>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var docNmae = await _dbContext.ExperienceAttachmentsEntitys.Where(e => e.ExperienceId == experienceId && !e.IsDeleted).ToListAsync();
            return docNmae;
        }
        public async Task DeleteExperience(ExperienceEntity experienceEntity)
        {
            _dbContext.Experience.Update(experienceEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task DeleteExperienceAttachement(List<ExperienceAttachmentsEntity> experienceAttachmentsEntity)
        {
            _dbContext.ExperienceAttachmentsEntitys.UpdateRange(experienceAttachmentsEntity);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<int> AddBankDetails(BankDetailsEntity bankDetailsEntity)
        {
            var result = 0;
            if (bankDetailsEntity?.BankId == 0)
            {
                await _dbContext.BankDetails.AddAsync(bankDetailsEntity);
                await _dbContext.SaveChangesAsync();
                result = bankDetailsEntity.EmpId;
            }
            else
            {
                if (bankDetailsEntity != null)
                {
                    _dbContext.BankDetails.Update(bankDetailsEntity);
                    await _dbContext.SaveChangesAsync();
                    result = bankDetailsEntity.EmpId;
                }
            }
            return result;
        }
        public async Task<int> UpdateStatus(EmployeesEntity employeesEntity)
        {
            var result = 0;
            if (employeesEntity != null)
            {
                 _dbContext.Employees.Update(employeesEntity);
                await _dbContext.SaveChangesAsync();
                result = 1;
            }
            return result;
        }
        // view
        public async Task<EmployeesEntity> GetEmployeeByIdView(int EmpId, int companyId)
        {
            var employeesEntity = await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmpId == EmpId && e.CompanyId == companyId);
            return employeesEntity;
        }
    }
}
