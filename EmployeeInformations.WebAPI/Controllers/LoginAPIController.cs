using EmployeeInformations.Business.API.IService;
using EmployeeInformations.Model.APIModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeInformations.APIController
{
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/LoginAPI")]
    [ApiController]
    [EnableCors]
    public class LoginAPIController : ControllerBase
    {
        private readonly ILoginAPIService _loginAPIService;

        public LoginAPIController(ILoginAPIService loginAPIService)
        {
            _loginAPIService = loginAPIService;
        }

        /// <summary>
        /// Logic to get all the login list and  particular empId in api 
        /// </summary>
        /// <param name="employees" ></param>
        [HttpPost("LoginDetails")]
        public async Task<UserEmployeesResponse> LoginDetails(LoginViewRequestModel employees)
        {
            var userEmployeesResponse = new UserEmployeesResponse();
            if (employees != null)
            {
                userEmployeesResponse = await _loginAPIService.LoginDetails(employees);
            }
            return userEmployeesResponse;
        }

    }
}
