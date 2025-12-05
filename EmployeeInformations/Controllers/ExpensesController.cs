using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.ExpensesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class ExpensesController : BaseController
    {
        private readonly IExpensesService _expensesService;
        public ExpensesController(IExpensesService expensesService)
        {
            _expensesService = expensesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logic to view all the expenses list
        /// </summary>
        public async Task<IActionResult> Expenses()
        {
            HttpContext.Session.SetString("LastView", Constant.Expenses);
            HttpContext.Session.SetString("LastController", Constant.Expenses);
            return View();
        }

        /// <summary>
        /// Logic to get all the expenses list
        /// </summary>
        /// <param name="pager, columnName, columnDirection" >expense</param>
        public async Task<IActionResult> GetExpenses(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            HttpContext.Session.SetString("LastView", Constant.Expenses);
            HttpContext.Session.SetString("LastController", Constant.Expenses);
            var cntExpense = await _expensesService.GetAllExpenseCount(pager, empId, companyId, sessionRoleId);
            var expenses = await _expensesService.GetAllExpenses(pager, columnName, columnDirection, empId, companyId, sessionRoleId);
            HttpContext.Session.SetString("LastView", Constant.Expenses);
            HttpContext.Session.SetString("LastController", Constant.Expenses);
            return Json(new
            {
                data = expenses,
                iTotalRecords = cntExpense,
                iTotalDisplayRecords = cntExpense
            });
        }
        public async Task<IActionResult> AddExpenses()
        {
            return View();
        }

        /// <summary>
        /// Logic to get edit the expense detail  by particular expense
        /// </summary>
        /// <param name="id" >expense</param>
        [HttpGet]
        public IActionResult EditExpenses(int expenseId)
        {
            var expenseDetails = new ExpenseDetailViews();
            expenseDetails.ExpenseId = expenseId;
            return PartialView("EditExpenses", expenseDetails);
        }

        /// <summary>
        /// Logic to get update the expense detail  by particular expense
        /// </summary>
        /// <param name="id" >expense</param>
        public async Task<IActionResult> UpdateExpenses(int expenseId)
        {
            var companyId = GetSessionValueForCompanyId;
            var expensesDetails = await _expensesService.GetExpensesById(expenseId, companyId);
            return View(expensesDetails);
        }

        /// <summary>
        /// Logic to get create the expense detail  by particular expense
        /// </summary>
        /// <param name="expenses,files" ></param>
        [HttpPost]
        public async Task<IActionResult> CreateExpenses(ExpenseDetailViews expenses, ICollection<Microsoft.AspNetCore.Http.IFormFile> files)
        {
            try
            {
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                var companyId = GetSessionValueForCompanyId;
                var roleId = GetSessionValueForRoleId;
                var listExpense = new List<ExpenseDetailView>();
                var GetFile = 0;
                foreach (var item in expenses.ListExpenseDetailView)
                {
                    var documentPath = string.Empty;
                    var expenceFileName = string.Empty;
                    if (item.ExpenseFileName != null && (files.Count() > 0))
                    {
                        string formFileName = files.ToList()[GetFile].FileName;
                        IFormFile formFile = files.ToList()[GetFile];
                        //var expenseAttachment = new Expen();
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Expenses");
                        //create folder if not exist
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var fileName = Guid.NewGuid() + Path.GetExtension(formFileName);
                        var combinedPath = Path.Combine(path, fileName);
                        documentPath = path.Replace(path, "~/Expenses/") + fileName;
                        expenceFileName = fileName;
                        using (var stream = new FileStream(combinedPath, FileMode.Create))
                        {
                            formFile.CopyTo(stream);
                        }
                        GetFile++;
                    }

                    listExpense.Add(new ExpenseDetailView()
                    {
                        ExpenseId = expenses.ExpenseId,
                        ExpenseCategory = item.ExpenseCategory,
                        Amount = item.Amount,
                        BillNumber = item.BillNumber,
                        EmpId = sessionEmployeeId,
                        Document = documentPath,
                        DetailId = item.DetailId,
                        ExpenseName = expenceFileName,
                        CreatedDate = DateTime.Now,
                        CreatedBy = sessionEmployeeId,
                    });

                }

                expenses.ListExpenseDetailView = listExpense;
                var result = await _expensesService.CreateExpenses(expenses, sessionEmployeeId, companyId,roleId);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Logic to get soft deleted the expense detail  by particular expense
        /// </summary>
        /// <param name="expenses" ></param>
        public async Task<IActionResult> DeleteExpenses(ExpenseDetailViews expenses)
        {
            var companyId = GetSessionValueForCompanyId;
            var expensesDetails = await _expensesService.DeleteExpenses(expenses, companyId);
            return new JsonResult(expensesDetails);
        }

        /// <summary>
        /// Logic to get soft deleted the expenses detail files by particular expense detail files
        /// </summary>
        /// <param name="expenses" ></param> 
        [HttpPost]
        public async Task<IActionResult> DeleteExpenseDetail(ExpenseDetailViews expenses)
        {
            var expensesDetails = await _expensesService.DeleteExpenseDetail(expenses);
            return new JsonResult(expensesDetails);
        }

        /// <summary>
        /// Logic to get approved the expenses detail  by particular expense 
        /// </summary>
        /// <param name="expenses" ></param> 
        [HttpPost]
        public async Task<IActionResult> ApprovedExpense(ExpenseDetailViews expenses)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (expenses != null)
            {
                result = await _expensesService.ApprovedExpense(expenses, sessionEmployeeId, companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get reject the expenses detail  by particular expense 
        /// </summary>
        /// <param name="expenses" ></param> 
        [HttpPost]
        public async Task<IActionResult> RejectExpense(ExpenseDetailViews expenses)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = 0;
            if (expenses != null)
            {
                result = await _expensesService.RejectExpense(expenses, sessionEmployeeId, companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get amountapproved the expenses detail  by particular expense 
        /// </summary>
        /// <param name="expenses" ></param> 
        [HttpPost]
        public async Task<IActionResult> AmountApprovedExpense(ExpenseDetailViews expenses)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (expenses != null)
            {
                result = await _expensesService.AmountApprovedExpense(expenses, sessionEmployeeId, companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get reject the expenses detail  by particular expense 
        /// </summary>
        /// <param name="expenses" ></param> 
        [HttpPost]
        public async Task<IActionResult> AmountRejectExpense(ExpenseDetailViews expenses)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (expenses != null)
            {
                result = await _expensesService.AmountRejectExpense(expenses, sessionEmployeeId, companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        ///  Logic to get dispaly the expenses detail  by particular expenses
        /// </summary>
        /// <param name="expenses" >expenses</param>
        [HttpGet]
        public async Task<IActionResult> ViewExpense(ExpenseDetailViews expenses)
        {
            var companyId = GetSessionValueForCompanyId;
            if (expenses != null)
            {
                var result = await _expensesService.ViewExpense(expenses.ExpenseId, companyId);
                return View(result);
            }
            return View(null);
        }


        /// <summary>
        /// Logic to view dispaly the expenses detail by particular employee
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EmployeeExpenses()
        {
            HttpContext.Session.SetString("LastView", Constant.EmployeeExpenses);
            HttpContext.Session.SetString("LastController", Constant.Expenses);
            return View();
        }


        /// <summary>
        /// Logic to get dispaly the expenses detail by particular employee
        /// /// <param name="pager, columnName, columnDirection" ></param> 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetEmployeeExpenses(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var sessionRoleId = GetSessionValueForRoleId;
            var expenses = await _expensesService.GetEmployeeExpenses(pager, columnName, columnDirection, sessionEmployeeId, companyId, sessionRoleId);
            HttpContext.Session.SetString("LastView", Constant.EmployeeExpenses);
            var cntExpense = await _expensesService.GetAllExpenseCount(pager, sessionEmployeeId, companyId,sessionRoleId);
            return Json(new
            {
                data = expenses,
                iTotalRecords = cntExpense,
                iTotalDisplayRecords = cntExpense
            });
        }
    }
}
