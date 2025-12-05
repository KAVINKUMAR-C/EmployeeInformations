using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IClientService
    {
        Task<List<ClientViewModel>> GetAllClient(int companyId);
        Task<ClientViewModel> GetByClientId(int ClientId,int companyId);
        Task<int> CreateClient(ClientViewModel client, int sessionEmployeeId,int companyId);
        Task<bool> DeleteClient(int ClientId,int companyId);
        Task<ClientViewModel> GetByViewClientId(int ClientId,int companyId);
        Task<int> ClientViewCount(int companyId, SysDataTablePager pager);
        Task<List<ClientFilterViewModel>> GetClientFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);
    }
}
