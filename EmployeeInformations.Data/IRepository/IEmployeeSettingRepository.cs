using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.EmployeeSettingViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IEmployeeSettingRepository
    {        
        Task<bool> CreateSetting(EmployeeSettingEntity entity,int companyId);
        Task<EmployeeSetting> GetEmployeeSetting(int companyId);
    }
}
