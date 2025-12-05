using ClosedXML.Excel;
using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Common.Helpers;
using EmployeeInformations.Model.AttendanceViewModel;
using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Document = iTextSharp.text.Document;
using Font = iTextSharp.text.Font;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using PageSize = iTextSharp.text.PageSize;

namespace EmployeeInformations.Controllers
{
    public class AttendanceController : BaseController
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IReportService _reportService;
        private readonly IEmployeesService _employeesService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILog _logger;
        public AttendanceController(IAttendanceService attendanceService, IReportService reportService, IEmployeesService employeesService, IHostingEnvironment hostingEnvironment, ILog logger)
        {
            _attendanceService = attendanceService;
            _reportService = reportService;
            _employeesService = employeesService;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        /// <summary>
        /// Logic to get all the attendace list for Hr
        /// </summary>

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var attendaceListViewModel = new AttendaceListViewModel();
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var attandances = await _attendanceService.GetWorkingHourListForAll(companyId);
                attandances.reportingPeople = await _reportService.GetAllEmployeesDrropdown(companyId);
                HttpContext.Session.SetString("LastView", Constant.AttendaceList);
                HttpContext.Session.SetString("LastController", Constant.Attendance);
                return View(attandances);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("Attendance (HR) List:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return View(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get all the attendace list for Admin
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AttendanceAdmin()
        {
            var attendaceListViewModel = new AttendaceListViewModel();
            try
            {
                var sessionRoleId = GetSessionValueForRoleId;
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                attendaceListViewModel.EmployeeId = sessionEmployeeId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var empId = Convert.ToInt32(sessionRoleId == 1 || sessionRoleId == 2 ? 0 : sessionEmployeeId);
                var attandances = await _attendanceService.GetWorkingHourForAdmin(attendaceListViewModel);
                attandances.reportingPeople = await _reportService.GetAllEmployeesDrropdown(attendaceListViewModel.CompanyId);
                attandances.Years = await _reportService.GetYear();
                return View(attandances);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("Attendance (Admin) List:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return View(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get all the attendace list for Admin
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> AttendanceEmployee()
        {
            var attendaceListViewModel = new AttendaceListViewModel();
            try
            {
                var sessionRoleId = GetSessionValueForRoleId;
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                attendaceListViewModel.EmployeeId = sessionEmployeeId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var empId = Convert.ToInt32(sessionRoleId == 1 || sessionRoleId == 2 ? 0 : sessionEmployeeId);
                var attandances = await _attendanceService.GetWorkingHourForEmployee(attendaceListViewModel);
                attandances.Years = await _reportService.GetYear();
                return View(attandances);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("Attendance (Admin) List:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return View(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get filter  the attendace detail  by particular attendaceemployee
        /// </summary>
        /// <param name="attendaceListViewModel" >attendaceListViewModel</param>
        [HttpPost]
        public async Task<IActionResult> FilterEmployeeByAttendance(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendanceList = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, attendaceListViewModel.CompanyId);
                return new JsonResult(attendanceList);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("FilterEmployeeByAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get filter  the attendace detail  by particular attendaceemployee
        /// </summary>
        /// <param name="attendaceListViewModel" >attendaceListViewModel</param>
        [HttpPost]
        public async Task<IActionResult> FilterEmployeeByAttendanceData(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var month = attendaceListViewModel.Month;
                var year = attendaceListViewModel.Year;
                var firstDayOfMonth = new DateTime(year, month, 1);
                var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                attendaceListViewModel.StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
                attendaceListViewModel.EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendanceList = await _attendanceService.GetWorkingHourForAdmin(attendaceListViewModel);
                return new JsonResult(attendanceList);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("FilterEmployeeByAttendanceData:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get view the AttendanceData 
        /// </summary>   
        [HttpGet]
        public async Task<IActionResult> ViewAttendanceData()
        {
            var attendaceListViewModel = new AttendaceListViewModel();
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var sessionEmployeeId = GetSessionValueForEmployeeId;
                var sessionRoleId = GetSessionValueForRoleId;
                var empId = Convert.ToInt32(sessionRoleId == 1 || sessionRoleId == 2 ? 0 : sessionEmployeeId);
                attendaceListViewModel.EmployeeId = sessionEmployeeId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                HttpContext.Session.SetString("LastView", Constant.ViewAttendanceData);
                HttpContext.Session.SetString("LastController", Constant.Attendance);
                var date = DateTime.Now.ToString(Constant.DateFormat);
                var attandances = await _attendanceService.GetInOutListForAll(attendaceListViewModel,companyId);
                attandances.reportingPeople = empId == 0 ? await _reportService.GetAllEmployeesDrropdown(attendaceListViewModel.CompanyId) : await _reportService.GetEmpDropDown(sessionEmployeeId, companyId);
                attandances.EmployeeId = empId == 0 ? 0 : sessionEmployeeId;
                attandances.RoleId = (Common.Enums.Role)(short)sessionRoleId;
                attandances.LogDate = date;
                attandances.EndDate = date;
                attandances.EmployeeStatus = 1;
                return View(attandances);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("ViewAttendanceData:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return View(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to GetByStatus
        /// </summary>   
        /// <param name="statusId" ></param>
        [HttpGet]
        public async Task<IActionResult> GetByStatusId(int statusId)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var result = await _attendanceService.GetByStatusId(statusId,companyId);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("GetByStatusId:" + statusId + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(statusId);
        }

        /// <summary>
        /// Logic to get employee in/out details
        /// </summary>   
        /// <param name="statusId" ></param>
        [HttpPost]
        public async Task<IActionResult> FilterEmployeeLogData(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var sessionRoleId = GetSessionValueForRoleId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendanceList = await _attendanceService.ViewAttendanceData(attendaceListViewModel, companyId);
                attendanceList.RoleId = (Common.Enums.Role)(short)sessionRoleId;
                return new JsonResult(attendanceList);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("FilterEmployeeLogData:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(attendaceListViewModel);
        }

        /// <summary>
        /// Logic to get download the employeeattendancedetails excelfile detail  by particular employeeattendancedetails excelfile
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<string> DownloadExcel(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var sessionRoleId = GetSessionValueForRoleId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                SetDates(attendaceListViewModel);

                var attendanceList = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, companyId);
                if (attendanceList.AttendaceListViewModel.Count == 0) return string.Empty;

                var fileName = attendaceListViewModel.EmployeeId == 0
                    ? $"EmployeeAttendanceDetails_{DateTime.Now.ToString(Constant.DateFormatYMD)}{Constant.Hyphen}{Constant.xlsx}"
                    : $"{(await _employeesService.GetEmployeeById(attendaceListViewModel.EmployeeId, sessionRoleId)).UserName}_AttendanceDetails_{DateTime.Now.ToString(Constant.DateFormatYMD)}{Constant.Hyphen}{Constant.xlsx}";

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add(attendaceListViewModel.EmployeeId == 0 ? "Employee Attendance Details" : $"{(await _employeesService.GetEmployeeById(attendaceListViewModel.EmployeeId, sessionRoleId)).UserName}_Attendance Details");

                SetHeader(worksheet);
                FillWorksheet(worksheet, attendanceList.AttendaceListViewModel);

                var fileId = $"{Guid.NewGuid()}_{fileName}";
                using var memoryStream = new MemoryStream();
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                HttpContext.Session.Set(Constant.fileId, memoryStream.ToArray());

                return fileId;
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog($"DownloadExcel Attendance: {attendaceListViewModel} | StackTrace: {ex.StackTrace} | Msg: {ex.Message}");
                return string.Empty;
            }
        }

        private void SetDates(AttendaceListViewModel model)
        {
            if (model.Month > 0)
            {
                var firstDay = new DateTime(model.Year, model.Month, 1);
                var lastDay = new DateTime(model.Year, model.Month, DateTime.DaysInMonth(model.Year, model.Month));
                model.StartDate = firstDay.ToString(Constant.DateFormat);
                model.EndDate = lastDay.ToString(Constant.DateFormat);
            }
        }

        private void SetHeader(IXLWorksheet worksheet)
        {
            var headers = new[]
            {
                Constant.EmployeeUserId,
                Constant.EmployeeUserName,
                Constant.Date,
                Constant.EntryTime,
                Constant.ExitTime,
                Constant.TotalHours,
                Constant.BreakHours,
                Constant.ActualHours,
                Constant.TimeSheetHours
            };

            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = headers[i];
                worksheet.Cell(1, i + 1).Style.Font.Bold = true;
            }
        }

        private void FillWorksheet(IXLWorksheet worksheet, IEnumerable<AttendaceListViewModel> attendanceData)
        {
            int currentRow = 1;
            foreach (var user in attendanceData)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = user.UserName;
                worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                worksheet.Cell(currentRow, 4).Value = user.EntryTime;
                worksheet.Cell(currentRow, 5).Value = user.ExitTime;
                worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                worksheet.Cell(currentRow, 9).Value = user.BurningHours;


            }
        }

        /// <summary>
        /// Logic to get download the employeesinandoutdetails excelfile detail  by particular employeinandoutetails excelfile
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<string> DownloadInandOutExcel(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendanceList = await _attendanceService.ViewAttendanceData(attendaceListViewModel, companyId);

                if (!attendanceList.ViewAttendanceLog.Any())
                {
                    return string.Empty;
                }

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Employee In and Out Details");

                // Define headers
                var headers = new[]
                {
                    Constant.EmployeeUserId,
                    Constant.EmployeeUserName,
                    Constant.Date,
                    "Log Date Time",
                    "Log Time",
                    "Direction"
                };

                // Add headers to worksheet
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                // Populate data
                int currentRow = 2;
                foreach (var user in attendanceList.ViewAttendanceLog)
                {
                    worksheet.Cell(currentRow, 1).Value = user.EmployeeId;
                    worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                    worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.LogDate);

                    worksheet.Cell(currentRow, 4).Value = user.LogDateTime;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat("dd/MM/yyyy hh:mm:ss AM/PM");

                    worksheet.Cell(currentRow, 5).Value = user.LogTime;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);

                    worksheet.Cell(currentRow, 6).Value = user.Direction;
                    currentRow++;
                }

                var fileName = $"EmployeeAttendanceDetails_{DateTime.Now.ToString(Constant.DateFormatYMD)}-{Constant.xlsx}";
                var fileId = $"{Guid.NewGuid()}_{fileName}";

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    HttpContext.Session.Set(Constant.fileId, memoryStream.ToArray());
                }

                return fileId;
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog($"DownloadExcel Attendance: {attendaceListViewModel} StackTrace: {ex.StackTrace} msg: {ex.Message}");
            }
            return attendaceListViewModel.ToString();
        }



        /// <summary>
        /// Logic to get download the employeeattendancedetails detail  by particular employeeattendancedetails
        /// </summary>
        /// <param name="fileGuid" ></param>
        [HttpGet]
        public virtual ActionResult Download(string fileGuid)
        {
            try
            {
                var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                if (HttpContext.Session.Get(Constant.fileId) != null)
                {
                    byte[] data = HttpContext.Session.Get(Constant.fileId);
                    return File(data, "application/vnd.ms-excel", fileName);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("Download Attendance:" + fileGuid + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Logic to get send mail the employeeattendancedetails detail  by particular employeeattendancedetails
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<IActionResult> SendEmployeeAttendance(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var sendMail = false;
                var combinedPath = new List<string>();
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                if (attendaceListViewModel.Month > 0)
                {
                    var month = attendaceListViewModel.Month;
                    var year = attendaceListViewModel.Year;
                    var firstDayOfMonth = new DateTime(year, month, 1);
                    var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    attendaceListViewModel.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(firstDayOfMonth.ToString(Constant.DateFormat)).ToString(Constant.DateFormat);
                    attendaceListViewModel.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(lastDayOfMonth.ToString(Constant.DateFormat)).ToString(Constant.DateFormat);
                }
                else
                {
                    attendaceListViewModel.StartDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.StartDate).ToString(Constant.DateFormat);
                    attendaceListViewModel.EndDate = DateTimeExtensions.ConvertToNotNullDatetime(attendaceListViewModel.EndDate).ToString(Constant.DateFormat);
                }

                if (attendaceListViewModel.EmployeeId == 0 && attendaceListViewModel.EmployeeStatus == 0)
                {
                    var attendanceList = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, companyId);
                    if (attendanceList.AttendaceListViewModel.Count() > 0)
                    {
                        using var workbook = new XLWorkbook();
                        var worksheet = workbook.Worksheets.Add("Employee Attendance Details");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                        worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                        worksheet.Cell(currentRow, 3).Value = Constant.Date;
                        worksheet.Cell(currentRow, 4).Value = Constant.EntryTime;
                        worksheet.Cell(currentRow, 5).Value = Constant.ExitTime;
                        worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
                        worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
                        worksheet.Cell(currentRow, 8).Value = Constant.ActualHours;
                        worksheet.Cell(currentRow, 9).Value = Constant.TimeSheetHours;
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;

                        foreach (var user in attendanceList.AttendaceListViewModel)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                            worksheet.Cell(currentRow, 1).Value = user.UserName;
                            worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                            worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                            worksheet.Cell(currentRow, 4).Value = Convert.ToString(user.EntryTime);
                            worksheet.Cell(currentRow, 5).Value = Convert.ToString(user.ExitTime);
                            worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                            worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                            worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                            worksheet.Cell(currentRow, 9).Value = user.BurningHours;
                        }
                        var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                        var fileId = Guid.NewGuid().ToString() + "_" + fileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
                        combinedPath.Add(Path.Combine(path, fileNames));
                        var compath = Path.Combine(path, fileNames);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            workbook.SaveAs(memoryStream);
                            workbook.SaveAs(compath);
                            memoryStream.Position = 0;
                            var content = memoryStream.ToArray();
                            HttpContext.Session.Set(Constant.fileId, content);
                        }

                        var pdf = await CreatePdfFiles(attendaceListViewModel);
                        combinedPath.Add(pdf);
                        sendMail = await _attendanceService.SendMail(attendaceListViewModel, combinedPath, companyId);
                        sendMail = await _attendanceService.SendEmployeeAttendanceForAllEmployee(attendaceListViewModel, companyId);
                    }
                }
                else
                {
                    if (attendaceListViewModel.Month > 0 && attendaceListViewModel.EmployeeStatus == 0)
                    {
                        var attendanceList = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, companyId);
                        if (attendanceList.AttendaceListViewModel.Count() > 0)
                        {
                            using var workbook = new XLWorkbook();
                            var worksheet = workbook.Worksheets.Add("Employee Attendance Details");
                            var currentRow = 1;
                            worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                            worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                            worksheet.Cell(currentRow, 3).Value = Constant.Date;
                            worksheet.Cell(currentRow, 4).Value = Constant.EntryTime;
                            worksheet.Cell(currentRow, 5).Value = Constant.ExitTime;
                            worksheet.Cell(currentRow, 6).Value = Constant.TotalHours;
                            worksheet.Cell(currentRow, 7).Value = Constant.BreakHours;
                            worksheet.Cell(currentRow, 8).Value = Constant.ActualHours;
                            worksheet.Cell(currentRow, 9).Value = Constant.TimeSheetHours;
                            worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                            worksheet.Cell(currentRow, 9).Style.Font.Bold = true;

                            foreach (var user in attendanceList.AttendaceListViewModel)
                            {
                                currentRow++;
                                worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                                worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                                worksheet.Cell(currentRow, 1).Value = user.UserName;
                                worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                                worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.Date);
                                worksheet.Cell(currentRow, 4).Value = Convert.ToString(user.EntryTime);
                                worksheet.Cell(currentRow, 5).Value = Convert.ToString(user.ExitTime);
                                worksheet.Cell(currentRow, 6).Value = user.TotalHours;
                                worksheet.Cell(currentRow, 7).Value = user.BreakHours;
                                worksheet.Cell(currentRow, 8).Value = user.InsideOffice;
                                worksheet.Cell(currentRow, 9).Value = user.BurningHours;
                            }
                            var fileName = string.Format("EmployeeAttendanceDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                            var fileId = Guid.NewGuid().ToString() + "_" + fileName;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesAttendanceDetails");
                            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                            var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
                            combinedPath.Add(Path.Combine(path, fileNames));
                            var compath = Path.Combine(path, fileNames);
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                workbook.SaveAs(memoryStream);
                                workbook.SaveAs(compath);
                                memoryStream.Position = 0;
                                var content = memoryStream.ToArray();
                                HttpContext.Session.Set(Constant.fileId, content);
                            }
                            sendMail = await _attendanceService.SendMail(attendaceListViewModel, combinedPath, companyId);
                        }
                    }
                    else if (attendaceListViewModel.EmployeeStatus == 0)
                    {
                        sendMail = await _attendanceService.SendEmployeeAttendance(attendaceListViewModel, companyId);
                    }
                }
                return new JsonResult(sendMail);
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("SendEmployeeAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(attendaceListViewModel);
        }


        /// <summary>
        /// Logic to get send mail the employeeinandoutdetails detail  by particular employeeinandoutdetails
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        public async Task<IActionResult> SendEmployeeInandOut(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                var companyId = GetSessionValueForCompanyId;
                var sendMail = false;
                var combinedPath = new List<string>();
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var sendMailResults = new Dictionary<string, bool>();
                var attendanceList = await _attendanceService.ViewAttendanceData(attendaceListViewModel, companyId);                
                if (attendanceList.ViewAttendanceLog.Count() > 0)
                {
                    var groupedData = attendanceList.ViewAttendanceLog.GroupBy(user => user.EmployeeId);

                    foreach (var group in groupedData)
                    {
                        var employeeId = group.Key;
                        var employeeData = group.ToList();
                        using var workbook = new XLWorkbook();
                        var worksheet = workbook.Worksheets.Add("Employee In and Out Details");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = Constant.EmployeeUserId;
                        worksheet.Cell(currentRow, 2).Value = Constant.EmployeeUserName;
                        worksheet.Cell(currentRow, 3).Value = Constant.Date;
                        worksheet.Cell(currentRow, 4).Value = "Log Date Time";
                        worksheet.Cell(currentRow, 5).Value = "Log Time";
                        worksheet.Cell(currentRow, 6).Value = "Direction";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;

                        foreach (var user in group)
                        {
                            currentRow++;
                            worksheet.Cell(currentRow, 4).Style.NumberFormat.SetFormat("dd/MM/yyyy hh:mm:ss AM/PM");
                            worksheet.Cell(currentRow, 5).Style.NumberFormat.SetFormat(Constant.TimeFormatAMPM);
                            worksheet.Cell(currentRow, 1).Value = user.EmployeeId;
                            worksheet.Cell(currentRow, 2).Value = user.EmployeeName;
                            worksheet.Cell(currentRow, 3).Value = Convert.ToString(user.LogDate);
                            worksheet.Cell(currentRow, 4).Value = user.LogDateTime;
                            worksheet.Cell(currentRow, 5).Value = user.LogTime;
                            worksheet.Cell(currentRow, 6).Value = user.Direction;

                        }
                        var fileName = string.Format("EmployeeInandOutDetails_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.xlsx);
                        var fileId = Guid.NewGuid().ToString() + "_" + fileName;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "EmployeesInandOutDetails");
                        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                        var fileNames = Guid.NewGuid() + Path.GetExtension(fileName);
                        combinedPath.Add(Path.Combine(path, fileNames));
                        var compath = Path.Combine(path, fileNames);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            workbook.SaveAs(memoryStream);
                            workbook.SaveAs(compath);
                            memoryStream.Position = 0;
                            var content = memoryStream.ToArray();
                            HttpContext.Session.Set(Constant.fileId, content);
                        }
                        if (attendaceListViewModel.EmployeeId == 0)
                        {
                            foreach (var file in group)
                            {
                                var empId = file.Employee;
                                attendaceListViewModel.EmployeeId = empId;
                                break;
                            }
                        }
                        sendMail = await _attendanceService.SendMail(attendaceListViewModel, combinedPath, companyId);
                        combinedPath = new List<string>();
                        sendMailResults.Add(employeeId, sendMail);
                        attendaceListViewModel.EmployeeId = 0;
                    }
                }
                return new JsonResult(sendMail);

            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("SendEmployeeAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new JsonResult(attendaceListViewModel);
        }


        /// <summary>
        /// Logic to Create Pdf
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        [HttpPost]
        public async Task<string> CreatePdf(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                if (attendaceListViewModel.Month > 0)
                {
                    var month = attendaceListViewModel.Month;
                    var year = attendaceListViewModel.Year;
                    var firstDayOfMonth = new DateTime(year, month, 1);
                    var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    attendaceListViewModel.StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
                    attendaceListViewModel.EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
                }

                //var month = attendaceListViewModel.Month;
                //var year = attendaceListViewModel.Year;
                //var firstDayOfMonth = new DateTime(year, month, 1);
                //var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                //var frDate = firstDayOfMonth.ToString(Constant.DateFormat);
                //var toDate = lastDayOfMonth.ToString(Constant.DateFormat);
                //attendaceListViewModel.StartDate = frDate;
                //attendaceListViewModel.EndDate = toDate;

                var attendacelistViewModel = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel,attendaceListViewModel.CompanyId);
                if (attendacelistViewModel.AttendaceListViewModel.Count > 0)
                {

                    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                    MemoryStream PDFData = new MemoryStream();
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

                    var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                    var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                    var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                    BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                    Rectangle pageSize = writer.PageSize;
                    // Open the Document for writing
                    pdfDoc.Open();

                    // Create the header table 
                    PdfPTable headertable = new PdfPTable(3);
                    headertable.HorizontalAlignment = 0;
                    headertable.WidthPercentage = 100;
                    headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                    // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                    headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                    logo.ScaleToFit(100, 70);

                    {
                        PdfPCell pdfCelllogo = new PdfPCell(logo);
                        pdfCelllogo.Border = Rectangle.NO_BORDER;
                        pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        pdfCelllogo.BorderWidthBottom = 1f;
                        pdfCelllogo.PaddingTop = 10f;
                        pdfCelllogo.PaddingBottom = 10f;
                        headertable.AddCell(pdfCelllogo);
                    }

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        middlecell.BorderWidthBottom = 1f;
                        headertable.AddCell(middlecell);
                    }

                    {
                        PdfPTable nested = new PdfPTable(1);
                        nested.DefaultCell.Border = Rectangle.NO_BORDER;
                        PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + attendaceListViewModel.StartDate + " " + "To" + " " + attendaceListViewModel.EndDate, titleFont));
                        nextPostCell1.Border = Rectangle.NO_BORDER;
                        nextPostCell1.PaddingBottom = 20f;
                        nested.AddCell(nextPostCell1);

                        nested.AddCell("");
                        PdfPCell nesthousing = new PdfPCell(nested);
                        nesthousing.Border = Rectangle.NO_BORDER;
                        nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        nesthousing.BorderWidthBottom = 1f;
                        nesthousing.Rowspan = 6;
                        nesthousing.PaddingTop = 10f;
                        headertable.AddCell(nesthousing);
                    }

                    PdfPTable Invoicetable = new PdfPTable(3);
                    Invoicetable.HorizontalAlignment = 0;
                    Invoicetable.WidthPercentage = 100;
                    Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                    Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(middlecell);
                    }

                    {
                        PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                        emptyCell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(emptyCell);
                    }

                    {
                        PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                        emptyCell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(emptyCell);
                    }

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        middlecell.PaddingTop = 20f;
                        Invoicetable.AddCell(middlecell);
                    }

