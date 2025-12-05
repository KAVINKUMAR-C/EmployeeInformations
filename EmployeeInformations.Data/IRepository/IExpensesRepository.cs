using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Model.ExpensesViewModel;
using EmployeeInformations.Model.PagerViewModel;

namespace EmployeeInformations.Data.IRepository
{
    public interface IExpensesRepository
    {
        Task<List<ExpensesDataModel>> GetAllExpensesAdmin(SysDataTablePager pager, string columnName, string columnDirection, int empId,int companyId, int roleId);
        Task<List<ExpensesDataModel>> GetAllExpenseSummarys(SysDataTablePager pager, string columnName, string columnDirection, int empId,int companyId, int roleId);
        Task<ExpensesEntity> GetExpensesById(int expenseId,int companyId);
        Task<ExpenseDetailsEntity> GetAllExpenseDetailsByDetailId(int DetailId);
        Task<List<ExpenseDetailsEntity>> GetAllExpenseDetailsById(int id,int companyId);
        Task<int> CreateExpenses(ExpensesEntity expensesEntity,int companyId);
        Task<bool> CreateExpenseDetails(List<ExpenseDetailsEntity> expenseDetailsEntity);
        Task<bool> DeleteExpenses(ExpensesEntity expensesEntity);
        Task DeleteExpenseDetails(List<ExpenseDetailsEntity> expenseDetailsEntitys);
        Task DeleteExpenseDetail(ExpenseDetailsEntity expenseDetailsEntity);
        Task<bool> ApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<bool> AmountApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<bool> AmountRejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<int> RejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId,int companyId);
        Task<int> GetAllExpenseCount(SysDataTablePager pager, int empId,int companyId);
        Task<int> GetEmployeeExpenseCount(SysDataTablePager pager, int empId, int roleId, int companyId);
    }
}
