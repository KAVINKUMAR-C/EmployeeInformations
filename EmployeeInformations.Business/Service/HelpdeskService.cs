using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.HelpdeskViewModel;
using EmployeeInformations.Model.PagerViewModel;



namespace EmployeeInformations.Business.Service
{
    public class HelpdeskService : IHelpdeskService
    {
        private readonly IHelpdeskRepository _helpdeskRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly IMasterRepository _masterRepository;
        private readonly ICompanyRepository _companyRepository;
        public HelpdeskService(IHelpdeskRepository helpdeskRepository, IEmployeesRepository employeesRepository, IMapper mapper, IEmailDraftRepository emailDraftRepository,
            IMasterRepository masterRepository, ICompanyRepository companyRepository)
        {
            _helpdeskRepository = helpdeskRepository;
            _employeesRepository = employeesRepository;
            _mapper = mapper;
            _emailDraftRepository = emailDraftRepository;
            _masterRepository = masterRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Logic to get all the helpdesk list
        /// </summary>
        public async Task<HelpdeskViewModel> GetAllHelpdesks(int companyId)
        {
            var helpdeskViewModel = new HelpdeskViewModel();
            helpdeskViewModel.Helpdesks = await _helpdeskRepository.GetAllHelpdesk(companyId);
            return helpdeskViewModel;
        }
        /// <summary>
        /// Logic to get all the helpdesk list
        /// </summary>
        /// <param name="pager,columnDirection,columnName" ></param>     
        public async Task<HelpdeskViewModel> GetAllHelpdeskFilter(SysDataTablePager pager, string columnDirection, string columnName,int companyId)
        {
            var helpdeskViewModel = new HelpdeskViewModel();

            helpdeskViewModel.helpDeskEntities = await _helpdeskRepository.GetAllHelpdeskDetailsByFilter(pager,columnDirection,columnName,companyId);

            return helpdeskViewModel;
        }
        /// <summary>
        /// Logic to get all the helpdesk filterCount list
        /// </summary>
        /// <param name="pager" ></param>
        public async Task<int> GetAllHelpdesksFilterCount(SysDataTablePager pager, int companyId)
        {
            var helpdeskViewModel = new HelpdeskViewModel();

            var dataCount = await _helpdeskRepository.GetAllHelpdeskDetailsByFilterCount(pager,companyId);

            return dataCount;
        }
        /// <summary>
        /// Logic to get the create and update helpdesk 
        /// </summary>
        /// <param name="helpdeskViewModel" ></param>
        /// <param name="sessionEmployeeId" ></param>   
        public async Task<bool> UpsertHelpdesk(HelpdeskViewModel helpdeskViewModel, int sessionEmployeeId,int companyId)
        {
            var result = false;
            if (helpdeskViewModel != null)
            {
                helpdeskViewModel.Description.TrimStart();
                _ = helpdeskViewModel.TicketTypeId;
                _ = helpdeskViewModel.Status;
                helpdeskViewModel.EmpId = sessionEmployeeId;
                if (helpdeskViewModel.Id == 0)
                {
                    helpdeskViewModel.CreatedBy = sessionEmployeeId;
                    helpdeskViewModel.CreatedDate = DateTime.Now;
                    var qualificationEntity = _mapper.Map<HelpdeskEntity>(helpdeskViewModel);
                    var helpdeskId = await _helpdeskRepository.UpsertHelpdesk(qualificationEntity);
                    result = true;
                    if (helpdeskViewModel.TicketAttachments != null && helpdeskViewModel.TicketAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<TicketAttachmentsEntity>();
                        foreach (var item in helpdeskViewModel.TicketAttachments)
                        {
                            var attachmentsEntity = new TicketAttachmentsEntity();
                            attachmentsEntity.HelpdeskId = helpdeskId;
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.AttachmentName = item.AttachmentName;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _helpdeskRepository.InsertTicketAttachment(attachmentsEntitys, helpdeskId);

                        var employees = await _employeesRepository.GetEmployeeByIdView(helpdeskViewModel.EmpId, companyId);
                        var draftTypeId = (int)EmailDraftType.HelpdeskTicket;
                        var emailDraftContentEntity = await _emailDraftRepository.GetByEmailDraftTypeId(draftTypeId, companyId);
                        var mailBody = EmailBodyContent.SendEmail_Body_HelpdeskTicket(employees, emailDraftContentEntity.DraftBody);
                        var ticket = await _masterRepository.GetByTicketTypeId(helpdeskViewModel.TicketTypeId, companyId);
                        var document = await _helpdeskRepository.GetTicketDocumentAndFilePath(helpdeskId);

                        string OfficeEmails = string.Empty;
                        var combinePath = new List<string>();
                        var ids = new List<int>();
                        if (ticket.ReportingPersonId != null)
                        {
                            var splitEmpId = ticket.ReportingPersonId.Split(',');
                            foreach (var data in splitEmpId)
                            {
                                ids.Add(Convert.ToInt32(data));
                            }
                            var employee = await _employeesRepository.GetTeambyId(ids,companyId);
                            foreach (var item in employee)
                            {
                                OfficeEmails += item.OfficeEmail + ",";
                            }
                        }

                        var toEmail = OfficeEmails;
                        var subject = emailDraftContentEntity.Subject;
                        var emailEntity = new EmailQueueEntity();
                        emailEntity.FromEmail = employees.OfficeEmail;
                        emailEntity.ToEmail = toEmail;
                        emailEntity.Subject = subject;
                        emailEntity.Body = mailBody;
                        foreach (var item in document)
                        {
                            if (!string.IsNullOrEmpty(item.AttachmentName))
                            {
                                var fileName = item.AttachmentName;
                                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/HelpdeskTicket");
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                combinePath.Add(Path.Combine(path, fileName));
                            }
                        }
                        var strSikillName = combinePath;
                        if (strSikillName.Count() > 0)
                        {
                            var str = string.Join(",", strSikillName);
                            emailEntity.Attachments = str.TrimEnd(',');
                        }

                        emailEntity.CCEmail = emailDraftContentEntity.Email;
                        emailEntity.IsSend = false;
                        emailEntity.Reason = Common.Constant.HelpdeskTicket;
                        emailEntity.DisplayName = emailDraftContentEntity.DisplayName;
                        emailEntity.CreatedDate = DateTime.Now;
                        await _companyRepository.InsertEmailQueueEntity(emailEntity);
                    }
                }
                else
                {
                    var helpDeskEntity = await _helpdeskRepository.GetHelpdeskByHelpdeskId(helpdeskViewModel.Id);

                    helpDeskEntity.UpdatedBy = sessionEmployeeId;
                    helpDeskEntity.UpdatedDate = DateTime.Now;
                    helpDeskEntity.Description = helpdeskViewModel.Description;
                    helpDeskEntity.TicketTypeId = helpdeskViewModel.TicketTypeId;
                    helpDeskEntity.Status = helpdeskViewModel.Status;
                    var helpdeskId = await _helpdeskRepository.UpsertHelpdesk(helpDeskEntity);
                    result = true;
                    if (helpdeskViewModel.TicketAttachments != null && helpdeskViewModel.TicketAttachments.Count() > 0)
                    {
                        var attachmentsEntitys = new List<TicketAttachmentsEntity>();
                        foreach (var item in helpdeskViewModel.TicketAttachments)
                        {
                            var attachmentsEntity = new TicketAttachmentsEntity();
                            attachmentsEntity.Document = item.Document;
                            attachmentsEntity.AttachmentName = item.AttachmentName;
                            attachmentsEntity.Id = item.Id;
                            attachmentsEntity.HelpdeskId = helpdeskViewModel.Id;
                            attachmentsEntitys.Add(attachmentsEntity);
                        }
                        result = await _helpdeskRepository.InsertTicketAttachment(attachmentsEntitys, helpdeskId);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Logic to get the delete helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<bool> DeleteHelpdesk(int id)
        {
            var qualificationEntity = await _helpdeskRepository.GetHelpdeskByHelpdeskId(id);
            qualificationEntity.IsDeleted = true;
            var result =  await _helpdeskRepository.DeleteHelpdesk(qualificationEntity);

            var qualificationAttachementEntitys = await _helpdeskRepository.GetTicketDocumentAndFilePath(id);
            foreach (var item in qualificationAttachementEntitys)
            {
                item.IsDeleted = true;
            }

            await _helpdeskRepository.DeleteTicketAttachement(qualificationAttachementEntitys);
            return result;
        }

        /// <summary>
        /// Logic to get the helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<Helpdesk> GetAllHelpdeskViewModel(int id)
        {
            var helpdesk = new Helpdesk();

            var helpdeskEntity = await _helpdeskRepository.GetByhelpId(id);
            if (helpdeskEntity != null)
            {
                helpdesk = _mapper.Map<Helpdesk>(helpdeskEntity);
            }
            var document = await _helpdeskRepository.GetTicketDocumentAndFilePath(helpdesk.Id);
            if (document.Count() > 0)
            {
                helpdesk.TicketAttachments = new List<TicketAttachments>();
                foreach (var item in document)
                {
                    var ticketAttachments = new TicketAttachments();
                    ticketAttachments.HelpdeskId = item.HelpdeskId;
                    ticketAttachments.AttachmentName = item.AttachmentName;
                    ticketAttachments.Document = item.Document;

                    helpdesk.TicketAttachments.Add(ticketAttachments);
                }
            }
            return helpdesk;
        }

        /// <summary>
        /// Logic to get the TicketAttachments detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<List<TicketAttachments>> GetTicketDocumentAndFilePath(int Id)
        {
            var ticketAttachmentsDocumentFilePaths = new List<TicketAttachments>();
            var docNmaesAndFilePath = await _helpdeskRepository.GetTicketDocumentAndFilePath(Id);
            foreach (var item in docNmaesAndFilePath)
            {
                var ticketAttachmentsDocumentFilePath = new TicketAttachments();
                ticketAttachmentsDocumentFilePath.Document = item.Document;
                ticketAttachmentsDocumentFilePath.AttachmentName = item.AttachmentName;
                ticketAttachmentsDocumentFilePaths.Add(ticketAttachmentsDocumentFilePath);
            }
            return ticketAttachmentsDocumentFilePaths;
        }

        /// <summary>
        /// Logic to get the view helpdesk detail by particular Id
        /// </summary>
        /// <param name="id" ></param>
        public async Task<Helpdesk> ViewHelpdesk(int id,int companyId)
        {
           
            var helpdeskEntity = await _helpdeskRepository.GetByhelpId(id);
            if (helpdeskEntity == null)
            return null;

            var employee = await _employeesRepository.GetEmployeeById(helpdeskEntity.EmpId, companyId);
            var ticketType = await _masterRepository.GetByTicketTypeId(helpdeskEntity.TicketTypeId, companyId);
            var document = await _helpdeskRepository.GetTicketDocumentAndFilePath(id);

            var helpdesk = new Helpdesk
            {
                Id = helpdeskEntity.Id,
                EmpId = helpdeskEntity.EmpId,
                EmployeeName = $"{employee.FirstName} {employee.LastName}",
                Status = helpdeskEntity.Status,
                TicketTypeId = helpdeskEntity.TicketTypeId,
                TicketType = ticketType.TicketName,
                TicketStatus = ((TicketStatus)helpdeskEntity.Status).ToString(),
                Description = helpdeskEntity.Description,
                TicketAttachments = document.Select(item => new TicketAttachments
                {
                    HelpdeskId = item.HelpdeskId,
                    AttachmentName = item.AttachmentName,
                    Document = item.Document
                }).ToList()
            };

            return helpdesk;
        }
    }
}


