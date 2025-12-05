using EmployeeInformations.Model.ReportsViewModel;

namespace EmployeeInformations.Model.EmployeesViewModel
{
    public class SalaryViewModel
    {
        public int SalaryId { get; set; }
        public decimal Amount { get; set; }
        public int EmpId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Year { get; set; }       
        public List<Years>? Years { get; set; }
        public List<salarys>? salary {  get; set; }
        public int Month { get; set; }      
    }

    public class salarys
    {
        public int SalaryId { get; set; }
        public decimal Amount { get; set; }
        public int EmpId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; } = 0;
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Year { get; set; }      
        public List<Years> Years { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }         
    }
}
