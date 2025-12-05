using EmployeeInformations.Business.API.IService;
using EmployeeInformations.CoreModels;
using EmployeeInformations.Model.APIDashboardModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.APIController
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/DashboardAPI")]
    [ApiController]
    [EnableCors]
    public class DashboardAPIController : ControllerBase
    {
        private readonly IDashboardAPIService _dashboardAPIService;

        public DashboardAPIController(IDashboardAPIService dashboardAPIService)
        {
            _dashboardAPIService = dashboardAPIService;
        }

        [HttpGet("GetAllDashboardView")]
        public async Task<DashboardViewModelAPI> GetAllDashboardView(int companyId, int empId, int roleId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var employeeId = empId;
            var roleIds = roleId;
            var dashboardmodel = await _dashboardAPIService.GetAllDashboardView(employeeId, roleIds, companyId);
            return dashboardmodel;
        }

        [HttpPost("InsertTimeLog")]
        public async Task<UserDashboardResponse> InsertTimeLog(TimeLoggerViewModelAPI timeLoggerViewModel)
        {
            var timelog = new UserDashboardResponse();
            if (timeLoggerViewModel != null)
            {
                timelog = await _dashboardAPIService.InsertTimeLog(timeLoggerViewModel);
            }
            return timelog;
        }

        /// <summary>
                /// Logic to get the employee clock in/out persentage api
        /// </summary>
        /// <param name="timeLoggerViewModel" ></param>
        [HttpPost("UpdateLogInTimeEveryOneMinuteByEmpId")]
        public async Task<UserTimeResponse> UpdateLogInTimeEveryOneMinuteByEmpId(TimeLoggerViewModelAPI timeLoggerViewModel)
        {
            var timelog = new UserTimeResponse();
            if (timeLoggerViewModel != null)
            {
                timelog = await _dashboardAPIService.GetLoggedInTotalTimeLog(timeLoggerViewModel);
            }

            return timelog;
        }

        /// <summary>
                /// Logic to get the annuncements detail by particular annuncementId view api 
        /// </summary>
        /// <param name="annuncementId" ></param>
        [HttpGet("GetAnnuncements")]
        public async Task<UserDashboardResponse> GetAnnuncements(int announcementId, int companyId)
        {
            var userDashboardResponse = new UserDashboardResponse();
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            if (announcementId != null)
            {
                userDashboardResponse = await _dashboardAPIService.GetAnnouncementById(announcementId,companyId);
            }
            return userDashboardResponse;
        }

        /// <summary>
                /// Logic to get all the leaveworkhrom api list
        /// </summary>
        [HttpGet("LeaveWorkFromHome")]
        public async Task<TopFiveLeaveWfhViewAPI> LeaveWorkFromHome(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var result = await _dashboardAPIService.GetTopFiveLeaveWfh(companyId);
            return result;
        }
    }
}
