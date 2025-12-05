
using EmployeeInformations.Model.CompanyViewModel;
using EmployeeInformations.Model.EmployeesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeDropdown = EmployeeInformations.Model.CompanyViewModel.DropdownEmployee;

namespace EmployeeInformations.Business.IService
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllCompany();
        Task<int> CreateCompany(Company company, int sessionEmployeeId);
        Task<Company> GetByCompanyId(int companyId);
        Task<List<State>> GetAllStates();
        Task<List<City>> GetAllCities();
        Task<List<City>> GetByStateId(int StateId);
        Task<bool> DeleteCompany(int CompanyId);
        Task<BranchLocationViewModel> GetAllBranchLocation(int companyId);        
        Task<int> GetBranchLocationName(string branchLocationName,int companyId);
        Task<int> UpdateBranchStatus(BranchLocation branchLocation);
        Task<bool> DeletedBranch(int branchLocationId,int companyId);
        Task<bool> DeletedMailSchedule(int schedulerId,int companyId);
        Task<bool> StatusMailSchedule(MailScheduler mailScheduler);
        Task<int> UpdateBranchLocation(BranchLocation branchLocation,int companyId);
        Task<MailSchedulerViewModels> GetMailScheduler(int companyId);
        Task<bool> CreateMailScheduler(MailScheduler mailScheduler, int sessionEmployeeId);
        Task<MailSchedulerViewModels> GetMailSchedulerBySchedulerId(int schedulerId);
        Task<Company> GetByViewCompanyId(int companyId);
        Task<CompanySetting> GetByCompanySettingId(int companyId);
        Task<int> CreateCompanySettings(CompanySetting companySetting);
        Task<bool> DeletedCompanySetting(int companySettingId);
        Task<BranchLocation> Create(BranchLocation branchLocation,int companyId);
        Task<int> GetCompanyListCount(SysDataTablePager pager);
        Task<Company> GetAllCompanyList(SysDataTablePager pager, string columnDirection, string columnName);
        Task<MailSchedulerViewModels>GetAllEmailScheduler(SysDataTablePager pager,  string columnName, string columnDirection,int companyId);
        Task<int> GetAllEmailSchedulerfilterCount(SysDataTablePager pager,int companyId);
        Task<List<DropdownEmployee>> GetAllEmployees(int companyId);

    }
}
