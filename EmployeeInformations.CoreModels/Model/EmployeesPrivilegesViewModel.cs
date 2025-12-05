
using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.Model
{
    [Keyless]
    public class EmployeesPrivilegesViewModel
    {
        public int PrivilegeID { get; set; }
        public string? EmployeesName { get; set; }
        public int EmployeeID { get; set; }
        public bool IsEarnLeave { get; set; }
        public int CompanyId { get; set; }
    }

    public class EmployeePrivilegesCount
    {
        public int Id { get; set; }
    }


}
