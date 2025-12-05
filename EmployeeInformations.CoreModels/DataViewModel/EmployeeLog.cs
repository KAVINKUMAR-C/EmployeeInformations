

namespace EmployeeInformations.CoreModels.DataViewModel
{
    public class EmployeeLog
    {
        public int Id { get; set; }
        public string? FieldName { get; set; }
        public string? PreviousValue { get; set; }
        public string? NewValue { get; set; }
        public string? Event { get; set; }
        public int EmpId { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedDate { get; set; }
    }

     public class EmployeesCount
     {
       public int   Id { get; set; }
     }
}
