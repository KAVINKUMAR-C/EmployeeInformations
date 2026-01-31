using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EmployeeInformations.Business.Service
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMasterRepository _masterRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly IBenefitRepository _benefitRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICompanyPolicyRepository _companyPolicyRepository;

        public EmployeesService(IEmployeesRepository employeesRepository, IMapper mapper, IConfiguration config, IMasterRepository masterRepository, ILeaveRepository leaveRepository, IAssetRepository assetRepository, IBenefitRepository benefitRepository, ICompanyRepository companyRepository, IEmailDraftRepository emailDraftRepository, IAuditLogRepository auditLogRepository, ICompanyPolicyRepository companyPolicyRepository)
        {
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _masterRepository = masterRepository;
            _leaveRepository = leaveRepository;
            _assetRepository = assetRepository;
            _benefitRepository = benefitRepository;
            _companyRepository = companyRepository;
            _emailDraftRepository = emailDraftRepository;
            _auditLogRepository = auditLogRepository;
            _companyPolicyRepository = companyPolicyRepository;
        }

        //Employees

        /// <summary>
        /// Logic to get display the employee detail  by particular employee view
        /// </summary>
        /// <param name="empId" >employee</param>
        public async Task<Employees> GetEmployeeByEmployeeIdView(int empId, int companyId)
        {
            var empEntity = await _employeesRepository.GetEmployeeByIdView(empId, companyId);
            if (empEntity == null) return new Employees();

            var employeeViewModel = _mapper.Map<Employees>(empEntity);
            employeeViewModel.EmployeeSortName = Common.Common.GetEmployeeSortName(empEntity.FirstName, empEntity.LastName);

            var profileByEmpId = await _employeesRepository.GetProfileByEmployeeId(empId, companyId);
            employeeViewModel.EmployeeProfileImage = profileByEmpId?.ProfileName ?? string.Empty;

            var assetEntity = await _assetRepository.GetAssetByEmployeeId(empId, companyId);
            employeeViewModel.AllAssetId = assetEntity.Count();

            var benefitEntity = await _benefitRepository.GetEmployeeBenefitsByEmployeeId(empId, companyId);
            employeeViewModel.BenefitId = benefitEntity?.BenefitId;
            employeeViewModel.UANNUmber = empEntity.UANNumber;
            return employeeViewModel;
        }


        /// <summary>
        /// Logic to get display the employee detail  by particular employee view
        /// </summary>
        /// <param name="empId" >employee</param>
        public async Task<ViewEmployee> GetEmployeeByEmployeeId(int empId, int companyId)
        {
            var viewEmployeeModel = new ViewEmployee();
            var empEntity = await _employeesRepository.GetEmployeeByIdView(empId, companyId);
            if (empEntity != null)
            {
                var designation = await _masterRepository.GetDesignationByEmployeeId(empEntity.DesignationId, companyId);
                var department = await _masterRepository.GetDepartmentByEmployeeId(empEntity.DepartmentId, companyId);
                var allemployee = await _employeesRepository.GetAllEmployees(companyId);
                List<int> reportingPersonEmployeeIds = await _employeesRepository.GetReportingPersonEmployeeById(empId, companyId);
                viewEmployeeModel.FirstName = empEntity.FirstName;
                viewEmployeeModel.LastName = empEntity.LastName;
                viewEmployeeModel.FatherName = string.IsNullOrEmpty(empEntity.FatherName) ? string.Empty : empEntity.FatherName;
                viewEmployeeModel.OfficeEmail = empEntity.OfficeEmail;
                viewEmployeeModel.PersonalEmail = string.IsNullOrEmpty(empEntity.PersonalEmail) ? string.Empty : empEntity.PersonalEmail;
                viewEmployeeModel.Designation = designation.DesignationName;
                viewEmployeeModel.Department = department.DepartmentName;
                viewEmployeeModel.UANNUmber = empEntity.UANNumber;
                viewEmployeeModel.SkillName = string.IsNullOrEmpty(empEntity.SkillName) ? string.Empty : empEntity.SkillName;
                viewEmployeeModel.ReportingPerson = await GetemployeeNameByReportingPersionId(reportingPersonEmployeeIds, allemployee);
            }
            return viewEmployeeModel;
        }

        /// <summary>
        /// Logic to get display the employee detail  by particular employee view
        /// </summary>
        /// <param name="empId" >employee</param>
        public async Task<ViewProfile> GetEmployeeProfileByEmployeeId(int empId, int companyId)
        {
            var profileInfoEntity = await _employeesRepository.GetProfileByEmployeeIdview(empId, companyId);
            var relationshipTypeEntitys = await _employeesRepository.GetAllRelationshipType();
            var relationshipTypeEntity = profileInfoEntity == null ? null : relationshipTypeEntitys.FirstOrDefault(x => x.RelationshipId == profileInfoEntity.RelationshipId);

            var viewEmployeeProfileModel = new ViewProfile();
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

        /// <summary>
             /// Logic to get the reporting person name
               /// </summary>
        /// <param name="reportingPersonEmployeeIds" ></param>
        /// <param name="allemployee" ></param>
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

        /// <summary>
        /// Logic to get login the employee detail  by particular employee
        /// </summary>       
        /// <param name="employees"></param>
        public async Task<Employees> GetByUserName(LoginViewModel employees)
        {
            var employeePassword = employees.Password.Trim();
            var password = Common.Common.sha256_hash(employeePassword);
            var data = await _employeesRepository.GetByUserName(employees.UserName, password);
            var employee = new Employees();
            employee.ProfileInfo = new ProfileInfo();
            if (data != null)
            {
                var profile = await _employeesRepository.GetProfileByEmployeeId(data.EmpId, data.CompanyId);
                var image = profile == null ? Common.Constant.DefaultProfileName : profile.ProfileName;

                employee.EmpId = data.EmpId;
                employee.RoleId = (Role)data.RoleId;
                employee.EmployeeUserId = data.UserName;
                employee.UserName = data.FirstName + "    " + data.LastName;
                employee.ProfileInfo.ProfileName = image;
                employee.OfficeEmail = data.OfficeEmail;
                employee.CompanyId = data.CompanyId;
                employee.IsActive = data.IsActive;
                employee.IsOnboarding = data.IsOnboarding;
            }
            return employee;
        }

        /// <summary>
        /// Logic to get the employee list
        /// </summary>             
        public async Task<List<Employees>> GetAllEmployees(int companyId)
        {
            var listOfEmployees = new List<Employees>();
            listOfEmployees = await _employeesRepository.GetAllEmployee(companyId);
            var probationPeriod = await _companyRepository.GetProbationMonth();
            var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
            foreach (var employee in listOfEmployees)
            {
                var profileByEmpId = allEmployeeProfiles.FirstOrDefault(x => x.EmpId == employee.EmpId);
                employee.ProbationDays = profileByEmpId == null ? "" : YetToGetProbationDay(profileByEmpId, probationPeriod);
                employee.ProfileCompletionPercentage = await GetEmployeeProfileRecordsByEmployeeId(employee.EmpId, companyId);
            }
            listOfEmployees.OrderByDescending(x => x.CreatedDate).ToList();

            return listOfEmployees ?? new List<Employees>();
        }

        /// <summary>
        /// Logic to get the employee details records check the persentage
        /// </summary>
        /// <param name="empId"></param>
        public async Task<string> GetEmployeeProfileRecordsByEmployeeId(int empId, int companyId)
        {
            var percentage = 0;
            var profileInfo = await _employeesRepository.GetProfileByEmployeeId(empId, companyId);
            if (profileInfo != null)
            {
                percentage += 20;
            }
            var addressInfo = await _employeesRepository.GetAddressByEmployeeId(empId, companyId);
            if (addressInfo != null)
            {
                percentage += 10;
            }
            var OtherDetailsInfo = await _employeesRepository.GetOtherDetailsByEmployeeId(empId, companyId);
            if (OtherDetailsInfo != null)
            {
                percentage += 10;
            }
            var qualificationInfo = await _employeesRepository.GetQualificationByEmployeeId(empId, companyId);
            if (qualificationInfo != null)
            {
                percentage += 20;
            }
            var experienceInfo = await _employeesRepository.GetExperienceByEmployeeId(empId, companyId);
            if (experienceInfo != null)
            {
                percentage += 20;
            }
            var bankDetailsInfo = await _employeesRepository.GetBankDetailsByEmployeeId(empId, companyId);
            if (bankDetailsInfo != null)
            {
                percentage += 20;
            }
            var completionPercentage = Convert.ToString(percentage);
            return completionPercentage;
        }

        /// <summary>
             /// Logic to get isactive personalEmail count  the employees detail by particular employees
              /// </summary>   
        /// <param name="personalEmail" >employees</param> 
        /// <param name="empId" >employees</param> 
        public async Task<int> GetPersonalEmail(string personalEmail, int empId, int companyId)
        {
            var result = 0;
            var employee = await _employeesRepository.GetEmployeeById(empId, companyId);
            var employeePersonalMailCount = await _employeesRepository.GetPersonalEmail(personalEmail);
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


        /// <summary>
             /// Logic to get create and update the employees 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="sessionEmployeeId"></param>

        public async Task<int> CreateEmployee(Employees employees, int sessionEmployeeId, int companyId)
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
                    try
                    {
                        var employeesEntity = _mapper.Map<EmployeesEntity>(employees);
                        employeesEntity.CompanyId = companyId;
                        var data = await _employeesRepository.CreateEmployee(employeesEntity, companyId);
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
                            await _employeesRepository.CreateReportingPersons(reportingPersonsEntitys, data);
                        }

                        var draftTypeId = (int)EmailDraftType.WelcomeEmployee;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                        var domainName = Convert.ToString(_config.GetSection("ConnectionUrl").GetSection("DomainName").Value);
                        var infoEmailName = Convert.ToString(_config.GetSection(Common.Constant.vphospitalInfoMailId).Value);
                        var allemployee = await _employeesRepository.GetAllEmployees(companyId);
                        List<int> reportingPersonEmployeeIds = await _employeesRepository.GetReportingPersonEmployeeById(data, companyId);
                        var reportingPersion = await GetemployeeNameByReportingPersionId(reportingPersonEmployeeIds, allemployee);
                        //var bodyContent = EmailBodyContent.WelcomeEmployeeEmailBodyContent(employeesEntity, randomPassword, domainName, infoEmailName, reportingPersion, emailDraftContentEntity.DraftBody);
                        //await InsertEmailQueue(employees.OfficeEmail, emailDraftContentEntity, bodyContent);
                        result = data;
                        await InsertEmployeesLog(Common.Constant.CreateEmployeesLog, result, Common.Constant.Add, sessionEmployeeId, companyId);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                  
                }
                else
                {
                    var employeesEntity = await _employeesRepository.GetEmployeeById(employees.EmpId, companyId);

                    var reportingPersonsEmployeeEntity = await _employeesRepository.GetReportingPersonEmployeeById(employees.EmpId, companyId);
                    var reportingPersonsEntities = await _employeesRepository.GetReportingPersonEmployeeId(employees.EmpId, companyId);
                    var employeesChangeLogs = await GetUpdateEmployeeDiffrencetFieldName(employeesEntity, employees, reportingPersonsEntities, companyId);
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
                    employeesEntity.UANNumber = employees.UANNUmber;

                    var strSikillName = employees.SkillNames;
                    if (strSikillName.Count() > 0)
                    {
                        var str = string.Join(",", strSikillName);
                        employeesEntity.SkillName = str.TrimEnd(',');
                    }

                    var data = await _employeesRepository.CreateEmployee(employeesEntity, companyId);
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

        /// <summary>
        /// Logic to get the admin create employee officemail get emails
        /// </summary>
        /// <param name="officeEmail"></param>
        /// <param name="emailDraftContentEntity"></param>
        /// <param name="bodyContent"></param>
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

        /// <summary>
               /// Logic to get employeeschangeLog all propertys
              /// </summary> 
        /// <param name="employeesEntity" ></param>
        /// <param name="employees" ></param>
        /// <param name="reportingPersonsEntity" ></param>
        public async Task<List<EmployeesChangeLog>> GetUpdateEmployeeDiffrencetFieldName(EmployeesEntity employeesEntity, Employees employees, ReportingPersonsEntity reportingPersonsEntity, int companyId) //ReportingPersonsEntity reportingPersonsEntity
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
                    var department = await _employeesRepository.GetAllDepartment(companyId);
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
                    var designation = await _employeesRepository.GetAllDesignation(companyId);
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
                    var reportingPersonsEmployeeEntity = await _employeesRepository.GetReportingPersonEmployeeById(employees.EmpId, employees.CompanyId);
                    var allemployee = await _employeesRepository.GetAllEmployees(companyId);
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

        /// <summary>
        /// Logic to get create, delete and update  the insertemployeeslog
        /// </summary>              
        /// <param name="sectionName" ></param>
        /// <param name="empId" ></param>
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param> 
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



        /// <summary>
           ///Logic to get empId verify the employee details 
           /// </summary>  
        /// <param name="empId" ></param>
        public async Task<bool> employeeIsverified(int empId, int companyId)
        {
            var result = await _employeesRepository.employeeIsverified(empId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get the employees detail get by  particular username
        /// </summary>         
        public async Task<string> GetEmployeeId(int companyId)
        {
            var listCompany = await _companyRepository.GetAllCompanySetting(companyId);
            var employeeCount = await _companyRepository.GetMaxCountOfEmployeesByCompanyId(companyId);

            string? userName;
            userName = listCompany.CompanyCode + (employeeCount).ToString("D3");
            return userName;
        }

        /// <summary>
        /// Logic to get officeEmail count  the employees detail 
        /// </summary>   
        /// <param name="officeEmail" >employees</param>      
        public async Task<int> GetEmployeeEmail(string officeEmail, int companyId)
        {
            var totalEmployeeCount = await _employeesRepository.GetEmployeeEmail(officeEmail, companyId);
            return totalEmployeeCount;
        }

        /// <summary>
        /// Logic to get isactive officeEmail count  the employees detail by particular employees
        /// </summary>   
        /// <param name="officeEmail" >employees</param> 
        public async Task<bool> EmployeeIsActiveCheck(string officeEmail)
        {
            var employeeIsActiveCheck = await _employeesRepository.EmployeeIsActiveCheck(officeEmail);
            return employeeIsActiveCheck;
        }

        /// <summary>
        /// Logic to get skillset list 
        /// </summary> 
        public async Task<List<SkillSet>> GetAllSkills(int companyId)
        {
            var listOfSkill = new List<SkillSet>();
            var listSkills = await _employeesRepository.GetAllSkills(companyId);
            if (listSkills != null)
            {
                listOfSkill = _mapper.Map<List<SkillSet>>(listSkills);
            }

            return listOfSkill;
        }

        /// <summary>
        /// Logic to get  update the skillset 
        /// </summary>
        /// <param name="employees"></param>       
        public async Task<bool> UpdateSkill(Employees employees)
        {
            var result = false;
            var str = String.Join(",", employees.SkillNames);
            var employeesEntity = new EmployeesEntity();
            employeesEntity.SkillName = str;
            employeesEntity.EmpId = employees.EmpId;
            await _employeesRepository.UpdateSkill(employeesEntity);
            return result;
        }

        /// <summary>
        /// Logic to get documenttypes list 
        /// </summary> 
        public async Task<List<DocumentTypes>> GetAllDocumentTypes(int companyId)
        {
            var listOfDocuments = new List<DocumentTypes>();
            var listDocuments = await _employeesRepository.GetAllDocumentTypes(companyId);
            if (listDocuments != null)
            {
                listOfDocuments = _mapper.Map<List<DocumentTypes>>(listDocuments);
            }
            return listOfDocuments;
        }

        /// <summary>
        /// Logic to get reportingperson list 
        /// </summary> 
        public async Task<List<ReportingPerson>> GetAllReportingPerson(int companyId)
        {
            try
            {
                var result = new List<ReportingPerson>();
                var listEmployee = await _employeesRepository.GetAllReportingPersons(companyId);
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logic to get designation list 
        /// </summary> 
        public async Task<List<Designation>> GetAllDesignation(int companyId)
        {
            var listOfDesignation = new List<Designation>();
            var listDesignation = await _employeesRepository.GetAllDesignation(companyId);
            if (listDesignation.Count() > 0)
            {
                listOfDesignation = _mapper.Map<List<Designation>>(listDesignation);
            }
            return listOfDesignation;
        }

        /// <summary>
        /// Logic to get department list 
        /// </summary>
        public async Task<List<Department>> GetAllDepartment(int companyId)
        {
            var listOfDepartment = new List<Department>();
            var listDepartment = await _employeesRepository.GetAllDepartment(companyId);
            if (listDepartment.Count() > 0)
            {
                listOfDepartment = _mapper.Map<List<Department>>(listDepartment);
            }
            return listOfDepartment;
        }

        /// <summary>
           /// Logic to get role list 
        /// </summary>
        public async Task<List<RoleViewModel>> GetAllRoleTable(int companyId)
        {
            var listRoleTable = await _employeesRepository.GetAllRoleTable(companyId);

            var rolePermission = listRoleTable
                .Where(r => !r.IsDeleted && r.CompanyId == companyId)
                .Select(r => new RoleViewModel
                {
                    RoleId = Enum.IsDefined(typeof(Common.Enums.Role), r.RoleId)
                                ? (Common.Enums.Role)r.RoleId
                                : Common.Enums.Role.Employee, // fallback
                    RoleName = r.RoleName,
                    IsActive = r.IsActive
                })
                .ToList();

            return rolePermission;
        }

        /// <summary>
            /// Logic to get the employees details by particular EmpId 
               /// </summary>
        /// <param name="EmpId">employees</param>
        public async Task<Employees> GetEmployeeById(int EmpId, int companyId)
        {
            var employees = new Employees();
            var employeesEntity = await _employeesRepository.GetEmployeeById(EmpId, companyId);
            if (employeesEntity != null)
            {
                employees = _mapper.Map<Employees>(employeesEntity);
            }

            var reportingPersonEmployeeIds = new List<int>();
            reportingPersonEmployeeIds = await _employeesRepository.GetReportingPersonEmployeeById(EmpId, companyId);
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

            var skillnames = await _employeesRepository.GetEmployeeById(EmpId, companyId);

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


        // ChangePassword

        /// <summary>
        /// Logic to get the employees update login password 
        /// </summary>
        /// <param name="loginViewModel"></param>
        public async Task UpdateNewPassword(LoginViewModel loginViewModel)
        {
            if (!string.IsNullOrEmpty(loginViewModel.OfficeEmail))
            {
                var employeeByOfficeEmail = await _employeesRepository.getEmloyeeDetailByOfficeEmail(loginViewModel.OfficeEmail);
                var password = loginViewModel.NewPassword;
                employeeByOfficeEmail.Password = string.IsNullOrEmpty(password) ? string.Empty : Common.Common.sha256_hash(password);
                employeeByOfficeEmail.IsActive = true;
                employeeByOfficeEmail.UpdatedDate = DateTime.Now;
                await _employeesRepository.UpdateNewPassword(employeeByOfficeEmail);
            }
        }

        /// <summary>
        /// Logic to get the employees changepassword 
        /// </summary>
        /// <param name="changePasswordViewModel"></param>
        public async Task<bool> UpdateEmployeeCurrentPassword(ChangePasswordViewModel changePasswordViewModel, int companyId)
        {
            var result = false;
            var getByEmpId = await _employeesRepository.GetEmployeeById(changePasswordViewModel.EmpId, companyId);

            if (getByEmpId != null)
            {
                var password = changePasswordViewModel.NewPassword;
                getByEmpId.Password = string.IsNullOrEmpty(password) ? string.Empty : Common.Common.sha256_hash(password);
                getByEmpId.UpdatedDate = DateTime.Now;
                result = await _employeesRepository.UpdateEmployeeCurrentPassword(getByEmpId);
            }
            var draftTypeId = (int)EmailDraftType.ChangePassword;
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
            var bodyContent = EmailBodyContent.SendEmail_Body_ChangePassword(emailDraftContentEntity.DraftBody);
            await InsertEmailChangePassword(getByEmpId.OfficeEmail, emailDraftContentEntity, bodyContent);
            return result;
        }

        /// <summary>
        /// Logic to get the employees changpassword get emails
        /// </summary>
        /// <param name="officeEmail"></param>
        /// <param name="emailDraftContentEntity"></param>
        /// <param name="bodyContent"></param>
        private async Task InsertEmailChangePassword(string officeEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.ChangePasswordReason;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
               /// Logic to get password the employees detail by particular employees
             /// </summary>   
        /// <param name="empId" ></param> 
        /// <param name="CurrentPassword" ></param> 
        public async Task<bool> GetEmpId(int empId, string CurrentPassword, int companyId)
        {
            var employee = await _employeesRepository.GetEmployeeById(empId, companyId);
            var value = Common.Common.sha256_hash(CurrentPassword);
            if (employee != null)
            {
                if (employee.Password == value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Logic to get changepassword the employees detail by particular employees
        /// </summary>   
        /// <param name="officeEmail" ></param>        
        public async Task ChangePassword(string officeEmail)
        {
            var domainName = Convert.ToString(_config.GetSection("ConnectionUrl").GetSection("DomainName").Value);
            var encriptUserName = Common.Common.Encrypt(officeEmail);
            var draftTypeId = (int)EmailDraftType.ForgotPassword;
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftType(draftTypeId);
            var bodyContent = EmailBodyContent.SendEmail_Body_ForgotPassword(encriptUserName, domainName, emailDraftContentEntity.DraftBody);
            var toEmail = officeEmail;
            var subject = Common.Constant.ResetPassword;
            //var displayName = "Management - Internal";
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();

            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.IsSend = false;
            emailEntity.Reason = Common.Constant.ForgotPassword;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            var result = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            // Common.Common.SendEmail(emailSettingsEntity, body, toEmail, subject, displayName);
        }

        // Qualification

        /// <summary>
        /// Logic to get the qualification list
        /// </summary>
        /// <param name="EmpId">employees</param>
        public async Task<List<Qualification>> GetAllQualification(int EmpId, int companyId)
        {
            // return await _employeesRepository.GetAllQualification(EmpId);
            var listOfQualification = new List<Qualification>();
            var listQualification = await _employeesRepository.GetAllQualification(EmpId, companyId);
            if (listQualification != null)
            {
                listOfQualification = _mapper.Map<List<Qualification>>(listQualification);
            }
            return listOfQualification;
        }

        /// <summary>
        /// Logic to get create and update the qualification 
        /// </summary>
        /// <param name="qualification"></param>
        /// <param name="sessionEmployeeId"></param>       
        public async Task<bool> InsertAndUpdateQualification(Qualification qualification, int sessionEmployeeId, int companyId)
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
                    var qualificationId = await _employeesRepository.InsertAndUpdateQualification(qualificationEntity);
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
                        result = await _employeesRepository.InsertQualificationAttachment(attachmentsEntitys, qualificationId);
                    }
                    await InsertQualificationEmployeesLog(Common.Constant.CreateQualification, qualification.EmpId, Common.Constant.Add, sessionEmployeeId, companyId, null);
                }
                else
                {
                    var qualificationEntity = await _employeesRepository.GetQualificationByQualificationId(qualification.QualificationId, qualification.CompanyId);
                    var attachmentsQualificationEntity = await _employeesRepository.GetQualificationAttachmentsByEmployeeId(qualification.QualificationId);
                    var employeesChangeLogs = await GetUpdateQualificationDiffrencetFieldName(qualificationEntity, qualification, attachmentsQualificationEntity);
                    qualificationEntity.UpdatedBy = sessionEmployeeId;
                    qualificationEntity.UpdatedDate = DateTime.Now;
                    qualificationEntity.Percentage = qualification.Percentage;
                    qualificationEntity.QualificationType = qualification.QualificationType;
                    qualificationEntity.YearOfPassing = qualification.YearOfPassing;
                    qualificationEntity.InstitutionName = qualification.InstitutionName;
                    var qualificationId = await _employeesRepository.InsertAndUpdateQualification(qualificationEntity);
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
                        result = await _employeesRepository.InsertQualificationAttachment(attachmentsEntitys, qualificationId);
                    }
                    await InsertQualificationEmployeesLog(Common.Constant.UpdateQualification, qualification.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get qualificationChangeLog all propertys
        /// </summary> 
        /// <param name="qualificationEntity" ></param>
        /// <param name="qualification" ></param>
        /// <param name="qualificationAttachmentsEntity" ></param>
        public async Task<List<EmployeesChangeLog>> GetUpdateQualificationDiffrencetFieldName(QualificationEntity qualificationEntity, Qualification qualification, QualificationAttachmentsEntity qualificationAttachmentsEntity)
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

                    var qualificationAttachment = await _employeesRepository.GetAllQualificationAttachments(qualification.QualificationId);
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
        /// <summary>
        /// Logic to get create,delete and update  the InsertqualificationLog
        /// </summary>              
        /// <param name="sectionName" ></param>
        /// <param name="empId" ></param>
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param>
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
        /// <summary>
        /// Logic to get  delete the qualification details by particular qualification 
        /// </summary>
        /// <param name="qualification"></param>  
        public async Task DeleteQualification(Qualification qualification)
        {
            var qualificationEntity = await _employeesRepository.GetQualificationByQualificationId(qualification.QualificationId, qualification.CompanyId);
            qualificationEntity.IsDeleted = true;
            await _employeesRepository.DeleteQualification(qualificationEntity);

            var qualificationAttachementEntitys = await _employeesRepository.GetQualificationDocumentAndFilePath(qualification.QualificationId);
            foreach (var item in qualificationAttachementEntitys)
            {
                item.IsDeleted = true;
            }

            await _employeesRepository.DeleteQualificationAttachement(qualificationAttachementEntitys);
        }

        /// <summary>
        /// Logic to get the qualification detail by particular empId
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<QulificationViewModel> GetAllQulificationViewModel(int empId, int companyId)
        {
            var qulificationModel = new List<Qualification>();
            var listOfQualificationEntity = await _employeesRepository.GetAllQulificationViewModel(empId, companyId);
            var qualificationViewModel = new QulificationViewModel();
            qualificationViewModel.EmpId = empId;

            if (listOfQualificationEntity.Count() > 0)
            {
                qulificationModel = _mapper.Map<List<Qualification>>(listOfQualificationEntity);
            }
            qualificationViewModel.Qualifications = qulificationModel;

            return qualificationViewModel;
        }

        /// <summary>
        /// Logic to get the qualification detail by particular empId using for view 
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<QulificationViewModel> GetAllQulificationView(int empId, int companyId)//view model
        {
            var qulificationModel = new List<Qualification>();
            var listOfQualificationEntity = await _employeesRepository.GetAllQulificationView(empId, companyId);
            var qualificationViewModel = new QulificationViewModel();
            qualificationViewModel.EmpId = empId;

            if (listOfQualificationEntity.Count() > 0)
            {
                qulificationModel = _mapper.Map<List<Qualification>>(listOfQualificationEntity);
            }
            qualificationViewModel.Qualifications = qulificationModel;

            return qualificationViewModel;
        }

        // profileInfo

        /// <summary>
           /// Logic to get state list 
        /// </summary>        
        public async Task<List<State>> GetAllStates()
        {
            var listOfState = new List<State>();
            var listState = await _employeesRepository.GetAllStates();
            if (listState != null)
            {
                listOfState = _mapper.Map<List<State>>(listState);
            }
            return listOfState;
        }

        /// <summary>
           /// Logic to get bloodgroup list 
        /// </summary>
        public async Task<List<BloodGroup>> GetAllBloodGroup()
        {
            var listOfBloodGroup = new List<BloodGroup>();
            var listBloodGroup = await _employeesRepository.GetAllBloodGroup();
            if (listBloodGroup != null)
            {
                listOfBloodGroup = _mapper.Map<List<BloodGroup>>(listBloodGroup);
            }
            return listOfBloodGroup;
        }

        /// <summary>
        /// Logic to get relationshiptype list 
        /// </summary>
        public async Task<List<RelationshipType>> GetAllRelationshipType()
        {
            var listOfRelationshipType = new List<RelationshipType>();
            var listRelationshipType = await _employeesRepository.GetAllRelationshipType();
            if (listRelationshipType != null)
            {
                listOfRelationshipType = _mapper.Map<List<RelationshipType>>(listRelationshipType);
            }
            return listOfRelationshipType;
        }

        /// <summary>
        /// Logic to get city list 
        /// </summary>
        public async Task<List<City>> GetAllCities()
        {
            var listOfCity = new List<City>();
            var listOfCities = await _employeesRepository.GetAllCities();
            if (listOfCities.Count() > 0)
            {
                listOfCity = _mapper.Map<List<City>>(listOfCities);
            }
            return listOfCity;
        }

        /// <summary>
        /// Logic to get add and update the address detail  by particular address in stateId
        /// </summary>
        /// <param name="StateId" >address</param>
        public async Task<List<City>> GetByStateId(int StateId)
        {
            var listOfCitys = new List<City>();
            var listOfCitiesed = await _employeesRepository.GetByStateId(StateId);
            if (listOfCitiesed.Count() > 0)
            {
                listOfCitys = _mapper.Map<List<City>>(listOfCitiesed);
            }
            return listOfCitys;
        }

        /// <summary>
        /// Logic to get create and update the profileinfo detail  
        /// </summary>
        /// <param name="profileInfo" ></param>
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<int> AddProfileInfo(ProfileInfo profileInfo, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (profileInfo != null)
            {
                var profileInfoDataEntity = await _employeesRepository.GetProfileByEmployeeId(profileInfo.EmpId, companyId);
                if (profileInfoDataEntity == null)
                {
                    profileInfo.CreatedBy = sessionEmployeeId;
                    profileInfo.CreatedDate = DateTime.Now;
                    profileInfo.DateOfBirth = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfBirth);
                    profileInfo.DateOfJoining = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfJoining);
                    var profileInfoEntity = _mapper.Map<ProfileInfoEntity>(profileInfo);
                    var datas = await _employeesRepository.AddProfileInfo(profileInfoEntity, true);
                    await InsertProfileEmployeesLog(Common.Constant.CreateProfileInfo, result, Common.Constant.Add, sessionEmployeeId, companyId, null);
                    result = profileInfoEntity.EmpId;
                }
                else
                {
                    var employeesChangeLogs = await GetUpdateProfileInfoDiffrencetFieldName(profileInfoDataEntity, profileInfo);
                    profileInfo.UpdatedBy = sessionEmployeeId;
                    profileInfo.UpdatedDate = DateTime.Now;
                    profileInfo.ProfileFilePath = string.IsNullOrEmpty(profileInfo.ProfileFilePath) ? profileInfoDataEntity.ProfileFilePath : profileInfo.ProfileFilePath;
                    profileInfo.ProfileName = string.IsNullOrEmpty(profileInfo.ProfileName) ? profileInfoDataEntity.ProfileName : profileInfo.ProfileName;
                    profileInfo.CreatedBy = profileInfoDataEntity.CreatedBy;
                    profileInfo.CreatedDate = profileInfoDataEntity.CreatedDate;
                    profileInfo.DateOfBirth = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfBirth);
                    profileInfo.DateOfJoining = DateTimeExtensions.ConvertToDatetime(profileInfo.StrDateOfJoining);
                    var mapProfileInfoEntity = _mapper.Map<ProfileInfoEntity>(profileInfo);//data model(destination) view model(source) asign                
                    await _employeesRepository.AddProfileInfo(mapProfileInfoEntity, false);
                    result = profileInfoDataEntity.EmpId;

                    await InsertProfileEmployeesLog(Common.Constant.UpdateProfileInfo, profileInfo.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);

                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get create,delete and update the insertprofileemployeeslog details  
        /// </summary>
        /// <param name="sectionName" ></param>
        /// <param name="empId" ></param> 
        /// <param name="eventName" ></param> 
        /// <param name="sessionEmployeeId" ></param> 
        /// <param name="employeesChangeLogs" ></param> 
        public async Task<bool> InsertProfileEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
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

        /// <summary>
        /// Logic to get profilechangeLog all propertys
        /// </summary> 
        /// <param name="profileInfoEntity" ></param>
        /// <param name="profileInfo" ></param> 
        public async Task<List<EmployeesChangeLog>> GetUpdateProfileInfoDiffrencetFieldName(ProfileInfoEntity profileInfoEntity, ProfileInfo profileInfo)
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

        /// <summary>
        /// Logic to get the ProfileInfo detail by particular empId
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<ProfileInfo> GetProfileByEmployeeId(int empId, int companyId)
        {
            var profileinfos = new ProfileInfo();
            var profileinfo = await _employeesRepository.GetProfileByEmployeeId(empId, companyId);

            if (profileinfo != null)
            {
                profileinfos = _mapper.Map<ProfileInfo>(profileinfo);
            }
            profileinfos.ProfileViewImage = "/ProfileImages/" + profileinfos.ProfileName;
            profileinfos.ProfileActionName = string.IsNullOrEmpty(profileinfos.ProfileFilePath) ? "create" : "update";
            return profileinfos;
        }

        // AddressInfo

        /// <summary>
        /// Logic to get create and update the addressInfo
        /// </summary> 
        /// <param name="addressInfo" ></param>
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<int> AddAddressInfo(AddressInfo addressInfo, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (addressInfo != null)
            {
                if (addressInfo.AddressId == 0)
                {
                    addressInfo.CreatedBy = sessionEmployeeId;
                    addressInfo.CreatedDate = DateTime.Now;
                    var addressInfoEntity = _mapper.Map<AddressInfoEntity>(addressInfo);
                    var add = await _employeesRepository.AddAddressInfo(addressInfoEntity);
                    await InsertAddressInfoEmployeesLog(Common.Constant.CreateAddressInfo, result, Common.Constant.Add, sessionEmployeeId, companyId, null);
                    result = addressInfoEntity.EmpId;
                }
                else
                {
                    var addressInfoEntity = await _employeesRepository.GetAddressByEmployeeId(addressInfo.EmpId, companyId);
                    var employeesChangeLogs = await GetUpdateAddressInfoDiffrencetFieldName(addressInfoEntity, addressInfo);
                    addressInfo.UpdatedBy = sessionEmployeeId;
                    addressInfo.UpdatedDate = DateTime.Now;
                    addressInfo.CreatedBy = addressInfoEntity.CreatedBy;
                    addressInfo.CreatedDate = addressInfoEntity.CreatedDate;
                    var mapAddressInfoEntity = _mapper.Map<AddressInfoEntity>(addressInfo);
                    var data = await _employeesRepository.AddAddressInfo(mapAddressInfoEntity);
                    await InsertAddressInfoEmployeesLog(Common.Constant.UpdateAddressInfo, addressInfo.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                    result = addressInfoEntity.EmpId;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get create,delete and update the insertaddressemployeeslog details  
        /// </summary> 
        /// <param name="sectionName" ></param>
        /// <param name="empId" ></param> 
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param>
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

        /// <summary>
            /// Logic to get addresschangeLog all propertys
             /// </summary> 
        /// <param name="addressInfoEntity" ></param>
        /// <param name="addressInfo" ></param> 

        public async Task<List<EmployeesChangeLog>> GetUpdateAddressInfoDiffrencetFieldName(AddressInfoEntity addressInfoEntity, AddressInfo addressInfo)
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
                    var addressCity = await _employeesRepository.GetAllCities();
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
                    var addressState = await _employeesRepository.GetAllStates();
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
                    var addressCountry = await _employeesRepository.GetAllCountry();
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
                    var addressSecondaryCity = await _employeesRepository.GetAllCities();
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
                    var secondaryState = await _employeesRepository.GetAllStates();
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
                    var addressCountry = await _employeesRepository.GetAllCountry();
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

        /// <summary>
        /// Logic to get the country list 
        /// </summary>  
        public async Task<List<Country>> GetAllCountry()
        {
            var listOfCountry = new List<Country>();
            var listCountry = await _employeesRepository.GetAllCountry();
            if (listCountry != null)
            {
                listOfCountry = _mapper.Map<List<Country>>(listCountry);
            }
            return listOfCountry;
        }

        /// <summary>
        /// Logic to get add and update the state detail by particular address in countryId
        /// </summary>
        /// <param name="countryId" >state</param>
        public async Task<List<State>> GetByCountryId(int countryId)
        {
            var listOfStates = new List<State>();
            var listOfState = await _employeesRepository.GetByCountryId(countryId);
            if (listOfState.Count() > 0)
            {
                listOfStates = _mapper.Map<List<State>>(listOfState);
            }
            return listOfStates;
        }

        /// <summary>
        /// Logic to get display the addressinfo detail  by particular addressinfo view
        /// </summary>
        /// <param name="empId" >employee</param>
        public async Task<ViewAddressInfo> GetEmployeeAddressByEmployeeId(int empId, int companyId)
        {
            var viewAddressInfoModel = new ViewAddressInfo();
            var addressinfo = await _employeesRepository.GetAddressByEmployeeId(empId, companyId);
            if (addressinfo != null)
            {
                viewAddressInfoModel.Address1 = addressinfo.Address1;
                viewAddressInfoModel.Address2 = addressinfo.Address2;
                viewAddressInfoModel.CountryName = await _employeesRepository.GetCountryNameByCountryId(addressinfo.CountryId);
                viewAddressInfoModel.StateName = await _employeesRepository.GetStateNameByStateId(addressinfo.StateId);
                viewAddressInfoModel.CityName = await _employeesRepository.GetCityNameByCityId(addressinfo.CityId);
                viewAddressInfoModel.Pincode = addressinfo.Pincode;
                viewAddressInfoModel.SecondaryAddress1 = string.IsNullOrEmpty(addressinfo.SecondaryAddress1) ? string.Empty : addressinfo.SecondaryAddress1;
                viewAddressInfoModel.SecondaryAddress2 = string.IsNullOrEmpty(addressinfo.SecondaryAddress2) ? string.Empty : addressinfo.SecondaryAddress2;
                viewAddressInfoModel.SecondaryCountryName = await _employeesRepository.GetCountryNameBySecondaryCountryId(addressinfo.SecondaryCountryId);
                viewAddressInfoModel.SecondaryStateName = await _employeesRepository.GetStateNameBySecondaryStateId(addressinfo.SecondaryStateId);
                viewAddressInfoModel.SecondaryCityName = await _employeesRepository.GetCityNameBySecondaryCityId(addressinfo.SecondaryCityId);
                viewAddressInfoModel.SecondaryPincode = addressinfo.SecondaryPincode;

            }
            return viewAddressInfoModel;
        }

        /// <summary>
        /// Logic to get the AddressInfo detail by particular empId
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<AddressInfo> GetAddressByEmployeeId(int empId, int companyId)
        {
            var addressinfos = new AddressInfo();
            var addressinfo = await _employeesRepository.GetAddressByEmployeeId(empId, companyId);
            if (addressinfo != null)
            {
                addressinfos = _mapper.Map<AddressInfo>(addressinfo);
            }
            return addressinfos;
        }

        //BankDetails

        /// <summary>
        ///  Logic to get create and update the bankdetails
        /// </summary> 
        /// <param name="bankDetails" ></param>
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<int> AddBankDetails(BankDetails bankDetails, int sessionEmployeeId, int companyId)
        {
            if (bankDetails == null) return 0;

            bankDetails.UpdatedBy = sessionEmployeeId;
            bankDetails.UpdatedDate = DateTime.Now;

            if (bankDetails.BankId == 0)
            {
                bankDetails.CreatedBy = sessionEmployeeId;
                bankDetails.CreatedDate = DateTime.Now;
                var bankDetailsEntity = _mapper.Map<BankDetailsEntity>(bankDetails);
                await _employeesRepository.AddBankDetails(bankDetailsEntity);
                await InsertBankDetailsEmployeesLog(Common.Constant.CreateBankDetails, 0, Common.Constant.Add, sessionEmployeeId, companyId, null);
                return bankDetailsEntity.EmpId;
            }
            else
            {
                var existingBankDetails = await _employeesRepository.GetBankDetailsByEmployeeId(bankDetails.EmpId, bankDetails.CompanyId);
                var employeesChangeLogs = await GetUpdateBankDetailsDiffrencetFieldName(existingBankDetails, bankDetails);

                bankDetails.CreatedDate = existingBankDetails.CreatedDate;
                bankDetails.CreatedBy = existingBankDetails.CreatedBy;

                var bankDetailsEntity = _mapper.Map<BankDetailsEntity>(bankDetails);
                await _employeesRepository.AddBankDetails(bankDetailsEntity);
                await InsertBankDetailsEmployeesLog(Common.Constant.UpdateBankDetails, bankDetails.EmpId, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);

                return bankDetails.EmpId;
            }
        }


        /// <summary>
        /// Logic to get create,delete and update the insertbankdetails employeeslog details  
        /// </summary> 
        /// <param name="sectionName" ></param>
        /// <param name="empId" ></param> 
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param>
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

        /// <summary>
        /// Logic to get bankdetails changeLog all propertys
        /// </summary> 
        /// <param name="bankDetailsEntity" ></param>
        /// <param name="bankDetails" ></param> 
        public async Task<List<EmployeesChangeLog>> GetUpdateBankDetailsDiffrencetFieldName(BankDetailsEntity bankDetailsEntity, BankDetails bankDetails)
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

        /// <summary>
        /// Logic to get the bankdetails detail by particular empId
        /// </summary>   
        /// <param name="empId" >employees</param> 
        public async Task<BankDetails> GetBankDetailsByEmployeeId(int empId, int companyId)
        {
            var bankDetails = new BankDetails();
            var addbankDetails = await _employeesRepository.GetBankDetailsByEmployeeId(empId, companyId);
            var isVerified = await _employeesRepository.GetEmployeeById(empId, companyId);
            if (addbankDetails != null)
            {
                bankDetails = _mapper.Map<BankDetails>(addbankDetails);
            }
            if (isVerified != null)
            {
                bankDetails.IsVerified = isVerified.IsVerified;
            }

            return bankDetails;
        }

        //otherDetails

        /// <summary>
        /// Logic to get  delete the otherDetails details by particular otherDetails 
        /// </summary>
        /// <param name="otherDetails"></param> 
        public async Task DeleteOtherDetails(OtherDetails otherDetails)
        {
            var otherDetailsEntity = await _employeesRepository.GetotherDetailsBydetailId(otherDetails.DetailId, otherDetails.CompanyId);
            otherDetailsEntity.IsDeleted = true;
            await _employeesRepository.DeleteOtherDetails(otherDetailsEntity);

            var otherDetailsAttachementEntitys = await _employeesRepository.GetOtherDetailsDocumentAndFilePath(otherDetails.DetailId);
            foreach (var item in otherDetailsAttachementEntitys)
            {
                item.IsDeleted = true;
            }
            await _employeesRepository.DeleteOtherDetailsAttachement(otherDetailsAttachementEntitys);
        }

        /// <summary>
        /// Logic to get create and update the otherDetails detail
        /// </summary> 
        /// <param name="otherDetails" >employees</param>
        /// <param name="sessionEmployeeId" >employees</param>
        public async Task<bool> InsertAndUpdateOtherDetails(OtherDetails otherDetails, int sessionEmployeeId)
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
                    var detailId = await _employeesRepository.InsertAndUpdateOtherDetails(otherDetailsEntity);
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
                        result = await _employeesRepository.InsertOtherDetailsAttachment(attachmentsEntitys, detailId);

                    }
                    await InsertOtherDetailsEmployeesLog(Common.Constant.CreateOtherDetails, otherDetails.EmpId, Common.Constant.Add, sessionEmployeeId, otherDetails.CompanyId, null);
                }
                else
                {
                    var otherDetailsEntity = await _employeesRepository.GetotherDetailsBydetailId(otherDetails.DetailId, otherDetails.CompanyId);
                    var otherDetailsAttachmentsEntity = await _employeesRepository.GetOtherDetailsAttachmentsByEmployeeId(otherDetails.DetailId);
                    var employeesChangeLogs = await GetUpdateOtherDetailsDiffrencetFieldName(otherDetailsEntity, otherDetails, otherDetailsAttachmentsEntity);
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
                    var detailId = await _employeesRepository.InsertAndUpdateOtherDetails(otherDetailsEntity);
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
                        result = await _employeesRepository.InsertOtherDetailsAttachment(attachmentsEntitys, detailId);
                    }
                    await InsertOtherDetailsEmployeesLog(Common.Constant.UpdateOtherDetails, otherDetails.EmpId, Common.Constant.Update, sessionEmployeeId, otherDetails.CompanyId, employeesChangeLogs);
                }
            }

            return result;
        }

        /// <summary>
             /// Logic to get the otherdetails detail by particular empId
             /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<OtherDetailsViewModel> GetAllOtherDetailsViewModel(int empId, int companyId)
        {
            var otherDetails = new List<OtherDetails>();
            var listOfOtherDetailsEntity = await _employeesRepository.GetAllOtherDetailsViewModel(empId, companyId);
            var otherDetailsViewModel = new OtherDetailsViewModel();

            var documentTypes = await GetAllDocumentTypes(companyId);

            otherDetailsViewModel.EmpId = empId;
            if (listOfOtherDetailsEntity.Count() > 0)
            {
                otherDetails = _mapper.Map<List<OtherDetails>>(listOfOtherDetailsEntity);
            }
            foreach (var item in otherDetails)
            {
                var docTypeEntity = documentTypes.FirstOrDefault(x => x.DocumentTypeId == item.DocumentTypeId);
                item.DocumentTypeName = docTypeEntity != null ? docTypeEntity.DocumentName : string.Empty;
            }
            otherDetailsViewModel.OtherDetails = otherDetails;
            return otherDetailsViewModel;
        }

        /// <summary>
        /// Logic to get the otherdetails detail by particular empId using for view 
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<OtherDetailsViewModel> GetAllOtherDetailsView(int empId, int companyId)//view model
        {
            var otherDetails = new List<OtherDetails>();
            var listOfOtherDetailsEntity = await _employeesRepository.GetAllOtherDetailsView(empId, companyId);
            var otherDetailsViewModel = new OtherDetailsViewModel();

            var documentTypes = await GetAllDocumentTypes(companyId);

            otherDetailsViewModel.EmpId = empId;
            if (listOfOtherDetailsEntity.Count() > 0)
            {
                otherDetails = _mapper.Map<List<OtherDetails>>(listOfOtherDetailsEntity);
            }
            foreach (var item in otherDetails)
            {
                var docTypeEntity = documentTypes.FirstOrDefault(x => x.DocumentTypeId == item.DocumentTypeId);
                item.DocumentTypeName = docTypeEntity != null ? docTypeEntity.DocumentName : string.Empty;
            }
            otherDetailsViewModel.OtherDetails = otherDetails;
            return otherDetailsViewModel;
        }

        /// <summary>
        /// Logic to get create,delete and update  the InsertOtherDetailsLog
        /// </summary>              
        /// <param name="assetName" ></param>
        /// <param name="empId" ></param>
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param>      
        public async Task<bool> InsertOtherDetailsEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
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

        /// <summary>
        /// Logic to get OtherDetailsChangeLog all propertys
        /// </summary> 
        /// <param name="otherDetailsEntity" ></param>
        /// <param name="otherDetails" ></param>
        /// <param name="otherDetailsAttachmentsEntity" ></param>
        public async Task<List<EmployeesChangeLog>> GetUpdateOtherDetailsDiffrencetFieldName(OtherDetailsEntity otherDetailsEntity, OtherDetails otherDetails, OtherDetailsAttachmentsEntity otherDetailsAttachmentsEntity)
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
                    var documentType = await _employeesRepository.GetAllDocumentTypes(otherDetails.CompanyId);
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

                    var otherDetailsAttachment = await _employeesRepository.GetAllOtherDetailsAttachment(otherDetails.DetailId);
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

        /// <summary>
        ///Logic to get detailId based otherdetailsdocumentfilepath
        /// </summary>  
        /// <param name="detailId" ></param>
        public async Task<List<OtherDetailsDocumentFilePath>> GetOtherDetailsDocumentAndFilePath(int detailId)
        {
            var otherDetailsDocumentFilePaths = new List<OtherDetailsDocumentFilePath>();
            var docNmaesAndFilePath = await _employeesRepository.GetOtherDetailsDocumentAndFilePath(detailId);
            foreach (var item in docNmaesAndFilePath)
            {
                var otherDetailsDocumentFilePath = new OtherDetailsDocumentFilePath();
                otherDetailsDocumentFilePath.EmpId = item.EmpId;
                otherDetailsDocumentFilePath.Document = item.Document;
                otherDetailsDocumentFilePath.DocumentName = item.DocumentName;
                otherDetailsDocumentFilePaths.Add(otherDetailsDocumentFilePath);
            }
            return otherDetailsDocumentFilePaths;
        }

        /// <summary>
        ///Logic to get detailId based otherdetailsdocumentname
        /// </summary>  
        /// <param name="detailId" ></param>
        public async Task<List<string>> GetDocumentNameByDetailId(int detailId)
        {
            var otherDetailsDocuments = await _employeesRepository.GetDocumentNameByDetailId(detailId);
            return otherDetailsDocuments;
        }

        // experience

        /// <summary>
        /// Logic to get the experience list
        /// </summary>
        /// <param name="EmpId">employees</param>
        public async Task<List<Experience>> GetAllExperience(int EmpId, int companyId)
        {
            var listOfExperience = new List<Experience>();
            var listExperience = await _employeesRepository.GetAllExperience(EmpId, companyId);
            if (listExperience != null)
            {
                listOfExperience = _mapper.Map<List<Experience>>(listExperience);
            }
            return listOfExperience;
        }

        /// <summary>
        /// Logic to get the experience detail by particular empId
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<ExperienceViewModel> GetAllExperienceViewModel(int empId, int companyId)
        {
            var experienceModel = new List<Experience>();
            var listOfExperienceEntity = await _employeesRepository.GetAllExperienceViewModel(empId, companyId);
            var experienceViewModel = new ExperienceViewModel();
            experienceViewModel.EmpId = empId;
            if (listOfExperienceEntity.Count() > 0)
            {
                experienceModel = _mapper.Map<List<Experience>>(listOfExperienceEntity);
            }
            experienceViewModel.Experiences = experienceModel;
            return experienceViewModel;
        }

        /// <summary>
        /// Logic to get the experience detail by particular empId using for view 
        /// </summary> 
        /// <param name="empId" >employees</param>
        public async Task<ExperienceViewModel> GetAllExperienceView(int empId, int companyId)
        {
            var experienceModel = new List<Experience>();
            var listOfExperienceEntity = await _employeesRepository.GetAllExperienceView(empId, companyId);
            var experienceViewModel = new ExperienceViewModel();
            experienceViewModel.EmpId = empId;
            if (listOfExperienceEntity.Count() > 0)
            {
                experienceModel = _mapper.Map<List<Experience>>(listOfExperienceEntity);
            }
            experienceViewModel.Experiences = experienceModel;
            return experienceViewModel;
        }

        /// <summary>
        /// Logic to get create and update the experience details
        /// </summary>
        /// <param name="experience"></param> 
        /// <param name="sessionEmployeeId"></param> 
        public async Task<bool> InsertAndUpdateExperience(Experience experience, int sessionEmployeeId)
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
                    var experienceId = await _employeesRepository.InsertAndUpdateExperience(experienceEntity);
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
                        result = await _employeesRepository.InsertExperienceAttachment(attachmentsEntitys, experienceId);
                    }
                    await InsertExperienceEmployeesLog(Common.Constant.CreateExperience, experience.EmpId, Common.Constant.Add, sessionEmployeeId, experience.CompanyId, null);
                }
                else
                {
                    var experienceEntity = await _employeesRepository.GetExperienceByExperienceId(experience.ExperienceId, experience.CompanyId);
                    var ExperienceAttachmentsEntity = await _employeesRepository.GetExperienceAttachmentsByEmployeeId(experience.ExperienceId);
                    var employeesChangeLogs = await GetUpdateExperienceDiffrencetFieldName(experienceEntity, experience, ExperienceAttachmentsEntity);
                    experienceEntity.UpdatedBy = sessionEmployeeId;
                    experienceEntity.UpdatedDate = DateTime.Now;
                    experienceEntity.PreviousCompany = experience.PreviousCompany;
                    experienceEntity.DateOfJoining = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfJoining);
                    experienceEntity.DateOfRelieving = DateTimeExtensions.ConvertToDatetime(experience.StrDateOfRelieving);
                    //experienceEntity.Document = string.IsNullOrEmpty(experience.Document) ? experienceEntity.Document : experience.Document;
                    //experienceEntity.ExperienceName = string.IsNullOrEmpty(experience.ExperienceName) ? experienceEntity.ExperienceName : experience.ExperienceName;
                    var experienceId = await _employeesRepository.InsertAndUpdateExperience(experienceEntity);
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
                        result = await _employeesRepository.InsertExperienceAttachment(attachmentsEntitys, experienceId);
                    }
                    await InsertExperienceEmployeesLog(Common.Constant.UpdateExperience, experience.EmpId, Common.Constant.Update, sessionEmployeeId, experience.CompanyId, employeesChangeLogs);

                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get create,delete and update  the InsertExperienceLog
        /// </summary>              
        /// <param name="assetName" ></param>
        /// <param name="empId" ></param>
        /// <param name="eventName" ></param>
        /// <param name="sessionEmployeeId" ></param>
        /// <param name="employeesChangeLogs" ></param>
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
            await _auditLogRepository.CreateEmployeeAuditLog(employeesLogEntitys, companyId);
            return true;
        }

        /// <summary>
        /// Logic to get ExperienceChangeLog all propertys
        /// </summary> 
        /// <param name="experienceEntity" ></param>
        /// <param name="experience" ></param>
        /// <param name="experienceAttachmentsEntity" ></param>
        public async Task<List<EmployeesChangeLog>> GetUpdateExperienceDiffrencetFieldName(ExperienceEntity experienceEntity, Experience experience, ExperienceAttachmentsEntity experienceAttachmentsEntity)
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
                    var experienceAttachments = await _employeesRepository.GetAllExperienceAttachment(experience.ExperienceId);
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


        /// <summary>
        /// Logic to get delete the experience details by particular experience 
        /// </summary>
        /// <param name="experience"></param> 
        /// <param name="sessionEmployeeId"></param>
        public async Task DeleteExperience(Experience experience, int sessionEmployeeId)
        {
            var experienceEntity = await _employeesRepository.GetExperienceByExperienceId(experience.ExperienceId, experience.CompanyId);
            experienceEntity.IsDeleted = true;
            await _employeesRepository.DeleteExperience(experienceEntity);

            var experienceAttachementEntitys = await _employeesRepository.GetExperienceDocumentAndFilePath(experience.ExperienceId);
            foreach (var item in experienceAttachementEntitys)
            {
                item.IsDeleted = true;
            }
            await _employeesRepository.DeleteExperienceAttachement(experienceAttachementEntitys);
        }

        /// <summary>
        ///Logic to get experienceId based experiencename   
        /// </summary>  
        /// <param name="experienceId" ></param>
        public async Task<List<string>> GetDocumentNameByExperienceId(int experienceId)
        {
            var qualificationDocuments = await _employeesRepository.GetDocumentNameByExperienceId(experienceId);
            return qualificationDocuments;
        }

        /// <summary>
        ///Logic to get experienceId based experiencedocumentfilepath
        /// </summary>  
        /// <param name="experienceId" ></param>
        public async Task<List<ExperienceDocumentFilePath>> GetExperienceDocumentAndFilePath(int experienceId)
        {
            var experienceDocumentFilePaths = new List<ExperienceDocumentFilePath>();
            var docNmaesAndFilePath = await _employeesRepository.GetExperienceDocumentAndFilePath(experienceId);
            foreach (var item in docNmaesAndFilePath)
            {
                var experienceDocumentFilePath = new ExperienceDocumentFilePath();
                experienceDocumentFilePath.Document = item.Document;
                experienceDocumentFilePath.ExperienceName = item.ExperienceName;
                experienceDocumentFilePath.EmpId = item.EmpId;
                experienceDocumentFilePaths.Add(experienceDocumentFilePath);
            }
            return experienceDocumentFilePaths;
        }

        /// <summary>
        /// Logic to get the casualandsick leavecalculation by particular employees
        /// </summary>   
        /// <param name="currentDate" ></param> 
        /// <param name="firstDateOfCurrentYear" ></param>
        /// <param name="lastDateOfCurrentYear" ></param>
        /// <param name="casualLeave" ></param>
        /// <param name="sickLeave" ></param>
        /// <param name="probationDate" ></param>
        public static void CasualAndSickLeaveCalculation(DateTime currentDate, ref DateTime firstDateOfCurrentYear, DateTime lastDateOfCurrentYear, out decimal casualLeave, out decimal sickLeave, DateTime probationDate)
        {
            var monthCountAfterProbation = 0.0m;
            if (probationDate.Year == currentDate.Year)
            {
                var date = probationDate.Day;
                if (date <= 10)
                {
                    firstDateOfCurrentYear = Convert.ToDateTime(probationDate.Month + "/" + "01/" + currentDate.Year);
                    //firstDateOfCurrentYear = Convert.ToDateTime("01/" + probationDate.Month + "/" + currentDate.Year);
                }
                else
                {
                    var probationDateAddOneMonth = probationDate.AddMonths(1);
                    firstDateOfCurrentYear = Convert.ToDateTime(probationDateAddOneMonth.Month + "/" + "01/" + probationDateAddOneMonth.Year);
                    //firstDateOfCurrentYear = Convert.ToDateTime("01/" + probationDateAddOneMonth.Month + "/" + probationDateAddOneMonth.Year);
                }

                for (int i = firstDateOfCurrentYear.Month; i <= lastDateOfCurrentYear.Month; i++)
                {
                    monthCountAfterProbation += 1;
                }

                if (monthCountAfterProbation == 1 || monthCountAfterProbation == 3 || monthCountAfterProbation == 5 || monthCountAfterProbation == 7 || monthCountAfterProbation == 9 || monthCountAfterProbation == 11)
                {
                    casualLeave = Convert.ToDecimal((monthCountAfterProbation / 2) + 0.5m);
                    sickLeave = Convert.ToDecimal((monthCountAfterProbation / 2) - 0.5m);
                }
                else
                {
                    casualLeave = Convert.ToDecimal(monthCountAfterProbation / 2);
                    sickLeave = Convert.ToDecimal(monthCountAfterProbation / 2);
                }
            }
            else
            {
                casualLeave = Convert.ToDecimal(6);
                sickLeave = Convert.ToDecimal(6);
            }
        }

        //Probation

        /// <summary>
        /// Logic to get the employees acceptprobation period check 
        /// </summary>
        /// <param name="employees"></param>
        /// <param name="sessionEmployeeId"></param>
        public async Task<bool> AcceptProbation(Employees employees, int sessionEmployeeId, int companyId)
        {
            var result = await _employeesRepository.AcceptProbation(employees, sessionEmployeeId, companyId);
            var employeeEntity = await _employeesRepository.GetEmployeeByIdForLeave(employees.EmpId, companyId);
            Common.Common.WriteServerErrorLog("Probation confirmation employees " + employees.EmpId);
            var designation = await _masterRepository.GetDesignationByEmployeeId(employeeEntity.DesignationId, companyId);
            var designationName = string.Empty;
            if (designation != null)
            {
                designationName = designation.DesignationName;
            }
            var department = await _masterRepository.GetDepartmentByEmployeeId(employeeEntity.DepartmentId, companyId);
            var departmentName = string.Empty;
            if (department != null)
            {
                departmentName = department.DepartmentName;
            }
            var names = await _employeesRepository.GetEmployeeByname(sessionEmployeeId, companyId);
            var acceptedBy = names.UserName;
            Common.Common.WriteServerErrorLog("Probation confirmation department " + departmentName);
            Common.Common.WriteServerErrorLog("Probation confirmation designation " + designationName);
            var probationPeriod = await _companyRepository.GetProbationMonth();
            var draftTypeId = (int)EmailDraftType.AcceptProbation;
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
            var body = EmailBodyContent.SendEmail_Body_ProbationConfirmation(employeeEntity, designationName, departmentName, probationPeriod, acceptedBy, emailDraftContentEntity.DraftBody);
            var ccEmails = new List<string>();
            var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(employees.EmpId, companyId);
            foreach (var item in reportingPersonEmployeeIds)
            {
                var email = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                ccEmails.Add(email);
            }
            var toEmail = employeeEntity.OfficeEmail;
            var subject = Common.Constant.ProbationConfirmation;
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = body;
            emailEntity.IsSend = false;
            emailEntity.Reason = Common.Constant.AcceptProbationReason;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.CompanyId = emailSettingsEntity.CompanyId;
            emailEntity.CreatedDate = DateTime.Now;
            var approvedemail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
            // Common.Common.SendEmailForAcceptProbation(_config, body,toEmail, ccEmails, subject, displayName);

            // Leave Update
            await LeaveCalculation(employeeEntity, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get the employees probation period check 
        /// </summary>
        /// <param name="profileInfoEntity"></param>
        /// <param name="probationPeriod"></param>
        public string YetToGetProbationDay(ProfileInfoEntity profileInfoEntity, int probationPeriod)
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

        /// <summary>
        /// Logic to get the leavecalculation by particular employees
        /// </summary>   
        /// <param name="item" >employeesentity</param> 
        public async Task<bool> LeaveCalculation(EmployeesEntity item, int companyId)
        {
            var profileInfo = await _employeesRepository.GetProfileByEmployeeId(item.EmpId, companyId);
            var result = false;
            if (profileInfo != null)
            {
                var joinDate = Convert.ToDateTime(profileInfo.DateOfJoining);
                var currentDate = DateTime.Now;

                var firstDateOfCurrentYear = Convert.ToDateTime("01/01/" + currentDate.Year);
                var lastDateOfCurrentYear = Convert.ToDateTime("12/31/" + currentDate.Year);/* Convert.ToDateTime("31/12/" + currentDate.Year);*/
                // Casual Leave and Sick Leave
                decimal casualLeave = 0.0m;
                decimal sickLeave = 0.0m;

                var probationDate = Convert.ToDateTime(item.ProbationDate);
                CasualAndSickLeaveCalculation(currentDate, ref firstDateOfCurrentYear, lastDateOfCurrentYear, out casualLeave, out sickLeave, probationDate);

                // Earned Leave
                decimal earnedLeave = EarnedLeaveCalculation(joinDate, currentDate);

                var allLeaveDetailsByEmpId = await _leaveRepository.GetAllLeaveDetails(item.EmpId, companyId);
                if (allLeaveDetailsByEmpId == null)
                {
                    var allLeaveDetailsEntity = new AllLeaveDetailsEntity();
                    allLeaveDetailsEntity.LeaveYear = DateTime.Now.Year;
                    allLeaveDetailsEntity.CasualLeaveCount = casualLeave;
                    allLeaveDetailsEntity.SickLeaveCount = sickLeave;
                    allLeaveDetailsEntity.EarnedLeaveCount = earnedLeave;
                    allLeaveDetailsEntity.EmpId = item.EmpId;
                    allLeaveDetailsEntity.CompanyId = companyId;
                    result = await _leaveRepository.InsertAllLeaveDetailsByEmpId(allLeaveDetailsEntity);
                }
                else
                {
                    allLeaveDetailsByEmpId.CasualLeaveCount = casualLeave;
                    allLeaveDetailsByEmpId.SickLeaveCount = sickLeave;
                    allLeaveDetailsByEmpId.EarnedLeaveCount = earnedLeave;
                    allLeaveDetailsByEmpId.CompanyId = companyId;
                    result = await _leaveRepository.InsertAllLeaveDetailsByEmpId(allLeaveDetailsByEmpId);
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get the earnedleavecalculation by particular employees
        /// </summary>   
        /// <param name="joinDate" ></param> 
        /// <param name="currentDate" ></param>        
        public decimal EarnedLeaveCalculation(DateTime joinDate, DateTime currentDate)
        {
            decimal earnedLeave = 0;
            var day = GetDaysInYear(joinDate);
            var isEarnedLeave = MonthDifference(currentDate, joinDate) > 12 ? true : false;

            if (isEarnedLeave)
            {
                var earnedDate = joinDate.AddYears(1);
                var earnedDay = earnedDate.Day;
                if (earnedDay <= 10)
                {
                    earnedDate = Convert.ToDateTime(earnedDate.Month + "/" + "01/" + earnedDate.Year);
                    // earnedDate = Convert.ToDateTime("01/"+ earnedDate.Month + "/" + earnedDate.Year);
                }
                else
                {
                    var earnedDateAddOneMonth = earnedDate.AddMonths(1);
                    earnedDate = Convert.ToDateTime(earnedDateAddOneMonth.Month + "/" + "01/" + earnedDateAddOneMonth.Year);
                    //earnedDate = Convert.ToDateTime("01/" + earnedDateAddOneMonth.Month + "/" + earnedDateAddOneMonth.Year);
                }
                var diffEarnedDate = MonthDifference(currentDate, earnedDate);
                if (diffEarnedDate > 0)
                {
                    earnedLeave = Convert.ToDecimal(diffEarnedDate);
                }
            }
            else
            {
                earnedLeave = 0;
            }

            return earnedLeave;
        }

        /// <summary>
        /// Logic to get the calculated the year
        /// </summary>   
        /// <param name="date" ></param>        
        static int GetDaysInYear(DateTime date)
        {
            if (date.Equals(DateTime.MinValue))
            {
                return -1;
            }

            DateTime thisYear = new DateTime(date.Year, 1, 1);
            DateTime nextYear = new DateTime(date.Year + 1, 1, 1);

            return (nextYear - thisYear).Days;
        }

        /// <summary>
        /// Logic to get the calculated the month
        /// </summary>   
        /// <param name="lValue" ></param>  
        /// <param name="rValue" ></param>  
        static int MonthDifference(DateTime lValue, DateTime rValue)
        {
            return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year);
        }

        /// <summary>
        /// Logic to get reject the employees detail by particular employees
              /// </summary>
        /// <param name="employees" ></param>
        public async Task<bool> GetRejectEmployees(Employees employees)
        {
            var result = await _employeesRepository.GetRejectEmployees(employees);
            return result;
        }

        /// <summary>
        ///Logic to get qualificationId based QualificationName
        /// </summary>  
        /// <param name="qualificationId" ></param>
        public async Task<List<string>> GetDocumentNameByQualificationId(int qualificationId)
        {
            var qualificationDocuments = await _employeesRepository.GetDocumentNameByQualificationId(qualificationId);
            return qualificationDocuments;
        }

        /// <summary>
        ///Logic to get qualificationId based qualificationdocumentfilepath
        /// </summary>  
        /// <param name="qualificationId" ></param>
        public async Task<List<QualificationDocumentFilePath>> GetQualificationDocumentAndFilePath(int qualificationId)
        {
            var qualificationDocumentFilePaths = new List<QualificationDocumentFilePath>();
            var docNmaesAndFilePath = await _employeesRepository.GetQualificationDocumentAndFilePath(qualificationId);
            foreach (var item in docNmaesAndFilePath)
            {
                var qualificationDocumentFilePath = new QualificationDocumentFilePath();
                qualificationDocumentFilePath.EmpId = item.EmpId;
                qualificationDocumentFilePath.Document = item.Document != null ? item.Document : string.Empty;
                qualificationDocumentFilePath.QualificationName = item.QualificationName != null ? item.QualificationName : string.Empty;
                qualificationDocumentFilePaths.Add(qualificationDocumentFilePath);
            }
            return qualificationDocumentFilePaths;
        }

        //employeeChangeLog sp

        /// <summary>
             /// Logic to get employeeChangeLog 
            /// </summary> 
        /// <param name="employeeChangeLogViewModel" ></param>
        /// <param name="companyId" ></param>        

        public async Task<EmployeeChangeLogViewModel> GetAllEmployeesChangeLog(EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            var employeeIdValue = employeeChangeLogViewModel.EmpId;
            var companyid = Convert.ToString(companyId);
            var employeeChangeLogViewModels = new EmployeeChangeLogViewModel();
            employeeChangeLogViewModels.EmployeesLog = new List<EmployeesLog>();
            employeeChangeLogViewModels.ReportingPeople = new List<EmployeeDropdown>();
            employeeChangeLogViewModels.EmployeeDetails = new List<EmployeeDetailsDropdown>();
            var today = DateTime.Today.AddDays(-2).Date;
            employeeChangeLogViewModel.StartDate = today.Date.ToString(Constant.DateFormat);
            var moduleName = "";
            var dFrom = string.IsNullOrEmpty(employeeChangeLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.StartDate).ToString(Constant.DateFormatMDY);
            if (employeeIdValue == 0)
            {
                var empId = Constant.ZeroStr;
                List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@EmployeeModules",moduleName),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dFrom),
                    new KeyValuePair<string, string>("@companyId",companyid),
                };
                var attendanceReportDateModels = await _employeesRepository.GetAllEmployessByEmployeeLogFilter("spGetEmployeeLogByEmployeeFilterData", p);
                foreach (var item in attendanceReportDateModels)
                {
                    var autherName = await _employeesRepository.GetEmployeeById(item.EmpId, companyId);
                    var employeesLog = new EmployeesLog();
                    employeesLog.EmployeesLogId = item.Id;
                    employeesLog.FieldName = item.FieldName == null ? "" : item.FieldName;
                    employeesLog.PreviousValue = item.PreviousValue == null ? "" : item.PreviousValue;
                    employeesLog.NewValue = item.NewValue == null ? "" : item.NewValue;
                    employeesLog.Event = item.Event == null ? "" : item.Event;
                    employeesLog.AuthorName = autherName == null ? "" : autherName.FirstName + " " + autherName.LastName;
                    employeesLog.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                    employeeChangeLogViewModels.EmployeesLog.Add(employeesLog);
                }
            }
            employeeChangeLogViewModels.ReportingPeople = await GetAllEmployeesDrropdown(companyId);
            employeeChangeLogViewModels.EmployeeDetails = await GetAllEmployeeDetailsDrropdown(companyId);
            return employeeChangeLogViewModels;
        }

        /// <summary>
        /// Logic to get employeeChangeLog 
        /// </summary> 
        /// <param name="employeeChangeLogViewModel,companyId,pager" ></param>
        public async Task<EmployeeChangeLogViewModel> GetAllEmployeesChangeLogs(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            var employeeChangeLogViewModels = new EmployeeChangeLogViewModel
            {
                EmployeeLogs = await _employeesRepository.GetAllEmployessLogFirst(pager, employeeChangeLogViewModel, companyId),
                ReportingPeople = await GetAllEmployeesDrropdown(companyId),
                EmployeeDetails = await GetAllEmployeeDetailsDrropdown(companyId)
            };

            return employeeChangeLogViewModels;
        }




        /// <summary>
        /// Logic to get dropdown reporting people
        /// </summary>               
        public async Task<List<EmployeeDropdown>> GetAllEmployeesDrropdown(int companyId)
        {
            var employeeDropdowns = new List<EmployeeDropdown>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            if (listEmployee != null)
            {
                var employeedropdown = new EmployeeDropdown();
                employeedropdown.EmployeeId = 0;
                employeedropdown.EmployeeIdWithName = Common.Constant.AllEmployees;
                employeeDropdowns.Add(employeedropdown);
                foreach (var item in listEmployee)
                {
                    var employeeDropdown = new EmployeeDropdown();
                    employeeDropdown.EmployeeId = item.EmpId;
                    employeeDropdown.EmployeeName = item.UserName;
                    employeeDropdown.EmployeeIdWithName = item.UserName + Constant.Hyphen + item.FirstName + " " + item.LastName;
                    employeeDropdowns.Add(employeeDropdown);
                }
            }
            return employeeDropdowns;
        }

        /// <summary>
        /// Logic to get dropdown employeedetails
        /// </summary>   
        public async Task<List<EmployeeDetailsDropdown>> GetAllEmployeeDetailsDrropdown(int companyId)
        {
            var employeeDropdowns = new List<EmployeeDetailsDropdown>();
            var listEmployee = await _employeesRepository.GetAllEmployeesLog(companyId);
            var filterSections = listEmployee.GroupBy(x => x.SectionName).ToList();
            if (filterSections != null)
            {
                var employeedropdown = new EmployeeDetailsDropdown();
                employeedropdown.ModuleName = "";
                employeedropdown.EmployeeModules = Common.Constant.AllModules;
                employeeDropdowns.Add(employeedropdown);
                foreach (var item in filterSections)
                {
                    var employeeDropdown = new EmployeeDetailsDropdown();
                    employeedropdown.ModuleName = item.Key;
                    employeeDropdown.EmployeeModules = item.Key;
                    employeeDropdowns.Add(employeeDropdown);
                }
            }
            return employeeDropdowns;
        }


        /// <summary>
        /// Logic to get employeeChangeLogfilter 
        /// </summary> 
        /// <param name="employeeChangeLogViewModel" ></param>
        /// <param name="companyId" ></param>        
        public async Task<EmployeeChangeLogViewModel> GetAllEmployessByEmployeeLogFilter(EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            var employeeIdValue = employeeChangeLogViewModel.EmpId;
            var companyid = Convert.ToString(companyId);
            var employeeChangeLogViewModels = new EmployeeChangeLogViewModel();
            employeeChangeLogViewModels.EmployeesLog = new List<EmployeesLog>();
            var moduleName = "";
            if (employeeChangeLogViewModel.EmployeeModules == Common.Constant.AllModules)
            {
                moduleName = "";
            }
            else
            {
                moduleName = employeeChangeLogViewModel.EmployeeModules;
            }
            var dFrom = string.IsNullOrEmpty(employeeChangeLogViewModel.StartDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.StartDate).ToString(Constant.DateFormatMDY);
            var dTo = string.IsNullOrEmpty(employeeChangeLogViewModel.EndDate) ? "" : DateTimeExtensions.ConvertToNotNullDatetime(employeeChangeLogViewModel.EndDate).ToString(Constant.DateFormatMDY);
            if (employeeIdValue == 0)
            {
                var empId = Constant.ZeroStr;
                List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@EmployeeModules",moduleName),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dTo),
                    new KeyValuePair<string, string>("@companyId",companyid),
                };
                var attendanceReportDateModels = await _employeesRepository.GetAllEmployessByEmployeeLogFilter("spGetEmployeeLogByEmployeeFilterData", p);
                foreach (var item in attendanceReportDateModels)
                {
                    var autherName = await _employeesRepository.GetEmployeeById(item.EmpId, companyId);
                    var employeesLog = new EmployeesLog();
                    employeesLog.EmployeesLogId = item.Id;
                    employeesLog.FieldName = item.FieldName == null ? "" : item.FieldName;
                    employeesLog.PreviousValue = item.PreviousValue == null ? "" : item.PreviousValue;
                    employeesLog.NewValue = item.NewValue == null ? "" : item.NewValue;
                    employeesLog.Event = item.Event == null ? "" : item.Event;
                    employeesLog.AuthorName = autherName == null ? "" : autherName.FirstName + " " + autherName.LastName;
                    employeesLog.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                    employeeChangeLogViewModels.EmployeesLog.Add(employeesLog);
                }
            }
            else
            {
                var empId = Convert.ToString(employeeIdValue);
                List<KeyValuePair<string, string>> p = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("@empId", empId),
                    new KeyValuePair<string, string>("@EmployeeModules",moduleName),
                    new KeyValuePair<string, string>("@startDate",dFrom),
                    new KeyValuePair<string, string>("@endDate",dTo),
                    new KeyValuePair<string, string>("@companyId",companyid),
                };
                var attendanceReportDateModels = await _employeesRepository.GetAllEmployessByEmployeeLogFilter("spGetEmployeeLogByEmployeeFilterData", p);
                foreach (var item in attendanceReportDateModels)
                {
                    var autherName = await _employeesRepository.GetEmployeeById(item.EmpId, companyId);
                    var attendacelistViewModel = new EmployeesLog();
                    attendacelistViewModel.EmployeesLogId = item.Id;
                    attendacelistViewModel.FieldName = item.FieldName == null ? "" : item.FieldName;
                    attendacelistViewModel.PreviousValue = item.PreviousValue == null ? "" : item.PreviousValue;
                    attendacelistViewModel.NewValue = item.NewValue == null ? "" : item.NewValue;
                    attendacelistViewModel.Event = item.Event == null ? "" : item.Event;
                    attendacelistViewModel.AuthorName = autherName == null ? "" : autherName.FirstName + " " + autherName.LastName;
                    attendacelistViewModel.CreatedDate = Convert.ToDateTime(item.CreatedDate);
                    employeeChangeLogViewModels.EmployeesLog.Add(attendacelistViewModel);
                }
            }
            return employeeChangeLogViewModels;

        }


        /// <summary>
        /// Logic to get employeeChangeLogfilter  details 
        /// </summary> 
        /// <param name="employeeChangeLogViewModel,companyId,pager" ></param>       
        public async Task<EmployeeChangeLogViewModel> GetAllEmployeesByEmployeeLogFilter(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {

            var employeeChangeLogViewModels = new EmployeeChangeLogViewModel();

            employeeChangeLogViewModels.EmployeeLogs = await _employeesRepository.GetAllEmployessByFilter(pager, employeeChangeLogViewModel, companyId);

            return employeeChangeLogViewModels;

        }

        // employeeactivitylog

        /// <summary>
        /// Logic to get create and update  the employeeactivitylog
        /// </summary> 
        /// <param name="empId" ></param>
        public async Task<bool> CreateEmployeeActivityLog(int empId, int companyId)
        {
            var employeeLoginActivityEntity = await _employeesRepository.GetEmployeeLoginDetails(empId, companyId);
            if (employeeLoginActivityEntity == null)
            {
                var employeeActivityLogs = new EmployeeActivityLogEntity();
                employeeActivityLogs.LastLoginDate = DateTime.Now;
                employeeActivityLogs.EmployeeId = empId;
                employeeActivityLogs.IPAddress = LoginIpAddress.GetLoginIpAddress();
                var create = await _employeesRepository.CreateEmployeeLogActivity(employeeActivityLogs, companyId);
            }
            else
            {
                try
                {
                    employeeLoginActivityEntity.LastLoginDate = DateTime.Now;
                    employeeLoginActivityEntity.IPAddress = LoginIpAddress.GetLoginIpAddress();
                    var create = await _employeesRepository.CreateEmployeeLogActivity(employeeLoginActivityEntity, companyId);
                }
                catch (Exception ex) { }


            }
            return true;
        }

        /// <summary>
        /// Logic to get employeeactivitylog list
        /// </summary>   
        public async Task<List<EmployeeActivityLog>> GetEmployeeActivity(int companyId)
        {
            var employeeActivityLogs = new List<EmployeeActivityLog>();
            employeeActivityLogs = await _employeesRepository.GetAllEmployeeActivityLogs(companyId);
            return employeeActivityLogs;
        }

        //Salary Modules

        /// <summary>
        /// Logic to get salary detail by particular employees
        /// </summary>
        /// <param name="EmpId" ></param>
        public async Task<SalaryViewModel> GetAllSalaryByEmpId(int EmpId)
        {
            var listOfSalary = new List<salarys>();
            var listOfSalaryViewModel = new SalaryViewModel();
            var listSalaryViewModel = await _employeesRepository.GetSalaryByEmpId(EmpId);
            listOfSalaryViewModel.EmpId = EmpId;
            listOfSalaryViewModel.salary = listSalaryViewModel;
            return listOfSalaryViewModel;
        }

        /// <summary>
        /// Logic to get create and update the salary detail 
        /// </summary>
        /// <param name="salarys" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> AddSalaryDetails(salarys salarys, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            if (salarys != null)
            {
                if (salarys.SalaryId == 0)
                {

                    salarys.CreatedBy = sessionEmployeeId;
                    salarys.CreatedDate = DateTime.Now;
                    var salaryEntity = _mapper.Map<SalaryEntity>(salarys);
                    var add = await _employeesRepository.AddSalaryDetails(salaryEntity);
                    await InsertSalaryEmployeesLog(Common.Constant.CreateSalaryDetails, result, Common.Constant.Add, sessionEmployeeId, companyId, null);
                    result = salaryEntity.EmpId;

                }
                else
                {

                    var salaryEntity = await _employeesRepository.GetBysalaryId(salarys.SalaryId);
                    var employeesChangeLogs = await GetUpdateSalaryDetailsDiffrencetFieldName(salaryEntity, salarys);
                    salarys.UpdatedBy = sessionEmployeeId;
                    salarys.UpdatedDate = DateTime.Now;
                    salarys.CreatedDate = salaryEntity.CreatedDate;
                    salarys.CreatedBy = salaryEntity.CreatedBy;
                    var mapSalaryEntity = _mapper.Map<SalaryEntity>(salarys);
                    var data = await _employeesRepository.AddSalaryDetails(mapSalaryEntity);
                    await InsertSalaryEmployeesLog(Common.Constant.UpdateSalaryDetails, result, Common.Constant.Update, sessionEmployeeId, companyId, employeesChangeLogs);
                    result = salaryEntity.EmpId;

                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get salary detail by particular employees get count
        /// </summary>
        /// <param name="EmpId,Month,Year" ></param>  
        public async Task<int> GetBySalaryCount(int month, int empId, int year)
        {
            var salaryMonthCount = await _employeesRepository.GetSalaryMonth(month, empId, year);
            return salaryMonthCount;
        }

        /// <summary>
        /// Logic to get salary detail by particular employees 
        /// </summary>
        /// <param name="EmpId" ></param>       
        public async Task<List<salarys>> GetSalaryByEmpId(int EmpId)
        {
            var listOfSalarys = new List<salarys>();
            listOfSalarys = await _employeesRepository.GetSalaryByEmpId(EmpId);
            return listOfSalarys;
        }

        /// <summary>
        /// Logic to get delete salary detail 
        /// </summary>
        /// <param name="salarys" ></param>     
        public async Task DeleteSalary(salarys salarys)
        {
            var salaryEntity = await _employeesRepository.GetBysalaryId(salarys.SalaryId);
            salaryEntity.IsDeleted = true;
            await _employeesRepository.DeleteSalary(salaryEntity);
        }

        /// <summary>
        /// Logic to get EmployeesLog added the salary detail 
        /// </summary>
        /// <param name="sectionName" ></param>     
        /// <param name="empId" ></param>  
        /// <param name="eventName" ></param>  
        /// <param name="sessionEmployeeId" ></param> 
        /// <param name="employeesChangeLogs" ></param>
        public async Task<bool> InsertSalaryEmployeesLog(string sectionName, int empId, string eventName, int sessionEmployeeId, int companyId, List<EmployeesChangeLog> employeesChangeLogs = null)
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

        /// <summary>
        /// Logic to get EmployeesLog added the salary detail 
        /// </summary>
        /// <param name="salaryEntity" ></param>     
        /// <param name="salarys" ></param>         
        public async Task<List<EmployeesChangeLog>> GetUpdateSalaryDetailsDiffrencetFieldName(SalaryEntity salaryEntity, salarys salarys)
        {
            var employeesChangeLogs = new List<EmployeesChangeLog>();
            if (salaryEntity != null)
            {

                if (salaryEntity.Year != salarys.Year)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Year;
                    employeesChangeLog.PreviousValue = salaryEntity.Year.ToString();
                    employeesChangeLog.NewValue = salarys.Year.ToString();
                    employeesChangeLogs.Add(employeesChangeLog);
                }

                if (salaryEntity.Amount != salarys.Amount)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Amount;
                    employeesChangeLog.PreviousValue = salaryEntity.Amount.ToString();
                    employeesChangeLog.NewValue = salarys.Amount.ToString();
                    employeesChangeLogs.Add(employeesChangeLog);
                }
                if (salaryEntity.Month != salarys.Month)
                {
                    var employeesChangeLog = new EmployeesChangeLog();
                    employeesChangeLog.FieldName = Common.Constant.Month;
                    employeesChangeLog.PreviousValue = salaryEntity.Month.ToString();
                    employeesChangeLog.NewValue = salarys.Month.ToString();
                    employeesChangeLogs.Add(employeesChangeLog);
                }
            }
            return employeesChangeLogs;
        }

        /// <summary>
        /// Logic to get count of Employee Log for all employees 
        /// </summary>
        /// <param name="pager,employeeChangeLogViewModel,companyId" ></param> 
        public async Task<int> GetAllEmployessByEmployeeLogCount(SysDataTablePager pager, EmployeeChangeLogViewModel employeeChangeLogViewModel, int companyId)
        {
            return await _employeesRepository.GetAllEmployessByEmployeeLogCount(pager, employeeChangeLogViewModel, companyId);
        }

        /// <summary>
        /// Logic to get all the Employee Activity Log list for employees
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param> 

        public async Task<EmployeeActivityLog> GetAllEmployeeActivitylogs(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            var employeeActivityLog = new EmployeeActivityLog();
            employeeActivityLog.EmployeeActivityLogViewModels = await _employeesRepository.GetEmployeeActivitylogs(pager, columnName, columnDirection, companyId);
            return employeeActivityLog;
        }

        /// <summary>
        /// Logic to get count of Employee Activity Log for all employees 
        /// </summary>
        /// <param name="pager" ></param> 
        public async Task<int> GetAllEmployeeActivityLogfilterCount(SysDataTablePager pager, int companyId)
        {
            var result = await _employeesRepository.GetAllEmployeeActivityLogfilterCount(pager, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get count of Employee  for all employees 
        /// </summary>
        /// <param name="pager" ></param> 
        public async Task<int> GetEmployeesListCount(SysDataTablePager pager, int companyId)
        {
            var employeesCount = await _employeesRepository.GetEmployeesListCount(pager, companyId);
            return employeesCount;
        }

        /// <summary>
        /// Logic to get the Employee list
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" ></param> 
        public async Task<List<Employees>> GetEmployeesDetailsList(SysDataTablePager pager, string columnDirection, string ColumnName, int companyId)
        {
            try
            {
                var employees = new List<Employees>();
                var employee = new Employees();
                employee.EmployeesDetailsDataModels = await _employeesRepository.GetEmployeeDetailsList(pager, columnDirection, ColumnName, companyId);
                employees = _mapper.Map<List<Employees>>(employee.EmployeesDetailsDataModels);
                var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
                var probationPeriod = await _companyRepository.GetProbationMonth();
                foreach (var emp in employees)
                {
                    var profileByEmpId = allEmployeeProfiles.FirstOrDefault(x => x.EmpId == employee.EmpId);
                    emp.ProbationDays = profileByEmpId == null ? "" : YetToGetProbationDay(profileByEmpId, probationPeriod);
                    emp.ProfileCompletionPercentage = await GetEmployeeProfileRecordsByEmployeeId(emp.EmpId, companyId);
                }
                return employees ?? new List<Employees>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Employees>> GetAllReleaveTypes(int companyId)
        {
            var employees = new List<Employees>();
            var employee = new Employees();
            var releaveReasonTypes = await _employeesRepository.GetAllReleaveTypes(companyId);
            try
            {
                employee.RelievingReasons = _mapper.Map<List<RelievingReasons>>(releaveReasonTypes);
            }
            catch (Exception ex)
            {
            }
            employees.Add(employee);
            return employees;
        }
    }


}

