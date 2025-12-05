using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmailDraftViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IEmailDraftRepository
    {
        Task<List<EmailDraftTypeEntity>> GetAllEmailDraftType();
        Task<EmailDraftContentEntity> GetByEmailDraftTypeId(int Id,int companyId);
        Task<int> CreateEmailDraftContent(EmailDraftContentEntity emailDraftContentEntity,int companyId);
        Task<EmailDraftTypeEntity> GetById(int Id);
        Task<EmailDraftContentEntity> GetEmailTrapTypeId(int emailDraftTypeId,int companyId);
        Task UpdateDraftType(EmailDraftTypeEntity emailDraftTypeEntity);
        Task<bool> DeletedEmailDraftType(int id);
        Task<int> GetDraftTypeName(string draftType,int companyId);
        Task<int> CreateEmailDraftType(EmailDraftTypeEntity emailDraftTypeEntity);
        Task<EmailDraftContentEntity> GetByEmailDraftType(int Id);
        Task<List<EmailDraftType>> GetAllEmailDraftTypes(int companyId);
        Task<List<EmailDraftEntites>> GetAllEmailDraftTypesByPagination(SysDataTablePager pager, string columnName, string columnDirection, int companyId);
        Task<int> GetAllEmailDraftTypesCountByPagination(SysDataTablePager pager, int companyId);

    }
}
