using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.API.Service
{
    public class EmployeesAPIService : IEmployeesAPIService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IMasterRepository _masterRepository;
        private readonly IAssetRepository _assetRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IBenefitRepository _benefitRepository;
        private readonly ICompanyPolicyRepository _companyPolicyRepository;
        public EmployeesAPIService(IEmployeesRepository employeesRepository, IMapper mapper, IConfiguration config, IMasterRepository masterRepository, IAssetRepository assetRepository, ICompanyRepository companyRepository, IEmailDraftRepository emailDraftRepository, IBenefitRepository benefitRepository, ICompanyPolicyRepository companyPolicyRepository)
        {
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _masterRepository = masterRepository;
            _assetRepository = assetRepository;
            _companyRepository = companyRepository;
            _emailDraftRepository = emailDraftRepository;
            _benefitRepository = benefitRepository;
            _companyPolicyRepository = companyPolicyRepository;
        }


        public async Task<List<EmployeesRequestModel>> GetAllEmployees(int empId,int companyId)
        {
            var listOfEmployees = new List<EmployeesRequestModel>();
            var listtype = new List<RelievingReasons>();
            var listEmployee = await _employeesRepository.GetAllEmployees(companyId);
            var allReleaveTypes = await _masterRepository.GetAllRelievingReason(companyId);
            if (empId == 0)
            {
                if (listEmployee != null)
                {
                    listOfEmployees = _mapper.Map<List<EmployeesRequestModel>>(listEmployee);
                }
                if (allReleaveTypes != null)
                {
                    listtype = _mapper.Map<List<RelievingReasons>>(allReleaveTypes);
                }

                var allDesignation = await _employeesRepository.GetAllDesignation(companyId);

                var allDepartment = await _employeesRepository.GetAllDepartment(companyId);
                var allEmployeeProfiles = await _employeesRepository.GetAllEmployeeProfile(companyId);
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
                    item.ProfileCompletionPercentage = await GetEmployeeProfileRecordsByEmployeeId(item.EmpId, companyId);
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
            }
            return listOfEmployees ?? new List<EmployeesRequestModel>();
        }

        public async Task<EmployeesRequestModel> GetEmployeeById(int empId,int companyId)
        {
            var employeeViewModel = new EmployeesRequestModel();
            var empEntity = await _employeesRepository.GetEmployeeByIdView(empId, companyId);
            var assetEntity = await _assetRepository.GetAssetByEmployeeId(empId,companyId);
            var benefitEntity = await _benefitRepository.GetEmployeeBenefitsByEmployeeId(empId,companyId);
            var count = assetEntity.Count();
            if (empEntity != null)
            {
                employeeViewModel = _mapper.Map<EmployeesRequestModel>(empEntity);
                employeeViewModel.EmployeeSortName = Common.Common.GetEmployeeSortName(empEntity.FirstName, empEntity.LastName);
                var profileByEmpId = await _employeesRepository.GetProfileByEmployeeId(empId, companyId);
                employeeViewModel.EmployeeProfileImage = profileByEmpId != null ? profileByEmpId.ProfileName : string.Empty;
            }
            if (assetEntity != null)
            {
                employeeViewModel.AllAssetId = count;
            }
            if (benefitEntity != null)
            {
                employeeViewModel.BenefitId = benefitEntity.BenefitId;
            }
            return employeeViewModel;
        }


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
            var OtherDetailsInfo = await _employeesRepository.GetOtherDetailsByEmployeeId(empId,companyId);
            if (OtherDetailsInfo != null)
            {
                percentage += 10;
            }
            var qualificationInfo = await _employeesRepository.GetQualificationByEmployeeId(empId,companyId);
            if (qualificationInfo != null)
            {
                percentage += 20;
            }
            var experienceInfo = await _employeesRepository.GetExperienceByEmployeeId(empId,companyId);
            if (experienceInfo != null)
            {
                percentage += 20;
            }
            var bankDetailsInfo = await _employeesRepository.GetBankDetailsByEmployeeId(empId,companyId);
            if (bankDetailsInfo != null)
            {
                percentage += 20;
            }
            var completionPercentage = Convert.ToString(percentage);
            return completionPercentage;
        }

        public async Task<UserTimeSheetResponse> InsertEmployees(EmployeesRequestModel employees, int companyId)
        {
            var userEmployeesResponse = new UserTimeSheetResponse();
            var result = 0;
            if (employees != null)
            {
                if (employees.EmpId == 0)
                {
                    var randomPassword = Common.Common.GeneratePassword();
                    employees.Password = Common.Common.sha256_hash(randomPassword);
                    employees.CreatedBy = employees.EmpId;
                    employees.CreatedDate = DateTime.Now;
                    employees.IsProbationary = false;
                    var employeesEntity = _mapper.Map<EmployeesEntity>(employees);
                    var data = await _employeesRepository.CreateEmployee(employeesEntity,companyId);
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
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId,companyId);
                    var domainName = Convert.ToString(_config.GetSection("ConnectionUrl").GetSection("DomainName").Value);
                    var infoEmailName = Convert.ToString(_config.GetSection(Common.Constant.vphospitalInfoMailId).Value);
                    var allemployee = await _employeesRepository.GetAllEmployees(employees.CompanyId);
                    List<int> reportingPersonEmployeeIds = await _employeesRepository.GetReportingPersonEmployeeById(data,companyId);
                    var reportingPersion = await GetemployeeNameByReportingPersionId(reportingPersonEmployeeIds, allemployee);
                    var bodyContent = EmailBodyContent.WelcomeEmployeeEmailBodyContent(employeesEntity, randomPassword, domainName, infoEmailName, reportingPersion, emailDraftContentEntity.DraftBody);
                    await InsertEmailQueue(employees.OfficeEmail, emailDraftContentEntity, bodyContent);
                    result = data;
                    // await InsertEmployeesLog(Common.Constant.CreateEmployeesLog, result, Common.Constant.Add, sessionEmployeeId);
                    userEmployeesResponse.IsSuccess = true;
                    userEmployeesResponse.Message = Common.Constant.Success;
                }
                else
                {
                    var employeesEntity = await _employeesRepository.GetEmployeeById(employees.EmpId, companyId);
                    var reportingPersonsEmployeeEntity = await _employeesRepository.GetReportingPersonEmployeeById(employees.EmpId,companyId);
                    var reportingPersonsEntities = await _employeesRepository.GetReportingPersonEmployeeId(employees.EmpId, companyId);
                    // var employeesChangeLogs = await GetUpdateEmployeeDiffrencetFieldName(employeesEntity, employees, reportingPersonsEntities);
                    employeesEntity.FatherName = employees.FatherName;
                    employeesEntity.PersonalEmail = employees.PersonalEmail;
                    employeesEntity.FirstName = employees.FirstName;
                    employeesEntity.LastName = employees.LastName;
                    employeesEntity.OfficeEmail = employees.OfficeEmail;
                    employeesEntity.DepartmentId = employees.DepartmentId;
                    employeesEntity.DesignationId = employees.DesignationId;
                    employeesEntity.EsslId = employees.EsslId;
                    employeesEntity.UpdatedDate = DateTime.Now;
                    employeesEntity.UpdatedBy = employees.EmpId;

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

                    //if (string.IsNullOrEmpty(employees.FatherName))
                    //{
                    //    await InsertEmployeesLog(Common.Constant.UpdateEmployeesLog, employees.EmpId, Common.Constant.Add, sessionEmployeeId, employeesChangeLogs);
                    //}
                    //else
                    //{
                    //    await InsertEmployeesLog(Common.Constant.UpdateEmployeesLog, employees.EmpId, Common.Constant.Update, sessionEmployeeId, employeesChangeLogs);
                    //}
                    result = data;
                    userEmployeesResponse.IsSuccess = true;
                    userEmployeesResponse.Message = Common.Constant.Success;
                }
            }
            else
            {
                userEmployeesResponse.IsSuccess = false;
                userEmployeesResponse.Message = Common.Constant.Failure;
            }

            return userEmployeesResponse;
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
                /// Logic to get reportingperson list 
        /// </summary> 
        public async Task<List<ReportingPersons>> GetAllReportingPersons(int companyId)
        {
            var result = new List<ReportingPersons>();
            var listEmployee = await _employeesRepository.GetAllReportingPersons(companyId);
            var reporters = _mapper.Map<List<EmployeesRequestModel>>(listEmployee);
            foreach (var item in reporters)
            {
                result.Add(new ReportingPersons()
                {
                    EmpId = item.EmpId,
                    EmployeeName = item?.FirstName + " " + item?.LastName,
                });
            }
            return result;
        }

        /// <summary>
                /// Logic to get designation list 
        /// </summary> 
        public async Task<List<Designations>> GetAllDesignation(int companyId)
        {
            var listOfDesignation = new List<Designations>();
            var listDesignation = await _employeesRepository.GetAllDesignation(companyId);
            if (listDesignation.Count() > 0)
            {
                listOfDesignation = _mapper.Map<List<Designations>>(listDesignation);
            }
            return listOfDesignation;
        }

        /// <summary>
                /// Logic to get department list 
        /// </summary>
        public async Task<List<Departments>> GetAllDepartment(int companyId)
        {
            var listOfDepartment = new List<Departments>();
            var listDepartment = await _employeesRepository.GetAllDepartment(companyId);
            if (listDepartment.Count() > 0)
            {
                listOfDepartment = _mapper.Map<List<Departments>>(listDepartment);
            }
            return listOfDepartment;
        }

        /// <summary>
                /// Logic to get role list 
        /// </summary>
        public async Task<List<RoleViewModels>> GetAllRoleTable(int companyId)
        {
            var listOfRoleTable = new List<RoleViewModels>();
            var listRoleTable = await _employeesRepository.GetAllRoleTable(companyId);
            if (listRoleTable != null)
            {
                listOfRoleTable = _mapper.Map<List<RoleViewModels>>(listRoleTable);
            }
            return listOfRoleTable;
        }

        /// <summary>
               /// Logic to get the employees detail get by  particular username
              /// </summary>         
        public async Task<string> GetEmployeeUserName(int companyId)
        {
            var listCompany = await _companyRepository.GetAllCompanySetting(companyId);
            var employeeCount = await _companyRepository.GetMaxCountOfEmployeesByCompanyId(companyId);

            string? userName;
            userName = listCompany.CompanyCode + (employeeCount).ToString("D3");
            return userName;
        }


        /// <summary>
               ///Logic to get relievingreasons detail list api
              /// </summary>                  
        public async Task<List<RelievingReasons>> GetAllRelievingReason(int companyId)
        {
            var listRelievingReason = new List<RelievingReasons>();
            var relievingReasonEntity = await _masterRepository.GetAllRelievingReason(companyId);
            if (relievingReasonEntity != null)
            {
                listRelievingReason = _mapper.Map<List<RelievingReasons>>(relievingReasonEntity);
            }
            return listRelievingReason;
        }


        /// <summary>
        /// Logic to get reject the employees detail by particular employees
        /// </summary>
        /// <param name="employees" ></param>
        public async Task<bool> GetRejectEmployees(EmployeesRequestModel employees)
        {
            var emp = _mapper.Map<Employees>(employees);
            var result = await _employeesRepository.GetRejectEmployees(emp);
            return result;
        }
    }
}
