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
    [Route("api/v{version:apiVersion}/EmployeesAPI")]
    [ApiController]
    [EnableCors]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly IEmployeesAPIService _employeesAPIService;
        public EmployeesAPIController(IEmployeesAPIService employeesAPIService)
        {
            _employeesAPIService = employeesAPIService;
        }

        /// <summary>
        /// Logic to get all the employees list in api 
        /// </summary>
        /// <param name="empId" ></param>
        /// <param name="companyId" ></param>
        [HttpGet("GetAllEmployees")]
        public async Task<List<EmployeesRequestModel>> GetAllEmployees(int companyId, int empId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var employeesRequestModel = new List<EmployeesRequestModel>();
            employeesRequestModel = await _employeesAPIService.GetAllEmployees(empId, companyId);
            return employeesRequestModel;
        }
		
		[HttpGet("GetEmployeeById")]
        public async Task<EmployeesRequestModel> GetEmployeeById(int empId, int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var employeesRequestModel = new EmployeesRequestModel();
            employeesRequestModel = await _employeesAPIService.GetEmployeeById(empId, companyId);
            return employeesRequestModel;
        }

        /// <summary>
        /// Logic to get all the employees details and insert employees  particular empId employees in api 
        /// </summary>
        /// <param name="employees" ></param>
        [HttpPost("InsertEmployees")]
        public async Task<UserTimeSheetResponse> InsertEmployees(EmployeesRequestModel employees)
        {
            var companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var userEmployeesResponse = new UserTimeSheetResponse();
            if (employees != null)
            {
                userEmployeesResponse = await _employeesAPIService.InsertEmployees(employees, companyId);
            }
            return userEmployeesResponse;
        }

        /// <summary>
        /// Logic to get all the reportingperson list in api 
        /// </summary>      
        /// <param name="companyId" ></param>
        [HttpGet("ReportingPerson")]
        public async Task<List<ReportingPersons>> ReportingPerson(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var reportingPersons = new List<ReportingPersons>();
            reportingPersons = await _employeesAPIService.GetAllReportingPersons(companyId);
            return reportingPersons;
        }

        /// <summary>
        /// Logic to get all the designation list in api 
        /// </summary>      
        /// <param name="companyId" ></param>
        [HttpGet("Designation")]
        public async Task<List<Designations>> Designation(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var designations = new List<Designations>();
            designations = await _employeesAPIService.GetAllDesignation(companyId);
            return designations;
        }

        /// <summary>
        /// Logic to get all the departments list in api 
        /// </summary>      
        /// <param name="companyId" ></param>
        [HttpGet("Departments")]
        public async Task<List<Departments>> Departments(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var departments = new List<Departments>();
            departments = await _employeesAPIService.GetAllDepartment(companyId);
            return departments;
        }

        /// <summary>
        /// Logic to get all the role list in api 
        /// </summary>      
        /// <param name="companyId" ></param>
        [HttpGet("Roles")]
        public async Task<List<RoleViewModels>> Roles(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var roleViews = new List<RoleViewModels>();
            roleViews = await _employeesAPIService.GetAllRoleTable(companyId);
            return roleViews;
        }

        /// <summary>
        /// Logic to get username in api 
        /// </summary>      
        /// <param name="companyId" ></param>
        [HttpGet("GetUserName")]
        public async Task<CompanyCode> GetUserName(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var userName = new CompanyCode();
            userName.UserName = await _employeesAPIService.GetEmployeeUserName(companyId);
            return userName;
        }

        /// <summary>
        /// Logic to get all the relievingReason api list
        /// </summary>
        [HttpGet("GetRelievingReason")]
        public async Task<List<RelievingReasons>> GetRelievingReason(int companyId)
        {
            companyId = Convert.ToInt32(HttpContext.Session.GetInt32("CompanyId"));
            var relievingReason = await _employeesAPIService.GetAllRelievingReason(companyId);
            return relievingReason;
        }

        /// <summary>
        /// Logic to get reject the employees detail api by particular employees
        /// </summary>
        /// <param name="employees" ></param>
        [HttpDelete("DeleteRejectEmployees")]
        public async Task<bool> DeleteRejectEmployees(EmployeesRequestModel employees)
        {
            var result = false;
            if (employees != null)
            {
                result = await _employeesAPIService.GetRejectEmployees(employees);
            }
            return result;
        }
    }
}
