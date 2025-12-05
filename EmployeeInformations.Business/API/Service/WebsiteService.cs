using AutoMapper;
using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.APIModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.API.IRepository;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.APIModel;
using EmployeeInformations.Model.EmployeesViewModel;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using EmployeeInformations.Common;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.CoreModels.DataViewModel;


namespace EmployeeInformations.Business.API.Service
{
    public class WebsiteService : IWebsiteService
    {
        private readonly IWebsiteRepository _websiteRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IHttpClientFactory _clientFactory;

        public WebsiteService(IWebsiteRepository websiteRepository, IConfiguration config, IMapper mapper, IEmployeesRepository employeesRepository, ICompanyRepository companyRepository, IEmailDraftRepository emailDraftRepository, IHttpClientFactory clientFactory)
        {
            _websiteRepository = websiteRepository;
            _config = config;
            _mapper = mapper;
            _employeesRepository = employeesRepository;
            _companyRepository = companyRepository;
            _emailDraftRepository = emailDraftRepository;
            _clientFactory = clientFactory;
        }

        public async Task<UserResponseModel> InsertContactUsReauest(ContactUsRequestModel model)
        {
            var contactUsResponseModel = new UserResponseModel();
            var contactUsEntity = _mapper.Map<WebsiteContactUsEntity>(model);
            await _websiteRepository.InsertContactUsReauest(contactUsEntity);

            var mailBody = EmailBodyContent.SendEmail_Body_WebsiteContactUs(contactUsEntity);
            var toEmail = _config.GetSection(Common.Constant.vphospitalInfoMailId).Value.ToString();
            var mngt = _config.GetSection(Common.Constant.vphospitalSupportEmailId).Value.ToString();
            var subject = Common.Constant.Welcometovphospital;
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = toEmail;
            emailEntity.Subject = subject;
            emailEntity.CCEmail = mngt;
            emailEntity.Body = mailBody;
            emailEntity.IsSend = false;
            emailEntity.Reason = Common.Constant.ContactUsReason;
            emailEntity.DisplayName = Common.Constant.ContactDisplayName;
            emailEntity.CreatedDate = DateTime.Now;
            var email = await _companyRepository.InsertEmailQueueEntity(emailEntity);

            // Common.Common.SendEmail(emailSettingsEntity, mailBody, toEmail, subject, displayName);
            contactUsResponseModel.IsSuccess = true;
            contactUsResponseModel.Message = Common.Constant.Success;
            return contactUsResponseModel;
        }

        public async Task<UserResponseModel> InsertJobPostReauest(JobPostRequestModel model)
        {
            var contactUsResponseModel = new UserResponseModel();
            if (model != null)
            {
                var result = await _websiteRepository.InsertJobPostReauest(model);
                var jobPostApplyName = await _websiteRepository.GetJobPostByJobId(model.JobId);
                if (jobPostApplyName != null)
                {
                    MailMessage mailMessage = new MailMessage();
                    var jobName = jobPostApplyName.JobName;
                    var toEmail = _config.GetSection(Common.Constant.vphospitalHrEmailId).Value.ToString();
                    var ccEmail = _config.GetSection(Common.Constant.vphospitalSupportEmailId).Value.ToString();
                    var subject = Common.Constant.Applyingfor + jobName;
                    var mailBody = EmailBodyContent.SendEmail_Body_WebsiteJobPostRequest(model.FullName, model.Email, model.Experience);
                    var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
                    var emailEntity = new EmailQueueEntity();
                    emailEntity.FromEmail = emailSettingsEntity.FromEmail;
                    emailEntity.ToEmail = toEmail;
                    emailEntity.Subject = subject;
                    emailEntity.Body = mailBody;
                    emailEntity.CCEmail = ccEmail;
                    var combinePath = "";
                    if (!string.IsNullOrEmpty(model.Base64string))
                    {
                        var fileFormat = model.FileFormat;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/WebSiteJobApply");
                        //create folder if not exist
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var fileName = result.FilePath + Path.GetExtension(fileFormat);
                        combinePath = Path.Combine(path, fileName);
                    }
                    emailEntity.Attachments = combinePath;
                    emailEntity.IsSend = false;
                    emailEntity.Reason = Common.Constant.JobPostReason;
                    emailEntity.DisplayName = Common.Constant.JobPostDisplayName;
                    emailEntity.CreatedDate = DateTime.Now;
                    var email = await _companyRepository.InsertEmailQueueEntity(emailEntity);
                    // Common.Common.SendEmail(emailSettingsEntity, mailBody, toEmail, subject, displayName);

                    contactUsResponseModel.IsSuccess = true;
                    contactUsResponseModel.Message = Common.Constant.Success;
                }
                else
                {
                    contactUsResponseModel.IsSuccess = false;
                    contactUsResponseModel.Message = Common.Constant.Failure;
                }
            }
            return contactUsResponseModel;
        }

        public async Task<JobPostResponseModel> GetJobPostByJobId(int JobId)
        {
            var jobPostEntityModel = await _websiteRepository.GetJobPostByJobId(JobId);
            var jobPostResponseModel = _mapper.Map<JobPostResponseModel>(jobPostEntityModel);
            return jobPostResponseModel;
        }

        public async Task<List<JobPostResponseModel>> GetAllJobPost()
        {
            var jobPostResponseModel = await _websiteRepository.GetAllJobPost();
            return jobPostResponseModel;
        }

        public async Task<List<WebsiteLeadTypeResponseModel>> GetWebsiteLeads()
        {
            var websiteLeadTypeResponseModels = await _websiteRepository.GetWebsiteLeads();
            return websiteLeadTypeResponseModels;
        }

