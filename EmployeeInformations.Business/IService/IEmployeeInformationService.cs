using EmployeeInformations.CoreModels.Model;

namespace EmployeeInformations.Business.IService
{
    public interface IEmployeeInformationService
    {
        Task<List<LeaveTypesEntity>> GetLeaveTypeData();
        Task<List<int>> GetPrivilegesByRole(int roleId);
        Task<List<int>> LayoutRolePermissionByRoleId(int roleId);
        Task<int> LayoutDisplayMode(int companyId);
        Task<List<int>> DashboardRolePermissionByRoleId(int roleId);
        Task<bool> ReadAndWritePermissionByRoleId(int subModuleId, int roleId);
    }
}
