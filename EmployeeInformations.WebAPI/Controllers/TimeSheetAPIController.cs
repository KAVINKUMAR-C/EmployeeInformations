using EmployeeInformations.Business.API.IService;
using EmployeeInformations.CoreModels;
using EmployeeInformations.Model.APIModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.APIController
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/TimeSheetAPI")]
    [ApiController]
    [EnableCors]
    public class TimeSheetAPIController : ControllerBase
    {
        private readonly ITimeSheetAPIService _timeSheetAPIService;

        public TimeSheetAPIController(ITimeSheetAPIService timeSheetAPIService)
        {
            _timeSheetAPIService = timeSheetAPIService;
        }

        /// <summary>
        /// Logic to get all the timesheet list and  particular empId timesheet in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetTimeSheet")]
        public async Task<List<TimeSheetRequestModel>> GetTimeSheet(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var timeSheetRequestModel = new List<TimeSheetRequestModel>();
            timeSheetRequestModel = await _timeSheetAPIService.GetTimesheet(empId, companyId);
            return timeSheetRequestModel;
        }

        /// <summary>
        /// Logic to get all the timesheet details and insert timesheet  particular empId timesheet in api 
        /// </summary>
        /// <param name="timeSheetRequestModel" ></param>
        [HttpPost("InsertTimesheet")]
        public async Task<UserTimeSheetResponse> InsertTimesheet(TimeSheetRequestModel timeSheetRequestModel)
        {
            var response = new UserTimeSheetResponse();
            if (timeSheetRequestModel != null)
            {
                response = await _timeSheetAPIService.InsertTimesheet(timeSheetRequestModel);
            }
            return response;
        }

        /// <summary>
        /// Logic to get all the timesheet details and update timesheet  particular empId timesheet in api 
        /// </summary>
        /// <param name="timeSheetRequestModel" ></param>
        [HttpPut("UpdateTimesheet")]
        public async Task<UserTimeSheetResponse> UpdateTimesheet(TimeSheetRequestModel timeSheetRequestModel)
        {
            var timesheet = new UserTimeSheetResponse();
            if (timeSheetRequestModel != null)
            {
                timesheet = await _timeSheetAPIService.InsertTimesheet(timeSheetRequestModel);
            }
            return timesheet;
        }

        /// <summary>
        /// Logic to get all the project details particular empId use  timesheet in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetProjectNamesByEmpId")]
        public async Task<List<ProjectNamesAPI>> GetProjectNamesByEmpId(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var projectNames = new List<ProjectNamesAPI>();
            projectNames = await _timeSheetAPIService.GetAllProjectNamesByEmpId(empId, companyId);
            return projectNames;
        }

        /// <summary>
                /// Logic to get soft deleted the timesheet api detail  by particular timesheet
                /// </summary>
        /// <param name="timeSheetId" >timesheet</param>
        [HttpDelete("DeleteTimeSheet")]
        public async Task<bool> DeleteTimeSheet(int companyId, int timeSheetId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var result = await _timeSheetAPIService.DeleteTimeSheet(timeSheetId,companyId);
            return result;
        }
    }
}
