using AutoMapper;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common.Enums;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.CoreModels.DataViewModel;
using EmployeeInformations.CoreModels.Model;
using EmployeeInformations.Data.IRepository;
using EmployeeInformations.Model.ExpensesViewModel;
using EmployeeInformations.Model.PagerViewModel;
using System.Text;

namespace EmployeeInformations.Business.Service
{
    public class ExpensesService : IExpensesService
    {
        private readonly IExpensesRepository _expensesRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IEmailDraftRepository _emailDraftRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILeaveRepository _leaveRepository;
        public ExpensesService(IExpensesRepository expensesRepository, IMapper mapper, IEmployeesRepository employeesRepository, IEmailDraftRepository emailDraftRepository, ICompanyRepository companyRepository, ILeaveRepository leaveRepository)
        {
            _mapper = mapper;
            _expensesRepository = expensesRepository;
            _employeesRepository = employeesRepository;
            _emailDraftRepository = emailDraftRepository;
            _leaveRepository = leaveRepository;
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Logic to get Expenses details the by particular empId
        /// </summary>
        /// <param name="empId, pager, columnName, columnDirection" ></param> 
        public async Task<List<ExpensesDataModel>> GetAllExpenses(SysDataTablePager pager, string columnName, string columnDirection, int empId, int companyId, int roleId)
        {
            var listOfExpensesViewModel = new List<ExpensesDataModel>();
            if (empId == 0)
            {
                listOfExpensesViewModel = await _expensesRepository.GetAllExpensesAdmin(pager, columnName, columnDirection, empId, companyId,roleId);
            }
            else
            {
                //var employeeIds = await _employeesRepository.GetAllEmployeeIdsReportingPersonForLeave(empId);
                //var empIds = new List<int>();
                //empIds.Add(empId);
                //foreach (var employee in employeeIds)
                //{
                //    empIds.Add(employee.EmployeeId);
                //}
                listOfExpensesViewModel = await _expensesRepository.GetAllExpenseSummarys(pager, columnName, columnDirection, empId, companyId, roleId);
            }
            return listOfExpensesViewModel;
        }

        /// <summary>
        /// Logic to get create and update the expenses details 
        /// </summary>
        /// <param name="expenses" ></param> 
        /// <param name="sessionEmployeeId" ></param> 
        public async Task<bool> CreateExpenses(ExpenseDetailViews expenses, int sessionEmployeeId, int companyId,int roleId)
        {
            var result = false;
            var toDate = Common.Helpers.DateTimeExtensions.ConvertToDatetime(expenses.strToDate);
            var fromDate = Common.Helpers.DateTimeExtensions.ConvertToDatetime(expenses.strFromDate);

            if (expenses != null)
            {
                if (expenses.ExpenseId == 0)
                {

                    try
                    {
                        //var totalAmount = String.Format("{0:000.0}", expenses.TotalAmount);
                        decimal totalAmountDecimal = decimal.Parse(expenses.TotalAmount);
                        var expensesEntity = new ExpensesEntity();
                        expensesEntity.ExpenseTitle = string.IsNullOrEmpty(expenses.ExpenseTitle) ? string.Empty : expenses.ExpenseTitle;
                        expensesEntity.FromDate = fromDate;
                        expensesEntity.ToDate = toDate;
                        expensesEntity.EmpId = sessionEmployeeId;
                        //expensesEntity.TotalAmount = Convert.ToInt32(totalAmount);
                        expensesEntity.TotalAmount = totalAmountDecimal;
                        expensesEntity.CreatedDate = DateTime.Now;
                        expensesEntity.CreatedBy = sessionEmployeeId;
                        expensesEntity.Reason = string.IsNullOrEmpty(expenses.Reason) ? null : expenses.Reason;
                        expensesEntity.CompanyId = companyId;
                        var expensesEntityId = await _expensesRepository.CreateExpenses(expensesEntity, companyId);
                        var expenseDetailsEntitys = new List<ExpenseDetailsEntity>();
                        foreach (var item in expenses.ListExpenseDetailView)
                        {
                            var expenseDetailsEntity = new ExpenseDetailsEntity();
                            expenseDetailsEntity.ExpenseId = expensesEntityId;
                            expenseDetailsEntity.ExpenseCategory = expenses.ListExpenseDetailView == null ? string.Empty : item.ExpenseCategory;
                            expenseDetailsEntity.Amount = expenses.ListExpenseDetailView == null ? 0.0m : item.Amount;
                            expenseDetailsEntity.BillNumber = expenses.ListExpenseDetailView == null ? string.Empty : item.BillNumber;
                            expenseDetailsEntity.EmpId = item.EmpId;
                            expenseDetailsEntity.Document = item.Document;
                            expenseDetailsEntity.ExpenseName = item.ExpenseName;
                            expenseDetailsEntity.CreatedDate = DateTime.Now;
                            expenseDetailsEntity.CreatedBy = sessionEmployeeId;
                            expenseDetailsEntitys.Add(expenseDetailsEntity);
                        }
                        result = await _expensesRepository.CreateExpenseDetails(expenseDetailsEntitys);

                        if (result == true)
                        {
                            await sendMail(expensesEntityId, sessionEmployeeId, companyId);
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    var expensesEntity = await _expensesRepository.GetExpensesById(expenses.ExpenseId, companyId);
                    var totalAmount = String.Format("{0:000.0}", expenses.TotalAmount);
                    expensesEntity.ExpenseTitle = string.IsNullOrEmpty(expenses.ExpenseTitle) ? string.Empty : expenses.ExpenseTitle;
                    expensesEntity.FromDate = fromDate;
                    expensesEntity.ToDate = toDate;
                    expensesEntity.EmpId = sessionEmployeeId;
                    expensesEntity.TotalAmount = Convert.ToDecimal(totalAmount);
                    expensesEntity.UpdatedDate = DateTime.Now;
                    expensesEntity.UpdatedBy = sessionEmployeeId;
                    expensesEntity.Reason = string.IsNullOrEmpty(expenses.Reason) ? null : expenses.Reason;
                    expensesEntity.CreatedDate = expensesEntity.CreatedDate;
                    expensesEntity.CreatedBy = expensesEntity.CreatedBy;
                    var expensesEntityId = await _expensesRepository.CreateExpenses(expensesEntity, companyId);
                    expensesEntity.CompanyId = companyId;
                    var expenseDetailsEntitys = new List<ExpenseDetailsEntity>();
                    foreach (var item in expenses.ListExpenseDetailView)
                    {

                        var expenseDetailModel = new ExpenseDetailsEntity();
                        var expenseDetailsEntity = await _expensesRepository.GetAllExpenseDetailsByDetailId(item.DetailId);
                        expenseDetailModel.ExpenseId = expensesEntityId;
                        expenseDetailModel.DetailId = item.DetailId;
                        expenseDetailModel.ExpenseCategory = item.ExpenseCategory;
                        expenseDetailModel.Amount = item.Amount;
                        expenseDetailModel.BillNumber = item.BillNumber;
                        expenseDetailModel.CreatedBy = expenseDetailsEntity.CreatedBy;
                        expenseDetailModel.CreatedDate = expenseDetailsEntity.CreatedDate;
                        expenseDetailModel.EmpId = item.EmpId;
                        expenseDetailModel.Document = item.Document == "" ? expenseDetailsEntity.Document : item.Document;
                        expenseDetailModel.ExpenseName = item.ExpenseName == "" ? expenseDetailsEntity.ExpenseName : item.ExpenseName;
                        expenseDetailModel.UpdatedDate = DateTime.Now;
                        expenseDetailModel.UpdatedBy = sessionEmployeeId;
                        expenseDetailsEntitys.Add(expenseDetailModel);
                    }
                    result = await _expensesRepository.CreateExpenseDetails(expenseDetailsEntitys);
                }
            }
            return result;
        }

        public async Task sendMail(int expenseId, int empId, int companyId)
        {
            var draftId = (int)EmailDraftType.ApplyExpense;
            var emailContent = await _emailDraftRepository.GetByEmailDraftTypeId(draftId, companyId);
            var expensesEntity = await _expensesRepository.GetExpensesById(expenseId, companyId);
            var employeeProfile = await _employeesRepository.GetEmployeeById(expensesEntity.EmpId, companyId);
            var reportingPerson = await _employeesRepository.GetEmployeeById(empId, companyId);
            var detailsEntity = await _expensesRepository.GetAllExpenseDetailsById(expenseId, companyId);
            var fromDate = expensesEntity.FromDate.ToString("dd/MM/yyyy");
            var toDate = expensesEntity.ToDate.ToString("dd/MM/yyyy");

            var totalAmount = Convert.ToString(expensesEntity.TotalAmount);
            var reportingPersonEmployeeIds = await _employeesRepository.GetAllReportingPersonsEmpIdForLeave(expensesEntity.EmpId, companyId);
            StringBuilder sb = new StringBuilder();
            sb.Append(emailContent.Email);
            foreach (var item in reportingPersonEmployeeIds)
            {
                var email = await _leaveRepository.GetEmployeeEmailByEmpIdForLeave(item.ReportingPersonEmpId);
                sb.Append(",");
                sb.Append(email);
            }
            string toMails = sb.ToString();

            var isApproved = expensesEntity.IsApproved == 4 ? 2 : expensesEntity.IsApproved;
            var status = Convert.ToString((ExpenseStatus)isApproved);
            var empName = reportingPerson.FirstName + " " + reportingPerson.LastName;
            if (isApproved == 0)
            {
                status = Common.Constant.WaitingForApproval;

            }
            var count = Convert.ToString(detailsEntity.Count());
            var bodyContent = EmailBodyContent.SendEmail_Body_ApplyExpense(expensesEntity, emailContent.DraftBody, employeeProfile.FirstName, employeeProfile.LastName, employeeProfile.UserName, fromDate, toDate, totalAmount, status, empName, count);

            var attachment = new List<string>();

            foreach (var item in detailsEntity)
            {
                if (!string.IsNullOrEmpty(item.ExpenseName))
                {
                    var fileName = item.ExpenseName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/Expenses");

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    attachment.Add(Path.Combine(path, fileName));
                }
            }

            var file = string.Join(",", attachment);

            await InsertEmailApplyExpense(emailContent, employeeProfile.OfficeEmail, bodyContent, file, toMails);
        }

        public async Task InsertEmailApplyExpense(EmailDraftContentEntity emailDraft, string officeEmail, string bodyContent, string file, string toMails)
        {
            var emailSettingEntity = await _companyRepository.GetEmailSettingsEntity();
            var emailEntity = new EmailQueueEntity();
            emailEntity.FromEmail = emailSettingEntity.FromEmail;
            emailEntity.Body = bodyContent;
            emailEntity.ToEmail = officeEmail;
            emailEntity.Subject = emailDraft.Subject;
            emailEntity.Reason = Common.Constant.ApplyExpense;
            emailEntity.DisplayName = emailDraft.DisplayName;
            emailEntity.CCEmail = toMails;
            emailEntity.Attachments = file.TrimEnd(',');
            emailEntity.IsSend = false;
            emailEntity.CreatedDate = DateTime.Now;
            await _companyRepository.InsertEmailQueueEntity(emailEntity);

        }

        /// <summary>
                /// Logic to get update the expense detail  by particular expense id
                /// </summary>
        /// <param name="id" >expense</param>
        public async Task<ExpenseDetailViews> GetExpensesById(int expenseId, int companyId)
        {

            var listOfExpensesViewModel = new ExpenseDetailViews();
            listOfExpensesViewModel.GetExpenses = new List<GetExpenses>();
            listOfExpensesViewModel.ListExpenseDetailView = new List<ExpenseDetailView>();
            var listExpenses = await _expensesRepository.GetExpensesById(expenseId, companyId);
            var totalAmount = String.Format("{0:0.00}", listExpenses.TotalAmount);
            if (listExpenses != null)
            {
                var listExpenseDetails = await _expensesRepository.GetAllExpenseDetailsById(expenseId, companyId);
                foreach (var item in listExpenseDetails)
                {
                    listOfExpensesViewModel.ListExpenseDetailView.Add(new ExpenseDetailView()
                    {
                        ExpenseCategory = item.ExpenseCategory,
                        BillNumber = item.BillNumber,
                        DetailId = item.DetailId,
                        EmpId = item.EmpId,
                        ExpenseId = item.ExpenseId,
                        Amount = item.Amount,
                        ExpenseName = item.ExpenseName,
                        Document = item.Document,
                        splitName = string.IsNullOrEmpty(item.ExpenseName) ? "" : item.ExpenseName.Substring(item.ExpenseName.LastIndexOf(".") + 1),
                    });
                }

                if (listExpenses != null)
                {
                    listOfExpensesViewModel.ExpenseId = listExpenses.ExpenseId;
                    listOfExpensesViewModel.ExpenseTitle = listExpenses.ExpenseTitle;
                    listOfExpensesViewModel.FromDate = listExpenses.FromDate;
                    listOfExpensesViewModel.ToDate = listExpenses.ToDate;
                    listOfExpensesViewModel.EmpId = listExpenses.EmpId;
                    listOfExpensesViewModel.TotalAmount = totalAmount;
                    listOfExpensesViewModel.Reason = listExpenses.Reason;
                }
            }
            return listOfExpensesViewModel;
        }

        /// <summary>
                /// Logic to get deleted the expense detail  by particular expense id
                /// </summary>
        /// <param name="id" >expense</param>
        public async Task<bool> DeleteExpenses(ExpenseDetailViews expense, int companyId)
        {
            var expenseEntity = await _expensesRepository.GetExpensesById(expense.ExpenseId, companyId);
            expenseEntity.IsDeleted = true;
            var result = await _expensesRepository.DeleteExpenses(expenseEntity);

            var expensesDetailsEntitys = await _expensesRepository.GetAllExpenseDetailsById(expense.ExpenseId, companyId);
            foreach (var item in expensesDetailsEntitys)
            {
                item.IsDeleted = true;
            }
            await _expensesRepository.DeleteExpenseDetails(expensesDetailsEntitys);
            return result;
        }

        /// <summary>
                /// Logic to get deleted the expense detail  by particular expense 
                /// </summary>
        /// <param name="expense" ></param>
        public async Task<bool> DeleteExpenseDetail(ExpenseDetailViews expense)
        {
            var expensesDetailsEntitys = await _expensesRepository.GetAllExpenseDetailsByDetailId(expense.DetailId);
            expensesDetailsEntitys.IsDeleted = true;
            await _expensesRepository.DeleteExpenseDetail(expensesDetailsEntitys);
            return true;
        }

        /// <summary>
                /// Logic to get approved the expense detail  by particular expense 
                /// </summary>
        /// <param name="expense" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> ApprovedExpense(ExpenseDetailViews expense, int sessionEmployeeId, int companyId)
        {
            var result = await _expensesRepository.ApprovedExpense(expense, sessionEmployeeId, companyId);

            if (result == true)
            {
                await sendMail(expense.ExpenseId, sessionEmployeeId, companyId);
            }
            return result;
        }

        /// <summary>
                /// Logic to get reject the expense detail  by particular expense 
                /// </summary>
        /// <param name="expense" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<int> RejectExpense(ExpenseDetailViews expense, int sessionEmployeeId, int companyId)
        {
            var result = await _expensesRepository.RejectExpense(expense, sessionEmployeeId, companyId);

            if (result == 1)
            {
                await sendMail(expense.ExpenseId, sessionEmployeeId, companyId);
            }
            return result;
        }

        /// <summary>
                /// Logic to get amountapproved the expense detail  by particular expense 
                /// </summary>
        /// <param name="expense" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> AmountApprovedExpense(ExpenseDetailViews expense, int sessionEmployeeId, int companyId)
        {
            var result = await _expensesRepository.AmountApprovedExpense(expense, sessionEmployeeId, companyId);
            if (result == true)
            {
                await sendMail(expense.ExpenseId, sessionEmployeeId, companyId);
            }

            return result;
        }

        /// <summary>
                /// Logic to get reject the expense detail  by particular expense 
                /// </summary>
        /// <param name="expense" ></param>
        /// <param name="sessionEmployeeId" ></param>
        public async Task<bool> AmountRejectExpense(ExpenseDetailViews expense, int sessionEmployeeId, int companyId)
        {
            var result = await _expensesRepository.AmountRejectExpense(expense, sessionEmployeeId, companyId);
            if (result == true)
            {
                await sendMail(expense.ExpenseId, sessionEmployeeId, companyId);
            }

            return result;
        }

        /// <summary>
                /// Logic to get view the expense detail  by particular expense 
                /// </summary>
        /// <param name="id" ></param>      
        public async Task<GetExpenses> ViewExpense(int expenseId, int companyId)
        {
            var expenseEntity = await _expensesRepository.GetExpensesById(expenseId, companyId);
            var userName = await _employeesRepository.GetEmployeeById(expenseEntity.EmpId, companyId);
            var getExpenses = new GetExpenses();
            getExpenses.ExpenseDetailView = new List<ExpenseDetailView>();
            var listExpenseDetails = await _expensesRepository.GetAllExpenseDetailsById(expenseId, companyId);
            foreach (var item in listExpenseDetails)
            {
                getExpenses.ExpenseDetailView.Add(new ExpenseDetailView()
                {
                    ExpenseCategory = item.ExpenseCategory,
                    BillNumber = item.BillNumber,
                    DetailId = item.DetailId,
                    EmpId = item.EmpId,
                    ExpenseId = item.ExpenseId,
                    Amount = item.Amount,
                    ExpenseName = item.ExpenseName,
                    Document = item.Document,
                    splitName = string.IsNullOrEmpty(item.ExpenseName) ? "" : item.ExpenseName.Substring(item.ExpenseName.LastIndexOf(".") + 1),
                });
            }
            getExpenses.ExpenseId = expenseEntity.ExpenseId;
            getExpenses.EmpId = expenseEntity.EmpId;
            getExpenses.EmployeeName = userName == null ? "" : userName.FirstName + " " + userName.LastName;
            getExpenses.ExpenseTitle = expenseEntity.ExpenseTitle;
            getExpenses.FromDate = expenseEntity.FromDate;
            getExpenses.ToDate = expenseEntity.ToDate;
            getExpenses.Amount = Convert.ToString(expenseEntity.TotalAmount);
            getExpenses.IsApproved = expenseEntity.IsApproved;
            getExpenses.Reason = expenseEntity.Reason;
            return getExpenses;
        }



        /// <summary>
        /// Logic to get expense detail by particular employees 
        /// </summary>
        /// <param name="pager, empId, columnName, columnDirection" ></param>  
        public async Task<List<ExpensesDataModel>> GetEmployeeExpenses(SysDataTablePager pager, string columnName, string columnDirection, int empId, int companyId, int roleId)
        {
            var empIds = new List<int>();
            empIds.Add(empId);
            var listOfExpensesViewModel = await _expensesRepository.GetAllExpenseSummarys(pager, columnName, columnDirection, empId, companyId, roleId);
            return listOfExpensesViewModel;
        }

        /// <summary>
        /// Logic to get count of expense detail  by all employees 
        /// </summary>
        /// <param name="pager, empId" ></param>  
        public async Task<int> GetAllExpenseCount(SysDataTablePager pager, int empId, int companyId, int roleId)
        {
            return await _expensesRepository.GetAllExpenseCount(pager, empId, companyId);
        }
        //public async Task<int> GetEmployeeExpenseCount(SysDataTablePager pager, int empId, int roleId)
        //{
        //    return await _expensesRepository.GetEmployeeExpenseCount(pager, empId, roleId);
        //}
    }
}
