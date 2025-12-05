using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.PrivilegeViewModel;
using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IPermissionRepository
    {    
        Task<RoleEntity> GetRolesByRoleId(int roleId,int companyId);
        Task<List<RolePrivilegesEntity>> GetRolePrivilegeByRoleId(int roleId, string delete,int companyId);
        Task<List<DashboardRolePrivilegesEntity>> GetDashboardRolePrivilegeByRoleId(int roleId, string delete,int companyId);
        Task<bool> AddPrivilegeByRoleId(List<RolePrivilegesEntity> rolePrivilegesEntitys,int companyId);
        Task<bool> AddDashboardPrivilegeByRoleId(List<DashboardRolePrivilegesEntity> dashboardRolePrivilegesEntity,int companyId);
        Task<List<ModulesEntity>> GetAllModules();
        Task<List<DashboardMenusEntity>> GetAllDashboardMenus();
        Task<List<SubModulesEntity>> GetAllSubModules();
        Task<List<RolePrivilegesEntity>> LayoutRolePermissionByRoleId(int roleId,int companyId);
        Task<List<RoleViewModel>> GetAllRoles(int companyId);
        Task<bool> CreateLeavePermission(EmployeesPrivileges employeePrivileges,int companyId);
        Task<List<EmployeesPrivilegesViewModel>> GetLeavePermissionView(SysDataTablePager pager, string columnName, string columnDirection, int companyId);
        Task<int> GetLeavePermissionCount(SysDataTablePager pager, int companyId);
        Task<List<EmployeesPrivileges>> GetAllEmployeePrivileges();
        Task<int> GetLeavePermission(EmployeesPrivileges employeePrivileges,int companyId);
    }
}
