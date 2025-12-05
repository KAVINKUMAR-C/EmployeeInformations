using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.Model.ReportsViewModel
{
    [Keyless]
    public class EmployeePrivilegesViewModel
    {
        
        public int PrivilegeID { get; set; }
        public int EmployeeID { get; set; }
        public string? EmployeesName { get; set; }
        public bool IsEarnLeave { get; set; }
        public List<EmployeeDropdown>? Employees { get; set; }
    }

    public class EmployeePrivilegesCount
    {
        public int Id { get; set; }
    }
}
