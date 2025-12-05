
using System.ComponentModel.DataAnnotations;

namespace EmployeeInformations.Model.ReportsViewModel
{
    public class EmployeePrivileges
    {
        [Key]
        public int PrivilegeID { get; set; }
        public int EmployeeID { get; set; }
        public bool IsEarnLeave { get; set; }
    }
}
