using EmployeeInformations.CoreModels.DataViewModel;

namespace EmployeeInformations.Model
{ 
   public class EmployeesListViewModel
   {
       public int EmpId { get; set; }
       public string? UserName { get; set; }
       public string? EmployeeName { get; set; }
       public string? OfficeEmail { get; set; }
       public string? PhoneNumber { get; set; }
       public List<EmployeesDataModel>? EmployeesDataModel { get; set; }
   }
    
}
