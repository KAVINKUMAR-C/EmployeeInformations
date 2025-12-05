using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.EmailDraftViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.Extensions.Configuration;

namespace EmployeeInformations.Business.Service
{
    public class EmailDraftService : IEmailDraftService
    {
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IMasterRepository _masterRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public EmailDraftService(IEmailDraftRepository emailDraftRepository, IMapper mapper, IConfiguration config, IMasterRepository masterRepository)
        {
            _emailDraftRepository = emailDraftRepository;
            _masterRepository = masterRepository;
            _mapper = mapper;
            _config = config;

        }

        /// <summary>
        /// Logic to get email draft type list
        /// </summary> 
        public async Task<EmailDraftTypeViewModel> GetAllEmailDraftType(int companyId)
        {           
            var listEmailDraftType = new EmailDraftTypeViewModel();
            listEmailDraftType.EmailDraftType = await _emailDraftRepository.GetAllEmailDraftTypes(companyId);                                
            return listEmailDraftType;
        }

        /// <summary>
        /// Logic to get all the emaildrafttype list using pagination
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>y> 
        public async Task<EmailDraftTypeViewModel> GetAllEmailDraftTypes(SysDataTablePager pager, string columnName, string columnDirection, int companyId)
        {
            var listEmailDraftType = new EmailDraftTypeViewModel();
            listEmailDraftType.EmailDraftPageniation = await _emailDraftRepository.GetAllEmailDraftTypesByPagination(pager, columnName, columnDirection, companyId);
            return listEmailDraftType;
        }
        /// <summary>
        /// Logic to get all the emaildrafttype count using pagination
        /// </summary>
        /// <param name="pager,columnName,columnDirection" ></param>
        public async Task<int>EamilDraftTypesCount(SysDataTablePager pager, int companyId)
        {
            var emailcount = 0;
            emailcount = await _emailDraftRepository.GetAllEmailDraftTypesCountByPagination(pager, companyId);
            return emailcount;
        }
        /// <summary>
            /// Logic to get create the emaildrafttype details 
        /// </summary>
        /// <param name="emailDraftType" ></param>
        public async Task<int> CreateEmailDraftType(EmailDraftType emailDraftType)
        {
            var result = 0;

            if (emailDraftType?.Id == 0)
            {
                var emailDraftTypeEntity = _mapper.Map<EmailDraftTypeEntity>(emailDraftType);
                emailDraftTypeEntity.Status = true;
                var datas = await _emailDraftRepository.CreateEmailDraftType(emailDraftTypeEntity);
                result = emailDraftTypeEntity.Id;
            }
            return result;
        }

        /// <summary>
        /// Logic to get update the emaildrafttype details by particular status
        /// </summary>
        /// <param name="emailDraftType" ></param> 
        public async Task<int> UpdateDraftType(EmailDraftType emailDraftType)
        {
            var emailDraftTypeEntity = _mapper.Map<EmailDraftTypeEntity>(emailDraftType);
            await _emailDraftRepository.UpdateDraftType(emailDraftTypeEntity);
            var result = emailDraftTypeEntity.Id;
            return result;
        }

        /// <summary>
        /// Logic to get delete the emaildrafttype details by particular id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<bool> DeletedEmailDraftType(int id)
        {
            var result = await _emailDraftRepository.DeletedEmailDraftType(id);
            return result;
        }

        /// <summary>
        /// Logic to get check draftType the emaildrafttype detail  by particular draftType not allow repeated draftType
        /// </summary>
        /// <param name="draftType" ></param> 
        public async Task<int> GetDraftTypeName(string draftType, int companyId)
        {
            var totaldraftTypeName = await _emailDraftRepository.GetDraftTypeName(draftType,companyId);
            return totaldraftTypeName;
        }