        public async Task<List<WebsiteServicesResponseModel>> GetWebsiteServices()
        {
            var websiteServicesResponseModels = await _websiteRepository.GetWebsiteServices();
            return websiteServicesResponseModels;
        }

        public async Task<UserResponseModel> InsertQuoteRequest(WebsiteQuoteRequestModel websiteQuoteRequestModel)
        {
            var userResponseModel = new UserResponseModel();
            var eRFNumber = await _websiteRepository.GetMaxServiceCount();
            var websiteQuoteEntity = _mapper.Map<WebsiteQuoteEntity>(websiteQuoteRequestModel);
            websiteQuoteEntity.ERFN = eRFNumber;
            websiteQuoteEntity.ServicesId = string.Join(",", websiteQuoteRequestModel.ServicesId);
            var combinePath = "";
            if (!string.IsNullOrEmpty(websiteQuoteRequestModel.Base64string))
            {
                var fileFormat = websiteQuoteRequestModel.FileFormat;
                var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "WebsiteQuote");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                combinePath = Path.Combine(path, fileName);

                Byte[] bytes = Convert.FromBase64String(websiteQuoteRequestModel.Base64string);
                SaveByteArrayToFileWithFileStream(bytes, combinePath);
            }

            websiteQuoteEntity.FilePath = combinePath.Replace(Directory.GetCurrentDirectory(), "~");
            websiteQuoteEntity.ApplyDate = DateTime.Now;

            var quoteId = await _websiteRepository.InsertQuoteRequest(websiteQuoteEntity);
            var serviceTypeName = string.Empty;
            foreach (var item in websiteQuoteRequestModel.ServicesId)
            {
                var serviceId = Convert.ToInt16(item);
                var serviceType = await _websiteRepository.GetWebsiteServiceType(serviceId);
                serviceTypeName += serviceType + ",";
            }
            serviceTypeName.Trim(new char[] { ',' });

            var displayName = Common.Constant.QuoteRequestDisplayName;
            var mailBody = EmailBodyContent.SendEmail_Body_WebsiteInsertQuoteRequest(websiteQuoteEntity, serviceTypeName);
            var toEmail = _config.GetSection(Common.Constant.vphospitalSupportEmailId).Value.ToString();
            var subject = Common.Constant.EnquirySupport;
            Common.Common.SendQuoteAndConsultantEmail(_config, mailBody, toEmail, subject, displayName);

            var toEmailEnquiryUser = websiteQuoteRequestModel.Email;
            var enquiryUserName = websiteQuoteRequestModel.FirstName + " " + websiteQuoteRequestModel.LastName;
            var mailEnquiryBody = EmailBodyContent.SendEmail_Body_WebsiteInsertQuote_SenderRequest(enquiryUserName, eRFNumber);
            Common.Common.SendQuoteAndConsultantEmail(_config, mailEnquiryBody, toEmailEnquiryUser, subject, displayName);

            userResponseModel.IsSuccess = true;
            userResponseModel.Message = Common.Constant.Success;
            userResponseModel.QuoteId = quoteId;
            return userResponseModel;
        }

        public async Task<List<WebsiteSurveyResponseModel>> GetWebsiteSurveyQuestionsRequest()
        {
            var websiteSurveyQuestionEntity = await _websiteRepository.GetWebsiteSurveyQuestions();
            var websiteSurveyQuestionOptionEntity = await _websiteRepository.GetWebsiteSurveyQuestionOptions();
            var websiteSurveyResponseModels = new List<WebsiteSurveyResponseModel>();

            foreach (var item in websiteSurveyQuestionEntity)
            {
                var websiteSurveyOptionsModel = new WebsiteSurveyOptionsModel();
                var websiteSurveyResponseModel = new WebsiteSurveyResponseModel();
                websiteSurveyResponseModel.SurveyQuestionName = item.SurveyQuestionName;
                // option
                var websiteSurveyQuestionOption = websiteSurveyQuestionOptionEntity.FirstOrDefault(x => x.SurveyQuestionId == item.SurveyQuestionId);
                if (websiteSurveyQuestionOption != null)
                {
                    websiteSurveyOptionsModel.SurveyQuestionId = websiteSurveyQuestionOption.SurveyQuestionId;
                    websiteSurveyOptionsModel.SurveyQuestionOptionName = websiteSurveyQuestionOption.SurveyQuestionOptionName;
                    websiteSurveyOptionsModel.SurveyOptionId = websiteSurveyQuestionOption.SurveyOptionId;

                }
                websiteSurveyResponseModel.WebsiteSurveyOptionsModels = websiteSurveyOptionsModel;
                websiteSurveyResponseModels.Add(websiteSurveyResponseModel);
            }

            return websiteSurveyResponseModels;
        }

