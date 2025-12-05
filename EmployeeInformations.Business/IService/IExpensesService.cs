using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.Model.ExpensesViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Business.IService
{
    public interface IExpensesService
    {
        Task<List<ExpensesDataModel>> GetAllExpenses(SysDataTablePager pager, string columnName, string columnDirection, int empId,int companyId, int roleId);
        Task<bool> CreateExpenses(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId, int roleId);
        Task<ExpenseDetailViews> GetExpensesById(int expenseId,int companyId);
        Task<bool> DeleteExpenses(ExpenseDetailViews expenses,int companyId);
        Task<bool> DeleteExpenseDetail(ExpenseDetailViews expenses);
        Task<bool> ApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<bool> AmountApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId);
        Task<bool> AmountRejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<int> RejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<GetExpenses> ViewExpense(int expenseId,int companyId);
        Task<List<ExpensesDataModel>> GetEmployeeExpenses(SysDataTablePager pager, string columnName, string columnDirection, int empId,int companyId, int roleId);
        Task<int> GetAllExpenseCount(SysDataTablePager pager, int empId,int companyId,int roleId);
    }
}
