using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Common;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Common.Enums;
using Microsoft.Extensions.Configuration;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.Model.PrivilegeViewModel;
using EmployeeInformations.Model.OnboardingViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Data.Repository;
using DocumentFormat.OpenXml.Office.CoverPageProps;

namespace EmployeeInformations.Business.Service
{
    public class OBEmployeeService : IOBEmployeeService
    {
        public readonly IOBEmployeesRepository _OBEmployeesRepository;
        public readonly IMapper _mapper;
        private readonly IMasterRepository _masterRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IConfiguration _config;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IAuditLogRepository _auditLogRepository;
       
        public OBEmployeeService(IOBEmployeesRepository oBEmployeesRepository , IMapper mapper, IMasterRepository masterRepository, ICompanyRepository companyRepository, IAssetRepository assetRepository, IEmployeesRepository employeesRepository, IConfiguration configuration, IEmailDraftRepository emailDraftRepository, IAuditLogRepository auditLogRepository)
        {
            _OBEmployeesRepository = oBEmployeesRepository;
            _mapper = mapper;
            _masterRepository = masterRepository;
            _companyRepository = companyRepository;
            _assetRepository = assetRepository;
            _employeesRepository = employeesRepository;
            _config = configuration;
            _emailDraftRepository = emailDraftRepository;
            _auditLogRepository = auditLogRepository;
          
        }
       

        public async Task<List<OBEmployees>> GetEmployee(int companyId)
        {
                var listOfEmployees = new List<OBEmployees>();
                var listtype = new List<RelievingReason>();
                var listEmployee = await _OBEmployeesRepository.GetAllEmployees(companyId);
                var allReleaveTypes = await _masterRepository.GetAllRelievingReason(companyId);
                if (listEmployee != null)
                {
                    listOfEmployees = _mapper.Map<List<OBEmployees>>(listEmployee);
                }
                if (allReleaveTypes != null)
                {
                    listtype = _mapper.Map<List<RelievingReason>>(allReleaveTypes);
                }

                var allDesignation = await _OBEmployeesRepository.GetAllDesignation(companyId);

                var allDepartment = await _OBEmployeesRepository.GetAllDepartment(companyId);
                var allEmployeeProfiles = await _OBEmployeesRepository.GetAllEmployeeProfile(companyId);
                var probationPeriod = await _companyRepository.GetProbationMonth();
                int count = 1;
                foreach (var item in listOfEmployees)
                {
                        var assetList = await _assetRepository.GetAssetByEmployeeId(item.EmpId,companyId);
                        var benefitList = await _assetRepository.GetBenefitByEmployeeId(item.EmpId,companyId);
                        count = count > 5 ? 1 : count;
                        var profileByEmpId = allEmployeeProfiles.FirstOrDefault(x => x.EmpId == item.EmpId);
                        item.EmployeeProfileImage = profileByEmpId != null ? profileByEmpId.ProfileName : string.Empty;
                        item.DateofJoin = profileByEmpId != null ? profileByEmpId.DateOfJoining.ToString(Constant.DateFormat) : string.Empty;
                        item.EmployeeSortName = Common.Common.GetEmployeeSortName(item.FirstName, item.LastName);
                        var designation = allDesignation.FirstOrDefault(x => x.DesignationId == item.DesignationId);
                        item.DesignationName = designation != null ? designation.DesignationName : string.Empty;
                        var departmentEntity = allDepartment.FirstOrDefault(y => y.DepartmentId == item.DepartmentId);
                        item.DepartmentName = departmentEntity != null ? departmentEntity.DepartmentName : string.Empty;
                        item.ProbationDays = profileByEmpId == null ? "" : YetToGetProbationDay(profileByEmpId, probationPeriod);
                        var releavingEntityType = allReleaveTypes != null ? allReleaveTypes.FirstOrDefault(y => y.RelievingReasonId == item.RelieveId) : null;
                        item.ReleaveName = releavingEntityType != null ? releavingEntityType.RelievingReasonName : string.Empty;
                        item.AllAssetId = assetList.Count();
                        item.BenefitId = benefitList.Count();
                        item.ClassName = Common.Common.GetClassNameForGrid(count);
                        item.ProfileCompletionPercentage = await GetEmployeeProfileRecordsByEmployeeId(item.EmpId,companyId);                    

                    count++;
                }
                if (listOfEmployees != null && listOfEmployees.Count() > 0)
                {
                    var addReleiveType = listOfEmployees.FirstOrDefault();
                    if (addReleiveType != null)
                    {
                        addReleiveType.RelievingReason = listtype;
                    }
                    listOfEmployees.OrderByDescending(x => x.CreatedDate).ToList();
                }

                return listOfEmployees ?? new List<OBEmployees>();
        }
        public async Task<int> GetEmployeeEmail(string officeEmail, int companyId)
        {
            var totalEmployeeCount = await _OBEmployeesRepository.GetEmployeeEmail(officeEmail, companyId);
            return totalEmployeeCount;
        }
        public async Task<int> GetPersonalEmail(string personalEmail, int empId,int companyId)
        {
            var result = 0;
            var employee = await _OBEmployeesRepository.GetEmployeeById(empId, companyId);
            var employeePersonalMailCount = await _OBEmployeesRepository.GetPersonalEmail(personalEmail);
            if (employeePersonalMailCount == 1)
            {
                if (personalEmail == employee.PersonalEmail)
                {
                    result = 1;
                }
                else
                {
                    result = 2;
                }
            }
            return result;
        }
        public async Task<OBEmployees> GetEmployeeById(int EmpId, int companyId)
        {
            var employees = new OBEmployees();
            var employeesEntity = await _OBEmployeesRepository.GetEmployeeById(EmpId, companyId);
            if (employeesEntity != null)
            {
                employees = _mapper.Map<OBEmployees>(employeesEntity);
            }

            var reportingPersonEmployeeIds = new List<int>();
            reportingPersonEmployeeIds = await _OBEmployeesRepository.GetReportingPersonEmployeeById(EmpId,companyId);
            employees.reportingPeople = await GetAllReportingPerson(companyId);
            employees.Departments = await GetAllDepartment(companyId);
            employees.Designations = await GetAllDesignation(companyId);

            var strFmtReportingPersonEmpId = "";
            var finalOut = "";
            for (int i = 0; i < reportingPersonEmployeeIds.Count(); i++)
            {
                var b = reportingPersonEmployeeIds[i];
                strFmtReportingPersonEmpId += string.Format(b + ",");
            }
            if (!string.IsNullOrEmpty(strFmtReportingPersonEmpId))
            {
                finalOut = strFmtReportingPersonEmpId.Remove(strFmtReportingPersonEmpId.Length - 1, 1);
            }
            employees.StrFmtReportingPersonEmpId = finalOut;

            var skillnames = await _OBEmployeesRepository.GetEmployeeById(EmpId, companyId);

            if (skillnames != null && !string.IsNullOrEmpty(skillnames.SkillName))
            {
                var frm = skillnames.SkillName.Split(",");
                var strFmtSkillId = "";
                var finalOutSkill = "";
                for (int i = 0; i < frm.Count(); i++)
                {
                    var b = frm[i];
                    strFmtSkillId += string.Format(b + ",");
                }
                if (!string.IsNullOrEmpty(strFmtSkillId))
                {
                    finalOutSkill = strFmtSkillId.Remove(strFmtSkillId.Length - 1, 1);
                }
                employees.StrFmtSkillId = finalOutSkill;
            }
            else
            {
                employees.StrFmtSkillId = string.Empty;
            }
            return employees;
        }
        public async Task<string> GetEmployeeId(int companyId)
        {
            var listCompany = await _companyRepository.GetAllCompanySetting(companyId);
            var employeeCount = await _companyRepository.GetMaxCountOfEmployeesByCompanyId(companyId);

            string? userName;
            userName = listCompany.CompanyCode + (employeeCount).ToString("D3");
            return userName;
        }

        public async Task<List<ReportingPerson>> GetAllReportingPerson(int companyId)
        {
            var result = new List<ReportingPerson>();
            var listEmployee = await _OBEmployeesRepository.GetAllReportingPersons(companyId);
            var reporters = _mapper.Map<List<Employees>>(listEmployee);
            foreach (var item in reporters)
            {
                result.Add(new ReportingPerson()
                {
                    EmpId = item.EmpId,
                    EmployeeName = item?.FirstName + " " + item?.LastName,
                });
            }
            return result;
        }

        public async Task<List<Designation>> GetAllDesignation(int companyId)
        {
            var listOfDesignation = new List<Designation>();
            var listDesignation = await _OBEmployeesRepository.GetAllDesignation(companyId);
            if (listDesignation.Count() > 0)
            {
                listOfDesignation = _mapper.Map<List<Designation>>(listDesignation);
            }
            return listOfDesignation;
        }
        public async Task<List<Department>> GetAllDepartment(int companyId)
        {
            var listOfDepartment = new List<Department>();
            var listDepartment = await _OBEmployeesRepository.GetAllDepartment(companyId);
            if (listDepartment.Count() > 0)
            {
                listOfDepartment = _mapper.Map<List<Department>>(listDepartment);
            }
            return listOfDepartment;
        }

        public async Task<List<RoleViewModel>> GetAllRoleTable(int companyId)
        {
            var listOfRoleTable = new List<RoleViewModel>();
            var listRoleTable = await _OBEmployeesRepository.GetAllRoleTable(companyId);
            if (listRoleTable != null)
            {
                listOfRoleTable = _mapper.Map<List<RoleViewModel>>(listRoleTable);
            }
            return listOfRoleTable;
        }

        public async Task<List<SkillSet>> GetAllSkills(int companyId)
        {
            var listOfSkill = new List<SkillSet>();
            var listSkills = await _OBEmployeesRepository.GetAllSkills(companyId);
            if (listSkills != null)
            {
                listOfSkill = _mapper.Map<List<SkillSet>>(listSkills);
            }

            return listOfSkill;
        }
        public async Task<bool> GetRejectEmployees(OBEmployees employees, int companyId)
        {
            var result = await _OBEmployeesRepository.GetRejectEmployees(employees,companyId);
            return result;
        }

