using EmployeeInformations.Common.Enums;

namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class RoleTable
    {
        public Role RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
