using EmployeeInformations.Model.EmployeeSettingViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IEmployeeSetting
    {
        Task<EmployeeSetting> GetEmployeeSetting(int companyId);
        Task<bool> CreateSetting(EmployeeSetting employeeSetting, int sessionValue,int companyId);
    }
}