        public async Task<int> CreateEmployee(OBEmployees employees , int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (employees != null)
            {
                if (employees.EmpId == 0)
                {
                    var randomPassword = Common.Common.GeneratePassword();
                    employees.Password = Common.Common.sha256_hash(randomPassword);
                    employees.CreatedBy = sessionEmployeeId;
                    employees.CreatedDate = DateTime.Now;
                    employees.IsProbationary = false;
                    var employeesEntity = _mapper.Map<EmployeesEntity>(employees);
                    var data = await _OBEmployeesRepository.CreateEmployee(employeesEntity,companyId);
                    if (employees.ReportingPersonEmpId.Count() > 0)
                    {
                        var reportingPersonsEntitys = new List<ReportingPersonsEntity>();
                        foreach (var item in employees.ReportingPersonEmpId)
                        {
                            var reportingPersonsEntity = new ReportingPersonsEntity();
                            reportingPersonsEntity.EmployeeId = data;
                            reportingPersonsEntity.CompanyId = employeesEntity.CompanyId;
                            reportingPersonsEntity.ReportingPersonEmpId = item;
                            reportingPersonsEntitys.Add(reportingPersonsEntity);
                        }
                        await _OBEmployeesRepository.CreateReportingPersons(reportingPersonsEntitys, data);
                    }

                    var draftTypeId = (int)EmailDraftType.WelcomeEmployee;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var domainName = Convert.ToString(_config.GetSection("ConnectionUrl").GetSection("DomainName").Value);
                    var infoEmailName = Convert.ToString(_config.GetSection(Common.Constant.vphospitalInfoMailId).Value);
                    var allemployee = await _OBEmployeesRepository.GetAllEmployees(companyId);
                    List<int> reportingPersonEmployeeIds = await _OBEmployeesRepository.GetReportingPersonEmployeeById(data,companyId);
                    var reportingPersion = await GetemployeeNameByReportingPersionId(reportingPersonEmployeeIds, allemployee);
                    var bodyContent = EmailBodyContent.WelcomeEmployeeEmailBodyContent(employeesEntity, randomPassword, domainName, infoEmailName, reportingPersion, emailDraftContentEntity.DraftBody);
                    await InsertEmailQueue(employees.OfficeEmail, emailDraftContentEntity, bodyContent);
                    result = data;
                    await InsertEmployeesLog(Common.Constant.CreateEmployeesLog, result, Common.Constant.Add, sessionEmployeeId, companyId,null);
                }
                else
                {
                    var employeesEntity = await _employeesRepository.GetEmployeeById(employees.EmpId, companyId);
                    var reportingPersonsEmployeeEntity = await _employeesRepository.GetReportingPersonEmployeeById(employees.EmpId,companyId);
                    var reportingPersonsEntities = await _employeesRepository.GetReportingPersonEmployeeId(employees.EmpId, companyId);
                    var employeesChangeLogs = await GetUpdateEmployeeDiffrencetFieldName(employeesEntity, employees, reportingPersonsEntities,companyId);
                    employeesEntity.FatherName = employees.FatherName;
                    employeesEntity.PersonalEmail = employees.PersonalEmail;
                    employeesEntity.FirstName = employees.FirstName;
                    employeesEntity.LastName = employees.LastName;
                    employeesEntity.OfficeEmail = employees.OfficeEmail;
                    employeesEntity.DepartmentId = employees.DepartmentId;
                    employeesEntity.DesignationId = employees.DesignationId;
                    employeesEntity.EsslId = employees.EsslId;
                    employeesEntity.UpdatedDate = DateTime.Now;
                    employeesEntity.UpdatedBy = sessionEmployeeId;

                    var strSikillName = employees.SkillNames;
                    if (strSikillName.Count() > 0)
                    {
                        var str = string.Join(",", strSikillName);
                        employeesEntity.SkillName = str.TrimEnd(',');
                    }

                    var data = await _employeesRepository.CreateEmployee(employeesEntity,companyId);
                    var reportingPersonsEntitys = new List<ReportingPersonsEntity>();
                    if (employees.ReportingPersonEmpId.Count() > 0)
                    {
                        foreach (var item in employees.ReportingPersonEmpId)
                        {
                            var reportingPersonsEntity = new ReportingPersonsEntity();
                            reportingPersonsEntity.EmployeeId = data;
                            reportingPersonsEntity.CompanyId = employeesEntity.CompanyId;
                            reportingPersonsEntity.ReportingPersonEmpId = item;
                            reportingPersonsEntitys.Add(reportingPersonsEntity);
                        }
                    }
                    await _employeesRepository.CreateReportingPersons(reportingPersonsEntitys, data);

                    if (string.IsNullOrEmpty(employees.FatherName))
                    {
                        await InsertEmployeesLog(Common.Constant.UpdateEmployeesLog, employees.EmpId, Common.Constant.Add, sessionEmployeeId, companyId, employeesChangeLogs);
                    }
                    else
                    {
                        await InsertEmployeesLog(Common.Constant.UpdateEmployeesLog, employees.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                    }
                    result = data;
                }
            }

            return result;


        }
        public async Task<OBProfileInfo> GetProfileByEmployeeId(int empId, int companyId)
        {
            var profileinfos = new OBProfileInfo();
            var profileinfo = await _OBEmployeesRepository.GetProfileByEmployeeId(empId,companyId);

            if (profileinfo != null)
            {
                profileinfos = _mapper.Map<OBProfileInfo>(profileinfo);
            }
            profileinfos.ProfileViewImage = "/ProfileImages/" + profileinfos.ProfileName;
            profileinfos.ProfileActionName = string.IsNullOrEmpty(profileinfos.ProfileFilePath) ? "create" : "update";
            return profileinfos;
        }

        public async Task<List<BloodGroup>> GetAllBloodGroup()
        {
            var listOfBloodGroup = new List<BloodGroup>();
            var listBloodGroup = await _OBEmployeesRepository.GetAllBloodGroup();
            if (listBloodGroup != null)
            {
                listOfBloodGroup = _mapper.Map<List<BloodGroup>>(listBloodGroup);
            }
            return listOfBloodGroup;
        }
        public async Task<List<RelationshipType>> GetAllRelationshipType()
        {
            var listOfRelationshipType = new List<RelationshipType>();
            var listRelationshipType = await _OBEmployeesRepository.GetAllRelationshipType();
            if (listRelationshipType != null)
            {
                listOfRelationshipType = _mapper.Map<List<RelationshipType>>(listRelationshipType);
            }
            return listOfRelationshipType;
        }
        public async Task<int> AddProfileInfo(OBProfileInfo profileInfo, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (profileInfo != null)
            {
                var profileInfoDataEntity = await _OBEmployeesRepository.GetProfileByEmployeeId(profileInfo.EmpId,companyId);
                if (profileInfoDataEntity == null)
                {
                    profileInfo.CreatedBy = sessionEmployeeId;
                    profileInfo.CreatedDate = DateTime.Now;
                    profileInfo.DateOfBirth = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfBirth);
                    profileInfo.DateOfJoining = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfJoining);
                    var profileInfoEntity = _mapper.Map<ProfileInfoEntity>(profileInfo);
                    var datas = await _OBEmployeesRepository.AddProfileInfo(profileInfoEntity, true);
                    await InsertProfileEmployeesLog(Common.Constant.CreateProfileInfo, result, Common.Constant.Add, sessionEmployeeId,companyId,null);
                    result = profileInfoEntity.EmpId;
                }
                else
                {

                    var employeesChangeLogs = await GetUpdateProfileInfoDiffrencetFieldName(profileInfoDataEntity, profileInfo);
                    profileInfo.UpdatedBy = sessionEmployeeId;
                    profileInfo.UpdatedDate = DateTime.Now;
                    profileInfo.ProfileId = profileInfoDataEntity.ProfileId;
                    profileInfo.ProfileFilePath = string.IsNullOrEmpty(profileInfo.ProfileFilePath) ? profileInfoDataEntity.ProfileFilePath : profileInfo.ProfileFilePath;
                    profileInfo.ProfileName = string.IsNullOrEmpty(profileInfo.ProfileName) ? profileInfoDataEntity.ProfileName : profileInfo.ProfileName;
                    profileInfo.CreatedBy = profileInfoDataEntity.CreatedBy;
                    profileInfo.CreatedDate = profileInfoDataEntity.CreatedDate;
                    profileInfo.DateOfBirth = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfBirth);
                    profileInfo.DateOfJoining = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfJoining);
                    var mapProfileInfoEntity = _mapper.Map<ProfileInfoEntity>(profileInfo);//data model(destination) view model(source) asign                
                    await _OBEmployeesRepository.AddProfileInfo(mapProfileInfoEntity, false);
                    result = profileInfoDataEntity.EmpId;

