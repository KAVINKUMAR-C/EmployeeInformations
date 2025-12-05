using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IPermissionService
    {
        Task<List<RoleViewModel>> GetAllRoles(int companyId);

        Task<RoleAssignViewModel> GetRolePermissionByRoleId(int roleId,int companyId);

        Task<DashboardRoleAssignViewModel> GetDashboardRolePermissionByRoleId(int roleId,int companyId);

        Task<bool> AddPrivilegeByRole(List<AssignRoleView> assignRoleViews, int sessionEmployeeId,int companyId);
        Task<bool> AddDashboardPrivilegeByRole(List<AssignDashboardRoleView> assignDashboardRoleView, int sessionEmployeeId,int companyId);
        Task<bool> CreateLeavePermission(EmployeePrivilegesViewModel employeePrivileges,int companyId);
        Task<int> GetLeavePermissionCount(SysDataTablePager pager,int companyId);
        Task<List<EmployeesPrivilegesViewModel>> GetLeavePermissionView(SysDataTablePager pager, string columnName, string columnDirection,int companyId);
        Task<List<EmployeesPrivileges>> GetAllEmployeePrivileges();
    }
}
