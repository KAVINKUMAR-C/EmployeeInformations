using EmployeeInformations.CoreModels.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformations.Data.Model
{
    public class EmployeeWorkAnniversary
    {
        public int EmpId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int Gender { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string OfficeEmail { get; set; }
        public List<EmployeesEntity> Employees { get; set; }
        public List<int> ReportingPersonEmplyeeIds { get; set; }
        public string FromEmail { get; set; }
    }
}
