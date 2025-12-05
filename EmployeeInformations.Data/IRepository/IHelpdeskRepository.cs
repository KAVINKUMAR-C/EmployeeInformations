using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.HelpdeskViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IHelpdeskRepository
    {      
        Task<int> UpsertHelpdesk(HelpdeskEntity helpdekEntity);
        Task<bool> InsertTicketAttachment(List<TicketAttachmentsEntity> ticketAttachments, int helpdeskId);
        Task<HelpdeskEntity> GetHelpdeskByHelpdeskId(int helpdeskId);
        Task<bool> DeleteHelpdesk(HelpdeskEntity helpdeskEntity);
        Task<List<TicketAttachmentsEntity>> GetTicketDocumentAndFilePath(int helpdeskId);
        Task<bool> DeleteTicketAttachement(List<TicketAttachmentsEntity> ticketAttachmentsEntities);
        Task<List<Helpdesk>> GetAllHelpdesk(int companyId);
        Task<HelpdeskEntity> GetByhelpId(int id);
        Task<int> GetAllHelpdeskDetailsByFilterCount(SysDataTablePager pager,int companyId);
        Task<List<HelpDeskFilterEntity>> GetAllHelpdeskDetailsByFilter(SysDataTablePager pager, string columnDirection, string columnName,int companyId);
    }
}
