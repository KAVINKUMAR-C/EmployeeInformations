using EmployeeInformations.Business.IService;
using EmployeeInformations.Common;
using EmployeeInformations.Filters;
using EmployeeInformations.Model.DashboardViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.Controllers
{
    [CheckSessionIsAvailable]
    public class DashboardController : BaseController
    {

        private readonly IDashboardService _dashboardService;
        private readonly IMasterService _masterService;
        private readonly IAttendanceService _attendanceService;
        private readonly IEmployeeInformationService _employeeInformationService;
        public DashboardController(IDashboardService dashboardService, IMasterService masterService, IAttendanceService attendanceService, IEmployeeInformationService employeeInformationService)
        {
            _dashboardService = dashboardService;
            _masterService = masterService;
            _attendanceService = attendanceService;
            _employeeInformationService = employeeInformationService;
        }

        /// <summary>
        /// Logic to get all the home list
        /// </summary>
        public async Task<IActionResult> Home()
        {
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var roleId = GetSessionValueForRoleId;
            HttpContext.Session.SetString("LastView", Constant.Home);
            HttpContext.Session.SetString("LastController", Constant.Dashboard);
            var dashboardViewModel = new DashboardViewModel();
            var companyId = GetSessionValueForCompanyId;
            dashboardViewModel = await _dashboardService.GetAllDashboardView(sessionEmployeeId, roleId, companyId);
            return View(dashboardViewModel);
        }

        /// <summary>
        /// Logic to get all the employee home list
        /// </summary>
        public async Task<IActionResult> EmployeeHome()
        {
            var companyId = GetSessionValueForCompanyId;
            var sessionEmployeeId = GetSessionValueForEmployeeId;
            var roleId = GetSessionValueForRoleId;
            var dashboardViewModel = new DashboardViewModel();
            dashboardViewModel = await _dashboardService.GetAllDashboardView(sessionEmployeeId, roleId, companyId);
            return View(dashboardViewModel);
        }


        /// <summary>
        /// Logic to get the employee clock in/out 
        /// </summary>
        /// <param name="timeLoggerViewModel" ></param> 
        [HttpPost]
        public async Task<bool> LogTiminingByEmpId(TimeLoggerViewModel timeLoggerViewModel)
        {
            var result = await _dashboardService.InsertTimeLog(timeLoggerViewModel);
            return result;
        }

        /// <summary>
        /// Logic to get the employee clock in/out persentage 
        /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        [HttpPost]
        public async Task<TimeLogView> UpdateLogInTimeEveryOneMinuteByEmpId(TimeLoggerViewModel timeLoggerViewModel)
        {
            var result = await _dashboardService.GetLoggedInTotalTimeLog(timeLoggerViewModel);
            return result;
        }

        /// <summary>
        /// Logic to get the annuncements detail by particular annuncementId view 
        /// </summary>
        /// <param name="annuncementId" ></param>
        [HttpPost]
        public async Task<IActionResult> GetAnnuncements(int annuncementId)
        {
     
            var sessionCompanyId = GetSessionValueForCompanyId;
            var announcement = await _dashboardService.GetAnnouncementById(annuncementId, sessionCompanyId);
            return new JsonResult(announcement);
        }

        /// <summary>
        /// Logic to get all the leaveworkhrom list
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LeaveWorkFromHome()
        {
            var companyId = GetSessionValueForCompanyId;
            var result = await _dashboardService.GetTopFiveLeaveWfh(companyId);
            return View(result);
        }

    }
}