                    await InsertProfileEmployeesLog(Common.Constant.UpdateProfileInfo, profileInfo.EmpId, Common.Constant.Update, sessionEmployeeId, companyId,employeesChangeLogs);

                }
            }
            return result;
        }
        public async Task<bool> InsertProfileEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId,List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys,companyId);
            return true;

        }
        public async Task<List<EmployeesChangeLog>> GetUpdateProfileInfoDiffrencetFieldName(ProfileInfoEntity profileInfoEntity, OBProfileInfo profileInfo)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (profileInfoEntity != null)
            {
                var result = string.Empty;
                var status = string.Empty;
                if (profileInfoEntity.Gender != profileInfo.Gender)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Gender;
                    result = profileInfoEntity.Gender == 1 ? Common.Constant.Male : Common.Constant.Female;
                    employeesChangeLog.PreviousValue = result;
                    result = profileInfo.Gender == 1 ? Common.Constant.Male : Common.Constant.Female;
                    employeesChangeLog.NewValue = result;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.MaritalStatus != profileInfo.MaritalStatus)
                {

                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.MaritalStatus;
                    status = (profileInfoEntity.MaritalStatus == 1) ? Common.Constant.Married : Common.Constant.Single;
                    employeesChangeLog.PreviousValue = status;
                    status = (profileInfo.MaritalStatus == 1) ? Common.Constant.Married : Common.Constant.Single;
                    employeesChangeLog.NewValue = status;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.DateOfBirth != (!string.IsNullOrEmpty(profileInfo.StrDateOfBirth) ? DateTimeExtensions.ConvertToNotNullDatetime(profileInfo.StrDateOfBirth) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DateOfBirth;
                    employeesChangeLog.PreviousValue = Convert.ToString(profileInfoEntity.DateOfBirth);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(profileInfo.StrDateOfBirth).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.BloodGroup != profileInfo.BloodGroup)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.BloodGroup;
                    employeesChangeLog.PreviousValue = Convert.ToString(profileInfoEntity.BloodGroup);
                    employeesChangeLog.NewValue = Convert.ToString(profileInfo.BloodGroup);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.DateOfJoining != (!string.IsNullOrEmpty(profileInfo.StrDateOfJoining) ? DateTimeExtensions.ConvertToNotNullDatetime(profileInfo.StrDateOfJoining) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DateOfJoining;
                    employeesChangeLog.PreviousValue = profileInfoEntity.DateOfJoining.ToString(Constant.DateFormatHyphen);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(profileInfo.StrDateOfJoining).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.CountryCode != profileInfo.CountryCode)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.CountryCode;
                    employeesChangeLog.PreviousValue = profileInfoEntity.CountryCode;
                    employeesChangeLog.NewValue = profileInfo.CountryCode;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.PhoneNumber != profileInfo.PhoneNumber)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.PhoneNumber;
                    employeesChangeLog.PreviousValue = profileInfoEntity.PhoneNumber;
                    employeesChangeLog.NewValue = profileInfo.PhoneNumber;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.ContactPersonName != profileInfo.ContactPersonName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ContactPersonName;
                    employeesChangeLog.PreviousValue = profileInfoEntity.ContactPersonName;
                    employeesChangeLog.NewValue = profileInfo.ContactPersonName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.RelationshipId != profileInfo.RelationshipId)
                {
                    var relationship = await _employeesRepository.GetAllRelationshipType();
                    var relationshipEntity = profileInfoEntity.RelationshipId == null ? null : relationship.FirstOrDefault(d => d.RelationshipId == profileInfoEntity.RelationshipId);
                    var profileInfoRelationshipEntity = profileInfo.RelationshipId == null ? null : relationship.FirstOrDefault(s => s.RelationshipId == profileInfo.RelationshipId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.RelationshipId;
                    employeesChangeLog.PreviousValue = relationshipEntity == null ? " " : relationshipEntity.RelationshipName;
                    employeesChangeLog.NewValue = profileInfoRelationshipEntity == null ? " " : profileInfoRelationshipEntity.RelationshipName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.ContactNumber != profileInfo.ContactNumber)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ContactNumber;
                    employeesChangeLog.PreviousValue = profileInfoEntity.ContactNumber;
                    employeesChangeLog.NewValue = profileInfo.ContactNumber;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.OthersName != profileInfo.OthersName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.OthersName;
                    employeesChangeLog.PreviousValue = profileInfoEntity.OthersName;
                    employeesChangeLog.NewValue = profileInfo.OthersName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.CountryCodeNumber != profileInfo.CountryCodeNumber)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.CountryCodeNumber;
                    employeesChangeLog.PreviousValue = profileInfoEntity.CountryCodeNumber;
                    employeesChangeLog.NewValue = profileInfo.CountryCodeNumber;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.ProfileName != profileInfo.ProfileName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ProfileName;
                    employeesChangeLog.PreviousValue = profileInfoEntity.ProfileName;
                    employeesChangeLog.NewValue = profileInfo.ProfileName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (profileInfoEntity.ProfileFilePath != profileInfo.ProfileFilePath)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ProfileFilePath;
                    employeesChangeLog.PreviousValue = string.IsNullOrEmpty(profileInfoEntity.ProfileFilePath) ? profileInfo.ProfileFilePath : profileInfoEntity.ProfileFilePath;
                    employeesChangeLog.NewValue = string.IsNullOrEmpty(profileInfo.ProfileName) ? profileInfoEntity.ProfileName : profileInfo.ProfileName;
                    employeesChangeLogs.Add(employeesChangeLog);

                }
            }
            return employeesChangeLogs;
        }

        public string YetToGetProbationDay(CoreModels.Model.ProfileInfoEntity profileInfoEntity, int probationPeriod)
        {
            string result = string.Empty;
            var fromDate = profileInfoEntity.DateOfJoining;
            var toDate = Convert.ToDateTime(fromDate).AddMonths(probationPeriod);
            var days = (toDate.Date - DateTime.Now.Date).Days;
            if (days <= 0)
            {
                return result;
            }
            else
            {
                result = days < 2 ? days + Common.Constant.Dayleft : days + Common.Constant.Daysleft;
            }
            return result;
        }
        public async Task<string> GetEmployeeProfileRecordsByEmployeeId(int empId, int companyId)
        {
            var percentage = 0;
            var profileInfo = await _OBEmployeesRepository.GetProfileByEmployeeId(empId,companyId);
            if (profileInfo != null)
            {
                percentage += 20;
            }
            var addressInfo = await _OBEmployeesRepository.GetAddressByEmployeeId(empId, companyId);
            if (addressInfo != null)
            {
                percentage += 10;
            }
            var OtherDetailsInfo = await _OBEmployeesRepository.GetOtherDetailsByEmployeeId(empId,companyId);
            if (OtherDetailsInfo != null)
            {
                percentage += 10;
            }
            var qualificationInfo = await _OBEmployeesRepository.GetQualificationByEmployeeId(empId,companyId);
            if (qualificationInfo != null)
            {
                percentage += 20;
            }
            var experienceInfo = await _OBEmployeesRepository.GetExperienceByEmployeeId(empId,companyId);
            if (experienceInfo != null)
            {
                percentage += 20;
            }
            var bankDetailsInfo = await _OBEmployeesRepository.GetBankDetailsByEmployeeId(empId,companyId);
            if (bankDetailsInfo != null)
            {
                percentage += 20;
            }
            var completionPercentage = Convert.ToString(percentage);
            return completionPercentage;
        }
        public async Task<string> GetemployeeNameByReportingPersionId(List<int> reportingPersonEmployeeIds, List<EmployeesEntity> allemployee)
        {
            var empNames = string.Empty;
            foreach (var eId in reportingPersonEmployeeIds)
            {
                var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                if (employeeEntity != null)
                {
                    empNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                }
            }
            return empNames.Trim(new char[] { ',' });
        }
        private async Task InsertEmailQueue(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var toEmail = officeEmail;
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.AdminCreateEmployeeReason;
            emailEntity.IsSend = false;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            var email = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }
        public async Task<bool> InsertEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys, companyId);
            return true;
        }

        public async Task<List<EmployeesChangeLog>> GetUpdateEmployeeDiffrencetFieldName(EmployeesEntity employeesEntity, OBEmployees employees, ReportingPersonsEntity reportingPersonsEntity,int companyId) //ReportingPersonsEntity reportingPersonsEntity
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (employeesEntity != null)
            {

                if (employeesEntity.OfficeEmail != employees.OfficeEmail)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.OfficeEmail;
                    employeesChangeLog.PreviousValue = employeesEntity.OfficeEmail;
                    employeesChangeLog.NewValue = employees.OfficeEmail;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.PersonalEmail != employees.PersonalEmail)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.PersonalEmail;
                    employeesChangeLog.PreviousValue = employeesEntity.PersonalEmail;
                    employeesChangeLog.NewValue = employees.PersonalEmail;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.FirstName != employees.FirstName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.FirstName;
                    employeesChangeLog.PreviousValue = employeesEntity.FirstName;
                    employeesChangeLog.NewValue = employees.FirstName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.LastName != employees.LastName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.LastName;
                    employeesChangeLog.PreviousValue = employeesEntity.LastName;
                    employeesChangeLog.NewValue = employees.LastName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.FatherName != employees.FatherName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.FatherName;
                    employeesChangeLog.PreviousValue = employeesEntity.FatherName;
                    employeesChangeLog.NewValue = employees.FatherName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.DepartmentId != employees.DepartmentId)
                {
                    var department = await _OBEmployeesRepository.GetAllDepartment(companyId);
                    var employeesDepartmentEntity = employeesEntity.DepartmentId == null ? null : department.FirstOrDefault(d => d.DepartmentId == employeesEntity.DepartmentId);
                    var departmentEntity = employees.DepartmentId == null ? null : department.FirstOrDefault(s => s.DepartmentId == employees.DepartmentId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Department;
                    employeesChangeLog.PreviousValue = employeesDepartmentEntity == null ? " " : employeesDepartmentEntity.DepartmentName;
                    employeesChangeLog.NewValue = departmentEntity == null ? " " : departmentEntity.DepartmentName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.DesignationId != employees.DesignationId)
                {
                    var designation = await _OBEmployeesRepository.GetAllDesignation(companyId);
                    var employeesDesignationEntity = employeesEntity.DesignationId == null ? null : designation.FirstOrDefault(d => d.DesignationId == employeesEntity.DesignationId);
                    var designationEntity = employees.DesignationId == null ? null : designation.FirstOrDefault(s => s.DesignationId == employees.DesignationId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Designation;
                    employeesChangeLog.PreviousValue = employeesDesignationEntity == null ? " " : employeesDesignationEntity.DesignationName;
                    employeesChangeLog.NewValue = designationEntity == null ? " " : designationEntity.DesignationName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (employeesEntity.SkillName != employees.SkillNames[0].Trim(new char[] { ',' }))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SkillName;
                    employeesChangeLog.PreviousValue = employeesEntity.SkillName;
                    employeesChangeLog.NewValue = employees.SkillNames[0];
                    employeesChangeLogs.Add(employeesChangeLog);
                }


                if (reportingPersonsEntity.ReportingPersonId != employees.ReportingPersonEmpId[0])
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    var reportingPersonsEmployeeEntity = await _OBEmployeesRepository.GetReportingPersonEmployeeById(employees.EmpId,companyId);
                    var allemployee = await _OBEmployeesRepository.GetAllEmployees(companyId);
                    var reportingNames = string.Empty;
                    foreach (var eId in reportingPersonsEmployeeEntity)
                    {
                        var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                        if (employeeEntity != null)
                        {
                            reportingNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                        }
                    }
                    reportingNames.Trim(new char[] { ',' });

                    var reportingPerson = employees.ReportingPersonEmpId;

                    var empNames = string.Empty;
                    foreach (var eId in reportingPerson)
                    {
                        var employeeEntity = allemployee.Where(r => r.EmpId == eId).FirstOrDefault();
                        if (employeeEntity != null)
                        {
                            empNames += employeeEntity.FirstName + employeeEntity.LastName + ",";
                        }
                    }
                    empNames.Trim(new char[] { ',' });

                    employeesChangeLog.FieldName = Common.Constant.ReportingPerson;
                    employeesChangeLog.PreviousValue = reportingNames;
                    employeesChangeLog.NewValue = empNames;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

            }
            return employeesChangeLogs;
        }
        public async Task<OBAddressInfo> GetAddressByEmployeeId(int empId, int companyId)
        {
            var addressinfos = new OBAddressInfo();
            var addressinfo = await _OBEmployeesRepository.GetAddressByEmployeeId(empId,companyId);
            if (addressinfo != null)
            {
                addressinfos = _mapper.Map<OBAddressInfo>(addressinfo);
            }
            return addressinfos;
        }
        public async Task<List<State>> GetAllStates()
        {
            var listOfState = new List<State>();
            var listState = await _OBEmployeesRepository.GetAllStates();
            if (listState != null)
            {
                listOfState = _mapper.Map<List<State>>(listState);
            }
            return listOfState;
        }
        public async Task<List<City>> GetAllCities()
        {
            var listOfCity = new List<City>();
            var listOfCities = await _OBEmployeesRepository.GetAllCities();
            if (listOfCities.Count() > 0)
            {
                listOfCity = _mapper.Map<List<City>>(listOfCities);
            }
            return listOfCity;
        }
        public async Task<List<Country>> GetAllCountry()
        {
            var listOfCountry = new List<Country>();
            var listCountry = await _OBEmployeesRepository.GetAllCountry();
            if (listCountry != null)
            {
                listOfCountry = _mapper.Map<List<Country>>(listCountry);
            }
            return listOfCountry;
        }
        public async Task<int> AddAddressInfo(OBAddressInfo addressInfo, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (addressInfo != null)
            {
                if (addressInfo.AddressId == 0)
                {
                    addressInfo.CreatedBy = sessionEmployeeId;
                    addressInfo.CreatedDate = DateTime.Now;
                    var addressInfoEntity = _mapper.Map<AddressInfoEntity>(addressInfo);
                    var add = await _OBEmployeesRepository.AddAddressInfo(addressInfoEntity);
                    await InsertAddressInfoEmployeesLog(Common.Constant.CreateAddressInfo, result, Common.Constant.Add, sessionEmployeeId, companyId,null);
                    result = addressInfoEntity.EmpId;
                }
                else
                {
                    var addressInfoEntity = await _OBEmployeesRepository.GetAddressByEmployeeId(addressInfo.EmpId,companyId);
                    var employeesChangeLogs = await GetUpdateAddressInfoDiffrencetFieldName(addressInfoEntity, addressInfo, companyId);
                    addressInfo.UpdatedBy = sessionEmployeeId;
                    addressInfo.UpdatedDate = DateTime.Now;
                    addressInfo.CreatedBy = addressInfoEntity.CreatedBy;
                    addressInfo.CreatedDate = addressInfoEntity.CreatedDate;
                    var mapAddressInfoEntity = _mapper.Map<AddressInfoEntity>(addressInfo);
                    var data = await _OBEmployeesRepository.AddAddressInfo(mapAddressInfoEntity);
                    await InsertAddressInfoEmployeesLog(Common.Constant.UpdateAddressInfo, addressInfo.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                    result = addressInfoEntity.EmpId;
                }
            }
            return result;
        }
        public async Task<bool> InsertAddressInfoEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys, companyId);
            return true;

        }
        public async Task<List<EmployeesChangeLog>> GetUpdateAddressInfoDiffrencetFieldName(AddressInfoEntity addressInfoEntity, OBAddressInfo addressInfo,int companyId)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (addressInfoEntity != null)
            {

                if (addressInfoEntity.Address1 != addressInfo.Address1)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Address1;
                    employeesChangeLog.PreviousValue = addressInfoEntity.Address1;
                    employeesChangeLog.NewValue = addressInfo.Address1;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.Address2 != addressInfo.Address2)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Address2;
                    employeesChangeLog.PreviousValue = addressInfoEntity.Address2;
                    employeesChangeLog.NewValue = addressInfo.Address2;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.CityId != addressInfo.CityId)
                {
                    var addressCity = await _OBEmployeesRepository.GetAllCities();
                    var cityEntity = addressInfoEntity?.CityId == 0 ? null : addressCity.FirstOrDefault(m => m.CityId == addressInfoEntity.CityId);
                    var employeesCityEntity = addressInfo?.CityId == 0 ? null : addressCity.FirstOrDefault(o => o.CityId == addressInfo.CityId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.CityId;
                    employeesChangeLog.PreviousValue = cityEntity == null ? " " : cityEntity.CityName;
                    employeesChangeLog.NewValue = employeesCityEntity == null ? " " : employeesCityEntity.CityName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.StateId != addressInfo.StateId)
                {
                    var addressState = await _OBEmployeesRepository.GetAllStates();
                    var stateEntity = addressInfoEntity?.StateId == 0 ? null : addressState.FirstOrDefault(m => m.StateId == addressInfoEntity.StateId);
                    var employeesStateEntity = addressInfo?.StateId == 0 ? null : addressState.FirstOrDefault(o => o.StateId == addressInfo.StateId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.StateId;
                    employeesChangeLog.PreviousValue = stateEntity == null ? " " : stateEntity.StateName;
                    employeesChangeLog.NewValue = employeesStateEntity == null ? " " : employeesStateEntity.StateName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.CountryId != addressInfo.CountryId)
                {
                    var addressCountry = await _OBEmployeesRepository.GetAllCountry();
                    var countryEntity = addressInfoEntity?.CountryId == 0 ? null : addressCountry.FirstOrDefault(m => m.CountryId == addressInfoEntity.CountryId);
                    var employeesCountryEntity = addressInfo?.CountryId == 0 ? null : addressCountry.FirstOrDefault(o => o.CountryId == addressInfo.CountryId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.CountryId;
                    employeesChangeLog.PreviousValue = countryEntity == null ? " " : countryEntity.Name;
                    employeesChangeLog.NewValue = employeesCountryEntity == null ? " " : employeesCountryEntity.Name;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.Pincode != addressInfo.Pincode)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Pincode;
                    employeesChangeLog.PreviousValue = Convert.ToString(addressInfoEntity.Pincode);
                    employeesChangeLog.NewValue = Convert.ToString(addressInfo.Pincode);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryAddress1 != addressInfo.SecondaryAddress1)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryAddress1;
                    employeesChangeLog.PreviousValue = addressInfoEntity.SecondaryAddress1;
                    employeesChangeLog.NewValue = addressInfo.SecondaryAddress1;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryAddress2 != addressInfo.SecondaryAddress2)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryAddress2;
                    employeesChangeLog.PreviousValue = addressInfoEntity.SecondaryAddress2;
                    employeesChangeLog.NewValue = addressInfo.SecondaryAddress2;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryCityId != addressInfo.SecondaryCityId)
                {
                    var addressSecondaryCity = await _OBEmployeesRepository.GetAllCities();
                    var secondaryCityEntity = addressInfoEntity?.SecondaryCityId == 0 ? null : addressSecondaryCity.FirstOrDefault(m => m.CityId == addressInfoEntity.SecondaryCityId);
                    var employeesSecondaryCityEntity = addressInfo?.SecondaryCityId == 0 ? null : addressSecondaryCity.FirstOrDefault(o => o.CityId == addressInfo.SecondaryCityId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryCityId;
                    employeesChangeLog.PreviousValue = secondaryCityEntity == null ? " " : secondaryCityEntity.CityName;
                    employeesChangeLog.NewValue = employeesSecondaryCityEntity == null ? " " : employeesSecondaryCityEntity.CityName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryStateId != addressInfo.SecondaryStateId)
                {
                    var secondaryState = await _OBEmployeesRepository.GetAllStates();
                    var secondaryStateEntity = addressInfoEntity?.SecondaryStateId == 0 ? null : secondaryState.FirstOrDefault(m => m.StateId == addressInfoEntity.SecondaryStateId);
                    var employeesSecondaryStateEntity = addressInfo?.SecondaryStateId == 0 ? null : secondaryState.FirstOrDefault(o => o.StateId == addressInfo.SecondaryStateId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryStateId;
                    employeesChangeLog.PreviousValue = secondaryStateEntity == null ? " " : secondaryStateEntity.StateName;
                    employeesChangeLog.NewValue = employeesSecondaryStateEntity == null ? " " : employeesSecondaryStateEntity.StateName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryCountryId != addressInfo.SecondaryCountryId)
                {
                    var addressCountry = await _OBEmployeesRepository.GetAllCountry();
                    var countryEntity = addressInfoEntity?.SecondaryCountryId == 0 ? null : addressCountry.FirstOrDefault(m => m.CountryId == addressInfoEntity.SecondaryCountryId);
                    var employeesSecondaryCountryEntity = addressInfo?.SecondaryCountryId == 0 ? null : addressCountry.FirstOrDefault(o => o.CountryId == addressInfo.SecondaryCountryId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryCountryId;
                    employeesChangeLog.PreviousValue = countryEntity == null ? " " : countryEntity.Name;
                    employeesChangeLog.NewValue = employeesSecondaryCountryEntity == null ? " " : employeesSecondaryCountryEntity.Name;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (addressInfoEntity.SecondaryPincode != addressInfo.SecondaryPincode)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.SecondaryPincode;
                    employeesChangeLog.PreviousValue = Convert.ToString(addressInfoEntity.SecondaryPincode);
                    employeesChangeLog.NewValue = Convert.ToString(addressInfo.SecondaryPincode);
                    employeesChangeLogs.Add(employeesChangeLog);
                }
            }
            return employeesChangeLogs;
        }
        public async Task<List<State>> GetByCountryId(int countryId)
        {
            var listOfStates = new List<State>();
            var listOfState = await _OBEmployeesRepository.GetByCountryId(countryId);
            if (listOfState.Count() > 0)
            {
                listOfStates = _mapper.Map<List<State>>(listOfState);
            }
            return listOfStates;
        }
        public async Task<List<City>> GetByStateId(int StateId)
        {
            var listOfCitys = new List<City>();
            var listOfCitiesed = await _OBEmployeesRepository.GetByStateId(StateId);
            if (listOfCitiesed.Count() > 0)
            {
                listOfCitys = _mapper.Map<List<City>>(listOfCitiesed);
            }
            return listOfCitys;
        }
        public async Task<OBOtherDetailsViewModel> GetAllOtherDetailsViewModel(int empId,int companyId)
        {
            var otherDetails = new List<OBOtherDetails>();
            var listOfOtherDetailsEntity = await _OBEmployeesRepository.GetAllOtherDetailsViewModel(empId, companyId);
            var otherDetailsViewModel = new OBOtherDetailsViewModel();

            var documentTypes = await GetAllDocumentTypes(companyId);

            otherDetailsViewModel.EmpId = empId;
            if (listOfOtherDetailsEntity.Count() > 0)
            {
                otherDetails = _mapper.Map<List<OBOtherDetails>>(listOfOtherDetailsEntity);
            }
            foreach (var item in otherDetails)
            {
                var docTypeEntity = documentTypes.FirstOrDefault(x => x.DocumentTypeId == item.DocumentTypeId);
                item.DocumentTypeName = docTypeEntity != null ? docTypeEntity.DocumentName : string.Empty;
            }
            otherDetailsViewModel.OtherDetails = otherDetails;
            return otherDetailsViewModel;
        }
        public async Task<List<OBDocumentTypes>> GetAllDocumentTypes(int companyId)
        {
            var listOfDocuments = new List<OBDocumentTypes>();
            var listDocuments = await _OBEmployeesRepository.GetAllDocumentTypes(companyId);
            if (listDocuments != null)
            {
                listOfDocuments = _mapper.Map<List<OBDocumentTypes>>(listDocuments);
            }
            return listOfDocuments;
        }
        public async Task<bool> InsertAndUpdateOtherDetails(OBOtherDetails otherDetails, int sessionEmployeeId, int companyId)
        {
            var result = false;
            if (otherDetails != null)
            {
                if (otherDetails.DetailId == 0)
                {
                    otherDetails.CreatedBy = sessionEmployeeId;
                    otherDetails.CreatedDate = DateTime.Now;
                    if (otherDetails.DocumentTypeId == (int)OtherDetailsStatus.Passport)
                    {
                        otherDetails.ValidFrom = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidFrom);
                        otherDetails.ValidTo = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidTo);
                    }
                    else if (otherDetails.DocumentTypeId == (int)OtherDetailsStatus.DrivingLicence)
                    {
                        otherDetails.ValidTo = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidTo);
                    }
                    var otherDetailsEntity = _mapper.Map<OtherDetailsEntity>(otherDetails);
                    var detailId = await _OBEmployeesRepository.InsertAndUpdateOtherDetails(otherDetailsEntity);
                    result = true;

                    if (otherDetails.OtherDetailsAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<OtherDetailsAttachmentsEntity>();
                        foreach (var item in otherDetails.OtherDetailsAttachments)
                        {
                            var attachmentsEntity = new OtherDetailsAttachmentsEntity();
                            attachmentsEntity.DetailId = detailId;
                            attachmentsEntity.EmpId = otherDetails.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.DocumentName = item.DocumentName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertOtherDetailsAttachment(attachmentsEntitys, detailId);

                    }
                    await InsertOtherDetailsEmployeesLog(Common.Constant.CreateOtherDetails, otherDetails.EmpId, Common.Constant.Add, sessionEmployeeId,companyId,null);
                }
                else
                {
                    var otherDetailsEntity = await _OBEmployeesRepository.GetotherDetailsBydetailId(otherDetails.DetailId, companyId);
                    var otherDetailsAttachmentsEntity = await _OBEmployeesRepository.GetOtherDetailsAttachmentsByEmployeeId(otherDetails.DetailId);
                    var employeesChangeLogs = await GetUpdateOtherDetailsDiffrencetFieldName(otherDetailsEntity, otherDetails, otherDetailsAttachmentsEntity, companyId);
                    otherDetailsEntity.UpdatedBy = sessionEmployeeId;
                    otherDetailsEntity.UpdatedDate = DateTime.Now;
                    otherDetailsEntity.DocumentTypeId = otherDetails.DocumentTypeId;
                    otherDetailsEntity.DocumentNumber = otherDetails.DocumentNumber;
                    otherDetailsEntity.UANNumber = otherDetails.UANNumber;
                    //otherDetailsEntity.Document = string.IsNullOrEmpty(otherDetails.Document) ? otherDetailsEntity.Document : otherDetails.Document;
                    //otherDetailsEntity.DocumentName = string.IsNullOrEmpty(otherDetails.DocumentName) ? otherDetailsEntity.DocumentName : otherDetails.DocumentName;
                    if (otherDetails.DocumentTypeId == (int)OtherDetailsStatus.Passport)
                    {
                        otherDetailsEntity.ValidFrom = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidFrom);
                        otherDetailsEntity.ValidTo = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidTo);
                    }
                    else if (otherDetails.DocumentTypeId == (int)OtherDetailsStatus.DrivingLicence)
                    {
                        otherDetailsEntity.ValidTo = DateTimeExtensions.ConvertToDatetime(otherDetails.StrValidTo);
                    }
                    var detailId = await _OBEmployeesRepository.InsertAndUpdateOtherDetails(otherDetailsEntity);
                    result = true;

                    if (otherDetails.OtherDetailsAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<OtherDetailsAttachmentsEntity>();
                        foreach (var item in otherDetails.OtherDetailsAttachments)
                        {
                            var attachmentsEntity = new OtherDetailsAttachmentsEntity();
                            attachmentsEntity.DetailId = detailId;
                            attachmentsEntity.EmpId = otherDetails.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.DocumentName = item.DocumentName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertOtherDetailsAttachment(attachmentsEntitys, detailId);
                    }
                    await InsertOtherDetailsEmployeesLog(Common.Constant.UpdateOtherDetails, otherDetails.EmpId, Common.Constant.Update, sessionEmployeeId, companyId,employeesChangeLogs);
                }
            }

            return result;
        }
        public async Task<bool> InsertOtherDetailsEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId,List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys,companyId);
            return true;
        }
        public async Task<List<EmployeesChangeLog>> GetUpdateOtherDetailsDiffrencetFieldName(OtherDetailsEntity otherDetailsEntity, OBOtherDetails otherDetails, OtherDetailsAttachmentsEntity otherDetailsAttachmentsEntity,int companyId)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (otherDetailsEntity != null)
            {

                if (otherDetailsEntity.DocumentNumber != otherDetails.DocumentNumber)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DocumentNumber;
                    employeesChangeLog.PreviousValue = otherDetailsEntity.DocumentNumber;
                    employeesChangeLog.NewValue = otherDetails.DocumentNumber;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (otherDetailsEntity.DocumentTypeId != otherDetails.DocumentTypeId)
                {
                    var documentType = await _OBEmployeesRepository.GetAllDocumentTypes(companyId);
                    var otherDocumentEntity = otherDetailsEntity.DocumentTypeId == null ? null : documentType.FirstOrDefault(d => d.DocumentTypeId == otherDetailsEntity.DocumentTypeId);
                    var documentEntity = otherDetails.DocumentTypeId == null ? null : documentType.FirstOrDefault(s => s.DocumentTypeId == otherDetails.DocumentTypeId);
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DocumentTypeId;
                    employeesChangeLog.PreviousValue = otherDocumentEntity == null ? " " : otherDocumentEntity.DocumentName;
                    employeesChangeLog.NewValue = documentEntity == null ? " " : documentEntity.DocumentName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (otherDetailsEntity.ValidFrom != (!string.IsNullOrEmpty(otherDetails.StrValidFrom) ? DateTimeExtensions.ConvertToNotNullDatetime(otherDetails.StrValidFrom) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ValidFrom;
                    employeesChangeLog.PreviousValue = otherDetailsEntity.ValidFrom?.ToString(Constant.DateFormatHyphen);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(otherDetails.StrValidFrom).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (otherDetailsEntity.ValidTo != (!string.IsNullOrEmpty(otherDetails.StrValidTo) ? DateTimeExtensions.ConvertToNotNullDatetime(otherDetails.StrValidTo) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.ValidTo;
                    employeesChangeLog.PreviousValue = otherDetailsEntity.ValidTo?.ToString(Constant.DateFormatHyphen);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(otherDetails.StrValidTo).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (otherDetailsAttachmentsEntity.Document.Trim(new char[] { ',' }) != (!string.IsNullOrEmpty(otherDetails.OtherDetailsAttachments.ToString()) ? (otherDetails.OtherDetailsAttachments.ToString()) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();

                    var otherDetailsAttachment = await _OBEmployeesRepository.GetAllOtherDetailsAttachment(otherDetails.DetailId);
                    List<string> listotherDetail = new List<string>();
                    string otherDetail = string.Empty;

                    foreach (var item in otherDetailsAttachment)
                    {
                        string fileName = item.Document;
                        listotherDetail.Add(fileName);
                    }
                    otherDetail = string.Join(",", listotherDetail);

                    var attachment = otherDetails.OtherDetailsAttachments;
                    List<string> listRows = new List<string>();
                    string rowValues = string.Empty;

                    foreach (var item in attachment)
                    {
                        string fileName = item.Document;
                        listRows.Add(fileName);
                    }
                    rowValues = string.Join(",", listRows);

                    employeesChangeLog.FieldName = Common.Constant.Document;
                    employeesChangeLog.PreviousValue = otherDetail;
                    employeesChangeLog.NewValue = rowValues;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

            }
            return employeesChangeLogs;
        }
        public async Task DeleteOtherDetails(OBOtherDetails otherDetails)
        {
            var otherDetailsEntity = await _OBEmployeesRepository.GetotherDetailsBydetailId(otherDetails.DetailId,otherDetails.CompanyId);
            otherDetailsEntity.IsDeleted = true;
            await _OBEmployeesRepository.DeleteOtherDetails(otherDetailsEntity);

            var otherDetailsAttachementEntitys = await _OBEmployeesRepository.GetOtherDetailsDocumentAndFilePath(otherDetails.DetailId);
            foreach (var item in otherDetailsAttachementEntitys)
            {
                item.IsDeleted = true;
            }
            await _OBEmployeesRepository.DeleteOtherDetailsAttachement(otherDetailsAttachementEntitys);
        }
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var otherDetailsDocuments = await _OBEmployeesRepository.GetDocumentNameByDetailId(detailId);
            return otherDetailsDocuments;
        }
        public async Task<List<OBOtherDetailsDocumentFilePath>> GetOtherDetailsDocumentAndFilePath(int detailId)
        {
            var otherDetailsDocumentFilePaths = new List<OBOtherDetailsDocumentFilePath>();
            var docNmaesAndFilePath = await _OBEmployeesRepository.GetOtherDetailsDocumentAndFilePath(detailId);
            foreach (var item in docNmaesAndFilePath)
            {
                var otherDetailsDocumentFilePath = new OBOtherDetailsDocumentFilePath();
                otherDetailsDocumentFilePath.EmpId = item.EmpId;
                otherDetailsDocumentFilePath.Document = item.Document;
                otherDetailsDocumentFilePath.DocumentName = item.DocumentName;
                otherDetailsDocumentFilePaths.Add(otherDetailsDocumentFilePath);
            }
            return otherDetailsDocumentFilePaths;
        }
        public async Task<OBQualificationViewModel> GetAllQulificationViewModel(int empId, int companyId)
        {
            var qulificationModel = new List<OBQulification>();
            var listOfQualificationEntity = await _OBEmployeesRepository.GetAllQulificationViewModel(empId,companyId);
            var qualificationViewModel = new OBQualificationViewModel();
            qualificationViewModel.EmpId = empId;

            if (listOfQualificationEntity.Count() > 0)
            {
                qulificationModel = _mapper.Map<List<OBQulification>>(listOfQualificationEntity);
            }
            qualificationViewModel.Qualifications = qulificationModel;

            return qualificationViewModel;
        }
        public async Task<bool> InsertAndUpdateQualification(OBQulification qualification, int sessionEmployeeId, int companyId)
        {
            var result = false;
            if (qualification != null)
            {
                qualification.Percentage.TrimStart();
                qualification.QualificationType.TrimStart();
                Convert.ToInt32(qualification.YearOfPassing.ToString().TrimStart());
                qualification.InstitutionName.TrimStart();

                if (qualification.QualificationId == 0)
                {
                    qualification.CreatedBy = sessionEmployeeId;
                    qualification.CreatedDate = DateTime.Now;
                    var qualificationEntity = _mapper.Map<QualificationEntity>(qualification);
                    var qualificationId = await _OBEmployeesRepository.InsertAndUpdateQualification(qualificationEntity);
                    result = true;
                    if (qualification.QualificationAttachments != null && qualification.QualificationAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<QualificationAttachmentsEntity>();
                        foreach (var item in qualification.QualificationAttachments)
                        {
                            var attachmentsEntity = new QualificationAttachmentsEntity();
                            attachmentsEntity.QualificationId = qualificationId;
                            attachmentsEntity.EmpId = qualification.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.QualificationName = item.QualificationName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertQualificationAttachment(attachmentsEntitys, qualificationId);
                    }
                    await InsertQualificationEmployeesLog(Common.Constant.CreateQualification, qualification.EmpId, Common.Constant.Add, sessionEmployeeId, companyId,null);
                }
                else
                {
                    var qualificationEntity = await _OBEmployeesRepository.GetQualificationByQualificationId(qualification.QualificationId, qualification.CompanyId);
                    var attachmentsQualificationEntity = await _OBEmployeesRepository.GetQualificationAttachmentsByEmployeeId(qualification.QualificationId);
                    var employeesChangeLogs = await GetUpdateQualificationDiffrencetFieldName(qualificationEntity, qualification, attachmentsQualificationEntity);
                    qualificationEntity.UpdatedBy = sessionEmployeeId;
                    qualificationEntity.UpdatedDate = DateTime.Now;
                    qualificationEntity.Percentage = qualification.Percentage;
                    qualificationEntity.QualificationType = qualification.QualificationType;
                    qualificationEntity.YearOfPassing = qualification.YearOfPassing;
                    qualificationEntity.InstitutionName = qualification.InstitutionName;
                    var qualificationId = await _OBEmployeesRepository.InsertAndUpdateQualification(qualificationEntity);
                    result = true;
                    if (qualification.QualificationAttachments != null && qualification.QualificationAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<QualificationAttachmentsEntity>();
                        foreach (var item in qualification.QualificationAttachments)
                        {
                            var attachmentsEntity = new QualificationAttachmentsEntity();
                            attachmentsEntity.QualificationId = qualificationId;
                            attachmentsEntity.EmpId = qualification.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.QualificationName = item.QualificationName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertQualificationAttachment(attachmentsEntitys, qualificationId);
                    }
                    await InsertQualificationEmployeesLog(Common.Constant.UpdateQualification, qualification.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                }
            }
            return result;
        }
        public async Task<bool> InsertQualificationEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys, companyId);
            return true;
        }
        public async Task<List<EmployeesChangeLog>> GetUpdateQualificationDiffrencetFieldName(QualificationEntity qualificationEntity, OBQulification qualification, QualificationAttachmentsEntity qualificationAttachmentsEntity)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (qualificationEntity != null)
            {
                if (qualificationEntity.QualificationType != qualification.QualificationType)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.QualificationType;
                    employeesChangeLog.PreviousValue = qualificationEntity.QualificationType;
                    employeesChangeLog.NewValue = qualification.QualificationType;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (qualificationEntity.Percentage != qualification.Percentage)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Percentage;
                    employeesChangeLog.PreviousValue = qualificationEntity.Percentage;
                    employeesChangeLog.NewValue = qualification.Percentage;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (qualificationEntity.YearOfPassing != qualification.YearOfPassing)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.YearOfPassing;
                    employeesChangeLog.PreviousValue = Convert.ToString(qualificationEntity.YearOfPassing);
                    employeesChangeLog.NewValue = Convert.ToString(qualification.YearOfPassing);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (qualificationEntity.InstitutionName != qualification.InstitutionName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.InstitutionName;
                    employeesChangeLog.PreviousValue = qualificationEntity.InstitutionName;
                    employeesChangeLog.NewValue = qualification.InstitutionName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (qualificationAttachmentsEntity.Document != (!string.IsNullOrEmpty(qualification.QualificationAttachments.ToString()) ? (qualification.QualificationAttachments.ToString()) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();

                    var qualificationAttachment = await _OBEmployeesRepository.GetAllQualificationAttachments(qualification.QualificationId);
                    List<string> listqualificationAttachment = new List<string>();
                    string qualificationAttachmentName = string.Empty;
                    foreach (var item in qualificationAttachment)
                    {
                        string fileName = item.Document;
                        listqualificationAttachment.Add(fileName);
                    }
                    qualificationAttachmentName = string.Join(",", listqualificationAttachment);

                    var attachmentQualification = qualification.QualificationAttachments;
                    List<string> listQualification = new List<string>();
                    string rowQualification = string.Empty;

                    foreach (var item in attachmentQualification)
                    {
                        string fileName = item.Document;
                        listQualification.Add(fileName);
                    }
                    rowQualification = string.Join(",", listQualification);

                    employeesChangeLog.FieldName = Common.Constant.Document;
                    employeesChangeLog.PreviousValue = qualificationAttachmentName;
                    employeesChangeLog.NewValue = rowQualification;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

            }
            return employeesChangeLogs;
        }
        public async Task<List<OBQulification>> GetAllQualification(int EmpId,int companyId)
        {
            // return await _employeesRepository.GetAllQualification(EmpId);
            var listOfQualification = new List<OBQulification>();
            var listQualification = await _OBEmployeesRepository.GetAllQualification(EmpId, companyId);
            if (listQualification != null)
            {
                listOfQualification = _mapper.Map<List<OBQulification>>(listQualification);
            }
            return listOfQualification;
        }
        public async Task<List<string>> GetDocumentNameByQualificationId(int qualificationId)
        {
            var qualificationDocuments = await _OBEmployeesRepository.GetDocumentNameByQualificationId(qualificationId);
            return qualificationDocuments;
        }
        public async Task<List<OBQualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var qualificationDocumentFilePaths = new List<OBQualificationDocumentFilePath>();
            var docNmaesAndFilePath = await _OBEmployeesRepository.GetQualificationDocumentAndFilePath(qualificationId);
            foreach (var item in docNmaesAndFilePath)
            {
                var qualificationDocumentFilePath = new OBQualificationDocumentFilePath();
                qualificationDocumentFilePath.EmpId = item.EmpId;
                qualificationDocumentFilePath.Document = item.Document != null ? item.Document : string.Empty;
                qualificationDocumentFilePath.QualificationName = item.QualificationName != null ? item.QualificationName : string.Empty;
                qualificationDocumentFilePaths.Add(qualificationDocumentFilePath);
            }
            return qualificationDocumentFilePaths;
        }
        public async Task DeleteQualification(OBQulification qualification)
        {
            var qualificationEntity = await _OBEmployeesRepository.GetQualificationByQualificationId(qualification.QualificationId, qualification.CompanyId);
            qualificationEntity.IsDeleted = true;
            await _OBEmployeesRepository.DeleteQualification(qualificationEntity);

            var qualificationAttachementEntitys = await _OBEmployeesRepository.GetQualificationDocumentAndFilePath(qualification.QualificationId);
            foreach (var item in qualificationAttachementEntitys)
            {
                item.IsDeleted = true;
            }

            await _OBEmployeesRepository.DeleteQualificationAttachement(qualificationAttachementEntitys);
        }
        public async Task<OBExperienceViewModel> GetAllExperienceViewModel(int empId,int companyId)
        {
            var experienceModel = new List<OBExperience>();
            var listOfExperienceEntity = await _OBEmployeesRepository.GetAllExperienceViewModel(empId, companyId);
            var experienceViewModel = new OBExperienceViewModel();
            experienceViewModel.EmpId = empId;
            if (listOfExperienceEntity.Count() > 0)
            {
                experienceModel = _mapper.Map<List<OBExperience>>(listOfExperienceEntity);
            }
            experienceViewModel.Experiences = experienceModel;
            return experienceViewModel;
        }
        public async Task<bool> InsertAndUpdateExperience(OBExperience experience, int sessionEmployeeId)
        {
            var result = false;
            if (experience != null)
            {
                if (experience.ExperienceId == 0)
                {
                    experience.CreatedBy = sessionEmployeeId;
                    experience.CreatedDate = DateTime.Now;
                    experience.DateOfJoining = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfJoining);
                    experience.DateOfRelieving = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfRelieving);
                    var experienceEntity = _mapper.Map<ExperienceEntity>(experience);
                    var experienceId = await _OBEmployeesRepository.InsertAndUpdateExperience(experienceEntity);
                    result = true;
                    if (experience.ExperienceAttachment.Count() > 0)
                    {
                        var attachmentsEntitys = new List<ExperienceAttachmentsEntity>();
                        foreach (var item in experience.ExperienceAttachment)
                        {
                            var attachmentsEntity = new ExperienceAttachmentsEntity();
                            attachmentsEntity.ExperienceId = experienceId;
                            attachmentsEntity.EmpId = experience.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.ExperienceName = item.ExperienceName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertExperienceAttachment(attachmentsEntitys, experienceId);
                    }
                    await InsertExperienceEmployeesLog(Common.Constant.CreateExperience, experience.EmpId, Common.Constant.Add, sessionEmployeeId, experience.CompanyId,null);
                }
                else
                {
                    var experienceEntity = await _OBEmployeesRepository.GetExperienceByExperienceId(experience.ExperienceId, experience.CompanyId);
                    var ExperienceAttachmentsEntity = await _OBEmployeesRepository.GetExperienceAttachmentsByEmployeeId(experience.ExperienceId);
                    var employeesChangeLogs = await GetUpdateExperienceDiffrencetFieldName(experienceEntity, experience, ExperienceAttachmentsEntity);
                    experienceEntity.UpdatedBy = sessionEmployeeId;
                    experienceEntity.UpdatedDate = DateTime.Now;
                    experienceEntity.PreviousCompany = experience.PreviousCompany;
                    experienceEntity.DateOfJoining = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfJoining);
                    experienceEntity.DateOfRelieving = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfRelieving);
                   
                    var experienceId = await _OBEmployeesRepository.InsertAndUpdateExperience(experienceEntity);
                    result = true;
                    if (experience.ExperienceAttachment.Count() > 0)
                    {
                        var attachmentsEntitys = new List<ExperienceAttachmentsEntity>();
                        foreach (var item in experience.ExperienceAttachment)
                        {
                            var attachmentsEntity = new ExperienceAttachmentsEntity();
                            attachmentsEntity.ExperienceId = experienceId;
                            attachmentsEntity.EmpId = experience.EmpId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.ExperienceName = item.ExperienceName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _OBEmployeesRepository.InsertExperienceAttachment(attachmentsEntitys, experienceId);
                    }
                    await InsertExperienceEmployeesLog(Common.Constant.UpdateExperience, experience.EmpId, Common.Constant.Update, sessionEmployeeId, experience.CompanyId, employeesChangeLogs);

                }
            }
            return result;
        }
        public async Task<bool> InsertExperienceEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Delete)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys,companyId);
            return true;
        }
        public async Task<List<EmployeesChangeLog>> GetUpdateExperienceDiffrencetFieldName(ExperienceEntity experienceEntity, OBExperience experience, ExperienceAttachmentsEntity experienceAttachmentsEntity)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (experienceEntity != null)
            {

                if (experienceEntity.PreviousCompany != experience.PreviousCompany)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.PreviousCompany;
                    employeesChangeLog.PreviousValue = experienceEntity.PreviousCompany;
                    employeesChangeLog.NewValue = experience.PreviousCompany;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (experienceEntity.DateOfJoining != (!string.IsNullOrEmpty(experience.StrDateOfJoining) ? DateTimeExtensions.ConvertToNotNullDatetime(experience.StrDateOfJoining) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DateOfJoining;
                    employeesChangeLog.PreviousValue = experienceEntity.DateOfJoining.ToString(Constant.DateFormatHyphen);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(experience.StrDateOfJoining).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }


                if (experienceEntity.DateOfRelieving != (!string.IsNullOrEmpty(experience.StrDateOfRelieving) ? DateTimeExtensions.ConvertToNotNullDatetime(experience.StrDateOfRelieving) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.DateOfRelieving;
                    employeesChangeLog.PreviousValue = experienceEntity.DateOfRelieving.ToString(Constant.DateFormatHyphen);
                    employeesChangeLog.NewValue = DateTimeExtensions.ConvertToNotNullDatetime(experience.StrDateOfRelieving).ToString(Constant.DateFormatHyphen);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (experienceAttachmentsEntity.Document != (!string.IsNullOrEmpty(experience.ExperienceAttachment.ToString()) ? (experience.ExperienceAttachment.ToString()) : null))
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    var experienceAttachments = await _OBEmployeesRepository.GetAllExperienceAttachment(experience.ExperienceId);
                    List<string> listExperienceAttachment = new List<string>();
                    string rowExperienceAttachment = string.Empty;
                    foreach (var item in experienceAttachments)
                    {
                        string fileName = item.Document;
                        listExperienceAttachment.Add(fileName);
                    }
                    rowExperienceAttachment = string.Join(",", listExperienceAttachment);

                    var experienceAttachment = experience.ExperienceAttachment;
                    List<string> listExperience = new List<string>();
                    string rowExperience = string.Empty;
                    foreach (var item in experienceAttachment)
                    {
                        string fileName = item.Document;
                        listExperience.Add(fileName);
                    }
                    rowExperience = string.Join(",", listExperience);

                    employeesChangeLog.FieldName = Common.Constant.Document;
                    employeesChangeLog.PreviousValue = rowExperienceAttachment;
                    employeesChangeLog.NewValue = rowExperience;
                    employeesChangeLogs.Add(employeesChangeLog);
                }
            }
            return employeesChangeLogs;
        }
        public async Task<List<OBExperience>> GetAllExperience(int EmpId, int companyId)
        {
            var listOfExperience = new List<OBExperience>();
            var listExperience = await _OBEmployeesRepository.GetAllExperience(EmpId,companyId);
            if (listExperience != null)
            {
                listOfExperience = _mapper.Map<List<OBExperience>>(listExperience);
            }
            return listOfExperience;
        }
        public async Task<List<string>> GetDocumentNameByExperienceId(int experienceId)
        {
            var qualificationDocuments = await _OBEmployeesRepository.GetDocumentNameByExperienceId(experienceId);
            return qualificationDocuments;
        }
        public async Task<List<OBExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var experienceDocumentFilePaths = new List<OBExperienceDocumentFilePath>();
            var docNmaesAndFilePath = await _OBEmployeesRepository.GetExperienceDocumentAndFilePath(experienceId);
            foreach (var item in docNmaesAndFilePath)
            {
                var experienceDocumentFilePath = new OBExperienceDocumentFilePath();
                experienceDocumentFilePath.Document = item.Document;
                experienceDocumentFilePath.ExperienceName = item.ExperienceName;
                experienceDocumentFilePath.EmpId = item.EmpId;
                experienceDocumentFilePaths.Add(experienceDocumentFilePath);
            }
            return experienceDocumentFilePaths;
        }
        public async Task DeleteExperience(OBExperience experience, int sessionEmployeeId)
        {
            var experienceEntity = await _OBEmployeesRepository.GetExperienceByExperienceId(experience.ExperienceId, experience.CompanyId);
            experienceEntity.IsDeleted = true;
            await _OBEmployeesRepository.DeleteExperience(experienceEntity);

            var experienceAttachementEntitys = await _OBEmployeesRepository.GetExperienceDocumentAndFilePath(experience.ExperienceId);
            foreach (var item in experienceAttachementEntitys)
            {
                item.IsDeleted = true;
            }
            await _OBEmployeesRepository.DeleteExperienceAttachement(experienceAttachementEntitys);
        }
        public async Task<OBBankDetails> GetBankDetailsByEmployeeId(int empId, int companyId)
        {
            var bankDetails = new OBBankDetails();
            var addbankDetails = await _OBEmployeesRepository.GetBankDetailsByEmployeeId(empId,companyId);
            var isVerified = await _OBEmployeesRepository.GetEmployeeById(empId, companyId);
            if (addbankDetails != null)
            {
                bankDetails = _mapper.Map<OBBankDetails>(addbankDetails);
            }
            if (isVerified != null)
            {
                bankDetails.IsVerified = isVerified.IsVerified;
            }

            return bankDetails;
        }
        public async Task<int> AddBankDetails(OBBankDetails bankDetails, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (bankDetails != null)
            {
                if (bankDetails.BankId == 0)
                {
                    bankDetails.CreatedBy = sessionEmployeeId;
                    bankDetails.CreatedDate = DateTime.Now;
                    var bankDetailsEntity = _mapper.Map<BankDetailsEntity>(bankDetails);
                    var add = await _OBEmployeesRepository.AddBankDetails(bankDetailsEntity);
                    await InsertBankDetailsEmployeesLog(Common.Constant.CreateBankDetails, result, Common.Constant.Add, sessionEmployeeId, companyId,null);
                    result = bankDetailsEntity.EmpId;
                }
                else
                {
                    var bankDetailsEntity = await _OBEmployeesRepository.GetBankDetailsByEmployeeId(bankDetails.EmpId,companyId);
                    var employeesChangeLogs = await GetUpdateBankDetailsDiffrencetFieldName(bankDetailsEntity, bankDetails);
                    bankDetails.UpdatedBy = sessionEmployeeId;
                    bankDetails.UpdatedDate = DateTime.Now;
                    bankDetails.CreatedDate = bankDetailsEntity.CreatedDate;
                    bankDetails.CreatedBy = bankDetailsEntity.CreatedBy;
                    var mapBankDetailsEntity = _mapper.Map<BankDetailsEntity>(bankDetails);
                    var data = await _OBEmployeesRepository.AddBankDetails(mapBankDetailsEntity);
                    await InsertBankDetailsEmployeesLog(Common.Constant.UpdateBankDetails, bankDetails.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                    result = bankDetailsEntity.EmpId;
                }
            }

            return result;
        }
        public async Task<bool> InsertBankDetailsEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
        {
            var employeesLogEntitys = new List<EmployeesLogEntity>();
            if (eventName == Common.Constant.Add || eventName == Common.Constant.Update)
            {
                var employeesLogEntity = new EmployeesLogEntity();
                employeesLogEntity.EmpId = empId;
                employeesLogEntity.SectionName = sectionName;
                employeesLogEntity.Event = eventName;
                employeesLogEntity.CreatedBy = sessionEmployeeId;
                employeesLogEntity.CreatedDate = DateTime.Now;
                employeesLogEntitys.Add(employeesLogEntity);
            }
            else
            {
                foreach (var item in employeesChangeLogs)
                {
                    var employeesLogEntity = new EmployeesLogEntity();
                    employeesLogEntity.EmpId = empId;
                    employeesLogEntity.SectionName = sectionName;
                    employeesLogEntity.Event = eventName;
                    employeesLogEntity.FieldName = item.FieldName;
                    employeesLogEntity.PreviousValue = item.PreviousValue;
                    employeesLogEntity.NewValue = item.NewValue;
                    employeesLogEntity.CreatedBy = sessionEmployeeId;
                    employeesLogEntity.CreatedDate = DateTime.Now;
                    employeesLogEntitys.Add(employeesLogEntity);
                }
            }
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys, companyId);
            return true;
        }
        public async Task<List<EmployeesChangeLog>> GetUpdateBankDetailsDiffrencetFieldName(BankDetailsEntity bankDetailsEntity, OBBankDetails bankDetails)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (bankDetailsEntity != null)
            {

                if (bankDetailsEntity.AccountHolderName != bankDetails.AccountHolderName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.AccountHolderName;
                    employeesChangeLog.PreviousValue = bankDetailsEntity.AccountHolderName;
                    employeesChangeLog.NewValue = bankDetails.AccountHolderName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (bankDetailsEntity.AccountNumber != bankDetails.AccountNumber)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.AccountNumber;
                    employeesChangeLog.PreviousValue = Convert.ToString(bankDetailsEntity.AccountNumber);
                    employeesChangeLog.NewValue = Convert.ToString(bankDetails.AccountNumber);
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (bankDetailsEntity.BankName != bankDetails.BankName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.BankName;
                    employeesChangeLog.PreviousValue = bankDetailsEntity.BankName;
                    employeesChangeLog.NewValue = bankDetails.BankName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (bankDetailsEntity.IFSCCode != bankDetails.IFSCCode)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.IFSCCode;
                    employeesChangeLog.PreviousValue = bankDetailsEntity.IFSCCode;
                    employeesChangeLog.NewValue = bankDetails.IFSCCode;
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (bankDetailsEntity.BranchName != bankDetails.BranchName)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.BankName;
                    employeesChangeLog.PreviousValue = bankDetailsEntity.BranchName;
                    employeesChangeLog.NewValue = bankDetails.BranchName;
                    employeesChangeLogs.Add(employeesChangeLog);
                }
            }
            return employeesChangeLogs;
        }

        // View 
        public async Task<OBViewEmployee> GetEmployeeByEmployeeId(int empId, int companyId)
        {
            var viewEmployeeModel = new OBViewEmployee();
            var empEntity = await _OBEmployeesRepository.GetEmployeeByIdView(empId,companyId);
            if (empEntity != null)
            {
                var designation = await _masterRepository.GetDesignationByEmployeeId(empEntity.DesignationId,companyId);
                var department = await _masterRepository.GetDepartmentByEmployeeId(empEntity.DepartmentId,companyId);
                var allemployee = await _OBEmployeesRepository.GetAllEmployees(companyId);
                List<int> reportingPersonEmployeeIds = await _OBEmployeesRepository.GetReportingPersonEmployeeById(empId,companyId);
                viewEmployeeModel.FirstName = empEntity.FirstName;
                viewEmployeeModel.LastName = empEntity.LastName;
                viewEmployeeModel.FatherName = string.IsNullOrEmpty(empEntity.FatherName) ? string.Empty : empEntity.FatherName;
                viewEmployeeModel.OfficeEmail = empEntity.OfficeEmail;
                viewEmployeeModel.PersonalEmail = string.IsNullOrEmpty(empEntity.PersonalEmail) ? string.Empty : empEntity.PersonalEmail;
                viewEmployeeModel.Designation = designation.DesignationName;
                viewEmployeeModel.Department = department.DepartmentName;
                viewEmployeeModel.SkillName = string.IsNullOrEmpty(empEntity.SkillName) ? string.Empty : empEntity.SkillName;
                viewEmployeeModel.ReportingPerson = await GetemployeeNameByReportingPersionId(reportingPersonEmployeeIds, allemployee);
            }
            return viewEmployeeModel;
        }
        public async Task<OBEmployees> GetEmployeeByEmployeeIdView(int empId, int companyId)
        {
            var employeeViewModel = new OBEmployees();
            var empEntity = await _OBEmployeesRepository.GetEmployeeByIdView(empId,companyId);
            //var assetEntity = await _assetRepository.GetAssetByEmployeeId(empId);
            //var benefitEntity = await _biofitRepository.GetEmployeeBenefitsByEmployeeId(empId);
            //var count = assetEntity.Count();
            //var medicalBenefitEntity = await _benefitRepository.GetAllEmployeeMedicalBenefits(empId);
            if (empEntity != null)
            {
                employeeViewModel = _mapper.Map<OBEmployees>(empEntity);
                employeeViewModel.EmployeeSortName = Common.Common.GetEmployeeSortName(empEntity.FirstName, empEntity.LastName);
                var profileByEmpId = await _OBEmployeesRepository.GetProfileByEmployeeId(empId,companyId);
                employeeViewModel.EmployeeProfileImage = profileByEmpId != null ? profileByEmpId.ProfileName : string.Empty;
            }
            //if (assetEntity != null)
            //{
            //    employeeViewModel.AllAssetId = count;
            //}
            //if (benefitEntity != null)
            //{
            //    employeeViewModel.BenefitId = benefitEntity.BenefitId;
            //}
            return employeeViewModel;
        }
        public async Task<OBViewProfile> GetEmployeeProfileByEmployeeId(int empId,int companyId)
        {
            var profileInfoEntity = await _OBEmployeesRepository.GetProfileByEmployeeIdview(empId, companyId);
            var relationshipTypeEntitys = await _OBEmployeesRepository.GetAllRelationshipType();
            var relationshipTypeEntity = profileInfoEntity == null ? null : relationshipTypeEntitys.FirstOrDefault(x => x.RelationshipId == profileInfoEntity.RelationshipId);

            var viewEmployeeProfileModel = new OBViewProfile();
            if (profileInfoEntity != null)
            {
                viewEmployeeProfileModel.Gender = Convert.ToString((Gender)profileInfoEntity.Gender);
                viewEmployeeProfileModel.MaritalStatus = Convert.ToString((MaritalStatus)profileInfoEntity.MaritalStatus);
                viewEmployeeProfileModel.DateOfBirth = profileInfoEntity.DateOfBirth.ToString(Constant.DateFormatHyphen);
                viewEmployeeProfileModel.BloodGroup = await _employeesRepository.GetBloodGroupNameById(profileInfoEntity.BloodGroup);
                viewEmployeeProfileModel.DateOfJoining = profileInfoEntity.DateOfJoining.ToString(Constant.DateFormatHyphen);
                viewEmployeeProfileModel.PhoneNumber = profileInfoEntity.PhoneNumber;
                viewEmployeeProfileModel.ProfileImage = profileInfoEntity.ProfileName;
                viewEmployeeProfileModel.CountryCode = profileInfoEntity.CountryCode;
                viewEmployeeProfileModel.ContactPersonName = string.IsNullOrEmpty(profileInfoEntity.ContactPersonName) ? string.Empty : profileInfoEntity.ContactPersonName;
                viewEmployeeProfileModel.Relationshipname = relationshipTypeEntity == null ? " " : relationshipTypeEntity.RelationshipName;
                viewEmployeeProfileModel.ContactNumber = string.IsNullOrEmpty(profileInfoEntity.ContactNumber) ? string.Empty : profileInfoEntity.ContactNumber;
                viewEmployeeProfileModel.CountryCodeNumber = string.IsNullOrEmpty(profileInfoEntity.CountryCodeNumber) ? string.Empty : profileInfoEntity.CountryCodeNumber;
            }
            return viewEmployeeProfileModel;
        }
        public async Task<OBViewAddressInfo> GetEmployeeAddressByEmployeeId(int empId, int companyId)
        {
            var viewAddressInfoModel = new OBViewAddressInfo();
            var addressinfo = await _OBEmployeesRepository.GetAddressByEmployeeId(empId,companyId);
            if (addressinfo != null)
            {
                viewAddressInfoModel.Address1 = addressinfo.Address1;
                viewAddressInfoModel.Address2 = addressinfo.Address2;
                viewAddressInfoModel.CountryName = await _OBEmployeesRepository.GetCountryNameByCountryId(addressinfo.CountryId);
                viewAddressInfoModel.StateName = await _OBEmployeesRepository.GetStateNameByStateId(addressinfo.StateId);
                viewAddressInfoModel.CityName = await _OBEmployeesRepository.GetCityNameByCityId(addressinfo.CityId);
                viewAddressInfoModel.Pincode = addressinfo.Pincode;
                viewAddressInfoModel.SecondaryAddress1 = string.IsNullOrEmpty(addressinfo.SecondaryAddress1) ? string.Empty : addressinfo.SecondaryAddress1;
                viewAddressInfoModel.SecondaryAddress2 = string.IsNullOrEmpty(addressinfo.SecondaryAddress2) ? string.Empty : addressinfo.SecondaryAddress2;
                viewAddressInfoModel.SecondaryCountryName = await _OBEmployeesRepository.GetCountryNameBySecondaryCountryId(addressinfo.SecondaryCountryId);
                viewAddressInfoModel.SecondaryStateName = await _OBEmployeesRepository.GetStateNameBySecondaryStateId(addressinfo.SecondaryStateId);
                viewAddressInfoModel.SecondaryCityName = await _OBEmployeesRepository.GetCityNameBySecondaryCityId(addressinfo.SecondaryCityId);
                viewAddressInfoModel.SecondaryPincode = addressinfo.SecondaryPincode;

            }
            return viewAddressInfoModel;
        }
        public async Task<OBOtherDetailsViewModel> GetAllOtherDetailsView(int empId, int companyId)//view model
        {
            var otherDetails = new List<OBOtherDetails>();
            var listOfOtherDetailsEntity = await _OBEmployeesRepository.GetAllOtherDetailsView(empId,companyId);
            var otherDetailsViewModel = new OBOtherDetailsViewModel();

            var documentTypes = await GetAllDocumentTypes(companyId);

            otherDetailsViewModel.EmpId = empId;
            if (listOfOtherDetailsEntity.Count() > 0)
            {
                otherDetails = _mapper.Map<List<OBOtherDetails>>(listOfOtherDetailsEntity);
            }
            foreach (var item in otherDetails)
            {
                var docTypeEntity = documentTypes.FirstOrDefault(x => x.DocumentTypeId == item.DocumentTypeId);
                item.DocumentTypeName = docTypeEntity != null ? docTypeEntity.DocumentName : string.Empty;
            }
            otherDetailsViewModel.OtherDetails = otherDetails;
            return otherDetailsViewModel;
        }
        public async Task<OBQualificationViewModel> GetAllQulificationView(int empId, int companyId)//view model
        {
            var qulificationModel = new List<OBQulification>();
            var listOfQualificationEntity = await _OBEmployeesRepository.GetAllQulificationView(empId,companyId);
            var qualificationViewModel = new OBQualificationViewModel();
            qualificationViewModel.EmpId = empId;

            if (listOfQualificationEntity.Count() > 0)
            {
                qulificationModel = _mapper.Map<List<OBQulification>>(listOfQualificationEntity);
            }
            qualificationViewModel.Qualifications = qulificationModel;

            return qualificationViewModel;
        }
        public async Task<OBExperienceViewModel> GetAllExperienceView(int empId, int companyId)
        {
            var experienceModel = new List<OBExperience>();
            var listOfExperienceEntity = await _OBEmployeesRepository.GetAllExperienceView(empId,companyId);
            var experienceViewModel = new OBExperienceViewModel();
            experienceViewModel.EmpId = empId;
            if (listOfExperienceEntity.Count() > 0)
            {
                experienceModel = _mapper.Map<List<OBExperience>>(listOfExperienceEntity);
            }
            experienceViewModel.Experiences = experienceModel;
            return experienceViewModel;
        }

        public async Task<WelcomeAboard> GetEmployee(int empId , int companyId)
        {
            var viewModel = new WelcomeAboard();
            var employee = await _OBEmployeesRepository.GetEmployeeById(empId, companyId);
            if (employee != null)
            {
                viewModel.EmpId = employee.EmpId;
                viewModel.EmployeeName = employee.FirstName + " " + employee.LastName;
            }
            var companyInfo = await _companyRepository.GetByCompanyId(companyId);
            if (companyInfo != null)
            {
                viewModel.CompanyName= companyInfo.CompanyName;
            }

            return viewModel;
        }
        public async Task<OnboardingCompletion> GetProfileCompletion (int empId , int companyId)
        {
            var progress = new OnboardingCompletion();

            var percentage = 0;
            var employeeInfo = await _OBEmployeesRepository.GetEmployeeById(empId, companyId);
            if (employeeInfo != null)
            {
                progress.EmployeeName = employeeInfo.FirstName + " " + employeeInfo.LastName;
            }
            var companyInfo = await _companyRepository.GetByCompanyId(companyId);
            if (companyInfo != null)
            {
                progress.CompanyName = companyInfo.CompanyName;
            }
            var profileInfo = await _OBEmployeesRepository.GetProfileByEmployeeId(empId,companyId);
            if (profileInfo != null)
            {
                percentage += 20;
                progress.ProfileInfo = 1;
         
            }
            else
            {
              
                progress.ProfileInfo = 0;
               
            }
            var addressInfo = await _OBEmployeesRepository.GetAddressByEmployeeId(empId,companyId);
            if (addressInfo != null)
            {
                percentage += 10;
                progress.AddressInfo = 1;
                
            }
            else
            {
               
                progress.AddressInfo = 0;
            
            }
            var OtherDetailsInfo = await _OBEmployeesRepository.GetOtherDetailsByEmployeeId(empId,companyId);
            if (OtherDetailsInfo != null)
            {
                percentage += 10;
                progress.OtherDetails = 1;
        
            }
            else
            {
                progress.OtherDetails = 0;
          
            }
            var qualificationInfo = await _OBEmployeesRepository.GetQualificationByEmployeeId(empId,companyId);
            if (qualificationInfo != null)
            {
                percentage += 20;
                progress.QualificationInfo = 1;
         
            }
            else
            {
                progress.QualificationInfo = 0;
             
            }
            var experienceInfo = await _OBEmployeesRepository.GetExperienceByEmployeeId(empId,companyId);
            if (experienceInfo != null)
            {
                percentage += 20;
                progress.ExperienceInfo = 1;
          
            }
            else
            {
                progress.ExperienceInfo = 0;
           
            }
            var bankDetailsInfo = await _OBEmployeesRepository.GetBankDetailsByEmployeeId(empId,companyId);
            if (bankDetailsInfo != null)
            {
                percentage += 20;
                progress.BankDetails = 1;
             
            }
            else
            {
                progress.BankDetails = 0;
            
            }
            var completionPercentage = Convert.ToString(percentage);
            progress.ProfileCompletion= completionPercentage;
            progress.EmpId = empId;
            return progress;

        }

        public async Task<int> UpdateStatus(int empId, int companyId)
        {
            var result = 0;
             var entity = await _OBEmployeesRepository.GetEmployeeById(empId, companyId);
            if (entity != null)
            {
                entity.IsOnboarding = true;
                var profile = await _OBEmployeesRepository.UpdateStatus(entity);
                result = profile;
            }
           
            return result;
        }
    }
}
