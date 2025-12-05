using Microsoft.EntityFrameworkCore;


namespace EmployeeInformations.CoreModels.DataViewModel
{
    [Keyless]
    public class ExpensesDataModel
    {
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string? ExpenseTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string EmployeeName { get; set; }
        public bool EmployeeStatus { get; set; }
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public int IsApproved { get; set; }
        public string EmployeeUserName { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string? ApprovalStatus { get; set; }
    }

}
