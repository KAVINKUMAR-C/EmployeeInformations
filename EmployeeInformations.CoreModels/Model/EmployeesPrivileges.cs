
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.CoreModels.Model
{
    public class EmployeesPrivileges
    {
        [Key]
        public int PrivilegeID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsEarnLeave { get; set; }
        public int CompanyId { get; set; }
    }
}