                    pdfDoc.Add(headertable);
                    pdfDoc.Add(Invoicetable);

                    //Create body table
                    PdfPTable tableLayout = new PdfPTable(9);
                    float[] headers = { 15, 46, 33, 36, 36, 36, 38, 42, 35 }; //Header Widths  
                    tableLayout.SetWidths(headers); //Set the pdf headers  
                    tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
                    tableLayout.HeaderRows = 0;

                    //Add header  
                    AddCellToHeader(tableLayout, "Id");
                    AddCellToHeader(tableLayout, "Name");
                    AddCellToHeader(tableLayout, "Date");
                    AddCellToHeader(tableLayout, "Entry Time");
                    AddCellToHeader(tableLayout, "Exit Time");
                    AddCellToHeader(tableLayout, "Total Hours");
                    AddCellToHeader(tableLayout, "Break Hours");
                    AddCellToHeader(tableLayout, "Actual Hours");
                    AddCellToHeader(tableLayout, "Time Sheet Hours");

                    //Add body  
                    foreach (var emp in attendacelistViewModel.AttendaceListViewModel)
                    {

                        AddCellToBody(tableLayout, emp.EmployeeId.ToString());
                        AddCellToBody(tableLayout, emp.EmployeeName);
                        AddCellToBody(tableLayout, emp.Date.ToString());
                        AddCellToBody(tableLayout, emp.EntryTime.ToString());
                        AddCellToBody(tableLayout, emp.ExitTime.ToString());
                        AddCellToBody(tableLayout, emp.TotalHours.ToString());
                        AddCellToBody(tableLayout, emp.BreakHours.ToString());
                        AddCellToBody(tableLayout, emp.InsideOffice.ToString());
                        AddCellToBody(tableLayout, emp.BurningHours.ToString());

                    }
                    pdfDoc.Add(tableLayout);

