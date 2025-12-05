using EmployeeInformations.Model.EmailDraftViewModel;
using EmployeeInformations.Model.MasterViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IEmailDraftService
    {
        Task<EmailDraftTypeViewModel> GetAllEmailDraftType(int companyId);
        Task<int> CreateEmailDraftContent(EmailDraftContent emailDraftContent,int companyId);
        Task<EmailDraftContent> GetById(int Id,int companyId);
        Task<int> UpdateDraftType(EmailDraftType emailDraftType);
        Task<bool> DeletedEmailDraftType(int id);
        Task<int> GetDraftTypeName(string draftType,int companyId);
        Task<int> CreateEmailDraftType(EmailDraftType emailDraftType);
        Task<List<SendEmails>> GetAllSendEmails(int companyId);
        Task<EmailDraftTypeViewModel> GetAllEmailDraftTypes(SysDataTablePager pager, string columnName, string columnDirection,int companyId);
        Task<int> EamilDraftTypesCount(SysDataTablePager pager,int companyId);
    }
}
