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
    [Route("api/v{version:apiVersion}/LeaveAPI")]
    [ApiController]
    [EnableCors]
    public class LeaveAPIController : ControllerBase
    {
        private readonly ILeaveAPIService _leaveAPIService;
        public LeaveAPIController(ILeaveAPIService leaveAPIService)
        {
            _leaveAPIService = leaveAPIService;
        }

        /// <summary>
        /// Logic to get all the leave list in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetAllLeaveSummarys")]
        public async Task<List<LeaveRequestModel>> GetAllLeaveSummarys(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var leaveRequestModel = new List<LeaveRequestModel>();
            leaveRequestModel = await _leaveAPIService.GetAllLeaveSummarys(empId, companyId);
            return leaveRequestModel;
        }

        /// <summary>
        /// Logic to get all the leave list and  particular empId leave in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetEmployeeLeave")]
        public async Task<List<LeaveRequestModel>> GetEmployeeLeave(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var leaveRequestModel = new List<LeaveRequestModel>();
            leaveRequestModel = await _leaveAPIService.GetEmployeeLeave(empId, companyId);
            return leaveRequestModel;
        }

        /// <summary>
        /// Logic to get all the leave list and  particular empId and reporting person leave in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetApporvedEmployees")]
        public async Task<List<LeaveRequestModel>> GetApporvedEmployees(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var leaveRequestModel = new List<LeaveRequestModel>();
            leaveRequestModel = await _leaveAPIService.GetApporvedEmployees(empId, companyId);
            return leaveRequestModel;
        }

        /// <summary>
        /// Logic to get all the compensatoryoff list in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>

        [HttpGet("GetAllCompensatoryOff")]
        public async Task<List<CompensatoryOffRequestModel>> GetAllCompensatoryOff(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var leaveRequestModel = new List<CompensatoryOffRequestModel>();
            leaveRequestModel = await _leaveAPIService.GetAllCompensatoryOff(empId, companyId);
            return leaveRequestModel;
        }

        /// <summary>
        /// Logic to get all the leave details insert leave by particular empId in api 
        /// </summary>
        /// <param name="leaveRequestModel" ></param>
        [HttpPost("InsertLeave")]
        public async Task<UserLeaveResponse> InsertLeave(LeaveRequestModel leaveRequestModel)
        {
            var userLeaveResponse = new UserLeaveResponse();
            if (leaveRequestModel != null)
            {
                userLeaveResponse = await _leaveAPIService.InsertLeave(leaveRequestModel);
            }
            return userLeaveResponse;
        }

        /// <summary>
        /// Logic to get all the leave details update leave by particular empId in api 
        /// </summary>
        /// <param name="leaveRequestModel" ></param>
        [HttpPut("UpdateLeave")]
        public async Task<UserLeaveResponse> UpdateLeave(LeaveRequestModel leaveRequestModel)
        {
            var userLeaveResponse = new UserLeaveResponse();
            if (leaveRequestModel != null)
            {
                userLeaveResponse = await _leaveAPIService.UpdateLeave(leaveRequestModel);
            }
            return userLeaveResponse;
        }

        /// <summary>
        /// Logic to get all the compensatoryoff details insert leave by particular empId in api 
        /// </summary>
        /// <param name="compensatoryRequestAPI" ></param>
        [HttpPost("InsertCompensatory")]
        public async Task<UserLeaveResponse> InsertCompensatory(CompensatoryRequestAPI compensatoryRequestAPI)
        {
            var userLeaveResponse = new UserLeaveResponse();
            if (compensatoryRequestAPI != null)
            {
                userLeaveResponse = await _leaveAPIService.InsertCompensatory(compensatoryRequestAPI);
            }
            return userLeaveResponse;
        }

        /// <summary>
        /// Logic to get all the compensatoryoff details update leave by particular empId in api 
        /// </summary>
        /// <param name="compensatoryRequestAPI" ></param>

        [HttpPut("UpdateCompensatory")]
        public async Task<UserLeaveResponse> UpdateCompensatory(CompensatoryRequestAPI compensatoryRequestAPI)
        {
            var userLeaveResponse = new UserLeaveResponse();
            if (compensatoryRequestAPI != null)
            {
                userLeaveResponse = await _leaveAPIService.InsertCompensatory(compensatoryRequestAPI);
            }
            return userLeaveResponse;
        }

        /// <summary>
        /// Logic to get all the leave details in leave count by particular empId in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetLeaveCountAPI")]
        public async Task<LeaveRequestModel> GetLeaveCountAPI(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var leaveCountAPI = new LeaveRequestModel();
            leaveCountAPI = await _leaveAPIService.GetAllLeaveDetails(empId, companyId);
            return leaveCountAPI;
        }
    }
}

