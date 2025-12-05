using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ClientSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IClientRepository
    {
        Task<List<ClientEntity>> GetAllClient(int companyId);
        Task<int> CreateClient(ClientEntity clientEntity,int companyId);
        Task<ClientEntity> GetByClientId(int ClientId,int companyId);
        Task<bool> DeleteClient(int ClientId,int companyId);
        Task<int> GetClientMaxCount(int companyId);
        Task<List<ClientViewModel>> GetAllClients(int companyId);
        Task<int> ClientViewCount(int companyId, SysDataTablePager pager);
        Task<List<ClientFilterViewModel>> GetClientFilterView(int companyId, SysDataTablePager pager, string columnName, string columnDirection);

    }
}
