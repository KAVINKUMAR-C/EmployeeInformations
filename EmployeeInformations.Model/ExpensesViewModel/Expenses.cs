namespace EmployeeInformations.Model.ExpensesViewModel
{

    public class Expenses
    {

        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string ExpenseTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CompanyId { get; set; }
        public Int32 TotalAmount { get; set; }
        public int IsApproved { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ExpenseDetails
    {
        public int? DetailId { get; set; }
        public int? EmpId { get; set; }
        public int? ExpenseId { get; set; }
        public string? ExpenseCategory { get; set; }
        public decimal? Amount { get; set; }
        public string? BillNumber { get; set; }
        public string? ExpenseName { get; set; }
        public string? Document { get; set; }
        public int CompanyId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class ExpenseAttachments
    {
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public int DetailId { get; set; }
        public string? ExpenseName { get; set; }
        public string? Document { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }

    }
}
