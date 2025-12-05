using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class HomeService : IHomeService
    {

        private readonly IConfiguration _config;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IHomeRepository _homeRepository;

        public HomeService(IConfiguration config, IEmailDraftRepository emailDraftRepository, ICompanyRepository companyRepository, IHomeRepository homeRepository)
        {
            _config = config;
            _emailDraftRepository = emailDraftRepository;
            _companyRepository = companyRepository;
            _homeRepository = homeRepository;
        }

        /// <summary>
        /// Logic to get create error the assetcategory detail
        /// </summary>
        /// <param name="host" ></param>
        /// <param name="path" ></param> 
        /// <param name="exmsg" ></param>  
        /// <param name="stacktrace" ></param>  
        /// <param name="sessionEmployeeId" ></param>   
        public async Task<int> createError(string host, string path, string exmsg, string stacktrace, int sessionEmployeeId, int companyId)
        {

            var applicationLog = new ApplicationLogEntity();
            applicationLog.Host = host;
            applicationLog.Path = path;
            applicationLog.ExecptionMessage = exmsg;
            applicationLog.Error = stacktrace;
            applicationLog.CreatedDate = DateTime.Now;
            applicationLog.CreatedBy = sessionEmployeeId;
            await _homeRepository.CreateApplicationLog(applicationLog);
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId((int)EmailDraftType.ErrorMessage, companyId); 
            var bodyContent = EmailBodyContent.SendEmail_Body_ErrorMessage(stacktrace, host, emailDraftContentEntity.DraftBody);
            await InsertEmailErrorMessage(emailDraftContentEntity, bodyContent);
            return 0;
        }

        /// <summary>
        /// Logic to get eamil detail
        /// </summary>
        /// <param name="emailDraftContentEntity" ></param>
        /// <param name="bodyContent" ></param>
        private async Task InsertEmailErrorMessage(EmailDraftContentEntity emailDraftContentEntity, string bodyContent)
        {
            var emailSettingsEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingsEntity.FromEmail;
            emailEntity.ToEmail = emailDraftContentEntity.Email;
            emailEntity.Subject = emailDraftContentEntity.Subject;
            emailEntity.Body = bodyContent;
            emailEntity.Reason = " ErrorMessage";
            emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            var isEmail = await _companyRepository.InsertEmailQueueEntity(emailEntity);
        }
    }


}
