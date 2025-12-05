using Microsoft.EntityFrameworkCore;

namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class EmployeeActivityLogViewModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public string companyName { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string?IPAddress { get; set; }
    }
    public class EmployeeActivityLogFilterCount
    {
        public int Id { get; set; }
    }
}


