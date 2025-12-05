using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.LeaveSummaryViewModel;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.ReportsViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class LeaveSummaryController : BaseController
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeesService _employeesService;
        private IConfiguration _configuration;
        private readonly IReportService _reportService;
        public LeaveSummaryController(ILeaveService leaveService, IConfiguration Configuration, IEmployeesService employeesService, IReportService reportService)
        {
            _leaveService = leaveService;
            _configuration = Configuration;
            _employeesService = employeesService;
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Logic to get all the leave list for admin
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> LeaveDetails()
        //{
        //    var sessionRoleId = GetSessionValueForRoleId;
        //    var sessionEmployeeId = GetSessionValueForEmployeeId;
        //    var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
        //    HttpContext.Session.SetString("LastView", Constant.LeaveAdminPage);
        //    var leaveSummary = await _leaveService.GetAllLeaveSummary(empId);
        //    var filter = leaveSummary.OrderByDescending(x => x.EmployeeStatus).Reverse().ToList();
        //    return View(filter);
        //}

        /// <summary>
        /// Logic to view all the leave list for admin
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LeaveDetails()
        {
            HttpContext.Session.SetString("LastView", Constant.LeaveAdminPage);
            HttpContext.Session.SetString("LastController", Constant.LeaveSummary);
            return View();
        }

        /// <summary>
        /// Logic to get all the leave list for admin
        /// </summary>
        ///<param name="pager, columnName, columnDirection" >leave</param>
        [HttpGet]
        public async Task<IActionResult> GetLeaveDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            HttpContext.Session.SetString("LastView", Constant.LeaveAdminPage);
            HttpContext.Session.SetString("LastController", Constant.LeaveSummary);
            var leaveSummary = await _leaveService.GetAllLeaveSummary(pager, empId, columnName, columnDirection,companyId);
            var cntLeave = await _leaveService.GetAllLeaveSummaryCount(pager, empId,companyId);
            return Json(new
            {
                data = leaveSummary,
                iTotalRecords = cntLeave,
                iTotalDisplayRecords = cntLeave
            });
        }

        /// <summary>
        /// Logic to get all the leave list for employees
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> EmployeesLeaveDetails()
        //{
        //    var sessionRoleId = GetSessionValueForRoleId;
        //    var sessionEmployeeId = GetSessionValueForEmployeeId;
        //    var empId = Convert.ToInt32(sessionEmployeeId);
        //    HttpContext.Session.SetString("LastView", Constant.LeaveEmployeePage);
        //    var leaveSummary = await _leaveService.GetApplyEmployee(empId);
        //    return View(leaveSummary);
        //}

        /// <summary>
        /// Logic to view all the leave list for employees
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EmployeesLeaveDetails()
        {
            HttpContext.Session.SetString("LastView", Constant.LeaveEmployeePage);
            return View();
        }

        /// <summary>
        /// Logic to get all the leave list for employees
        /// </summary>
        /// <param name="pager, columnName, columnDirection" >leave</param>

        [HttpGet]
        public async Task<IActionResult> GetEmployeesLeaveDetails(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32(sessionEmployeeId);
            HttpContext.Session.SetString("LastView", Constant.LeaveEmployeePage);
            var empLeaveDetails = await _leaveService.GetApplyEmployee(pager, empId, columnName, columnDirection,companyId);
            var cntLeave = await _leaveService.GetEmployeesLeaveDetailsCount(pager, empId,companyId);
            return Json(new
            {
                data = empLeaveDetails,
                iTotalRecords = cntLeave,
                iTotalDisplayRecords = cntLeave
            });
        }

        /// <summary>
        /// Logic to get all the leave list for reportingperson
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> LeaveApporvedEmployees()
        //{
        //    var sessionRoleId = GetSessionValueForRoleId;
        //    var sessionEmployeeId = GetSessionValueForEmployeeId;
        //    var empId = Convert.ToInt32(sessionEmployeeId);
        //    HttpContext.Session.SetString("LastView", Constant.LeaveApprovalPage);
        //    var leaveSummary = await _leaveService.GetApporvedEmployee(empId);
        //    var filter = leaveSummary.OrderByDescending(x => x.EmployeeStatus).Reverse().ToList();
        //    return View(filter);
        //}

        /// <summary>
        /// Logic to get all the leave list for reportingperson
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LeaveApporvedEmployees()
        {

            HttpContext.Session.SetString("LastView", Constant.LeaveApprovalPage);
            HttpContext.Session.SetString("LastController", Constant.LeaveSummary);
            return View();
        }


        /// <summary>
        /// Logic to get all the leave list for reportingperson
        /// </summary>
        /// <param name="pager, columnName, columnDirection" >leave</param>

        [HttpGet]
        public async Task<IActionResult> EmployeesLeaveApporved(SysDataTablePager pager, string columnName, string columnDirection)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var empId = Convert.ToInt32(sessionEmployeeId);
            HttpContext.Session.SetString("LastView", Constant.LeaveApprovalPage);
            HttpContext.Session.SetString("LastController", Constant.LeaveSummary);
            var getApprovedLeave = await _leaveService.GetApporvedEmployees(pager, empId, columnName, columnDirection,companyId);
            var cntLeave = await _leaveService.GetAllLeaveSummaryCount(pager, empId,companyId);
            return Json(new
            {
                data = getApprovedLeave,
                iTotalRecords = cntLeave,
                iTotalDisplayRecords = cntLeave
            });
        }

        /// <summary>
        ///  Logic to get display the leave detail  by particular leave
        /// </summary>
        /// <param name="appliedLeaveId" >leave</param>
        [HttpGet]
        public async Task<IActionResult> ViewEmployeeLeave(int appliedLeaveId)
        {
            var companyId = GetSessionValueForCompanyId;
            var viewEmployeeLeaveSummary = await _leaveService.GetViewLeaveByAppliedLeaveId(appliedLeaveId,companyId);
            return View(viewEmployeeLeaveSummary);
        }

        [HttpGet]
        public async Task<IActionResult> CreateLeave()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var leave = new EmployeeLeaveViewModel();
            var leaveSummary = await _leaveService.GetAllLeaveDetails(sessionEmployeeId,companyId);
            leave.LeaveCounts = leaveSummary.LeaveCounts;
            leaveSummary.EmpId = sessionEmployeeId;
            return View(leaveSummary);
        }
        [HttpPost]
        public async Task<IActionResult> VerifyCount(EmployeeLeaveViewModel leave)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var verifyCount = await _leaveService.VerifyLeave(leave, sessionEmployeeId, companyId);
            return new JsonResult(verifyCount);
        }

        /// <summary>
        /// Logic to get create the leave detail  by particular employee leave
        /// </summary> 
        /// <param name="leave,file" ></param> 
        [HttpPost]
        public async Task<IActionResult> CreateLeave(EmployeeLeaveViewModel leave, IFormFile file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            if (file != null && file.Name != "")
            {
                //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/LeaveAttachments");

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/LeaveAttachments");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                leave.LeaveFilePath = path.Replace(path, "~/LeaveAttachments/") + fileName;
                leave.LeaveName = fileName;
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            var result = await _leaveService.CreateLeave(leave, sessionEmployeeId,companyId);
            return new JsonResult(result);
        }



        /// <summary>
        /// Logic to get download the leave detail  by particular employee leave
        /// </summary> 
        /// <param name="appliedLeaveId" >leave</param> 
        public async Task<FileResult> DownloadFile(int appliedLeaveId)
        {
            var sessionRoleId = GetSessionValueForRoleId;
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetLeaveByAppliedLeaveId(appliedLeaveId,companyId);
            if (leaveSummary != null)
            {
                var empUserName = string.Empty;
                var empId = leaveSummary.EmpId;
                var getUserName = await _employeesService.GetEmployeeById(empId, sessionRoleId);
                empUserName = getUserName.UserName;
                string path = !string.IsNullOrEmpty(leaveSummary.LeaveFilePath) ? leaveSummary.LeaveFilePath.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/")) : string.Empty;
                //Read the File data into Byte Array.
                var bytes = System.IO.File.ReadAllBytes(path);
                var file = File(bytes, "application/octet-stream", leaveSummary.LeaveFilePath);
                file.FileDownloadName = empUserName + "_" + leaveSummary.LeaveFilePath;
                //Send the File to Download.
                return file;
            }
            return null;

        }

        /// <summary>
        /// Logic to get update the leave detail  by particular leave
        /// </summary>
        /// <param name="AppliedLeaveId" >leave</param>
        [HttpGet]
        public async Task<IActionResult> UpdateLeave(int AppliedLeaveId)
        {
            var companyId = GetSessionValueForCompanyId;
            var leaveSummary = await _leaveService.GetLeaveByAppliedLeaveId(AppliedLeaveId,companyId);
            leaveSummary.LeaveType = await _leaveService.GetAllLeave();
            var leaveSummarys = await _leaveService.GetAllLeaveDetails(leaveSummary.EmpId,companyId);
            leaveSummary.LeaveCounts = leaveSummarys.LeaveCounts;
            leaveSummary.CasualLeaveRemaining = leaveSummarys.CasualLeaveRemaining;
            leaveSummary.SickLeaveRemaining = leaveSummarys.SickLeaveRemaining;
            leaveSummary.EarnedLeaveRemaining = leaveSummarys.EarnedLeaveRemaining;
            leaveSummary.MaternityLeaveRemaining = leaveSummarys.MaternityLeaveRemaining;
            return PartialView("UpdateLeave", leaveSummary);
        }

        /// <summary>
        /// Logic to get edit the leave detail  by particular leave
        /// </summary>
        /// <param name="AppliedLeaveId" >leave</param>
        [HttpGet]
        public IActionResult ChangeLeave(int AppliedLeaveId)
        {
            var employeeLeaveViewModel = new EmployeeLeaveViewModel();
            employeeLeaveViewModel.AppliedLeaveId = AppliedLeaveId;
            return PartialView("LeaveManagement", employeeLeaveViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLeave(EmployeeLeaveViewModel leave, IFormFile file)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            if (file != null && file.Name != "")
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/LeaveAttachments");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var combinedPath = Path.Combine(path, fileName);
                leave.LeaveFilePath = path.Replace(path, "~/LeaveAttachments/") + fileName;
                leave.LeaveName = fileName;
                using (var stream = new FileStream(combinedPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            var result = await _leaveService.UpdateLeave(leave, sessionEmployeeId,companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteLeave(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (leave != null)
            {
                result = await _leaveService.DeleteLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get approvedleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> ApprovedLeave(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (leave != null)
            {
                result = await _leaveService.ApprovedLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get rejectleave the leave detail  by particular leave
        /// </summary>
        /// <param name="leave" ></param>
        [HttpPost]
        public async Task<IActionResult> RejectLeave(EmployeeAppliedLeave leave)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = 0;
            if (leave != null)
            {
                result = await _leaveService.RejectLeave(leave, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        //Holidays

        /// <summary>
        /// Logic to get all the holidays list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> EmployeeHolidays(int year)
        {
            var companyId = GetSessionValueForCompanyId;
            var holidaysSummary = await _leaveService.GetAllEmployeeHolidays(year,companyId);
            holidaysSummary.Years = await _reportService.GetYear();
            holidaysSummary.Year = DateTime.Now.Year;
            var futureYear = DateTime.Now.AddYears(1).Year;
            holidaysSummary.Years?.Add(new Years()
            {
                Year = futureYear,
                StrYear = Convert.ToString(futureYear),
            });
            return View(holidaysSummary);

        }

        /// <summary>
        /// Logic to get all Holidays
        /// </summary>
        public async Task<IActionResult> FilterHolidays(int year)
        {
            var companyId = GetSessionValueForCompanyId;
            var holidaysSummary = await _leaveService.GetAllEmployeeHolidays(year,companyId);
            return new JsonResult(holidaysSummary);
        }

        /// <summary>
        /// Logic to Create Holiday
        /// </summary>
        /// <param name="employeeHolidays" ></param>
        [HttpPost]
        public async Task<IActionResult> CreateHoliday(EmployeeHolidays employeeHolidays)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = await _leaveService.CreateHoliday(employeeHolidays, sessionEmployeeId, companyId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to Update Holiday
        /// </summary>
        /// <param name="employeeHolidays" ></param>
        [HttpPost]
        public async Task<IActionResult> UpdateHoliday(EmployeeHolidays employeeHolidays)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var result = await _leaveService.UpdateHoliday(employeeHolidays, sessionEmployeeId);
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get soft deleted the holiday detail  by particular holiday
        /// </summary>
        /// <param name="employeeHolidays" ></param>
        [HttpPost]
        public async Task<IActionResult> DeleteHoliday(EmployeeHolidays employeeHolidays)
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            var result = false;
            if (employeeHolidays.HolidayId > 0)
            {
                result = await _leaveService.DeleteHoliday(employeeHolidays, sessionEmployeeId,companyId);
                return new JsonResult(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Logic to get all Holidays
        /// </summary>
        public async Task<IActionResult> HolidayList()
        {
            var companyId = GetSessionValueForCompanyId;
            var year = 0;
            var holidaysSummary = await _leaveService.GetAllEmployeeHolidays(year,companyId);
            return View(holidaysSummary);
        }

        /// <summary>
        /// Logic to get Holiday by date
        /// </summary>
        [HttpPost]
        public async Task<int> GetHolidayDate(string HolidayDates)
        {
            var companyId = GetSessionValueForCompanyId;
            var moduleNameCount = await _leaveService.GetHolidayDate(HolidayDates,companyId);
            return moduleNameCount;
        }

        /// <summary>
        /// Logic to get Holiday by date and id
        /// </summary>
        [HttpPost]
        public async Task<int> GetHolidayDatesId(string holidayDate, int holidayid)
        {
            var companyId = GetSessionValueForCompanyId;
            var moduleNameCount = await _leaveService.GetHolidayDatesId(holidayDate, holidayid,companyId);
            return moduleNameCount;
        }

        /// <summary>
        /// Logic to get download the holidays detail files 
        /// </summary>
        public async Task<FileResult> Excel(int year)
        {
            var companyId = GetSessionValueForCompanyId;
            var holidaysSummary = await _leaveService.GetAllEmployeeHolidays(year,companyId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Employee Holidays");
            var currentRow = 1;

            //worksheet.Cell(currentRow, 1).Value = Common.Constant.HolidayId;
            worksheet.Cell(currentRow, 1).Value = Common.Constant.Title;
            worksheet.Cell(currentRow, 2).Value = Common.Constant.HolidayDate;
            worksheet.Cell(currentRow, 3).Value = Common.Constant.HolidayName;
            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
            foreach (var user in holidaysSummary?.EmployeeHolidays)
            {
                currentRow++;

                //worksheet.Cell(currentRow, 1).Value = user.HolidayId;
                worksheet.Cell(currentRow, 1).Value = user.Title;
                worksheet.Cell(currentRow, 2).Value = user.HolidayDate.ToString("dd/MM/yyyy");
                worksheet.Cell(currentRow, 3).Value = user.HolidayName;

            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();
            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeHolidays" + Common.Constant.xlsx);
        }
    }
}