        public async Task<UserResponseModel> InsertSoftwareConsultProfileRequest(WebsiteSurveyRequestModel websiteQuoteRequestModel)
        {
            var userResponseModel = new UserResponseModel();
            var eRFNumber = await _websiteRepository.GetMaxServiceCount();
            var websiteQuoteEntity = _mapper.Map<WebsiteQuoteEntity>(websiteQuoteRequestModel);
            websiteQuoteEntity.ERFN = eRFNumber;

            //var combinePath = "";
            //if (!string.IsNullOrEmpty(websiteQuoteRequestModel.Base64string))
            //{
            //    var fileFormat = websiteQuoteRequestModel.FileFormat;
            //    var fileName = Guid.NewGuid().ToString() + "." + fileFormat;
            //    var path = Path.Combine(Directory.GetCurrentDirectory(), "WebsiteSoftwareConsult");

            //    if (!Directory.Exists(path))
            //        Directory.CreateDirectory(path);
            //    combinePath = Path.Combine(path, fileName);

            //    Byte[] bytes = Convert.FromBase64String(websiteQuoteRequestModel.Base64string);
            //    SaveByteArrayToFileWithFileStream(bytes, combinePath);
            //}

            //websiteQuoteEntity.FilePath = combinePath.Replace(Directory.GetCurrentDirectory(), "~");
            websiteQuoteEntity.ApplyDate = DateTime.Now;

            var quoteId = await _websiteRepository.InsertQuoteRequest(websiteQuoteEntity);
            var serviceTypeName = Common.Constant.SoftwareConsultServiceType;
            var displayName = Common.Constant.SoftwareConsultDisplayName;
            var mailBody = EmailBodyContent.SendEmail_Body_WebsiteInsertQuoteRequest(websiteQuoteEntity, serviceTypeName);
            var toEmail = _config.GetSection(Common.Constant.vphospitalSupportEmailId).Value.ToString();
            var subject = Common.Constant.EnquirySoftwareConsultSupport;
            Common.Common.SendQuoteAndConsultantEmail(_config, mailBody, toEmail, subject, displayName);

            var toEmailEnquiryUser = websiteQuoteRequestModel.Email;
            var enquiryUserName = websiteQuoteRequestModel.FirstName;
            var mailEnquiryBody = EmailBodyContent.SendEmail_Body_WebsiteInsertQuote_SenderRequest(enquiryUserName, eRFNumber);
            Common.Common.SendQuoteAndConsultantEmail(_config, mailBody, toEmail, subject, displayName);

            userResponseModel.IsSuccess = true;
            userResponseModel.Message = Common.Constant.Success;
            userResponseModel.QuoteId = quoteId;
            return userResponseModel;
        }

        public async Task<UserResponseModel> InsertSoftwareConsultSurveyRequest(List<SurveyAnswerRequestModel> websiteSurveyAnswerRequestModel)
        {
            var userResponseModel = new UserResponseModel();

            var websiteSurveyAnswerEntity = _mapper.Map<List<WebsiteSurveyAnswerEntity>>(websiteSurveyAnswerRequestModel);
            var result = await _websiteRepository.InsertSoftwareConsultSurveyRequest(websiteSurveyAnswerEntity);
            if (result)
            {
                userResponseModel.IsSuccess = true;
                userResponseModel.Message = Common.Constant.Success;
                return userResponseModel;
            }
            userResponseModel.IsSuccess = false;
            userResponseModel.Message = Common.Constant.Failure;
            return userResponseModel;
        }

        public static void SaveByteArrayToFileWithFileStream(byte[] data, string filePath)
        {
            using var stream = File.Create(filePath);
            stream.Write(data, 0, data.Length);
        }
        /// <summary>
        /// Logic to get WebsiteJobPost data total count of the employees
        /// </summary>
        ///  <param name="pager"></param>
        public async Task<int> WebsiteJobPostCount(SysDataTablePager pager)
        {
            var WebsiteJobPostcount = await _websiteRepository.WebsiteJobPostCount(pager);
            return WebsiteJobPostcount;
        }

        /// <summary>
        /// Logic to get  WebsiteJobPost Filter data of the employees
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        public async Task<List<WebsiteJobPostEntity>> WebsiteJobPostFilter(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var WebsiteJobPostFilter = await _websiteRepository.WebsiteJobPostFilter(pager, columnName, columnDirection);
            return WebsiteJobPostFilter;

        }
        public async Task<string> GetWebsiteJobPostId()
        {
            var totalWebsiteJobPostCount = await _websiteRepository.GetWebsiteJobPostMaxCount();
            var userName = Common.Constant.JC + (totalWebsiteJobPostCount + 1).ToString("D3");
            return userName;
        }

        public async Task<List<WebsiteJobViewModel>> GetAllWebsiteJobPost()
        {
            var websiteJobentityModel = await _websiteRepository.GetAllWebsiteJobPost();
            var websiteJobViewModels = _mapper.Map<List<WebsiteJobViewModel>>(websiteJobentityModel);
            websiteJobViewModels = websiteJobViewModels.OrderByDescending(x => x.JobPostedDate).ToList();
            return websiteJobViewModels;
        }

        public async Task<WebsiteJobViewModel> GetWebsiteJobPostByJobId(int jobId)
        {
            var websiteJobentityModel = await _websiteRepository.GetWebsiteJobPostByJobId(jobId);
            var websiteJobViewModels = _mapper.Map<WebsiteJobViewModel>(websiteJobentityModel);
            return websiteJobViewModels;
        }

        public async Task<bool> InsertWebsiteJobPost(WebsiteJobViewModel websiteJobViewModel)
        {
            var str = String.Join(",", websiteJobViewModel.SkillNames);
            websiteJobViewModel.SkillName = str;
            var websiteJobEntityModels = _mapper.Map<WebsiteJobsEntity>(websiteJobViewModel);
            var result = await _websiteRepository.InsertWebsiteJobPost(websiteJobEntityModels);
            return result;
        }

        public async Task<bool> CreateSkills(SkillSet skill)
        {
            var result = false;
            var websiteJobEntityModels = _mapper.Map<SkillSetEntity>(skill);
            result = await _websiteRepository.CreateSkills(websiteJobEntityModels);
            return result;
        }

        public async Task<int> GetSkill(string SkillName)
        {
            var totalEmployeeCount = await _websiteRepository.GetSkill(SkillName);
            return totalEmployeeCount;
        }

