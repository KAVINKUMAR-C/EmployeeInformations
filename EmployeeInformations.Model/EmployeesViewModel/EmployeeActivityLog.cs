using EmployeeInformations.CoreModels.DataViewModel;
namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class EmployeeActivityLog
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string IPAddress { get; set; }
        public string EmployeeName { get; set; }
        public string companyName { get; set; }
     
        public List<EmployeeActivityLogViewModel>? EmployeeActivityLogViewModels { get; set; }

    }
}
