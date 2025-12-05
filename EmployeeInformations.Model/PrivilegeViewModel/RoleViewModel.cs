using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Model.PrivilegeViewModel
{
    public class RoleViewModel
    {
        public Role RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class RolePrivilegeViewModel
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int SubModuleId { get; set; }
        public string SubModuleName { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsWritable { get; set; }
    }

    public class DashboardRolePrivilegeViewModel
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        // public int ModuleId { get; set; }
        public string MenuName { get; set; }
        public int MenuId { get; set; }
        // public string SubModuleName { get; set; }
        public bool IsEnabled { get; set; }
    }

    public class RoleAssignViewModel
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        public List<List<RolePrivilegeViewModel>> RolePrivilegeViewModels { get; set; }
    }

    public class DashboardRoleAssignViewModel
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        public List<DashboardRolePrivilegeViewModel> DashboardRolePrivilegeViewModel { get; set; }
    }

    public class AssignRoleView
    {
        public Role RoleId { get; set; }
        public int SubModuleId { get; set; }
        public string IsEnabled { get; set; }
        public string IsWritable { get; set; }
    }

    public class AssignDashboardRoleView
    {
        public Role RoleId { get; set; }
        public int MenuId { get; set; }
        public string IsEnabled { get; set; }
        public string IsWritable { get; set; }
    }

}