        public async Task<bool> DeleteWebsiteJobPost(int jobId)
        {
            if (jobId > 0)
            {
                _websiteRepository.DeleteWebsiteJobPost(jobId);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeStatusWebsiteJobPost(int jobId, int status, int companyId)
        {
            if (jobId > 0)
            {
                _websiteRepository.ChangeStatusWebsiteJobPost(jobId, status);
                var jobPostApplyName = await _websiteRepository.GetJobPostByJobId(jobId);
                if (status == 2)
                {
                    var draftTypeId = (int)EmailDraftType.JobPostActive;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var bodyContent = EmailBodyContent.SendEmail_Body_JobPostStatus(jobPostApplyName.JobName, emailDraftContentEntity.DraftBody);
                    await InsertEmailJobPostActive(emailDraftContentEntity, bodyContent);
                }
                else
                {
                    var draftTypeId = (int)EmailDraftType.JobPostDeactive;
                    var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                    var bodyContent = EmailBodyContent.SendEmail_Body_JobPostStatus(jobPostApplyName.JobName, emailDraftContentEntity.DraftBody);
                    await InsertEmailJobPostDeactive(emailDraftContentEntity, bodyContent);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task InsertEmailJobPostActive(EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = emailDraftContentEntity.Email;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.JobPostActive;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        private async Task InsertEmailJobPostDeactive(EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = emailDraftContentEntity.Email;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.JobPostDeactive;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }

        public async Task<List<WebsiteJobApplyModel>> GetAllJobApply()
        {
            var websiteJobApplyModel = new List<WebsiteJobApplyModel>();
            var websiteJobApplyModels = await _websiteRepository.GetAllJobApply();
            //websiteJobApplyModel =_mapper.Map<List<WebsiteJobApplyModel>>(websiteJobApplyModels);
            int count = 1;
            foreach (var item in websiteJobApplyModels)
            {
                var jobName = await _websiteRepository.GetWebsiteJobPostByJobId(item.JobId);
                count = count > 5 ? 1 : count;
                websiteJobApplyModel.Add(new WebsiteJobApplyModel()
                {
                    JobApplyId = item.JobApplyId,
                    JobId = item.JobId,
                    JobName = jobName != null ? jobName.JobName : "",
                    FullName = item.FullName,
                    SortFullName = !string.IsNullOrEmpty(item.FullName) ? item.FullName.Substring(0, 1) : string.Empty,
                    Email = item.Email,
                    Experience = item.Experience,
                    SkillSet = item.SkillSet,
                    RelevantExperience = item.RelevantExperience,
                    FilePath = item.FilePath,
                    ApplyDate = Convert.ToDateTime(item.ApplyDate),
                    FileName = !string.IsNullOrEmpty(item.FilePath) ? item.FilePath : string.Empty,
                    File = string.IsNullOrEmpty(item.FilePath) ? "" : item.FilePath.Substring(item.FilePath.LastIndexOf("/") + 1),
                    FileFormat = item.FilePath,
                    ClassName = Common.Common.GetClassNameForGrid(count),
                    IsActive = item.IsActive,
                    IsRecordForm = item.IsRecordForm,

                });
                count++;
            }
            return websiteJobApplyModel;
        }

        public async Task<List<WebsiteQuickQuoteViewModel>> GetAllWebsiteQuote()
        {
            var listQuickQuote = new List<WebsiteQuickQuoteViewModel>();
            var listQuick = await _websiteRepository.GetAllWebsiteQuote();
            if (listQuick != null)
            {
                listQuickQuote = _mapper.Map<List<WebsiteQuickQuoteViewModel>>(listQuick);
            }

            var allProposalNames = await _websiteRepository.GetAllWebsiteProposal();
            var allServicesName = await _websiteRepository.GetAllWebsiteServices();
            foreach (var item in listQuickQuote)
            {
                item.FilepathName = string.IsNullOrEmpty(item.FilePath) ? "" : item.FilePath.Substring(item.FilePath.LastIndexOf('\\') + 1);
                var proposalTypeEntity = allProposalNames.FirstOrDefault(y => y.ProposalTypeId == item.ProposalTypeId);
                item.ProposalName = proposalTypeEntity != null ? proposalTypeEntity.ProposalTypeName : string.Empty;
                var serviceNames = GetServiceName(item.ServicesId, allServicesName);
                item.ServicesName = serviceNames != null ? serviceNames : string.Empty;
            }
            return listQuickQuote;
        }

        public string GetServiceName(string servicesId, List<WebsiteServicesEntity> allServicesName)
        {
            string serviceNames = string.Empty;
            if (!string.IsNullOrEmpty(servicesId))
            {
                var splitServicesId = servicesId.Split(',');
                foreach (var data in splitServicesId)
                {
                    var serviceId = Convert.ToInt32(data);
                    var value = allServicesName.FirstOrDefault(r => r.ServicesId == serviceId);
                    if (value != null)
                    {
                        serviceNames += value.ServiceName + ",";
                    }
                }
            }
            return serviceNames.Trim(new char[] { ',' });
        }

        public async Task<List<WebsiteSoftwareConsultant>> GetAllWebsiteSurveyAnswer()
        {
            var listSurveyAnswer = new List<WebsiteSoftwareConsultant>();
            var listSurvey = await _websiteRepository.GetAllWebsiteSurveyAnswer();
            if (listSurvey != null)
            {
                listSurveyAnswer = _mapper.Map<List<WebsiteSoftwareConsultant>>(listSurvey);
            }

            var allSurveyQuestion = await _websiteRepository.GetAllSurveyQuestion();
            var allQuestionOption = await _websiteRepository.GetAllSurveyQuestionOption();
            var allQuote = await _websiteRepository.GetAllWebsiteQuote();
            foreach (var item in listSurveyAnswer)
            {
                var surveyQuestionEntity = allSurveyQuestion.FirstOrDefault(h => h.SurveyQuestionId == item.SurveyQuestionId);
                item.QuestionName = surveyQuestionEntity != null ? surveyQuestionEntity.SurveyQuestionName : string.Empty;
                var surveyQuestionOptionEntity = allQuestionOption.FirstOrDefault(l => l.SurveyOptionId == item.SurveyOptionId);
                item.OptionName = surveyQuestionOptionEntity != null ? surveyQuestionOptionEntity.SurveyQuestionOptionName : string.Empty;
                var quoteEntity = allQuote.FirstOrDefault(k => k.QuoteId == item.QuoteId);
                item.QuoteName = quoteEntity != null ? quoteEntity.ERFN : string.Empty;
            }

            return listSurveyAnswer;
        }

        public async Task<WebsiteJobApplyModel> GetWebsiteJobApplyId(int jobApplyId)
        {
            var websiteJobApply = new WebsiteJobApplyModel();
            var websiteJobApplyEntity = await _websiteRepository.GetWebsiteJobApplyId(jobApplyId);
            if (websiteJobApplyEntity != null)
            {
                websiteJobApply = _mapper.Map<WebsiteJobApplyModel>(websiteJobApplyEntity);
                websiteJobApply.FileName = string.IsNullOrEmpty(websiteJobApply.FilePath) ? "" : websiteJobApply.FilePath.Substring(websiteJobApply.FilePath.LastIndexOf("/") + 1);
                websiteJobApply.splitdocname = string.IsNullOrEmpty(websiteJobApply.FilePath) ? "" : websiteJobApply.FilePath.Substring(websiteJobApply.FilePath.LastIndexOf(".") + 1);
            }
            return websiteJobApply;
        }

        public async Task<WebsiteCandidateMenuModel> GetWebsiteCandidateMenus(int companyId)
        {
            var websiteCandidateMenuModel = new WebsiteCandidateMenuModel();
            websiteCandidateMenuModel.WebsiteCandidateMenus = await _websiteRepository.GetWebsiteCandidateMenus();

            foreach (var item in websiteCandidateMenuModel.WebsiteCandidateMenus)
            {
                if (item.EmployeeName != null)
                {
                    var emps = item.EmployeeName.Split(",");
                    var empList = new List<string>();
                    var empSets = "";
                    for (int i = 0; i < emps.Length; i++)
                    {
                        var empId = Convert.ToInt32(emps[i]);
                        var emp = await _employeesRepository.GetEmployeeById(empId, companyId);
                        empSets = "";

                        if (emps[i] == emp.EmpId.ToString())
                        {

                            empSets = emp.FirstName + " " + emp.LastName;
                            empList.Add(empSets);
                        }
                    }

                    string result = string.Join(", ", empList);
                    item.Employees = result.TrimEnd(',');
                }
            }

            var candidateScheduleEntitys = await _websiteRepository.GetCandidateSechudule(companyId);
            if (candidateScheduleEntitys != null)
            {
                websiteCandidateMenuModel.WebsiteCandidateSchedules = _mapper.Map<List<WebsiteCandidateSchedule>>(candidateScheduleEntitys);
            }
            return websiteCandidateMenuModel;
        }

        /// <summary>
        /// Logic to get all the WebsiteCandidateMenu details  list 
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" >company</param> 
        public async Task<List<WebsiteCandidatesMenuModel>> GetWebsiteCandidateMenu(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {            
            var websiteCandidatesMenuModel = await _websiteRepository.GetWebsiteCandidateMenu(pager, columnName, columnDirection, companyId);            
            return websiteCandidatesMenuModel;
        }

        /// <summary>
        /// Logic to get all the WebsiteCandidateMenu count
        /// </summary>
        /// <param name="pager" >company</param> 
        public async Task<int> GetWebsiteCandidateMenuCount(SysDataTablePager pager, int companyId)
        {
            return await _websiteRepository.GetWebsiteCandidateMenuCount(pager, companyId);
        }

        public async Task<List<WebsiteCandidateSchedule>> GetCandidateSechudule(int companyId)
        {
            var websiteCandidateMenuModel = new WebsiteCandidateMenuModel();
            var candidateScheduleEntitys = await _websiteRepository.GetCandidateSechudule(companyId);
            if (candidateScheduleEntitys != null)
            {
                websiteCandidateMenuModel.WebsiteCandidateSchedules = _mapper.Map<List<WebsiteCandidateSchedule>>(candidateScheduleEntitys);
            }
            return websiteCandidateMenuModel.WebsiteCandidateSchedules;
        }

        /// <summary>
        /// Logic to get WebsiteCandidateMenu selected details
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        public async Task<WebsiteCandidateMenuModel> GetWebsiteCandidateMenusDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var candidateScheduleId = 1;
            var websiteCandidateMenuModel = new WebsiteCandidateMenuModel();
            websiteCandidateMenuModel.websiteCandidateMenuModels = await _websiteRepository.GetWebsiteCandidateMenusDetails(pager, columnName, columnDirection, candidateScheduleId);
            return websiteCandidateMenuModel;
        }
        /// <summary>
        /// Logic to get WebsiteCandidateMenu rejected details
        /// </summary>
        /// <param name="pager,columnName,columnDirection"></param>
        public async Task<WebsiteCandidateMenuModel> GetWebsiteCandidateRejectedDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var candidateScheduleId = 2;
            var websiteCandidateMenuModel = new WebsiteCandidateMenuModel();
            websiteCandidateMenuModel.websiteCandidateMenuModels = await _websiteRepository.GetWebsiteCandidateMenusDetails(pager, columnName, columnDirection, candidateScheduleId);
            return websiteCandidateMenuModel;
        }
        // <summary>
        /// Logic to get WebsiteCandidateMenu selected Count
        /// </summary>
        /// <param name="pager"></param>
        public async Task<int> GetWebsiteCandidateMenusCount(SysDataTablePager pager)
        {
            var candidateScheduleId = 1;
            var result = await _websiteRepository.GetWebsiteCandidateMenusCount(pager, candidateScheduleId);
            return result;
        }
        // <summary>
        /// Logic to get WebsiteCandidateMenu rejected Count
        /// </summary>
        /// <param name="pager"></param>
        public async Task<int> RejectedCandidateCount(SysDataTablePager pager)
        {
            var candidateScheduleId = 2;
            var result = await _websiteRepository.GetWebsiteCandidateMenusCount(pager, candidateScheduleId);
            return result;
        }
        public async Task<bool> UpdateWebsiteCandidateMenu(WebsiteCandidateMenuModel websiteCandidateMenuModel)      
        {
            var result = false;

            DateTime startTime = Convert.ToDateTime(websiteCandidateMenuModel.StrScheduledDate + " " + websiteCandidateMenuModel.StrStartTime);
            DateTime endTime = Convert.ToDateTime(websiteCandidateMenuModel.StrScheduledDate + " " + websiteCandidateMenuModel.StrEndTime);
            websiteCandidateMenuModel.StartTime = startTime;
            websiteCandidateMenuModel.EndTime = endTime;
            websiteCandidateMenuModel.ScheduledDate = Convert.ToDateTime(websiteCandidateMenuModel.StrScheduledDate);

            var strEmpName = websiteCandidateMenuModel.EmployeeId;
            websiteCandidateMenuModel.EmployeeName = Convert.ToString(strEmpName);


            if (websiteCandidateMenuModel != null)
            {
                var candidateMenu = _mapper.Map<WebsiteCandidateMenuEntity>(websiteCandidateMenuModel);
                result = await _websiteRepository.UpdateWebsiteCandidateMenu(candidateMenu);
            }            
            return result;
        }
        public async Task<bool> UpdateWebsiteCandidateMenuMeetingLink(WebsiteCandidateMenuModel websiteCandidateMenu, int companyId)
        {
            var result = false;
            if (websiteCandidateMenu != null)
            {
                var candidateMenu = _mapper.Map<WebsiteCandidateMenuEntity>(websiteCandidateMenu);
                var data = await _websiteRepository.UpdateWebsiteCandidateMenuMeetinglink(candidateMenu);

            }
            var menuId = websiteCandidateMenu.CandidateMenuId;
            var candidateDetails = await _websiteRepository.GetCandidateMenuByIds(menuId);

            var draftTypeId = (int)EmailDraftType.WebSiteCandidateMenu;
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
            var StartTime = candidateDetails.StartTime?.ToString("hh:mm tt");
            var EndTime = candidateDetails.EndTime?.ToString("hh:mm tt");
            var date = candidateDetails.ScheduledDate?.ToString("dd/MM/yyyy");
            var meetingLink = candidateDetails.MeetingLink;
            if (websiteCandidateMenu.EmployeeName != null)
            {
                var emps = websiteCandidateMenu.EmployeeName.Split(",");
                var empList = new List<string>();
                var empSets = "";
                for (int i = 0; i < emps.Length; i++)
                {
                    var empId = Convert.ToInt32(emps[i]);
                    var emp = await _employeesRepository.GetEmployeeById(empId, companyId);
                    empSets = "";

                    if (emps[i] == emp.EmpId.ToString())
                    {

                        empSets = emp.FirstName + " " + emp.LastName;
                        empList.Add(empSets);
                    }
                }

                string names = string.Join(", ", empList);
                websiteCandidateMenu.Employees = names.TrimEnd(',');
            }
            var employeesName = websiteCandidateMenu.Employees;
            var mailBody = EmailBodyContent.Website_CandidateMenu(candidateDetails.FullName, candidateDetails.Email, candidateDetails.CandidateScheduleName, StartTime, EndTime, date, employeesName, meetingLink, emailDraftContentEntity.DraftBody);
            await InsertWebsiteCandidateMenu(candidateDetails.Email, emailDraftContentEntity, mailBody);
            return result;
        }
        public async Task<WebsiteJobApplyModel> CreateWebsiteJob(int companyId)
        {
            var website = new WebsiteJobApplyModel();
            var skillset = await _employeesRepository.GetAllSkills(companyId);
            var jobs = await _websiteRepository.GetAllWebsiteJobPost();
            if (skillset != null)
            {
                website.skillSets = _mapper.Map<List<SkillSet>>(skillset);
            }
            if (jobs != null)
            {
                website.WebsiteJobs = _mapper.Map<List<WebsiteJobViewModel>>(jobs);
            }
            return website;
        }
        public async Task<bool> AddSiteJopApply(WebsiteJobApplyModel websiteJobApply, int companyId)
        {
            var websiteapply = _mapper.Map<WebsiteJobApplyEntity>(websiteJobApply);
            websiteapply.IsRecordForm = "Manual";
            websiteapply.IsActive = false;
            var skillset = await _employeesRepository.GetAllSkills(companyId);
            var strSikillName = websiteJobApply.SkillSet;
            var strskil = strSikillName.Split(',');
            var skillsetlist = new List<string>();
            var skillsets = "";
            for (int i = 0; i < strskil.Length; i++)
            {
                foreach (var skills in skillset)
                {
                    skillsets = "";

                    if (strskil[i] == skills.SkillId.ToString())
                    {

                        skillsets = skills.SkillName;
                        skillsetlist.Add(skillsets);
                    }
                }
            }

            string result = string.Join(",", skillsetlist);
            websiteapply.SkillSet = result.TrimEnd(',');

            var addWebsite = await _websiteRepository.AddSiteJopApply(websiteapply);
            return addWebsite;
        }
        public async Task<int> WebSiteApplyJobIsActive(int jobapplyId, bool isActive)
        {
            var result = await _websiteRepository.WebSiteApplyJobIsActive(jobapplyId, isActive);
            var websiteJobApplyEntity = await _websiteRepository.GetWebsiteJobApplyId(jobapplyId);
            if (websiteJobApplyEntity != null)
            {
                if (isActive == true)
                {
                    var candidateMenu = _mapper.Map<WebsiteCandidateMenuEntity>(websiteJobApplyEntity);
                    result = await _websiteRepository.AddCandidateMenu(candidateMenu);
                }

            }
            return result;
        }


        public async Task<bool> UpdateWebsiteAppliedJob(WebsiteJobApplyModel websiteJobApplyModel)
        {
            var str = String.Join(",", websiteJobApplyModel.SkillNames);
            websiteJobApplyModel.SkillSet = str;
            var websiteJobEntityModels = _mapper.Map<WebsiteJobApplyEntity>(websiteJobApplyModel);
            var result = await _websiteRepository.UpdateWebsiteAppliedJob(websiteJobEntityModels);
            return result;
        }

        public async Task<List<WebsiteJobViewModel>> GetAllWebsiteJobPostName()
        {
            var websiteJobViewModels = new List<WebsiteJobViewModel>();
            var websiteJobEntitys = await _websiteRepository.GetAllWebsiteJobPost();
            if (websiteJobEntitys != null)
            {
                websiteJobViewModels = _mapper.Map<List<WebsiteJobViewModel>>(websiteJobEntitys);
            }
            return websiteJobViewModels;
        }

        public async Task<WebsiteCandidateScheduleViewModel> CandidateSchedule(int companyId)
        {
            var Candidatedetails = new WebsiteCandidateScheduleViewModel();
            var candidateEntity = await _websiteRepository.CandidateSechudule(companyId);
            if (candidateEntity != null)
            {
                Candidatedetails.websiteCandidateSchedules = _mapper.Map<List<WebsiteCandidateSchedule>>(candidateEntity);
            }
            return Candidatedetails;

        }
        public async Task<int> WebsiteUpdateCandidateStatus(WebsiteCandidateMenuModel websiteCandidateMenuModel, int companyId)
        {
            var candidateEntity = _mapper.Map<WebsiteCandidateMenuEntity>(websiteCandidateMenuModel);
            await _websiteRepository.WebsiteUpdateCandidateStatus(candidateEntity);
            var result = candidateEntity.CandidateMenuId;
            var candidateDetails = await _websiteRepository.GetCandidateMenuByIds(websiteCandidateMenuModel.CandidateMenuId);
            var draftTypeId = (int)EmailDraftType.WebSiteCandidateMenuReject;
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
            var mailBody = EmailBodyContent.Website_CandidateMenuStatus(candidateDetails.FullName, candidateDetails.Email, candidateDetails.CandidateScheduleName, candidateDetails.CandidateStatusName, emailDraftContentEntity.DraftBody);
            await InsertWebsiteCandidateMenuReject(candidateDetails.Email, emailDraftContentEntity, mailBody);
            return result;
        }

        public async Task<CandidatePrivilegesModel> GetCandidatePrivileges(int companyId)
        {
            var candidatePrivilegesModel = new CandidatePrivilegesModel();
            candidatePrivilegesModel.candidatePrivileges = await _websiteRepository.GetCandidatePrivilege(companyId);
            return candidatePrivilegesModel;
        }

        public async Task<bool> AddCandidatePrivilegeByRoleId(List<CandidatePrivilegeAssign> candidatePrivileges, int companyId)
        {
            if (candidatePrivileges != null && candidatePrivileges.Count() > 0)
            {

                var candidatePrivilegesEntitys = new List<WebsiteCandidatePrivilegesEntitys>();
                foreach (var item in candidatePrivileges)
                {
                    var candidatescheduleId = item.CandidatescheduleId;
                    await _websiteRepository.GetCandidatePrivilegeByRoleId(candidatescheduleId, Common.Constant.Delete, companyId);
                    var candidatePrivilegesEntity = new WebsiteCandidatePrivilegesEntitys();
                    candidatePrivilegesEntity.CandidatescheduleId = candidatescheduleId;
                    candidatePrivilegesEntity.IsEnabled = item.IsEnabled == Constant.ZeroStr ? false : true;
                    candidatePrivilegesEntitys.Add(candidatePrivilegesEntity);
                }
                var result = await _websiteRepository.AddCandidatePrivilegeByRoleId(candidatePrivilegesEntitys, companyId);
                return result;
            }
            return true;
        }
        public async Task<WebsiteCandidateSchedule> CreateCandidateSchedule(WebsiteCandidateSchedule websiteCandidate, int companyId)
        {
            var candidateSchedule = new WebsiteCandidateSchedule();
            var totalScheduleName = await _websiteRepository.GetCandidateName(websiteCandidate.ScheduleName, companyId);
            if (totalScheduleName == 0)
            {
                if (websiteCandidate != null)
                {
                    if (websiteCandidate.CandidateScheduleId == 0)
                    {
                        var websitecandidatemodel = _mapper.Map<WebSiteCandidateScheduleEntity>(websiteCandidate);
                        var result = await _websiteRepository.CreateWebsiteCandidateSechudule(websitecandidatemodel, companyId);
                        candidateSchedule.CandidateScheduleId = result;
                    }
                }
            }
            else
            {
                candidateSchedule.CandidateScheduleCount = totalScheduleName;
            }
            return candidateSchedule;
        }
        public async Task<int> UpdateCandidateSchedule(WebsiteCandidateSchedule websiteCandidate)
        {
            var websitecandidatemodel = _mapper.Map<WebSiteCandidateScheduleEntity>(websiteCandidate);
            var result = await _websiteRepository.UpdateWebsiteCandidateSechudule(websitecandidatemodel);
            return result;
        }
        public async Task<int> DeleteWebsiteSchedule(int candidateId)
        {
            var result = await _websiteRepository.DeleteWebsiteSchedule(candidateId);
            return result;
        }

        public async Task<WebsiteCandidateMenuModel> GetCandidateById(int candidateid, int companyId)
        {
            var menuentity = new WebsiteCandidateMenuModel();
            var candidatescheduleEntity = await _websiteRepository.GetCandidateById(candidateid, companyId);

            if (candidatescheduleEntity != null)
            {
                menuentity.WebsiteCandidateSchedules = _mapper.Map<List<WebsiteCandidateSchedule>>(candidatescheduleEntity);
            }
            return menuentity;



        }
        private async Task InsertWebsiteCandidateMenu(string Email, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = Email;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.WebSiteCandidateMenu;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);

        }

        private async Task InsertWebsiteCandidateMenuReject(string Email, EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = Email;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = Common.Constant.WebSiteCandidateMenuReject;
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.CCEmail = emailDraftContentEntity.Email;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);

        }

        public async Task<List<DropdownEmployees>> GetAllEmployees(int companyId)
        {
            var listOfEmployees = new List<DropdownEmployees>();
            var listEmployees = await _websiteRepository.GetAllEmployees(companyId);
            var employees = _mapper.Map<List<DropdownEmployees>>(listEmployees);
            foreach (var item in employees)
            {
                listOfEmployees.Add(new DropdownEmployees()
                {
                    EmpId = item.EmpId,
                    FirstName = item?.UserName + " " + "-" + " " + item?.FirstName + " " + item?.LastName,
                });

            }
            return listOfEmployees;
        }

        public async Task<WebsiteCandidateMenuModel> GetCandidateMenuById(int id)
        {
            var result = await _websiteRepository.GetCandidateMenuByIds(id);
            string dateAsString = result.StrScheduledDate;
            result.StrScheduledDate = dateAsString + " " + result.StartTime;
            var end = Convert.ToDateTime(result.EndTime);
            var start = Convert.ToDateTime(result.StartTime);
            var endDateTime = end - start;
            int hours = 0;
            if (endDateTime.Hours != null)
            {
                hours = endDateTime.Hours * 60;
            }

            var mintes = endDateTime.Minutes;
            result.Duration = hours + mintes;
            return result;
        }
        public async Task<string> CreateZoomMeeting(string accessToken, ZoomMeetingSchedule meeting)
        {
            var baseUrl = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("BaseUrl").Value);
            var client = _clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var requestContent = new StringContent(JsonConvert.SerializeObject(meeting));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync($"{baseUrl}/users/me/meetings", requestContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        public async Task<WebsiteCandidateMenuModel> WebsiteCandidateZoomMeetingSchedule(int id ,string code, int companyId)
        {
            var result = await GetCandidateMenuById(id);

            var meeting = new ZoomMeetingSchedule();

            meeting.topic = result.CandidateScheduleName;
            meeting.start_time = (DateTime)result.StartTime;
            meeting.duration = result.Duration;

            var clientIdSecret = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("Secretkey").Value);
            var clientId = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("ClientId").Value);
            var responseUrl = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("ResponseUrl").Value);
            var redirectUri = Convert.ToString(_config.GetSection("ZoomMeetingLinkGenerated").GetSection("RedirectUrl").Value);

            var client = _clientFactory.CreateClient();

            var requestContent = new FormUrlEncodedContent(new[]
            {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("redirect_uri", redirectUri),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientIdSecret)
             });

            var response = await client.PostAsync(responseUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonConvert.DeserializeObject<ZoomTokenResponse>(responseString);

            var meetingResponse = await CreateZoomMeeting(tokenResponse.access_token, meeting);

            var zoomlink = JsonConvert.DeserializeObject<ZoomLink>(meetingResponse); 
            result.MeetingLink = zoomlink.join_url;
            var updateCandidate = await UpdateWebsiteCandidateMenuMeetingLink(result, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get all WebsideJobApply details list 
        /// </summary>
        /// <param name="pager,columnDirection,columnName" ></param>
        public async Task<WebsiteJobApplyModel> GetWebsideJobApplyList(SysDataTablePager pager, string columnDirection, string columnName)
        {      
            var websiteJobApplyModel = new WebsiteJobApplyModel();
            websiteJobApplyModel.websiteJobApplyViewModels = await _websiteRepository.GetWebsideJobApplyList(pager, columnDirection, columnName);
            return websiteJobApplyModel;
        }

        /// <summary>
        /// Logic to get the WebsideJobApply filter count
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<int> GetWebsideJobApplyListCount(SysDataTablePager pager)
        {
            var websitejobApplyCount = await _websiteRepository.GetWebsideJobApplyListCount(pager);
            return websitejobApplyCount;
        }
    }
}
