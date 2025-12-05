using EmployeeInformations.CoreModels;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.DbConnection;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.ExpensesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace EmployeeInformations.Data.Repository
{
    public class ExpensesRepository : IExpensesRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public ExpensesRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// Expense Entity


        /// <summary>
        /// Logic to get create and update the expensesentitys detail 
        /// </summary>   
        /// <param name="expensesEntity" ></param>   
        public async Task<int> CreateExpenses(ExpensesEntity expensesEntity,int companyId)
        {

            var result = 0;
            if (expensesEntity?.ExpenseId == 0)
            {
                expensesEntity.CompanyId = companyId;
                await _dbContext.ExpensesEntitys.AddAsync(expensesEntity);
                await _dbContext.SaveChangesAsync();
                result = expensesEntity != null ? expensesEntity.ExpenseId : 0;
            }
            else
            {
                if (expensesEntity != null)
                {
                    _dbContext.ExpensesEntitys.Update(expensesEntity);
                    await _dbContext.SaveChangesAsync();
                    result = expensesEntity != null ? expensesEntity.ExpenseId : 0;
                }
            }
            return result;
        }


        /// <summary>
        /// Logic to get delete the expensesentitys detail by particular expensesentitys
        /// </summary>   
        /// <param name="expensesEntity" ></param>        
        public async Task<bool> DeleteExpenses(ExpensesEntity expensesEntity)
        {
            var result = false;
            if (expensesEntity != null)
            {
                _dbContext.ExpensesEntitys.Update(expensesEntity);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get the expensesentitys detail by all expensesentitys
        /// </summary>   
        /// <param name="pager, columnName, columnDirection, empId" ></param> 
        public async Task<List<ExpensesDataModel>> GetAllExpensesAdmin(SysDataTablePager pager, string columnName, string columnDirection, int empId, int companyId, int roleId)
        {
            try
            {
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }

                var sorting = columnName + " " + columnDirection;

                var parameters = new[]
                {
                    new NpgsqlParameter("@empId", empId),
                    new NpgsqlParameter("@companyId", companyId),
                    new NpgsqlParameter("@roleId", roleId),
                    new NpgsqlParameter("@pagingSize", pager.iDisplayLength),
                    new NpgsqlParameter("@offsetValue", pager.sEcho),
                    new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? (object)DBNull.Value : pager.sSearch),
                    new NpgsqlParameter("@sorting", sorting)
                };

                var expenses = await _dbContext.employeeExpensesEntities
                    .FromSqlRaw("SELECT * FROM spgetallexpensefilterlist(@empId, @companyId, @roleId, @pagingSize, @offsetValue, @searchText, @sorting)", parameters)
                    .ToListAsync();

                return expenses;
            }
            catch (Exception ex)
            {
                throw; // Rethrow preserves the original stack trace
            }
        }

        /// <summary>
         /// Logic to get id the expensesentitys detail by particular expensesentitys
         /// </summary>   
        /// <param name="id" >expensesentitys</param>
        /// <param name="IsDeleted" >expensesentitys</param>
        /// <param name="CompanyId" >expensesentitys</param>
        public async Task<ExpensesEntity> GetExpensesById(int expenseId, int companyId)
        {
            var expensesEntity = await _dbContext.ExpensesEntitys.FirstOrDefaultAsync(x => x.CompanyId == companyId && !x.IsDeleted && x.ExpenseId == expenseId);
            return expensesEntity ?? new ExpensesEntity();
        }


        /// Expense Details

        /// <summary>
         /// Logic to get create and update the expensedetailsentitys detail 
         /// </summary>   
        /// <param name="expensesEntity" ></param> 
        public async Task<bool> CreateExpenseDetails(List<ExpenseDetailsEntity> expenseDetailsEntity)
        {
            var result = false;
            try
            {
                foreach (var item in expenseDetailsEntity)
                {
                    if (item.DetailId == 0)
                    {
                        item.CreatedDate = DateTime.Now;
                        item.CreatedBy = item.EmpId;
                        await _dbContext.ExpenseDetailsEntitys.AddAsync(item);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                    else
                    {
                        _dbContext.ExpenseDetailsEntitys.Update(item);
                        result = await _dbContext.SaveChangesAsync() > 0;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        /// <summary>
        /// Logic to get id the expensedetailsentitys detail by particular expensedetailsentitys
        /// </summary>   
        /// <param name="id" >expensedetailsentitys</param>
        /// <param name="CompanyId" >expensedetailsentitys</param>
        /// <param name="IsDeleted" >expensedetailsentitys</param>
        public async Task<List<ExpenseDetailsEntity>> GetAllExpenseDetailsById(int id, int companyId)
        {
            var expenseDetailsEntity = await _dbContext.ExpenseDetailsEntitys.Where(x => x.Expenses.CompanyId == companyId && !x.IsDeleted && x.ExpenseId == id).AsNoTracking().ToListAsync();
            return expenseDetailsEntity;
        }


        /// <summary>
        /// Logic to get delete the expensedetailsentitys detail by particular expensedetailsentitys
        /// </summary>   
        /// <param name="expenseDetailsEntitys" ></param> 
        public async Task DeleteExpenseDetails(List<ExpenseDetailsEntity> expenseDetailsEntitys)
        {
            _dbContext.ExpenseDetailsEntitys.UpdateRange(expenseDetailsEntitys);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get delete the expensedetailsentitys detail by particular expensedetailsentitys
        /// </summary>   
        /// <param name="expenseDetailsEntity" ></param> 
        public async Task DeleteExpenseDetail(ExpenseDetailsEntity expenseDetailsEntity)
        {
            _dbContext.ExpenseDetailsEntitys.Update(expenseDetailsEntity);
            await _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Logic to get detailId the expensedetailsentitys detail by particular expensedetailsentitys
        /// </summary>   
        /// <param name="detailId" >expensedetailsentitys</param>
        /// <param name="IsDeleted" >expensedetailsentitys</param>      
        public async Task<ExpenseDetailsEntity> GetAllExpenseDetailsByDetailId(int DetailId)
        {
            var expensesDetailEntity = await _dbContext.ExpenseDetailsEntitys.AsNoTracking().FirstOrDefaultAsync(x => x.DetailId == DetailId && !x.IsDeleted);
            return expensesDetailEntity ?? new ExpenseDetailsEntity();
        }
       

        /// <summary>
         /// Logic to get empIds the expensesentitys detail by particular expensesentitys
        /// </summary>   
        /// <param name="empId, pager, columnName, columnDirection" >expensesentitys</param>
        public async Task<List<ExpensesDataModel>> GetAllExpenseSummarys(SysDataTablePager pager, string columnName, string columnDirection, int empId,int companyId,int roleId)
        {
            try
            {  
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@roleId", roleId);
                var param3 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param4 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param6 = new NpgsqlParameter("@sorting", _params.Sorting);

                var expenseAllEmp = await _dbContext.employeeExpensesEntities.FromSqlRaw("EXEC [dbo].[spGetAllExpenseFilterList] @empId, @companyId, @roleId, @pagingSize, @offsetValue, @searchText, @sorting", param, param1, param2, param3, param4, param5,param6).ToListAsync();
                return expenseAllEmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<ExpensesDataModel>> GetAllEmployeeExpenseSummarys(SysDataTablePager pager, string columnName, string columnDirection, int empId, int companyId, int roleId)
        {
            try
            {
                if (pager.iDisplayStart >= pager.iDisplayLength)
                {
                    pager.sEcho = (pager.iDisplayStart / pager.iDisplayLength) + 1;
                }
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    OffsetValue = pager.sEcho,
                    PagingSize = pager.iDisplayLength,
                    SearchText = pager.sSearch,
                    Sorting = columnName + " " + columnDirection
                };
                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@roleId", roleId);
                var param3 = new NpgsqlParameter("@pagingSize", pager.iDisplayLength);
                var param4 = new NpgsqlParameter("@offsetValue", pager.sEcho);
                var param5 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(pager.sSearch) ? DBNull.Value : (object)pager.sSearch);
                var param6 = new NpgsqlParameter("@sorting", _params.Sorting);

                var expenseAllEmp = await _dbContext.employeeExpensesEntities.FromSqlRaw("EXEC [dbo].[spGetAllEmployeeExpenseFilterList] @empId, @companyId, @roleId, @pagingSize, @offsetValue, @searchText, @sorting", param, param1, param2, param3, param4, param5, param6).ToListAsync();
                return expenseAllEmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Logic to get approvedexpense the expensesentitys detail by particular expensesentitys
        /// </summary>   
        /// <param name="expenses,sessionEmployeeId" ></param>
        /// <param name="IsDeleted" >expensesentitys</param>
        /// <param name="CompanyId" >expensesentitys</param>
        public async Task<bool> ApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeExpenseEntities = await _dbContext.ExpensesEntitys.Where(e => e.ExpenseId == expenses.ExpenseId && !e.IsDeleted && e.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            if (employeeExpenseEntities != null)
            {
                employeeExpenseEntities.IsApproved = 1;
                employeeExpenseEntities.Reason = expenses.Reason;
                _dbContext.ExpensesEntitys.Update(employeeExpenseEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }


        /// <summary>
        /// Logic to get rejectexpense the expensesentitys detail by particular expensesentitys
        /// </summary>   
        /// <param name="expenses,sessionEmployeeId" ></param>
        /// <param name="IsDeleted" >expensesentitys</param>
        /// <param name="CompanyId" >expensesentitys</param>
        public async Task<int> RejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId)
        {
            var result = 0;
            var employeeExpenseEntities = await _dbContext.ExpensesEntitys.Where(e => e.ExpenseId == expenses.ExpenseId && !e.IsDeleted && e.CompanyId == companyId).FirstOrDefaultAsync();
            if (employeeExpenseEntities != null)
            {
                employeeExpenseEntities.IsApproved = 2;
                employeeExpenseEntities.Reason = expenses.Reason;
                _dbContext.ExpensesEntitys.Update(employeeExpenseEntities);
                await _dbContext.SaveChangesAsync();
                result = 1;
            }
            return result;
        }


        /// <summary>
        /// Logic to get amountapporvedexpense the expensesentitys detail by particular expensesentitys
        /// </summary>   
        /// <param name="expenses,sessionEmployeeId" ></param>
        /// <param name="IsDeleted" >expensesentitys</param>
        /// <param name="CompanyId" >expensesentitys</param>
        public async Task<bool> AmountApprovedExpense(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeExpenseEntities = await _dbContext.ExpensesEntitys.Where(e => e.ExpenseId == expenses.ExpenseId && !e.IsDeleted && e.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            if (employeeExpenseEntities != null)
            {
                employeeExpenseEntities.IsApproved = 3;
                employeeExpenseEntities.Reason = expenses.Reason;
                _dbContext.ExpensesEntitys.Update(employeeExpenseEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
         /// Logic to get amountapporvedexpense the expensesentitys detail by particular expensesentitys
         /// </summary>   
        /// <param name="expenses,sessionEmployeeId" ></param>
        /// <param name="IsDeleted" >expensesentitys</param>
        /// <param name="CompanyId" >expensesentitys</param>
        public async Task<bool> AmountRejectExpense(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId)
        {
            var result = false;
            var employeeExpenseEntities = await _dbContext.ExpensesEntitys.Where(e => e.ExpenseId == expenses.ExpenseId && !e.IsDeleted && e.CompanyId == companyId).AsNoTracking().FirstOrDefaultAsync();
            if (employeeExpenseEntities != null)
            {
                employeeExpenseEntities.IsApproved = 4;
                employeeExpenseEntities.Reason = expenses.Reason;
                _dbContext.ExpensesEntitys.Update(employeeExpenseEntities);
                result = await _dbContext.SaveChangesAsync() > 0;
            }
            return result;
        }

        /// <summary>
        /// Logic to get Allexpensedetails count of all employees and reporting person
        /// </summary> 
        /// <param name="empId, pager" ></param>
        public async Task<int> GetEmployeeExpenseCount(SysDataTablePager pager, int empId, int roleId,int companyId)
        {
            try
            {

                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@roleId", roleId);
                var param3 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> allExpensesCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetAllEmployeeExpenseFilterCount]  @empId, @companyId,@roleId,@searchText", param, param1, param2,param3).ToListAsync();
                foreach (var item in allExpensesCounts)
                {
                    var result = item.Id;
                    return result;
                }
                return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetAllExpenseCount(SysDataTablePager pager, int empId, int companyId)
        {
            try
            {
                if (pager.sSearch == null)
                {
                    pager.sSearch = "";
                }
                var _params = new
                {
                    SearchText = pager.sSearch
                };

                var param = new NpgsqlParameter("@empId", empId);
                var param1 = new NpgsqlParameter("@companyId", companyId);
                var param2 = new NpgsqlParameter("@searchText", string.IsNullOrEmpty(_params.SearchText) ? DBNull.Value : (object)_params.SearchText);
                List<EmployeesDataCount> allExpensesCounts = await _dbContext.EmployeesDataCounts.FromSqlRaw("EXEC [dbo].[spGetAllExpenseFilterCount] @empId, @companyId,@searchText", param, param1, param2).ToListAsync();
                foreach (var item in allExpensesCounts)
                {
                    var result = item.Id;
                    return result;
                }
                return 0;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
