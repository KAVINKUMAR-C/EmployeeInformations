

using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class EmployeesDataModel
    {
        public int EmpId { get; set; }
        public string? UserName { get; set; }
        public string? EmployeeName { get; set; }
        public string ? OfficeEmail { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class EmployeesDataCount
    {
        public int Id { get; set;}
    }
}
