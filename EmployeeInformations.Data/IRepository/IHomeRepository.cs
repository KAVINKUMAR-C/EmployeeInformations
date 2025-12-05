using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Data.IRepository
{
    public interface IHomeRepository
    {
        Task<int> CreateApplicationLog(ApplicationLogEntity applicationLogEntity);
    }
}