                    PdfContentByte cb = new PdfContentByte(writer);

                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                    cb = new PdfContentByte(writer);
                    cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 8);
                    cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                    cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                    cb.SetColorFill(BaseColor.LIGHT_GRAY);
                    cb.EndText();

                    //Move the pointer and draw line to separate footer section from rest of page
                    cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                    cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                    cb.Stroke();
                    string strPDFFileName = string.Format("EmployeeAttendanceDetails" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
                    pdfDoc.Close();
                    var content = PDFData.ToArray();
                    HttpContext.Session.Set(Constant.fileId, content);
                    var fileId = Guid.NewGuid().ToString() + Constant.Hyphen + strPDFFileName;
                    return fileId;
                }
                return "";
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("CreatePdfAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return attendaceListViewModel.ToString();
        }


        /// <summary>
        /// Logic to Create In and Out Pdf download
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        [HttpPost]
        public async Task<string> CreateInandOutPdf(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendanceList = await _attendanceService.ViewAttendanceData(attendaceListViewModel, attendaceListViewModel.CompanyId);
                if (attendanceList.ViewAttendanceLog.Count() > 0)
                {
                    Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                    MemoryStream PDFData = new MemoryStream();
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

                    var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                    var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                    var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                    var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                    var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                    BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                    Rectangle pageSize = writer.PageSize;
                    // Open the Document for writing
                    pdfDoc.Open();

                    // Create the header table 
                    PdfPTable headertable = new PdfPTable(3);
                    headertable.HorizontalAlignment = 0;
                    headertable.WidthPercentage = 100;
                    headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                    // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                    headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                    logo.ScaleToFit(100, 70);

                    {
                        PdfPCell pdfCelllogo = new PdfPCell(logo);
                        pdfCelllogo.Border = Rectangle.NO_BORDER;
                        pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        pdfCelllogo.BorderWidthBottom = 1f;
                        pdfCelllogo.PaddingTop = 10f;
                        pdfCelllogo.PaddingBottom = 10f;
                        headertable.AddCell(pdfCelllogo);
                    }

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        middlecell.BorderWidthBottom = 1f;
                        headertable.AddCell(middlecell);
                    }

                    {
                        PdfPTable nested = new PdfPTable(1);
                        nested.DefaultCell.Border = Rectangle.NO_BORDER;
                        PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee In and Out Details - " + attendaceListViewModel.StartDate + " " + "To" + " " + attendaceListViewModel.EndDate, titleFont));
                        nextPostCell1.Border = Rectangle.NO_BORDER;
                        nextPostCell1.PaddingBottom = 20f;
                        nested.AddCell(nextPostCell1);

                        nested.AddCell("");
                        PdfPCell nesthousing = new PdfPCell(nested);
                        nesthousing.Border = Rectangle.NO_BORDER;
                        nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                        nesthousing.BorderWidthBottom = 1f;
                        nesthousing.Rowspan = 6;
                        nesthousing.PaddingTop = 10f;
                        headertable.AddCell(nesthousing);
                    }

                    PdfPTable Invoicetable = new PdfPTable(3);
                    Invoicetable.HorizontalAlignment = 0;
                    Invoicetable.WidthPercentage = 100;
                    Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                    Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(middlecell);
                    }

                    {
                        PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                        emptyCell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(emptyCell);
                    }

                    {
                        PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                        emptyCell.Border = Rectangle.NO_BORDER;
                        Invoicetable.AddCell(emptyCell);
                    }

                    {
                        PdfPCell middlecell = new PdfPCell();
                        middlecell.Border = Rectangle.NO_BORDER;
                        middlecell.PaddingTop = 20f;
                        Invoicetable.AddCell(middlecell);
                    }

                    pdfDoc.Add(headertable);
                    pdfDoc.Add(Invoicetable);

                    //Create body table
                    PdfPTable tableLayout = new PdfPTable(6);
                    float[] headers = { 18, 33, 23, 38, 30, 18 }; //Header Widths  
                    tableLayout.SetWidths(headers); //Set the pdf headers  
                    tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
                    tableLayout.HeaderRows = 0;

                    //Add header  
                    AddCellToHeader(tableLayout, Constant.EmployeeUserId);
                    AddCellToHeader(tableLayout, Constant.EmployeeUserName);
                    AddCellToHeader(tableLayout, Constant.Date);
                    AddCellToHeader(tableLayout, "Log Date Time");
                    AddCellToHeader(tableLayout, "Log Time");
                    AddCellToHeader(tableLayout, "Direction");

                    //Add body  
                    foreach (var user in attendanceList.ViewAttendanceLog)
                    {

                        AddCellToBody(tableLayout, user.EmployeeId.ToString());
                        AddCellToBody(tableLayout, user.EmployeeName);
                        AddCellToBody(tableLayout, Convert.ToString(user.LogDate));
                        AddCellToBody(tableLayout, user.LogDateTime.ToString());
                        AddCellToBody(tableLayout, user.LogTime.ToString());
                        AddCellToBody(tableLayout, user.Direction.ToString());

                    }
                    pdfDoc.Add(tableLayout);

                    PdfContentByte cb = new PdfContentByte(writer);

                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                    cb = new PdfContentByte(writer);
                    cb = writer.DirectContent;
                    cb.BeginText();
                    cb.SetFontAndSize(bf, 8);
                    cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                    cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                    cb.SetColorFill(BaseColor.LIGHT_GRAY);
                    cb.EndText();

                    //Move the pointer and draw line to separate footer section from rest of page
                    cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                    cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                    cb.Stroke();
                    string strPDFFileName = string.Format("EmployeeAttendanceDetails" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
                    pdfDoc.Close();
                    var content = PDFData.ToArray();
                    HttpContext.Session.Set(Constant.fileId, content);
                    var fileId = Guid.NewGuid().ToString() + Constant.Hyphen + strPDFFileName;
                    return fileId;
                }
                return "";
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("CreatePdfAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return attendaceListViewModel.ToString();
        }

        /// <summary>
        /// Logic to Create In and Out Pdf for send mail purpose only
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        [HttpPost]
        public async Task<string> CreateInandOutPdfFiles(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                var attendaceList = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel,attendaceListViewModel.CompanyId);

                string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "InandOutDetails");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + "EmployeeInandOut" + Convert.ToDateTime(attendaceListViewModel.StartDate).ToString("yyyyMMdd") + Constant.Hyphen + Convert.ToDateTime(attendaceListViewModel.EndDate).ToString("yyyyMMdd") + ".pdf");

                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();
                //Add elements to the document here

                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                {
                    PdfPCell pdfCelllogo = new PdfPCell(logo);
                    pdfCelllogo.Border = Rectangle.NO_BORDER;
                    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    pdfCelllogo.BorderWidthBottom = 1f;
                    pdfCelllogo.PaddingTop = 10f;
                    pdfCelllogo.PaddingBottom = 10f;
                    headertable.AddCell(pdfCelllogo);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee In and Out Details - " + attendaceListViewModel.StartDate + " " + "To" + " " + attendaceListViewModel.EndDate, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nextPostCell1.PaddingBottom = 20f;
                    nested.AddCell(nextPostCell1);

                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 6;
                    nesthousing.PaddingTop = 10f;
                    headertable.AddCell(nesthousing);
                }

                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(middlecell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.PaddingTop = 20f;
                    Invoicetable.AddCell(middlecell);
                }

                pdfDoc.Add(headertable);
                pdfDoc.Add(Invoicetable);

                //Create body table
                PdfPTable tableLayout = new PdfPTable(6);
                float[] headers = { 15, 46, 33, 42, 36, 15 }; //Header Widths  
                tableLayout.SetWidths(headers); //Set the pdf headers  
                tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
                tableLayout.HeaderRows = 0;

                //Add header  
                AddCellToHeader(tableLayout, Constant.EmployeeUserId);
                AddCellToHeader(tableLayout, Constant.EmployeeUserName);
                AddCellToHeader(tableLayout, Constant.Date);
                AddCellToHeader(tableLayout, "Log Date Time");
                AddCellToHeader(tableLayout, "Log Time");
                AddCellToHeader(tableLayout, "Direction");

                //Add body  
                foreach (var user in attendaceList.ViewAttendanceLog)
                {

                    AddCellToBody(tableLayout, user.EmployeeId.ToString());
                    AddCellToBody(tableLayout, user.EmployeeName);
                    AddCellToBody(tableLayout, Convert.ToString(user.LogDate));
                    AddCellToBody(tableLayout, user.LogDateTime.ToString());
                    AddCellToBody(tableLayout, user.LogTime.ToString());
                    AddCellToBody(tableLayout, user.Direction.ToString());

                }
                pdfDoc.Add(tableLayout);

                PdfContentByte cb = new PdfContentByte(writer);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();
                pdfDoc.Close();
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("CreatePdfFilesInandOut:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return attendaceListViewModel.ToString();
        }


        /// <summary>
        /// Logic to Create Pdf for send mail purpose only
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        [HttpPost]
        public async Task<string> CreatePdfFiles(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {
                attendaceListViewModel.CompanyId = GetSessionValueForCompanyId;
                if (attendaceListViewModel.Month > 0)
                {
                    var month = attendaceListViewModel.Month;
                    var year = attendaceListViewModel.Year;
                    var firstDayOfMonth = new DateTime(year, month, 1);
                    var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    attendaceListViewModel.StartDate = firstDayOfMonth.ToString(Constant.DateFormat);
                    attendaceListViewModel.EndDate = lastDayOfMonth.ToString(Constant.DateFormat);
                }

                //var month = attendaceListViewModel.Month;
                //var year = attendaceListViewModel.Year;
                //var firstDayOfMonth = new DateTime(year, month, 1);
                //var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                //var frDate = firstDayOfMonth.ToString(Constant.DateFormat);
                //var todate = lastDayOfMonth.ToString(Constant.DateFormat);
                //attendaceListViewModel.StartDate = frDate;
                //attendaceListViewModel.EndDate = todate;

                var attendaceListViewModels = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, attendaceListViewModel.CompanyId);

                string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "AttendanceDetails");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string filePath = Path.Combine(directoryPath, Guid.NewGuid().ToString() + "_" + "EmployeeAttendance" + Convert.ToDateTime(attendaceListViewModel.StartDate).ToString("yyyyMMdd") + Constant.Hyphen + Convert.ToDateTime(attendaceListViewModel.EndDate).ToString("yyyyMMdd") + ".pdf");

                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));

                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();
                //Add elements to the document here

                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                {
                    PdfPCell pdfCelllogo = new PdfPCell(logo);
                    pdfCelllogo.Border = Rectangle.NO_BORDER;
                    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    pdfCelllogo.BorderWidthBottom = 1f;
                    pdfCelllogo.PaddingTop = 10f;
                    pdfCelllogo.PaddingBottom = 10f;
                    headertable.AddCell(pdfCelllogo);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + attendaceListViewModel.StartDate + " " + "To" + " " + attendaceListViewModel.EndDate, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nextPostCell1.PaddingBottom = 20f;
                    nested.AddCell(nextPostCell1);

                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 6;
                    nesthousing.PaddingTop = 10f;
                    headertable.AddCell(nesthousing);
                }

                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(middlecell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.PaddingTop = 20f;
                    Invoicetable.AddCell(middlecell);
                }

                pdfDoc.Add(headertable);
                pdfDoc.Add(Invoicetable);

                //Create body table
                PdfPTable tableLayout = new PdfPTable(9);
                float[] headers = { 15, 46, 33, 36, 36, 36, 38, 42, 35 }; //Header Widths  
                tableLayout.SetWidths(headers);
                tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
                tableLayout.HeaderRows = 0;

                //Add header
                AddCellToHeader(tableLayout, "Id");
                AddCellToHeader(tableLayout, "Name");
                AddCellToHeader(tableLayout, "Date");
                AddCellToHeader(tableLayout, "Entry Time");
                AddCellToHeader(tableLayout, "Exit Time");
                AddCellToHeader(tableLayout, "Total Hours");
                AddCellToHeader(tableLayout, "Break Hours");
                AddCellToHeader(tableLayout, "Actual Hours");
                AddCellToHeader(tableLayout, "Time Sheet Hours");

                //Add body  
                foreach (var emp in attendaceListViewModels.AttendaceListViewModel)
                {

                    AddCellToBody(tableLayout, emp.EmployeeId.ToString());
                    AddCellToBody(tableLayout, emp.EmployeeName);
                    AddCellToBody(tableLayout, emp.Date.ToString());
                    AddCellToBody(tableLayout, emp.EntryTime.ToString());
                    AddCellToBody(tableLayout, emp.ExitTime.ToString());
                    AddCellToBody(tableLayout, emp.TotalHours.ToString());
                    AddCellToBody(tableLayout, emp.BreakHours.ToString());
                    AddCellToBody(tableLayout, emp.InsideOffice.ToString());
                    AddCellToBody(tableLayout, emp.BurningHours.ToString());

                }
                pdfDoc.Add(tableLayout);

                PdfContentByte cb = new PdfContentByte(writer);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();
                pdfDoc.Close();
                return filePath;
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("CreatePdfFilesAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return attendaceListViewModel.ToString();
        }


        // Method to add single cell to the Header  
        private static void AddCellToHeader(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 8, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                Padding = 5,
                BackgroundColor = WebColors.GetRGBColor("#1a76d1")
            });
        }

        // Method to add single cell to the body  
        private static void AddCellToBody(PdfPTable tableLayout, string cellText)
        {
            tableLayout.AddCell(new PdfPCell(new Phrase(1, cellText, new Font(Font.FontFamily.HELVETICA, 7, 1, BaseColor.DARK_GRAY)))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                Padding = 5
            });
        }

        // Method to Download PDF File  
        [HttpGet]
        public virtual ActionResult DownloadPdf(string fileGuid)
        {
            try
            {
                var fileName = string.Format("EmployeeAttendanceReport_" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + Constant.pdf);
                if (HttpContext.Session.Get(Constant.fileId) != null)
                {
                    byte[] data = HttpContext.Session.Get(Constant.fileId);
                    return File(data, "pdf/application", fileName);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("DownloadPdfAttendance:" + fileGuid + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return new EmptyResult();
        }
        /// <summary>
        /// Logic to Create Pdf for Attendance HR
        /// </summary>
        /// <param name="attendaceListViewModel" ></param>
        [HttpPost]
        public async Task<string> GeneratePdf(AttendaceListViewModel attendaceListViewModel)
        {
            try
            {

                var companyId = GetSessionValueForCompanyId;
                var attendacelistViewModels = await _attendanceService.GetAllEmployessByAttendanceFilter(attendaceListViewModel, companyId);

                if (attendacelistViewModels.AttendaceListViewModel.Count == 0)
                {
                    return "";
                }
                Document pdfDoc = new Document(PageSize.A4, 10, 10, 10, 10);
                MemoryStream PDFData = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, PDFData);

                var titleFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var titleFontBlue = FontFactory.GetFont("Arial", 14, Font.NORMAL, BaseColor.BLUE);
                var boldTableFont = FontFactory.GetFont("Arial", 8, Font.BOLD);
                var bodyFont = FontFactory.GetFont("Arial", 8, Font.NORMAL);
                var EmailFont = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLUE);
                BaseColor TabelHeaderBackGroundColor = WebColors.GetRGBColor("#EEEEEE");

                Rectangle pageSize = writer.PageSize;
                // Open the Document for writing
                pdfDoc.Open();

                // Create the header table 
                PdfPTable headertable = new PdfPTable(3);
                headertable.HorizontalAlignment = 0;
                headertable.WidthPercentage = 100;
                headertable.SetWidths(new float[] { 200f, 5f, 350f });  // then set the column's __relative__ widths

                // headertable.DefaultCell.Border = Rectangle.NO_BORDER;            
                headertable.DefaultCell.Border = Rectangle.BOX; //for testing           
                string webRootPath = _hostingEnvironment.WebRootPath;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(webRootPath + "/images/logo.png");
                logo.ScaleToFit(100, 70);

                {
                    PdfPCell pdfCelllogo = new PdfPCell(logo);
                    pdfCelllogo.Border = Rectangle.NO_BORDER;
                    pdfCelllogo.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    pdfCelllogo.BorderWidthBottom = 1f;
                    pdfCelllogo.PaddingTop = 10f;
                    pdfCelllogo.PaddingBottom = 10f;
                    headertable.AddCell(pdfCelllogo);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    middlecell.BorderWidthBottom = 1f;
                    headertable.AddCell(middlecell);
                }

                {
                    PdfPTable nested = new PdfPTable(1);
                    nested.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell nextPostCell1 = new PdfPCell(new Phrase("Employee Attendance Details - " + attendaceListViewModel.StartDate + " " + "To" + " " + attendaceListViewModel.EndDate, titleFont));
                    nextPostCell1.Border = Rectangle.NO_BORDER;
                    nextPostCell1.PaddingBottom = 20f;
                    nested.AddCell(nextPostCell1);

                    nested.AddCell("");
                    PdfPCell nesthousing = new PdfPCell(nested);
                    nesthousing.Border = Rectangle.NO_BORDER;
                    nesthousing.BorderColorBottom = new BaseColor(System.Drawing.Color.Black);
                    nesthousing.BorderWidthBottom = 1f;
                    nesthousing.Rowspan = 6;
                    nesthousing.PaddingTop = 10f;
                    headertable.AddCell(nesthousing);
                }

                PdfPTable Invoicetable = new PdfPTable(3);
                Invoicetable.HorizontalAlignment = 0;
                Invoicetable.WidthPercentage = 100;
                Invoicetable.SetWidths(new float[] { 400f, 600f, 400f });  // then set the column's __relative__ widths
                Invoicetable.DefaultCell.Border = Rectangle.NO_BORDER;

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(middlecell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell emptyCell = new PdfPCell(new Phrase("\n")); // Add a new line
                    emptyCell.Border = Rectangle.NO_BORDER;
                    Invoicetable.AddCell(emptyCell);
                }

                {
                    PdfPCell middlecell = new PdfPCell();
                    middlecell.Border = Rectangle.NO_BORDER;
                    middlecell.PaddingTop = 20f;
                    Invoicetable.AddCell(middlecell);
                }

                pdfDoc.Add(headertable);
                pdfDoc.Add(Invoicetable);

                //Create body table
                PdfPTable tableLayout = new PdfPTable(9);
                float[] headers = { 15, 46, 33, 36, 36, 36, 38, 42, 35 }; //Header Widths  
                tableLayout.SetWidths(headers); //Set the pdf headers  
                tableLayout.WidthPercentage = 85; //Set the PDF File witdh percentage  
                tableLayout.HeaderRows = 0;

                //Add header  
                AddCellToHeader(tableLayout, "Id");
                AddCellToHeader(tableLayout, "Name");
                AddCellToHeader(tableLayout, "Date");
                AddCellToHeader(tableLayout, "Entry Time");
                AddCellToHeader(tableLayout, "Exit Time");
                AddCellToHeader(tableLayout, "Total Hours");
                AddCellToHeader(tableLayout, "Break Hours");
                AddCellToHeader(tableLayout, "Actual Hours");
                AddCellToHeader(tableLayout, "Time Sheet Hours");

                //Add body  
                foreach (var emp in attendacelistViewModels.AttendaceListViewModel)
                {

                    AddCellToBody(tableLayout, emp.EmployeeId.ToString());
                    AddCellToBody(tableLayout, emp.EmployeeName);
                    AddCellToBody(tableLayout, emp.Date.ToString());
                    AddCellToBody(tableLayout, emp.EntryTime.ToString());
                    AddCellToBody(tableLayout, emp.ExitTime.ToString());
                    AddCellToBody(tableLayout, emp.TotalHours.ToString());
                    AddCellToBody(tableLayout, emp.BreakHours.ToString());
                    AddCellToBody(tableLayout, emp.InsideOffice.ToString());
                    AddCellToBody(tableLayout, emp.BurningHours.ToString());

                }
                pdfDoc.Add(tableLayout);

                PdfContentByte cb = new PdfContentByte(writer);

                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                cb = new PdfContentByte(writer);
                cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(bf, 8);
                cb.SetTextMatrix(pageSize.GetLeft(120), 20);
                cb.ShowText(@DateTime.Now.Year + " " + "VpHospital.All Rights Reserved");
                cb.SetColorFill(BaseColor.LIGHT_GRAY);
                cb.EndText();

                //Move the pointer and draw line to separate footer section from rest of page
                cb.MoveTo(40, pdfDoc.PageSize.GetBottom(50));
                cb.LineTo(pdfDoc.PageSize.Width - 40, pdfDoc.PageSize.GetBottom(50));
                cb.Stroke();
                string strPDFFileName = string.Format("EmployeeAttendanceDetails" + DateTime.Now.ToString(Constant.DateFormatYMD) + Constant.Hyphen + ".pdf");
                pdfDoc.Close();
                var content = PDFData.ToArray();
                HttpContext.Session.Set(Constant.fileId, content);
                var fileId = Guid.NewGuid().ToString() + Constant.Hyphen + strPDFFileName;
                return fileId;
            }
            catch (Exception ex)
            {
                _logger.WriteErrorLog("CreatePdfAttendance:" + attendaceListViewModel + "StackTrace: " + ex.StackTrace + "msg :" + ex.Message);
            }
            return attendaceListViewModel.ToString();
        }

    }
}
