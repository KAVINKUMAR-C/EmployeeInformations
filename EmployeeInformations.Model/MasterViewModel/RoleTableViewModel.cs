using EmployeeInformations.Common.Enums;
using EmployeeInformations.Model.EmployeesViewModel;

namespace EmployeeInformations.Model.MasterViewModel
{
    public class RoleTableViewModel
    {
        public Role RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<RoleTableMaster> RoleTableMaster { get; set; }
    }

    public class RoleTableMaster
    {
        public Role RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public List<RoleTable> RoleTable { get; set; }
        public int RoleNameCount { get; set; }
    }
}
