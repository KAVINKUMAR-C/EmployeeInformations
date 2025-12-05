using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;
using Duration = EmployeeInformations.Common.Enums.Duration;

namespace EmployeeInformations.Business.Service
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, IConfiguration config, IEmployeesRepository employeesRepository, IEmailDraftRepository emailDraftRepository)
        {
            _companyRepository = companyRepository;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _config = config;
            _emailDraftRepository = emailDraftRepository;
        }

        /// <summary>
        /// Logic to get company list
        /// </summary>  
        public async Task<List<Company>> GetAllCompany()
        {
            var listOfCompany = new List<Company>();
            listOfCompany = await _companyRepository.GetAllCompanys();            
            return listOfCompany;
        }

        /// <summary>
        /// Logic to get create and update  the company detail
        /// </summary>
        /// <param name="company" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> CreateCompany(Company company, int sessionEmployeeId)
        {
            var result = 0;
            if (company != null)
            {
                if (company.CompanyId == 0)
                {
                    company.CreatedBy = sessionEmployeeId;
                    company.CreatedDate = DateTime.Now;
                    var companyEntity = _mapper.Map<CompanyEntity>(company);
                    result = await _companyRepository.CreateCompany(companyEntity);
                   

                    var randomPassword = Common.Common.GeneratePassword();

                    var employeeEntity = new EmployeesEntity();
                    employeeEntity.OfficeEmail = company.ContactPersonEmail;
                    employeeEntity.UserName = Common.Constant.Admin;
                    employeeEntity.FirstName = company.ContactPersonFirstName;
                    employeeEntity.LastName = company.ContactPersonLastName;
                    employeeEntity.Password = Common.Common.sha256_hash(randomPassword);
                    employeeEntity.CompanyId = companyEntity.CompanyId;
                    employeeEntity.RoleId = 1;
                    employeeEntity.DesignationId = 0;
                    employeeEntity.DepartmentId = 0;
                    employeeEntity.CreatedBy = sessionEmployeeId;
                    employeeEntity.CreatedDate = DateTime.Now;
                    employeeEntity.IsOnboarding = true;
                    employeeEntity.DepartmentId = 1;
                    employeeEntity.DesignationId = 1;
                    
                    var data = await _employeesRepository.CreateEmployeeFromCompany(employeeEntity);

                    var draftTypeId = (int)EmailDraftType.WelcomeEmployee;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, employeeEntity.CompanyId);
                    var domainName = Convert.ToString(_config.GetSection("ConnectionUrl").GetSection("DomainName").Value);
                    var infoEmailName = Convert.ToString(_config.GetSection(Common.Constant.vphospitalInfoMailId).Value);
                    List<int> reportingPersonEmployeeIds = await _employeesRepository.GetReportingPersonEmployeeById(data, employeeEntity.CompanyId);
                    var allemployee = await _employeesRepository.GetAllEmployees(result);
                    var reportingPersion = await GetemployeeNameByReportingPersionIds(reportingPersonEmployeeIds, allemployee);
                    var bodyContent = EmailBodyContent.WelcomeEmployeeEmailBodyContent(employeeEntity, randomPassword, domainName, infoEmailName, reportingPersion, emailDraftContentEntity.DraftBody);
                    await InsertEmailQueue(company.ContactPersonEmail, emailDraftContentEntity, bodyContent);
                }
                else
                {
                    var companyEntity = await _companyRepository.GetByCompanyId(company.CompanyId);
                    companyEntity.UpdatedDate = DateTime.Now;
                    companyEntity.UpdatedBy = sessionEmployeeId;
                    companyEntity.CompanyName = company.CompanyName;
                    companyEntity.CompanyEmail = company.CompanyEmail;
                    companyEntity.CompanyPhoneNumber = company.CompanyPhoneNumber;
                    companyEntity.Industry = company.Industry;
                    companyEntity.ContactPersonFirstName = company.ContactPersonFirstName;
                    companyEntity.ContactPersonLastName = company.ContactPersonLastName;
                    companyEntity.ContactPersonGender = company.ContactPersonGender;
                    companyEntity.ContactPersonEmail = company.ContactPersonEmail;
                    companyEntity.ContactPersonPhoneNumber = company.ContactPersonPhoneNumber;
                    companyEntity.PhysicalAddress1 = company.PhysicalAddress1;
                    companyEntity.PhysicalAddress2 = company.PhysicalAddress2;
                    companyEntity.PhysicalAddressState = company.PhysicalAddressState;
                    companyEntity.PhysicalAddressCity = company.PhysicalAddressCity;
                    companyEntity.PhysicalAddressZipCode = company.PhysicalAddressZipCode;
                    companyEntity.MailingAddress1 = company.MailingAddress1;
                    companyEntity.MailingAddress2 = company.MailingAddress2;
                    companyEntity.MailingAddressState = company.MailingAddressState;
                    companyEntity.MailingAddressCity = company.MailingAddressCity;
                    companyEntity.MailingAddressZipCode = company.MailingAddressZipCode;
                    companyEntity.CountryCode = company.CountryCode;
                    companyEntity.CompanyCountryCode = company.CompanyCountryCode;
                    companyEntity.CompanyFilePath = company.CompanyFilePath;
                    companyEntity.CompanyLogo = string.IsNullOrEmpty(company.CompanyLogo) ? companyEntity.CompanyLogo : company.CompanyLogo;
                    companyEntity.PhysicalCountryId = company.PhysicalCountryId;
                    companyEntity.MailingCountryId = company.MailingCountryId;
                    companyEntity.IsActive = company.IsActive;
                    companyEntity.CompanyId = company.CompanyId;
                    result = await _companyRepository.CreateCompany(companyEntity);
                }
            }

            return result;
        }

        /// <summary>
        /// Logic to get reportingPersonname employee detail
        /// </summary>
        /// <param name="reportingPersonEmployeeIds" ></param>
        /// <param name="allemployee" ></param>
        public async Task<string> GetemployeeNameByReportingPersionIds(List<int> reportingPersonEmployeeIds, List<EmployeesEntity> allemployee)
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
        /// Logic to get insertemailqueue details
        /// </summary>
        /// <param name="contactPersonEmail" ></param>
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailQueue(string contactPersonEmail, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var toEmail = contactPersonEmail;
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.CompanyCreateEmployeeReason;
            emailEntity.IsSend = false;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            var email = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        /// <summary>
        /// Logic to get create and update  the companyemailsetting detail
        /// </summary>
        /// <param name="mailScheduler" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> CreateMailScheduler(MailScheduler mailScheduler, int sessionEmployeeId)
        {
            try
            {
                var result = false;
                var sendDate = new DateTime();
                var strEmployeeId = mailScheduler.EmployeeId;
                var strMailSendingDays = mailScheduler.SendigStatus;
                if (strEmployeeId.Count() > 0)
                {
                    var str = string.Join(",", strEmployeeId);
                    mailScheduler.WhomToSend = str.TrimEnd(',');
                }
                if (strMailSendingDays.Count() > 0)
                {
                    var str = string.Join(",", strMailSendingDays);
                    mailScheduler.MailSendingDays = str.TrimEnd(',');
                }

                DateTime sendTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                var schedulerDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                var sendMailDate = schedulerDate.AddHours(sendTime.Hour).AddMinutes(sendTime.Minute);

                var getScheduler = await _companyRepository.GetMailSchedulerBySchedulerId(mailScheduler.SchedulerId);

                if (mailScheduler.SchedulerId == 0 || getScheduler.MailTime != sendMailDate)
                {
                    if (mailScheduler.DurationId == (int)Duration.Once)
                    {
                        if (mailScheduler.EmailDraftId == (int)EmailDraftType.DailyAttendance)
                        {
                            var day = mailScheduler.MailTime.DayOfWeek;

                            if (day == DayOfWeek.Monday)
                            {
                                DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                                mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                                sendDate = mailScheduler.MailDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                            }
                            else
                            {
                                DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                                mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                                sendDate = mailScheduler.MailDate.AddDays(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                            }
                        }
                        else if (mailScheduler.EmailDraftId == (int)EmailDraftType.WeeklyAttendance)
                        {
                            DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                            mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                            sendDate = mailScheduler.MailDate.AddDays(7).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                        }
                        else if (mailScheduler.EmailDraftId == (int)EmailDraftType.MonthlyAttendance)
                        {
                            DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                            mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                            sendDate = mailScheduler.MailDate.AddMonths(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                        }
                        else if (mailScheduler.EmailDraftId == (int)EmailDraftType.YearlyAttendance)
                        {
                            DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                            mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                            sendDate = mailScheduler.MailDate.AddYears(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                        }
                    }
                    else if (mailScheduler.DurationId == (int)Duration.Daily)
                    {
                        var day = mailScheduler.MailTime.DayOfWeek;

                        if (day == DayOfWeek.Monday)
                        {
                            DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                            mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                            sendDate = mailScheduler.MailDate.AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                        }
                        else
                        {
                            DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                            mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                            sendDate = mailScheduler.MailDate.AddDays(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                        }
                    }
                    else if (mailScheduler.DurationId == (int)Duration.Monthly)
                    {
                        DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                        mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                        sendDate = mailScheduler.MailDate.AddMonths(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                    }
                    else if (mailScheduler.DurationId == (int)Duration.Yearly)
                    {
                        DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                        mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                        sendDate = mailScheduler.MailDate.AddYears(1).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                    }
                    else if (mailScheduler.DurationId == (int)Duration.Custom)
                    {
                        DateTime fromTime = Convert.ToDateTime(mailScheduler.StrMailTime);
                        mailScheduler.MailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.StrMailDate);
                        sendDate = mailScheduler.MailDate.AddDays(7).AddHours(fromTime.Hour).AddMinutes(fromTime.Minute);
                    }
                }

                if (mailScheduler != null)
                {
                    if (mailScheduler.SchedulerId == 0)
                    {

                        mailScheduler.CreatedBy = sessionEmployeeId;
                        mailScheduler.CreatedDate = DateTime.Now;
                        mailScheduler.EmailDraftId = mailScheduler.EmailDraftId;
                        var mailSend = sendDate;
                        mailScheduler.MailTime = mailSend;
                        var companyEntity = _mapper.Map<MailSchedulerEntity>(mailScheduler);
                        result = await _companyRepository.CreateMailScheduler(companyEntity);
                    }
                    else
                    {
                        var mailSchedulerEntity = await _companyRepository.GetMailSchedulerBySchedulerId(mailScheduler.SchedulerId);
                        mailSchedulerEntity.UpdatedDate = DateTime.Now;
                        mailSchedulerEntity.UpdatedBy = sessionEmployeeId;
                        mailSchedulerEntity.CompanyId = mailScheduler.CompanyId;
                        mailSchedulerEntity.DurationId = mailScheduler.DurationId;
                        mailSchedulerEntity.WhomToSend = mailScheduler.WhomToSend;
                        mailSchedulerEntity.FileFormat = mailScheduler.FileFormat;
                        mailSchedulerEntity.MailTime = sendDate == DateTime.MinValue ? mailSchedulerEntity.MailTime : sendDate;
                        mailSchedulerEntity.MailSendingDays = mailScheduler.MailSendingDays;
                        mailSchedulerEntity.EmailDraftId = mailScheduler.EmailDraftId;
                        mailSchedulerEntity.IsActive = mailScheduler.IsActive;
                        result = await _companyRepository.CreateMailScheduler(mailSchedulerEntity);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }

        }

        public async Task<MailSchedulerViewModels> GetMailSchedulerBySchedulerId(int schedulerId)
        {
            var mailSchedulerViewModels = new MailSchedulerViewModels();
            var mailSchedulerEntity = await _companyRepository.GetMailSchedulerBySchedulerId(schedulerId);
            var mailScheduler = _mapper.Map<MailScheduler>(mailSchedulerEntity);
            var strEmployeeId = mailSchedulerEntity.WhomToSend;
            var listOfDays = new List<int>();
            listOfDays = mailScheduler.MailSendingDays.Split(',').Select(int.Parse).ToList();

            if (mailSchedulerEntity != null)
            {
                var mailTime = mailScheduler.MailTime.ToString("hh:mm tt");
                mailSchedulerViewModels.SchedulerId = mailScheduler.SchedulerId;
                mailSchedulerViewModels.CompanyId = mailScheduler.CompanyId;
                mailSchedulerViewModels.FileFormat = mailScheduler.FileFormat;
                mailSchedulerViewModels.ReportName = mailScheduler.ReportName;
                mailSchedulerViewModels.EmployeeId = mailScheduler.WhomToSend;
                mailSchedulerViewModels.MailSendingDays = mailScheduler.MailSendingDays;
                mailSchedulerViewModels.EmailDraftId = mailScheduler.EmailDraftId;
                mailSchedulerViewModels.WhomToSend = mailScheduler.WhomToSend;
                mailSchedulerViewModels.DurationId = mailScheduler.DurationId;
                mailSchedulerViewModels.StrMailDate = DateTimeExtensions.ConvertToNotNullDatetime(mailScheduler.MailTime.ToString(Constant.DateFormat)).ToString(Constant.DateFormat);
                mailSchedulerViewModels.StrMailTime = mailTime;
                mailSchedulerViewModels.IsActive = mailScheduler.IsActive;
                mailSchedulerViewModels.SendigStatus = listOfDays;


            }
            return mailSchedulerViewModels;
        }



        /// <summary>
        ///  Logic to get the company detail by particular companyId 
        /// </summary>
        /// <param name="companyId" ></param>
        public async Task<Company> GetByCompanyId(int companyId)
        {
           
            var companyEntity = await _companyRepository.GetByCompanyId(companyId);

            if (companyEntity == null)
            {
                return new Company();
            }
            var company = _mapper.Map<Company>(companyEntity);
            company.CompanyLogo = string.IsNullOrEmpty(company.CompanyLogo) ? company.CompanyLogo : company.CompanyLogo;
            return company;
        }

        /// <summary>
         ///  Logic to get the company detail view by particular companyId 
         /// </summary>
        /// <param name="companyId" ></param>
        public async Task<Company> GetByViewCompanyId(int companyId)
        {
            var company = new Company();
            var companyEntity = await _companyRepository.GetByCompanyId(companyId);
            if (companyEntity == null)
            {
                return new Company();
            }
             company = _mapper.Map<Company>(companyEntity);
            company.PersonGender = Convert.ToString((Gender)companyEntity.ContactPersonGender);
            company.CountryName = await _employeesRepository.GetCountryNameBySecondaryCountryId(companyEntity.PhysicalCountryId);
            company.PhysicalStatename = await _employeesRepository.GetStateNameBySecondaryStateId(companyEntity.PhysicalAddressState);
            company.PhysicalCityname = await _employeesRepository.GetCityNameBySecondaryCityId(companyEntity.PhysicalAddressCity);
            company.MailingCountryname = await _employeesRepository.GetCountryNameBySecondaryCountryId(companyEntity.MailingCountryId);
            company.MailingStatename = await _employeesRepository.GetStateNameBySecondaryStateId(companyEntity.MailingAddressState);
            company.MailingCityname = await _employeesRepository.GetCityNameBySecondaryCityId(companyEntity.MailingAddressCity);
            company.CompanyLogo = string.IsNullOrEmpty(company.CompanyLogo) ? company.CompanyLogo : company.CompanyLogo;
            return company;
        }


        /// <summary>
        /// Logic to get states list
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
        /// Logic to get the state detail by particular StateId 
        /// </summary> 
        /// <param name="StateId" ></param>
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
        /// Logic to get the delete company detail by particular CompanyId 
        /// </summary> 
        /// <param name="CompanyId" ></param>
        public async Task<bool> DeleteCompany(int CompanyId)
        {
            var result = await _companyRepository.DeleteCompany(CompanyId);
            return result;
        }

        /// <summary>
        /// Logic to get branch location list
        /// </summary> 
        public async Task<BranchLocationViewModel> GetAllBranchLocation(int companyId)
        {            
            var branchLocationViewModel = new BranchLocationViewModel();
            branchLocationViewModel.BranchLocation = await _companyRepository.GetAllBranchLocations(companyId);
            return branchLocationViewModel;
        }

        /// <summary>
        /// Logic to get the create branchLocation detail
        /// </summary> 
        /// <param name="branchLocation" ></param>       
        public async Task<BranchLocation>Create (BranchLocation branchLocation, int companyId)
        {
            var branchLocations = new BranchLocation ();
            var totalbranchLocationName = await _companyRepository.GetBranchLocationName(branchLocation.BranchLocationName, companyId);
            if(totalbranchLocationName == 0)
            {
                if (branchLocation?.BranchLocationId == 0)
                {
                    var branchLocationEntity = _mapper.Map<BranchLocationEntity>(branchLocation);
                    branchLocationEntity.IsActive = true;
                    var datas = await _companyRepository.Create(branchLocationEntity, companyId);
                    branchLocations.BranchLocationId = branchLocationEntity.BranchLocationId;
                }
            }
            else
            {
                branchLocations.BranchLocationNameCount = totalbranchLocationName;
            }
            return branchLocations;
        }

        /// <summary>
        /// Logic to get check branchLocationName the branchLocation detail  by particular branchLocationName not allow repeated branchLocationName
        /// </summary> 
        /// <param name="branchLocation" ></param>
        public async Task<int> GetBranchLocationName(string branchLocationName, int companyId)
        {
            var totalbranchLocationName = await _companyRepository.GetBranchLocationName(branchLocationName, companyId);
            return totalbranchLocationName;
        }

        /// <summary>
        /// Logic to get check status the branchLocation detail by particular branchLocation status 
        /// </summary> 
        /// <param name="branchLocation" ></param>
        public async Task<int> UpdateBranchStatus(BranchLocation branchLocation)
        {
            var branchLocationEntity = _mapper.Map<BranchLocationEntity>(branchLocation);
            await _companyRepository.UpdateBranchStatus(branchLocationEntity);
            var result = branchLocationEntity.BranchLocationId;
            return result;
        }

        /// <summary>
        /// Logic to get the update branchLocation detail by particular branchLocation
        /// </summary> 
        /// <param name="branchLocation" ></param>
        public async Task<int> UpdateBranchLocation(BranchLocation branchLocation, int companyId)
        {
            var branchLocationEntity = _mapper.Map<BranchLocationEntity>(branchLocation);
            await _companyRepository.UpdateBranchLocation(branchLocationEntity, companyId);
            var branchLocations = branchLocationEntity.BranchLocationId;
            return branchLocations;
        }

        /// <summary>
        /// Logic to get the delete branchLocation detail by particular branchLocationId
        /// </summary> 
        /// <param name="branchLocationId" ></param>
        public async Task<bool> DeletedBranch(int branchLocationId, int companyId)
        {
            var result = await _companyRepository.DeletedBranch(branchLocationId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get the check status companyemailsetting detail by particular companyemailsetting id
        /// </summary> 
        /// <param name="mailScheduler" ></param>
        public async Task<bool> StatusMailSchedule(MailScheduler mailScheduler)
        {
            var designationEntity = _mapper.Map<MailSchedulerEntity>(mailScheduler);
            var result = await _companyRepository.StatusMailSchedule(designationEntity);

            return result;
        }

        /// <summary>
        /// Logic to get companyemailsetting list
        /// </summary> 
        public async Task<MailSchedulerViewModels> GetMailScheduler(int companyId)
        {
            var mailSchedulerViewModels = new MailSchedulerViewModels();
            mailSchedulerViewModels.mailSchedulerViewModel = new List<MailSchedulerViewModel>();
            var mailSchedulerList = await _companyRepository.GetAllMailSchedulerViewModel(companyId);
            if (mailSchedulerList.Count() > 0)
            {
                foreach (var item in mailSchedulerList)
                {                   
                    var model = new MailSchedulerViewModel();
                    if (item.DurationId == (int)Duration.Once)
                    {
                        model.Duration = Constant.Once;
                    }
                    else if (item.DurationId == (int)Duration.Daily)
                    {
                        model.Duration = Constant.Daily;
                    }
                    else if (item.DurationId == (int)Duration.Monthly)
                    {
                        model.Duration = Constant.Monthly;
                    }
                    else if (item.DurationId == (int)Duration.Yearly)
                    {
                        model.Duration = Constant.Yearly;
                    }
                    else if (item.DurationId == (int)Duration.Custom)
                    {
                        model.Duration = Constant.Weekly;
                    }
                    model.EmailDraftId = item.EmailDraftId;
                    model.FileFormat = item.FileFormat;
                    model.WhomToSend = item.WhomToSend;
                    model.ReportName = item.ReportName;
                    model.MailTime = Convert.ToDateTime(item.MailTime);
                    model.CompanyName = item.CompanyName;
                    model.IsActive = item.IsActive;
                    model.SchedulerId = item.SchedulerId;
                    model.CompanyId = item.CompanyId;
                    mailSchedulerViewModels.mailSchedulerViewModel.Add(model);
                };
            }
            return mailSchedulerViewModels;
        }

        /// <summary>
        /// Logic to get employees list
        /// </summary> 
        public async Task<List<DropdownEmployee>> GetAllEmployees(int companyId)
        {

            var listOfEmployees = new List<DropdownEmployee>();
            var listEmployees = await _employeesRepository.GetAllEmployeeDetails(companyId);

            listOfEmployees = new List<DropdownEmployee>();
            listOfEmployees.Add(new DropdownEmployee()
            {
                EmpId = 1000000000,
                FirstName = "Management - Management",
            });
            listOfEmployees.Add(new DropdownEmployee()
            {
                EmpId = 0,
                FirstName = "All Employees",
            });
            foreach (var item in listEmployees)
            {
                listOfEmployees.Add(new DropdownEmployee()
                {
                    EmpId = item.EmpId,
                    FirstName = item?.FirstName,
                });
            }
            return listOfEmployees;
        }

        /// <summary>
        /// Logic to get the delete mailschedule detail by particular mailschedule Id
        /// </summary> 
        /// <param name="id" ></param>
        public async Task<bool> DeletedMailSchedule(int schedulerId, int companyId)
        {
            var result = await _companyRepository.DeletedMailSchedule(schedulerId, companyId);
            return result;
        }


        /// <summary>
        /// Logic to get update companyId details the companySetting details by particular companyId
        /// </summary>
        /// <param name="companyId" ></param> 
        public async Task<CompanySetting> GetByCompanySettingId(int companyId)
        {
            var companySetting = new CompanySetting();
            var companyEntity = await _companyRepository.GetByCompanyId(companyId);
            var companySettingEntity = await _companyRepository.GetByCompanySettingId(companyId);
            if (companyEntity != null)
            {
                companySetting = _mapper.Map<CompanySetting>(companyEntity);
            }
            companySetting.CompanySettingId = companySettingEntity.CompanySettingId;
            companySetting.GSTNumber = companySettingEntity.GSTNumber;
            companySetting.TimeZone = companySettingEntity.TimeZone;
            companySetting.Currency = companySettingEntity.Currency;
            companySetting.CompanyCode = companySettingEntity.CompanyCode;
            companySetting.ModeId = companySettingEntity.ModeId;
            companySetting.Language = companySettingEntity.Language;
            companySetting.IsTimeLockEnable = companySettingEntity.IsTimeLockEnable;
            companySetting.CompanyId = companyId;
            return companySetting;
        }

        /// <summary>
        /// Logic to get create and update the companysetting details
        /// </summary>
        /// <param name="companySetting" ></param>
        public async Task<int> CreateCompanySettings(CompanySetting companySetting)
        {
            var result = 0;
            if (companySetting != null)
            {
                var companySettingEntity = await _companyRepository.GetcompanysettingId(companySetting.CompanySettingId);
                if (companySettingEntity.CompanySettingId == 0)
                {
                    var companySettingsEntity = _mapper.Map<CompanySettingEntity>(companySetting);
                    result = await _companyRepository.CreateEmailSettings(companySettingsEntity);
                    return result;
                }
                else
                {
                    var companySettingsEntity = _mapper.Map<CompanySettingEntity>(companySetting);
                    result = await _companyRepository.CreateEmailSettings(companySettingsEntity);
                    return result;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get the delete companySetting detail by particular companySettingId
         /// </summary> 
        /// <param name="companySettingId" ></param>
        public async Task<bool> DeletedCompanySetting(int companySettingId)
        {
            var result = await _companyRepository.DeletedCompanySetting(companySettingId);
            return result;
        }
        /// <summary>
        /// Logic to get all the eMailScheduler list 
        /// </summary>
        /// <param name="pager, companyId,columnName,columnDirection" ></param> 

        public async Task<MailSchedulerViewModels> GetAllEmailScheduler(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            var mailscheduler = new MailSchedulerViewModels();
            mailscheduler.emailSchedulerViewModels = await _companyRepository.GetAllEmailScheduler(pager, columnName, columnDirection, companyId);
            return mailscheduler;
        }
        /// <summary>
        /// Logic to get the company filter count
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<int> GetCompanyListCount(SysDataTablePager pager)
        {
            var companyCount = await _companyRepository.GetCompanyListCount(pager);
            return companyCount;
        }

        /// <summary>
        /// Logic to get all company details list 
        /// </summary>
        /// <param name="pager,columnDirection,columnName" ></param>
        public async Task<Company> GetAllCompanyList(SysDataTablePager pager, string columnDirection, string columnName)
        {
            var company = new Company();
            company.companyViewModels = await _companyRepository.GetCompanyDetailsList(pager, columnDirection, columnName);
            return company;

        }
        /// <summary>
        /// Logic to get count of MailScheduler 
        /// </summary>
        /// <param name="pager" ></param> 

        public async Task<int> GetAllEmailSchedulerfilterCount(SysDataTablePager pager, int companyId)
        {
            var result = await _companyRepository.GetAllEmailSchedulerfilterCount(pager, companyId);
            return result;

        }
    }
}
