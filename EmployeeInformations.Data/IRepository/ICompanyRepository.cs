using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.PagerViewModel;
using System.ComponentModel.Design;

namespace EmployeeInformations.Data.IRepository
{
    public interface ICompanyRepository
    {
        Task<List<CompanyEntity>> GetAllCompany();
        Task<int> CreateCompany(CompanyEntity companyEntity);
        Task<CompanyEntity> GetByCompanyId(int companyId);
        Task<bool> DeleteCompany(int CompanyId);
        Task<int> GetProbationMonth();
        Task<EmailSettingsEntity> GetEmailSettingsEntity();
        Task<bool> InsertEmailQueueEntity(EmailQueueEntity emailQueueEntity);
        Task<List<BranchLocationEntity>> GetAllBranchLocation(int companyId);
        Task<List<EmailQueueEntity>> GetEmailQueueEntity();
        Task<EmployeeSettingEntity> GetEmployeeSetting();
        Task<List<EmailQueueEntity>> GetEmailQueueEntity(int companyId);
        Task<EmailSettingsEntity> GetEmailSettingsEntity(int companyId);
        Task<List<Company>> GetAllCompanys();
        Task<List<MailSchedulerViewModel>> GetAllMailSchedulerViewModel(int companyId);
        Task<int> Create(BranchLocationEntity branchLocationEntity,int companyId);
        Task<int> GetBranchLocationName(string branchLocationName,int companyId);
        Task UpdateBranchStatus(BranchLocationEntity branchLocationEntity);
        Task<bool> DeletedBranch(int branchLocationId,int companyId);
        Task<bool> DeletedMailSchedule(int schedulerId,int companyId);
        Task<bool> StatusMailSchedule(MailSchedulerEntity mailSchedulerEntity);
        Task UpdateBranchLocation(BranchLocationEntity branchLocationEntity,int companyId);
        Task<bool> CreateMailScheduler(MailSchedulerEntity mailSchedulerEntity);
        Task<MailSchedulerEntity> GetMailSchedulerBySchedulerId(int schedulerId);   
        Task<CompanySettingEntity> GetByCompanySettingId(int CompanyId);
        Task<int> CreateEmailSettings(CompanySettingEntity companySettingEntiy);
        Task<CompanySettingEntity> GetcompanysettingId(int companySettingId);
        Task<bool> DeletedCompanySetting(int companySettingId);
        Task<bool> UpdateEmailQueue(int emailQueueId, bool isSend);
        Task<List<BranchLocation>> GetAllBranchLocations(int companyId);
        Task<int> GetCompanyListCount(SysDataTablePager pager);
        Task<List<CompanyViewModels>> GetCompanyDetailsList(SysDataTablePager pager, string columnDirection, string columnName);
        Task<List<EmailSchedulerViewModel>> GetAllEmailScheduler(SysDataTablePager pager, string columnName, string columnDirection,int companyId);
        Task<int> GetAllEmailSchedulerfilterCount(SysDataTablePager pager,int companyId);
        Task<CompanySettingEntity> GetAllCompanySetting(int companyId);
        Task<int> GetMaxCountOfEmployeesByCompanyId(int companyId);

    }
}
