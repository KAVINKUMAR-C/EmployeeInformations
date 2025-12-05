using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Model.PagerViewModel;
using EmployeeInformations.Model.TimesheetSummaryViewModel;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeInformations.Controllers
{
    public class TimeSheetController : BaseController
    {

        private readonly ITimeSheetService _timeSheetService;
        private readonly IEmployeesService _employeesService;

        public TimeSheetController(ITimeSheetService timeSheetService, IEmployeesService employeesService)
        {
            _timeSheetService = timeSheetService;
            _employeesService = employeesService;
        }

        //TimeSheet

        /// <summary>
        /// Logic to get all the timesheet list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TimeSheet()
        {
            HttpContext.Session.SetString("LastView", Constant.TimeSheet);
            HttpContext.Session.SetString("LastController", Constant.TimeSheet);
            return View();        
        }

        /// <summary>
        ///  Logic to get all the timesheet list
        /// </summary>
        /// <param name="pager,columnDirection,ColumnName" ></param>
        [HttpGet]
        public async Task<IActionResult> GetTimeSheets(SysDataTablePager pager, string columnDirection, string ColumnName)     
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionRoleId = GetSessionValueForRoleId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
             var empId = Convert.ToInt32(sessionRoleId == 1 || sessionRoleId == 2 ? 0 : sessionEmployeeId);            
            var timeSheets = await _timeSheetService.GetAllTimeSheets(pager, empId, columnDirection, ColumnName, companyId);
            return Json(new
            {               
                data = timeSheets.TimeSheetModels,
                iTotalDisplayRecords = timeSheets.TimeSheetCount,
                iTotalRecords = timeSheets.TimeSheetCount
            });         
        }       
        
        /// <summary>
        ///  Logic to get display the timesheet detail  by particular timesheet view
        /// </summary>
        /// <param name="timeSheetId" >timesheet</param>
        [HttpGet]
        public async Task<IActionResult> ViewTimeSheet(int timeSheetId)
        {
            var companyId = GetSessionValueForCompanyId;
            var timeSheet = await _timeSheetService.GetTimeSheetDetailsByTimeSheetId(timeSheetId, companyId);
            return View(timeSheet);
        }

        /// <summary>
        /// Logic to get create the timesheet detail  by particular timesheet
        /// </summary>       
        [HttpGet]
        public async Task<IActionResult> CreateTimeSheet()
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var timeSheet = new TimeSheet();
            timeSheet.EmpId = sessionEmployeeId;
            var sessionRoleId = GetSessionValueForRoleId;
            var empId = Convert.ToInt32((sessionRoleId == 1 || sessionRoleId == 2) ? 0 : sessionEmployeeId);
            timeSheet.ProjectNames = await _timeSheetService.GetAllProjectNames(empId, companyId);
            return View(timeSheet);
        }

        /// <summary>
        /// Logic to get edit the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="TimeSheetId" >timesheet</param>
        [HttpGet]
        public IActionResult EditTimeSheet(int TimeSheetId)
        {
            var timeSheet = new TimeSheet();
            timeSheet.TimeSheetId = TimeSheetId;
            return PartialView("EditTimeSheet", timeSheet);
        }

        /// <summary>
        /// Logic to get create the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="timeSheet,file" ></param>
        [HttpPost]
        public async Task<int> AddTimeSheet(TimeSheet timeSheet, IFormFile file)
        {
            // Retrieve the employee ID from the session
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;

            // Validate the file input
            if (file != null)
            {
                //throw new ArgumentException("No file was provided for the timesheet.");
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/TimeSheets");
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists

                // Generate a unique file name and full path
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(directoryPath, fileName);

                // Set the attachment details in the timeSheet object
                timeSheet.AttachmentFilePath = $"~/Management/TimeSheets/{fileName}"; // Ensure the correct virtual path
                timeSheet.AttachmentFileName = fileName;

                // Save the file to the server
                await SaveFileAsync(file, fullPath);
            }

            // Prepare the directory for file storage
          

            // Create the timesheet in the database
            var result = await _timeSheetService.CreateTimeSheet(timeSheet, sessionEmployeeId, companyId);
            return result;
        }

        /// <summary>
        /// Logic to get update the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="TimeSheetId" >timesheet</param>
        [HttpGet]
        public async Task<IActionResult> UpdateTimeSheet(int TimeSheetId)
        {
            var companyId = GetSessionValueForCompanyId;
            var timeSheet = await _timeSheetService.GetByTimeSheetId(TimeSheetId, companyId);
            timeSheet.ProjectNames = await _timeSheetService.GetAllProjectNames(timeSheet.EmpId,companyId);
            return PartialView("UpdateTimeSheet", timeSheet);
        }

        /// <summary>
        /// Logic to get update the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="timeSheet,file" ></param>

        [HttpPost]
        public async Task<int> UpdateTimeSheet(TimeSheet timeSheet, IFormFile file)
        {
            // Retrieve the session employee ID
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var companyId = GetSessionValueForCompanyId;
            // Validate the input
            if (file != null && !string.IsNullOrWhiteSpace(file.FileName))
            {
                // Prepare the storage path
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/TimeSheets");
                Directory.CreateDirectory(directoryPath); // Create directory if it doesn't exist

                // Generate a unique file name
                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(directoryPath, fileName);

                // Set attachment details in the timeSheet object
                timeSheet.AttachmentFilePath = $"~/Management/TimeSheets/{fileName}"; // Ensure correct virtual path
                timeSheet.AttachmentFileName = fileName;

                // Save the file to the server
                await SaveFileAsync(file, fullPath);
            }

            // Update the timesheet in the database
            var result = await _timeSheetService.CreateTimeSheet(timeSheet, sessionEmployeeId, companyId);
            return result;
        }
        public async Task SaveFileAsync(IFormFile file, string path)
        {
            try
            {
                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Asynchronously copy the file
                }
            }
            catch (IOException ex)
            {
                // Log the exception (implement a logging mechanism as per your setup)
                throw new Exception("An error occurred while saving the file.", ex);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                throw new Exception("An unexpected error occurred while saving the file.", ex);
            }
        }

        /// <summary>
        /// Logic to get soft deleted the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="TimeSheetId" >timesheet</param>
        [HttpPost]
        public async Task<IActionResult> DeleteTimeSheet(int TimeSheetId)
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _timeSheetService.DeleteTimeSheet(TimeSheetId, companyId);
            return Json(result);
        }

        /// <summary>
        /// Logic to get download the timesheet detail  by particular timesheet
        /// </summary>
        /// <param name="TimeSheetId" >timesheet</param>
        public async Task<FileResult> DownloadFiles(int timeSheetId)
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var timeStheet = await _timeSheetService.GetByTimeSheetId(timeSheetId, companyId);
            var empId = timeStheet.EmpId;
            var getUserName = await _employeesService.GetEmployeeById(empId,sessionEmployeeId);
            var empUserName = getUserName.UserName;
            string path = string.IsNullOrEmpty(timeStheet.AttachmentFilePath) ? string.Empty : timeStheet.AttachmentFilePath.Replace("~", Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Management/"));
            var bytes = System.IO.File.ReadAllBytes(path);
            var file = File(bytes, "application/octet-stream", timeStheet.AttachmentFilePath);
            file.FileDownloadName = empUserName + "_" + timeStheet.AttachmentFileName;
            return file;
        }
    }
}