        /// <summary>
               /// Logic to get create and update the email draft content details
        /// </summary>
        /// <param name="emailDraftContent" ></param>
        public async Task<int> CreateEmailDraftContent(EmailDraftContent emailDraftContent, int companyId)
        {
            var result = 0;
            if (emailDraftContent != null)
            {
                var emailDraftContentEntitys = await _emailDraftRepository.GetEmailTrapTypeId(emailDraftContent.EmailDraftTypeId,companyId);
                if (emailDraftContentEntitys.EmailDraftTypeId == 0)
                {
                    var emailDraftContentEntity = _mapper.Map<EmailDraftContentEntity>(emailDraftContent);
                    var datas = await _emailDraftRepository.CreateEmailDraftContent(emailDraftContentEntity,companyId);
                    result = emailDraftContentEntity.EmailDraftTypeId;
                }
                else
                {
                    // var emailDraftContentEntity = await _emailDraftRepository.GetByEmailTrapTypeId(emailDraftContent.EmailDraftTypeId);                    
                    var mapDraftContentEntity = _mapper.Map<EmailDraftContentEntity>(emailDraftContent);
                    var strEmailName = emailDraftContent.Email;
                    if (strEmailName != null)
                    {
                        if (strEmailName.Count() > 0)
                        {
                            var str = string.Join(",", strEmailName);
                            mapDraftContentEntity.Email = str.TrimEnd(',');
                        }
                    }

                    var datas = await _emailDraftRepository.CreateEmailDraftContent(mapDraftContentEntity,companyId);

                    result = emailDraftContentEntitys.EmailDraftTypeId;
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get update id details the emaildraftcontent details by particular Id
        /// </summary>
        /// <param name="Id" ></param> 
        public async Task<EmailDraftContent> GetById(int Id, int companyId)
        {
            var emailDraftType = new EmailDraftContent();
            var emailDraftTypeEntity = await _emailDraftRepository.GetById(Id);
            var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(Id, companyId);
            if (emailDraftTypeEntity != null)
            {
                emailDraftType = _mapper.Map<EmailDraftContent>(emailDraftTypeEntity);
            }

            emailDraftType.Subject = emailDraftContentEntity.Subject;
            emailDraftType.DraftVariable = emailDraftContentEntity.DraftVariable;
            emailDraftType.DraftBody = emailDraftContentEntity.DraftBody;
            //emailDraftType.Id= emailDraftContentEntity.Id;
            emailDraftType.EmailDraftTypeName = (emailDraftTypeEntity != null ? emailDraftTypeEntity.DraftType : string.Empty);
            emailDraftType.EmailDraftTypeId = emailDraftContentEntity.EmailDraftTypeId;
            emailDraftType.DisplayName = emailDraftContentEntity.DisplayName;

            if (emailDraftContentEntity != null && !string.IsNullOrEmpty(emailDraftContentEntity.Email))
            {
                var frm = emailDraftContentEntity.Email.Split(",");
                var strFmtEmailId = "";
                var finalOutEmail = "";
                for (int i = 0; i < frm.Count(); i++)
                {
                    var b = frm[i];
                    strFmtEmailId += string.Format(b + ",");
                }
                if (!string.IsNullOrEmpty(strFmtEmailId))
                {
                    finalOutEmail = strFmtEmailId.Remove(strFmtEmailId.Length - 1, 1);
                }
                emailDraftType.strFmtEmailId = finalOutEmail;
            }
            else
            {
                emailDraftType.strFmtEmailId = string.Empty;
            }

            return emailDraftType;
        }

        /// <summary>
        /// Logic to get sendemails list
        /// </summary>
        public async Task<List<SendEmails>> GetAllSendEmails(int companyId)
        {
            var sendEmailsList = new List<SendEmails>();
            var listSendEmails = await _masterRepository.GetAllSendEmails(companyId);
            if (listSendEmails != null)
            {
                sendEmailsList = _mapper.Map<List<SendEmails>>(listSendEmails);
            }
            return sendEmailsList;
        }
    }
}
