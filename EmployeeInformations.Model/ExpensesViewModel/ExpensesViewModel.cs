namespace EmployeeInformations.Model.ExpensesViewModel
{
    public class ExpensesViewModel
    {
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string? ExpenseTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TotalAmount { get; set; }
        public int IsApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public ExpenseDetails? ExpenseDetails { get; set; }
        public List<GetExpenses> GetExpenses { get; set; }

    }

    public class ExpenseDetailViews
    {
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string? ExpenseTitle { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TotalAmount { get; set; }
        public int IsApproved { get; set; }
        public string? Reason { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<ExpenseDetailView> ListExpenseDetailView { get; set; }
        public List<GetExpenses> GetExpenses { get; set; }
        public string strFromDate { get; set; }
        public string strToDate { get; set; }
        public int DetailId { get; set; }
    }

    public class ExpenseDetailView
    {
        public string Category { get; set; }
        //public string Amount { get; set; }
        public string BillNumber { get; set; }
        public int DetailId { get; set; }
        public int EmpId { get; set; }
        public int ExpenseId { get; set; }
        public string ExpenseCategory { get; set; }
        public decimal Amount { get; set; }
        public string? ExpenseName { get; set; }
        public string? Document { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string ExpenseFileName { get; set; }
        public string splitName { get; set; }
    }

    public class GetExpenses
    {
        public string EmployeeName { get; set; }
        public string EmployeeUserName { get; set; }
        public bool EmployeeStatus { get; set; }
        public int ExpenseId { get; set; }
        public int EmpId { get; set; }
        public string ExpenseTitle { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Amount { get; set; }
        public int IsApproved { get; set; }
        public string EmployeeProfileImage { get; set; }
        public string ClassName { get; set; }
        public string? Reason { get; set; }
        public List<ExpenseDetailViews> ExpenseDetails { get; set; }
        public List<ExpenseDetailView> ExpenseDetailView { get; set; }
    }

}
