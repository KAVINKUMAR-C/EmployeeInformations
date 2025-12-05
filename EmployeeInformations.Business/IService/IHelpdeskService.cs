using DocumentFormat.OpenXml.ExtendedProperties;
using EmployeeInformations.Model.HelpdeskViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IHelpdeskService
    {        
        Task<HelpdeskViewModel> GetAllHelpdesks(int companyId);
        Task<bool> DeleteHelpdesk(int id);
        Task<Helpdesk> GetAllHelpdeskViewModel(int id);
        Task<List<TicketAttachments>> GetTicketDocumentAndFilePath(int Id);
        Task<HelpdeskViewModel> GetAllHelpdeskFilter(SysDataTablePager pager, string columnDirection, string columnName,int companyId);
        Task<int> GetAllHelpdesksFilterCount(SysDataTablePager pager,int companyId);
        Task<bool> UpsertHelpdesk(HelpdeskViewModel helpdeskViewModel, int sessionEmployeeId, int companyId);
        Task<Helpdesk> ViewHelpdesk(int id, int companyId);
    }
}
